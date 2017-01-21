using System;
using BLL.Tracks;

namespace BLL.Threats.External.Serious.Yellow
{
	public class Nemesis : SeriousYellowExternalThreat
	{
		//TODO: Change this to actually track damage taken
		private int healthAtStartOfTurn;
		private bool TookDamageThisTurn => healthAtStartOfTurn > RemainingHealth;

		public Nemesis()
			: base(1, 9, 3)
		{
			healthAtStartOfTurn = RemainingHealth;
		}

		public override void PlaceOnBoard(Track track, int trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			ThreatController.DamageResolutionEnding += OnDamageResolutionEnding;
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

		private void OnDamageResolutionEnding(object sender, EventArgs args)
		{
			if (TookDamageThisTurn)
				AttackAllZones(1);
		}
		protected override void OnTurnEnded(object sender, EventArgs args)
		{
			base.OnTurnEnded(sender, args);
			healthAtStartOfTurn = RemainingHealth;
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			ThreatController.DamageResolutionEnding -= OnDamageResolutionEnding;
		}

		public override string Id { get; } = "SE2-05";
		public override string DisplayName { get; } = "Nemesis";
		public override string FileName { get; } = "Nemesis";
	}
}
