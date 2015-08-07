using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.External.Serious.Red
{
	public class EnergyDragon : SeriousRedExternalThreat
	{
		private int healthAtStartOfTurn;
		private bool TookDamageThisTurn { get { return healthAtStartOfTurn > RemainingHealth; } }

		public EnergyDragon()
			: base(3, 9, 4)
		{
			healthAtStartOfTurn = RemainingHealth;
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			ThreatController.EndOfDamageResolution += PerformEndOfDamageResolution;
			ThreatController.EndOfTurn += PerformEndOfTurn;
		}

		protected override void PerformXAction(int currentTurn)
		{
			var result = AttackCurrentZone(2);
			Repair(result.DamageShielded);
		}

		protected override void PerformYAction(int currentTurn)
		{
			var result = AttackCurrentZone(3);
			Repair(result.DamageShielded);
		}

		protected override void PerformZAction(int currentTurn)
		{
			foreach(var zone in EnumFactory.All<ZoneLocation>())
				AttackCurrentZone(1 + SittingDuck.GetEnergyInReactor(zone));
		}
		private void PerformEndOfDamageResolution()
		{
			if (TookDamageThisTurn)
				Speed -= 2;
		}

		private void PerformEndOfTurn()
		{
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
			ThreatController.EndOfDamageResolution -= PerformEndOfDamageResolution;
			ThreatController.EndOfTurn -= PerformEndOfTurn;
		}
	}
}
