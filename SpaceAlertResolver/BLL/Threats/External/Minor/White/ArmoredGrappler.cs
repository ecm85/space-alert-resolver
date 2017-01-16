namespace BLL.Threats.External.Minor.White
{
	public class ArmoredGrappler : MinorWhiteExternalThreat
	{
		public ArmoredGrappler()
			: base(3, 4, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Repair(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(4);
		}

		public override string Id { get; } = "E1-08";
		public override string DisplayName { get; } = "Armored Grappler";
		public override string FileName { get; } = "ArmoredGrappler";
	}
}
