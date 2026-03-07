using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class VoiceConfigRow
{
	public uint VoiceID { get; set; }
	public string VoicePath { get; set; }
	public FFKLHHCMOOM VoiceType { get; set; }
	public TextID TextmapVoiceName { get; set; }
	public TextID TextmapVoiceTalk { get; set; }
	public string Character { get; set; }
}
