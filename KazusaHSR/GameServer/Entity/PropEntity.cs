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

public class PropEntity : BaseEntity
{
	public LevelPropInfo DbInfo { get; init; }
	private uint _state;
	public PropState State
	{
		get => (PropState)_state;
		set => _state = (uint)value;
	}
	public uint GroupId { get; init; }

	public PropEntity(Session session, LevelPropInfo _levelPropInfo, uint groupId) : base(
		session, 
		Protocol.EntityType.EntityProp, 
		new Protocol.Vector() { 
			X = (int)(_levelPropInfo.PosX * 1000), 
			Y = (int)(_levelPropInfo.PosY * 1000), 
			Z = (int)(_levelPropInfo.PosZ * 1000)
		},
		new Protocol.Vector()
		{
			X = 0,
			Y = (int)(_levelPropInfo.RotY * 1000),
			Z = 0,
		})
	{
		this.DbInfo = _levelPropInfo;
		// ik its hacky but whatever. its a ps, let people have fun
		this.State = _levelPropInfo.State != PropState.CheckPointDisable ? _levelPropInfo.State : PropState.CheckPointEnable;
		this.GroupId = groupId;
	}

	protected override void BuildKindSpecific(SceneEntityInfo info)
	{
		info.Prop = new ScenePropInfo()
		{
			CreateTimeMs = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
			// LifeTimeMs = 0 means infinite lifetime
			PropId = DbInfo.PropID,
			PropState = _state,
		};
		info.InstId = DbInfo.ID;
		info.GroupId = GroupId;
	}

}
