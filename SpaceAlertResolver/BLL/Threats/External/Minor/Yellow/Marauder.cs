using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Yellow
{
	public class Marauder : MinorYellowExternalThreat
	{
		public Marauder()
			: base(1, 6, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			ThreatController.AddExternalThreatEffect(ThreatStatus.BonusShield);
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.DrainShields(EnumFactory.All<ZoneLocation>(), 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(4);
		}

		protected override void OnHealthReducedToZero()
		{
			ThreatController.RemoveExternalThreatEffect(ThreatStatus.BonusShield);
			base.OnHealthReducedToZero();
		}

		public override string Id { get; } = "E2-06";
		public override string DisplayName { get; } = "Marauder";
		public override string FileName { get; } = "Marauder";
	}
}
