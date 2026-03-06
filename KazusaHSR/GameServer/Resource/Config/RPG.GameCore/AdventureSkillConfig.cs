namespace KazusaHSR.GameServer.Resource;

public class AdventureSkillConfig
{
	public string Name { get; set; }
	public bool CanSwitchActiveState { get; set; }
	public bool CanCastWithoutTarget { get; set; }
	public bool SubmitSkillWhenCast { get; set; }
	public AdventureSkillType AdventureSkillType { get; set; }
	public LFMPHDMIDPN UseType { get; set; }
	public AdventureSkillTargetConfig TargetInfo { get; set; }
	public string EntryAbility { get; set; }
	public SkillRangeConfig RangeConfig { get; set; }
	public float CoolDown { get; set; }
}
