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
		int DrainShields(IEnumerable<ZoneLocation> zoneLocations);
		int DrainShields(IEnumerable<ZoneLocation> zoneLocations, int amount);
		int DrainAllShields(int amount);
		int DrainAllShields();
		int DrainReactors(IEnumerable<ZoneLocation> zoneLocations);
		int DrainReactors(IEnumerable<ZoneLocation> zoneLocations, int amount);
		int DrainAllReactors(int amount);
		int DrainAllReactors();
		void AddDebuff(IEnumerable<ZoneLocation> zoneLocations, ZoneDebuff debuff, InternalThreat source);
		void RemoveDebuffForSource(IEnumerable<ZoneLocation> zoneLocations, InternalThreat source);
		ThreatDamageResult TakeAttack(ThreatDamage damage);
		int GetPlayerCount(StationLocation station);
		int GetPoisonedPlayerCount(IEnumerable<StationLocation> locations);
		void KnockOutPlayersWithBattleBots();
		void KnockOutPlayersWithBattleBots(IEnumerable<StationLocation> locations);
		void KnockOutPlayersWithoutBattleBots();
		void KnockOutPlayersWithoutBattleBots(IEnumerable<StationLocation> locations);
		void KnockOutPoisonedPlayers(IEnumerable<StationLocation> locations);
		void KnockOutPlayers(IEnumerable<StationLocation> locations);
		void TransferEnergyToShields(IEnumerable<ZoneLocation> zoneLocations);
		IList<ExternalThreat> CurrentExternalThreats { get; }
		void DisableInactiveBattlebots(IEnumerable<StationLocation> stationLocations);

		IDictionary<ExternalThreat, ExternalThreatBuff> CurrentThreatBuffsBySource { get; }
		IDictionary<StationLocation, Station> StationByLocation { get; }
		RocketsComponent RocketsComponent { get; }
		IList<InternalThreat> CurrentInternalThreats { get; }
	}
}
