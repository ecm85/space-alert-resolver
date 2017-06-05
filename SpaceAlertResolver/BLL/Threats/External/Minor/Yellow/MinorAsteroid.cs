using BLL.Common;
using BLL.Players;

namespace BLL.Threats.External.Minor.Yellow
{
	public class MinorAsteroid : MinorYellowExternalThreat
	{
		private int breakpointsCrossed;

		internal MinorAsteroid()
			: base(0, 7, 4)
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
			Attack(RemainingHealth);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			Attack(1 * breakpointsCrossed);
		}

		public override string Id { get; } = "E2-07";
		public override string DisplayName { get; } = "Minor Asteroid";
		public override string FileName { get; } = "MinorAsteroid";
	}
}
