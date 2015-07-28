using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			ThreatController.EndOfDamageResolution += PerformEndOfDamageResolution;
			ThreatController.EndOfTurn += PerformEndOfTurn;
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
			if (TookDamageThisTurn)
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

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			ThreatController.EndOfDamageResolution -= PerformEndOfDamageResolution;
			ThreatController.EndOfTurn -= PerformEndOfTurn;
		}

		public static string GetId()
		{
			return "SE2-05";
		}
	}
}
