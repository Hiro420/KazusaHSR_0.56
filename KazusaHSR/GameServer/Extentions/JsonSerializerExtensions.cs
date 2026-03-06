using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer;

public static class JsonSerializerExtensions
{
	// Well, Newtonsoft's JsonSerializer doesn't have a built-in method to directly serialize an object to a string, so here we are.
	// This is basically a wrapper around the usual StringBuilder + StringWriter + JsonTextWriter
	public static string SerializeObject(this JsonSerializer serializer, object value, Formatting formatting = Formatting.None)
	{
		if (serializer == null) throw new ArgumentNullException(nameof(serializer));

		var sb = new StringBuilder(256);
		using var sw = new StringWriter(sb, CultureInfo.InvariantCulture);
		using var writer = new JsonTextWriter(sw) { Formatting = formatting };

		serializer.Serialize(writer, value);
		writer.Flush();
		return sb.ToString();
	}

	// We don't really need this, but just for convenience's sake
	public static string SerializeObjectIndented(this JsonSerializer serializer, object value) =>
		SerializeObject(serializer, value, Formatting.Indented);
}