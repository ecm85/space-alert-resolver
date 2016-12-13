using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Tracks;

namespace BLL.Threats.External.Minor.Red
{
	public class EnergySnake : MinorRedExternalThreat
	{
		private int healthAtStartOfTurn;
		private bool TookDamageThisTurn => healthAtStartOfTurn > RemainingHealth;

		public EnergySnake()
			: base(3, 6, 4)
		{
			healthAtStartOfTurn = RemainingHealth;
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			ThreatController.DamageResolutionEnding += OnDamageResolutionEnding;
		}

		protected override void PerformXAction(int currentTurn)
		{
			var result = AttackCurrentZone(1);
			Repair(result.DamageShielded);
		}

		protected override void PerformYAction(int currentTurn)
		{
			var result = AttackCurrentZone(2);
			Repair(result.DamageShielded);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(1 + SittingDuck.GetEnergyInReactor(CurrentZone));
		}
		private void OnDamageResolutionEnding(object sender, EventArgs args)
		{
			if (TookDamageThisTurn)
				Speed -= 2;
		}

		protected override void OnTurnEnded(object sender, EventArgs args)
		{
			base.OnTurnEnded(sender, args);
			if (TookDamageThisTurn)
				Speed += 2;
			healthAtStartOfTurn = RemainingHealth;
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			var hitByPulse = damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Pulse);
			if (hitByPulse)
			{
				var oldShields = Shields;
				Shields = 0;
				base.TakeDamage(damages);
				Shields = oldShields;
			}
			else
				base.TakeDamage(damages);
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			ThreatController.DamageResolutionEnding -= OnDamageResolutionEnding;
		}
	}
}
