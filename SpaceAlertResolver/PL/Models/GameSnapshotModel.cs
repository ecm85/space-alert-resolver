using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;
using BLL.Threats.External;

namespace PL.Models
{
	public class GameSnapshotModel
	{
		public IEnumerable<ExternalThreatSnapshotModel> RedThreats { get; }
		public IEnumerable<ExternalThreatSnapshotModel> WhiteThreats { get; }
		public IEnumerable<ExternalThreatSnapshotModel> BlueThreats { get; }
		public IEnumerable<InternalThreatSnapshotModel> InternalThreats { get; }
		public IEnumerable<PlayerSnapshotModel> Players { get; }
		public TrackSnapshotModel RedTrack { get; }
		public TrackSnapshotModel WhiteTrack { get; }
		public TrackSnapshotModel BlueTrack { get; }
		public TrackSnapshotModel InternalTrack { get; }
		public string Description { get; }
		public int Turn { get; }
		public int Phase { get; }
		public GameSnapshotModel(Game game, string description, Func<int> getPhase)
		{
			var redThreats = GetThreatsInZone(game, ZoneLocation.Red).ToList();
			var whiteThreats = GetThreatsInZone(game, ZoneLocation.White).ToList();
			var blueThreats = GetThreatsInZone(game, ZoneLocation.Blue).ToList();
			var internalThreats = game.ThreatController.InternalThreatsOnTrack.ToList();

			RedThreats = redThreats.Select(threat => new ExternalThreatSnapshotModel(threat)).ToList();
			WhiteThreats = whiteThreats.Select(threat => new ExternalThreatSnapshotModel(threat)).ToList();
			BlueThreats = blueThreats.Select(threat => new ExternalThreatSnapshotModel(threat)).ToList();
			InternalThreats = internalThreats.Select(threat => new InternalThreatSnapshotModel(threat)).ToList();
			Players = game.Players.Select(player => new PlayerSnapshotModel(player)).ToList();
			RedTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.Red], redThreats);
			WhiteTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.White], whiteThreats);
			BlueTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.Blue], blueThreats);
			InternalTrack = new TrackSnapshotModel(game.ThreatController.InternalTrack, internalThreats);
			Description = description;
			Turn = game.CurrentTurn;
			Phase = getPhase();
		}

		private static IEnumerable<ExternalThreat> GetThreatsInZone(Game game, ZoneLocation zoneLocation)
		{
			return game.ThreatController.ExternalThreatsOnTrack.Where(threat => threat.CurrentZone == zoneLocation);
		}
	}
}
