using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public abstract class BaseEntity
{
	public Session Session { get; private set; }
	public uint _EntityId { get; private set; }
	private Protocol.Vector _position;
	public HashSet<uint> AttachedMazeBuffs { get; set; } = new HashSet<uint>();
	public HashSet<string> AttachedModifiers { get; set; } = new HashSet<string>();

	public Protocol.Vector Position
	{
		get
		{
			return this is AvatarEntity avatarEntity ? avatarEntity.Session.player!.Pos : _position;
		}
		set
		{
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
		MazeBuffRow? buffRow = MainApp.resourceManager.MazeBuff.Find(x => x.ID == mazeBuffId);
		if (buffRow != null && !string.IsNullOrEmpty(buffRow.ModifierName))
		{
			Session.c.Message($"Adding modifier {buffRow.ModifierName} from maze buff {mazeBuffId} to entity {_EntityId}");
			AttachedModifiers.Add(buffRow.ModifierName);
		}
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
		MazeBuffRow? buffRow = MainApp.resourceManager.MazeBuff.Find(x => x.ID == mazeBuffId);
		if (buffRow != null && !string.IsNullOrEmpty(buffRow.ModifierName))
		{
			Session.c.Message($"Removing modifier {buffRow.ModifierName} from maze buff {mazeBuffId} from entity {_EntityId}");
			AttachedModifiers.Remove(buffRow.ModifierName);
		}
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
		foreach (var buffId in buffsToRemove)
		{
			RemoveMazeBuff(buffId, broadcast);
		}
	}

	public bool HasAdventureModifier(string modifierName)
	{
		return AttachedModifiers.Contains(modifierName);
	}

	protected abstract void BuildKindSpecific(SceneEntityInfo info);
}
