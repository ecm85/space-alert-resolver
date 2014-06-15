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
		public ISet<InternalThreat> Threats { get; private set; }
		public IList<Player> Players { get; private set; }
		public virtual EnergyContainer EnergyContainer { get; set; }
		public event Action<Player, int> MoveIn = (player, i) => { };
		public event Action<Player, int> MoveOut = (player, i) => { };
		public IList<IrreparableMalfunction> IrreparableMalfunctions { get; private set; }

		protected Station()
		{
			Players = new List<Player>();
			Threats = new HashSet<InternalThreat>();
			IrreparableMalfunctions = new List<IrreparableMalfunction>();
		}

		public abstract void PerformBAction(Player performingPlayer, int currentTurn, bool isHeroic);
		public abstract PlayerDamage PerformAAction(Player performingPlayer, int currentTurn, bool isHeroic);
		public abstract void PerformCAction(Player performingPlayer, int currentTurn);
		public abstract void UseBattleBots(Player performingPlayer, bool isHeroic);
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

		public virtual void UseInterceptors(Player performingPlayer, bool isHeroic)
		{
		}

		protected InternalThreat GetFirstThreatOfType(PlayerAction playerAction)
		{
			return Threats
				.Where(threat => threat.ActionType == playerAction)
				.OrderBy(threat => threat.TimeAppears)
				.FirstOrDefault();
		}

		protected void DamageThreat(InternalThreat threat, Player performingPlayer, bool isHeroic)
		{
			var damage = isHeroic ? 2 : 1;
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