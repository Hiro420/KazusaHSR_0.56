using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MainMissionRow
{
	public uint MainMissionId { get; set; }
	public string Type { get; set; }
	public List<uint> NextMainMissionList { get; set; }
	public TextID Name { get; set; }
	public string TakeType { get; set; }
	public List<uint> TakeParamIntList { get; set; }
	public string BeginType { get; set; }
	public TextID BeginDesc { get; set; }
	public List<object> BeginParamIntList { get; set; }
	public List<uint> StartSubMissionList { get; set; }
	public List<uint> FinishSubMissionList { get; set; }
	public uint TrackWeight { get; set; }
	public bool? IsShowStartHint { get; set; }
	public bool? IsShowFinishHint { get; set; }
	public uint RewardId { get; set; }
	public uint DisplayRewardId { get; set; }
	public uint? BeginParamInt1 { get; set; }
	public uint? NextTrackMainMission { get; set; }
}