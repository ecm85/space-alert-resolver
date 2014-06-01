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
		InterceptorsMultiple,
		InterceptorsSingle
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
				case DamageType.InterceptorsMultiple:
					return BLL.DamageTargetType.All;
				case DamageType.InterceptorsSingle:
					return BLL.DamageTargetType.Single;
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
