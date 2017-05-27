using BLL.Common;
using BLL.Players;

namespace BLL.Threats.External.Minor.White
{
	public class Amoeba : MinorWhiteExternalThreat
	{
		internal Amoeba()
			: base(0, 8, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Repair(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Repair(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(5);
		}

		public override string Id { get; } = "E1-09";
		public override string DisplayName { get; } = "Amoeba";
		public override string FileName { get; } = "Amoeba";

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
