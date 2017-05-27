using System.Collections.Generic;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public class Fissure : SeriousWhiteInternalThreat
	{
		internal Fissure()
			: base(2, 2, StationLocation.Interceptor1, PlayerActionType.BattleBots)
		{
		}

		public override IList<StationLocation> DisplayOnTrackStations { get;} = new List<StationLocation> {StationLocation.UpperRed};
		public override IList<StationLocation> DisplayOnShipStations { get; } = new List<StationLocation> { StationLocation.UpperRed };

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.AddZoneDebuff(new [] {ZoneLocation.Red}, ZoneDebuff.DoubleDamage, this);
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.AddZoneDebuff(EnumFactory.All<ZoneLocation>(), ZoneDebuff.DoubleDamage, this);
		}

		protected override void PerformZAction(int currentTurn)
		{
			throw new LoseException(this);
		}

		protected override void OnThreatTerminated()
		{
			SittingDuck.RemoveZoneDebuffForSource(EnumFactory.All<ZoneLocation>(), this);
			base.OnThreatTerminated();
		}

		public override string Id { get; } = "SI1-04";
		public override string DisplayName { get; } = "Fissure";
		public override string FileName { get; } = "Fissure";
	}
}
