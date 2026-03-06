using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KazusaHSR.Utils;
using System.Threading.Tasks;
using KazusaHSR.Protocol;
using System.Net;
using System.Net.Sockets;

namespace KazusaHSR.GameServer;

public class GameServerManager
{
    public static List<Session> sessions = new List<Session>();

    public static void StartLoop()
    {
        Config config = MainApp.config;
        Logger logger = new("GameServer");

        foreach (var type in typeof(HandlerFactory).Assembly.GetTypes().Where(a => a.FullName!.Contains("KazusaHSR.GameServer.Handlers")))
        {
            foreach (var method in type.GetMethods())
            {
                var attributes = method.GetCustomAttributes(typeof(Packet.PacketCmdId), false);
                if (attributes.Length > 0)
                {
                    var attribute = (Packet.PacketCmdId)attributes[0];
                    var handler = (Action<Session, Packet>)Delegate.CreateDelegate(typeof(Action<Session, Packet>), method);
                    HandlerFactory.RegisterHandler(attribute.Id, handler);
                }
            }
        }

        logger.LogSuccess($"Starting GameServer on {config.GameServer.ServerIP}:{config.GameServer.ServerPort}");

        TcpListener listener = new TcpListener(IPAddress.Parse(config.GameServer.ServerIP), config.GameServer.ServerPort);
        listener.Start();
        logger.LogSuccess("GameServer listener started");

        CancellationTokenSource cts = new CancellationTokenSource();

        while (true)
        {
            TcpClient tcpClient = listener.AcceptTcpClient();
            var client = new GameClient(tcpClient);
            var session = new Session(client);
            sessions.Add(session);

            if (config.LogOption.Connections)
            {
                logger.LogSuccess($"Client connected: {tcpClient.Client.RemoteEndPoint}");
            }

            _ = Task.Run(async () =>
            {
                await session.StartReceiveLoopAsync(cts.Token);
                sessions.Remove(session);
                if (config.LogOption.Connections)
                {
                    logger.LogError($"Client disconnected: {tcpClient.Client.RemoteEndPoint}");
                }
                tcpClient.Close();
            });
        }
    }
}
