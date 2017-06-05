namespace BLL.Threats.External.Minor.White
{
	public class ArmoredGrappler : MinorWhiteExternalThreat
	{
		internal ArmoredGrappler()
			: base(3, 4, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Repair(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(4);
		}

		public override string Id { get; } = "E1-08";
		public override string DisplayName { get; } = "Armored Grappler";
		public override string FileName { get; } = "ArmoredGrappler";
	}
}
