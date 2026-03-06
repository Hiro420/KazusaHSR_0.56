namespace KazusaHSR.GameServer.Resource;

public class SummonPartner : TaskConfig
{
	public string PrefabPath { get; set; }
	public string ConfigPath { get; set; }
	public string MemberName { get; set; }
	public MVector3 PosOffset { get; set; }
}
