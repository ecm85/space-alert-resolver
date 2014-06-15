using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Red
{
	public class PhasingDestroyer : MinorRedExternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		public PhasingDestroyer()
			: base(2, 5, 2)
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
			Attack(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 1 : 2, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 2 : 3, ThreatDamageType.DoubleDamageThroughShields);
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

		protected override void OnThreatTerminated()
		{
			BeforeMove -= PerformBeforeMove;
			AfterMove -= PerformAfterMove;
			ThreatController.EndOfTurn -= PerformEndOfTurn;
			base.OnThreatTerminated();
		}

		public static string GetDisplayName()
		{
			return "Phasing Destroyer";
		}
	}
}
