namespace BLL.Threats.Internal.Minor.White
{
    public class SaboteurA : Saboteur
    {
        protected override void PerformXAction(int currentTurn)
        {
            MoveRed();
        }

        public override string Id { get; } = "I1-04";
        public override string DisplayName { get; } = "Saboteur";
        public override string FileName { get; } = "SaboteurA";
    }
}
