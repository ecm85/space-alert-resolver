using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Virus : MinorYellowInternalThreat
	{
		public Virus()
			: base(3, 3, StationLocation.UpperWhite, PlayerAction.C)
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
			SittingDuck.ShiftPlayers(EnumFactory.All<StationLocation>().Except(new [] {StationLocation.Interceptor}), currentTurn + 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			DamageAllZones(1);
		}
	}
}
