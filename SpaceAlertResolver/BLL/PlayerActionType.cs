using System;
using System.Globalization;

namespace BLL
{
	public enum PlayerActionType
	{
		Alpha,
		Bravo,
		Charlie,
		MoveRed,
		MoveBlue,
		ChangeDeck,
		BattleBots,
		HeroicA,
		HeroicB,
		HeroicBattleBots,
		TeleportBlueUpper,
		TeleportBlueLower,
		TeleportWhiteUpper,
		TeleportWhiteLower,
		TeleportRedUpper,
		TeleportRedLower,
		BasicSpecialization,
		AdvancedSpecialization
	}

	public static class PlayerActionExtensions
	{
		public static bool CanBeMadeHeroic(this PlayerActionType? playerActionType)
		{
			switch (playerActionType)
			{
				case PlayerActionType.Alpha:
				case PlayerActionType.Bravo:
				case PlayerActionType.BattleBots:
					return true;
				default:
					return false;
			}
		}

		public static PlayerActionType MakeHeroic(this PlayerActionType? playerActionType)
		{
			switch (playerActionType)
			{
				case PlayerActionType.Alpha:
					return PlayerActionType.HeroicA;
				case PlayerActionType.Bravo:
					return PlayerActionType.HeroicB;
				case PlayerActionType.BattleBots:
					return PlayerActionType.HeroicBattleBots;
				default:
					var message = string.Format(
						CultureInfo.CurrentCulture,
						"Cannot make {0} heroic",
						playerActionType);
					throw new InvalidOperationException(message);
			}
		}

		public static bool IsBasicMovement(this PlayerActionType? playerActionType)
		{
			switch (playerActionType)
			{
				case PlayerActionType.MoveBlue:
				case PlayerActionType.MoveRed:
				case PlayerActionType.ChangeDeck:
					return true;
				default:
					return false;
			}
		}
	}
}
