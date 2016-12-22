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
		public IEnumerable<ExternalThreatModel> RedThreats { get; }
		public IEnumerable<ExternalThreatModel> WhiteThreats { get; }
		public IEnumerable<ExternalThreatModel> BlueThreats { get; }
		public IEnumerable<InternalThreatModel> InternalThreats { get; }
		public IEnumerable<PlayerModel> Players { get; }
		public TrackSnapshotModel RedTrack { get; }
		public TrackSnapshotModel WhiteTrack { get; }
		public TrackSnapshotModel BlueTrack { get; }
		public TrackSnapshotModel InternalTrack { get; }
		public IEnumerable<PlayerModel> UpperRedPlayers { get; }
		public IEnumerable<PlayerModel> LowerRedPlayers { get; }
		public IEnumerable<PlayerModel> UpperWhitePlayers { get; }
		public IEnumerable<PlayerModel> LowerWhitePlayers { get; }
		public IEnumerable<PlayerModel> UpperBluePlayers { get; }
		public IEnumerable<PlayerModel> LowerBluePlayers { get; }

		public string Description { get; }
		public int Turn { get; }
		public int Phase { get; }
		public GameSnapshotModel(Game game, string description, Func<int> getPhase)
		{
			var redThreats = GetThreatsInZone(game, ZoneLocation.Red).ToList();
			var whiteThreats = GetThreatsInZone(game, ZoneLocation.White).ToList();
			var blueThreats = GetThreatsInZone(game, ZoneLocation.Blue).ToList();
			var internalThreats = game.ThreatController.InternalThreatsOnTrack.ToList();

			RedThreats = redThreats.Select(threat => new ExternalThreatModel(threat)).ToList();
			WhiteThreats = whiteThreats.Select(threat => new ExternalThreatModel(threat)).ToList();
			BlueThreats = blueThreats.Select(threat => new ExternalThreatModel(threat)).ToList();
			InternalThreats = internalThreats.Select(threat => new InternalThreatModel(threat)).ToList();
			Players = game.Players.Select(player => new PlayerModel(player)).ToList();
			RedTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.Red], redThreats);
			WhiteTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.White], whiteThreats);
			BlueTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.Blue], blueThreats);
			InternalTrack = new TrackSnapshotModel(game.ThreatController.InternalTrack, internalThreats);
			UpperRedPlayers = GetPlayersInStation(game, StationLocation.UpperRed).Select(player => new PlayerModel(player)).ToList();
			LowerRedPlayers = GetPlayersInStation(game, StationLocation.LowerRed).Select(player => new PlayerModel(player)).ToList();
			UpperWhitePlayers = GetPlayersInStation(game, StationLocation.UpperWhite).Select(player => new PlayerModel(player)).ToList();
			LowerWhitePlayers = GetPlayersInStation(game, StationLocation.LowerWhite).Select(player => new PlayerModel(player)).ToList();
			UpperBluePlayers = GetPlayersInStation(game, StationLocation.UpperBlue).Select(player => new PlayerModel(player)).ToList();
			LowerBluePlayers = GetPlayersInStation(game, StationLocation.LowerBlue).Select(player => new PlayerModel(player)).ToList();
			Description = description;
			Turn = game.CurrentTurn;
			Phase = getPhase();
		}

		private static IEnumerable<Player> GetPlayersInStation(Game game, StationLocation location)
		{
			return game.Players.Where(player => player.CurrentStation.StationLocation == location);
		}

		private static IEnumerable<ExternalThreat> GetThreatsInZone(Game game, ZoneLocation zoneLocation)
		{
			return game.ThreatController.ExternalThreatsOnTrack.Where(threat => threat.CurrentZone == zoneLocation);
		}
	}
}
