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

		protected override void PerformXAction(int currentTurn)
		{
			Damage(SittingDuck.GetEnergyInStation(CurrentStation));
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.DrainReactors(CurrentZones, 1);
		}

		protected override void PerformZAction(int currentTurn)
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
