using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public class CommandosB : Commandos
	{
		public CommandosB()
			: base(StationLocation.UpperBlue)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			if (IsDamaged)
				MoveRed();
			else
				Damage(2);
		}
	}
}
