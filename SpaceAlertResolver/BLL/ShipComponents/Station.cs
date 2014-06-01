using System.Collections.Generic;
using System.Linq;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public abstract class Station
	{
		public StandardStation RedwardStation { get; set; }
		public StandardStation BluewardStation { get; set; }
		public StandardStation OppositeDeckStation { get; set; }
		public EnergyContainer EnergyContainer { get; set; }
		public Cannon Cannon { get; set; }
		public CComponent CComponent { protected get; set; }
		public ZoneLocation ZoneLocation { get; set; }
		public ISet<InternalThreat> Threats { get; private set; }
		public IList<Player> Players { get; private set; }

		protected Station()
		{
			Players = new List<Player>();
			Threats = new HashSet<InternalThreat>();
		}

		public abstract void PerformBAction(Player performingPlayer, int currentTurn);
		public abstract PlayerDamage PerformAAction(Player performingPlayer, int currentTurn);
		public abstract void PerformCAction(Player performingPlayer, int currentTurn);
		public abstract void UseBattleBots(Player performingPlayer);

		public virtual void PerformNoAction(Player performingPlayer)
		{
		}

		public virtual void UseInterceptors(Player performingPlayer)
		{
		}

		protected InternalThreat GetFirstThreatOfType(PlayerAction playerAction)
		{
			return Threats
				.Where(threat => threat.ActionType == playerAction)
				.OrderBy(threat => threat.TimeAppears)
				.FirstOrDefault();
		}

		protected void DamageThreat(InternalThreat threat, Player performingPlayer)
		{
			threat.TakeDamage(1, performingPlayer);
			if (threat.RemainingHealth <= 0)
			{
				//TODO: Handle removing from track, removing from ship.CurrentList and scoring
			}
		}
	}
}