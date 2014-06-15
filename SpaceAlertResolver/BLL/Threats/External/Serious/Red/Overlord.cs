using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Red
{
	public class Overlord : SeriousRedExternalThreat
	{
		public Overlord()
			: base(5, 14, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			shields = 4;
			//TODO: Calls in external threat in current zone
			//TODO: Points
			//Killed before x: worth 8 + internal threat points
			//Killed after x: worth 8, internal threat worth normal points
			//Hits Z: worth 4, internal threat worth normal points
			throw new NotImplementedException();
		}

		protected override void PerformYAction(int currentTurn)
		{
			foreach (var threat in ThreatController.DamageableExternalThreats)
				threat.Repair(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			throw new LoseException(this);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.AffectedDistance.Contains(DistanceToShip);
		}

		public static string GetDisplayName()
		{
			return "Overlord";
		}
	}
}
