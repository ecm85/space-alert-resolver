using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class PhasingAnomaly : SeriousYellowInternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		private int numberOfYsCrossed;
		
		public PhasingAnomaly()
			: base(2, 3, StationLocation.UpperWhite, PlayerAction.C, 1)
		{
		}

		protected override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
		}

		public static string GetDisplayName()
		{
			return "Phasing Anomaly";
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.AddZoneDebuff(new [] {ZoneLocation.White}, ZoneDebuff.DisruptedOptics, this);
		}

		protected override void PerformYAction(int currentTurn)
		{
			switch (numberOfYsCrossed)
			{
				case 0:
					SittingDuck.AddZoneDebuff(new[] { ZoneLocation.Red }, ZoneDebuff.DisruptedOptics, this);
					break;
				case 1:
					SittingDuck.AddZoneDebuff(new[] { ZoneLocation.Blue }, ZoneDebuff.DisruptedOptics, this);
					break;
				default:
					throw new InvalidOperationException("Invalid number of Y's crossed.");
			}
			numberOfYsCrossed++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(new[] {StationLocation.LowerWhite, StationLocation.UpperWhite});
			Damage(3);
		}

		private void PerformBeforeMove()
		{
			isPhased = false;
		}

		private void PerformAfterMove()
		{
			isPhased = !wasPhasedAtStartOfTurn;
		}

		protected override void PerformEndOfTurn()
		{
			if (isPhased)
				wasPhasedAtStartOfTurn = isPhased;
			base.PerformEndOfTurn();
		}

		public override bool IsDamageable
		{
			get { return base.IsDamageable && !isPhased; }
		}

		protected override void OnHealthReducedToZero()
		{
			SittingDuck.RemoveZoneDebuffForSource(EnumFactory.All<ZoneLocation>(), this);
			base.OnHealthReducedToZero();
		}

		protected override void OnThreatTerminated()
		{
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
			base.OnThreatTerminated();
		}
	}
}
