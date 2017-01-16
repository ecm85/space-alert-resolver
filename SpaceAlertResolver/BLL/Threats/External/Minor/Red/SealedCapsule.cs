using BLL.Threats.Internal;

namespace BLL.Threats.External.Minor.Red
{
	public class SealedCapsule : MinorRedExternalThreat, IThreatWithBonusThreat<InternalThreat>
	{
		private InternalThreat BonusThreat { get; set; }

		public SealedCapsule()
			: base(4, 4, 4)
		{
		}

		public void SetBonusThreat(InternalThreat bonusThreat)
		{
			BonusThreat = bonusThreat;
		}

		public override bool NeedsBonusInternalThreat { get { return true; } }

		protected override void PerformXAction(int currentTurn)
		{
			Speed++;
			Shields = 3;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Speed++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			CallInInternalThreat(currentTurn);
			var newthreatGetsExtraSpeed = TotalHealth - RemainingHealth < 2;
			if (newthreatGetsExtraSpeed)
				BonusThreat.Speed++;
		}

		public override string Id { get; } = "E3-107";
		public override string DisplayName { get; } = "Sealed Capsule";
		public override string FileName { get; } = "SealedCapsule";

		private void CallInInternalThreat(int currentTurn)
		{
			ThreatController.AddInternalThreat(BonusThreat, 1000 + currentTurn);
		}

		public override int PointsForDefeating
		{
			get
			{
				return BonusThreat.PointsForDefeating;
			}
		}

		protected override int PointsForSurviving
		{
			get
			{
				return 0;
			}
		}
	}
}
