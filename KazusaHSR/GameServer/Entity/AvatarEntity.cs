using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public class AvatarEntity : BaseEntity
{
	public PlayerAvatar DbInfo { get; }

	public AvatarEntity(Session session, PlayerAvatar _playerAvatar) : base(session, Protocol.EntityType.EntityAvatar, session.player.Pos, session.player.Rot)
	{
		DbInfo = _playerAvatar;
	}

	protected override void BuildKindSpecific(SceneEntityInfo info)
	{
		info.Actor = new SceneActorInfo()
		{
			AvatarId = DbInfo.AvatarId,
			AvatarType = AvatarType.AvatarFormalType,
			Uid = DbInfo.Guid,
		};
	}


	public void RecoverHp(uint hp = 0)
	{
		if (hp == 0)
			hp = this.DbInfo.GetMaxHp();
		this.DbInfo.Hp += hp;
		if (this.DbInfo.Hp > this.DbInfo.MaxHp)
			this.DbInfo.Hp = this.DbInfo.MaxHp;
		SendEntityUpdate();
	}

	public void ConsumeSp(uint sp)
	{
		if (sp > this.DbInfo.SP)
			sp = this.DbInfo.SP; // prevent underflow, just set to 0 if trying to consume more than current SP
		this.DbInfo.SP -= sp;
		SendEntityUpdate();
	}

	public void RecoverSP(uint sp = 0)
	{
		if (sp == 0)
			sp = (uint)this.DbInfo.AvatarExcel.SPNeed.Value; // if no specific SP amount provided, recover to full SP
		this.DbInfo.SP += sp;
		this.DbInfo.SP = Math.Min(this.DbInfo.SP, (uint)this.DbInfo.AvatarExcel.SPNeed.Value); // cap SP at max SP from config
		SendEntityUpdate();
	}

	public void SendEntityUpdate()
	{
		SceneEntityInfo info = this.ToSceneEntityInfo();
		SceneEntityUpdateScNotify updateInfo = new SceneEntityUpdateScNotify()
		{
			EntityLists = { info }
		};
		this.Session.SendPacket(updateInfo);
	}
}
