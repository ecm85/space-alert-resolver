using System;
using BLL.Common;

namespace BLL.Threats.External.Serious.Red
{
	public class Planetoid : SeriousRedExternalThreat
	{
		private int breakpointsCrossed;

		public Planetoid()
			: base(0, 13, 1)
		{
		}

		protected override void OnTurnEnded(object sender, EventArgs args)
		{
			base.OnTurnEnded(sender, args);
			Speed++;
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
			AttackCurrentZone(4 * breakpointsCrossed);
		}

		public override string Id { get; } = "SE3-107";
		public override string DisplayName { get; } = "Planetoid";
		public override string FileName { get; } = "Planetoid";
	}
}
