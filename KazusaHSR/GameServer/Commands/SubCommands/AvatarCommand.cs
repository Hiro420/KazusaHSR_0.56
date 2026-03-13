using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using System.Globalization;

namespace KazusaHSR.GameServer.ConsoleCommands.Examples;

[ConsoleCommand("avatar", "Modify specific avatar", "av", RequiresTarget = true)]
public sealed class AvatarCommand : IConsoleCommand
{
	private enum AvatarSelectorKind
	{
		All,
		Single
	}

	private enum AvatarModificationKind
	{
		Level,
		Eidolon,
		SkillLevel
	}

	private readonly record struct AvatarSelector(
		AvatarSelectorKind Kind,
		uint AvatarId = 0);

	private readonly record struct AvatarModification(
		AvatarModificationKind Kind,
		uint Value);

	private readonly record struct AvatarCommandRequest(
		AvatarSelector Selector,
		AvatarModification Modification);

	public void Execute(CommandExecutionContext context)
	{
		if (context.TargetSession == null || context.TargetUid == null)
		{
			context.Logger.Alert("Avatar requires a valid target. Use 'target <uid>' first.");
			return;
		}

		if (!TryParseRequest(context.Args, out AvatarCommandRequest request, out string error))
		{
			context.Logger.Alert(error);
			return;
		}

		switch (request.Modification.Kind)
		{
			case AvatarModificationKind.Level:
				SetLevel(context, request.Selector, request.Modification.Value);
				break;
			case AvatarModificationKind.Eidolon:
				SetEidolon(context, request.Selector, request.Modification.Value);
				break;
			case AvatarModificationKind.SkillLevel:
				SetSkillLevel(context, request.Selector, request.Modification.Value);
				break;
		}
		context.TargetSession.player.SavePersistent();
	}

	private static bool TryParseRequest(
		IReadOnlyList<string> args,
		out AvatarCommandRequest request,
		out string error)
	{
		request = default;
		error = string.Empty;

		if (args.Count < 2)
		{
			error = GetUsage();
			return false;
		}

		if (!TryParseSelector(args[0], out AvatarSelector selector, out error))
		{
			return false;
		}

		if (!TryParseModification(args, startIndex: 1, out AvatarModification modification, out error))
		{
			return false;
		}

		request = new AvatarCommandRequest(selector, modification);
		return true;
	}

	private static bool TryParseSelector(
		string rawSelector,
		out AvatarSelector selector,
		out string error)
	{
		selector = default;
		error = string.Empty;

		if (string.Equals(rawSelector, "all", StringComparison.OrdinalIgnoreCase))
		{
			selector = new AvatarSelector(AvatarSelectorKind.All);
			return true;
		}

		if (uint.TryParse(rawSelector, NumberStyles.None, CultureInfo.InvariantCulture, out uint avatarId))
		{
			selector = new AvatarSelector(AvatarSelectorKind.Single, avatarId);
			return true;
		}

		error = $"Invalid avatar selector '{rawSelector}'. Expected 'all' or a numeric avatar id.";
		return false;
	}

	private static bool TryParseModification(
		IReadOnlyList<string> args,
		int startIndex,
		out AvatarModification modification,
		out string error)
	{
		modification = default;
		error = string.Empty;

		if (startIndex >= args.Count)
		{
			error = "Missing modification arguments. " + GetUsage();
			return false;
		}

		string token = args[startIndex].ToLowerInvariant();

		// Examples:
		// lv 80
		// level 80
		// sl 10
		// skill 10
		// e5
		// e 5
		// eidolon 5

		if (token is "lv" or "level")
		{
			return TryParseValueArgument(
				args,
				startIndex + 1,
				AvatarModificationKind.Level,
				min: 1,
				max: 100,
				out modification,
				out error,
				"value for level");
		}

		if (token is "sl" or "skill" or "skilllevel" or "skill-level")
		{
			return TryParseValueArgument(
				args,
				startIndex + 1,
				AvatarModificationKind.SkillLevel,
				min: 1,
				max: 15,
				out modification,
				out error,
				"value for skill level");
		}

		if (token is "e" or "eidolon")
		{
			return TryParseValueArgument(
				args,
				startIndex + 1,
				AvatarModificationKind.Eidolon,
				min: 0,
				max: 6,
				out modification,
				out error,
				"value for eidolon");
		}

		// Compact eidolon syntax: e5
		if (token.Length >= 2 && token[0] == 'e')
		{
			string eidolonValue = token[1..];
			if (TryParseInt(eidolonValue, out uint parsedEidolon) && parsedEidolon is >= 0 and <= 6)
			{
				modification = new AvatarModification(AvatarModificationKind.Eidolon, parsedEidolon);
				return true;
			}

			error = $"Invalid eidolon syntax '{args[startIndex]}'. Expected e0-e6 or 'eidolon <value>'.";
			return false;
		}

		error = $"Unknown modification '{args[startIndex]}'. Supported: lv, e, sl.";
		return false;
	}

