using Spectre.Console;
using System.Diagnostics;

namespace KazusaHSR;

public class Logger(string moduleName)
{
	private static readonly List<FileInfo> TargetLogFiles = [];
	private static readonly object SyncLock = new();
	private readonly string Module = moduleName;

	public void Log(string message, Severity severity)
	{
		lock (SyncLock)
		{
			var prevInput = IConsole.Input.ToList();
			IConsole.RedrawInput("", false);

			var formatter = new MessageFormatter(DateTime.Now, Module, severity, message);
			AnsiConsole.MarkupLine(formatter.Format());

			IConsole.RedrawInput(prevInput);

			string logEntry = $"[{DateTime.Now:HH:mm:ss}] [{Module}] [{severity}] {message}";
			PersistToFile(logEntry);
		}
	}

	public void Emit(string text, Exception? ex = null)
	{
		Log(text, Severity.SUCCESS);
		if (ex != null)
		{
			Log(ex.Message, Severity.SUCCESS);
			Log(ex.StackTrace!, Severity.SUCCESS);
		}
	}

	public void Message(string text, Exception? ex = null)
	{
		Log(text, Severity.INFO);
		if (ex != null)
		{
			Log(ex.Message, Severity.INFO);
			Log(ex.StackTrace!, Severity.INFO);
		}
	}

	public void Alert(string text, Exception? ex = null)
	{
		Log(text, Severity.WARNING);
		if (ex != null)
		{
			Log(ex.Message, Severity.WARNING);
			Log(ex.StackTrace!, Severity.WARNING);
		}
	}

	public void Fail(string text, Exception? ex = null)
	{
		Log(text, Severity.FAILURE);
		if (ex != null)
		{
			Log(ex.Message, Severity.FAILURE);
			Log(ex.StackTrace!, Severity.FAILURE);
		}
	}

	public void Trace(string text, Exception? ex = null)
	{
		Log(text, Severity.TRACE);
		if (ex != null)
		{
			Log(ex.Message, Severity.TRACE);
			Log(ex.StackTrace!, Severity.TRACE);
		}
	}

	public static void AssignLogFile(FileInfo file)
	{
		lock (SyncLock)
		{
			if (TargetLogFiles.Any(x => string.Equals(x.FullName, file.FullName, StringComparison.OrdinalIgnoreCase)))
				return;

			TargetLogFiles.Add(file);
		}
	}

	public static void PersistToFile(string entry)
	{
		try
		{
			if (TargetLogFiles.Count == 0)
				throw new Exception("LogFile is not set");

			foreach (var target in TargetLogFiles)
			{
				using var writer = target.AppendText();
				writer.WriteLine(entry);
			}
		}
		catch { }
	}

	public static Logger Create()
	{
		var callerType = new StackTrace().GetFrame(1)?.GetMethod()?.ReflectedType?.Name ?? "";
		return new Logger(callerType);
	}

	public void PrintKazusa()
	{
		lock (SyncLock)
		{
			var prevInput = IConsole.Input.ToList();
			IConsole.RedrawInput("", false);
			AnsiConsole.MarkupLine($"[[[DeepPink1_1]{AsciiArt}[/]]]");
			IConsole.RedrawInput(prevInput);
		}
	}

	public enum Severity
	{
		INFO = ConsoleColor.Cyan,
		WARNING = ConsoleColor.Yellow,
		FAILURE = ConsoleColor.Red,
		SUCCESS = ConsoleColor.Green,
		TRACE = ConsoleColor.Blue,
	}

