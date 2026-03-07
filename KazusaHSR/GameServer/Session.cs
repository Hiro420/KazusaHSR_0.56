using KazusaHSR.Protocol;
using System;
using KazusaHSR.Utils;
using System.Numerics;
using System.Reflection;
using Microsoft.Extensions.Logging;
using ProtoBuf;
using Serilog;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;

namespace KazusaHSR.GameServer;

public class Session
{
	public readonly Logger c;
	private readonly GameClient _client;
	private readonly TcpClient _tcpClient;
	private readonly NetworkStream _stream;
	public byte[]? key;
	private uint lastGuid = 0;
	private uint lastEntityId = 0;
	private ILogger<Session> fileLogger;
	private static readonly string logsFolder = "Logs";
	private static readonly List<string> blacklist = new List<string>()
	{  	// to not flood the console
		// TODO
	};
	private JsonSerializer _JsonConverter;
	public Player player;
	private readonly object _entityIdLock = new();
	public AbilityManager AbilityManager;
	public string AccountId { get; private set; } = string.Empty;
	public string Token { get; private set; } = string.Empty;
	public uint AccountUid { get; private set; }

	public Session(GameClient client)
	{
		_client = client;
		_tcpClient = client.TcpClient;
		_stream = _tcpClient.GetStream();

		var remote = (IPEndPoint)_tcpClient.Client.RemoteEndPoint!;
		string sessionId = $"{remote.Address}:{remote.Port}";

		c = new Logger($"Session {sessionId}");
		if (!Directory.Exists(logsFolder))
			Directory.CreateDirectory(logsFolder);
		Log.Logger = new LoggerConfiguration()
				.WriteTo.File(Path.Combine(logsFolder, $"latest_{sessionId}.log"), rollingInterval: RollingInterval.Day)
				.CreateLogger();

		// Create logger instance for the session
		fileLogger = LoggerFactory.Create(builder =>
		{
			builder.AddFile(Path.Combine(logsFolder, $"session_{sessionId}.log"));
		}).CreateLogger<Session>();

		_JsonConverter = new JsonSerializer
		{
			NullValueHandling = NullValueHandling.Ignore
		};
		_JsonConverter.Converters.Add(new StringEnumConverter());
	}

	public void BindAccount(string accountId, string token, uint uid)
	{
		AccountId = accountId;
		Token = token;
		AccountUid = uid;

		// Load or create the persistent player for this account.
		player = MainApp.databaseManager
			.LoadOrCreatePlayerAsync(this, AccountId, Token, AccountUid)
			.GetAwaiter()
			.GetResult();
		player.GiveAllItems();
		player.Scene.AddAllEntities();

		AbilityManager = new AbilityManager(this);
	}

	public async Task StartReceiveLoopAsync(CancellationToken cancellationToken)
	{
		byte[] tempBuffer = new byte[8192];
		List<byte> buffer = new List<byte>();

		try
		{
			while (!cancellationToken.IsCancellationRequested && _tcpClient.Connected)
			{
				int read = await _stream.ReadAsync(tempBuffer.AsMemory(0, tempBuffer.Length), cancellationToken);
				if (read == 0)
				{
					// remote closed
					break;
				}

				buffer.AddRange(tempBuffer.AsSpan(0, read).ToArray());

				while (true)
				{
					var data = buffer.ToArray();
					var packet = Packet.Read(this, data, out int consumed);
					if (packet == null || consumed == 0)
					{
						break;
					}

					buffer.RemoveRange(0, consumed);
					onMessage(packet);
				}
			}
		}
		catch (IOException)
		{
			// connection closed or network error
		}
		catch (Exception ex)
		{
			c.LogError($"ReceiveLoop error: {ex}");
		}
	}

	public async Task LogToFileAsync(string message)
	{
		await Task.Run(() =>
		{
			fileLogger.LogInformation(message);
		});
	}

	private string PacketToJson(Packet packet)
	{
		try
		{
			PacketId cmd = (PacketId)packet.CmdId;
			string protoName = $"{cmd}";
			Type protoType = Type.GetType($"KazusaHSR.Protocol.{protoName}")!;
			MethodInfo method = typeof(Packet).GetMethod(nameof(packet.GetDecodedBody))!.MakeGenericMethod(protoType);
			string jsonBody = _JsonConverter.SerializeObject(method.Invoke(packet, null)!);
			return jsonBody;
		}
		catch (Exception e)
		{
			c.LogError($"{e.Message}, {e.InnerException}, {e.Source}");
			return String.Empty;
		}
	}

	public static Vector3 VectorProto2Vector3(Protocol.Vector vectorProto)
	{
		return new Vector3(vectorProto.X, vectorProto.Y, vectorProto.Z);
	}

	public static Protocol.Vector Vector3ToVector(Vector3 pos)
	{
		return new Protocol.Vector()
		{
			X = (int)(pos.X * 1000f),
			Y = (int)(pos.Y * 1000f),
			Z = (int)(pos.Z * 1000f)
		};
	}

	public uint GetGuid()
	{
		lastGuid++;
		return lastGuid;
	}

	public uint GetEntityId(EntityType type)
	{
		var scene = player?.Scene;
		if (scene != null)
		{
			return scene.GenNewEntityId(type);
		}
		lastEntityId++;
		var index = lastEntityId & 0xFFFFFFu;
		if (index == 0)
		{
			index = 1;
			lastEntityId = 1;
		}
		return ((uint)type << 24) | index;
	}

	public void onMessage(Packet packet)
	{
		if (packet == null)
		{
			return;
		}
		string protoName = $"{(PacketId)packet.CmdId}";
		string logStr = $"Received {protoName} {PacketToJson(packet)}";
		if (!blacklist.Contains(protoName) && MainApp.config.LogOption.Packets)
			c.LogInfo(logStr);
		LogToFileAsync(logStr).Wait();
		var handler = HandlerFactory.GetHandler((PacketId)packet.CmdId);
		if (handler == null)
		{
			c.LogError($"No handler for {(PacketId)packet.CmdId}");
			return;
		}

		handler?.Invoke(this, packet);
	}


	public bool SendPacket(IExtensible protoMessage)
	{
		try
		{
			string protoName = protoMessage.ToString()!.Split("KazusaHSR.Protocol.").Last();
			PacketId packetId = (PacketId)Enum.Parse(typeof(PacketId), protoName);
			byte[] packet = Packet.EncodePacket(this, (ushort)packetId, protoMessage);
			// send the packet over TCP
			_stream.Write(packet, 0, packet.Length);
			string logStr = $"Sent {protoName} {JsonConvert.SerializeObject(protoMessage)}";
			if (!blacklist.Contains(protoName) && MainApp.config.LogOption.Packets)
				c.LogInfo(logStr);
			LogToFileAsync(logStr).Wait();
			return true;
		}
		catch (Exception e)
		{
			c.LogError($"{e.Message}");
			return false;
		}
	}

	public void Terminate()
	{
		// usually id send the logout packet rsp, but it doesnt matter since the connection is closed anyway
		_tcpClient.Close();
		GameServerManager.sessions.Remove(this);
	}
}