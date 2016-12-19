using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PL
{
	public static class JavaScriptConvert
	{
		[SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times",
			Justification = "CA is registering a false positive. JsonWriter doesn't dispose of its own internal writer.")]
		public static IHtmlString SerializeObject(object value)
		{
			using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			using (var jsonWriter = new JsonTextWriter(stringWriter))
			{
				var serializer = new JsonSerializer
				{
					// Let's use camelCasing as is common practice in JavaScript
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				};

				// We don't want quotes around object names
				jsonWriter.QuoteName = false;
				serializer.Serialize(jsonWriter, value);

				return new HtmlString(stringWriter.ToString());
			}
		}
	}
}
