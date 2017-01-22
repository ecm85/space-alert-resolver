using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Threats.Internal;

namespace BLL
{
	public class SittingDuck : ISittingDuck
	{
		public BlueZone BlueZone { get; }
		public WhiteZone WhiteZone { get; }
		public RedZone RedZone { get; }
		public IDictionary<ZoneLocation, Zone> ZonesByLocation { get; }
		public IDictionary<StationLocation, Station> StationsByLocation { get; }
		public IEnumerable<Zone> Zones => ZonesByLocation.Values;
		public IDictionary<StationLocation, StandardStation> StandardStationsByLocation { get; }
		public IList<InterceptorStation> InterceptorStations { get; }
		public event EventHandler<RocketsRemovedEventArgs> RocketsModified = (sender, args) => { };
		public event EventHandler CentralLaserCannonFired = (sender, args) => { };
		public ThreatController ThreatController { get; private set; }
		public Game Game { get; private set; }

		private Doors BlueDoors { get; }
		private Doors RedDoors { get; }

		public SittingDuck(ThreatController threatController, Game game)
		{
			ThreatController = threatController;
			Game = game;

			var redDoors = new Doors();
			var blueDoors = new Doors();


			var interceptors = new Interceptors();
			var interceptorComponent1 = new InterceptorsInSpaceComponent(this, StationLocation.Interceptor1);
			var interceptorComponent2 = new InterceptorsInSpaceComponent(this, StationLocation.Interceptor2);
			var interceptorComponent3 = new InterceptorsInSpaceComponent(this, StationLocation.Interceptor3);

			var interceptorStation1 = new InterceptorStation(
				StationLocation.Interceptor1,
				threatController,
				interceptorComponent1);
			var interceptorStation2 = new InterceptorStation(
				StationLocation.Interceptor2,
				threatController,
				interceptorComponent2);
			var interceptorStation3 = new InterceptorStation(
				StationLocation.Interceptor3,
				threatController,
				interceptorComponent3);

			BlueDoors = blueDoors;
			RedDoors = redDoors;
			WhiteZone = new WhiteZone(threatController, redDoors, blueDoors, this);
			RedZone = new RedZone(threatController, WhiteZone.LowerWhiteStation.CentralReactor, redDoors, this, interceptors);
			BlueZone = new BlueZone(threatController, WhiteZone.LowerWhiteStation.CentralReactor, blueDoors, this);

			BlueZone.LowerBlueStation.RocketsComponent.RocketsModified += (sender, args) => RocketsModified(sender, args);
			WhiteZone.UpperWhiteStation.AlphaComponent.CannonFired += (sender, args) => CentralLaserCannonFired(this, EventArgs.Empty);

			ZonesByLocation = new Zone[] {RedZone, WhiteZone, BlueZone}.ToDictionary(zone => zone.ZoneLocation);
			InterceptorStations = new [] {interceptorStation1, interceptorStation2, interceptorStation3};
			StationsByLocation = Zones
				.SelectMany(zone => new Station[] {zone.LowerStation, zone.UpperStation})
				.Concat(InterceptorStations)
				.ToDictionary(station => station.StationLocation);
			StandardStationsByLocation = Zones
				.SelectMany(zone => new StandardStation[] {zone.LowerStation, zone.UpperStation})
				.ToDictionary(station => station.StationLocation);
		}

		public void SetPlayers(IEnumerable<Player> players)
		{
			foreach (var player in players)
			{
				player.CurrentStation = WhiteZone.UpperStation;
				WhiteZone.UpperStation.Players.Add(player);
			}
		}

