using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class StageRow
{
	public uint StageID { get; set; }
	public StageType StageType { get; set; }
	public TextID StageName { get; set; }
	public uint HardLevelGroup { get; set; }
	public uint Level { get; set; }
	public string LevelGraphPath { get; set; }
	public string[] StageAbilityConfig { get; set; }
	public GLHBGCGGOCN[] SubLevelGraphs { get; set; }
	public KIAELGDFONN[] StageConfigData { get; set; }
	public List<Dictionary<string, uint>> MonsterList { get; set; } //public StageMonsterWave[] MonsterList { get; set; }
	public string[] LevelLoseCondition { get; set; }
	public string[] LevelWinCondition { get; set; }
	public bool ForbidAutoBattle { get; set; }
	public bool Release { get; set; }
	public bool ForbidExitBattle { get; set; }
	public Dictionary<string, Object> _ProcessedTemplateVariables { get; set; }
	public Dictionary<string, string[]> _ProcessedCustomStringList { get; set; }
	public Dictionary<string, string[]> _ProcessedSubLevelGraphList { get; set; }
}
