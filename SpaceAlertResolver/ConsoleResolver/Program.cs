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
		private class ThreatInfo<T> where T: Threat
		{
			public T Threat { get; set; }
			public ThreatInfo<InternalThreat> BonusInternalThreatInfo { get; set; }
			public ThreatInfo<ExternalThreat> BonusExternalThreatInfo { get; set; }

			public virtual bool IsValid()
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

		private class StandardThreatInfo<T>: ThreatInfo<T> where T : Threat
		{
			public int? TimeAppears { get; set; }

			public override bool IsValid()
			{
				if (TimeAppears == null)
					return false;
				return base.IsValid();
			}
		}

		private class ExternalThreatInfo : StandardThreatInfo<ExternalThreat>
		{
			public ZoneLocation? ZoneLocation { get; set; }

			public override bool IsValid()
			{

				if ( ZoneLocation == null)
					return false;
				return base.IsValid();
			}
		}

		private class InternalThreatInfo : StandardThreatInfo<InternalThreat>
		{
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
			IList<InternalThreat> internalThreats = null;

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
							internalThreats = ParseInternalThreats(chunk);
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
			if (internalThreats != null)
				foreach (var internalThreat in internalThreats)
				{
					Console.WriteLine("{0}, {1}", internalThreat.GetType(), internalThreat.TimeAppears);
				}

			return 0;
		}

		private static IList<ExternalThreat> ParseExternalThreats(IEnumerable<string> chunk)
		{
			var externalThreats = new List<ExternalThreat>();
			var threatTokens = new Queue<string>(chunk.Skip(1));
			ExternalThreatInfo nextThreatInfo = null;
			while (threatTokens.Any())
			{
				var nextToken = ParseToken(threatTokens.Dequeue());
				if (nextToken == null)
					throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
				switch (nextToken.Item1)
				{
					case "id":
						if (nextThreatInfo != null)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						nextThreatInfo = new ExternalThreatInfo {Threat = ThreatFactory.CreateThreat<ExternalThreat>(nextToken.Item2)};
						if (nextThreatInfo.Threat == null)
							throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
						break;
					case "time":
						SetTimeAppears(nextThreatInfo, nextToken.Item2, externalThreats.Count + 1);
						break;
					case "location":
						SetLocation(nextThreatInfo, nextToken.Item2, externalThreats.Count + 1);
						break;
					case "extra-internal-threat-id":
						SetExtraInternalThreat(nextToken.Item2, nextThreatInfo, externalThreats.Count + 1);
						break;
					case "extra-external-threat-id":
						SetExtraExternalThreat(nextToken.Item2, nextThreatInfo, externalThreats.Count + 1);
						break;
					default:
						throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
				}
				if (nextThreatInfo != null && nextThreatInfo.IsValid())
				{
					var threat = nextThreatInfo.Threat;
					threat.CurrentZone = nextThreatInfo.ZoneLocation.GetValueOrDefault();
					threat.TimeAppears = nextThreatInfo.TimeAppears.GetValueOrDefault();
					InitializeBonusThreats(nextThreatInfo);
					externalThreats.Add(threat);
					nextThreatInfo = null;
				}
			}
			if (nextThreatInfo != null)
				throw new InvalidOperationException("Error on external threat #" + (externalThreats.Count + 1));
			return externalThreats;
		}

		private static IList<InternalThreat> ParseInternalThreats(IEnumerable<string> chunk)
		{
			var internalThreats = new List<InternalThreat>();
			var threatTokens = new Queue<string>(chunk.Skip(1));
			InternalThreatInfo nextThreatInfo = null;
			while (threatTokens.Any())
			{
				var nextToken = ParseToken(threatTokens.Dequeue());
				if (nextToken == null)
					throw new InvalidOperationException("Error on internal threat #" + (internalThreats.Count + 1));
				switch (nextToken.Item1)
				{
					case "id":
						if (nextThreatInfo != null)
							throw new InvalidOperationException("Error on internal threat #" + (internalThreats.Count + 1));
						nextThreatInfo = new InternalThreatInfo{Threat = ThreatFactory.CreateThreat<InternalThreat>(nextToken.Item2)};
						if (nextThreatInfo.Threat == null)
							throw new InvalidOperationException("Error on internal threat #" + (internalThreats.Count + 1));
						break;
					case "time":
						SetTimeAppears(nextThreatInfo, nextToken.Item2, internalThreats.Count + 1);
						break;
					case "extra-internal-threat-id":
						SetExtraInternalThreat(nextToken.Item2, nextThreatInfo, internalThreats.Count + 1);
						break;
					case "extra-external-threat-id":
						SetExtraExternalThreat(nextToken.Item2, nextThreatInfo, internalThreats.Count + 1);
						break;
					default:
						throw new InvalidOperationException("Error on internal threat #" + (internalThreats.Count + 1));
				}
				if (nextThreatInfo != null && nextThreatInfo.IsValid())
				{
					var threat = nextThreatInfo.Threat;
					threat.TimeAppears = nextThreatInfo.TimeAppears.GetValueOrDefault();
					InitializeBonusThreats(nextThreatInfo);
					internalThreats.Add(threat);
					nextThreatInfo = null;
				}
			}
			if (nextThreatInfo != null)
				throw new InvalidOperationException("Error on external threat #" + (internalThreats.Count + 1));
			return internalThreats;
		}

		private static void SetExtraExternalThreat<T>(
			string extraExternalThreatId,
			StandardThreatInfo<T> nextThreatInfo,
			int currentThreatIndex) where T: Threat
		{
			var bonusExternalThreat = ThreatFactory.CreateThreat<ExternalThreat>(extraExternalThreatId);
			if (nextThreatInfo == null || bonusExternalThreat == null)
				throw new InvalidOperationException("Error on external threat #" + currentThreatIndex);
			AddBonusThreatInfo(nextThreatInfo, bonusExternalThreat);
		}

		private static void SetExtraInternalThreat<T>(
			string extraInternalThreatId,
			StandardThreatInfo<T> nextThreatInfo,
			int currentThreatIndex) where T: Threat
		{
			var bonusInternalThreat = ThreatFactory.CreateThreat<InternalThreat>(extraInternalThreatId);
			if (nextThreatInfo == null || bonusInternalThreat == null)
				throw new InvalidOperationException("Error on external threat #" + currentThreatIndex);
			AddBonusThreatInfo(nextThreatInfo, bonusInternalThreat);
		}

		private static void SetLocation(ExternalThreatInfo nextThreatInfo, string location, int currentThreatIndex)
		{
			if (nextThreatInfo == null || nextThreatInfo.ZoneLocation.HasValue)
				throw new InvalidOperationException("Error on external threat #" + currentThreatIndex);
			nextThreatInfo.ZoneLocation = TryParseEnum<ZoneLocation>(location);
			if (!nextThreatInfo.ZoneLocation.HasValue)
				throw new InvalidOperationException("Error on external threat #" + currentThreatIndex);
		}

		private static void SetTimeAppears<T>(StandardThreatInfo<T> nextThreatInfo, string timeAppears, int currentThreatIndex) where T: Threat
		{
			if (nextThreatInfo == null || nextThreatInfo.TimeAppears.HasValue)
				throw new InvalidOperationException("Error on external threat #" + currentThreatIndex);
			nextThreatInfo.TimeAppears = TryParseInt(timeAppears);
			if (!nextThreatInfo.TimeAppears.HasValue)
				throw new InvalidOperationException("Error on external threat #" + currentThreatIndex);
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

		private static void InitializeBonusThreats<T>(ThreatInfo<T> threatInfo) where T: Threat
		{
			if(threatInfo == null)
				throw new InvalidOperationException("Missing extra threat.");
			if (threatInfo.Threat.NeedsBonusInternalThreat)
			{
				InitializeBonusThreats(threatInfo.BonusInternalThreatInfo);
				((IThreatWithBonusInternalThreat)threatInfo.Threat).SetBonusThreat(threatInfo.BonusInternalThreatInfo.Threat);
			}
			if (threatInfo.Threat.NeedsBonusExternalThreat)
			{
				InitializeBonusThreats(threatInfo.BonusExternalThreatInfo);
				((IThreatWithBonusExternalThreat)threatInfo.Threat).SetBonusThreat(threatInfo.BonusExternalThreatInfo.Threat);
			}
		}

		private static void AddBonusThreatInfo<T>(ThreatInfo<T> threatInfo, ExternalThreat newThreat) where T: Threat
		{
			if(threatInfo.BonusExternalThreatInfo != null)
				AddBonusThreatInfo(threatInfo.BonusExternalThreatInfo, newThreat);
			else if (threatInfo.BonusInternalThreatInfo != null)
				AddBonusThreatInfo(threatInfo.BonusInternalThreatInfo, newThreat);
			else
				threatInfo.BonusExternalThreatInfo = new ThreatInfo<ExternalThreat> {Threat = newThreat};
		}

		private static void AddBonusThreatInfo<T>(ThreatInfo<T> threatInfo, InternalThreat newThreat) where T: Threat
		{
			if(threatInfo.BonusExternalThreatInfo != null)
				AddBonusThreatInfo(threatInfo.BonusExternalThreatInfo, newThreat);
			else if (threatInfo.BonusInternalThreatInfo != null)
				AddBonusThreatInfo(threatInfo.BonusInternalThreatInfo, newThreat);
			else
				threatInfo.BonusInternalThreatInfo = new ThreatInfo<InternalThreat>{Threat = newThreat};
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
				"-external-threats [id:<string> time:<int> location:<red|white|blue> [extra-external-threat-id:<string>]? [extra-internal-threat-id:<string>]? ]+");
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
