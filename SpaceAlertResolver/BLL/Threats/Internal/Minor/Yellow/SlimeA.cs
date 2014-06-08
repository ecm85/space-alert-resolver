using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class SlimeA : Slime
	{
		public SlimeA(int timeAppears, ISittingDuck sittingDuck)
			: base(timeAppears, StationLocation.LowerBlue, sittingDuck)
		{
		}

		protected SlimeA(int timeAppears, StationLocation stationLocation, ISittingDuck sittingDuck)
			: base(timeAppears, stationLocation, sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Slime I2-01";
		}

		public override void PeformXAction()
		{
			sittingDuck.RemoveRocket();
		}

		public override void PerformYAction()
		{
			var leftStation = sittingDuck.StationByLocation[CurrentStation].RedwardStation;
			Spread(leftStation);
		}

		private class ProgenySlime : SlimeA
		{
			public ProgenySlime(int timeAppears, StationLocation stationLocation, ISittingDuck sittingDuck)
				: base(timeAppears, stationLocation, sittingDuck)
			{
			}

			public override int Points
			{
				get { return 0; }
			}
		}

		protected override Slime CreateProgeny()
		{
			return new ProgenySlime(TimeAppears, sittingDuck.StationByLocation[CurrentStation].RedwardStation.StationLocation, sittingDuck);
		}
	}
}
