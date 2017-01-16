using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public class SkirmishersB : Skirmishers
	{
		public SkirmishersB()
			: base(StationLocation.UpperBlue)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			MoveRed();
		}

		public override string Id { get; } = "I1-02";
		public override string DisplayName { get; } = "Skirmishers";
		public override string FileName { get; } = "SkirmishersB";
	}
}
