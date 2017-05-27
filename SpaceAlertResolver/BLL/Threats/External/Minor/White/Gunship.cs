namespace BLL.Threats.External.Minor.White
{
	public class Gunship : MinorWhiteExternalThreat
	{
		internal Gunship()
			: base(2, 5, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(3);
		}

		public override string Id { get; } = "E1-05";
		public override string DisplayName { get; } = "Gunship";
		public override string FileName { get; } = "Gunship";
	}
}
