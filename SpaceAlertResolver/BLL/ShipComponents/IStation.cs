using System.Collections.Generic;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public interface IStation
	{
		Station RedwardStation { get; }
		Station BluewardStation { get; }
		Station OppositeDeckStation { get; }
		EnergyContainer EnergyContainer { get; }
		ZoneLocation ZoneLocation { get; }
		ISet<InternalThreat> Threats { get; }
		IList<Player> Players { get; }
		void PerformBAction(Player performingPlayer, int currentTurn);
		PlayerDamage PerformAAction(Player performingPlayer, int currentTurn);
		void PerformCAction(Player performingPlayer, int currentTurn);
		void UseBattleBots(Player performingPlayer);
		void UseInterceptors(Player performingPlayer);
		void PerformNoAction(Player performingPlayer);
	}
}