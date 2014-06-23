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

		public override void SetDamaged()
		{
			IsDamaged = true;
			distancesAffected = new[] {1};
		}

		public override void Repair()
		{
			IsDamaged = false;
			distancesAffected = new [] {1, 2};
		}

		protected override PlayerDamage[] GetPlayerDamage(Player performingPlayer, bool isHeroic, bool isAdvanced)
		{
			var amount = isHeroic || isAdvanced ? damage + 1 : damage;

			var damages = new List<PlayerDamage> {new PlayerDamage(amount, PlayerDamageType.Pulse, distancesAffected, zonesAffected, performingPlayer)};
			if(isAdvanced)
				damages.Add(new PlayerDamage(damage, PlayerDamageType.Pulse, new [] {distancesAffected.Max(distance => distance) + 1}, zonesAffected, performingPlayer));
			return damages.ToArray();
		}
	}
}
