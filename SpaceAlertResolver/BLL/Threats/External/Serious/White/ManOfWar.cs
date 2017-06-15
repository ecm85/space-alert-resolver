namespace BLL.Threats.External.Serious.White
{
    public class ManOfWar : SeriousWhiteExternalThreat
    {
        internal ManOfWar()
            : base(2, 9, 1)
        {
        }

        protected override void PerformXAction(int currentTurn)
        {
            Attack(2);
            Speed++;
        }

        protected override void PerformYAction(int currentTurn)
        {
            Attack(3);
            Shields++;
        }

        protected override void PerformZAction(int currentTurn)
        {
            Attack(3);
        }

        public override string Id { get; } = "SE1-02";
        public override string DisplayName { get; } = "Man-Of-War";
        public override string FileName { get; } = "ManOfWar";
    }
}
