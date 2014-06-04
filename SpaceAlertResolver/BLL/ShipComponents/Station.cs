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
		public StationLocation StationLocation { get; set; }
		public ISet<InternalThreat> Threats { get; private set; }
		public IList<Player> Players { get; private set; }
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

		public virtual void PerformNoAction(Player performingPlayer)
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

		protected bool HasIrreparableMalfunctionOfType(PlayerAction playerAction)
		{
			return IrreparableMalfunctions.Any(malfunction => malfunction.ActionType == playerAction);
		}

		protected void DamageThreat(InternalThreat threat, Player performingPlayer, bool isHeroic)
		{
			var damage = isHeroic ? 2 : 1;
			threat.TakeDamage(damage, performingPlayer, isHeroic);
		}
	}
}