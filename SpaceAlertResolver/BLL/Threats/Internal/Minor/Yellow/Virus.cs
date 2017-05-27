using System.Linq;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Virus : MinorYellowInternalThreat
	{
		internal Virus()
			: base(3, 3, StationLocation.UpperWhite, PlayerActionType.Charlie)
		{
		}
		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainAllReactors(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.ShiftPlayersAfterPlayerActions(EnumFactory.All<StationLocation>().Where(stationLocation => stationLocation.IsOnShip()), currentTurn + 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			DamageAllZones(1);
		}

		public override string Id { get; } = "I2-05";
		public override string DisplayName { get; } = "Virus";
		public override string FileName { get; } = "Virus";
	}
}
