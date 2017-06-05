namespace BLL.Threats.External.Serious.White
{
	public class Frigate : SeriousWhiteExternalThreat
	{
		internal Frigate()
			: base(2, 7, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(4);
		}

		public override string Id { get; } = "SE1-01";
		public override string DisplayName { get; } = "Frigate";
		public override string FileName { get; } = "Frigate";
	}
}
