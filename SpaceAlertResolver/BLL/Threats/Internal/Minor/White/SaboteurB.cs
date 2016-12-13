namespace BLL.Threats.Internal.Minor.White
{
	public class SaboteurB : Saboteur
	{
		protected override void PerformXAction(int currentTurn)
		{
			MoveBlue();
		}
	}
}
