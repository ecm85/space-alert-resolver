using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;
using BLL;
using BLL.Players;
using BLL.ShipComponents;
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
		[Route("NewGameInput")]
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
				SingleActions = ActionModel.AllSingleActionModels.OrderBy(action => action.FirstAction).ThenBy(action => action.SecondAction),
				DoubleActions = ActionModel.AllSelectableDoubleActionModels.OrderBy(action => action.FirstAction).ThenBy(action => action.SecondAction),
				SpecializationActions = PlayerSpecializationActionModel.AllPlayerSpecializationActionModels
					.OrderBy(action => action.PlayerSpecialization)
					.ThenBy(player => player.Hotkey),
				Tracks = TrackFactory.CreateAllTracks()
					.Select(track => new TrackSnapshotModel(track, new List<int>()))
					.ToList(),
				AllInternalThreats = new AllThreatsModel(allInternalThreats),
				AllExternalThreats = new AllThreatsModel(allExternalThreats),
				PlayerSpecializations = EnumFactory.All<PlayerSpecialization>().ToList(),
				AllDamageTokens = EnumFactory.All<DamageToken>(),
				DamageableZones = EnumFactory.All<ZoneLocation>()
		};
			return inputModel;
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		[HttpPost]
		[Route("ProcessGame")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Really?")]
		public IList<IGrouping<int, GameSnapshotModel>> ProcessGame([FromBody]NewGameModel newGameModel)
		{
			var game = newGameModel.ConvertToGame();
			if (game.Players.First().Actions.All(action => action.FirstActionSegment.SegmentType == PlayerActionType.BattleBots))
				throw new InvalidOperationException("Successfully triggered test Exception. You can't do that many battle bots!");
			var models = new List<GameSnapshotModel>();
			var currentTurnModels = new Dictionary<string, GameSnapshotModel>();
			game.PhaseStarting += (sender, eventArgs) => currentTurnModels[eventArgs.PhaseHeader + "," + eventArgs.PhaseSubHeader] = null;
			game.PhaseEnded += (sender, eventArgs) => currentTurnModels[eventArgs.PhaseHeader + "," + eventArgs.PhaseSubHeader] = new GameSnapshotModel((Game)sender, eventArgs.PhaseHeader, eventArgs.PhaseSubHeader);
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
					var currentPhaseTokens = currentPhase.Split(',');
					currentTurnModels[currentPhase] = new GameSnapshotModel(game, currentPhaseTokens[0], currentPhaseTokens[1]);
					models.AddRange(currentTurnModels.Values);
					lost = true;
				}
			}

			return models.GroupBy(model => model.TurnNumber).ToList();
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Really?")]
		[HttpPost]
		[Route("SendGameMessage")]
		public void SendGameMessage([FromBody]SendGameMessageModel model, string senderEmailAddress)
		{
			EmailService.SendEmail(model.MessageText, senderEmailAddress);
		}
	}
}
