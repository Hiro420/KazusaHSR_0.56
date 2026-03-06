using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource;

public class DynamicFloat
{
	public FixedDynamicFloat FixedValue { get; set; } = new();

	public class FixedDynamicFloat
	{
		public bool IsDynamic { get; set; } = false;
		public double FixedValue { get; set; } = 0f;
	}

	// todo ig?
	public double Evaluate()
	{
		if (!FixedValue.IsDynamic)
			return FixedValue.FixedValue;

		// For now, just return a placeholder value
		return 0f;
	}
}
