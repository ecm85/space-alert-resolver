using System.Collections.Generic;
using System.Linq;
using BLL.Tracks;

namespace BLL.Threats.External.Minor.White
{
	public class CryoshieldFighter : MinorWhiteExternalThreat
	{
		private bool cryoshieldUp;

		public CryoshieldFighter()
			: base(1, 4, 3)
		{
		}

		public override void PlaceOnBoard(Track track, int trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			cryoshieldUp = true;
			BuffCount++;
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		public override string Id { get; } = "E1-06";
		public override string DisplayName { get; } = "Cryoshield Fighter";
		public override string FileName { get; } = "CryoshieldFighter";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			if (cryoshieldUp && damages.Any())
			{
				cryoshieldUp = false;
				BuffCount--;
			}
			else
				base.TakeDamage(damages);
		}
	}
}
