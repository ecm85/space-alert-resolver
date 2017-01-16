namespace BLL.Threats.External.Serious.White
{
	public class Frigate : SeriousWhiteExternalThreat
	{
		public Frigate()
			: base(2, 7, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(4);
		}

		public override string Id { get; } = "SE1-01";
		public override string DisplayName { get; } = "Frigate";
		public override string FileName { get; } = "Frigate";
	}
}
