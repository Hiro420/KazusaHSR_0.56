using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class NpcRow
{
	public uint Id { get; set; }
	public TextID TextIDName { get; set; }
	public string PrefabPath { get; set; }
	public string TextIDIconPath { get; set; }
	public string DefaultStory { get; set; }
	public TextID TextIDTitle { get; set; }
	public List<uint> BoardShowList { get; set; }
	public string JsonPath { get; set; }
	public string DefaultAiPath { get; set; }
	public string DefaultIdleStateName { get; set; }
	public string CharacterType { get; set; }
	public string Rank { get; set; }
	public List<uint> TalkDialogueGroupIdList { get; set; }
	public uint? MiniMapIconType { get; set; }
	public string TalkChosenType { get; set; }
}