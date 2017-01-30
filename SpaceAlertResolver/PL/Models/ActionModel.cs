using System.Collections.Generic;
using System.Linq;
using BLL;
using Newtonsoft.Json;

namespace PL.Models
{
	public class ActionModel
	{
		public string Hotkey { get; set; }
		public string DisplayText { get; set; }
		public string Description { get; set; }
		public PlayerActionType? FirstAction { get; set; }
		public PlayerActionType? SecondAction { get; set; }

		[JsonConstructor]
		public ActionModel()
		{

		}

		public static ActionModel Create(PlayerAction action)
		{
			return AllDoubleActionModels.Concat(AllSingleActionModels).SingleOrDefault(actionModel =>
				action.FirstActionType == actionModel.FirstAction &&
				action.SecondActionType == actionModel.SecondAction);
		}

		public static IEnumerable<ActionModel> AllSingleActionModels { get; } = new []
		{
			new ActionModel {Hotkey = "-", DisplayText = "Blank", Description = null, FirstAction = null},
			new ActionModel {Hotkey = "b", DisplayText = "B", Description = "B", FirstAction = PlayerActionType.Bravo},
			new ActionModel {Hotkey = "a", DisplayText = "A", Description = "A", FirstAction = PlayerActionType.Alpha},
			new ActionModel {Hotkey = "c", DisplayText = "C", Description = "C", FirstAction = PlayerActionType.Charlie},
			new ActionModel {Hotkey = "x", DisplayText = "BattleBots", Description = "BattleBots", FirstAction = PlayerActionType.BattleBots},
			new ActionModel {Hotkey = "<", DisplayText = "Red", Description = "Red", FirstAction = PlayerActionType.MoveRed},
			new ActionModel {Hotkey = ">", DisplayText = "Blue", Description = "Blue", FirstAction = PlayerActionType.MoveBlue},
			new ActionModel {Hotkey = "^", DisplayText = "Down", Description = "Down", FirstAction = PlayerActionType.ChangeDeck},
			new ActionModel {Hotkey = "A", DisplayText = "HeroicA", Description = "HeroicA", FirstAction = PlayerActionType.HeroicA},
			new ActionModel {Hotkey = "B", DisplayText = "HeroicB", Description = "HeroicB", FirstAction = PlayerActionType.HeroicB},
			new ActionModel {Hotkey = "X", DisplayText = "HeroicBattleBots", Description = "HeroicBattleBots", FirstAction = PlayerActionType.HeroicBattleBots},
			new ActionModel {Hotkey = "1", DisplayText = "TeleportUpperRed", Description = "TeleportUpperRed", FirstAction = PlayerActionType.TeleportRedUpper},
			new ActionModel {Hotkey = "2", DisplayText = "TeleportUpperWhite", Description = "TeleportUpperWhite", FirstAction = PlayerActionType.TeleportWhiteUpper},
			new ActionModel {Hotkey = "3", DisplayText = "TeleportUpperBlue", Description = "TeleportUpperBlue", FirstAction = PlayerActionType.TeleportBlueUpper},
			new ActionModel {Hotkey = "4", DisplayText = "TeleportLowerRed", Description = "TeleportLowerRed", FirstAction = PlayerActionType.TeleportRedLower},
			new ActionModel {Hotkey = "5", DisplayText = "TeleportLowerWhite", Description = "TeleportLowerWhite", FirstAction = PlayerActionType.TeleportWhiteLower},
			new ActionModel {Hotkey = "6", DisplayText = "TeleportLowerBlue", Description = "TeleportLowerBlue", FirstAction = PlayerActionType.TeleportBlueLower},
		};

		public static IEnumerable<ActionModel> AllDoubleActionModels { get; } = new[]
		{
			new ActionModel {Hotkey = "-", DisplayText = "Blank", Description = null, FirstAction = null},
			new ActionModel { Description="AB", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.Bravo },
			new ActionModel { Description="ABlue", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.MoveBlue },
			new ActionModel { Description="AC", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.Charlie },
			new ActionModel { Description="ADown", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.ChangeDeck },
			new ActionModel { Description="ARed", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.MoveRed },
			new ActionModel { Description="BA", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.Alpha },
			new ActionModel { Description="BattleBotsA", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.Alpha },
			new ActionModel { Description="BattleBotsB", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.Bravo },
			new ActionModel { Description="BattleBotsBlue", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.MoveBlue },
			new ActionModel { Description="BattleBotsC", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.Charlie },
			new ActionModel { Description="BattleBotsDown", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.ChangeDeck },
			new ActionModel { Description="BBlue", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.MoveBlue },
			new ActionModel { Description="BC", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.Charlie },
			new ActionModel { Description="BDown", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.ChangeDeck },
			new ActionModel { Description="BlueA", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.Alpha },
			new ActionModel { Description="BlueB", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.Bravo },
			new ActionModel { Description="BlueBattleBots", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.BattleBots },
			new ActionModel { Description="BlueBlue", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.MoveBlue },
			new ActionModel { Description="BlueC", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.Charlie },
			new ActionModel { Description="BlueDown", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.ChangeDeck },
			new ActionModel { Description="BRed", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.MoveRed },
			new ActionModel { Description="CA", FirstAction=PlayerActionType.Charlie, SecondAction=PlayerActionType.Alpha },
			new ActionModel { Description="CBlue", FirstAction=PlayerActionType.Charlie, SecondAction=PlayerActionType.MoveBlue },
			new ActionModel { Description="CDown", FirstAction=PlayerActionType.Charlie, SecondAction=PlayerActionType.ChangeDeck },
			new ActionModel { Description="CRed", FirstAction=PlayerActionType.Charlie, SecondAction=PlayerActionType.MoveRed },
			new ActionModel { Description="DownA", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.Alpha },
			new ActionModel { Description="DownB", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.Bravo },
			new ActionModel { Description="DownBattleBots", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.BattleBots },
			new ActionModel { Description="DownC", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.Charlie },
			new ActionModel { Description="RedA", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.Alpha },
			new ActionModel { Description="RedB", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.Bravo },
			new ActionModel { Description="RedBattleBots", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.BattleBots },
			new ActionModel { Description="RedC", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.Charlie },
			new ActionModel { Description="RedDown", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.ChangeDeck },
			new ActionModel { Description="RedRed", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.MoveRed }
		};
	}
}
