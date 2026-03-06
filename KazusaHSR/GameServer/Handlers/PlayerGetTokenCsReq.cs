using KazusaHSR.Protocol;
using KazusaHSR.GameServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandlePlayerGetTokenCsReq
{
	[Packet.PacketCmdId(PacketId.PlayerGetTokenCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		PlayerGetTokenCsReq req = packet.GetDecodedBody<PlayerGetTokenCsReq>();
		// Token/UID MUST BE CORRECT!!!!
		var db = MainApp.databaseManager;
		var account = db.GetByTokenAsync(req.Token).GetAwaiter().GetResult();
		if (account == null)
		{
			PlayerGetTokenScRsp errorRsp = new PlayerGetTokenScRsp()
			{
				Retcode = (int)Retcode.RetAccountVerifyError,
				Msg = "ACCOUNT_VERIFY_ERROR",
				Uid = 0
			};
			session.SendPacket(errorRsp);
			return;
		}

		// Now we can bind the account to session
		session.BindAccount(account.AccountId, account.Token, account.Uid);

		PlayerGetTokenScRsp rsp = new PlayerGetTokenScRsp()
		{
			Retcode = 0,
			Msg = "OK",
			Uid = account.Uid
		};
		session.SendPacket(rsp);
	}
}