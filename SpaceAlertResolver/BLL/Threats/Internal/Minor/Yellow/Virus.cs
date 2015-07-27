using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Virus : MinorYellowInternalThreat
	{
		public Virus()
			: base(3, 3, StationLocation.UpperWhite, PlayerActionType.C)
		{
		}

		public static string GetDisplayName()
		{
			return "Virus";
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainAllReactors(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.ShiftPlayers(EnumFactory.All<StationLocation>().Where(stationLocation => stationLocation.IsOnShip()), currentTurn + 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			DamageAllZones(1);
		}

		public static string GetId()
		{
			return "I2-05";
		}
	}
}
