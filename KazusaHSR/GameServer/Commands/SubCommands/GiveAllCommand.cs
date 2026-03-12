using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.ConsoleCommands.Examples;

[ConsoleCommand("giveall", "Give all items or avatars to a player (risky)", "ga", RequiresTarget = true)]
public sealed class GiveAllCommand : IConsoleCommand
{
	public void Execute(CommandExecutionContext context)
	{
		if (context.TargetSession == null || context.TargetUid == null)
		{
			context.Logger.Alert("Give requires a valid target. Use 'target <uid>' first.");
			return;
		}

		if (context.Args.Count == 0)
		{
			context.Logger.Alert("Usage: giveall <items|avatars> [level]");
			return;
		}

		string action = context.Args[0].ToLowerInvariant();
		string levelStr = context.Args.Count > 1 ? context.Args[1] : "1";

		if (!uint.TryParse(levelStr, out uint level))
		{
			context.Logger.Alert("Invalid parameters. Level must be an integer.");
			return;
		}

		switch (action)
		{
			case "items":
				DoItems(level, context);
				break;
			case "avatars":
				DoAvatars(level, context);
				context.TargetSession.player.SendSyncLineupNotify();
				break;
		}
		context.TargetSession.player.SavePersistent();
	}

	private void DoItems(uint level, CommandExecutionContext context)
	{
		Session session = context.TargetSession!;
		PlayerSyncScNotify ntf = new PlayerSyncScNotify()
		{
			MaterialLists = { },
			EquipmentLists = { }
		};
		MazePlaneEventScNotify ntf2 = new MazePlaneEventScNotify()
		{
			Reward = new RewardData()
			{
				ItemLists = { }
			}
		};
		foreach (PlayerItem item in session.player.GiveAllItems(level))
		{
			switch (item)
			{
				case ItemLightcone lc:
					ntf.EquipmentLists.Add(lc.ToEquipmentProto());
					ntf2.Reward.ItemLists.Add(new RewardItem()
					{
						ItemId = lc.ItemId,
						Level = lc.Level,
						Num = 1,
						Promotion = lc.Promotion,
						Rank = lc.Rank
					});
					break;
				default:
					ntf.MaterialLists.Add(item.ToMaterialProto());
					ntf2.Reward.ItemLists.Add(new RewardItem()
					{
						ItemId = item.ItemId,
						Level = level,
						Num = 1,
					});
					break;
			}
		}
		if (session.SendPacket(ntf) && session.SendPacket(ntf2))
			context.Logger.Emit($"Gave all items to {session.player.Name} (Level {level}).");
		else
			context.Logger.Alert("Failed to send items.");
	}

	private void DoAvatars(uint level, CommandExecutionContext context)
	{
		Session session = context.TargetSession!;

		PlayerSyncScNotify ntf = new()
		{
			AvatarSync = { AvatarLists = { } }
		};

		List<AddAvatarScNotify> ntfs = [];

		foreach (PlayerAvatar avatar in session.player.GiveAllAvatars(level))
		{
			AddAvatarScNotify scNotify = new AddAvatarScNotify()
			{
				AvatarId = avatar.AvatarId,
				Rank = avatar.Rank,
			};
			ntfs.Add(scNotify);
		}

		if (session.SendPacket(ntf) && ntfs.All(session.SendPacket))
			context.Logger.Emit($"Gave all avatars to {session.player.Name} (Level {level}).");
		else
			context.Logger.Alert("Failed to send avatars.");
	}
}