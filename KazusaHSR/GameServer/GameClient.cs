using System.Net.Sockets;

namespace KazusaHSR.GameServer;

public class GameClient
{
	public TcpClient TcpClient { get; }
	public bool Connected => TcpClient.Connected;

	public GameClient(TcpClient tcpClient)
	{
		TcpClient = tcpClient;
	}
}