using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class PlayerDamage
	{
		public int[] AffectedDistance { get; set; }
		public IList<ZoneLocation> ZoneLocations { get; set; }
		public int Amount { get; set; }
		public PlayerDamageType PlayerDamageType { get; set; }
		public bool RequiresTargetingAssistance { get; set; }
		public Player PerformingPlayer { get; set; }

		public PlayerDamage(int amount, PlayerDamageType playerDamageType, int[] affectedDistance, IList<ZoneLocation> zoneLocations, Player performingPlayer, bool requiresTargetingAssistance = false)
		{
			Amount = amount;
			PlayerDamageType = playerDamageType;
			AffectedDistance = affectedDistance;
			ZoneLocations = zoneLocations;
			RequiresTargetingAssistance = requiresTargetingAssistance;
			PerformingPlayer = performingPlayer;
		}

		public PlayerDamage(PlayerDamage other)
			: this(
				other.Amount,
				other.PlayerDamageType,
				other.AffectedDistance.ToArray(),
				other.ZoneLocations.ToList(),
				other.PerformingPlayer,
				other.RequiresTargetingAssistance)
		{
		}
	}
}
