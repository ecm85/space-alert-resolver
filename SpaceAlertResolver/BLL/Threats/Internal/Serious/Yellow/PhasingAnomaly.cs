using System;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class PhasingAnomaly : SeriousYellowInternalThreat
	{
		private PhasingThreatCore phasingThreatCore;

		private int numberOfYsCrossed;

		public PhasingAnomaly()
			: base(2, 3, StationLocation.UpperWhite, PlayerActionType.Charlie, 1)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			phasingThreatCore = new PhasingThreatCore(this);
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

		public override bool IsDamageable => base.IsDamageable && phasingThreatCore.IsDamageable;

		public override bool IsMoveable => base.IsDamageable && phasingThreatCore.IsDamageable;

		protected override void OnHealthReducedToZero()
		{
			SittingDuck.RemoveZoneDebuffForSource(EnumFactory.All<ZoneLocation>(), this);
			base.OnHealthReducedToZero();
		}

		protected override void OnThreatTerminated()
		{
			phasingThreatCore.ThreatTerminated();
			base.OnThreatTerminated();
		}

		public override string Id { get; } = "SI2-101";
		public override string DisplayName { get; } = "Phasing Anomaly";
		public override string FileName { get; } = "PhasingAnomaly";
	}
}
