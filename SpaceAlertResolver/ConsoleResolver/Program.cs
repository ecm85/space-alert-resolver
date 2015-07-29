using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
			var chunkIdentifiers = new[] {"-tracks", "-players", "-threats"};
			var chunks = ChunkArguments(validArguments, chunkIdentifiers).ToList();
			if (chunks.Count() != 3 || chunkIdentifiers.Except(chunks.Select(chunk => chunk[0])).Any())
				return HandleInvalidArgument("Invalid arguments.");

			var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>();
			TrackConfiguration internalTrackConfiguration = TrackConfiguration.Track1;

			foreach (var chunk in chunks)
			{
				var chunkType = chunk[0];
				switch (chunkType)
				{
					case "-tracks":
						if (chunk.Count != 5)
							return HandleInvalidArgument("Invalid tracks.");
						var trackStrings = chunk.Skip(1).OrderBy(token => token).ToList();
						if (!Regex.IsMatch(trackStrings[0], @"^blue:\d$") ||
							!Regex.IsMatch(trackStrings[1], @"^internal:\d$") ||
							!Regex.IsMatch(trackStrings[2], @"^red:\d$") ||
							!Regex.IsMatch(trackStrings[3], @"^white:\d$"))
							return HandleInvalidArgument("Invalid tracks.");
						externalTracksByZone[ZoneLocation.Blue] = (TrackConfiguration)Enum.Parse(typeof(TrackConfiguration), trackStrings[0].Substring(5));
						internalTrackConfiguration = (TrackConfiguration)Enum.Parse(typeof(TrackConfiguration), trackStrings[1].Substring(9));
						externalTracksByZone[ZoneLocation.Red] = (TrackConfiguration)Enum.Parse(typeof(TrackConfiguration), trackStrings[2].Substring(4));
						externalTracksByZone[ZoneLocation.White] = (TrackConfiguration)Enum.Parse(typeof(TrackConfiguration), trackStrings[3].Substring(6));
						break;
					case "-players":
						break;
					case "-threats":
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
			Console.WriteLine("-tracks blue:<int> white:<int> red:<int> internal:<int>");
			Console.WriteLine(
				"-threats [id:<string> time:<int> (optional)location:<red|white|blue> [extra-threat-id:<string>]? ]+");
			Console.WriteLine("-players <int> [player-index:<int> actions:<string>]+)");
			return -1;
		}
	}
}
