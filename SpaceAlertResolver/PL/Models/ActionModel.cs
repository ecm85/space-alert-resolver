using System.Collections.Generic;
using BLL;
using Newtonsoft.Json;

namespace PL.Models
{
	public class ActionModel
	{
		public string Hotkey { get; set; }
		public string DisplayText { get; set; }
		public string Description { get; set; }
		public PlayerActionType? Action { get; set; }

		[JsonConstructor]
		public ActionModel()
		{

		}

		public static IEnumerable<ActionModel> AllActionModels { get; } = new []{
			new ActionModel {Hotkey = "-", DisplayText = "Blank", Description = null, Action = null},
			new ActionModel {Hotkey = "b", DisplayText = "B", Description = "B", Action = PlayerActionType.Bravo},
			new ActionModel {Hotkey = "a", DisplayText = "A", Description = "A", Action = PlayerActionType.Alpha},
			new ActionModel {Hotkey = "c", DisplayText = "C", Description = "C", Action = PlayerActionType.Charlie},
			new ActionModel {Hotkey = "x", DisplayText = "BattleBots", Description = "BattleBots", Action = PlayerActionType.BattleBots},
			new ActionModel {Hotkey = "<", DisplayText = "Red", Description = "Red", Action = PlayerActionType.MoveRed},
			new ActionModel {Hotkey = ">", DisplayText = "Blue", Description = "Blue", Action = PlayerActionType.MoveBlue},
			new ActionModel {Hotkey = "^", DisplayText = "Down", Description = "Down", Action = PlayerActionType.ChangeDeck},
			new ActionModel {Hotkey = "A", DisplayText = "HeroicA", Description = "HeroicA", Action = PlayerActionType.HeroicA},
			new ActionModel {Hotkey = "B", DisplayText = "HeroicB", Description = "HeroicB", Action = PlayerActionType.HeroicB},
			new ActionModel {Hotkey = "X", DisplayText = "HeroicBattleBots", Description = "HeroicBattleBots", Action = PlayerActionType.HeroicBattleBots},
			new ActionModel {Hotkey = "1", DisplayText = "TeleportUpperRed", Description = "TeleportUpperRed", Action = PlayerActionType.TeleportRedUpper},
			new ActionModel {Hotkey = "2", DisplayText = "TeleportUpperWhite", Description = "TeleportUpperWhite", Action = PlayerActionType.TeleportWhiteUpper},
			new ActionModel {Hotkey = "3", DisplayText = "TeleportUpperBlue", Description = "TeleportUpperBlue", Action = PlayerActionType.TeleportBlueUpper},
			new ActionModel {Hotkey = "4", DisplayText = "TeleportLowerRed", Description = "TeleportLowerRed", Action = PlayerActionType.TeleportRedLower},
			new ActionModel {Hotkey = "5", DisplayText = "TeleportLowerWhite", Description = "TeleportLowerWhite", Action = PlayerActionType.TeleportWhiteLower},
			new ActionModel {Hotkey = "6", DisplayText = "TeleportLowerBlue", Description = "TeleportLowerBlue", Action = PlayerActionType.TeleportBlueLower}
		};
	}
}
