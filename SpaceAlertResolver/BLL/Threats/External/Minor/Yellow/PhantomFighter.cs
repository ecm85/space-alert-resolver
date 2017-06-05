using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.Tracks;

namespace BLL.Threats.External.Minor.Yellow
{
	public class PhantomFighter : MinorYellowExternalThreat
	{
		private bool PhantomMode
		{
			get { return GetThreatStatus(ThreatStatus.Stealthed); }
			set { SetThreatStatus(ThreatStatus.Stealthed, value); }
		}

		internal PhantomFighter()
			: base(3, 3, 3)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			PhantomMode = true;
		}

		protected override void PerformXAction(int currentTurn)
		{
			PhantomMode = false;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(3);
		}

		public override string Id { get; } = "E2-03";
		public override string DisplayName { get; } = "Phantom Fighter";
		public override string FileName { get; } = "PhantomFighter";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages.Where(damage => damage.PlayerDamageType != PlayerDamageType.Rocket).ToList());
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return !PhantomMode && base.CanBeTargetedBy(damage);
		}
	}
}
