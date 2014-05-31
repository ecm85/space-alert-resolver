using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class SittingDuck
	{
		public Zone BlueZone { get; private set; }
		public Zone WhiteZone { get; private set; }
		public Zone RedZone { get; private set; }
		public IDictionary<ZoneLocation, Zone> ZonesByLocation { get; private set; }
		public IEnumerable<Zone> Zones { get { return ZonesByLocation.Values; } }

		public SittingDuck(IEnumerable<Player> players)
		{
			var whiteReactor = new CentralReactor();
			var redReactor = new SideReactor(whiteReactor);
			var blueReactor = new SideReactor(whiteReactor);
			var redBatteryPack = new BatteryPack();
			var blueBatteryPack = new BatteryPack();
			var upperRedStation = new Station
			{
				Cannon = new SideHeavyLaserCannon(redReactor, ZoneLocation.Red),
				EnergyContainer = new SideShield(redReactor),
				ZoneLocation = ZoneLocation.Red
			};
			var upperWhiteStation = new Station
			{
				Cannon = new CentralHeavyLaserCannon(whiteReactor, ZoneLocation.White),
				EnergyContainer = new CentralShield(whiteReactor),
				ZoneLocation = ZoneLocation.White
			};
			var upperBlueStation = new Station
			{
				Cannon = new SideHeavyLaserCannon(blueReactor, ZoneLocation.Blue),
				EnergyContainer = new SideShield(blueReactor),
				ZoneLocation = ZoneLocation.Blue
			};
			var lowerRedStation = new Station
			{
				Cannon = new SideLightLaserCannon(redBatteryPack, ZoneLocation.Red),
				EnergyContainer = redReactor,
				ZoneLocation = ZoneLocation.Red
			};
			var lowerWhiteStation = new Station
			{
				Cannon = new PulseCannon(whiteReactor),
				EnergyContainer = whiteReactor,
				ZoneLocation = ZoneLocation.White
			};
			var lowerBlueStation = new Station
			{
				Cannon = new SideLightLaserCannon(blueBatteryPack, ZoneLocation.Blue),
				EnergyContainer = blueReactor,
				ZoneLocation = ZoneLocation.Blue
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
				player.CurrentStation = upperWhiteStation;
			RedZone = new Zone {LowerStation = lowerRedStation, UpperStation = upperRedStation, ZoneLocation = ZoneLocation.Red};
			WhiteZone = new Zone { LowerStation = lowerWhiteStation, UpperStation = upperWhiteStation, ZoneLocation = ZoneLocation.White};
			BlueZone = new Zone { LowerStation = lowerBlueStation, UpperStation = upperBlueStation, ZoneLocation = ZoneLocation.Blue};
			ZonesByLocation = new[] {RedZone, WhiteZone, BlueZone}.ToDictionary(zone => zone.ZoneLocation);
		}

		public void TakeDamage(int damage, params ZoneLocation[] zones)
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
