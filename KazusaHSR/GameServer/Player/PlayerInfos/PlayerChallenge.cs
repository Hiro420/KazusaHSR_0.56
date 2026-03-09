namespace KazusaHSR.GameServer.PlayerInfos;

public class PlayerChallenge
{
	public uint ChallengeId { get; set; }
	public uint Stars { get; set; }
}

public class PlayerChallengeVirtualLineup
{
	public uint PlaneId { get; set; }
	public uint CurMp { get; set; } = 5;
	public uint MaxMp { get; set; } = 5;
	public uint LeaderAvatarId { get; set; }
	public List<uint> AvatarIds { get; set; } = new(new uint[4]);
}
