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

		public override string Id { get; } = "SI1-01";
		public override string DisplayName { get; } = "Commandos";
		public override string FileName { get; } = "CommandosA";
	}
}
