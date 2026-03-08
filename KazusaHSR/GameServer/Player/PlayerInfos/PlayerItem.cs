using KazusaHSR.Protocol;
using KazusaHSR.Resource;
using KazusaHSR;
using KazusaHSR.GameServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KazusaHSR.GameServer.Resource.Excel;

namespace KazusaHSR.GameServer.PlayerInfos;

public class PlayerItem
{
	public Session Session { get; set; } // just in case ill need it
	public ItemRow? ItemConfig => MainApp.resourceManager.GetItemRowById(this.ItemId);
	public uint Guid { get; set; }
	public uint ItemId { get; set; }
	public uint Count { get; set; }
	public uint Level { get; set; }

	public PlayerItem(Session session, uint materialId, uint? tid = null)
	{
		this.ItemId = materialId;
		this.Session = session;
		//this.ItemConfig = MainApp.resourceManager.ItemConfig.First(i => i.ID == materialId);
		this.Count = 1;
		this.Guid = tid ?? session.player.GetNextItemGuid();
		this.Level = 1;
	}

	public virtual void AllToRsp(GetBagScRsp rsp)
	{
		rsp.MaterialLists.Add(ToMaterialProto());
	}

	public virtual void AddToSync(PlayerSyncScNotify ntf)
	{
		ntf.MaterialLists.Add(ToMaterialProto());
	}

	public virtual Material ToMaterialProto()
	{
		return new Material()
		{
			Tid = this.ItemId,
			Num = this.Count
		};
	}
}
