namespace KazusaHSR.GameServer.Resource;

public class ActiveVirtualCamera : TaskConfig
{
	public string AreaName { get; set; }
	public string AnchorName { get; set; }
	public DynamicString LevelAreaCameraKey { get; set; }
	public bool IsEntryPointCamera { get; set; }
	public bool IsActive { get; set; }
	public string FollowTargetUniqueName { get; set; }
	public string FollowTargetAnchorName { get; set; }
	public string LookAtTargetUniqueName { get; set; }
	public string LookAtTargetAnchorName { get; set; }
	public DynamicString LevelAreaLookAtKey { get; set; }
	public bool WaitBlendFinish { get; set; }
	public VCameraBlend BlendConfig { get; set; }
	public bool VCameraDitherNPCOn { get; set; }
	public float VCameraDitherMaxDistance { get; set; }
	public float VCameraDitherAlphaMin { get; set; }
}
