using System.Text.Json.Serialization;
using BLL.ShipComponents;
using BLL.Threats.Internal;

namespace API.Models
{
    public class InternalThreatModel : ThreatModel
    {
        public int TotalInaccessibility { get; set; }
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
