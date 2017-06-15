using System.Collections.Generic;
using BLL.Players;

namespace BLL.ShipComponents
{
    public abstract class LaserCannon : Cannon
    {
        protected LaserCannon(IEnergyProvider source, int baseDamage, PlayerDamageType playerDamageType, ZoneLocation currentZone)
            : base(source, baseDamage, new [] {1, 2, 3}, playerDamageType, currentZone)
        {
        }

        protected override IEnumerable<PlayerDamage> GetPlayerDamage(Player performingPlayer, bool isHeroic, bool isAdvanced)
        {
            var damage = BaseDamage;
            if (isHeroic)
                damage++;
            if (IsDamaged)
                damage--;
            if (HasMechanicBuff)
                damage++;
            return new [] {new PlayerDamage(damage, PlayerDamageType, BaseAffectedDistances, AffectedZones, performingPlayer, OpticsDisrupted)};
        }
    }
}
