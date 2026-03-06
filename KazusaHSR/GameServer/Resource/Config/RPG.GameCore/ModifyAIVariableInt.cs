namespace KazusaHSR.GameServer.Resource;

public class ModifyAIVariableInt : TaskConfig
{
	public string VarName { get; set; }
	public MMLKBAFCOEM ModifyFunc { get; set; }
	public int ChangeValue { get; set; }
}
