namespace BLL.Threats.Internal.Minor.White
{
	public class SaboteurB : Saboteur
	{
		protected override void PerformXAction(int currentTurn)
		{
			MoveBlue();
		}

		public override string Id { get; } = "I1-03";
		public override string DisplayName { get; } = "Saboteur";
		public override string FileName { get; } = "SaboteurB";
	}
}
