using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.Yellow
{
	public class Nemesis : SeriousYellowExternalThreat
	{
		private int healthAtStartOfTurn;

		public Nemesis()
			: base(1, 9, 3)
		{
			healthAtStartOfTurn = RemainingHealth;
		}

		public override void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears, ZoneLocation currentZone)
		{
			ThreatController.EndOfDamageResolution += PerformEndOfDamageResolution;
			threatController.EndOfTurn += PerformEndOfTurn;
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1);
			TakeIrreducibleDamage(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2);
			TakeIrreducibleDamage(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			throw new LoseException(this);
		}

		private void PerformEndOfDamageResolution()
		{
			if (healthAtStartOfTurn > RemainingHealth)
				AttackAllZones(1);
		}

		public static string GetDisplayName()
		{
			return "Nemesis";
		}

		private void PerformEndOfTurn()
		{
			healthAtStartOfTurn = RemainingHealth;
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			ThreatController.EndOfDamageResolution += PerformEndOfDamageResolution;
			ThreatController.EndOfTurn += PerformEndOfTurn;
		}
	}
}
