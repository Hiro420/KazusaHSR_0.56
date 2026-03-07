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
	public ItemRow ItemConfig { get; set; }
	public uint Guid { get; set; }
	public uint ItemId { get; set; }
	public uint Count { get; set; }

	public PlayerItem(Session session, uint materialId, uint? tid = null)
	{
		this.Session = session;
		//this.ItemConfig = MainApp.resourceManager.ItemConfig.First(i => i.ID == materialId);
		this.ItemId = materialId;
		this.Count = 1;
		this.Guid = tid ?? session.player.GetNextItemGuid();

		Initialize();
	}

	public virtual void Initialize()
	{
		ItemRow? itemRow = MainApp.resourceManager.ItemConfig.FirstOrDefault(i => i.ID == ItemId);
		if (itemRow == null)
		{
			itemRow = MainApp.resourceManager.ItemConfigAvatar.FirstOrDefault(i => i.ID == ItemId);
		}
		if (itemRow == null)
		{
			itemRow = MainApp.resourceManager.ItemConfigEquipment.First(i => i.ID == ItemId);
		}
		this.ItemConfig = itemRow;
	}

	public virtual void AllToRsp(GetBagScRsp rsp)
	{
		Material info = new Material()
		{
			Num = this.Count,
			Tid = this.ItemId
		};
		rsp.MaterialLists.Add(info);
	}
}
