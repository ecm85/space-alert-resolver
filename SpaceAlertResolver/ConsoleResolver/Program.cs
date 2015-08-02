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
		private abstract class ThreatInfo<T>
		{
			public T Threat { get; set; }
			public BonusInternalThreatInfo BonusInternalThreatInfo { get; set; }
			public BonusExternalThreatInfo BonusExternalThreatInfo { get; set; }
			public abstract bool IsValid();
		}

		private class ExternalThreatInfo : ThreatInfo<ExternalThreat>
		{
			public int? TimeAppears { get; set; }
			public ZoneLocation? ZoneLocation { get; set; }

			public override bool IsValid()
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

		private class InternalThreatInfo : ThreatInfo<InternalThreat>
		{
			public int? TimeAppears { get; set; }

			public override bool IsValid()
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

		private class BonusExternalThreatInfo : ThreatInfo<ExternalThreat>
		{
			public ZoneLocation? ZoneLocation { get; set; }

			public override bool IsValid()
			{
				if (Threat == null || ZoneLocation == null)
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

		private class BonusInternalThreatInfo : ThreatInfo<InternalThreat>
		{
			public override bool IsValid()
			{
				if (Threat == null)
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

		private static int Main(string[] args)
		{
			if (!args.Any())
				return HandleInvalidArgument();
			var validArguments = args.Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
			var chunkIdentifiers = new[] {"-external-tracks", "-internal-track", "-players", "-external-threats", "-internal-threats"};
			var chunks = ChunkArguments(validArguments, chunkIdentifiers).ToList();
			if (chunks.Count() != 5 || chunkIdentifiers.Except(chunks.Select(chunk => chunk[0])).Any())
				return HandleInvalidArgument("Invalid arguments.");

			IDictionary<ZoneLocation, TrackConfiguration> externalTracksByZone = null;
			TrackConfiguration? internalTrackConfiguration = null;
			IList<ExternalThreat> externalThreats = null;

			try
			{
				foreach (var chunk in chunks)
				{
					var chunkType = chunk[0];
					switch (chunkType)
					{
						case "-external-tracks":
							externalTracksByZone = ParseExternalTracks(chunk);
							break;
						case "-internal-track":
							internalTrackConfiguration = ParseInternalTrack(chunk);
							break;
						case "-players":
							break;
						case "-external-threats":
							externalThreats = ParseExternalThreats(chunk);
							break;
						case "-internal-threats":
							break;
					}
				}
			}
			catch (InvalidOperationException exception)
			{
				return HandleInvalidArgument(exception.Message);
			}

			Console.WriteLine("Internal: {0}", internalTrackConfiguration);
			if (externalTracksByZone != null)
				foreach (var trackConfiguration in externalTracksByZone)
					Console.WriteLine("{0}: {1}", trackConfiguration.Key, trackConfiguration.Value);
			if (externalThreats != null)
				foreach (var externalThreat in externalThreats)
				{
					Console.WriteLine("{0}, {1}, {2}", externalThreat.GetType(), externalThreat.TimeAppears, externalThreat.CurrentZone);
				}

			return 0;
		}

		private static IList<ExternalThreat> ParseExternalThreats(IEnumerable<string> chunk)
		{
			var externalThreats = new List<ExternalThreat>();
			var threatTokens = new Queue<string>(chunk.Skip(1));
			var nextThreatInfo = new ExternalThreatInfo();
			while (threatTokens.Any())
			{
				var nextToken = ParseToken(threatTokens.Dequeue());
				if (nextToken == null)
					throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
				switch (nextToken.Item1)
				{
					case "id":
						if (nextThreatInfo.Threat != null)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						nextThreatInfo.Threat = ThreatFactory.CreateThreat<ExternalThreat>(nextToken.Item2);
						if (nextThreatInfo.Threat == null)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						break;
					case "time":
						if (nextThreatInfo.TimeAppears.HasValue)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						nextThreatInfo.TimeAppears = TryParseInt(nextToken.Item2);
						if (!nextThreatInfo.TimeAppears.HasValue)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						break;
					case "location":
						if (nextThreatInfo.ZoneLocation.HasValue)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						nextThreatInfo.ZoneLocation = TryParseEnum<ZoneLocation>(nextToken.Item2);
						if (!nextThreatInfo.ZoneLocation.HasValue)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						break;
					case "extra-internal-threat-id":
						var bonusInternalThreat = ThreatFactory.CreateThreat<InternalThreat>(nextToken.Item2);
						if (bonusInternalThreat == null)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						AddBonusThreatInfo(nextThreatInfo, bonusInternalThreat);
						break;
					case "extra-external-threat-id":
						var bonusExternalThreat = ThreatFactory.CreateThreat<InternalThreat>(nextToken.Item2);
						if (bonusExternalThreat == null)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						AddBonusThreatInfo(nextThreatInfo, bonusExternalThreat);
						break;
					case "extra-external-threat-location":
						var bonusExternalThreatLocation = TryParseEnum<ZoneLocation>(nextToken.Item2);
						if (bonusExternalThreatLocation == null)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						SetBonusExternalThreatLocation(nextThreatInfo, bonusExternalThreatLocation.Value);
						break;
					default:
						throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
				}
				if (nextThreatInfo.IsValid())
				{
					var threat = nextThreatInfo.Threat;
					threat.CurrentZone = nextThreatInfo.ZoneLocation.GetValueOrDefault();
					threat.TimeAppears = nextThreatInfo.TimeAppears.GetValueOrDefault();
					if (threat.NeedsBonusInternalThreat)
						InitializeBonusThreat(threat, nextThreatInfo.BonusInternalThreatInfo);
					if (threat.NeedsBonusExternalThreat)
						InitializeBonusThreat(threat, nextThreatInfo.BonusExternalThreatInfo);
					externalThreats.Add(threat);
					nextThreatInfo = new ExternalThreatInfo();
				}
			}
			return externalThreats;
		}

		private static TrackConfiguration ParseInternalTrack(IList<string> chunk)
		{
			if (chunk.Count != 2)
				throw new InvalidOperationException("Invalid internal tracks.");
			var internalTrackConfiguration = TryParseEnum<TrackConfiguration>(chunk[1]);
			if (!internalTrackConfiguration.HasValue)
				throw new InvalidOperationException("Invalid internal track: " + chunk[1]);
			return internalTrackConfiguration.Value;
		}

		private static IDictionary<ZoneLocation, TrackConfiguration> ParseExternalTracks(IList<string> chunk)
		{
			var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>();
			if (chunk.Count != 4)
				throw new InvalidOperationException("Invalid external tracks.");
			var trackStrings = chunk.Skip(1).OrderBy(token => token).ToList();
			foreach (var trackString in trackStrings)
			{
				var zoneLocation = ParseZoneLocationWithTrackConfiguration(trackString);
				if (zoneLocation == null)
					throw new InvalidOperationException("Invalid external track: " + trackString);
				externalTracksByZone[zoneLocation.Item1] = zoneLocation.Item2;
			}
			return externalTracksByZone;
		}

		private static void InitializeBonusThreat(Threat threatToAddTo, BonusInternalThreatInfo bonusInternalThreatInfo)
		{
			//TODO: Initialize bonus threat. Check if it needs a threat, and recurse if so.
		}

		private static void InitializeBonusThreat(Threat threatToAddTo, BonusExternalThreatInfo bonusExternalThreatInfo)
		{
			//TODO: Initialize bonus threat. Check if it needs a threat, and recurse if so.
		}

		private static void AddBonusThreatInfo<T1, T2>(ThreatInfo<T1> threatInfo, T2 newThreat ) where T1: Threat where T2 : Threat
		{
			//TODO: Create bonus threat info. If there's a spot on current threat info, add it; otherwise recurse.
		}

		private static void SetBonusExternalThreatLocation<T>(ThreatInfo<T> threatInfo, ZoneLocation location)  where T: Threat
		{
			//TODO: if threat has a bonus info set, recurse
			//TODO: otherwise, if the location is null, set it; otherwise throw
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

		private static int? TryParseInt(string value)
		{
			int result;
			var success = int.TryParse(value, out result);
			if (success)
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
