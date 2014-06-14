using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.Yellow
{
	public class PhasingFrigate : SeriousYellowExternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		public PhasingFrigate()
			: base(2, 7, 2)
		{
		}

		public override void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears, ZoneLocation currentZone)
		{
			base.Initialize(sittingDuck, threatController, timeAppears, currentZone);
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
			ThreatController.EndOfTurn += PerformEndOfTurn;
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 1 : 2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 2 : 3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 3 : 4);
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

		private void PerformEndOfTurn()
		{
			wasPhasedAtStartOfTurn = isPhased;
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return !isPhased && base.CanBeTargetedBy(damage);
		}

		protected override void OnHealthReducedToZero()
		{
			BeforeMove -= PerformBeforeMove;
			AfterMove -= PerformAfterMove;
			ThreatController.EndOfTurn -= PerformEndOfTurn;
			base.OnHealthReducedToZero();
		}

		protected override void OnReachingEndOfTrack()
		{
			BeforeMove -= PerformBeforeMove;
			AfterMove -= PerformAfterMove;
			ThreatController.EndOfTurn -= PerformEndOfTurn;
			base.OnReachingEndOfTrack();
		}

		public static string GetDisplayName()
		{
			return "Phasing Frigate";
		}
	}
}
