namespace KazusaHSR.GameServer.Resource;

public class ByAvatarBaseType : PredicateConfig
{
	public AvatarBaseType[] BaseTypeList { get; set; }
	public TargetEvaluator TargetType { get; set; }
}
