﻿using System.Linq;
using BLL.Common;
using BLL.Players;

namespace BLL.Threats.External.Serious.Red
{
    public class Overlord : SeriousRedExternalThreat, IThreatWithBonusThreat<ExternalThreat>
    {
        public ExternalThreat BonusThreat { get; set; }
        private bool calledInThreat;

        internal Overlord()
            : base(5, 14, 2)
        {
        }


        public override bool NeedsBonusExternalThreat { get { return true; } }

        protected override void PerformXAction(int currentTurn)
        {
            Shields = 4;
            CallInExternalThreat(currentTurn);
        }

        public override int PointsForDefeating
        {
            get
            {
                return 8 + (calledInThreat ? 0 : BonusThreat.PointsForDefeating);
            }
        }

        protected override int PointsForSurviving
        {
            get
            {
                return 4;
            }
        }

        private void CallInExternalThreat(int currentTurn)
        {
            BonusThreat.Initialize(SittingDuck, ThreatController, EventMaster);
            ThreatController.AddExternalThreat(BonusThreat, 1000 + currentTurn, CurrentZone);
            calledInThreat = true;
        }

        protected override void PerformYAction(int currentTurn)
        {
            foreach (var threat in ThreatController.DamageableExternalThreats)
                threat.Repair(1);
        }

        protected override void PerformZAction(int currentTurn)
        {
            throw new LoseException(this);
        }

        public override string Id { get; } = "SE3-105";
        public override string DisplayName { get; } = "Overlord";
        public override string FileName { get; } = "Overlord";

        public override bool CanBeTargetedBy(PlayerDamage damage)
        {
            Check.ArgumentIsNotNull(damage, "damage");
            return damage.AffectedDistances.Contains(DistanceToShip);
        }
    }
}
