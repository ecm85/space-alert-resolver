using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.External.Minor.White;
using BLL.Threats.Internal;
using BLL.Threats.Internal.Minor.White;
using BLL.Threats.Internal.Serious.White;
using BLL.Threats.Internal.Serious.Yellow;
using BLL.Tracks;

namespace ConsoleResolver
{
	public class Program
	{
		static void Main(string[] args)
		{
			if (!args.Any())
			{
				Console.WriteLine("Usage: ConsoleResolver");
				Console.WriteLine("-tracks blue:<int> white:<int> red:<int>");
				Console.WriteLine("-threats [id:<string> time:<int> (optional)location:<red|white|blue> [extra-threat-id:<string>]? ]+");
				Console.WriteLine("-players <int> [player-index:<int> actions:<string>]+)");
			}
			var players = GetPlayers();

			var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
			{
				{ZoneLocation.Blue, TrackConfiguration.Track1},
				{ZoneLocation.Red, TrackConfiguration.Track2},
				{ZoneLocation.White, TrackConfiguration.Track3},
			};
			var internalTrack = TrackConfiguration.Track4;

			var destroyer = new Destroyer { TimeAppears = 4, CurrentZone = ZoneLocation.Blue };
			var fighter1 = new Fighter { TimeAppears = 5, CurrentZone = ZoneLocation.Red };
			var fighter2 = new Fighter { TimeAppears = 6, CurrentZone = ZoneLocation.White };
			var externalThreats = new ExternalThreat[] { destroyer, fighter1, fighter2 };

			var skirmishers = new SkirmishersA { TimeAppears = 4 };
			var fissure = new Fissure { TimeAppears = 2 };
			var nuclearDevice = new NuclearDevice { TimeAppears = 6 };
			var internalThreats = new InternalThreat[] { skirmishers, fissure, nuclearDevice };

			var bonusThreats = new Threat[0];

			var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack);

			var currentTurn = 0;
			try
			{
				for (currentTurn = 0; currentTurn < game.NumberOfTurns; currentTurn++)
					game.PerformTurn();
			}
			catch (LoseException loseException)
			{
				Console.WriteLine("Killed on turn {0} by: {1}", currentTurn + 1, loseException.Threat);
			}
			Console.WriteLine("Damage Taken:\r\nBlue: {0}\r\nRed: {1}\r\nWhite: {2}",
				game.SittingDuck.BlueZone.TotalDamage,
				game.SittingDuck.RedZone.TotalDamage,
				game.SittingDuck.WhiteZone.TotalDamage);
			Console.WriteLine("Threats killed: {0}. Threats survived: {1}", game.ThreatController.DefeatedThreats.Count(), game.ThreatController.SurvivedThreats.Count());
			Console.WriteLine("Total points: {0}", game.TotalPoints);
			foreach (var zone in game.SittingDuck.Zones)
			{
				foreach (var token in zone.AllDamageTokensTaken)
					Console.WriteLine("{0} damage token taken in zone {1}. Still damaged: {2}", token, zone.ZoneLocation, zone.CurrentDamageTokens.Contains(token));
			}
		}

		private static IList<Player> GetPlayers()
		{
			var players = Enumerable.Range(0, 6).Select(index => new Player()).ToList();
			players[0].Actions = PlayerActionFactory.CreateSingleActionList(players[0], new PlayerActionType?[]
			{
				null,
				PlayerActionType.ChangeDeck,
				PlayerActionType.B,
				PlayerActionType.ChangeDeck,
				PlayerActionType.A,
				PlayerActionType.A,
				PlayerActionType.A,
				PlayerActionType.A,
				PlayerActionType.A,
				PlayerActionType.A
			});
			players[0].Index = 0;
			players[1].Actions = PlayerActionFactory.CreateSingleActionList(players[1], new PlayerActionType?[]
			{
				PlayerActionType.MoveRed,
				PlayerActionType.ChangeDeck,
				PlayerActionType.C,
				PlayerActionType.ChangeDeck,
				null,
				PlayerActionType.C,
				PlayerActionType.BattleBots,
				PlayerActionType.C,
				PlayerActionType.BattleBots,
				PlayerActionType.A
			});
			players[1].Index = 1;
			players[2].Actions = PlayerActionFactory.CreateSingleActionList(players[2], new PlayerActionType?[]
			{
				null,
				PlayerActionType.C,
				null,
				null,
				PlayerActionType.C,
				PlayerActionType.ChangeDeck,
				PlayerActionType.C
			});
			players[2].Index = 2;
			players[3].Actions = PlayerActionFactory.CreateSingleActionList(players[3], new PlayerActionType?[]
			{
				PlayerActionType.ChangeDeck,
				null,
				null,
				null,
				null,
				null,
				PlayerActionType.C
			});
			players[3].Index = 3;
			players[4].Actions = PlayerActionFactory.CreateSingleActionList(players[4], new PlayerActionType?[]
			{
				null,
				PlayerActionType.ChangeDeck,
				null,
				null,
				null,
				null,
				PlayerActionType.C
			});
			players[4].Index = 4;
			players[5].Actions = PlayerActionFactory.CreateSingleActionList(players[5], new PlayerActionType?[]
			{
				PlayerActionType.TeleportBlueLower,
				PlayerActionType.TeleportRedUpper,
				PlayerActionType.TeleportWhiteLower,
				PlayerActionType.TeleportWhiteUpper
			});
			players[5].Index = 5;
			return players;
		}
	}
}
