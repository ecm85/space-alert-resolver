using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;

namespace BLL.Players
{
    public class PlayerDamage
    {
        public IEnumerable<int> AffectedDistances { get; private set; }
        public IList<ZoneLocation> AffectedZones { get; private set; }
        public int Amount { get; set; }
        public PlayerDamageType PlayerDamageType { get; private set; }
        public bool RequiresTargetingAssistance { get; private set; }
        public Player PerformingPlayer { get; private set; }

        internal PlayerDamage(int amount, PlayerDamageType playerDamageType, IEnumerable<int> affectedDistances, IList<ZoneLocation> affectedZones)
        {
            Amount = amount;
            PlayerDamageType = playerDamageType;
            AffectedDistances = affectedDistances;
            AffectedZones = affectedZones;
        }

        internal PlayerDamage(int amount, PlayerDamageType playerDamageType, IEnumerable<int> affectedDistances, IList<ZoneLocation> affectedZones, Player performingPlayer)
            : this(amount, playerDamageType, affectedDistances, affectedZones)
        {
            PerformingPlayer = performingPlayer;
        }

        internal PlayerDamage(int amount, PlayerDamageType playerDamageType, IEnumerable<int> affectedDistances, IList<ZoneLocation> affectedZones, Player performingPlayer, bool requiresTargetingAssistance)
            : this(amount, playerDamageType, affectedDistances, affectedZones, performingPlayer)
        {
            RequiresTargetingAssistance = requiresTargetingAssistance;
        }

        internal PlayerDamage(PlayerDamage other)
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
