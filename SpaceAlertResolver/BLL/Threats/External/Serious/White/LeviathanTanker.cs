namespace BLL.Threats.External.Serious.White
{
	public class LeviathanTanker : SeriousWhiteExternalThreat
	{
		internal LeviathanTanker()
			: base(3, 8, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2);
			Repair(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			foreach (var threat in ThreatController.DamageableExternalThreats)
				threat.TakeIrreducibleDamage(1);
		}

		public override string Id { get; } = "SE1-03";
		public override string DisplayName { get; } = "Leviathan Tanker";
		public override string FileName { get; } = "LeviathanTanker";
	}
}
