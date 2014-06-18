using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.Internal;

namespace BLL
{
	public class SittingDuck : ISittingDuck
	{
		public Zone BlueZone { get; private set; }
		public Zone WhiteZone { get; private set; }
		public Zone RedZone { get; private set; }
		private IDictionary<ZoneLocation, Zone> ZonesByLocation { get; set; }
		public IEnumerable<Zone> Zones { get { return ZonesByLocation.Values; } }
		public IDictionary<StationLocation, Station> StationsByLocation { get; private set; }
		public IList<InterceptorStation> InterceptorStations { get; private set; }
		public ComputerComponent Computer { get; private set; }
		public RocketsComponent RocketsComponent { get; private set; }
		private CentralReactor CentralReactor { get; set; }
		public VisualConfirmationComponent VisualConfirmationComponent { get; private set; }
		public event Action RocketsModified = () => { };
		private IDictionary<StationLocation, BattleBotsComponent> BattleBotsComponentsByLocation { get; set; }

		public SittingDuck(ThreatController threatController)
		{
			var redGravolift = new Gravolift();
			var whiteGravolift = new Gravolift();
			var blueGravolift = new Gravolift();
			var redAirlock = new Airlock();
			var blueAirlock = new Airlock();
			var whiteReactor = new CentralReactor();
			CentralReactor = whiteReactor;
			var redReactor = new SideReactor(whiteReactor);
			var blueReactor = new SideReactor(whiteReactor);
			var redBatteryPack = new BatteryPack();
			var blueBatteryPack = new BatteryPack();
			var computerComponent = new ComputerComponent();
			var visualConfirmationComponent = new VisualConfirmationComponent();
			var rocketsComponent = new RocketsComponent();
			Computer = computerComponent;
			VisualConfirmationComponent = visualConfirmationComponent;
			RocketsComponent = rocketsComponent;
			rocketsComponent.RocketsModified += () => RocketsModified();
			var interceptorStation1 = new InterceptorStation
			{
				StationLocation = StationLocation.Interceptor1,
				ThreatController = threatController
			};
			var interceptorStation2 = new InterceptorStation
			{
				StationLocation = StationLocation.Interceptor2,
				ThreatController = threatController
			};
			var interceptorStation3 = new InterceptorStation
			{
				StationLocation = StationLocation.Interceptor3,
				ThreatController = threatController
			};
			var upperRedStation = new UpperStation
			{
				Cannon = new SideHeavyLaserCannon(redReactor, ZoneLocation.Red),
				Shield = new SideShield(redReactor),
				StationLocation = StationLocation.UpperRed,
				BluewardAirlock = redAirlock,
				Gravolift = redGravolift,
				ThreatController = threatController
			};
			var interceptors = new Interceptors();
			interceptorStation1.InterceptorComponent = new InterceptorComponent(interceptorStation2, upperRedStation, interceptors);
			interceptorStation2.InterceptorComponent = new InterceptorComponent(interceptorStation3, interceptorStation1, interceptors);
			interceptorStation3.InterceptorComponent = new InterceptorComponent(null, interceptorStation2, interceptors);
			upperRedStation.CComponent = new InterceptorComponent(interceptorStation1, null, interceptors);
			var upperWhiteStation = new UpperStation
			{
				Cannon = new CentralHeavyLaserCannon(whiteReactor, ZoneLocation.White),
				Shield = new CentralShield(whiteReactor),
				StationLocation = StationLocation.UpperWhite,
				CComponent = computerComponent,
				BluewardAirlock = blueAirlock,
				RedwardAirlock = redAirlock,
				Gravolift = whiteGravolift,
				ThreatController = threatController
			};
			var upperBlueBattleBots = new BattleBotsComponent();
			var upperBlueStation = new UpperStation
			{
				Cannon = new SideHeavyLaserCannon(blueReactor, ZoneLocation.Blue),
				Shield = new SideShield(blueReactor),
				StationLocation = StationLocation.UpperBlue,
				CComponent = upperBlueBattleBots,
				RedwardAirlock = blueAirlock,
				Gravolift = blueGravolift,
				ThreatController = threatController
			};
			var lowerRedBattleBots = new BattleBotsComponent();
			var lowerRedStation = new LowerStation
			{
				Cannon = new SideLightLaserCannon(redBatteryPack, ZoneLocation.Red),
				Reactor = redReactor,
				StationLocation = StationLocation.LowerRed,
				CComponent = lowerRedBattleBots,
				BluewardAirlock = redAirlock,
				Gravolift = redGravolift,
				ThreatController = threatController
			};

			var lowerWhiteStation = new LowerStation
			{
				Cannon = new PulseCannon(whiteReactor),
				Reactor = whiteReactor,
				StationLocation = StationLocation.LowerWhite,
				CComponent = visualConfirmationComponent,
				BluewardAirlock = blueAirlock,
				RedwardAirlock = redAirlock,
				Gravolift = whiteGravolift,
				ThreatController = threatController
			};
			var lowerBlueStation = new LowerStation
			{
				Cannon = new SideLightLaserCannon(blueBatteryPack, ZoneLocation.Blue),
				Reactor = blueReactor,
				StationLocation = StationLocation.LowerBlue,
				CComponent = rocketsComponent,
				RedwardAirlock = blueAirlock,
				Gravolift = blueGravolift,
				ThreatController = threatController
			};

			RedZone = new Zone { LowerStation = lowerRedStation, UpperStation = upperRedStation, ZoneLocation = ZoneLocation.Red, Gravolift = redGravolift};
			WhiteZone = new Zone { LowerStation = lowerWhiteStation, UpperStation = upperWhiteStation, ZoneLocation = ZoneLocation.White, Gravolift = whiteGravolift};
			BlueZone = new Zone { LowerStation = lowerBlueStation, UpperStation = upperBlueStation, ZoneLocation = ZoneLocation.Blue, Gravolift = blueGravolift};
			ZonesByLocation = new[] {RedZone, WhiteZone, BlueZone}.ToDictionary(zone => zone.ZoneLocation);
			InterceptorStations = new [] {interceptorStation1, interceptorStation2, interceptorStation3};
			StationsByLocation = Zones
				.SelectMany(zone => new Station[] {zone.LowerStation, zone.UpperStation})
				.Concat(InterceptorStations)
				.ToDictionary(station => station.StationLocation);

			BattleBotsComponentsByLocation = new Dictionary<StationLocation, BattleBotsComponent>
			{
				{StationLocation.UpperBlue, upperBlueBattleBots},
				{StationLocation.LowerRed, lowerRedBattleBots}
			};
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

		public void DrainReactor(ZoneLocation zoneLocation)
		{
			ZonesByLocation[zoneLocation].DrainReactor();
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

		public ThreatDamageResult TakeAttack(ThreatDamage damage)
		{
			var shipDestroyed = false;
			var damageShielded = 0;
			foreach (var zone in damage.ZoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
			{
				ThreatDamageResult damageResult;
				switch (damage.ThreatDamageType)
				{
					case ThreatDamageType.IgnoresShields:
						damageResult = zone.TakeDamage(damage.Amount);
						shipDestroyed = shipDestroyed || damageResult.ShipDestroyed;
						damageShielded += damageResult.DamageShielded;
						break;
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

		public int GetPoisonedPlayerCount(IEnumerable<StationLocation> locations)
		{
			return locations
				.Select(location => StationsByLocation[location])
				.SelectMany(station => station.Players)
				.Count(player => player.IsPoisoned);
		}

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

		public void KnockOutPoisonedPlayers(IEnumerable<StationLocation> locations)
		{
			var poisonedPlayers = locations
				.Select(location => StationsByLocation[location])
				.SelectMany(zone => zone.Players)
				.Where(player => player.IsPoisoned);
			KnockOut(poisonedPlayers);
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
				TransferEnergyToShield(zone.UpperStation.Shield, zone.LowerStation.Reactor);
		}

		private static void TransferEnergyToShield(Shield shield, Reactor reactor)
		{
			var roomForShields = shield.Capacity - shield.Energy;
			var energyTransferredToShields = Math.Min(roomForShields, reactor.Energy);
			shield.Energy += energyTransferredToShields;
			reactor.Energy -= energyTransferredToShields;
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

		public void DisableInactiveBattlebots(IEnumerable<StationLocation> stationLocations)
		{
			foreach (var stationLocation in stationLocations.Where(stationLocation => BattleBotsComponentsByLocation.ContainsKey(stationLocation)))
				BattleBotsComponentsByLocation[stationLocation].DisableInactiveBattleBots();
		}

		public int GetRocketCount()
		{
			return RocketsComponent.RocketCount;
		}

		public void RemoveRocket()
		{
			RocketsComponent.RemoveRocket();
		}

		public void RemoveAllRockets()
		{
			RocketsComponent.RemoveAllRockets();
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

		public void SubscribeToMoveIn(IEnumerable<StationLocation> stationLocations, Action<Player, int> handler)
		{
			foreach (var station in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]))
				station.MoveIn += handler;
		}

		public void SubscribeToMoveOut(IEnumerable<StationLocation> stationLocations, Action<Player, int> handler)
		{
			foreach (var station in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]))
				station.MoveOut += handler;
		}

		public void UnsubscribeFromMoveIn(IEnumerable<StationLocation> stationLocations, Action<Player, int> handler)
		{
			foreach (var station in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]))
				station.MoveIn -= handler;
		}

		public void UnsubscribeFromMoveOut(IEnumerable<StationLocation> stationLocations, Action<Player, int> handler)
		{
			foreach (var station in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]))
				station.MoveOut -= handler;
		}

		public void AddIrreparableMalfunctionToStations(IEnumerable<StationLocation> stationLocations, IrreparableMalfunction malfunction)
		{
			foreach (var station in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]))
				station.IrreparableMalfunctions.Add(malfunction);
		}

		public bool DestroyFuelCapsule()
		{
			var oldFuelCapsules = CentralReactor.FuelCapsules;
			CentralReactor.FuelCapsules--;
			return CentralReactor.FuelCapsules < oldFuelCapsules;
		}

		public int GetEnergyInReactor(ZoneLocation currentZone)
		{
			return ZonesByLocation[currentZone].GetEnergyInReactor();
		}

		public void KnockOutCaptain()
		{
			var captain = Zones.SelectMany(zone => zone.Players).Single(player => player.IsCaptain);
			captain.IsKnockedOut = true;
		}

		public void InfectPlayers(StationLocation currentStation)
		{
			foreach (var player in StationsByLocation[currentStation].Players)
				player.IsInfected = true;
		}
	}
}
