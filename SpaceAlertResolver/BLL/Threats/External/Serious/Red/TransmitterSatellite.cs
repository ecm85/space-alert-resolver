using BLL.ShipComponents;
using BLL.Threats.Internal;

namespace BLL.Threats.External.Serious.Red
{
	public class TransmitterSatellite : SeriousRedExternalThreat, IThreatWithBonusThreat<InternalThreat>
	{
		public InternalThreat BonusThreat { get; set; }
		private bool calledInThreat;

		public TransmitterSatellite()
			: base(2, 5, 3)
		{
		}

		public override bool NeedsBonusInternalThreat { get { return true; } }

		protected override void PerformXAction(int currentTurn)
		{
			CallInInternalThreat(currentTurn);
		}

		protected override void PerformYAction(int currentTurn)
		{
			ThreatController.MoveInternalThreats(currentTurn, 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutCaptain();
			SittingDuck.ShiftPlayersAfterPlayerActions(EnumFactory.All<StationLocation>(), currentTurn + 1);
		}

		public override string Id { get; } = "SE3-104";
		public override string DisplayName { get; } = "Transmitter Satellite";
		public override string FileName { get; } = "TransmitterSatellite";

		public override int PointsForDefeating
		{
			get
			{
				return 8 + (calledInThreat ? 0 : BonusThreat.PointsForDefeating);
			}
		}

		protected override int PointsForSurviving
		{
			get
			{
				return 4;
			}
		}

		private void CallInInternalThreat(int currentTurn)
		{
			BonusThreat.Initialize(SittingDuck, ThreatController);
			ThreatController.AddInternalThreat(BonusThreat, 1000 + currentTurn);
			calledInThreat = true;
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return DistanceToShip != 3 && base.CanBeTargetedBy(damage);
		}
	}
}
