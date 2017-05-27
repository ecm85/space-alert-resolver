using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.Players;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;

namespace PL.Models
{
	public class NewGameModel
	{
		public IEnumerable<PlayerModel> Players { get; set; }
		public IEnumerable<ExternalThreatModel> RedThreats { get; set; }
		public IEnumerable<ExternalThreatModel> WhiteThreats { get; set; }
		public IEnumerable<ExternalThreatModel> BlueThreats { get; set; }
		public IEnumerable<InternalThreatModel> InternalThreats { get; set; }
		public TrackSnapshotModel RedTrack { get; set; }
		public TrackSnapshotModel WhiteTrack { get; set; }
		public TrackSnapshotModel BlueTrack { get; set; }
		public TrackSnapshotModel InternalTrack { get; set; }
		public IEnumerable<InitialDamageModel> InitialDamageModels { get; set; }

		public Game ConvertToGame()
		{
			//TODO: For all 4 tracksTry using trackConfiguration instead of using TrackIndex and casting
			var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
			{
				{ZoneLocation.Red, (TrackConfiguration)RedTrack.TrackIndex},
				{ZoneLocation.White, (TrackConfiguration)WhiteTrack.TrackIndex},
				{ZoneLocation.Blue, (TrackConfiguration)BlueTrack.TrackIndex}
			};
			var internalTrack = (TrackConfiguration)InternalTrack.TrackIndex;

			var players = Players
				.Select(playerModel => PlayerFactory.CreatePlayer(
					playerModel.Actions.Select(action => PlayerActionFactory.CreatePlayerAction(action.FirstAction, action.SecondAction, action.BonusAction.FirstAction)),
					playerModel.Index,
					playerModel.PlayerColor,
					playerModel.PlayerSpecialization))
				.ToList();
			var internalThreats = CreateInternalThreats(InternalThreats);
			var redThreats = CreateExternalThreats(RedThreats, ZoneLocation.Red);
			var whiteThreats = CreateExternalThreats(WhiteThreats, ZoneLocation.White);
			var blueThreats = CreateExternalThreats(BlueThreats, ZoneLocation.Blue);
			var externalThreats = redThreats.Concat(whiteThreats).Concat(blueThreats).ToList();
			var allThreats = redThreats.Cast<Threat>().Concat(whiteThreats).Concat(blueThreats).Concat(internalThreats).ToList();
			var bonusExternalThreats = allThreats
				.Where(threat => threat.NeedsBonusExternalThreat)
				.Select(threat => ((IThreatWithBonusThreat<ExternalThreat>)threat).BonusThreat);
			var bonusInternalThreats = allThreats
				.Where(threat => threat.NeedsBonusExternalThreat)
				.Select(threat => ((IThreatWithBonusThreat<ExternalThreat>)threat).BonusThreat);
			var bonusThreats = bonusExternalThreats.Cast<Threat>().Concat(bonusInternalThreats).ToList();
			var damageTokens = InitialDamageModels.ToLookup(model => model.ZoneLocation, model => model.DamageToken);
			return new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack, damageTokens);
		}

		private static IList<ExternalThreat> CreateExternalThreats(IEnumerable<ExternalThreatModel> threatModels, ZoneLocation? zone = null)
		{
			return threatModels
				.Select(threatModel =>
				{
					var threat = ExternalThreatFactory.CreateThreat<ExternalThreat>(threatModel.Id);
					threat.TimeAppears = threatModel.TimeAppears;
					if (zone != null)
						threat.CurrentZone = zone.Value;
					if (threat.NeedsBonusInternalThreat)
						((IThreatWithBonusThreat<InternalThreat>)threat).BonusThreat = InternalThreatFactory.CreateThreat<InternalThreat>(threatModel.BonusInternalThreat.Id);
					if (threat.NeedsBonusExternalThreat)
						((IThreatWithBonusThreat<ExternalThreat>)threat).BonusThreat = ExternalThreatFactory.CreateThreat<ExternalThreat>(threatModel.BonusExternalThreat.Id);
					return threat;
				})
				.ToList();
		}

		private static IList<InternalThreat> CreateInternalThreats(IEnumerable<InternalThreatModel> threatModels)
		{
			return threatModels
				.Select(threatModel =>
				{
					var threat = InternalThreatFactory.CreateThreat<InternalThreat>(threatModel.Id);
					threat.TimeAppears = threatModel.TimeAppears;
					if (threat.NeedsBonusInternalThreat)
						((IThreatWithBonusThreat<InternalThreat>)threat).BonusThreat = InternalThreatFactory.CreateThreat<InternalThreat>(threatModel.BonusInternalThreat.Id);
					if (threat.NeedsBonusExternalThreat)
						((IThreatWithBonusThreat<ExternalThreat>)threat).BonusThreat = ExternalThreatFactory.CreateThreat<ExternalThreat>(threatModel.BonusExternalThreat.Id);
					return threat;
				})
				.ToList();
		}
	}
}
