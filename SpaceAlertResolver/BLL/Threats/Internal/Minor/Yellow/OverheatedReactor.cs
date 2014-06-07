using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class OverheatedReactor : MinorYellowInternalThreat
	{
		public OverheatedReactor(int timeAppears, ISittingDuck sittingDuck)
			: base(3, 2, timeAppears, StationLocation.LowerWhite, PlayerAction.B, sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Overheated Reactor";
		}

		public override void PeformXAction()
		{
			Damage(sittingDuck.StationByLocation[CurrentStation].EnergyContainer.Energy);
		}

		public override void PerformYAction()
		{
			sittingDuck.DrainReactors(new[] {ZoneLocation.White}, 1);
		}

		public override void PerformZAction()
		{
			Damage(3);
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			sittingDuck.KnockOutPlayers(new [] {StationLocation.LowerBlue, StationLocation.LowerRed});
		}
	}
}
