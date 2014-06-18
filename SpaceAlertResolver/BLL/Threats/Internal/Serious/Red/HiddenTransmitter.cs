using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class HiddenTransmitter : SeriousRedInternalThreat
	{
		public HiddenTransmitter()
			: base(3, 2, StationLocation.LowerRed, PlayerAction.C, 1)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			totalInaccessibility = 0;
			//TODO: Calls in external threat in red zone
			//TODO: Points
			//Killed before x: worth 8 + internal threat points
			//Killed after x: worth 8, internal threat worth normal points
			//Hits Z: worth 4, internal threat worth normal points
			throw new NotImplementedException();
		}

		protected override void PerformYAction(int currentTurn)
		{
			//TODO: Move all threats in red zone
			throw new NotImplementedException();
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.DrainReactor(CurrentZone);
			Damage(4);
		}
	}
}
