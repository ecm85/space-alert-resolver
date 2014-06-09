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

		public override void PerformXAction()
		{
			SittingDuck.RemoveRocket();
		}

		public override void PerformYAction()
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
		}

		protected override Slime CreateProgeny()
		{
			var newSlimeLocation = CurrentStation.RedwardStationLocation();
			if (!newSlimeLocation.HasValue)
				throw new InvalidOperationException("Tried to spread to invalid station.");
			return new ProgenySlime(newSlimeLocation.Value);
		}
	}
}
