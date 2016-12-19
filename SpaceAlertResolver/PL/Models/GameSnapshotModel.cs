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
			RedThreats = GetThreatsInZone(game, ZoneLocation.Red).Select(threat => new ExternalThreatSnapshotModel(threat)).ToList();
			WhiteThreats = GetThreatsInZone(game, ZoneLocation.White).Select(threat => new ExternalThreatSnapshotModel(threat)).ToList();
			BlueThreats = GetThreatsInZone(game, ZoneLocation.Blue).Select(threat => new ExternalThreatSnapshotModel(threat)).ToList();
			InternalThreats = game.ThreatController.InternalThreatsOnTrack.Select(threat => new InternalThreatSnapshotModel(threat)).ToList();
			Players = game.Players.Select(player => new PlayerSnapshotModel(player)).ToList();
			RedTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.Red]);
			WhiteTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.White]);
			BlueTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.Blue]);
			InternalTrack = new TrackSnapshotModel(game.ThreatController.InternalTrack);
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
