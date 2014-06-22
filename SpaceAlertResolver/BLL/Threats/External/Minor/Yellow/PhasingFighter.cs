using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Tracks;

namespace BLL.Threats.External.Minor.Yellow
{
	public class PhasingFighter : MinorYellowExternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		public PhasingFighter()
			: base(2, 4, 3)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
			ThreatController.EndOfTurn += PerformEndOfTurn;
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 1 : 2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 2 : 3);
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

		public override bool IsMoveable
		{
			get { return base.IsDamageable && !isPhased; }
		}

		private void PerformEndOfTurn()
		{
			wasPhasedAtStartOfTurn = isPhased;
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
			return "Phasing Fighter";
		}
	}
}
