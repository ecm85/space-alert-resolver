using BLL.Common;

namespace BLL.Threats.External.Serious.Yellow
{
	public class MajorAsteroid : SeriousYellowExternalThreat
	{
		private int breakpointsCrossed;

		public MajorAsteroid()
			: base(0, 11, 2)
		{
		}
		protected override void PerformXAction(int currentTurn)
		{
			breakpointsCrossed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			breakpointsCrossed++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(RemainingHealth);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			AttackCurrentZone(3 * breakpointsCrossed);
		}

		public override string Id { get; } = "SE2-06";
		public override string DisplayName { get; } = "Major Asteroid";
		public override string FileName { get; } = "MajorAsteroid";
	}
}
