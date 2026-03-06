namespace KazusaHSR.GameServer.Resource;

public class LevelMinimapVolume
{
	//public Sprite BackgroundMapSpriteInstance { get; set; }
	public MVector3 Center { get; set; }
	public MVector3 Size { get; set; }
	public float Scale { get; set; }
	public float CompleteScale { get; set; }
	public MVector2[] SectionVertices { get; set; }
	public string Atlas { get; set; }
	public LevelMinimapSection[] Sections { get; set; }
	public string BackgroundMapSprite { get; set; }
	public bool CombineLayerAutomatic { get; set; }
	public float Rotation { get; set; }
}
