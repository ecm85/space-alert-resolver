using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Slime : MinorYellowInternalThreat
	{
		//TODO: Implement this
		//TODO: Make SlimeProgeny
		public Slime(int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck) : base(health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
		}

		public Slime(int health, int speed, int timeAppears, IList<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck) : base(health, speed, timeAppears, currentStations, actionType, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			throw new NotImplementedException();
		}

		public override void PerformYAction()
		{
			throw new NotImplementedException();
		}

		public override void PerformZAction()
		{
			throw new NotImplementedException();
		}
	}
}
