using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;

namespace KazusaHSR.WebServer.Handlers;

#nullable enable

public sealed class GlobalDispatchResponse
{
	[JsonProperty("region_list")]
	public List<GlobalDispatchServerData>? RegionList { get; set; }

	[JsonProperty("top_server_region_name")]
	public string? TopServerRegionName { get; set; }

	[JsonProperty("retcode")]
	public int? Retcode { get; set; }
}

public sealed class GlobalDispatchServerData
{
	[JsonProperty("dispatch_url")]
	public string? DispatchUrl { get; set; }

	[JsonProperty("name")]
	public string? Name { get; set; }

	[JsonProperty("title")]
	public string? Title { get; set; }

	[JsonProperty("display_name")]
	public string? DisplayName { get; set; }

	[JsonProperty("env_type")]
	public string? EnvType { get; set; }
}

public sealed class ServerDispatchResponse
{
	[JsonProperty("region_name")]
	public string? RegionName { get; set; }

	[JsonProperty("gateway")]
	public GatewayInfo? Gateway { get; set; }

	[JsonProperty("retcode")]
	public int? Retcode { get; set; }

	[JsonProperty("stop_begin_time")]
	public int? StopBeginTime { get; set; }

	[JsonProperty("stop_end_time")]
	public int? StopEndTime { get; set; }

	[JsonProperty("msg")]
	public string? Message { get; set; }

	[JsonProperty("design_data_relogin")]
	public int? DesignDataRelogin { get; set; }

	[JsonProperty("design_data_memo")]
	public string? DesignDataMemo { get; set; }

	[JsonProperty("asb_relogin")]
	public int? AsbRelogin { get; set; }

	[JsonProperty("asb_memo")]
	public string? AsbMemo { get; set; }

	[JsonProperty("ext")]
	public ExtInfo? Ext { get; set; }

	[JsonProperty("ex_resource_url")]
	public string? ExResourceUrl { get; set; }

	[JsonProperty("asset_bundle_url")]
	public string? AssetBundleUrl { get; set; }

	[JsonProperty("lua_url")]
	public string? LuaUrl { get; set; }
}

public sealed class GatewayInfo
{
	[JsonProperty("ip")]
	public string? Ip { get; set; }

	[JsonProperty("port")]
	public int? Port { get; set; }
}

public sealed class ExtInfo
{
	[JsonProperty("data_use_asset_bundle")]
	public string? DataUseAssetBundle { get; set; }

	[JsonProperty("forbid_recharge")]
	public string? ForbidRecharge { get; set; }

	[JsonProperty("res_use_asset_bundle")]
	public string? ResUseAssetBundle { get; set; }

	[JsonProperty("update_streaming_asb")]
	public string? UpdateStreamingAsb { get; set; }
}

public sealed class ServerStopInfo
{
	public int? StopBeginTime { get; set; }
	public int? StopEndTime { get; set; }
	public string? Message { get; set; }
}
