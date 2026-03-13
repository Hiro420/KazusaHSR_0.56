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

		sb.AppendLine("\n# Avatars");
		foreach (AvatarRow avatar in MainApp.resourceManager.AvatarExcel.OrderBy(a => a.AvatarID))
		{
			sb.AppendLine($"{avatar.AvatarID} - {GetTextMap(avatar.AvatarName)}");
		}

		sb.AppendLine("\n# Items");
		foreach (ItemRow itemRow in MainApp.resourceManager.ItemConfig.OrderBy(a => a.ID))
		{
			sb.AppendLine($"{itemRow.ID} - {GetTextMap(itemRow.ItemName)}");
		}

		sb.AppendLine("\n# Lightcones");
		foreach (ItemRow itemRow in MainApp.resourceManager.ItemConfigEquipment.OrderBy(a => a.ID))
		{
			sb.AppendLine($"{itemRow.ID} - {GetTextMap(itemRow.ItemName)}");
		}

		sb.AppendLine("\n# Scenes");
		foreach (MazePlaneRow sceneRow in MainApp.resourceManager.MazePlaneExcel.OrderBy(a => a.PlaneID))
		{
			sb.AppendLine($"{sceneRow.PlaneID} - {GetTextMapByName(sceneRow.PlaneName)}");
			foreach (uint floorID in sceneRow.FloorIDList)
			{
				var floorRow = MainApp.resourceManager.MazeFloorExcel.FirstOrDefault(f => f.FloorID == floorID);
				if (floorRow != null)
				{
					sb.AppendLine($"\t- {floorRow.FloorID} - {GetTextMapByName(floorRow.FloorName)}");
				}
			}
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

	private static string GetTextMapByName(string str)
	{
		unchecked
		{
			int hash1 = 5381;
			int hash2 = hash1;

			for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
			{
				hash1 = ((hash1 << 5) + hash1) ^ str[i];
				if (i == str.Length - 1 || str[i + 1] == '\0')
					break;
				hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
			}

			int ihash = (hash1 + (hash2 * 1566083941));

			return GetTextMap(new TextID() { Hash = ihash });
		}
	}
}
