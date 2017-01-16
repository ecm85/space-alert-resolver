namespace BLL.Threats.External.Serious.White
{
	public class SpacecraftCarrier : SeriousWhiteExternalThreat
	{
		public SpacecraftCarrier()
			: base(3, 6, 2)
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

		public override string Id { get; } = "SE1-101";
		public override string DisplayName { get; } = "Spacecraft Carrier";
		public override string FileName { get; } = "SpacecraftCarrier";
	}
}
