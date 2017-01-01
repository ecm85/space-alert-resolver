using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL;
using BLL.Threats;
using BLL.Tracks;
using PL.Models;

namespace PL.Controllers
{
	public class InputController : Controller
	{
		public ActionResult Index()
		{
			var inputModel = new InputModel
			{
				Actions = CreateAllActionModels(),
				Tracks = EnumFactory.All<TrackConfiguration>()
					.Select(trackConfiguration => new Track(trackConfiguration))
					.Select(track => new TrackSnapshotModel(track, new List<Threat>()))
					.ToList(),
				AllThreats = new AllThreatsModel(ThreatFactory.ThreatTypesById.Select(threat => ThreatFactory.CreateThreat<Threat>(threat.Key)))
			};
			var modelsString = JavaScriptConvert.SerializeObject(inputModel);
			return View(modelsString);
		}

		private static ActionModel[] CreateAllActionModels()
		{
			return new[]
			{
				new ActionModel {DisplayText = "space", EntryText = " ", Description = null, Action = null},
				new ActionModel {DisplayText = "a", EntryText = "a", Description = "A", Action = PlayerActionType.Alpha},
				new ActionModel {DisplayText = "b", EntryText = "b", Description = "B", Action = PlayerActionType.Bravo},
				new ActionModel {DisplayText = "c", EntryText = "c", Description = "C", Action = PlayerActionType.Charlie},
				new ActionModel {DisplayText = "x", EntryText = "x", Description = "BattleBots", Action = PlayerActionType.BattleBots},
				new ActionModel {DisplayText = "<", EntryText = "<", Description = "Red", Action = PlayerActionType.MoveRed},
				new ActionModel {DisplayText = ">", EntryText = ">", Description = "Blue", Action = PlayerActionType.MoveBlue},
				new ActionModel {DisplayText = "^", EntryText = "^", Description = "Down", Action = PlayerActionType.ChangeDeck},
				new ActionModel {DisplayText = "A", EntryText = "A", Description = "HeroicA", Action = PlayerActionType.HeroicA},
				new ActionModel {DisplayText = "B", EntryText = "B", Description = "HeroicB", Action = PlayerActionType.HeroicB},
				new ActionModel {DisplayText = "X", EntryText = "X", Description = "HeroicBattleBots", Action = PlayerActionType.HeroicBattleBots},
				new ActionModel {DisplayText = "1", EntryText = "1", Description = "TeleportUpperRed", Action = PlayerActionType.TeleportRedUpper},
				new ActionModel {DisplayText = "2", EntryText = "2", Description = "TeleportUpperWhite", Action = PlayerActionType.TeleportWhiteUpper},
				new ActionModel {DisplayText = "3", EntryText = "3", Description = "TeleportUpperBlue", Action = PlayerActionType.TeleportBlueUpper},
				new ActionModel {DisplayText = "4", EntryText = "4", Description = "TeleportLowerRed", Action = PlayerActionType.TeleportRedLower},
				new ActionModel {DisplayText = "5", EntryText = "5", Description = "TeleportLowerWhite", Action = PlayerActionType.TeleportWhiteLower},
				new ActionModel {DisplayText = "6", EntryText = "6", Description = "TeleportLowerBlue", Action = PlayerActionType.TeleportBlueLower}
			};
		}
	}
}
