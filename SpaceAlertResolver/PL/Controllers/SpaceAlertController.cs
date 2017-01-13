using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BLL;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;
using PL.Models;

namespace PL.Controllers
{
	public class SpaceAlertController : Controller
	{
		[HttpGet]
		[ActionName("Index")]
		public ActionResult IndexGet(string newGameText)
		{
			var allExternalThreats = ExternalThreatFactory.ThreatTypesById
				.Select(threat => ExternalThreatFactory.CreateThreat<ExternalThreat>(threat.Key))
				.Select(threat => new ExternalThreatModel(threat))
				.ToList();
			var allInternalThreats = InternalThreatFactory.ThreatTypesById
				.Select(threat => InternalThreatFactory.CreateThreat<InternalThreat>(threat.Key))
				.Select(threat => new InternalThreatModel(threat))
				.ToList();
			var newGameModel = newGameText != null ? new JavaScriptSerializer().Deserialize<NewGameModel>(newGameText) : null;
			var inputModel = new InputModel
			{
				Actions = ActionModel.AllActionModels,
				Tracks = EnumFactory.All<TrackConfiguration>()
					.Select(trackConfiguration => new Track(trackConfiguration))
					.Select(track => new TrackSnapshotModel(track, new List<Threat>()))
					.ToList(),
				AllInternalThreats = new AllThreatsModel(allInternalThreats),
				AllExternalThreats = new AllThreatsModel(allExternalThreats),
				NewGameModel = newGameModel
			};
			var modelsString = JavaScriptConvert.SerializeObject(inputModel);
			return View("Input", modelsString);
		}

		[HttpPost]
		[ActionName("Index")]
		public ActionResult IndexPost(string newGameText)
		{
			var newGameModel = new JavaScriptSerializer().Deserialize<NewGameModel>(newGameText);
			var game = newGameModel.ConvertToGame();
			var models = new List<GameSnapshotModel>();
			var currentTurnModels = new Dictionary<ResolutionPhase, GameSnapshotModel>();
			game.PhaseStarting += (sender, eventArgs) => currentTurnModels[eventArgs.Phase] = null;
			game.PhaseEnded += (sender, eventArgs) => currentTurnModels[eventArgs.Phase] = (new GameSnapshotModel((Game)sender, eventArgs.Phase));
			game.StartGame();
			var lost = false;
			for (var i = 0; i < game.NumberOfTurns && !lost; i++)
			{
				try
				{
					game.PerformTurn();
					models.AddRange(currentTurnModels.Select(currentTurnModel => currentTurnModel.Value));
					currentTurnModels.Clear();
				}
				catch (LoseException)
				{
					var currentPhase = currentTurnModels.Single(modelWithPhase => modelWithPhase.Value == null).Key;
					currentTurnModels[currentPhase] = new GameSnapshotModel (game, currentPhase);
					models.AddRange(currentTurnModels.Values);
					lost = true;
				}
			}

			var modelsByTurn = models.GroupBy(model => model.TurnNumber).ToList();
			var modelsString = JavaScriptConvert.SerializeObject(modelsByTurn);
			return View("Resolution", modelsString);
		}
	}
}
