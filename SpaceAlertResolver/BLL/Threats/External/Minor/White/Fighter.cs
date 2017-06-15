namespace BLL.Threats.External.Minor.White
{
    public class Fighter : MinorWhiteExternalThreat
    {
        internal Fighter()
            : base(2, 4, 3)
        {
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
            Attack(3);
        }

        public override string Id { get; } = "E1-07";
        public override string DisplayName { get; } = "Fighter";
        public override string FileName { get; } = "Fighter";
    }
}
