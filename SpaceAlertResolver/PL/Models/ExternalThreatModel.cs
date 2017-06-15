using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
    public class ExternalThreatModel : ThreatModel
    {
        public int Shields { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ZoneLocation CurrentZone { get; set; }
        public int Position { get; set; }

        public ExternalThreatModel(ExternalThreat threat) : base(threat)
        {
            Shields = threat.Shields + (threat.GetThreatStatus(ThreatStatus.BonusShield) ? 1 : 0);
            CurrentZone = threat.CurrentZone;
            Position = threat.Position;
        }

        [JsonConstructor]
        public ExternalThreatModel()
        {
        }
    }
}
