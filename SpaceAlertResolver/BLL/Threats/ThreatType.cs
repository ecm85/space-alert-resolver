using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace BLL.Threats
{
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ThreatType
    {
        SeriousInternal = 1,
        MinorInternal = 2,
        SeriousExternal = 3,
        MinorExternal = 4
    }
}
