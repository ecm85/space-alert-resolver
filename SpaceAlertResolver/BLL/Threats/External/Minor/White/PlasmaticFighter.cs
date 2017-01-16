namespace BLL.Threats.External.Minor.White
{
	public class PlasmaticFighter : MinorWhiteExternalThreat
	{
		public PlasmaticFighter()
			: base(2, 4, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1, ThreatDamageType.Plasmatic);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(1, ThreatDamageType.Plasmatic);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(2, ThreatDamageType.Plasmatic);
		}

		public override string Id { get; } = "E1-101";
		public override string DisplayName { get; } = "Plasmatic Fighter";
		public override string FileName { get; } = "PlasmaticFighter";
	}
}
