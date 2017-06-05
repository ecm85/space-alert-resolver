namespace BLL.Threats.External.Minor.White
{
	public class PlasmaticFighter : MinorWhiteExternalThreat
	{
		internal PlasmaticFighter()
			: base(2, 4, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.Plasmatic);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.Plasmatic);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.Plasmatic);
		}

		public override string Id { get; } = "E1-101";
		public override string DisplayName { get; } = "Plasmatic Fighter";
		public override string FileName { get; } = "PlasmaticFighter";
	}
}
