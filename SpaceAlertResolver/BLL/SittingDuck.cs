using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class SittingDuck
	{
		private readonly Zone blueZone;
		private readonly Zone whiteZone;
		private readonly Zone redZone;


		public SittingDuck(IEnumerable<Player> players)
		{
			var whiteReactor = new CentralReactor();
			var redReactor = new SideReactor(whiteReactor);
			var blueReactor = new SideReactor(whiteReactor);
			var redBatteryPack = new BatteryPack();
			var blueBatteryPack = new BatteryPack();
			var upperRedStation = new Station
			{
				Cannon = new SideHeavyLaserCannon(redReactor, ZoneType.Red),
				EnergyContainer = new SideShield(redReactor)
			};
			var upperWhiteStation = new Station
			{
				Cannon = new CentralHeavyLaserCannon(whiteReactor, ZoneType.White),
				EnergyContainer = new CentralShield(whiteReactor)
			};
			var upperBlueStation = new Station
			{
				Cannon = new SideHeavyLaserCannon(blueReactor, ZoneType.Blue),
				EnergyContainer = new SideShield(blueReactor)
			};
			var lowerRedStation = new Station
			{
				Cannon = new SideLightLaserCannon(redBatteryPack, ZoneType.Red),
				EnergyContainer = redReactor
			};
			var lowerWhiteStation = new Station
			{
				Cannon = new PulseCannon(whiteReactor),
				EnergyContainer = whiteReactor
			};
			var lowerBlueStation = new Station
			{
				Cannon = new SideLightLaserCannon(blueBatteryPack, ZoneType.Blue),
				EnergyContainer = blueReactor
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
			redZone = new Zone {LowerStation = lowerRedStation, UpperStation = upperRedStation};
			whiteZone = new Zone { LowerStation = lowerWhiteStation, UpperStation = upperWhiteStation };
			blueZone = new Zone { LowerStation = lowerBlueStation, UpperStation = upperBlueStation };
		}

		public DamageResult TakeDamage(int damage, params ZoneType[] zoneTypes)
		{
			var damageResult = new DamageResult();
			foreach (var zoneType in zoneTypes)
				switch (zoneType)
				{
					case ZoneType.Blue:
						damageResult.AddDamage(blueZone.TakeDamage(damage));
						break;
					case ZoneType.Red:
						damageResult.AddDamage(redZone.TakeDamage(damage));
						break;
					case ZoneType.White:
						damageResult.AddDamage(whiteZone.TakeDamage(damage));
						break;
				}
			return damageResult;
		}

		public void DrainAllShields(params ZoneType[] zoneTypes)
		{
			foreach (var zoneType in zoneTypes)
				switch (zoneType)
				{
					case ZoneType.Blue:
						blueZone.DrainShields();
						break;
					case ZoneType.Red:
						redZone.DrainShields();
						break;
					case ZoneType.White:
						whiteZone.DrainShields();
						break;
				}
		}
	}
}
