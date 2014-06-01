using System.Collections.Generic;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public interface IStation
	{
		Station RedwardStation { get; set; }
		Station BluewardStation { get; set; }
		Station OppositeDeckStation { get; set; }
		EnergyContainer EnergyContainer { get; set; }
		ZoneLocation ZoneLocation { get; set; }
		ISet<InternalThreat> Threats { get; }
		IList<Player> Players { get; }
		void PerformBAction();
		PlayerDamage PerformAAction();
		CResult PerformCAction(Player performingPlayer);
		InternalPlayerDamageResult UseBattleBots();
	}
}