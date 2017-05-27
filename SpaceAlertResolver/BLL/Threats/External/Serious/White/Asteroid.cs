using BLL.Common;
using BLL.Players;

namespace BLL.Threats.External.Serious.White
{
	public class Asteroid : SeriousWhiteExternalThreat
	{
		private int breakpointsCrossed;

		internal Asteroid()
			: base(0, 9, 3)
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
			AttackCurrentZone(2 * breakpointsCrossed);
		}

		public override string Id { get; } = "SE1-08";
		public override string DisplayName { get; } = "Asteroid";
		public override string FileName { get; } = "Asteroid";
	}
}
