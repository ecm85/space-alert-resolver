using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;
using PL.Models;
using ThreatFactory = BLL.Threats.Internal.ThreatFactory;

namespace PL.Controllers
{
	public class SpaceAlertController : Controller
	{
		public ActionResult Index()
		{
			var allExternalThreats = BLL.Threats.External.ThreatFactory.ThreatTypesById
				.Select(threat => BLL.Threats.External.ThreatFactory.CreateThreat<ExternalThreat>(threat.Key))
				.Select(threat => new ExternalThreatModel(threat))
				.ToList();
			var allInternalThreats = ThreatFactory.ThreatTypesById
				.Select(threat => ThreatFactory.CreateThreat<InternalThreat>(threat.Key))
				.Select(threat => new InternalThreatModel(threat))
				.ToList();
			var inputModel = new InputModel
			{
				Actions = CreateAllActionModels(),
				Tracks = EnumFactory.All<TrackConfiguration>()
					.Select(trackConfiguration => new Track(trackConfiguration))
					.Select(track => new TrackSnapshotModel(track, new List<Threat>()))
					.ToList(),
				AllInternalThreats = new AllThreatsModel(allInternalThreats),
				AllExternalThreats = new AllThreatsModel(allExternalThreats)
			};
			var modelsString = JavaScriptConvert.SerializeObject(inputModel);
			return View("Input", modelsString);
		}

		[HttpPost]
		public ActionResult Index(string gameArgs)
		{
			var game = GameParser.ParseArgsIntoGame(gameArgs.Split(' '));
			var nextPhase = 0;
			var models = new List<GameSnapshotModel> { new GameSnapshotModel(game, "Start of Game", () => nextPhase++) };
			game.NewThreatsAdded += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Threats Appeared", () => nextPhase++));
			game.PlayerActionsPerformed += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Player Action Finished", () => nextPhase++));
			game.ResolvedDamage += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Resolved Damage", () => nextPhase++));
			game.ThreatsMoved += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Threats Moved", () => nextPhase++));
			game.CheckedForComputer += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "Checked Computer Maintenance", () => nextPhase++));
			game.TurnEnding += (sender, eventArgs) => models.Add(new GameSnapshotModel((Game)sender, "End of Turn", () => nextPhase++));
			for (var i = 0; i < game.NumberOfTurns; i++)
			{
				game.PerformTurn();
				nextPhase = 0;
			}
			var modelsString = JavaScriptConvert.SerializeObject(models.GroupBy(model => model.Turn).ToList());
			return View("Resolution", modelsString);
		}

		private static ActionModel[] CreateAllActionModels()
		{
			return new[]
			{
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
}
