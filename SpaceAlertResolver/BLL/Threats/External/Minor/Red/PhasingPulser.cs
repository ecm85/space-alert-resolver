using BLL.Tracks;

namespace BLL.Threats.External.Minor.Red
{
	public class PhasingPulser : MinorRedExternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		public PhasingPulser()
			: base(1, 6, 2)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
			ThreatController.EndOfTurn += PerformEndOfTurn;
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackAllZones(wasPhasedAtStartOfTurn ? 0 : 1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackAllZones(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(wasPhasedAtStartOfTurn ? 2 : 3);
		}

		private void PerformBeforeMove()
		{
			isPhased = false;
		}

		private void PerformAfterMove()
		{
			isPhased = !wasPhasedAtStartOfTurn;
		}

		public override bool IsDamageable
		{
			get { return base.IsDamageable && !isPhased; }
		}

		public override bool IsMoveable
		{
			get { return base.IsDamageable && !isPhased; }
		}

		private void PerformEndOfTurn()
		{
			wasPhasedAtStartOfTurn = isPhased;
		}

		protected override void OnThreatTerminated()
		{
			BeforeMove -= PerformBeforeMove;
			AfterMove -= PerformAfterMove;
			ThreatController.EndOfTurn -= PerformEndOfTurn;
			base.OnThreatTerminated();
		}
	}
}
