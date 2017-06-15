using BLL.Players;
using BLL.ShipComponents;
using BLL.Threats.External;

namespace BLL.Threats.Internal.Serious.Red
{
    public abstract class HiddenTransmitter : SeriousRedInternalThreat, IThreatWithBonusThreat<ExternalThreat>
    {
        public ExternalThreat BonusThreat { get; set; }
        private bool calledInThreat;
        protected HiddenTransmitter(StationLocation stationLocation)
            : base(3, 2, stationLocation, PlayerActionType.Charlie, 1)
        {
        }

        public override bool NeedsBonusExternalThreat { get { return true; } }

        protected override void PerformXAction(int currentTurn)
        {
            TotalInaccessibility = 0;
            CallInExternalThreat(currentTurn);
        }

        private void CallInExternalThreat(int currentTurn)
        {
            BonusThreat.Initialize(SittingDuck, ThreatController, EventMaster);
            ThreatController.AddExternalThreat(BonusThreat, 1000 + currentTurn, CurrentZone);
            calledInThreat = true;
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

        protected override void PerformYAction(int currentTurn)
        {
            ThreatController.MoveExternalThreatsInZone(currentTurn, 2, CurrentZone);
        }
    }
}
