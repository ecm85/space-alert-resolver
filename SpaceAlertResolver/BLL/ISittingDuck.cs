using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.Internal;

namespace BLL
{
	public interface ISittingDuck
	{
		void DrainShields(IEnumerable<ZoneLocation> zoneLocations);
		void DrainShields(IEnumerable<ZoneLocation> zoneLocations, int amount);
		IDictionary<ExternalThreat, ExternalThreatBuff> CurrentThreatBuffs { get; }
		IList<ExternalThreat> CurrentExternalThreats { get; }
		ThreatDamageResult TakeAttack(ThreatDamage damage);
		int GetPlayerCount(StationLocation station);
		void KnockOutPlayersWithBattleBots();
		void KnockOutPlayers(IEnumerable<StationLocation> locations);
		void TransferEnergyToShields(IEnumerable<ZoneLocation> zoneLocations);
		void EnergyLeaksOut(IEnumerable<ZoneLocation> zoneLocations);

		Zone BlueZone { get; }
		Zone WhiteZone { get; }
		Zone RedZone { get; }
		IDictionary<ZoneLocation, Zone> ZonesByLocation { get; }
		IEnumerable<Zone> Zones { get; }
		IDictionary<StationLocation, Station> StationByLocation { get; }
		InterceptorStation InterceptorStation { get; set; }
		ComputerComponent Computer { get; }
		RocketsComponent RocketsComponent { get; }
		VisualConfirmationComponent VisualConfirmationComponent { get; }
		IList<InternalThreat> CurrentInternalThreats { get; }
	}
}
