using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class NuclearDevice : SeriousYellowInternalThreat
	{
		public NuclearDevice()
			: base(1, 4, StationLocation.LowerWhite, PlayerActionType.C, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Speed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Speed++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			throw new LoseException(this);
		}
	}
}
