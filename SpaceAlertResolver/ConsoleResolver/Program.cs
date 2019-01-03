using System;
using System.Linq;
using BLL;

namespace ConsoleResolver
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var game = ParseAndRunGame(args);
            return game == null ? -1 : 0;
        }

        public static Game ParseAndRunGame(string[] args)
        {
            try
            {
                var game = GameParser.ParseArgsIntoGame(args);
                RunGame(game);
                return game;
            }
            catch (InvalidOperationException exception)
            {
                HandleInvalidArgument(exception.Message);
                return null;
            }
        }

        private static void HandleInvalidArgument(string error = null)
        {
            if (!string.IsNullOrWhiteSpace(error))
                Console.WriteLine(error);
            Console.WriteLine("Usage: ConsoleResolver");
            Console.WriteLine("-external-tracks blue:<int> white:<int> red:<int>");
            Console.WriteLine("-internal-track <int>");
            Console.WriteLine(
                "-external-threats [id:<string> time:<int> location:<red|white|blue> [extra-external-threat-id:<string>]? [extra-internal-threat-id:<string>]? ]+");
            Console.WriteLine(
                "-internal-threats [id:<string> time:<int> [extra-threat-id:<string>]? ]+");
            Console.WriteLine("-players [player-index:<int> actions:<string>]+)");
        }

        public static void RunGame(Game game)
        {
            game.StartGame();
            game.LostGame += (sender, args) => 
                Console.WriteLine("Killed on turn {0} by: {1}", game.CurrentTurn, game.KilledBy);

            for (var currentTurn = 0; currentTurn < game.NumberOfTurns + 1; currentTurn++)
                game.PerformTurn();
            Console.WriteLine("Damage Taken:\r\nBlue: {0}\r\nRed: {1}\r\nWhite: {2}",
                game.SittingDuck.BlueZone.TotalDamage,
                game.SittingDuck.RedZone.TotalDamage,
                game.SittingDuck.WhiteZone.TotalDamage);
            Console.WriteLine("Threats killed: {0}. Threats survived: {1}", game.ThreatController.DefeatedThreats.Count(),
                game.ThreatController.SurvivedThreats.Count());
            Console.WriteLine("Total points: {0}", game.TotalPoints);
            foreach (var zone in game.SittingDuck.Zones)
            {
                foreach (var token in zone.CurrentDamageTokens)
                    Console.WriteLine("{0} damage token taken in zone {1}. Still damaged: {2}", token, zone.ZoneLocation,
                        zone.CurrentDamageTokens.Contains(token));
            }
        }

        
    }
}
