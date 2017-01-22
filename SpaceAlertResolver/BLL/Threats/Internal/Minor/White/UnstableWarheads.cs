using System;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.White
{
	public class UnstableWarheads : MinorWhiteInternalThreat
	{
		public UnstableWarheads()
			: base(3, 3, StationLocation.LowerBlue, PlayerActionType.Charlie)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			SetHealthToRemainingRockets(this, EventArgs.Empty);
			SittingDuck.RocketsModified += SetHealthToRemainingRockets;
		}

		private void SetHealthToRemainingRockets(object sender, EventArgs args)
		{
			//TODO: If this goes to 0, is the threat auto-defeated?
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

		public override string Id { get; } = "I1-07";
		public override string DisplayName { get; } = "Unstable Warheads";
		public override string FileName { get; } = "UnstableWarheads";
	}
}
