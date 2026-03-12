using KazusaHSR.GameServer.ConsoleCommands;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.GameServer.Resource.Excel;
using System.Text;

namespace KazusaHSR.GameServer;

public class HandbookGen
{
	public static Dictionary<long, TextmapRow> textmap_en = MainApp.resourceManager.Textmap_en;
	public static Dictionary<long, TextmapRow> textmap_cn = MainApp.resourceManager.Textmap_cn;
	public const string FileName = "Handbook.txt";

	public static void GenerateHandbook()
	{
		StringBuilder sb = new();
		sb.AppendLine("# KazusaHSR Handbook");
		sb.AppendLine($"# Created {DateTime.Now:yyyy/MM/dd HH:mm:ss}\n");

		sb.AppendLine("# Commands");
		var context = new CommandExecutionContext("", "", new List<string>(), Logger.Create(), MainApp.commandDispatcher, null, null);
		foreach (var cmd in context.Dispatcher.Commands.OrderBy(x => x.Attribute.Name, StringComparer.OrdinalIgnoreCase))
		{
			string aliases = cmd.Attribute.Aliases.Count == 0
				? ""
				: $" (aliases: {string.Join(", ", cmd.Attribute.Aliases)})";
			string targetHint = cmd.Attribute.RequiresTarget ? " [target-required]" : "";
			sb.AppendLine($"{cmd.Attribute.Name} - {cmd.Attribute.Description}{aliases}{targetHint}");
		}

		sb.AppendLine("");
		sb.AppendLine("# Avatars");
		foreach (AvatarRow avatar in MainApp.resourceManager.AvatarExcel.OrderBy(a => a.AvatarID))
		{
			sb.AppendLine($"{avatar.AvatarID} - {GetTextMap(avatar.AvatarName)}");
		}

		sb.AppendLine("");
		sb.AppendLine("# Items");
		foreach (ItemRow itemRow in MainApp.resourceManager.ItemConfig.OrderBy(a => a.ID))
		{
			sb.AppendLine($"{itemRow.ID} - {GetTextMap(itemRow.ItemName)}");
		}

		sb.AppendLine("");
		sb.AppendLine("# Lightcones");
		foreach (ItemRow itemRow in MainApp.resourceManager.ItemConfigEquipment.OrderBy(a => a.ID))
		{
			sb.AppendLine($"{itemRow.ID} - {GetTextMap(itemRow.ItemName)}");
		}

		File.WriteAllText(FileName, sb.ToString());
	}

	private static string GetTextMap(TextID textID)
	{
		if (textmap_en.TryGetValue(textID.Hash, out TextmapRow? row))
			return row.Text;
		else if (textmap_cn.TryGetValue(textID.Hash, out TextmapRow? row2))
			return row2.Text;
		else
			return "???";
	}
}
