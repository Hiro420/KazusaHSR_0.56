namespace KazusaHSR.GameServer.Resource;

public class CharacterChangePhase : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string PhaseName { get; set; }
	public CharacterPhaseAnimConfig PhaseAnimConfig { get; set; }
}
