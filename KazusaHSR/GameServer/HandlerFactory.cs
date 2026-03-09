using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

internal class HandlerFactory
{
	private static readonly Dictionary<PacketId, Action<Session, Packet>> _handlers = new();

	public static void RegisterHandler(PacketId cmdId, Action<Session, Packet> handler)
	{
		_handlers.Add(cmdId, handler);
	}

	public static Action<Session, Packet>? GetHandler(PacketId cmdId)
	{
		return (_handlers.TryGetValue(cmdId, out var handler)) ? handler : null;
	}
}