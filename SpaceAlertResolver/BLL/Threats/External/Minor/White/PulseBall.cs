using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class PulseBall : MinorWhiteExternalThreat
	{
		public PulseBall()
			: base(1, 5, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackAllZones(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackAllZones(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(2);
		}

		public static string GetDisplayName()
		{
			return "Pulse Ball";
		}
	}
}
