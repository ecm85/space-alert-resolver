using System.Text.Json.Serialization;
using BLL.Tracks;

namespace API.Models
{
    public class TrackSpaceModel
    {
        public int Space { get; set; }
        public bool HasAnyThreats { get; set; }
        public TrackBreakpointType? Breakpoint { get; set; }

        [JsonConstructor]
        public TrackSpaceModel()
        {
            
        }
    }
}