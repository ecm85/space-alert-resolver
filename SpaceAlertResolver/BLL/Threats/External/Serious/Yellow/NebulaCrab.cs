using BLL.Common;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.Yellow
{
	public class NebulaCrab : SeriousYellowExternalThreat
	{
		internal NebulaCrab()
			: base(2, 7, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Shields = 4;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Speed += 2;
			Shields = 2;
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackSpecificZones(5, new[] {ZoneLocation.Red, ZoneLocation.Blue});
		}

		public override string Id { get; } = "SE2-04";
		public override string DisplayName { get; } = "Nebula Crab";
		public override string FileName { get; } = "NebulaCrab";

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
