using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class Fissure : SeriousWhiteInternalThreat
	{
		//TODO: Figure out this threat should behave (only interact with battlebots, fired either via c or from space via battlebots)
		public Fissure(int timeAppears, SittingDuck sittingDuck)
			: base(2, 2, timeAppears, sittingDuck.InterceptorStation1, PlayerAction.Interceptors, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			//TODO: Add 'double damage' debuff to current zone
		}

		public override void PerformYAction()
		{
			//TODO: Add 'double damage' debuff to entire ship
		}

		public override void PerformZAction()
		{
			//TODO: Lose.
		}
	}
}
