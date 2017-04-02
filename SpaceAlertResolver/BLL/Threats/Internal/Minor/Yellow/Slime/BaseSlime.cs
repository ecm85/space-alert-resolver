using System;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.Yellow.Slime
{
	public abstract class BaseSlime : MinorYellowInternalThreat
	{
		protected BaseSlime(int health, StationLocation currentStation)
			: base(health, 2, currentStation, PlayerActionType.BattleBots)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(2);
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			SittingDuck.SubscribeToMovingIn(CurrentStations, DelayPlayer);
		}

		private static void DelayPlayer(object sender, PlayerMoveEventArgs args)
		{
			if (!args.CurrentTurn.HasValue)
				throw new InvalidOperationException("Tried to delay player but didn't specify turn to do so.");
			args.MovingPlayer.Shift(args.CurrentTurn.Value + 1);
		}

		protected override void OnHealthReducedToZero()
		{
			OnThreatTerminated();
			SittingDuck.UnsubscribeFromMovingIn(CurrentStations, DelayPlayer);
		}
	}
}
