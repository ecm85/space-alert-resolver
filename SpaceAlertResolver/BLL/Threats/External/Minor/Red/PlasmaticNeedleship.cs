using BLL.Common;
using BLL.Players;

namespace BLL.Threats.External.Minor.Red
{
	public class PlasmaticNeedleship : MinorRedExternalThreat
	{
		internal PlasmaticNeedleship()
			: base(1, 3, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.Plasmatic);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.Plasmatic);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(4, ThreatDamageType.Plasmatic);
		}

		public override string Id { get; } = "E3-101";
		public override string DisplayName { get; } = "Plasmatic Needleship";
		public override string FileName { get; } = "PlasmaticNeedleship";

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.HeavyLaser && base.CanBeTargetedBy(damage);
		}
	}
}
