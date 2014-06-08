using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.External;
using BLL.Threats.Internal;

namespace BLL
{
	public interface ISittingDuck
	{
		ThreatDamageResult TakeAttack(ThreatDamage damage);

		int DrainShields(IEnumerable<ZoneLocation> zoneLocations);
		int DrainShields(IEnumerable<ZoneLocation> zoneLocations, int amount);
		int DrainAllShields(int amount);
		int DrainAllShields();
		int DrainReactors(IEnumerable<ZoneLocation> zoneLocations);
		int DrainReactors(IEnumerable<ZoneLocation> zoneLocations, int amount);
		int DrainAllReactors(int amount);
		int DrainAllReactors();

		void TransferEnergyToShields(IEnumerable<ZoneLocation> zoneLocations);
		int GetEnergyInStation(StationLocation currentStation);

		void AddZoneDebuff(IEnumerable<ZoneLocation> zoneLocations, ZoneDebuff debuff, InternalThreat source);
		void RemoveZoneDebuffForSource(IEnumerable<ZoneLocation> zoneLocations, InternalThreat source);

		IEnumerable<ExternalThreatBuff> CurrentExternalThreatBuffs();
		void AddExternalThreatBuff(ExternalThreatBuff buff, ExternalThreat source);
		void RemoveExternalThreatBuffForSource(ExternalThreat source);

		int GetPlayerCount(StationLocation station);
		int GetPoisonedPlayerCount(IEnumerable<StationLocation> locations);

		void KnockOutPlayersWithBattleBots();
		void KnockOutPlayersWithBattleBots(IEnumerable<StationLocation> locations);
		void KnockOutPlayersWithoutBattleBots();
		void KnockOutPlayersWithoutBattleBots(IEnumerable<StationLocation> locations);
		void KnockOutPoisonedPlayers(IEnumerable<StationLocation> locations);
		void KnockOutPlayers(IEnumerable<StationLocation> locations);

		IList<ExternalThreat> CurrentExternalThreats { get; }
		IList<InternalThreat> CurrentInternalThreats { get; }

		void AddInternalThreatToStations(IEnumerable<StationLocation> stationLocations, InternalThreat threat);
		void RemoveInternalThreatFromStations(IEnumerable<StationLocation> stationLocations, InternalThreat threat);
		IEnumerable<InternalThreat> GetThreatsInStation(StationLocation stationLocation);

		void DisableInactiveBattlebots(IEnumerable<StationLocation> stationLocations);

		event EventHandler RocketsModified;
		int GetRocketCount();
		bool RemoveRocket();
		int RemoveAllRockets();
	}
}