	private const string AsciiArt =
@"                         .                                                                                         
                  ::-:.:.-@#*==-     :+                                              :-..=:     .::..--:::-=...      
                  ..:::==:.:@%*++++=-:   :     ...               -+                   .:..:..    .-...-.....:=...=   
                  .:::.-==-:..@@#***+*+=:.    ..--=++******+=-.       @                --..-:.   .::..-.......:-..:. 
                   .:.:++--=--..@%+++**####**+*###**+=++**##%@@@@@#*-       .@          :=..--.   .:-..=........+... 
                    .:. *=====-:.@#######**++******#*@@@@#+**+++=+**%@@*....   =+        .:..--    -.-..-........-.. 
                    .::.+====--.%%*******++@@@%*#*+%*=--=+#%#*******+++#%@@*=-:    .     ..-...--:  ---...-....:.:-. 
                     ::.-*-=:.@@%#****+==@@*==+++#@++%%@@#++*******+********#**+=-.         :-..-::- ..::...=....-:. 
                     .:: =-.=@%#******+%@==***++%@=#**=*++@%****+#+++*+*******####*+-  :@      ...=-=  -:::...==--+. 
                      .:..:%%=*##*****@*=+++=@#+-@*+*+++#*++******#*++*+***+***+******+::+@@@@       ..  .-::......  
                       .  @#++*#*#**#%*+****@*+*+#%+*+#*******#****##++++**+*****++*******++*%@@@@#          ::-...  
                     :. .@#++=-*#**#*+****+%++***+@+*******%*+**#*****=+*+++**+****+***+++****+=+#@@@@@@@@*     ..   
                        @#++=-#%%##*++#****%+**#*+%*********#*+**#***#@=+**+*+++******++++*****++++*#@@@@            
                      @#*++==@%.-###*#****#*+*****#*####***+@**+***#*+*@+-+****+**+***+***+**+++#@@@@: :  ::.        
                      .:**==%*-=#@+*#@@%##@*%%%@@#@##*#@@@%=#@##+***#*=#@@*%@@@@%%#######%%@@@@@@:   .+ ....=:.      
                     ::-#++#+--=%=-=--:-+%#*+-+-:.++----:: ++##@**+**##=*@#   .:*#%#%@@@@@%*:            .:...--.    
                     .-#=++#:-**=:--=====*--====+=#*+++=-:=#+%+-@+#**+**=*@@@@=     ..--:.  ...           .:-:...    
                    :-*=**##%#%%#****==-*+-:--*-=-##-:.=%@#*+***=%#*+++**=+:%%@@@@@%            :            -:=     
                    --**#+%**+%**####%%#@##*###=-:**-%%#****+%%**+*#*#++*+*#=#%=:*@@@@@@@@@@@@@                      
                   -:#****%**+#*#**#***+%*##*#%###@@+*******+*@-#**+*+#**++####@=-=+@%%+  @%.:                       
                  ..-#+++%+*=*+*=++++++#*+**+****+@@=*******++#@=+##+*#**+++#+#@#+=*@% #-:@=.#                       
                   :#++++#+*+#*%#++++++%=++++##+++=@+*******+=@#%*=%*++#**#+***#@=+*+@:%+:@%:+#                      
                  -=%=++***-#*=+++***++*++**#@@+***##+**+****=@:#*%:+@+=%*%@++#=%#=+%@-**-*@:=*                      
                  -*+++=#+*-@%.#*-**+**#%=++@ @-++# @==+***++*@@-+#@@@@@@*.:%#**%#=+=@+=*=:@:-+                      
                  :@-++=%=#-#- @#-+=-=#=@-++@  ==+@ @@=+++*++%.@@@%+    =@%@#*+*#@=++#@=#=.@-==:                     
                 -=%+++=%:%=@* @%.+=+*- @:.-@@@@-=@ %*+=+++==%.. @. .  @##@:%*#==@*++=@=#=:@+-==                     
                .-+=%-+-@@* =  .@ =:=@:.@% :@  --@#  @@-+=+-=@+-   @@@@@%-+#-%##+*@+*-@+*=:@#-+==                    
                --%=#-+-@.%@@@@.+=-.+@. #@= @.  +@@# :@--==--%@* .   @@@*=*@#%+**-@+*+@+*+=##-==-                    
               :::=-*-+:@+=.@ @@* @@@@  .@@-@-   +@@  @@====-#-@ ...  @@*%@@*#@@@@@@@+@+*+=*#-==-                    
               .=#*-#-+-@@:%. # @-     .    @#..        :.:-=+:@ ....  +@##@@@@@    @@%+++=+#-==#                    
               .==@:#-+-@@#@   @@@ ........   .@@@@@@@@@@+=-..+@ .....   =@@    .#*  @@++==+*=#=%                    
                @.*-*-+=%@ @. @%   ........ @        .@#@@@@@@@.+ ...... .   .%  .*: @@=*+=++-@:%                    
               .*:*=*:*+*@ @*    ..........   ... :@= : -@  @=# + ...... +.. %*% .++  @-*+=#=:@.#.                   
               -====+:#+*@ *  .................... %@+.=@ *%=-@ * ......% ..  :*..*  %@:++=#=:@.#.                   
               ==#=+=:##+@ +.......................       @+=+@ # .....:.   %#%  +  @@*+++-@=+*:*.                   
               +-=:+=.#@+@ =............................. -@=*# :.........@==. -. .@#%-#==-#====-                    
               :.=+++.*@+@ =............................. .@+#* .......        .-#-@%@-*==+%=@:%.                    
                 +:-+.*@*@ -. ............................ @*@ :.....   @@@@@@#.  @@ =%-=-+**::::                    
                 =+=*.*@=@  @ ............................ @%- =..... @=:.:  .-. :@:=#*---#@=.=.@                    
                ==-:#:+@: @ .+  ..   ..................... @@-=...  #=..:.::.*-  @@ +@::.:@+ .                       
                -+*.@.-@-  =. @ .. @ ..................... #@    =+.%.....:..=  @@ -#-  :=-                          
                 +*.@+:@- .:...=..  +..................... +#=.+===:@ .::.-.-: @@  %*                                
                 -=.@::@: ::.:::+.......................   @+.:-==. +.=:...:- @@  =.                                 
                  : %:.@: ::.:::..+................    .+-:%.:=+ ...# -      -                                       
                    +@.@= .:.:::-..*............   =*===- =@.+....                                                   
                     @ @# .:...--...+=....+-:%*. @::::--: @         +                                                
                     @.@% .....-.-..+..-  .      @        ..-@@%@@@@@                                                
                     %*+@ ....:-  ..#=:#:        @ .=@@@@@=#%%@@@@@@                                                 
                      @ @ ......%                #:@@@@@ #@*                                                         
                      : @.:                       @*         .......  @                                              
                      -:                         -.:  .......          :.=@                                          
                    . ..                         %  ...       @@@@@@%%=.                                             
                     =-                                 @@@@@%+===++==-+*+***@@#*@@@@                                
                                                  .@@@@%*=--:...:=*####**+==:.                                       
                                                                                                                     
";
}

