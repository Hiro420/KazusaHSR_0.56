using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class StageRow
{
	public uint StageId { get; set; }
	public string StageType { get; set; }
	public TextID StageName { get; set; }
	public uint HardLevelGroup { get; set; }
	public uint Level { get; set; }
	public string LevelGraphPath { get; set; }
	public List<string> StageAbilityConfig { get; set; }
	public List<object> SubLevelGraphs { get; set; }
	public List<object> StageConfigData { get; set; }
	public List<Dictionary<string, uint>> MonsterList { get; set; }
	public List<object> LevelLoseCondition { get; set; }
	public List<string> LevelWinCondition { get; set; }
	public bool? ForbidExitBattle { get; set; }
	public bool? Release { get; set; }
	public bool? ForbidAutoBattle { get; set; }
}