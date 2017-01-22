using BLL.Tracks;

namespace BLL.Threats.External.Minor.White
{
	public class StealthFighter : MinorWhiteExternalThreat
	{
		private bool Stealthed
		{
			get { return GetThreatStatus(ThreatStatus.Stealthed); }
			set { SetThreatStatus(ThreatStatus.Stealthed, value); }
		}

		public StealthFighter()
			: base(2, 4, 3)
		{
		}

		public override void PlaceOnBoard(Track track, int trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			Stealthed = true;
		}

		protected override void PerformXAction(int currentTurn)
		{
			Stealthed = false;
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		public override string Id { get; } = "E1-03";
		public override string DisplayName { get; } = "Stealth Fighter";
		public override string FileName { get; } = "StealthFighter";

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return !Stealthed && base.CanBeTargetedBy(damage);
		}
	}
}
