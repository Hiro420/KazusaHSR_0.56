namespace KazusaHSR.GameServer.Resource;

public class LevelMinimapSection
{
	public uint ID { get; set; }
	public string Sprite { get; set; }
	public bool IsRect { get; set; }
	public uint[] Indices { get; set; }
	public MVector2 UIPosition { get; set; }
	public bool InitialHidden { get; set; }
	public LevelMinimapSectionType Type { get; set; }
	public MVector2 Center { get; set; }
	public uint GroupID { get; set; }
	public uint PropID { get; set; }
}
