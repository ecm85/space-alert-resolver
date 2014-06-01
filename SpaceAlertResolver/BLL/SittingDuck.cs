using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.Internal;

namespace BLL
{
	public class SittingDuck
	{
		public Zone BlueZone { get; private set; }
		public Zone WhiteZone { get; private set; }
		public Zone RedZone { get; private set; }
		public IDictionary<ZoneLocation, Zone> ZonesByLocation { get; private set; }
		public IEnumerable<Zone> Zones { get { return ZonesByLocation.Values; } }
		public InterceptorStation InterceptorStation1 { get; set; }
		public ComputerComponent Computer { get; private set; }
		public RocketsComponent RocketsComponent { get; private set; }
		public VisualConfirmationComponent VisualConfirmationComponent { get; private set; }
		public IList<ExternalThreat> CurrentExternalThreats { get; private set; }
		public IList<InternalThreat> CurrentInternalThreats { get; private set; }

		public SittingDuck(IEnumerable<Player> players)
		{
			CurrentInternalThreats = new List<InternalThreat>();
			CurrentExternalThreats = new List<ExternalThreat>();
			var whiteReactor = new CentralReactor();
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
			var upperRedStation = new Station
			{
				Cannon = new SideHeavyLaserCannon(redReactor, ZoneLocation.Red),
				EnergyContainer = new SideShield(redReactor),
				ZoneLocation = ZoneLocation.Red,
				CComponent = new InterceptorComponent()
			};
			
			var upperWhiteStation = new Station
			{
				Cannon = new CentralHeavyLaserCannon(whiteReactor, ZoneLocation.White),
				EnergyContainer = new CentralShield(whiteReactor),
				ZoneLocation = ZoneLocation.White,
				CComponent = computerComponent
			};
			var upperBlueStation = new Station
			{
				Cannon = new SideHeavyLaserCannon(blueReactor, ZoneLocation.Blue),
				EnergyContainer = new SideShield(blueReactor),
				ZoneLocation = ZoneLocation.Blue,
				CComponent = new BattleBotsComponent()
			};
			var lowerRedStation = new Station
			{
				Cannon = new SideLightLaserCannon(redBatteryPack, ZoneLocation.Red),
				EnergyContainer = redReactor,
				ZoneLocation = ZoneLocation.Red,
				CComponent = new BattleBotsComponent()
			};
			
			var lowerWhiteStation = new Station
			{
				Cannon = new PulseCannon(whiteReactor),
				EnergyContainer = whiteReactor,
				ZoneLocation = ZoneLocation.White,
				CComponent = visualConfirmationComponent
			};
			var lowerBlueStation = new Station
			{
				Cannon = new SideLightLaserCannon(blueBatteryPack, ZoneLocation.Blue),
				EnergyContainer = blueReactor,
				ZoneLocation = ZoneLocation.Blue,
				CComponent = rocketsComponent
			};
			upperRedStation.BluewardStation = upperWhiteStation;
			upperRedStation.OppositeDeckStation = lowerRedStation;
			upperWhiteStation.RedwardStation = upperRedStation;
			upperWhiteStation.BluewardStation = upperBlueStation;
			upperWhiteStation.OppositeDeckStation = lowerWhiteStation;
			upperBlueStation.RedwardStation = upperWhiteStation;
			upperBlueStation.OppositeDeckStation = lowerBlueStation;
			lowerRedStation.BluewardStation = lowerWhiteStation;
			lowerRedStation.OppositeDeckStation = upperRedStation;
			lowerWhiteStation.RedwardStation = lowerRedStation;
			lowerWhiteStation.BluewardStation = lowerBlueStation;
			lowerWhiteStation.OppositeDeckStation = upperWhiteStation;
			lowerBlueStation.RedwardStation = lowerWhiteStation;
			lowerBlueStation.OppositeDeckStation = upperBlueStation;
			foreach (var player in players)
			{
				player.CurrentStation = upperWhiteStation;
				upperWhiteStation.Players.Add(player);
			}
			RedZone = new Zone { LowerStation = lowerRedStation, UpperStation = upperRedStation, ZoneLocation = ZoneLocation.Red, Gravolift = new Gravolift() };
			WhiteZone = new Zone { LowerStation = lowerWhiteStation, UpperStation = upperWhiteStation, ZoneLocation = ZoneLocation.White, Gravolift = new Gravolift() };
			BlueZone = new Zone { LowerStation = lowerBlueStation, UpperStation = upperBlueStation, ZoneLocation = ZoneLocation.Blue, Gravolift = new Gravolift() };
			ZonesByLocation = new[] {RedZone, WhiteZone, BlueZone}.ToDictionary(zone => zone.ZoneLocation);
		}

		public void TakeDamage(int damage, params ZoneLocation[] zones)
		{
			TakeDamage(damage, zones.ToList());
		}

		private void TakeDamage(int damage, IEnumerable<ZoneLocation> zones)
		{
			foreach (var zone in zones)
				ZonesByLocation[zone].TakeDamage(damage);
		}

		public void DrainAllShields()
		{
			BlueZone.DrainShields();
			RedZone.DrainShields();
			WhiteZone.DrainShields();
		}
	}
}
