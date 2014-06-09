using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class OverheatedReactor : MinorYellowInternalThreat
	{
		public OverheatedReactor()
			: base(3, 2, StationLocation.LowerWhite, PlayerAction.B)
		{
		}

		public static string GetDisplayName()
		{
			return "Overheated Reactor";
		}

		public override void PerformXAction()
		{
			Damage(SittingDuck.GetEnergyInStation(CurrentStation));
		}

		public override void PerformYAction()
		{
			SittingDuck.DrainReactors(CurrentZones, 1);
		}

		public override void PerformZAction()
		{
			Damage(3);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			SittingDuck.KnockOutPlayers(new [] {StationLocation.LowerBlue, StationLocation.LowerRed});
		}
	}
}