	private static bool TryParseValueArgument(
		IReadOnlyList<string> args,
		int index,
		AvatarModificationKind kind,
		int min,
		int max,
		out AvatarModification modification,
		out string error,
		string valueDescription)
	{
		modification = default;
		error = string.Empty;

		if (index >= args.Count)
		{
			error = $"Missing {valueDescription}.";
			return false;
		}

		if (!TryParseInt(args[index], out uint value))
		{
			error = $"Invalid {valueDescription} '{args[index]}'. Expected an integer.";
			return false;
		}

		if (value < min || value > max)
		{
			error = $"{kind} must be between {min} and {max}.";
			return false;
		}

		modification = new AvatarModification(kind, value);
		return true;
	}

	private static bool TryParseInt(string raw, out uint value)
	{
		return uint.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
	}

	private static string GetUsage()
	{
		return "Usage: avatar <all|id> <lv|level|e|eidolon|sl|skill> <value>  (example: avatar 1001 lv 80, avatar all e5, avatar 1001 sl 10)";
	}

	private void SetLevel(CommandExecutionContext context, AvatarSelector selector, uint level)
	{
		PlayerSyncScNotify ntf = new()
		{
			AvatarSync = new AvatarSync()
		};
		foreach (PlayerAvatar avatar in GetTargetAvatars(context, selector))
		{
			AvatarPromotionRow? maxPromo = MainApp.resourceManager.AvatarPromotionExcel.Where(p => p.AvatarID == avatar.AvatarId)
				.OrderByDescending(p => p.Promotion)
				.FirstOrDefault();
			uint maxLevel = maxPromo != null ? maxPromo.MaxLevel : 80;
			avatar.Level = Math.Min(maxLevel, level);
			avatar.PromoteLevel = ResolvePromotionAV(avatar.AvatarExcel, avatar.Level);
			ntf.AvatarSync.AvatarLists.Add(avatar.ToAvatarProto());
			context.Logger.Emit($"Set level of avatar {avatar.AvatarId} to {level}");
		}
		if (context.TargetSession!.SendPacket(ntf))
		{
			context.Logger.Emit($"Successfully applied changes to {ntf.AvatarSync.AvatarLists.Count} avatars.");
		}
		else
		{
			context.Logger.Alert("Failed to send avatar sync notification to client.");
		}
	}

	private void SetEidolon(CommandExecutionContext context, AvatarSelector selector, uint eidolon)
	{
		PlayerSyncScNotify ntf = new()
		{
			AvatarSync = new AvatarSync()
		};
		foreach (PlayerAvatar avatar in GetTargetAvatars(context, selector))
		{
			avatar.Rank = Math.Min(avatar.AvatarExcel.MaxRank, eidolon);
			ntf.AvatarSync.AvatarLists.Add(avatar.ToAvatarProto());
			context.Logger.Emit($"Set eidolon of avatar {avatar.AvatarId} to {eidolon}");
		}
		if (context.TargetSession!.SendPacket(ntf))
		{
			context.Logger.Emit($"Successfully applied changes to {ntf.AvatarSync.AvatarLists.Count} avatars.");
		}
		else
		{
			context.Logger.Alert("Failed to send avatar sync notification to client.");
		}
	}

	private void SetSkillLevel(CommandExecutionContext context, AvatarSelector selector, uint skillLevel)
	{
		PlayerSyncScNotify ntf = new()
		{
			AvatarSync = new AvatarSync()
		};
		foreach (PlayerAvatar avatar in GetTargetAvatars(context, selector))
		{
			foreach (var group in MainApp.resourceManager.AvatarSkillTreeExcel
				.Where(s => s.AvatarID == avatar.AvatarId && s.Level == Math.Min(skillLevel, s.MaxLevel))
				.GroupBy(a => a.PointID))
			{
				var firstRow = group.First();
				avatar.SkilltreeLists[group.Key] = Math.Min(skillLevel, firstRow.MaxLevel);
			}
			ntf.AvatarSync.AvatarLists.Add(avatar.ToAvatarProto());
			context.Logger.Emit($"Set skill level of avatar {avatar.AvatarId} to {skillLevel}");
		}
		if (context.TargetSession!.SendPacket(ntf))
		{
			context.Logger.Emit($"Successfully applied changes to {ntf.AvatarSync.AvatarLists.Count} avatars.");
		}
		else
		{
			context.Logger.Alert("Failed to send avatar sync notification to client.");
		}
	}

	private IEnumerable<PlayerAvatar> GetTargetAvatars(CommandExecutionContext context, AvatarSelector selector)
	{
		Player player = context.TargetSession!.player;
		return selector.Kind switch
		{
			AvatarSelectorKind.All => player.avatarDict.Values.Where(a => a != null)!,
			AvatarSelectorKind.Single => player.avatarDict.Values.Where(a => a.AvatarId == selector.AvatarId),
			_ => Enumerable.Empty<PlayerAvatar>()
		};
	}

	private uint ResolvePromotionAV(AvatarRow avatarRow, uint level)
	{
		IEnumerable<AvatarPromotionRow> promotionRows = MainApp.resourceManager.AvatarPromotionExcel.Where(i =>
			i.MaxLevel >= level && i.AvatarID == avatarRow.AvatarID);
		return promotionRows.OrderByDescending(i => i.Promotion).Select(i => i.Promotion).FirstOrDefault();
	}
}