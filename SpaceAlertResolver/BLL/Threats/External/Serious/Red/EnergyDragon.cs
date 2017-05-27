using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.External.Serious.Red
{
	public class EnergyDragon : SeriousRedExternalThreat
	{
		private int healthAtStartOfTurn;
		private bool TookDamageThisTurn { get { return healthAtStartOfTurn > RemainingHealth; } }

		internal EnergyDragon()
			: base(3, 9, 4)
		{
			healthAtStartOfTurn = RemainingHealth;
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			ThreatController.DamageResolutionEnding += OnDamageResolutionEnding;
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

		public override string Id { get; } = "SE3-108";
		public override string DisplayName { get; } = "Energy Dragon";
		public override string FileName { get; } = "EnergyDragon";
	}
}
