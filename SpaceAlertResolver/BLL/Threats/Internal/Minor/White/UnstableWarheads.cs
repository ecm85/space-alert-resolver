using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			SetHealthToRemainingRockets();
			SittingDuck.RocketsModified += SetHealthToRemainingRockets;
		}

		private void SetHealthToRemainingRockets()
		{
			RemainingHealth = SittingDuck.GetRocketCount();
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

		public static string GetDisplayName()
		{
			return "Unstable Warheads";
		}
	}
}
