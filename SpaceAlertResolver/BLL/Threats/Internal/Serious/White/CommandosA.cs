using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public class CommandosA : Commandos
	{
		public CommandosA()
			: base(StationLocation.LowerRed)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			if (IsDamaged)
				MoveBlue();
			else
				Damage(2);
		}
	}
}
