using System.Collections.Generic;
using BLL.Players;
using Newtonsoft.Json;

namespace PL.Models
{
    public class ActionModel
    {
        public virtual string Hotkey { get; private set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
        public PlayerActionType? FirstAction { get; set; }
        public PlayerActionType? SecondAction { get; set; }
        public PlayerSpecializationActionModel BonusAction { get; set; }

        public virtual ActionModel Clone()
        {
            return new ActionModel
            {
                Hotkey = Hotkey,
                DisplayText = DisplayText,
                Description = Description,
                FirstAction = FirstAction,
                SecondAction = SecondAction,
                BonusAction = BonusAction
            };
        }

        [JsonConstructor]
        public ActionModel()
        {

        }

        public static IEnumerable<ActionModel> AllSingleActionModels { get; } = new []
        {
            new ActionModel {Hotkey = "-", DisplayText = "Blank", Description = null, FirstAction = null},
            new ActionModel {Hotkey = "a", DisplayText = "A", Description = "A", FirstAction = PlayerActionType.Alpha},
            new ActionModel {Hotkey = "b", DisplayText = "B", Description = "B", FirstAction = PlayerActionType.Bravo},
            new ActionModel {Hotkey = "c", DisplayText = "C", Description = "C", FirstAction = PlayerActionType.Charlie},
            new ActionModel {Hotkey = "x", DisplayText = "BattleBots", Description = "BattleBots", FirstAction = PlayerActionType.BattleBots},
            new ActionModel {Hotkey = "<", DisplayText = "Red", Description = "Red", FirstAction = PlayerActionType.MoveRed},
            new ActionModel {Hotkey = ">", DisplayText = "Blue", Description = "Blue", FirstAction = PlayerActionType.MoveBlue},
            new ActionModel {Hotkey = "^", DisplayText = "Down", Description = "Down", FirstAction = PlayerActionType.ChangeDeck},
            new ActionModel {Hotkey = "A", DisplayText = "Heroic A", Description = "HeroicA", FirstAction = PlayerActionType.HeroicA},
            new ActionModel {Hotkey = "B", DisplayText = "Heroic B", Description = "HeroicB", FirstAction = PlayerActionType.HeroicB},
            new ActionModel {Hotkey = "X", DisplayText = "Heroic BattleBots", Description = "HeroicBattleBots", FirstAction = PlayerActionType.HeroicBattleBots},
            new ActionModel {Hotkey = "1", DisplayText = "Heroic Move To Upper Red", Description = "TeleportUpperRed", FirstAction = PlayerActionType.TeleportRedUpper},
            new ActionModel {Hotkey = "2", DisplayText = "Heroic Move To Upper White", Description = "TeleportUpperWhite", FirstAction = PlayerActionType.TeleportWhiteUpper},
            new ActionModel {Hotkey = "3", DisplayText = "Heroic Move To Upper Blue", Description = "TeleportUpperBlue", FirstAction = PlayerActionType.TeleportBlueUpper},
            new ActionModel {Hotkey = "4", DisplayText = "Heroic Move To Lower Red", Description = "TeleportLowerRed", FirstAction = PlayerActionType.TeleportRedLower},
            new ActionModel {Hotkey = "5", DisplayText = "Heroic Move To Lower White", Description = "TeleportLowerWhite", FirstAction = PlayerActionType.TeleportWhiteLower},
            new ActionModel {Hotkey = "6", DisplayText = "Heroic Move To Lower Blue", Description = "TeleportLowerBlue", FirstAction = PlayerActionType.TeleportBlueLower},
        };

        public static IEnumerable<ActionModel> AllSelectableDoubleActionModels { get; } = new[]
        {
            new ActionModel { Hotkey = "ab", Description="AB", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.Bravo },
            new ActionModel { Hotkey = "ac", Description="AC", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.Charlie },
            new ActionModel { Hotkey = "a>", Description="ABlue", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.MoveBlue },
            new ActionModel { Hotkey = "a<", Description="ARed", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.MoveRed },
            new ActionModel { Hotkey = "a^", Description="ADown", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.ChangeDeck },
            new ActionModel { Hotkey = "ba", Description="BA", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Hotkey = "b>", Description="BBlue", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.MoveBlue },
            new ActionModel { Hotkey = "bc", Description="BC", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.Charlie },
            new ActionModel { Hotkey = "b^", Description="BDown", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.ChangeDeck },
            new ActionModel { Hotkey = "b<", Description="BRed", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.MoveRed },
            new ActionModel { Hotkey = "ca", Description="CA", FirstAction=PlayerActionType.Charlie, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Hotkey = "c>", Description="CBlue", FirstAction=PlayerActionType.Charlie, SecondAction=PlayerActionType.MoveBlue },
            new ActionModel { Hotkey = "c^", Description="CDown", FirstAction=PlayerActionType.Charlie, SecondAction=PlayerActionType.ChangeDeck },
            new ActionModel { Hotkey = "c<", Description="CRed", FirstAction=PlayerActionType.Charlie, SecondAction=PlayerActionType.MoveRed },
            new ActionModel { Hotkey = "xa", Description="BattleBotsA", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Hotkey = "xb", Description="BattleBotsB", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.Bravo },
            new ActionModel { Hotkey = "x>", Description="BattleBotsBlue", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.MoveBlue },
            new ActionModel { Hotkey = "xc", Description="BattleBotsC", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.Charlie },
            new ActionModel { Hotkey = "x^", Description="BattleBotsDown", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.ChangeDeck },
            new ActionModel { Hotkey = ">a", Description="BlueA", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Hotkey = ">b", Description="BlueB", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.Bravo },
            new ActionModel { Hotkey = ">x", Description="BlueBattleBots", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.BattleBots },
            new ActionModel { Hotkey = ">>", Description="BlueBlue", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.MoveBlue },
            new ActionModel { Hotkey = ">c", Description="BlueC", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.Charlie },
            new ActionModel { Hotkey = ">^", Description="BlueDown", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.ChangeDeck },
            new ActionModel { Hotkey = "<a", Description="RedA", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Hotkey = "<b", Description="RedB", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.Bravo },
            new ActionModel { Hotkey = "<x", Description="RedBattleBots", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.BattleBots },
            new ActionModel { Hotkey = "<c", Description="RedC", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.Charlie },
            new ActionModel { Hotkey = "<^", Description="RedDown", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.ChangeDeck },
            new ActionModel { Hotkey = "<<", Description="RedRed", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.MoveRed },
            new ActionModel { Hotkey = "^a", Description="DownA", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Hotkey = "^b", Description="DownB", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.Bravo },
            new ActionModel { Hotkey = "^x", Description="DownBattleBots", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.BattleBots },
            new ActionModel { Hotkey = "^c", Description="DownC", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.Charlie }
        };

        public static IEnumerable<ActionModel> AllNonSelectableDoubleActionModels { get; } = new []
        {
            new ActionModel { Description="HeroicAB", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.Bravo },
            new ActionModel { Description="HeroicAC", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.Charlie },
            new ActionModel { Description="HeroicABlue", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.MoveBlue },
            new ActionModel { Description="HeroicARed", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.MoveRed },
            new ActionModel { Description="HeroicADown", FirstAction=PlayerActionType.Alpha, SecondAction=PlayerActionType.ChangeDeck },
            new ActionModel { Description="HeroicBA", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Description="HeroicBBlue", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.MoveBlue },
            new ActionModel { Description="HeroicBC", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.Charlie },
            new ActionModel { Description="HeroicBDown", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.ChangeDeck },
            new ActionModel { Description="HeroicBRed", FirstAction=PlayerActionType.Bravo, SecondAction=PlayerActionType.MoveRed },
            new ActionModel { Description="CHeroicA", FirstAction=PlayerActionType.Charlie, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Description="HeroicBattleBotsA", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Description="HeroicBattleBotsB", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.Bravo },
            new ActionModel { Description="HeroicBattleBotsBlue", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.MoveBlue },
            new ActionModel { Description="HeroicBattleBotsC", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.Charlie },
            new ActionModel { Description="HeroicBattleBotsDown", FirstAction=PlayerActionType.BattleBots, SecondAction=PlayerActionType.ChangeDeck },
            new ActionModel { Description="BlueHeroicA", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Description="BlueHeroicB", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.Bravo },
            new ActionModel { Description="BlueHeroicBattleBots", FirstAction=PlayerActionType.MoveBlue, SecondAction=PlayerActionType.BattleBots },
            new ActionModel { Description="RedHeroicA", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Description="RedHeroicB", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.Bravo },
            new ActionModel { Description="RedHeroicBattleBots", FirstAction=PlayerActionType.MoveRed, SecondAction=PlayerActionType.BattleBots },
            new ActionModel { Description="DownHeroicA", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.Alpha },
            new ActionModel { Description="DownHeroicB", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.Bravo },
            new ActionModel { Description="DownHeroicBattleBots", FirstAction=PlayerActionType.ChangeDeck, SecondAction=PlayerActionType.BattleBots }
        };
    }
}
