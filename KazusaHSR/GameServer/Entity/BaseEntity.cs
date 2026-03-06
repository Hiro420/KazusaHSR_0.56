using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer;

public abstract class BaseEntity
{
	public Session Session { get; private set; }
	public uint _EntityId { get; private set; }
	private Protocol.Vector _position;
	public HashSet<uint> AttachedMazeBuffs { get; set; } = new HashSet<uint>();

	public Protocol.Vector Position 
	{ 
		get {
			return this is AvatarEntity avatarEntity ? avatarEntity.Session.player!.Pos : _position;
		}
		set { 
			_position = value; 
		}
	}
	public Protocol.Vector Rotation { get; set; }

	public BaseEntity(Session _session, EntityType entityType, Protocol.Vector pos, Protocol.Vector rot)
	{
		_EntityId = _session.GetEntityId(entityType);
		Position = pos;
		Rotation = rot;
		Session = _session;
	}

	public SceneEntityInfo ToSceneEntityInfo()
	{
		var info = new SceneEntityInfo()
		{
			EntityId = _EntityId,
			Motion = this.GetMotionInfo(),
		};

		BuildKindSpecific(info);

		return info;
	}

	public MotionInfo GetMotionInfo()
	{
		return new MotionInfo()
		{
			Pos = this.Position,
			Rot = this.Rotation
		};
	}

	public void AddMazeBuff(uint mazeBuffId, bool broadcast = true)
	{
		AttachedMazeBuffs.Add(mazeBuffId);
		if (broadcast)
		{
			var packet = new MazeBuffScNotify()
			{
				BuffId = mazeBuffId,
				Op = MazeBuffOp.MazeBuffOpAdd,
			};
			this.Session?.SendPacket(packet);
		}
	}

	public void RemoveMazeBuff(uint mazeBuffId, bool broadcast = true)
	{
		AttachedMazeBuffs.Remove(mazeBuffId);
		if (broadcast)
		{
			var packet = new MazeBuffScNotify()
			{
				BuffId = mazeBuffId,
				Op = MazeBuffOp.MazeBuffOpDel,
			};
			this.Session?.SendPacket(packet);
		}
	}

	public void RemoveAllMazeBuffs(bool broadcast = true)
	{
		var buffsToRemove = AttachedMazeBuffs.ToList();
		AttachedMazeBuffs.Clear();
		if (broadcast)
		{
			foreach (var buffId in buffsToRemove)
			{
				var packet = new MazeBuffScNotify()
				{
					BuffId = buffId,
					Op = MazeBuffOp.MazeBuffOpDel,
				};
				this.Session?.SendPacket(packet);
			}
		}
	}

	protected abstract void BuildKindSpecific(SceneEntityInfo info);
}
