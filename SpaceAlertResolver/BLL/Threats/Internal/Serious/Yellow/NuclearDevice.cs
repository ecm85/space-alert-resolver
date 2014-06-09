using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class NuclearDevice : SeriousYellowInternalThreat
	{
		public NuclearDevice()
			: base(1, 4, StationLocation.LowerWhite, PlayerAction.C, 2)
		{
		}

		public override void PerformXAction()
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
