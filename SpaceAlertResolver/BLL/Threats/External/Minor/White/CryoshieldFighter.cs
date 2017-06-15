using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.Tracks;

namespace BLL.Threats.External.Minor.White
{
    public class CryoshieldFighter : MinorWhiteExternalThreat
    {
        private bool CryoshieldUp
        {
            get { return GetThreatStatus(ThreatStatus.Cryoshielded); }
            set { SetThreatStatus(ThreatStatus.Cryoshielded, value); }
        }

        internal CryoshieldFighter()
            : base(1, 4, 3)
        {
        }

        public override void PlaceOnTrack(Track track, int trackPosition)
        {
            base.PlaceOnTrack(track, trackPosition);
            CryoshieldUp = true;
        }

        protected override void PerformXAction(int currentTurn)
        {
            Attack(1);
        }

        protected override void PerformYAction(int currentTurn)
        {
            Attack(2);
        }

        protected override void PerformZAction(int currentTurn)
        {
            Attack(2);
        }

        public override string Id { get; } = "E1-06";
        public override string DisplayName { get; } = "Cryoshield Fighter";
        public override string FileName { get; } = "CryoshieldFighter";

        public override void TakeDamage(IList<PlayerDamage> damages)
        {
            if (CryoshieldUp && damages.Any())
                CryoshieldUp = false;
            else
                base.TakeDamage(damages);
        }
    }
}
