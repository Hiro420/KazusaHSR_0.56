using KazusaHSR.GameServer;
using KazusaHSR.GameServer.ConsoleCommands;
using KazusaHSR.Resource;
using KazusaHSR.Utils;
using KazusaHSR.WebServer;

namespace KazusaHSR;

public class MainApp
{
	public static Config config = Config.Load();
	public static ResourceManager resourceManager = new("resources");
	public static DatabaseManager databaseManager = new(config.AccountDataBase);
	public static ConsoleCommandDispatcher commandDispatcher;
	public static void Main(string[] args)
	{
		IConsole.InitConsole();
		Logger logger = new("MainApp");

		string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
		if (!Directory.Exists(logDir))
			Directory.CreateDirectory(logDir);

		string latestLog = Path.Combine(logDir, "latest.log");
		string bootLog = Path.Combine(logDir, $"log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");
		File.WriteAllText(latestLog, string.Empty);

		Logger.AssignLogFile(new FileInfo(latestLog));
		Logger.AssignLogFile(new FileInfo(bootLog));

		logger.PrintKazusa();
		commandDispatcher = new ConsoleCommandDispatcher(logger);
		logger.Emit("Session started");
		HandbookGen.GenerateHandbook();

		Thread webServerThread = new Thread(() => WebProgram.StartWebServer(config.WebServer.ServerIP, config.WebServer.ServerPort));
		webServerThread.Start();
		Thread gameServerThread = new Thread(() => GameServer.GameServerManager.StartLoop());
		gameServerThread.Start();
		IConsole.ListenConsole();
	}
}
