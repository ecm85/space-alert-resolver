using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.Red
{
	public class CyberGremlin : SeriousRedInternalThreat
	{
		public CyberGremlin()
			: base(1, 2, StationLocation.UpperRed, PlayerAction.BattleBots, 1)
		{
		}

		protected override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			ThreatController.JumpingToHyperspace += OnJumpingToHyperspace;
		}

		private void OnJumpingToHyperspace()
		{
			SittingDuck.KnockOutPlayers(EnumFactory.All<StationLocation>());
		}

		protected override void PerformXAction(int currentTurn)
		{
			var energyDrained = SittingDuck.DrainReactor(CurrentZone);
			Speed += energyDrained;
		}

		protected override void PerformYAction(int currentTurn)
		{
			SabotageAllSystems();
			MoveBlue();
		}

		protected override void PerformZAction(int currentTurn)
		{
			SabotageAllSystems();
		}

		private void SabotageAllSystems()
		{
			throw new NotImplementedException();
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnThreatTerminated();
			ThreatController.JumpingToHyperspace -= OnJumpingToHyperspace;
		}

		public static string GetDisplayName()
		{
			return "Cyber Gremlin";
		}
	}
}
