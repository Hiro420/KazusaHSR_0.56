using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class NPCDataRow
{
	public uint ID { get; set; }
	public TextID NPCName { get; set; }
	public string PrefabPath { get; set; }
	public string NPCIconPath { get; set; }
	public string DefaultStory { get; set; }
	public TextID NPCTitle { get; set; }
	public int[] BoardShowList { get; set; }
	public string JsonPath { get; set; }
	public string DefaultAIPath { get; set; }
	public string DefaultIdleStateName { get; set; }
	public CharacterType CharacterType { get; set; }
	public uint MiniMapIconType { get; set; }
	public MonsterRank Rank { get; set; }
	public LLOLMGEEBKJ TalkChosenType { get; set; }
	public uint[] TalkDialogueGroupIDList { get; set; }
}
