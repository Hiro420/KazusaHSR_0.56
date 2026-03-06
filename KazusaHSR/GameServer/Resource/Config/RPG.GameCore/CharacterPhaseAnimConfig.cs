namespace KazusaHSR.GameServer.Resource;

public class CharacterPhaseAnimConfig : CharacterReactionAnimConfig
{
	public string StandBy { get; set; }
	public string StandByLowHP { get; set; }
	public ModifierBehaviorVisual[] ModifierBehaviorVisuals { get; set; }
}