		public void DrainShields(IEnumerable<ZoneLocation> zoneLocations)
		{
			foreach (var zone in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
				zone.DrainShield();
		}

		public void DrainShields(IEnumerable<ZoneLocation> zoneLocations, int amount)
		{
			foreach (var zone in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
				zone.DrainShield(amount);
		}

		public int DrainReactor(ZoneLocation zoneLocation)
		{
			return ZonesByLocation[zoneLocation].DrainReactor();
		}

		public void DrainReactors(IEnumerable<ZoneLocation> zoneLocations, int amount)
		{
			foreach (var zone in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
				zone.DrainReactor(amount);
		}

		public int DrainShield(ZoneLocation zoneLocation)
		{
			return ZonesByLocation[zoneLocation].DrainShield();
		}

		public int DrainReactor(ZoneLocation zoneLocation, int amount)
		{
			return ZonesByLocation[zoneLocation].DrainReactor(amount);
		}

		public void DrainAllReactors(int amount)
		{
			DrainReactors(EnumFactory.All<ZoneLocation>(), amount);
		}

		public void DrainEnergy(StationLocation stationLocation, int amount)
		{
			StandardStationsByLocation[stationLocation].DrainEnergy(amount);
		}

		public ThreatDamageResult TakeAttack(ThreatDamage damage)
		{
			var shipDestroyed = false;
			var damageShielded = 0;
			foreach (var zone in damage.ZoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
			{
				ThreatDamageResult damageResult;
				switch (damage.ThreatDamageType)
				{
					case ThreatDamageType.ReducedByTwoAgainstInterceptors:
						var amount = damage.Amount;
						if (damage.DistanceToSource != null)
						{
							var stationsBetweenThreatAndShip = InterceptorStations
								.Where(station => station.StationLocation.DistanceFromShip() <= damage.DistanceToSource);
							if (stationsBetweenThreatAndShip.Any(station => station.Players.Any()))
								amount -= 2;
						}
						damageResult = zone.TakeAttack(amount, ThreatDamageType.Standard);
						shipDestroyed = shipDestroyed || damageResult.ShipDestroyed;
						damageShielded += damageResult.DamageShielded;
						break;
					default:
						damageResult = zone.TakeAttack(damage.Amount, damage.ThreatDamageType);
						shipDestroyed = shipDestroyed || damageResult.ShipDestroyed;
						damageShielded += damageResult.DamageShielded;
						break;
				}
			}
			return new ThreatDamageResult{DamageShielded = damageShielded, ShipDestroyed = shipDestroyed};
		}

		public virtual int GetPlayerCount(StationLocation station)
		{
			return StationsByLocation[station].Players.Count;
		}

		public IEnumerable<Player> GetPlayersInStation(StationLocation station)
		{
			return StationsByLocation[station].Players;
		}

		public IEnumerable<Player> PlayersOnShip => EnumFactory.All<StationLocation>().SelectMany(stationLocation => StandardStationsByLocation[stationLocation].Players);

		public void KnockOutPlayersWithBattleBots(IEnumerable<StationLocation> locations)
		{
			var playersWithBattleBots = locations
				.Select(location => StationsByLocation[location])
				.SelectMany(zone => zone.Players)
				.Where(player => player.BattleBots != null);
			KnockOut(playersWithBattleBots);
		}

		public void KnockOutPlayersWithoutBattleBots(IEnumerable<StationLocation> locations)
		{
			var playersWithBattleBots = locations
				.Select(location => StationsByLocation[location])
				.SelectMany(zone => zone.Players)
				.Where(player => player.BattleBots == null);
			KnockOut(playersWithBattleBots);
		}

		public void KnockOutPlayers(IEnumerable<StationLocation> locations)
		{
			var players = locations.SelectMany(location => StationsByLocation[location].Players);
			KnockOut(players);
		}

		public void KnockOutPlayers(IEnumerable<ZoneLocation> locations)
		{
			var players = locations.SelectMany(location => ZonesByLocation[location].Players);
			KnockOut(players);
		}

		public void TransferEnergyToShields(IEnumerable<ZoneLocation> zoneLocations)
		{
			foreach (var zone in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
				zone.UpperStation.FillToCapacity();
		}

		private static void KnockOut(IEnumerable<Player> players)
		{
			foreach (var player in players)
				player.IsKnockedOut = true;
		}

		public void AddZoneDebuff(IEnumerable<ZoneLocation> zoneLocations, ZoneDebuff debuff, InternalThreat source)
		{
			foreach (var zone in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
				zone.AddDebuff(debuff, source);
		}

		public void RemoveZoneDebuffForSource(IEnumerable<ZoneLocation> zoneLocations, InternalThreat source)
		{
			foreach (var zone in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
				zone.RemoveDebuffForSource(source);
		}

		public void DisableLowerRedInactiveBattleBots()
		{
			RedZone.LowerRedStation.BattleBotsComponent.DisableInactiveBattleBots();
		}

		public int RocketCount => BlueZone.LowerBlueStation.RocketsComponent.RocketCount;

		public void RemoveRocket()
		{
			BlueZone.LowerBlueStation.RocketsComponent.RemoveRocket();
		}

		public int RemoveAllRockets()
		{
			return BlueZone.LowerBlueStation.RocketsComponent.RemoveAllRockets();
		}

		public void ShiftPlayers(IEnumerable<ZoneLocation> zoneLocations, int turnToShift)
		{
			foreach (var player in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]).SelectMany(zone => zone.Players))
				player.Shift(turnToShift);
		}

		public void ShiftPlayers(IEnumerable<StationLocation> stationLocations, int turnToShift)
		{
			foreach (var player in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]).SelectMany(station => station.Players))
				player.Shift(turnToShift);
		}

		public void ShiftPlayersAndRepeatPreviousAction(IEnumerable<StationLocation> stationLocations, int turnToShift)
		{
			foreach (var player in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]).SelectMany(station => station.Players))
				player.ShiftAndRepeatPreviousAction(turnToShift);
		}

		public void SubscribeToMovingIn(IEnumerable<StationLocation> stationLocations, EventHandler<PlayerMoveEventArgs> handler)
		{
			foreach (var station in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]))
				station.MovingIn += handler;
		}

