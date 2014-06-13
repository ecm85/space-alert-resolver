using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class SlimeA : Slime
	{
		public SlimeA()
			: base(StationLocation.LowerBlue)
		{
		}

		protected SlimeA(StationLocation stationLocation)
			: base(stationLocation)
		{
		}

		public static string GetDisplayName()
		{
			return "Slime I2-01";
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.RemoveRocket();
		}

		protected override void PerformYAction(int currentTurn)
		{
			var redwardStation = CurrentStation.RedwardStationLocation();
			Spread(redwardStation);
		}

		private class ProgenySlime : SlimeA
		{
			public ProgenySlime(StationLocation stationLocation)
				: base(stationLocation)
			{
			}

			public override int Points
			{
				get { return 0; }
			}

			public override bool IsDefeated
			{
				get { return false; }
			}

			public override bool IsSurvived 
			{
				get { return false; }
			}
		}

		protected override Slime CreateProgeny(StationLocation stationLocation)
		{
			return new ProgenySlime(stationLocation);
		}
	}
}
