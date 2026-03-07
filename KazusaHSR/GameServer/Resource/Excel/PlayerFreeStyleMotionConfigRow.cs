using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class PlayerFreeStyleMotionConfigRow
{
	public uint ID { get; set; }
	public string StoryCharacterID { get; set; }
	public string StartMotion { get; set; }
	public string StartMotionPath { get; set; }
	public string LoopMotionPath { get; set; }
	public string StartMotionRibbonPath { get; set; }
	public string LoopMotionRibbonPath { get; set; }
	public float ExitTime { get; set; }
	public float TransitionDuration { get; set; }
	public float TransitionOffset { get; set; }
}
