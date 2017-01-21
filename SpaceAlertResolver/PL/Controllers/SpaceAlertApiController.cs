using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;
using BLL;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;
using PL.Models;

namespace PL.Controllers
{
	public class SpaceAlertApiController : ApiController
	{
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Really?")]
		[HttpGet]
		public InputModel NewGameInput()
		{
			var allExternalThreats = ExternalThreatFactory.AllExternalThreats
				.Select(threat => new ExternalThreatModel(threat))
				.ToList();
			var allInternalThreats = InternalThreatFactory.AllInternalThreats
				.Select(threat => new InternalThreatModel(threat))
				.ToList();
			var inputModel = new InputModel
			{
				Actions = ActionModel.AllActionModels,
				Tracks = EnumFactory.All<TrackConfiguration>()
					.Select(trackConfiguration => new Track(trackConfiguration))
					.Select(track => new TrackSnapshotModel(track, new List<int>()))
					.ToList(),
				AllInternalThreats = new AllThreatsModel(allInternalThreats),
				AllExternalThreats = new AllThreatsModel(allExternalThreats)
			};
			return inputModel;
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		[HttpPost]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Really?")]
		public IList<IGrouping<int, GameSnapshotModel>> ProcessGame([FromBody]NewGameModel newGameModel)
		{
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
					currentTurnModels[currentPhase] = new GameSnapshotModel(game, currentPhase);
					models.AddRange(currentTurnModels.Values);
					lost = true;
				}
			}

			return models.GroupBy(model => model.TurnNumber).ToList();
		}
	}
}