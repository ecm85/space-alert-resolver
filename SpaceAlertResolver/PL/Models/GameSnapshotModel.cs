using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class GameSnapshotModel
	{
		public IEnumerable<InternalThreatModel> InternalThreats { get; }
		public IEnumerable<PlayerModel> Players { get; }
		public TrackSnapshotModel InternalTrack { get; }
		public ZoneModel RedZone { get; set; }
		public ZoneModel WhiteZone { get; set; }
		public ZoneModel BlueZone { get; set; }

		public string Description { get; }
		public int Turn { get; }
		public int Phase { get; }
		public GameSnapshotModel(Game game, string description, Func<int> getPhase)
		{
			var internalThreats = game.ThreatController.InternalThreatsOnTrack.ToList();
			RedZone = new ZoneModel(game, ZoneLocation.Red);
			WhiteZone = new ZoneModel(game, ZoneLocation.White);
			BlueZone = new ZoneModel(game, ZoneLocation.Blue);
			InternalThreats = internalThreats.Select(threat => new InternalThreatModel(threat)).ToList();
			Players = game.Players.Select(player => new PlayerModel(player)).ToList();
			InternalTrack = new TrackSnapshotModel(game.ThreatController.InternalTrack, internalThreats);
			Description = description;
			Turn = game.CurrentTurn;
			Phase = getPhase();
		}
	}
}
