using KazusaHSR.GameServer.PlayerInfos;

namespace KazusaHSR.GameServer;

public sealed class MPManager
{
	private readonly Player _player;

	public MPManager(Player player)
	{
		_player = player ?? throw new ArgumentNullException(nameof(player));
	}

	public uint GetCurrentAmount()
	{
		return GetActiveLineup().CurMp;
	}

	public uint GetMaxAmount()
	{
		return GetActiveLineup().MaxMp;
	}

	public (uint Current, uint Max) GetCurrentState()
	{
		var lineup = GetActiveLineup();
		return (lineup.CurMp, lineup.MaxMp);
	}

	public uint AddAmount(uint amount)
	{
		if (amount == 0)
			return GetCurrentAmount();

		var lineup = GetActiveLineup();
		lineup.CurMp = Math.Min(lineup.CurMp + amount, lineup.MaxMp);
		PersistAndNotify();
		return lineup.CurMp;
	}

	public uint RestoreFull()
	{
		var lineup = GetActiveLineup();
		lineup.CurMp = lineup.MaxMp;
		PersistAndNotify();
		return lineup.CurMp;
	}

	public uint SetAmount(uint amount)
	{
		var lineup = GetActiveLineup();
		lineup.CurMp = Math.Min(amount, lineup.MaxMp);
		PersistAndNotify();
		return lineup.CurMp;
	}

	public void SpendMP(uint amount)
	{
		if (amount == 0)
			return;

		var lineup = GetActiveLineup();
		lineup.CurMp = Math.Max(lineup.CurMp - amount, 0);
		PersistAndNotify();
	}

	private PlayerTeam GetActiveLineup()
	{
		return _player.GetCurrentLineup();
	}

	private void PersistAndNotify()
	{
		SyncChallengeVirtualLineup();
		if (_player.ChallengeManager.IsInChallenge)
		{
			_player.ChallengeManager.SendSyncLineupNotify();
		}
		else
		{
			_player.SendSyncLineupNotify();
		}

		_player.SavePersistent();
	}

	private void SyncChallengeVirtualLineup()
	{
		if (!_player.ChallengeManager.IsInChallenge)
			return;

		var currentLineup = GetActiveLineup();
		var virtualLineup = _player.ChallengeManager.VirtualLineup;
		virtualLineup.CurMp = currentLineup.CurMp;
		virtualLineup.MaxMp = currentLineup.MaxMp;
	}
}