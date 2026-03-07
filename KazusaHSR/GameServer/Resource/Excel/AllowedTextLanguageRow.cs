using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AllowedTextLanguageRow
{
	public string TextLanguageKey { get; set; }
	public string SDKkey { get; set; }
	public uint LanguageType { get; set; }
	public TextID ShowString { get; set; }
	public string Font { get; set; }
	public string LogoImgPath { get; set; }
}
