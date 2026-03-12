using Kodnix.Character;

namespace KazusaHSR;

public static class IConsole
{
	private const string CommandPrefix = "KazusaHSR >> ";
	private const int MaxHistorySize = 10;
	private static readonly ColorBuilder PrefixFormatter = new ColorBuilder(175, 135, 255);
	private static string FormattedPrefix => PrefixFormatter.Build(CommandPrefix);

	public static List<char> Input { get; set; } = [];
	private static int Position { get; set; } = 0;
	private static readonly List<string> CommandHistory = [];
	private static int HistoryPointer = -1;

	public static event Action<string>? OnCommandExecuted;

	public static void InitConsole()
	{
		Console.Title = "KazusaHSR_0.56";
	}

	public static int GetWidth(string str)
		=> str.ToCharArray().Sum(EastAsianWidth.GetLength);

	public static void RedrawInput(List<char> buffer, bool showPrefix = true)
		=> RedrawInput(string.Concat(buffer), showPrefix);

	public static void RedrawInput(string buffer, bool showPrefix = true)
	{
		int displayWidth = GetWidth(buffer);
		string display = buffer;

		if (showPrefix)
		{
			display = FormattedPrefix + buffer;
			displayWidth += GetWidth(CommandPrefix);
		}

		var (currentLeft, _) = Console.GetCursorPosition();
		if (currentLeft > 0)
			Console.SetCursorPosition(0, Console.CursorTop);

		Console.Write(display + new string(' ', Math.Max(0, Console.BufferWidth - displayWidth)));
		Console.SetCursorPosition(displayWidth, Console.CursorTop);
	}

	public static string ListenConsole()
	{
		while (true)
		{
			ConsoleKeyInfo keyInfo;
			try { keyInfo = Console.ReadKey(true); }
			catch (InvalidOperationException) { continue; }

			_ = keyInfo.Key switch
			{
				ConsoleKey.Enter => ProcessEnter(),
				ConsoleKey.Backspace => ProcessBackspace(),
				ConsoleKey.LeftArrow => ProcessLeftMove(),
				ConsoleKey.RightArrow => ProcessRightMove(),
				ConsoleKey.UpArrow => ProcessUpHistory(),
				ConsoleKey.DownArrow => ProcessDownHistory(),
				_ => ProcessCharacter(keyInfo),
			};
		}
	}

	private static int ProcessEnter()
	{
		string command = string.Concat(Input);
		if (string.IsNullOrWhiteSpace(command))
			return 0;

		Console.WriteLine();
		Input.Clear();
		Position = 0;

		if (CommandHistory.Count >= MaxHistorySize)
			CommandHistory.RemoveAt(0);

		CommandHistory.Add(command);
		HistoryPointer = CommandHistory.Count;

		string trimmed = command.StartsWith('/') ? command[1..].Trim() : command;
		OnCommandExecuted?.Invoke(trimmed);
		RedrawInput("", true);
		return 0;
	}

	private static int ProcessBackspace()
	{
		if (Position <= 0)
			return 0;

		Position--;
		int charWidth = GetWidth(Input[Position].ToString());
		Input.RemoveAt(Position);

		var (left, _) = Console.GetCursorPosition();
		Console.SetCursorPosition(left - charWidth, Console.CursorTop);

		string suffix = string.Concat(Input.Skip(Position));
		Console.Write(suffix + new string(' ', charWidth));
		Console.SetCursorPosition(left - charWidth, Console.CursorTop);
		return 0;
	}

	private static int ProcessLeftMove()
	{
		if (Position <= 0)
			return 0;

		var (left, _) = Console.GetCursorPosition();
		Position--;
		int moveDistance = GetWidth(Input[Position].ToString());
		Console.SetCursorPosition(left - moveDistance, Console.CursorTop);
		return 0;
	}

	private static int ProcessRightMove()
	{
		if (Position >= Input.Count)
			return 0;

		var (left, _) = Console.GetCursorPosition();
		int moveDistance = GetWidth(Input[Position].ToString());
		Position++;
		Console.SetCursorPosition(left + moveDistance, Console.CursorTop);
		return 0;
	}

	private static int ProcessUpHistory()
	{
		if (CommandHistory.Count == 0 || HistoryPointer <= 0)
			return 0;

		HistoryPointer--;
		string entry = CommandHistory[HistoryPointer];
		Input = [.. entry];
		Position = Input.Count;
		RedrawInput(Input);
		return 0;
	}

	private static int ProcessDownHistory()
	{
		if (HistoryPointer >= CommandHistory.Count)
			return 0;

		HistoryPointer++;
		if (HistoryPointer >= CommandHistory.Count)
		{
			HistoryPointer = CommandHistory.Count;
			Input.Clear();
			Position = 0;
		}
		else
		{
			string entry = CommandHistory[HistoryPointer];
			Input = [.. entry];
			Position = Input.Count;
		}
		RedrawInput(Input);
		return 0;
	}

	private static int ProcessCharacter(ConsoleKeyInfo keyInfo)
	{
		if (char.IsControl(keyInfo.KeyChar))
			return 0;

		if (Input.Count >= (Console.BufferWidth - CommandPrefix.Length))
			return 0;

		Input.Insert(Position, keyInfo.KeyChar);
		Position++;

		var (left, _) = Console.GetCursorPosition();
		string suffix = string.Concat(Input.Skip(Position - 1));
		Console.Write(suffix);

		int charWidth = GetWidth(keyInfo.KeyChar.ToString());
		Console.SetCursorPosition(left + charWidth, Console.CursorTop);
		return 0;
	}
}

internal class ColorBuilder
{
	private readonly int _r, _g, _b;

	public ColorBuilder(int red, int green, int blue)
	{
		_r = Math.Clamp(red, 0, 255);
		_g = Math.Clamp(green, 0, 255);
		_b = Math.Clamp(blue, 0, 255);
	}

	public string Build(string text)
		=> $"\u001b[38;2;{_r};{_g};{_b}m{text}\u001b[0m";
}