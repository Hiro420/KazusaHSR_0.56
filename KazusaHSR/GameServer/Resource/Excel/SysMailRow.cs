using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class SysMailRow
{
	public uint MailID { get; set; }
	public string MailTitle { get; set; }
	public string MailSender { get; set; }
	public string MailDetail { get; set; }
	public uint MailLifeTime { get; set; }
}
