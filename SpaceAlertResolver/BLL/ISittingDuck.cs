using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.Internal;

namespace BLL
{
	public interface ISittingDuck
	{
		ThreatDamageResult TakeAttack(ThreatDamage damage);

		void DrainShields(IEnumerable<ZoneLocation> zoneLocations);
		void DrainShields(IEnumerable<ZoneLocation> zoneLocations, int amount);
		void DrainReactors(IEnumerable<ZoneLocation> zoneLocations, int amount);
		int DrainShield(ZoneLocation zoneLocation);
		int DrainReactor(ZoneLocation zoneLocation);
		int DrainReactor(ZoneLocation zoneLocation, int amount);
		void DrainAllReactors(int amount);
		void DrainEnergyContainer(StationLocation stationLocation, int amount);

		void TransferEnergyToShields(IEnumerable<ZoneLocation> zoneLocations);

		void AddZoneDebuff(IEnumerable<ZoneLocation> zoneLocations, ZoneDebuff debuff, InternalThreat source);
		void RemoveZoneDebuffForSource(IEnumerable<ZoneLocation> zoneLocations, InternalThreat source);

		int GetPlayerCount(StationLocation station);
		int GetPoisonedPlayerCount(IEnumerable<StationLocation> locations);

		void KnockOutPlayersWithBattleBots(IEnumerable<StationLocation> locations);
		void KnockOutPlayersWithoutBattleBots(IEnumerable<StationLocation> locations);
		void KnockOutPoisonedPlayers(IEnumerable<StationLocation> locations);
		void KnockOutPlayers(IEnumerable<StationLocation> locations);
		void KnockOutPlayers(IEnumerable<ZoneLocation> locations);

		void DisableInactiveBattlebots(IEnumerable<StationLocation> stationLocations);

		event Action RocketsModified;
		int GetRocketCount();
		void RemoveRocket();
		void RemoveAllRockets();
		void ShiftPlayers(IEnumerable<ZoneLocation> zoneLocations, int turnToShift);
		void ShiftPlayers(IEnumerable<StationLocation> stationLocations, int turnToShift);

		void SubscribeToMoveIn(IEnumerable<StationLocation> stationLocations, Action<Player, int> handler);
		void SubscribeToMoveOut(IEnumerable<StationLocation> stationLocations, Action<Player, int> handler);
		void UnsubscribeFromMoveIn(IEnumerable<StationLocation> stationLocations, Action<Player, int> handler);
		void UnsubscribeFromMoveOut(IEnumerable<StationLocation> stationLocations, Action<Player, int> handler);

		void AddIrreparableMalfunctionToStations(IEnumerable<StationLocation> stationLocations, IrreparableMalfunction malfunction);
		bool DestroyFuelCapsule();
		int GetEnergyInReactor(ZoneLocation zoneLocation);
		void KnockOutCaptain();
		void InfectPlayers(StationLocation stationLocation);
		void BreachRedAirlock();
		void BreachBlueAirlock();
		void RepairAllAirlockBreaches();
		bool RedAirlockIsBreached { get; }
		bool BlueAirlockIsBreached { get; }
		int GetDamageToZone(ZoneLocation zoneLocation);
	}
}
