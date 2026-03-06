using KazusaHSR.Protocol;
using System.Buffers.Binary;
using System.Net;
using ProtoBuf;
using System.Reflection;
//using static KazusaHSR.Utils.Crypto;
using System.Runtime.InteropServices;
using System.IO;

namespace KazusaHSR.GameServer;

public class Packet
{
    public uint CmdId { get; set; }
    public byte[] FinishedBody { get; set; }
    public IExtensible Ori { get; set; } // protobuf-net compatible
    private const uint HeaderMagic = 0x01234567;
    private const uint FooterMagic = 0x89ABCDEF;


	public void SetData<T>(PacketId cmdType, T msg) where T : class, IExtensible
    {
        CmdId = (uint)cmdType;
        FinishedBody = SerializeToByteArray(msg);
        Ori = msg;
    }

    public static byte[] SerializeToByteArray<T>(T obj) where T : class, IExtensible
    {
        using (var ms = new MemoryStream())
        {
            Serializer.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static T DeserializeFromByteArray<T>(byte[] data) where T : class, IExtensible
    {
        using (var ms = new MemoryStream(data))
        {
            return Serializer.Deserialize<T>(ms);
        }
    }

    public T DecodeBody<T>() where T : class, IExtensible, new()
    {
        return DeserializeFromByteArray<T>(FinishedBody);
    }

    public T GetDecodedBody<T>()
    {
        T serializedBody;

        using (MemoryStream stream = new(FinishedBody))
        {
            serializedBody = Serializer.Deserialize<T>(stream);
        }

        return serializedBody;
    }

	public static ushort GetUInt16(byte[] buf, int index)
	{
		return BinaryPrimitives.ReadUInt16BigEndian(buf.AsSpan(index, 2));
	}

	public static uint GetUInt32(byte[] buf, int index)
	{
		return BinaryPrimitives.ReadUInt32BigEndian(buf.AsSpan(index, 4));
	}

	public static void PutUInt16(byte[] buf, ushort value, int offset)
	{
		BinaryPrimitives.WriteUInt16BigEndian(buf.AsSpan(offset, 2), value);
	}

	public static void PutUInt32(byte[] buf, uint value, int offset)
	{
		BinaryPrimitives.WriteUInt32BigEndian(buf.AsSpan(offset, 4), value);
	}

	public static void PutByteArray(byte[] destination, byte[] source, int offset)
    {
        Buffer.BlockCopy(source, 0, destination, offset, source.Length);
    }

    public static byte[] ToByteArray(IntPtr ptr, int length)
    {
        if (ptr == IntPtr.Zero)
        {
            throw new ArgumentException("Pointer cannot be null", nameof(ptr));
        }

        byte[] byteArray = new byte[length];
        Marshal.Copy(ptr, byteArray, 0, length);
        return byteArray;
    }

	public static byte[] EncodePacket(Session session, ushort CmdId, IExtensible body)
	{
		PacketHead header = new PacketHead { PacketId = CmdId };

		byte[] headerBytes = SerializeToByteArray(header);
		byte[] finishedBody = SerializeToByteArray(body);

		// NetPacket: 16 bytes
		// [0..4)  u32 head magic
		// [4..6)  u16 cmd
		// [6..8)  u16 head len
		// [8..12) i32 body len
		// [12..]  head, body, u32 tail magic
		int totalSize = 16 + headerBytes.Length + finishedBody.Length;
		byte[] packetData = new byte[totalSize];

		PutUInt32(packetData, HeaderMagic, 0);
		PutUInt16(packetData, CmdId, 4);
		PutUInt16(packetData, (ushort)headerBytes.Length, 6);
		BinaryPrimitives.WriteInt32BigEndian(packetData.AsSpan(8, 4), finishedBody.Length);

		PutByteArray(packetData, headerBytes, 12);
		PutByteArray(packetData, finishedBody, 12 + headerBytes.Length);

		PutUInt32(packetData, FooterMagic, 12 + headerBytes.Length + finishedBody.Length);

		return packetData;
	}

	public static Packet? Read(Session session, byte[] data, out int consumed)
	{
		consumed = 0;

		// Not a valid packet? Too short to even contain header and footer.
		if (data.Length < 16)
			return null;

		// Better validate head first (u32 big-endian)
		uint headMagic = GetUInt32(data, 0);
		if (headMagic != HeaderMagic)
		{
			Console.WriteLine($"invalid header magic. expected: 0x{HeaderMagic:X8}, received: 0x{headMagic:X8}");
			return null;
		}

		ushort cmdId = GetUInt16(data, 4);
		ushort headerLength = GetUInt16(data, 6);
		int bodyLength = BinaryPrimitives.ReadInt32BigEndian(data.AsSpan(8, 4));

		if (bodyLength < 0)
		{
			Console.WriteLine("invalid body length (negative)");
			return null;
		}

		int totalSize = 16 + headerLength + bodyLength;
		if (data.Length < totalSize)
			// need moreeeeee
			return null;

		// Validate the tail magic (u32 big-endian)
		int tailOffset = 12 + headerLength + bodyLength;
		uint tailMagic = GetUInt32(data, tailOffset);
		if (tailMagic != FooterMagic)
		{
			Console.WriteLine($"invalid footer magic. expected: 0x{FooterMagic:X8}, received: 0x{tailMagic:X8}");
			return null;
		}

		// Here goes our juicy packet body
		byte[] body = new byte[bodyLength];
		Buffer.BlockCopy(data, 12 + headerLength, body, 0, bodyLength);

		consumed = totalSize;
		return new Packet { CmdId = cmdId, FinishedBody = body };
	}

	public static Packet? Read(Session session, byte[] data)
	{
		return Read(session, data, out _);
	}

	[AttributeUsage(AttributeTargets.Method)]
    public class PacketCmdId : Attribute
    {
        public PacketId Id { get; }

        public PacketCmdId(PacketId id)
        {
            Id = id;
        }
    }
}