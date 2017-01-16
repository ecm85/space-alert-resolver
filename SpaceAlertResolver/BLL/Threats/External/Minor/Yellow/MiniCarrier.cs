namespace BLL.Threats.External.Minor.Yellow
{
	public class MiniCarrier : MinorYellowExternalThreat
	{
		public MiniCarrier()
			: base(2, 5, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(2, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackOtherTwoZones(3, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(4, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}

		public override string Id { get; } = "E2-101";
		public override string DisplayName { get; } = "Mini-Carrier";
		public override string FileName { get; } = "MiniCarrier";
	}
}
