namespace KazusaHSR.GameServer.Resource.Excel;

public class EquipmentSkillRow
{
	public uint SkillID { get; set; }
	public TextID SkillName { get; set; }
	public TextID SkillDesc { get; set; }
	public uint Level { get; set; }
	public string AbilityName { get; set; }
	public FixPoint[] ParamList { get; set; }
}
