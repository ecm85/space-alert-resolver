using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class PulseCannon : Cannon
	{
		public PulseCannon(Reactor source) : base(source, 1, new [] {1, 2}, PlayerDamageType.Pulse, EnumFactory.All<ZoneLocation>())
		{
		}

		protected override PlayerDamage[] GetPlayerDamage(Player performingPlayer, bool isHeroic, bool isAdvanced)
		{
			var damage = BaseDamage;
			var affectedDistances = BaseAffectedDistances;
			if (IsDamaged)
				affectedDistances = affectedDistances.Except(new[] {affectedDistances.Max()}).ToArray();
			if (MechanicBuff)
				affectedDistances = affectedDistances.Concat(new[] {affectedDistances.Max() + 1}).ToArray();
			if (isHeroic)
				damage++;
			if (isAdvanced)
				damage++;
			var damages = new List<PlayerDamage> {new PlayerDamage(damage, PlayerDamageType.Pulse, affectedDistances, AffectedZones, performingPlayer)};
			if(isAdvanced && !affectedDistances.Contains(3)) //If we already hit distance 3 with our regular attack (via mechanic when not damaged) there's no point to adding range 4
				damages.Add(new PlayerDamage(damage - 1, PlayerDamageType.Pulse, new [] {affectedDistances.Max(distance => distance) + 1}, AffectedZones, performingPlayer));
			return damages.ToArray();
		}
	}
}
