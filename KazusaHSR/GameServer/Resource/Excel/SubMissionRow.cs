using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class SubMissionRow
{
	public uint SubMissionID { get; set; }
	public uint NextSubMissionID { get; set; }
	public uint[] NextSubMissionList { get; set; }
	public uint MainMissionID { get; set; }
	public TextID TargetText { get; set; }
	public TextID DescrptionText { get; set; }
	public uint MazePlaneID { get; set; }
	public uint MazeFloorID { get; set; }
	public uint[] LoadGroupList { get; set; }
	public uint[] UnLoadGroupList { get; set; }
	public uint[] ClearGroupList { get; set; }
	public DIFGOEEHPHC[] MapNPCList { get; set; }
	public PNIGKBPEHJO[] MapPropList { get; set; }
	public uint FinishWayID { get; set; }
	public string MissionJsonPath { get; set; }
	public AEGBKHMFGPO TakeType { get; set; }
	public uint TakeParamInt1 { get; set; }
	public uint[] TakeParamIntList { get; set; }
	public AEGBKHMFGPO BeginType { get; set; }
	public uint BeginParamInt1 { get; set; }
	public uint[] BeginParamIntList { get; set; }
	public bool IsShow { get; set; }
	public uint ProgressGroup { get; set; }
	public BeginHintType IsShowStartHint { get; set; }
	public MissionWayPointType WayPointType { get; set; }
	public string WayPointUIPrefabPath { get; set; }
	public uint WayPointFloorID { get; set; }
	public uint WayPointGroupID { get; set; }
	public uint WayPointEntityID { get; set; }
	public uint WayPointShowRangeMin { get; set; }
	public uint MapWaypointIconType { get; set; }
	public float MapWaypointRange { get; set; }
	public string MapWayPointCircleColor { get; set; }
	public OPFENPAPKPO FinishActorType { get; set; }
	public bool FroceMapHint { get; set; }
}
