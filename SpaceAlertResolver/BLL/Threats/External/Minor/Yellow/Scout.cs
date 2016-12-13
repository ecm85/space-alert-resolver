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
			ThreatController.AddExternalThreatEffect(ExternalThreatEffect.BonusAttack, this);
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
			ThreatController.RemoveExternalThreatEffectForSource(this);
			base.OnHealthReducedToZero();
		}
		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.HeavyLaser && base.CanBeTargetedBy(damage);
		}
	}
}
