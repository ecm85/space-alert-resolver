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

		public override void PerformXAction(int currentTurn)
		{
			AttackAllZones(1);
		}

		public override void PerformYAction(int currentTurn)
		{
			AttackAllZones(2);
		}

		public override void PerformZAction(int currentTurn)
		{
			AttackAllZones(2);
		}

		public static string GetDisplayName()
		{
			return "Pulse Ball";
		}
	}
}
