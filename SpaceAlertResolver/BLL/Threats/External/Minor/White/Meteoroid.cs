using BLL.Common;
using BLL.Players;

namespace BLL.Threats.External.Minor.White
{
    public class Meteoroid : MinorWhiteExternalThreat
    {
        internal Meteoroid()
            : base(0, 5, 5)
        {
        }

        protected override void PerformXAction(int currentTurn)
        {
        }

        protected override void PerformYAction(int currentTurn)
        {
        }

        protected override void PerformZAction(int currentTurn)
        {
            Attack(RemainingHealth);
        }

        public override string Id { get; } = "E1-10";
        public override string DisplayName { get; } = "Meteoroid";
        public override string FileName { get; } = "Meteoroid";

        public override bool CanBeTargetedBy(PlayerDamage damage)
        {
            Check.ArgumentIsNotNull(damage, "damage");
            return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
        }
    }
}
