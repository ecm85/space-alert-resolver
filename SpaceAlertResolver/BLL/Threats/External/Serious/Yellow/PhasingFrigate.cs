using BLL.Tracks;

namespace BLL.Threats.External.Serious.Yellow
{
    public class PhasingFrigate : SeriousYellowExternalThreat
    {
        private PhasingThreatCore phasingThreatCore;

        internal PhasingFrigate()
            : base(2, 7, 2)
        {
        }

        public override void PlaceOnTrack(Track track, int trackPosition)
        {
            base.PlaceOnTrack(track, trackPosition);
            phasingThreatCore = new PhasingThreatCore(this);
        }

        protected override void PerformXAction(int currentTurn)
        {
            Attack(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 1 : 2);
        }

        protected override void PerformYAction(int currentTurn)
        {
            Attack(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 2 : 3);
        }

        protected override void PerformZAction(int currentTurn)
        {
            Attack(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 3 : 4);
        }

        public override bool IsDamageable => base.IsDamageable && phasingThreatCore.IsDamageable;

        public override bool IsMoveable => base.IsDamageable && phasingThreatCore.IsDamageable;

        protected override void OnThreatTerminated()
        {
            phasingThreatCore.ThreatTerminated();
            base.OnThreatTerminated();
        }

        public override string Id { get; } = "SE2-102";
        public override string DisplayName { get; } = "Phasing Frigate";
        public override string FileName { get; } = "PhasingFrigate";
    }
}
