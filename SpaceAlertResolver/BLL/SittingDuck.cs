using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Threats.Internal;

namespace BLL
{
	public class SittingDuck : ISittingDuck
	{
		public Zone BlueZone { get; private set; }
		public Zone WhiteZone { get; private set; }
		public Zone RedZone { get; private set; }
		public IDictionary<ZoneLocation, Zone> ZonesByLocation { get; private set; }
		public IDictionary<StationLocation, Station> StationsByLocation { get; private set; }
		public IEnumerable<Zone> Zones { get { return ZonesByLocation.Values; } }
		public IDictionary<StationLocation, StandardStation> StandardStationsByLocation { get; private set; }
		public IList<InterceptorStation> InterceptorStations { get; private set; }
		public ComputerComponent Computer { get; private set; }
		public RocketsComponent RocketsComponent { get; private set; }
		public VisualConfirmationComponent VisualConfirmationComponent { get; private set; }
		public event EventHandler RocketsModified = (sender, args) => { };
		public event EventHandler CentralLaserCannonFired = (sender, args) => { };
		public ThreatController ThreatController { get; private set; }
		public Game Game { get; private set; }

		private CentralReactor CentralReactor { get; set; }
		private IDictionary<StationLocation, BattleBotsComponent> BattleBotsComponentsByLocation { get; set; }
		private Airlock BlueAirlock { get; set; }
		private Airlock RedAirlock { get; set; }

