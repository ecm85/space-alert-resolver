using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
	public class Driller : MinorRedInternalThreat
	{
		public Driller()
			: base(2, 3, StationLocation.LowerBlue, PlayerAction.BattleBots, 1)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			totalInaccessibility = 0;
			MoveTowardsMostDamagedZone();
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveTowardsMostDamagedZone();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(4);
		}

		private void MoveTowardsMostDamagedZone()
		{
			//TODO: Move towards most damaged zone
		}

		public static string GetDisplayName()
		{
			return "Driller";
		}
	}
}
