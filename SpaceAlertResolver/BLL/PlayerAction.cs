using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BLL
{
	public enum PlayerAction
	{
		None,
		A,
		B,
		C,
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
		public static bool CanBeMadeHeroic(this PlayerAction playerAction)
		{
			switch (playerAction)
			{
				case PlayerAction.A:
				case PlayerAction.B:
				case PlayerAction.BattleBots:
					return true;
				default:
					return false;
			}
		}

		public static PlayerAction MakeHeroic(this PlayerAction playerAction)
		{
			switch (playerAction)
			{
				case PlayerAction.A:
					return PlayerAction.HeroicA;
				case PlayerAction.B:
					return PlayerAction.HeroicB;
				case PlayerAction.BattleBots:
					return PlayerAction.HeroicBattleBots;
				default:
					var message = string.Format(
						CultureInfo.CurrentCulture,
						"Cannot make {0} heroic",
						playerAction);
					throw new InvalidOperationException(message);
			}
		}
	}
}
