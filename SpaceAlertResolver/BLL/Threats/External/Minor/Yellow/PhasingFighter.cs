using BLL.Tracks;

namespace BLL.Threats.External.Minor.Yellow
{
    public class PhasingFighter : MinorYellowExternalThreat
    {
        private PhasingThreatCore phasingThreatCore;

        internal PhasingFighter()
            : base(2, 4, 3)
        {
        }

        public override void PlaceOnTrack(Track track, int trackPosition)
        {
            base.PlaceOnTrack(track, trackPosition);
            phasingThreatCore = new PhasingThreatCore(this);
        }

        protected override void PerformXAction(int currentTurn)
        {
            Attack(1);
        }

        protected override void PerformYAction(int currentTurn)
        {
            Attack(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 1 : 2);
        }

        protected override void PerformZAction(int currentTurn)
        {
            Attack(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 2 : 3);
        }

        public override bool IsDamageable => base.IsDamageable && phasingThreatCore.IsDamageable;

        public override bool IsMoveable => base.IsDamageable && phasingThreatCore.IsDamageable;

        protected override void OnThreatTerminated()
        {
            phasingThreatCore.ThreatTerminated();
            base.OnThreatTerminated();
        }

        public override string Id { get; } = "E2-102";
        public override string DisplayName { get; } = "Phasing Fighter";
        public override string FileName { get; } = "PhasingFighter";
    }
}
