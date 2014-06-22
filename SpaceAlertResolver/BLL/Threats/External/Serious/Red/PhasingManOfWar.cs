using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Tracks;

namespace BLL.Threats.External.Serious.Red
{
	public class PhasingManOfWar : SeriousRedExternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		public PhasingManOfWar()
			: base(2, 9, 1)
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
			Attack(wasPhasedAtStartOfTurn ? 1 : 2);
			Speed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 2 : 3);
			shields++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 4 : 5);
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
			return "Phasing Man-Of-War";
		}
	}
}
