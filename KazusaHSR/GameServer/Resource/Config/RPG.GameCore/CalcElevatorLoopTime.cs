namespace KazusaHSR.GameServer.Resource;

public class CalcElevatorLoopTime : TaskConfig
{
	public DynamicFloat Elevator2FHeight { get; set; }
	public DynamicFloat MoveDistanceBSAS { get; set; }
	public DynamicFloat MoveDistanceLoop { get; set; }
	public string[] LoopAnimStateNameArray { get; set; }
}
