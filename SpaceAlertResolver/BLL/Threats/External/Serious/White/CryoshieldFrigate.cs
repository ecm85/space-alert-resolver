using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.Tracks;

namespace BLL.Threats.External.Serious.White
{
	public class CryoshieldFrigate : SeriousWhiteExternalThreat
	{
		private bool CryoshieldUp
		{
			get { return GetThreatStatus(ThreatStatus.Cryoshielded); }
			set { SetThreatStatus(ThreatStatus.Cryoshielded, value); }
		}

		internal CryoshieldFrigate()
			: base(1, 7, 2)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			CryoshieldUp = true;
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(4);
		}

		public override string Id { get; } = "SE1-05";
		public override string DisplayName { get; } = "Cryoshield Frigate";
		public override string FileName { get; } = "CryoshieldFrigate";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			if (CryoshieldUp && damages.Any())
				CryoshieldUp = false;
			else
				base.TakeDamage(damages);
		}
	}
}
