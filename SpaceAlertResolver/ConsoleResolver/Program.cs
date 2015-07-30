using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;

namespace ConsoleResolver
{
	public static class Program
	{
		private class ExternalThreatInfo
		{
			public ExternalThreat Threat { get; set; }
			public int? TimeAppears { get; set; }
			public BonusInternalThreatInfo BonusInternalThreatInfo { get; set; }
			public BonusExternalThreatInfo BonusExternalThreatInfo { get; set; }
			public ZoneLocation? ZoneLocation { get; set; }

			public bool IsValid()
			{
				if (Threat == null || TimeAppears == null || ZoneLocation == null)
					return false;
				if (Threat.NeedsBonusInternalThreat)
					if(BonusInternalThreatInfo == null || !BonusInternalThreatInfo.IsValid())
						return false;
				if (Threat.NeedsBonusExternalThreat)
					if(BonusExternalThreatInfo == null || !BonusExternalThreatInfo.IsValid())
						return false;
				return true;
			}
		}

		private class InternalThreatInfo
		{
			public InternalThreat Threat { get; set; }
			public int? TimeAppears { get; set; }
			public BonusInternalThreatInfo BonusInternalThreatInfo { get; set; }
			public BonusExternalThreatInfo BonusExternalThreatInfo { get; set; }
			public bool IsValid()
			{
				if (Threat == null || TimeAppears == null)
					return false;
				if (Threat.NeedsBonusInternalThreat)
					if(BonusInternalThreatInfo == null || !BonusInternalThreatInfo.IsValid())
						return false;
				if (Threat.NeedsBonusExternalThreat)
					if(BonusExternalThreatInfo == null || !BonusExternalThreatInfo.IsValid())
						return false;
				return true;
			}
		}

		private class BonusInternalThreatInfo
		{
			public ExternalThreat Threat { get; set; }
			public BonusInternalThreatInfo NextBonusInternalThreatInfo { get; set; }
			public BonusExternalThreatInfo NextBonusExternalThreatInfo { get; set; }

			public bool IsValid()
			{
				if (Threat == null)
					return false;
				if (Threat.NeedsBonusInternalThreat)
					if(NextBonusInternalThreatInfo == null || !NextBonusInternalThreatInfo.IsValid())
						return false;
				if (Threat.NeedsBonusExternalThreat)
					if(NextBonusExternalThreatInfo == null || !NextBonusExternalThreatInfo.IsValid())
						return false;
				return true;
			}
		}

		private class BonusExternalThreatInfo
		{
			public ExternalThreat Threat { get; set; }
			public ZoneLocation? ZoneLocation { get; set; }
			public BonusInternalThreatInfo NextBonusInternalThreatInfo { get; set; }
			public BonusExternalThreatInfo NextBonusExternalThreatInfo { get; set; }

			public bool IsValid()
			{
				if (Threat == null || ZoneLocation == null)
					return false;
				if (Threat.NeedsBonusInternalThreat)
					if(NextBonusInternalThreatInfo == null || !NextBonusInternalThreatInfo.IsValid())
						return false;
				if (Threat.NeedsBonusExternalThreat)
					if(NextBonusExternalThreatInfo == null || !NextBonusExternalThreatInfo.IsValid())
						return false;
				return true;
			}
		}

