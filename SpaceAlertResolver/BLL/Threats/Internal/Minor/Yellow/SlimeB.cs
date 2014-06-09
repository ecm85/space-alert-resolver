using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class SlimeB : Slime
	{
		public SlimeB()
			: base(StationLocation.LowerRed)
		{
		}

		protected SlimeB(StationLocation stationLocation)
			: base(stationLocation)
		{
		}

		public static string GetDisplayName()
		{
			return "Slime I2-02";
		}

		public override void PerformXAction(int currentTurn)
		{
			SittingDuck.DisableInactiveBattlebots(new[] { StationLocation.LowerRed });
		}

		public override void PerformYAction(int currentTurn)
		{
			var bluewardStation = CurrentStation.BluewardStationLocation();
			Spread(bluewardStation);
		}

		private class ProgenySlime : SlimeB
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
			var newSlimeLocation = CurrentStation.BluewardStationLocation();
			if (!newSlimeLocation.HasValue)
				throw new InvalidOperationException("Tried to spread to invalid station.");
			return new ProgenySlime(newSlimeLocation.Value);
		}
	}
}
