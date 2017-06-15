namespace BLL.Threats.External.Minor.White
{
    public class PulseBall : MinorWhiteExternalThreat
    {
        internal PulseBall()
            : base(1, 5, 2)
        {
        }

        protected override void PerformXAction(int currentTurn)
        {
            AttackAllZones(1);
        }

        protected override void PerformYAction(int currentTurn)
        {
            AttackAllZones(2);
        }

        protected override void PerformZAction(int currentTurn)
        {
            AttackAllZones(2);
        }

        public override string Id { get; } = "E1-01";
        public override string DisplayName { get; } = "Pulse Ball";
        public override string FileName { get; } = "PulseBall";
    }
}
