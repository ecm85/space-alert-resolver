using BLL.Common;
using BLL.Players;

namespace BLL.Threats.External.Serious.White
{
	public class InterstellarOctopus : SeriousWhiteExternalThreat
	{
		internal InterstellarOctopus()
			: base(1, 8, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			if (IsDamaged)
				AttackAllZones(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			if (IsDamaged)
				AttackAllZones(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(RemainingHealth * 2);
		}

		public override string Id { get; } = "SE1-06";
		public override string DisplayName { get; } = "Interstellar Octopus";
		public override string FileName { get; } = "InterstellarOctopus";

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
