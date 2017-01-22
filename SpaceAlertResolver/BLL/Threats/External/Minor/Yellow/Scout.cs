namespace BLL.Threats.External.Minor.Yellow
{
	public class Scout : MinorYellowExternalThreat
	{
		public Scout()
			: base(1, 3, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			ThreatController.AddExternalThreatEffect(ThreatStatus.BonusAttack);
		}

		protected override void PerformYAction(int currentTurn)
		{
			ThreatController.MoveOtherExternalThreats(currentTurn, 1, this);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(2, ThreatDamageType.IgnoresShields);
		}

		protected override void OnHealthReducedToZero()
		{
			ThreatController.RemoveExternalThreatEffect(ThreatStatus.BonusAttack);
			base.OnHealthReducedToZero();
		}

		public override string Id { get; } = "E2-02";
		public override string DisplayName { get; } = "Scout";
		public override string FileName { get; } = "Scout";

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.HeavyLaser && base.CanBeTargetedBy(damage);
		}
	}
}
