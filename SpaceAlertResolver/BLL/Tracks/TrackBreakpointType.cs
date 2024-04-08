using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace BLL.Tracks
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TrackBreakpointType
    {
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X")]
        X,
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y")]
        Y,
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Z")]
        Z
    }
}
