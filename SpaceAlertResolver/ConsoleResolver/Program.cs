using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace ConsoleResolver
{
	public static class Program
	{
		private static int Main(string[] args)
		{
			if (!args.Any())
				return HandleInvalidArgument();
			var validArguments = args.Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
			var chunkIdentifiers = new[] {"-external-tracks", "-internal-track", "-players", "-external-threats", "-internal-threats"};
			var chunks = ChunkArguments(validArguments, chunkIdentifiers).ToList();
			if (chunks.Count() != 5 || chunkIdentifiers.Except(chunks.Select(chunk => chunk[0])).Any())
				return HandleInvalidArgument("Invalid arguments.");

			var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>();
			TrackConfiguration internalTrackConfiguration = TrackConfiguration.Track1;

			foreach (var chunk in chunks)
			{
				var chunkType = chunk[0];
				switch (chunkType)
				{
					case "-external-tracks":
						if (chunk.Count != 4)
							return HandleInvalidArgument("Invalid external tracks.");
						var trackStrings = chunk.Skip(1).OrderBy(token => token).ToList();
						foreach (var trackString in trackStrings)
						{
							var zoneLocation = ParseZoneLocation(trackString);
							if(zoneLocation == null)
								return HandleInvalidArgument("Invalid external track: " + trackString);
							externalTracksByZone[zoneLocation.Item1] = zoneLocation.Item2;
						}
						break;
					case "-internal-track":
						if (chunk.Count != 2)
							return HandleInvalidArgument("Invalid internal tracks.");
						if (!(Enum.TryParse(chunk[1], true, out internalTrackConfiguration)))
							return HandleInvalidArgument("Invalid internal track: " + chunk[1]);
						break;
					case "-players":
						break;
					case "-external-threats":
						break;
					case "-internal-threats":
						break;
				}
			}

			Console.WriteLine("Internal: {0}", internalTrackConfiguration);
			foreach (var trackConfiguration in externalTracksByZone)
			{
				Console.WriteLine("{0}: {1}", trackConfiguration.Key, trackConfiguration.Value);
			}

			return 0;
		}

		private static IEnumerable<IList<string>> ChunkArguments(IList<string> arguments, params string[] splitters)
		{
			var remainingArguments = arguments.ToList();
			while (remainingArguments.Any())
			{
				var nextList = remainingArguments.Take(1).ToList();
				remainingArguments = remainingArguments.Skip(1).ToList();
				nextList = nextList
					.Concat(remainingArguments.TakeWhile(argument => !splitters.Contains(argument)))
					.ToList();
				remainingArguments = remainingArguments
					.SkipWhile(argument => !splitters.Contains(argument))
					.ToList();
				yield return nextList;
			}
		}

		private static int HandleInvalidArgument(string error = null)
		{
			if (!string.IsNullOrWhiteSpace(error))
				Console.WriteLine(error);
			Console.WriteLine("Usage: ConsoleResolver");
			Console.WriteLine("-external-tracks blue:<int> white:<int> red:<int>");
			Console.WriteLine("-internal-track <int>");
			Console.WriteLine(
				"-external-threats [id:<string> time:<int> location:<red|white|blue> [extra-threat-id:<string>]? ]+");
			Console.WriteLine(
				"-internal-threats [id:<string> time:<int> [extra-threat-id:<string>]? ]+");
			Console.WriteLine("-players count:<int> [player-index:<int> actions:<string>]+)");
			return -1;
		}

		private static Tuple<ZoneLocation, TrackConfiguration> ParseZoneLocation(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return null;
			var pieces = token.Split(new [] {":"}, StringSplitOptions.RemoveEmptyEntries);
			if (pieces.Count() != 2)
				return null;
			ZoneLocation zoneLocation;
			TrackConfiguration trackConfiguration;
			if (!Enum.TryParse(pieces[0], true, out zoneLocation) || !(Enum.TryParse(pieces[1], true, out trackConfiguration)))
				return null;
			return Tuple.Create(zoneLocation, trackConfiguration);
		}
	}
}
