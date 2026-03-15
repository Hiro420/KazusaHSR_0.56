using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public sealed class AdventureAbilityContext
{
	public Session Session { get; }
	public PlayerAvatar Avatar { get; }
	public AdventureAbilityConfig AbilityConfig { get; }
	public SceneCastSkillCsReq Request { get; }
	public SceneCastSkillScRsp Response { get; }

	public AdventureAbilityContext(Session session, PlayerAvatar avatar, AdventureAbilityConfig abilityConfig, SceneCastSkillCsReq request, SceneCastSkillScRsp response)
	{
		Session = session;
		Avatar = avatar;
		AbilityConfig = abilityConfig;
		Request = request;
		Response = response;
	}

	public BaseEntity? GetAbilityEntity()
	{
		if (Request.AbilityTargetEntityId == 0)
			return null;

		Session.player.Scene.EntityManager.TryGet(Request.AbilityTargetEntityId, out BaseEntity? entity);
		return entity;
	}
}