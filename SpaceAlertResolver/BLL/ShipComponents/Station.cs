using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public abstract class Station
	{
		public StationLocation StationLocation { get; private set; }
		public IList<Player> Players { get; private set; }
		//TODO: change moveIn and moveOut to be EventHandler<MoveEventArgs>
		public event Action<Player, int> MoveIn = (performingPlayer, currentTurn) => { };
		public event Action<Player, int> MoveOut = (performingPlayer, currentTurn) => { };
		public IList<IrreparableMalfunction> IrreparableMalfunctions { get; private set; }
		protected ThreatController ThreatController { get; private set; }

		protected Station(StationLocation stationLocation, ThreatController threatController)
		{
			Players = new List<Player>();
			IrreparableMalfunctions = new List<IrreparableMalfunction>();
			StationLocation = stationLocation;
			ThreatController = threatController;
		}

		public abstract void PerformMoveIn(Player performingPlayer, int currentTurn);

		protected InternalThreat GetFirstThreatOfType(PlayerActionType playerActionType, Player performingPlayer)
		{
			return ThreatController.DamageableInternalThreats
				.Where(threat => threat.CanBeTargetedBy(StationLocation, playerActionType, performingPlayer))
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

		public abstract void PerformPlayerAction(Player performingPlayer, int currentTurn);
	}
}