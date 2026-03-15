using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Resource;

namespace KazusaHSR.GameServer.Resource;

public class DynamicFloat
{
	public FixedDynamicFloat? FixedValue { get; set; }
	public FixedDynamicFloat? Value { get; set; }

	public class FixedDynamicFloat
	{
		public bool IsDynamic { get; set; } = false;
		public double FixedValue { get; set; } = 0f;
		public List<DynamicExprToken> PostfixExpr { get; set; } = new();
	}

	public class DynamicExprToken
	{
		public string Type { get; set; } = string.Empty;
		public uint Int { get; set; }
	}

	public double Evaluate(AdventureAbilityContext? context = null)
	{
		var valueNode = Value ?? FixedValue;
		if (valueNode == null)
			return 0f;

		if (!valueNode.IsDynamic)
			return valueNode.FixedValue;

		if (context == null)
			return 0f;

		if (TryResolveDynamicNumber(valueNode, context, out double resolved))
			return resolved;

		return 0f;
	}

	private static bool TryResolveDynamicNumber(FixedDynamicFloat valueNode, AdventureAbilityContext context, out double resolved)
	{
		resolved = 0f;
		if (valueNode.PostfixExpr == null || valueNode.PostfixExpr.Count == 0)
			return false;

		DynamicExprToken? token = valueNode.PostfixExpr.FirstOrDefault(t => string.Equals(t.Type, "DynamicNumber", StringComparison.Ordinal));
		if (token == null)
			return false;

		int dynamicKey = DecodeZigZag32(token.Int);
		return TryResolveSkillParam(dynamicKey.ToString(), context, out resolved);
	}

	private static bool TryResolveSkillParam(string dynamicKey, AdventureAbilityContext context, out double resolved)
	{
		resolved = 0f;
		ResourceManager rm = MainApp.resourceManager;

		AdventurePlayerRow? adventurePlayer = rm.AdventurePlayerExcel.FirstOrDefault(p => p.AvatarID == context.Avatar.AvatarId);
		if (adventurePlayer == null)
			return false;

		AdventureCharacterConfig? localConfig = rm.GetLocalPlayerConfig(adventurePlayer.PlayerJsonPath);
		if (localConfig?.DynamicValues?.Floats == null)
			return false;

		if (!localConfig.DynamicValues.Floats.TryGetValue(dynamicKey, out DynamicFloatEntry? dynamicEntry))
			return false;

		DynamicReadInfo? readInfo = dynamicEntry?.ReadInfo;
		if (readInfo == null || !string.Equals(readInfo.Type, "SkillParam", StringComparison.Ordinal))
			return false;

		MazeSkillRow? mazeSkill = ResolveMazeSkillForReadInfo(adventurePlayer, context, rm, readInfo.TriggerKey);
		if (mazeSkill == null)
			return false;

		AvatarSkillRow? avatarSkill = rm.AvatarSkillExcel
			.Where(s => s.SkillID == mazeSkill.RelatedAvatarSkill)
			.OrderBy(s => s.Level)
			.FirstOrDefault();

		if (avatarSkill?.ParamList == null)
			return false;

		if (readInfo.Index < 0 || readInfo.Index >= avatarSkill.ParamList.Length)
			return false;

		resolved = avatarSkill.ParamList[readInfo.Index].Value;
		return true;
	}

	private static MazeSkillRow? ResolveMazeSkillForReadInfo(
		AdventurePlayerRow adventurePlayer,
		AdventureAbilityContext context,
		ResourceManager rm,
		string readInfoTriggerKey)
	{
		if (adventurePlayer.MazeSkillIdList == null || adventurePlayer.MazeSkillIdList.Length == 0)
			return null;

		if (context.Request.SkillIndex < adventurePlayer.MazeSkillIdList.Length)
		{
			uint castMazeSkillId = adventurePlayer.MazeSkillIdList[(int)context.Request.SkillIndex];
			MazeSkillRow? castSkill = rm.MazeSkillExcel.FirstOrDefault(ms => ms.MazeSkillId == castMazeSkillId);
			if (castSkill != null && IsEquivalentTriggerKey(readInfoTriggerKey, castSkill.SkillTriggerKey))
				return castSkill;
		}

		foreach (uint mazeSkillId in adventurePlayer.MazeSkillIdList)
		{
			MazeSkillRow? skill = rm.MazeSkillExcel.FirstOrDefault(ms => ms.MazeSkillId == mazeSkillId);
			if (skill != null && IsEquivalentTriggerKey(readInfoTriggerKey, skill.SkillTriggerKey))
				return skill;
		}

		return null;
	}

	private static bool IsEquivalentTriggerKey(string left, string right)
	{
		if (string.Equals(left, right, StringComparison.OrdinalIgnoreCase))
			return true;

		if (string.Equals(left, "SkillMaze", StringComparison.OrdinalIgnoreCase) &&
			string.Equals(right, "MazeSkill", StringComparison.OrdinalIgnoreCase))
			return true;

		if (string.Equals(left, "MazeSkill", StringComparison.OrdinalIgnoreCase) &&
			string.Equals(right, "SkillMaze", StringComparison.OrdinalIgnoreCase))
			return true;

		return false;
	}

	private static int DecodeZigZag32(uint value)
	{
		return (int)((value >> 1) ^ (uint)-(int)(value & 1));
	}
}
