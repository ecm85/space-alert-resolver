using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public class InterceptorStation : IStation
	{
		public Station RedwardStation { get; set; }
		public Station BluewardStation { get; set; }
		public Station OppositeDeckStation { get; set; }
		public EnergyContainer EnergyContainer { get; set; }
		public ZoneLocation ZoneLocation { get; set; }
		public InterceptorComponent InterceptorComponent { private get; set; }
		public ISet<InternalThreat> Threats { get; private set; }
		public IList<Player> Players { get; private set; }
		public PlayerInterceptorDamage PlayerInterceptorDamage { get; set; }

		public InterceptorStation()
		{
			Players = new List<Player>();
			Threats = new HashSet<InternalThreat>();
		}

		public void PerformBAction(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer);
		}

		public PlayerDamage PerformAAction(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer);
			return null;
		}

		public void PerformCAction(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer);
			//TODO: Change to a further interceptor station instead, if variable range interceptors are in use
		}

		public void UseBattleBots(Player performingPlayer)
		{
			UseInterceptors(performingPlayer);
		}

		public void UseInterceptors(Player performingPlayer)
		{
			var firstThreat = GetFirstThreatOfType(PlayerAction.BattleBots);
			if (firstThreat == null)
				PlayerInterceptorDamage = new PlayerInterceptorDamage();
			else
			{
				firstThreat.TakeDamage(1, performingPlayer);
				if (firstThreat.RemainingHealth <= 0)
				{
					//TODO: Handle removing from track, removing from ship.CurrentList and scoring
				}
			}
		}

		private InternalThreat GetFirstThreatOfType(PlayerAction playerAction)
		{
			return Threats
				.Where(threat => threat.ActionType == playerAction)
				.OrderBy(threat => threat.TimeAppears)
				.FirstOrDefault();
		}

		public void PerformNoAction(Player performingPlayer)
		{
			InterceptorComponent.PerformNoAction(performingPlayer);
		}
	}
}
