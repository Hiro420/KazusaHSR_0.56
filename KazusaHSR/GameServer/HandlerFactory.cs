using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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