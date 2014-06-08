using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class SlimeB : Slime
	{
		public SlimeB(int timeAppears, ISittingDuck sittingDuck)
			: base(timeAppears, StationLocation.LowerRed, sittingDuck)
		{
		}

		protected SlimeB(int timeAppears, StationLocation stationLocation, ISittingDuck sittingDuck)
			: base(timeAppears, stationLocation, sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Slime I2-02";
		}

		public override void PeformXAction()
		{
			sittingDuck.DisableInactiveBattlebots(new[] { StationLocation.LowerRed });
		}

		public override void PerformYAction()
		{
			var bluewardStation = CurrentStation.BluewardStationLocation();
			Spread(bluewardStation);
		}

		private class ProgenySlime : SlimeB
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
			var newSlimeLocation = CurrentStation.BluewardStationLocation();
			if (!newSlimeLocation.HasValue)
				throw new InvalidOperationException("Tried to spread to invalid station.");
			return new ProgenySlime(TimeAppears, newSlimeLocation.Value, sittingDuck);
		}
	}
}
