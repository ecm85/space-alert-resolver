using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public abstract class Station
	{
		public CComponent CComponent { get; set; }
		public StationLocation StationLocation { get; set; }
		public IList<Player> Players { get; private set; }
		public event Action<Player, int> MoveIn = (performingPlayer, currentTurn) => { };
		public event Action<Player, int> MoveOut = (performingPlayer, currentTurn) => { };
		public IList<IrreparableMalfunction> IrreparableMalfunctions { get; private set; }
		public ThreatController ThreatController { get; set; }

		protected Station()
		{
			Players = new List<Player>();
			IrreparableMalfunctions = new List<IrreparableMalfunction>();
		}

		public abstract void PerformBAction(Player performingPlayer, int currentTurn, bool isHeroic);
		public abstract PlayerDamage PerformAAction(Player performingPlayer, int currentTurn, bool isHeroic);
		public abstract void PerformCAction(Player performingPlayer, int currentTurn);
		public abstract void UseBattleBots(Player performingPlayer, int currentTurn, bool isHeroic);
		public abstract bool PerformMoveOutTowardsRed(Player performingPlayer, int currentTurn);
		public abstract bool PerformMoveOutTowardsOppositeDeck(Player performingPlayer, int currentTurn, bool isHeroic);
		public abstract bool PerformMoveOutTowardsBlue(Player performingPlayer, int currentTurn);
		public abstract void PerformMoveIn(Player performingPlayer, int currentTurn);
		public abstract bool CanMoveOutTowardsRed();
		public abstract bool CanMoveOutTowardsOppositeDeck();
		public abstract bool CanMoveOutTowardsBlue();

		public virtual void PerformNoAction(Player performingPlayer, int currentTurn)
		{
		}

		protected InternalThreat GetFirstThreatOfType(PlayerAction playerAction)
		{
			return ThreatController.DamageableInternalThreats
				.Where(threat => threat.ActionType == playerAction && threat.CurrentStations.Contains(StationLocation))
				.OrderBy(threat => threat.TimeAppears)
				.FirstOrDefault();
		}

		protected void DamageThreat(int damage, InternalThreat threat, Player performingPlayer, bool isHeroic)
		{
			threat.TakeDamage(damage, performingPlayer, isHeroic, StationLocation);
		}

		protected void OnMoveIn(Player performingPlayer, int currentTurn)
		{
			MoveIn(performingPlayer, currentTurn);
		}

		protected void OnMoveOut(Player performingPlayer, int currentTurn)
		{
			MoveOut(performingPlayer, currentTurn);
		}
	}
}