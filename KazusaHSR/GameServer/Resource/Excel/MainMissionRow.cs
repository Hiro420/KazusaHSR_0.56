using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MainMissionRow
{
	public uint MainMissionID { get; set; }
	public MainMissionType Type { get; set; }
	public bool IsLoop { get; set; }
	public uint[] NextMainMissionList { get; set; }
	public TextID Name { get; set; }
	public AEGBKHMFGPO TakeType { get; set; }
	public uint TakeParamInt1 { get; set; }
	public uint[] TakeParamIntList { get; set; }
	public AEGBKHMFGPO BeginType { get; set; }
	public uint BeginParamInt1 { get; set; }
	public TextID BeginDesc { get; set; }
	public uint[] BeginParamIntList { get; set; }
	public uint[] StartSubMissionList { get; set; }
	public uint[] FinishSubMissionList { get; set; }
	public uint NextTrackMainMission { get; set; }
	public uint TrackWeight { get; set; }
	public bool IsShowStartHint { get; set; }
	public bool IsShowFinishHint { get; set; }
	public uint RewardID { get; set; }
	public uint DisplayRewardID { get; set; }
}
