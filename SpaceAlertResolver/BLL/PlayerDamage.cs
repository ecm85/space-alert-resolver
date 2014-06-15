using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class PlayerDamage
	{
		public int[] AffectedDistance { get; private set; }
		public IList<ZoneLocation> ZoneLocations { get; private set; }
		public int Amount { get; private set; }
		public PlayerDamageType PlayerDamageType { get; private set; }
		public bool RequiresTargetingAssistance { get; private set; }
		public Player PerformingPlayer { get; private set; }

		public PlayerDamage(int amount, PlayerDamageType playerDamageType, int[] affectedDistance, IList<ZoneLocation> zoneLocations, Player performingPlayer, bool requiresTargetingAssistance = false)
		{
			Amount = amount;
			PlayerDamageType = playerDamageType;
			AffectedDistance = affectedDistance;
			ZoneLocations = zoneLocations;
			RequiresTargetingAssistance = requiresTargetingAssistance;
			PerformingPlayer = performingPlayer;
		}
	}
}
