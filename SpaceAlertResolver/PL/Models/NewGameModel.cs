using System.Collections.Generic;
using System.Linq;
using BLL;
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
				.Select(player => new Player(
					PlayerActionFactory.CreateSingleActionList(
						null,
						null,
						player.Actions.Select(action => action.Action).ToList()),
					player.Index,
					player.PlayerColor))
				.ToList();
			var internalThreats = CreateInternalThreatModels(InternalThreats);
			var redThreats = CreateExternalThreatModels(RedThreats, ZoneLocation.Red);
			var whiteThreats = CreateExternalThreatModels(WhiteThreats, ZoneLocation.White);
			var blueThreats = CreateExternalThreatModels(BlueThreats, ZoneLocation.Blue);
			var externalThreats = redThreats.Concat(whiteThreats).Concat(blueThreats).ToList();
			var bonusThreats = new List<Threat>();
			return new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack);
		}

		private static IList<ExternalThreat> CreateExternalThreatModels(IEnumerable<ExternalThreatModel> threatModels, ZoneLocation zone)
		{
			return threatModels
				.Select(threatModel =>
				{
					var threat = ExternalThreatFactory.CreateThreat<ExternalThreat>(threatModel.Id);
					threat.TimeAppears = threatModel.TimeAppears;
					threat.CurrentZone = zone;
					return threat;
				})
				.ToList();
		}

		private static IList<InternalThreat> CreateInternalThreatModels(IEnumerable<InternalThreatModel> threatModels)
		{
			return threatModels
				.Select(threatModel =>
				{
					var threat = InternalThreatFactory.CreateThreat<InternalThreat>(threatModel.Id);
					threat.TimeAppears = threatModel.TimeAppears;
					return threat;
				})
				.ToList();
		}
	}
}
