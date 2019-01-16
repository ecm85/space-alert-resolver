namespace BLL.ShipComponents
{
    public class SideReactor : Reactor
    {
        private CentralReactor Source { get; set; }
        internal SideReactor(CentralReactor source) : base(3, 2)
        {
            Source = source;
        }

        public override void PerformBAction(bool isHeroic)
        {
            FillToCapacity(isHeroic);
        }

        public void FillToCapacity(bool isHeroic)
        {
            var energyToPull = Capacity - Energy;
            var oldSourceEnergy = Source.Energy;
            Source.Energy -= energyToPull;
            var newSourceEnergy = Source.Energy;
            var energyPulled = oldSourceEnergy - newSourceEnergy;
            Energy += energyPulled;
            if (energyPulled > 0 && isHeroic)
                Energy++;
        }
    }
}