internal class MessageFormatter
{
	private readonly DateTime Timestamp;
	private readonly string ComponentName;
	private readonly Logger.Severity Level;
	private readonly string Content;

	public MessageFormatter(DateTime timestamp, string component, Logger.Severity level, string content)
	{
		Timestamp = timestamp;
		ComponentName = component;
		Level = level;
		Content = content.Replace("[", "[[").Replace("]", "]]");
	}

	public string Format()
	{
		var timeSection = $"[[[deepskyblue1]{Timestamp:HH:mm:ss}[/]]]";
		var componentSection = $"[[[mediumpurple1]{ComponentName}[/]]]";
		var levelSection = GetLevelSection();
		return $"{timeSection} {componentSection} {levelSection} {Content}";
	}

	private string GetLevelSection()
	{
		return Level switch
		{
			Logger.Severity.SUCCESS => $"[[[green1]{Level}[/]]]",
			Logger.Severity.FAILURE => $"[[[red1]{Level}[/]]]",
			Logger.Severity.WARNING => $"[[[yellow1]{Level}[/]]]",
			Logger.Severity.INFO => $"[[[cyan1]{Level}[/]]]",
			Logger.Severity.TRACE => $"[[[blue1]{Level}[/]]]",
			_ => $"[[[{(ConsoleColor)Level}]{Level}[/]]]",
		};
	}
}
