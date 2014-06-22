using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Tracks;

namespace BLL.Threats.External.Serious.Red
{
	public class Planetoid : SeriousRedExternalThreat
	{
		private int breakpointsCrossed;

		public Planetoid()
			: base(0, 13, 1)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			ThreatController.EndOfTurn += IncreaseSpeed;
		}

		private void IncreaseSpeed()
		{
			Speed++;
		}

		protected override void PerformXAction(int currentTurn)
		{
			breakpointsCrossed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			breakpointsCrossed++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			breakpointsCrossed++;
		}

		public static string GetDisplayName()
		{
			return "Planetoid";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			Attack(4 * breakpointsCrossed);
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			ThreatController.EndOfTurn -= IncreaseSpeed;
		}
	}
}
