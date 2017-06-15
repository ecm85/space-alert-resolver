using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Threats.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
    public class InternalThreatModel : ThreatModel
    {
        public int TotalInaccessibility { get; set; }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<StationLocation> DisplayOnTrackStations { get; set; }
        public int Position { get; set; }

        public InternalThreatModel(InternalThreat threat) : base(threat.Parent ?? threat)
        {
            TotalInaccessibility = threat.TotalInaccessibility.GetValueOrDefault();
            DisplayOnTrackStations = threat.DisplayOnTrackStations.ToList();
            Position = threat.Position;
        }

        [JsonConstructor]
        public InternalThreatModel()
        {
        }
    }
}
