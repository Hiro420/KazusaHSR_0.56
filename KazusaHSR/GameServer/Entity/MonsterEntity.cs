using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer;

public class MonsterEntity : BaseEntity
{
	public LevelMonsterInfo DbInfo { get; init; }
	public uint GroupId { get; init; }
	public uint EventId { get; set; }
	public MonsterRow MonsterData => MainApp.resourceManager.MonsterExcel.First(m => m.MonsterId == DbInfo.NPCMonsterID);

	public MonsterEntity(Session session, LevelMonsterInfo _levelMonsterInfo, uint groupId) : base(
		session,
		Protocol.EntityType.EntityMonster,
		new Protocol.Vector()
		{
			X = (int)(_levelMonsterInfo.PosX * 1000),
			Y = (int)(_levelMonsterInfo.PosY * 1000),
			Z = (int)(_levelMonsterInfo.PosZ * 1000)
		},
		new Protocol.Vector()
		{
			X = 0,
			Y = (int)(_levelMonsterInfo.RotY * 1000),
			Z = 0,
		})
	{
		this.DbInfo = _levelMonsterInfo;
		this.GroupId = groupId;
		this.EventId = _levelMonsterInfo.EventID;
	}

	protected override void BuildKindSpecific(SceneEntityInfo info)
	{
		info.NpcMonster = new SceneNpcMonsterInfo()
		{
			// todo: figure out wtf IsGenMonster is for
			MonsterId = DbInfo.NPCMonsterID,
		};
		info.InstId = DbInfo.ID;
		info.GroupId = GroupId;
	}

}