		public void SubscribeToMovingOut(IEnumerable<StationLocation> stationLocations, EventHandler<PlayerMoveEventArgs> handler)
		{
			foreach (var station in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]))
				station.MovingOut += handler;
		}

		public void UnsubscribeFromMovingIn(IEnumerable<StationLocation> stationLocations, EventHandler<PlayerMoveEventArgs> handler)
		{
			foreach (var station in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]))
				station.MovingIn -= handler;
		}

		public void UnsubscribeFromMovingOut(IEnumerable<StationLocation> stationLocations, EventHandler<PlayerMoveEventArgs> handler)
		{
			foreach (var station in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]))
				station.MovingOut -= handler;
		}

		public void AddIrreparableMalfunctionToStations(IEnumerable<StationLocation> stationLocations, IrreparableMalfunction malfunction)
		{
			foreach (var station in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]))
				station.IrreparableMalfunctions.Add(malfunction);
		}

		public bool DestroyFuelCapsule()
		{
			var centralReactor = WhiteZone.LowerWhiteStation.CentralReactor;
			var oldFuelCapsules = centralReactor.FuelCapsules;
			centralReactor.FuelCapsules--;
			return centralReactor.FuelCapsules < oldFuelCapsules;
		}

		public int GetEnergyInReactor(ZoneLocation zoneLocation)
		{
			return ZonesByLocation[zoneLocation].EnergyInReactor;
		}

		public void KnockOutCaptain()
		{
			var captain = Zones.SelectMany(zone => zone.Players).Single(player => player.IsCaptain);
			captain.IsKnockedOut = true;
		}

		public void SealRedDoors()
		{
			BlueDoors.Sealed = true;
		}

		public void SealBlueDoors()
		{
			RedDoors.Sealed = true;
		}

		public void RepairAllSealedDoors()
		{
			BlueDoors.Sealed = false;
			RedDoors.Sealed = false;
		}

		public virtual bool RedDoorIsSealed => RedDoors.Sealed;
		public virtual bool BlueDoorIsSealed => BlueDoors.Sealed;

		public virtual int GetDamageToZone(ZoneLocation zoneLocation)
		{
			return ZonesByLocation[zoneLocation].TotalDamage;
		}

		public void TeleportPlayers(IEnumerable<Player> playersToTeleport, StationLocation newStationLocation)
		{
			foreach (var player in playersToTeleport)
				player.CurrentStation = StationsByLocation[newStationLocation];
		}
	}
}
