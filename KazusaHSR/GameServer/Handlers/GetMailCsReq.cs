using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetMailCsReq
{
	[Packet.PacketCmdId(PacketId.GetMailCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		var req = packet.GetDecodedBody<GetMailCsReq>();

		uint now = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

		var welcomeMail = new ClientMail
		{
			Id = 1,
			Title = "Welcome to KazusaHSR!",
			Content = "Hope you have fun, please don't forget to give the repo a star.",
			Sender = "Kazusa",
			Time = now - 1000,
			ExpireTime = now + 604800,
			IsRead = true,
			Attachment = new MailAttachment()
		};

		uint total = 1;
		var rsp = new GetMailScRsp();

		// self:get_mail(localCount, endIndex)
		uint start = 0;
		uint stop = req.Stop;

		if (welcomeMail.Id >= start && welcomeMail.Id <= stop)
			rsp.MailLists.Add(welcomeMail);

		rsp.TotalNum = total;
		rsp.IsEnd = (stop >= total - 1);

		session.SendPacket(rsp);
	}

}