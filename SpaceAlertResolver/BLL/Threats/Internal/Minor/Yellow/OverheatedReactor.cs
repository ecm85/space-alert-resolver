using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class OverheatedReactor : MinorYellowInternalThreat
	{
		public OverheatedReactor()
			: base(3, 2, StationLocation.LowerWhite, PlayerActionType.B)
		{
		}
		protected override void PerformXAction(int currentTurn)
		{
			Damage(SittingDuck.GetEnergyInReactor(CurrentZone));
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.DestroyFuelCapsule();
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
