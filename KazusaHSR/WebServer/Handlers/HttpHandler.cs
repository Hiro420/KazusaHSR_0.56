using KazusaHSR.GameServer;
using KazusaHSR.Protocol;
using KazusaHSR.Utils;
using KazusaHSR.WebServer.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;

namespace KazusaHSR.WebServer;

public class HttpHandler
{
    Config config = MainApp.config;

	[HttpEndpoint("/account/risky/api/check", "POST")]
	public HttpResponse AccountRiskyCheck(HttpListenerRequest request)
	{
		JObject rspObj = new()
		{
			["retcode"] = 0,
			["message"] = "OK",
			["data"] = new JObject()
			{
				["id"] = "06611ed14c3131a676b19c0d34c0644b",
				["action"] = "ACTION_NONE"
			}
		};
		return new JsonResponse(rspObj.ToString());
	}

	[HttpEndpoint("/hkrpg_global/mdk/shield/api/verify", "POST")]
	public HttpResponse MdkShieldVerify(HttpListenerRequest request)
	{
		// request example: {"uid":"69420","token":"test_token"}
		using var reader = new StreamReader(request.InputStream);
		string body = reader.ReadToEnd();
		JObject reqObj = JObject.Parse(body);
		string uidStr = reqObj.Value<string>("uid") ?? "0";
		string token = reqObj.Value<string>("token") ?? string.Empty;
		uint uid = uint.TryParse(uidStr, out var v) ? v : 0u;

		var db = MainApp.databaseManager;
		var account = db.GetByTokenAsync(token).GetAwaiter().GetResult();
		bool ok = account != null && account.Uid == uid;

		JObject rspObj = new()
		{
			["retcode"] = ok ? 0 : (int)Retcode.RetAccountVerifyError,
			["message"] = ok ? "OK" : "ACCOUNT_VERIFY_ERROR",
			["data"] = new JObject()
		};
		if (ok && account != null) // ik the account is not null here, but compiler doesn't for some weird reason
		{
			rspObj["data"] = new JObject()
			{
				["account"] = new JObject()
				{
					["name"] = account.AccountId,
					["token"] = token,
					["uid"] = account.Uid.ToString(CultureInfo.InvariantCulture)
				},
			};
		}
		return new JsonResponse(rspObj.ToString());
	}

	[HttpEndpoint("/hkrpg_global/mdk/shield/api/login", "POST")]
	public HttpResponse MdkShieldLogin(HttpListenerRequest request)
	{
		// request example: {"account":"Kazusa","password":"...","is_crypto":true}
		// For now we ignore password and treat "account" as unique account id.
		using var reader = new StreamReader(request.InputStream);
		string body = reader.ReadToEnd();
		JObject reqObj = JObject.Parse(body);
		string accountName = reqObj.Value<string>("account") ?? "Kazusa";

		var db = MainApp.databaseManager;
		var account = db.GetOrCreateAccountAsync(accountName).GetAwaiter().GetResult();
		string token = Guid.NewGuid().ToString("N");
		account.Token = token;
		db.UpdateAccountAsync(account).GetAwaiter().GetResult();

		JObject rspObj = new()
		{
			["retcode"] = 0,
			["message"] = "OK",
			["data"] = new JObject()
			{
				["account"] = new JObject()
				{
					["name"] = account.AccountId,
					["token"] = token,
					["uid"] = account.Uid.ToString(CultureInfo.InvariantCulture)
				},
			},
		};
		return new JsonResponse(rspObj.ToString());
	}

	[HttpEndpoint("/hkrpg_global/combo/granter/login/v2/login", "POST")]
	public HttpResponse ComboGranterLogin(HttpListenerRequest request)
	{
		// example request body: {"data":"{\"uid\":\"69420\",\"guest\":false,\"token\":\"...\"}",...}
		using var reader = new StreamReader(request.InputStream);
		string body = reader.ReadToEnd();
		JObject root = JObject.Parse(body);
		string inner = root.Value<string>("data") ?? "{}";
		JObject dataObj = JObject.Parse(inner);
		string uidStr = dataObj.Value<string>("uid") ?? "0";
		string token = dataObj.Value<string>("token") ?? string.Empty;
		uint uid = uint.TryParse(uidStr, out var v) ? v : 0u;

		var db = MainApp.databaseManager;
		var account = db.GetByTokenAsync(token).GetAwaiter().GetResult();
		bool ok = account != null && account.Uid == uid;
		if (!ok)
		{
			JObject error = new()
			{
				["retcode"] = (int)Retcode.RetAccountVerifyError,
				["message"] = "ACCOUNT_VERIFY_ERROR",
				["data"] = new JObject()
			};
			return new JsonResponse(error.ToString());
		}

		JObject rspObj = new()
		{
			["retcode"] = 0,
			["message"] = "OK",
			["data"] = new JObject()
			{
				["combo_id"] = 1,
				["open_id"] = account.Uid,
				["combo_token"] = token,
				["account_type"] = 1, // 0 = Guest, 1 = Normal
				["heartbeat"] = false,
				["data"] = new JObject()
				{
					["guest"] = false,
				}
			}
		};
		return new JsonResponse(rspObj.ToString());
	}

	[HttpEndpoint("/query_gateway", "GET")]
	public HttpResponse QueryGateway(HttpListenerRequest request)
	{
		ServerDispatchResponse rspObj = new()
		{
			Retcode = 0,
			Message = "OK",
			AsbRelogin = 1,
			RegionName = "KazusaHSR",
			Gateway = new GatewayInfo
			{
				Ip = "127.0.0.1",
				Port = 6969
			},
			Ext = new ExtInfo
			{
				DataUseAssetBundle = "0",
				ResUseAssetBundle = "0",
				ForbidRecharge = "0",
				UpdateStreamingAsb = "0"
			}
		};
		return new JsonResponse(JsonConvert.SerializeObject(rspObj, settings));
	}


	[HttpEndpoint("/query_dispatch", "GET")]
	public HttpResponse QueryDispatch(HttpListenerRequest request)
	{
		GlobalDispatchResponse rspObj = new()
		{
			Retcode = 0,
			TopServerRegionName = "KazusaHSR",
			RegionList = new List<GlobalDispatchServerData>
			{
				new GlobalDispatchServerData
				{
					EnvType = "2",
					DispatchUrl = $"http://{config.WebServer.ServerIP}:{config.WebServer.ServerPort}/query_gateway",
					Name = "KazusaHSR",
					Title = "KazusaHSR",
					DisplayName = "KazusaHSR"
				}
			}
		};

		return new JsonResponse(JsonConvert.SerializeObject(rspObj, settings));
	}

	JsonSerializerSettings settings = new()
	{
		NullValueHandling = NullValueHandling.Ignore,
		DefaultValueHandling = DefaultValueHandling.Ignore
	};
}