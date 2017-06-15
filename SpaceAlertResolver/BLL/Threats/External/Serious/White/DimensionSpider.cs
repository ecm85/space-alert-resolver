using System;
using BLL.Common;
using BLL.Players;
using BLL.Tracks;

namespace BLL.Threats.External.Serious.White
{
    public class DimensionSpider : SeriousWhiteExternalThreat
    {
        internal DimensionSpider()
            : base(0, 13, 1)
        {
        }

        public override void PlaceOnTrack(Track track, int trackPosition)
        {
            base.PlaceOnTrack(track, trackPosition);
            ThreatController.JumpingToHyperspace += OnJumpingToHyperspace;
        }

        protected override void PerformXAction(int currentTurn)
        {
            Shields = 1;
        }

        protected override void PerformYAction(int currentTurn)
        {
            Shields++;
        }

        protected override void PerformZAction(int currentTurn)
        {
            AttackAllZones(4);
        }

        private void OnJumpingToHyperspace(object sender, EventArgs args)
        {
            PerformZAction(-1);
            OnReachingEndOfTrack();
        }
        public override bool CanBeTargetedBy(PlayerDamage damage)
        {
            Check.ArgumentIsNotNull(damage, "damage");
            return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
        }

        protected override void OnThreatTerminated()
        {
            base.OnThreatTerminated();
            ThreatController.JumpingToHyperspace -= OnJumpingToHyperspace;
        }

        public override string Id { get; } = "SE1-102";
        public override string DisplayName { get; } = "Dimension Spider";
        public override string FileName { get; } = "DimensionSpider";
    }
}