		public SittingDuck(ThreatController threatController, Game game)
		{
			ThreatController = threatController;
			Game = game;

			var redGravolift = new Gravolift();
			var whiteGravolift = new Gravolift();
			var blueGravolift = new Gravolift();

			var redAirlock = new Airlock();
			var blueAirlock = new Airlock();

			var whiteReactor = new CentralReactor();
			var redReactor = new SideReactor(whiteReactor);
			var blueReactor = new SideReactor(whiteReactor);

			var redBatteryPack = new BatteryPack();
			var blueBatteryPack = new BatteryPack();

			var computerComponent = new ComputerComponent();
			var visualConfirmationComponent = new VisualConfirmationComponent();
			var rocketsComponent = new RocketsComponent();
			rocketsComponent.RocketsModified += (sender, args) => RocketsModified(sender, args);

			var interceptors = new Interceptors();
			var interceptorComponent0 = new InterceptorComponent(this, interceptors, StationLocation.UpperRed);
			var interceptorComponent1 = new InterceptorComponent(this, interceptors, StationLocation.Interceptor1);
			var interceptorComponent2 = new InterceptorComponent(this, interceptors, StationLocation.Interceptor2);
			var interceptorComponent3 = new InterceptorComponent(this, interceptors, StationLocation.Interceptor2);

			var upperBlueBattleBots = new BattleBotsComponent();
			var lowerRedBattleBots = new BattleBotsComponent();

			var blueSideHeavyLaserCannon = new SideHeavyLaserCannon(blueReactor, ZoneLocation.Blue);
			var redSideLightLaserCannon = new SideLightLaserCannon(redBatteryPack, ZoneLocation.Red);
			var pulseCannon = new PulseCannon(whiteReactor);
			var blueSideLightLaserCannon = new SideLightLaserCannon(blueBatteryPack, ZoneLocation.Blue);
			var redSideHeavyLaserCannon = new SideHeavyLaserCannon(redReactor, ZoneLocation.Red);
			var centralHeavyLaserCannon = new CentralHeavyLaserCannon(whiteReactor, ZoneLocation.White);
			centralHeavyLaserCannon.CannonFired += (sender, args) => CentralLaserCannonFired(this, EventArgs.Empty);

			var blueSideShield = new SideShield(blueReactor);
			var redSideShield = new SideShield(redReactor);

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
			var upperRedStation = new UpperStation(
				StationLocation.UpperRed,
				threatController,
				interceptorComponent0,
				redGravolift,
				redAirlock,
				null,
				redSideHeavyLaserCannon,
				this,
				redSideShield);
			var upperWhiteStation = new UpperStation(
				StationLocation.UpperWhite,
				threatController,
				computerComponent,
				whiteGravolift,
				blueAirlock,
				redAirlock,
				centralHeavyLaserCannon,
				this,
				new CentralShield(whiteReactor));
			var upperBlueStation = new UpperStation(
				StationLocation.UpperBlue,
				threatController,
				upperBlueBattleBots,
				blueGravolift,
				null,
				BlueAirlock,
				blueSideHeavyLaserCannon,
				this,
				blueSideShield);
			var lowerRedStation = new LowerStation(
				StationLocation.LowerRed,
				threatController,
				lowerRedBattleBots,
				redGravolift,
				RedAirlock,
				null,
				redSideLightLaserCannon,
				this,
				redReactor);
			var lowerWhiteStation = new LowerStation(
				StationLocation.LowerWhite,
				threatController,
				visualConfirmationComponent,
				whiteGravolift,
				blueAirlock,
				redAirlock,
				pulseCannon,
				this,
				whiteReactor);
			var lowerBlueStation = new LowerStation(
				StationLocation.LowerBlue,
				threatController,
				rocketsComponent,
				blueGravolift,
				null,
				blueAirlock,
				blueSideLightLaserCannon,
				this,
				blueReactor);

			CentralReactor = whiteReactor;
			Computer = computerComponent;
			VisualConfirmationComponent = visualConfirmationComponent;
			RocketsComponent = rocketsComponent;
			BlueAirlock = blueAirlock;
			RedAirlock = redAirlock;
			RedZone = new Zone { LowerStation = lowerRedStation, UpperStation = upperRedStation, ZoneLocation = ZoneLocation.Red, Gravolift = redGravolift};
			WhiteZone = new Zone { LowerStation = lowerWhiteStation, UpperStation = upperWhiteStation, ZoneLocation = ZoneLocation.White, Gravolift = whiteGravolift};
			BlueZone = new Zone { LowerStation = lowerBlueStation, UpperStation = upperBlueStation, ZoneLocation = ZoneLocation.Blue, Gravolift = blueGravolift};
			ZonesByLocation = new[] {RedZone, WhiteZone, BlueZone}.ToDictionary(zone => zone.ZoneLocation);
			InterceptorStations = new [] {interceptorStation1, interceptorStation2, interceptorStation3};
			StationsByLocation = Zones
				.SelectMany(zone => new Station[] {zone.LowerStation, zone.UpperStation})
				.Concat(InterceptorStations)
				.ToDictionary(station => station.StationLocation);
			StandardStationsByLocation = Zones
				.SelectMany(zone => new StandardStation[] {zone.LowerStation, zone.UpperStation})
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

		public IEnumerable<Player> GetPlayersOnShip()
		{
			return EnumFactory.All<StationLocation>().SelectMany(stationLocation => StandardStationsByLocation[stationLocation].Players);
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

		public void DisableInactiveBattleBots(IEnumerable<StationLocation> stationLocations)
		{
			foreach (var stationLocation in stationLocations.Where(stationLocation => BattleBotsComponentsByLocation.ContainsKey(stationLocation)))
				BattleBotsComponentsByLocation[stationLocation].DisableInactiveBattleBots();
		}

		public int RocketCount
		{
			get { return RocketsComponent.RocketCount; }
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

		public void ShiftPlayersAndRepeatPreviousAction(IEnumerable<StationLocation> stationLocations, int turnToShift)
		{
			foreach (var player in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]).SelectMany(station => station.Players))
				player.ShiftAndRepeatPreviousAction(turnToShift);
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

		public int GetEnergyInReactor(ZoneLocation zoneLocation)
		{
			return ZonesByLocation[zoneLocation].GetEnergyInReactor();
		}

		public void KnockOutCaptain()
		{
			var captain = Zones.SelectMany(zone => zone.Players).Single(player => player.IsCaptain);
			captain.IsKnockedOut = true;
		}

		public void BreachRedAirlock()
		{
			BlueAirlock.Breached = true;
		}

		public void BreachBlueAirlock()
		{
			RedAirlock.Breached = true;
		}

		public void RepairAllAirlockBreaches()
		{
			BlueAirlock.Breached = false;
			RedAirlock.Breached = false;
		}

		public virtual bool RedAirlockIsBreached { get { return RedAirlock.Breached; } }
		public virtual bool BlueAirlockIsBreached { get { return BlueAirlock.Breached; }}

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
