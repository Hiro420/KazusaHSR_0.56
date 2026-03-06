using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer;

public class NpcEntity : BaseEntity
{
	public LevelNPCInfo DbInfo { get; init; }
	public uint GroupId { get; init; }

	public NpcEntity(Session session, LevelNPCInfo _levelNpcInfo, uint groupId) : base(
		session, 
		Protocol.EntityType.EntityNpc, 
		new Protocol.Vector() { 
			X = (int)(_levelNpcInfo.PosX * 1000), 
			Y = (int)(_levelNpcInfo.PosY * 1000), 
			Z = (int)(_levelNpcInfo.PosZ * 1000)
		},
		new Protocol.Vector()
		{
			X = 0,
			Y = (int)(_levelNpcInfo.RotY * 1000),
			Z = 0,
		})
	{
		this.DbInfo = _levelNpcInfo;
		this.GroupId = groupId;
	}

	protected override void BuildKindSpecific(SceneEntityInfo info)
	{
		info.Npc = new SceneNpcInfo()
		{
			NpcId = DbInfo.NPCID,
		};
		info.InstId = DbInfo.ID;
		info.GroupId = GroupId;
	}

}
