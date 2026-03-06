using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer;

public class EntityManager
{
	public Session session { get; private set; }
	private readonly Dictionary<uint, BaseEntity> _entities = new();
	public IReadOnlyDictionary<uint, BaseEntity> Entities => _entities;
	public EntityManager(Session session)
	{
		this.session = session;
	}

	public void Add(BaseEntity entity)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		_entities[entity._EntityId] = entity;
	}

	public void Remove(uint entityId)
	{
		_entities.Remove(entityId);
	}

	public void AddRange(IEnumerable<BaseEntity> entities)
	{
		if (entities == null)
			throw new ArgumentNullException(nameof(entities));

		foreach (var entity in entities)
		{
			if (entity == null)
				continue;
			Add(entity);
		}
	}

	public void DespawnMany(IEnumerable<uint> entityIds)
	{
		var notify = new SceneEntityDisappearScNotify();
		List<uint> idsToRemove = new List<uint>();
		foreach (var entityId in entityIds ?? Enumerable.Empty<uint>())
		{
			idsToRemove.Add(entityId);
			Remove(entityId);
		}
		notify.EntityIdLists = idsToRemove.ToArray();
		session.SendPacket(notify);
	}


	public AvatarEntity? TryGetByPlayerAvatar(PlayerAvatar playerAvatar)
	{
		foreach (var entity in _entities.Values.OfType<AvatarEntity>())
		{
			if (entity.DbInfo.AvatarId == playerAvatar.AvatarId)
				return entity;
		}
		return null;
	}

	public bool TryGet(uint entityId, out BaseEntity entity) => _entities.TryGetValue(entityId, out entity!);

}
