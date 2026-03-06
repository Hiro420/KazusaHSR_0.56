using KazusaHSR.Protocol;
using KazusaHSR.Resource;
using KazusaHSR;
using KazusaHSR.GameServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.PlayerInfos;

public class PlayerItem
{
	private Session Session { get; set; } // just in case ill need it
	//private MaterialExcelConfig MaterialExcel { get; set; }
	public ulong Guid { get; set; }
	public uint ItemId { get; set; }
	public uint Count { get; set; }

	public PlayerItem(Session session, uint materialId)
	{
		this.Session = session;
		//this.MaterialExcel = MainApp.resourceManager.MaterialExcel[materialId];
		this.Guid = session.GetGuid();
		this.ItemId = materialId;
		this.Count = 1;
	}

	public Material ToMaterialInfo()
	{
		Material info = new Material()
		{
			Num = this.Count,
			Tid = this.ItemId
		};
		return info;
	}
}
