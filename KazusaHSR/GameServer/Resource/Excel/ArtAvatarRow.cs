using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ArtAvatarRow
{
	public string AvatarName { get; set; }
	public GPFOIOKFAJN Body { get; set; }
	public string Brow { get; set; }
	public string Eye { get; set; }
	public string Mouth { get; set; }
	public bool EnableAutoBlink { get; set; }
	public float MinBlinkGap { get; set; }
	public float MaxBlinkGap { get; set; }
	public float BlinkingDuration { get; set; }
	public float DoubleBlinkProbability { get; set; }
	public string EyeCloseClip { get; set; }
	public string BrowPermanentClip { get; set; }
	public string EyePermanentClip { get; set; }
	public string MouthPermanentClip { get; set; }
}
