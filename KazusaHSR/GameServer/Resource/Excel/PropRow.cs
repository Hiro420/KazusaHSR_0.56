using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class PropRow
{
	public uint ID { get; set; }
	public PropType PropType { get; set; }
	public bool HideInCutScene { get; set; }
	public bool HideInStory { get; set; }
	public uint PropParam { get; set; }
	public TextID PropName { get; set; }
	public TextID PropTitle { get; set; }
	public string PropTypeIconPath { get; set; }
	public string PropIconPath { get; set; }
	public int[] BoardShowList { get; set; }
	public string PrefabPath { get; set; }
	public string InitLevelGraph { get; set; }
	public AttackDamageType[] DamageTypeList { get; set; }
	public uint MiniMapIconType { get; set; }
	public PropStateIcon[] MiniMapStateIcons { get; set; }
	public string JsonPath { get; set; }
	public PropState[] PropStateList { get; set; }
	public LLOLMGEEBKJ TalkChosenType { get; set; }
	public uint[] TalkDialogueGroupIDList { get; set; }
}
