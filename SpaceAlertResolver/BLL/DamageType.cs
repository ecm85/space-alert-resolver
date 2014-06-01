using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public enum DamageType
	{
		HeavyLaser,
		LightLaser,
		Pulse,
		Rocket,
		Interceptors,
		ExternalThreat
	}

	public static class DamageTypeExtensions
	{
		public static DamageTargetType DamageTargetType(this DamageType damageType)
		{
			switch (damageType)
			{
				case DamageType.HeavyLaser:
				case DamageType.LightLaser:
					return BLL.DamageTargetType.Single;
				case DamageType.Pulse:
					return BLL.DamageTargetType.All;
				case DamageType.Rocket:
					return BLL.DamageTargetType.Single;
				case DamageType.Interceptors:
					//TODO: How to indicate that the damage # changes depending on target
					throw new NotImplementedException();
				case DamageType.ExternalThreat:
					throw new NotImplementedException();
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
