using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.ShipComponents;
using BLL.Threats;
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

        internal SittingDuck(ThreatController threatController, Game game, ILookup<ZoneLocation, DamageToken> initialDamage)
        {
            ThreatController = threatController;
            threatController.ThreatAttackedShip += TakeAttack;
            Game = game;

            var redDoors = new Doors();
            var blueDoors = new Doors();


            var interceptors = new Interceptors();

            var interceptorStation1 = CreateInterceptorStation1(threatController);
            var interceptorStation2 = CreateInterceptorStation2(threatController);
            var interceptorStation3 = CreateInterceptorStation3(threatController);

            BlueDoors = blueDoors;
            RedDoors = redDoors;
            WhiteZone = CreateWhiteZone(threatController, initialDamage, redDoors, blueDoors);
            RedZone = CreateRedZone(threatController, initialDamage, redDoors, WhiteZone, interceptors);
            BlueZone = CreateBlueZone(threatController, initialDamage, WhiteZone, blueDoors);

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

        private InterceptorStation CreateInterceptorStation3(ThreatController threatController)
        {
            var interceptorComponent3 = new InterceptorsInSpaceComponent(this, StationLocation.Interceptor3);
            var interceptorStation3 = new InterceptorStation(
                StationLocation.Interceptor3,
                threatController,
                interceptorComponent3);
            return interceptorStation3;
        }

        private InterceptorStation CreateInterceptorStation2(ThreatController threatController)
        {
            var interceptorComponent2 = new InterceptorsInSpaceComponent(this, StationLocation.Interceptor2);
            var interceptorStation2 = new InterceptorStation(
                StationLocation.Interceptor2,
                threatController,
                interceptorComponent2);
            return interceptorStation2;
        }

        private InterceptorStation CreateInterceptorStation1(ThreatController threatController)
        {
            var interceptorComponent1 = new InterceptorsInSpaceComponent(this, StationLocation.Interceptor1);
            var interceptorStation1 = new InterceptorStation(
                StationLocation.Interceptor1,
                threatController,
                interceptorComponent1);
            return interceptorStation1;
        }

        private BlueZone CreateBlueZone(
            ThreatController threatController,
            ILookup<ZoneLocation, DamageToken> initialDamage,
            WhiteZone whiteZone,
            Doors blueDoors)
        {
            var blueZone = new BlueZone(threatController, whiteZone.LowerWhiteStation.CentralReactor, blueDoors, this);
            DamageZone(initialDamage, ZoneLocation.Blue, blueZone);
            blueZone.LowerBlueStation.RocketsComponent.RocketsModified += (sender, args) => RocketsModified(sender, args);
            return blueZone;
        }

        private RedZone CreateRedZone(
            ThreatController threatController,
            ILookup<ZoneLocation, DamageToken> initialDamage,
            Doors redDoors,
            WhiteZone whiteZone,
            Interceptors interceptors)
        {
            var redZone = new RedZone(threatController, whiteZone.LowerWhiteStation.CentralReactor, redDoors, this, interceptors);
            DamageZone(initialDamage, ZoneLocation.Red, redZone);
            return redZone;
        }

        private WhiteZone CreateWhiteZone(
            ThreatController threatController,
            ILookup<ZoneLocation, DamageToken> initialDamage,
            Doors redDoors,
            Doors blueDoors)
        {
            var whiteZone = new WhiteZone(threatController, redDoors, blueDoors, this);
            DamageZone(initialDamage, ZoneLocation.White, whiteZone);
            whiteZone.UpperWhiteStation.AlphaComponent.CannonFired += (sender, args) => CentralLaserCannonFired(this, EventArgs.Empty);
            return whiteZone;
        }

        private static void DamageZone(ILookup<ZoneLocation, DamageToken> initialDamage, ZoneLocation zoneLocation, Zone zone)
        {
            if (initialDamage == null || !initialDamage.Any())
                return;
            foreach (var damageToken in initialDamage[zoneLocation].ToList())
                zone.TakeDamage(damageToken, true);
        }

        public void SetPlayers(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                player.CurrentStation = WhiteZone.UpperStation;
                WhiteZone.UpperStation.Players.Add(player);
            }
        }

        public int DrainShields(IEnumerable<ZoneLocation> zoneLocations, int? amount = null)
        {
            return zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation].DrainShield(amount)).Sum();
        }

        public int DrainReactors(IEnumerable<ZoneLocation> zoneLocations, int? amount = null)
        {
            return zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation].DrainReactor(amount)).Sum();
        }

        public void DrainEnergy(StationLocation stationLocation, int? amount)
        {
            StandardStationsByLocation[stationLocation].DrainEnergy(amount);
        }

        private void TakeAttack(object sender, ThreatDamageEventArgs args)
        {
            var damage = args.ThreatDamage;
            var threat = sender as Threat;
            var zone = ZonesByLocation[damage.ZoneLocation];
            if (damage.ThreatDamageType == ThreatDamageType.ReducedByTwoAgainstInterceptors)
            {
                var amount = damage.Amount;
                if (damage.DistanceToSource != null)
                {
                    var stationsBetweenThreatAndShip = InterceptorStations
                        .Where(station => station.StationLocation.DistanceFromShip() <= damage.DistanceToSource);
                    if (stationsBetweenThreatAndShip.Any(station => station.Players.Any()))
                        amount -= 2;
                }
                var damageResult = zone.TakeAttack(amount, ThreatDamageType.Standard);
                if (damageResult.ShipDestroyed)
                    throw new LoseException(threat);
                damage.DamageShielded += damageResult.DamageShielded;
            }
            else
            {
                var damageResult = zone.TakeAttack(damage.Amount, damage.ThreatDamageType);
                if (damageResult.ShipDestroyed)
                    throw new LoseException(threat);
                damage.DamageShielded += damageResult.DamageShielded;
            }
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
                player.KnockOut();
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

        public void ShiftPlayersAfterPlayerActions(IEnumerable<ZoneLocation> zoneLocations, int turnToShift)
        {
            foreach (var player in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]).SelectMany(zone => zone.Players))
                player.ShiftAfterPlayerActions(turnToShift);
        }

        public void ShiftPlayersAfterPlayerActions(IEnumerable<StationLocation> stationLocations, int turnToShift)
        {
            foreach (var player in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]).SelectMany(station => station.Players))
                player.ShiftAfterPlayerActions(turnToShift);
        }

        public void ShiftPlayersAndRepeatPreviousAction(IEnumerable<StationLocation> stationLocations, int turnToShift)
        {
            foreach (var player in stationLocations.Select(stationLocation => StationsByLocation[stationLocation]).SelectMany(station => station.Players))
                player.ShiftAndRepeatPreviousActionAfterPlayerActions(turnToShift);
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
            captain.KnockOut();
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
