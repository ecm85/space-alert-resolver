namespace BLL.ShipComponents
{
    public abstract class UpperStation : StandardStation
    {
        protected abstract Shield Shield { get; }

        public override IBravoComponent BravoComponent => Shield;

        protected UpperStation(
            StationLocation stationLocation,
            ThreatController threatController,
            Gravolift gravolift,
            Doors bluewardDoors,
            Doors redwardDoors,
            SittingDuck sittingDuck) : base(stationLocation, threatController, gravolift, bluewardDoors, redwardDoors, sittingDuck)
        {
        }

        public int ShieldThroughAttack(int amount)
        {
            return Shield.ShieldThroughAttack(amount);
        }

        public override void DrainEnergy(int? amount)
        {
            DrainShield(amount);
        }

        public override void PerformEndOfTurn()
        {
            base.PerformEndOfTurn();
            Shield.PerformEndOfTurn();
        }

        public int DrainShield(int? amount)
        {
            return Shield.Drain(amount);
        }

        public void AddBonusShield(int amount)
        {
            Shield.BonusShield += amount;
        }

        public void FillToCapacity()
        {
            Shield.FillToCapacity(false);
        }

        public void SetIneffectiveShields(bool ineffectiveShields)
        {
            Shield.SetIneffectiveShields(ineffectiveShields);
        }

        public void SetReversedShields(bool reversedShields)
        {
            Shield.SetReversedShields(reversedShields);
        }
    }
}
