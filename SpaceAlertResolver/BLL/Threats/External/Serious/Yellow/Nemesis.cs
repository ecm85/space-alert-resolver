using BLL.Tracks;

namespace BLL.Threats.External.Serious.Yellow
{
	public class Nemesis : SeriousYellowExternalThreat
	{
		private int healthAtStartOfTurn;
		private bool TookDamageThisTurn { get { return healthAtStartOfTurn > RemainingHealth; } }

		public Nemesis()
			: base(1, 9, 3)
		{
			healthAtStartOfTurn = RemainingHealth;
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			ThreatController.EndOfDamageResolutionEventHandler += HandleEndOfDamageResolution;
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
			TakeIrreducibleDamage(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2);
			TakeIrreducibleDamage(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			throw new LoseException(this);
		}

		private void HandleEndOfDamageResolution()
		{
			if (TookDamageThisTurn)
				AttackAllZones(1);
		}
		protected override void HandleEndOfTurn()
		{
			base.HandleEndOfTurn();
			healthAtStartOfTurn = RemainingHealth;
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			ThreatController.EndOfDamageResolutionEventHandler -= HandleEndOfDamageResolution;
		}
	}
}
