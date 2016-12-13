using System;

namespace BLL
{
	public enum PlayerDamageType
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
		public static DamageTargetType DamageTargetType(this PlayerDamageType playerDamageType)
		{
			switch (playerDamageType)
			{
				case PlayerDamageType.HeavyLaser:
				case PlayerDamageType.LightLaser:
					return BLL.DamageTargetType.Single;
				case PlayerDamageType.Pulse:
					return BLL.DamageTargetType.All;
				case PlayerDamageType.Rocket:
					return BLL.DamageTargetType.Single;
				case PlayerDamageType.InterceptorsMultiple:
					return BLL.DamageTargetType.All;
				case PlayerDamageType.InterceptorsSingle:
					return BLL.DamageTargetType.Single;
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
