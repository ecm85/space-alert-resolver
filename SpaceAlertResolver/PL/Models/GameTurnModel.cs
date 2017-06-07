using System.Collections.Generic;

namespace PL.Models
{
	public class GameTurnModel
	{
		public int Turn { get; set; }
		public IList<GamePhaseModel> Phases { get; } = new List<GamePhaseModel>();
	}
}
