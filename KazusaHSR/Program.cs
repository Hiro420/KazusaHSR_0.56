using KazusaHSR.Utils;
using KazusaHSR.Resource;
using KazusaHSR.WebServer;
using KazusaHSR.GameServer;
using System.Diagnostics;
using System.IO.Pipes;

namespace KazusaHSR;

public class MainApp
{
	public static Config config = Config.Load();
	public static ResourceManager resourceManager = new("resources");
	public static DatabaseManager databaseManager = new(config.AccountDataBase);
	public static void Main(string[] args)
	{
		Logger.DoLogUselessInfo = true;
		Logger logger = new("MainApp");
		logger.LogKazusa();
		Thread webServerThread = new Thread(() => WebProgram.StartWebServer(config.WebServer.ServerIP, config.WebServer.ServerPort));
		webServerThread.Start();
		Thread gameServerThread = new Thread(() => GameServer.GameServerManager.StartLoop());
		gameServerThread.Start();

		// TODO later
		//Thread serverThread = new Thread(() => KazusaConsoleServer.StartLoop());
		//serverThread.Start();
	}
}