		private static int Main(string[] args)
		{
			foreach (var threat in ThreatFactory.ThreatTypesByDisplayName)
			{
				try
				{
					Activator.CreateInstance(threat.Key);
					//Console.WriteLine("Created {0}", threat.Value);
				}
				catch (Exception)
				{
					Console.WriteLine("Could not create {0}", threat.Value);
				}
			}

			if (!args.Any())
				return HandleInvalidArgument();
			var validArguments = args.Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
			var chunkIdentifiers = new[] {"-external-tracks", "-internal-track", "-players", "-external-threats", "-internal-threats"};
			var chunks = ChunkArguments(validArguments, chunkIdentifiers).ToList();
			if (chunks.Count() != 5 || chunkIdentifiers.Except(chunks.Select(chunk => chunk[0])).Any())
				return HandleInvalidArgument("Invalid arguments.");

			var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>();
			TrackConfiguration? internalTrackConfiguration = TrackConfiguration.Track1;
			var externalThreats = new List<ExternalThreat>();

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
							var zoneLocation = ParseZoneLocationWithTrackConfiguration(trackString);
							if(zoneLocation == null)
								return HandleInvalidArgument("Invalid external track: " + trackString);
							externalTracksByZone[zoneLocation.Item1] = zoneLocation.Item2;
						}
						break;
					case "-internal-track":
						if (chunk.Count != 2)
							return HandleInvalidArgument("Invalid internal tracks.");
						internalTrackConfiguration = TryParseEnum<TrackConfiguration>(chunk[1]);
						if (!internalTrackConfiguration.HasValue)
							return HandleInvalidArgument("Invalid internal track: " + chunk[1]);
						break;
					case "-players":
						break;
					case "-external-threats":
						var threatTokens = new Queue<string>(chunk.Skip(1));
						var nextThreatInfo = new ExternalThreatInfo();
						while (threatTokens.Any())
						{
							var nextToken = ParseToken(threatTokens.Dequeue());
							if (nextToken == null)
								return HandleInvalidArgument("Error on external threat #" + (externalThreats.Count + 1));
							switch (nextToken.Item1)
							{
								case "id":
									//TODO: Create the threat, throwing if already created
									break;
								case "time":
									//TODO: Set the time on the info, throwing if already set
									break;
								case "location":
									//TODO: Set the location on the info, throwing if already set
									break;
								case "extra-internal-threat-id":
									//TODO: Create the threat bonus info (recursively find last one)
									break;
								case "extra-external-threat-id":
									//TODO: Create the threat bonus info (recursively find last one)
									break;
								case "extra-external-threat-location":
									//TODO: Set the location on the last threat bonus info (or throw if it's already set) (recursively find last one)
									break;
								default:
									return HandleInvalidArgument("Error on external threat #" + (externalThreats.Count + 1));
							}
							if (nextThreatInfo.IsValid())
							{
								var threat = nextThreatInfo.Threat;
								threat.CurrentZone = nextThreatInfo.ZoneLocation.GetValueOrDefault();
								threat.TimeAppears = nextThreatInfo.TimeAppears.GetValueOrDefault();
								if (threat.NeedsBonusInternalThreat)
									CreateBonusThreat(threat, nextThreatInfo.BonusInternalThreatInfo);
								if (threat.NeedsBonusExternalThreat)
									CreateBonusThreat(threat, nextThreatInfo.BonusExternalThreatInfo);
								externalThreats.Add(threat);
								nextThreatInfo = new ExternalThreatInfo();
							}
						}

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

		private static void CreateBonusThreat(Threat threatToAddTo, BonusInternalThreatInfo bonusInternalThreatInfo)
		{
			//TODO: Create bonus threat. Check if it needs a threat, and recurse if so.
		}

		private static void CreateBonusThreat(Threat threatToAddTo, BonusExternalThreatInfo bonusExternalThreatInfo)
		{
			//TODO: Create bonus threat. Check if it needs a threat, and recurse if so.
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
				"-external-threats [id:<string> time:<int> location:<red|white|blue> [extra-external-threat-id:<string> extra-external-threat-location: <red|white|blue>]? [extra-internal-threat-id:<string>]? ]+");
			Console.WriteLine(
				"-internal-threats [id:<string> time:<int> [extra-threat-id:<string>]? ]+");
			Console.WriteLine("-players count:<int> [player-index:<int> actions:<string>]+)");
			return -1;
		}

		private static Tuple<ZoneLocation, TrackConfiguration> ParseZoneLocationWithTrackConfiguration(string token)
		{
			var pieces = ParseToken(token);
			if (pieces == null)
				return null;
			var zoneLocation = TryParseEnum<ZoneLocation>(pieces.Item1);
			var trackConfiguration = TryParseEnum<TrackConfiguration>(pieces.Item2);
			if (zoneLocation == null || trackConfiguration == null)
				return null;
			return Tuple.Create(zoneLocation.Value, trackConfiguration.Value);
		}

		private static TEnum? TryParseEnum<TEnum>(string value) where TEnum : struct
		{
			TEnum result;
			var success = Enum.TryParse(value, true, out result);
			if (success && Enum.IsDefined(typeof(TEnum), result))
				return result;
			return null;
		}

		private static Tuple<string, string> ParseToken(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return null;
			var pieces = token.Split(new [] {":"}, StringSplitOptions.RemoveEmptyEntries);
			if (pieces.Count() != 2)
				return null;
			return Tuple.Create(pieces[0], pieces[1]);
		}
	}
}
