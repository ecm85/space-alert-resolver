using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class SlimeB : Slime
	{
		public SlimeB()
			: base(StationLocation.LowerRed)
		{
		}

		private SlimeB(StationLocation stationLocation)
			: base(stationLocation)
		{
		}


		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DisableInactiveBattlebots(new[] { StationLocation.LowerRed });
		}

		protected override void PerformYAction(int currentTurn)
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
