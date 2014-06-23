using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class PlayerDamage
	{
		public int[] AffectedDistances { get; set; }
		public IList<ZoneLocation> AffectedZones { get; set; }
		public int Amount { get; set; }
		public PlayerDamageType PlayerDamageType { get; set; }
		public bool RequiresTargetingAssistance { get; set; }
		public Player PerformingPlayer { get; set; }

		public PlayerDamage(int amount, PlayerDamageType playerDamageType, int[] affectedDistances, IList<ZoneLocation> affectedZones, Player performingPlayer, bool requiresTargetingAssistance = false)
		{
			Amount = amount;
			PlayerDamageType = playerDamageType;
			AffectedDistances = affectedDistances;
			AffectedZones = affectedZones;
			RequiresTargetingAssistance = requiresTargetingAssistance;
			PerformingPlayer = performingPlayer;
		}

		public PlayerDamage(PlayerDamage other)
			: this(
				other.Amount,
				other.PlayerDamageType,
				other.AffectedDistances.ToArray(),
				other.AffectedZones.ToList(),
				other.PerformingPlayer,
				other.RequiresTargetingAssistance)
		{
		}
	}
}
