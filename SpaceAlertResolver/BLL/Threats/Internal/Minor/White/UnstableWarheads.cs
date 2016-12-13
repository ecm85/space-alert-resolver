using System;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.White
{
	public class UnstableWarheads : MinorWhiteInternalThreat
	{
		public UnstableWarheads()
			: base(3, 3, StationLocation.LowerBlue, PlayerActionType.C)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			SetHealthToRemainingRockets(this, EventArgs.Empty);
			SittingDuck.RocketsModified += SetHealthToRemainingRockets;
		}

		private void SetHealthToRemainingRockets(object sender, EventArgs args)
		{
			RemainingHealth = SittingDuck.RocketCount;
		}

		protected override void PerformXAction(int currentTurn)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(RemainingHealth * 3);
		}

		protected override void OnThreatTerminated()
		{
			SittingDuck.RocketsModified -= SetHealthToRemainingRockets;
			base.OnThreatTerminated();
		}
	}
}
