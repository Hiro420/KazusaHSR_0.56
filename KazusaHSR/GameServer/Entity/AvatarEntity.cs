using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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

}
