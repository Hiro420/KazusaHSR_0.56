namespace KazusaHSR.GameServer.Resource;

public class StackWeakResistance : TaskConfig
{
	public AFPCGEIFEIF StackType { get; set; }
	public TargetEvaluator TargetType { get; set; }
	public ResistanceItem[] StackResistanceList { get; set; }
}
