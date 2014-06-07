using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class NuclearDevice : SeriousYellowInternalThreat
	{
		public NuclearDevice(int timeAppears, ISittingDuck sittingDuck)
			: base(1, 4, timeAppears, StationLocation.LowerWhite, PlayerAction.C, sittingDuck, 2)
		{
		}

		public override void PeformXAction()
		{
			Speed++;
		}

		public override void PerformYAction()
		{
			Speed++;
		}

		public override void PerformZAction()
		{
			throw new LoseException(this);
		}

		public static string GetDisplayName()
		{
			return "Nuclear Device";
		}
	}
}
