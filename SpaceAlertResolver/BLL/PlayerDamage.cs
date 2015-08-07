using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class PlayerDamage
	{
		public IEnumerable<int> AffectedDistances { get; private set; }
		public IList<ZoneLocation> AffectedZones { get; private set; }
		public int Amount { get; set; }
		public PlayerDamageType PlayerDamageType { get; private set; }
		public bool RequiresTargetingAssistance { get; private set; }
		public Player PerformingPlayer { get; private set; }

		public PlayerDamage(int amount, PlayerDamageType playerDamageType, IEnumerable<int> affectedDistances, IList<ZoneLocation> affectedZones)
		{
			Amount = amount;
			PlayerDamageType = playerDamageType;
			AffectedDistances = affectedDistances;
			AffectedZones = affectedZones;
		}

		public PlayerDamage(int amount, PlayerDamageType playerDamageType, IEnumerable<int> affectedDistances, IList<ZoneLocation> affectedZones, Player performingPlayer)
			: this(amount, playerDamageType, affectedDistances, affectedZones)
		{
			PerformingPlayer = performingPlayer;
		}

		public PlayerDamage(int amount, PlayerDamageType playerDamageType, IEnumerable<int> affectedDistances, IList<ZoneLocation> affectedZones, Player performingPlayer, bool requiresTargetingAssistance)
			: this(amount, playerDamageType, affectedDistances, affectedZones, performingPlayer)
		{
			RequiresTargetingAssistance = requiresTargetingAssistance;
		}

		public PlayerDamage(PlayerDamage other)
			: this(
				other.Amount,
				other.PlayerDamageType,
				other.AffectedDistances.ToList(),
				other.AffectedZones.ToList(),
				other.PerformingPlayer,
				other.RequiresTargetingAssistance)
		{
		}
	}
}
