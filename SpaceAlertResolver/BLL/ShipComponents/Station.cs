using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public abstract class Station
	{
		public StationLocation StationLocation { get; }
		public IList<Player> Players { get; private set; }
		public event EventHandler<PlayerMoveEventArgs> MovingIn = (sender, args) => { };
		public event EventHandler<PlayerMoveEventArgs> MovingOut = (sender, args) => { };
		public IList<IrreparableMalfunction> IrreparableMalfunctions { get; private set; }
		protected ThreatController ThreatController { get; }

		protected Station(StationLocation stationLocation, ThreatController threatController)
		{
			Players = new List<Player>();
			IrreparableMalfunctions = new List<IrreparableMalfunction>();
			StationLocation = stationLocation;
			ThreatController = threatController;
		}

		public abstract void MovePlayerIn(Player performingPlayer, int currentTurn);

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

		protected void OnPlayerMovingIn(Player performingPlayer, int currentTurn)
		{
			MovingIn(this, new PlayerMoveEventArgs {CurrentTurn = currentTurn, MovingPlayer = performingPlayer});
		}

		protected void OnPlayerMovingOut(Player performingPlayer, int currentTurn)
		{
			MovingOut(this, new PlayerMoveEventArgs {CurrentTurn = currentTurn, MovingPlayer = performingPlayer});
		}

		public abstract void PerformNextPlayerAction(Player performingPlayer, int currentTurn);
	}
}