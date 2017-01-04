using System.Collections.Generic;
using BLL;
using Newtonsoft.Json;

namespace PL.Models
{
	public class ActionModel
	{
		public string SerializationText { get; set; }
		public string DisplayText { get; set; }
		public string EntryText { get; set; }
		public string Description { get; set; }
		public PlayerActionType? Action { get; set; }

		[JsonConstructor]
		public ActionModel()
		{

		}

		public static IEnumerable<ActionModel> AllActionModels { get; } = new []{
			new ActionModel {SerializationText = "-", DisplayText = "space", EntryText = " ", Description = null, Action = null},
			new ActionModel {SerializationText = "b", DisplayText = "b", EntryText = "b", Description = "B", Action = PlayerActionType.Bravo},
			new ActionModel {SerializationText = "a", DisplayText = "a", EntryText = "a", Description = "A", Action = PlayerActionType.Alpha},
			new ActionModel {SerializationText = "c", DisplayText = "c", EntryText = "c", Description = "C", Action = PlayerActionType.Charlie},
			new ActionModel {SerializationText = "x", DisplayText = "x", EntryText = "x", Description = "BattleBots", Action = PlayerActionType.BattleBots},
			new ActionModel {SerializationText = "<", DisplayText = "<", EntryText = "<", Description = "Red", Action = PlayerActionType.MoveRed},
			new ActionModel {SerializationText = ">", DisplayText = ">", EntryText = ">", Description = "Blue", Action = PlayerActionType.MoveBlue},
			new ActionModel {SerializationText = "^", DisplayText = "^", EntryText = "^", Description = "Down", Action = PlayerActionType.ChangeDeck},
			new ActionModel {SerializationText = "A", DisplayText = "A", EntryText = "A", Description = "HeroicA", Action = PlayerActionType.HeroicA},
			new ActionModel {SerializationText = "B", DisplayText = "B", EntryText = "B", Description = "HeroicB", Action = PlayerActionType.HeroicB},
			new ActionModel {SerializationText = "X", DisplayText = "X", EntryText = "X", Description = "HeroicBattleBots", Action = PlayerActionType.HeroicBattleBots},
			new ActionModel {SerializationText = "1", DisplayText = "1", EntryText = "1", Description = "TeleportUpperRed", Action = PlayerActionType.TeleportRedUpper},
			new ActionModel {SerializationText = "2", DisplayText = "2", EntryText = "2", Description = "TeleportUpperWhite", Action = PlayerActionType.TeleportWhiteUpper},
			new ActionModel {SerializationText = "3", DisplayText = "3", EntryText = "3", Description = "TeleportUpperBlue", Action = PlayerActionType.TeleportBlueUpper},
			new ActionModel {SerializationText = "4", DisplayText = "4", EntryText = "4", Description = "TeleportLowerRed", Action = PlayerActionType.TeleportRedLower},
			new ActionModel {SerializationText = "5", DisplayText = "5", EntryText = "5", Description = "TeleportLowerWhite", Action = PlayerActionType.TeleportWhiteLower},
			new ActionModel {SerializationText = "6", DisplayText = "6", EntryText = "6", Description = "TeleportLowerBlue", Action = PlayerActionType.TeleportBlueLower}
		};
	}
}
