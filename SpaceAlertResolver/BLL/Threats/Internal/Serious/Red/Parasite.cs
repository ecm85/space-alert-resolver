using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class Parasite : SeriousRedInternalThreat
	{
		private Player attachedPlayer;

		public Parasite()
			: base(1, 2, new List<StationLocation>(), PlayerAction.BattleBots)
		{
		}

		//TODO: Set Player to first player that moves

		protected override void PerformXAction(int currentTurn)
		{
			if (attachedPlayer != null && !attachedPlayer.IsKnockedOut && attachedPlayer.Interceptors != null)
				SittingDuck.DrainEnergyContainer(attachedPlayer.CurrentStation.StationLocation, 1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			//TODO: Knocks out other players in station
		}

		protected override void PerformZAction(int currentTurn)
		{
			if (attachedPlayer != null)
			{
				if (attachedPlayer.Interceptors != null)
					SittingDuck.TakeAttack(new ThreatDamage(5, ThreatDamageType.Standard, new[] {ZoneLocation.White}));
				else
					Damage(5);
			}
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if(IsDefeated)
				attachedPlayer.IsKnockedOut = true;
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}

		public override bool CanBeTargetedBy(StationLocation stationLocation, PlayerAction playerAction, Player performingPlayer)
		{
			return ActionType == playerAction && CurrentStations.Contains(performingPlayer.CurrentStation.StationLocation) && performingPlayer != attachedPlayer;
		}

		public static string GetDisplayName()
		{
			return "Cyber Gremlin";
		}
	}
}
