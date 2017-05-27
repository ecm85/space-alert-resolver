using BLL.Players;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.White
{
	public class UnstableWarheads : MinorWhiteInternalThreat
	{
		internal UnstableWarheads()
			: base(3, 3, StationLocation.LowerBlue, PlayerActionType.Charlie)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			RemainingHealth = SittingDuck.RocketCount;
			SittingDuck.RocketsModified += UpdateHealthFromRemovedRockets;
		}

		private void UpdateHealthFromRemovedRockets(object sender, RocketsRemovedEventArgs args)
		{
			RemainingHealth -= args.RocketsRemovedCount;
			CheckDefeated();
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
			SittingDuck.RocketsModified -= UpdateHealthFromRemovedRockets;
			base.OnThreatTerminated();
		}

		public override string Id { get; } = "I1-07";
		public override string DisplayName { get; } = "Unstable Warheads";
		public override string FileName { get; } = "UnstableWarheads";
	}
}
