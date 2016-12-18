using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;

namespace BLL
{
	public static class GameParser
	{
		public static Game ParseArgsIntoGame(string[] args)
		{
			if (!args.Any())
				throw new InvalidOperationException();
			var validArguments = args.Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
			var chunkIdentifiers = new[] { "-external-tracks", "-internal-track", "-players", "-external-threats", "-internal-threats" };
			var chunks = ChunkArguments(validArguments, chunkIdentifiers).ToList();
			if (chunks.Count != 5 || chunkIdentifiers.Except(chunks.Select(chunk => chunk[0])).Any())
				throw new InvalidOperationException("Invalid arguments.");

			IDictionary<ZoneLocation, TrackConfiguration> externalTracksByZone = null;
			TrackConfiguration? internalTrackConfiguration = null;
			IList<ExternalThreat> externalThreats = null;
			IList<InternalThreat> internalThreats = null;
			IList<Player> players = null;
			IList<Threat> bonusThreats = new List<Threat>();

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
						players = ParsePlayers(chunk);
						break;
					case "-external-threats":
						var externalThreatResult = ParseExternalThreats(chunk);
						externalThreats = externalThreatResult.Threats;
						bonusThreats = bonusThreats.Concat(externalThreatResult.BonusThreats).ToList();
						break;
					case "-internal-threats":
						var internalThreatResult = ParseInternalThreats(chunk);
						internalThreats = internalThreatResult.Threats;
						bonusThreats = bonusThreats.Concat(internalThreatResult.BonusThreats).ToList();
						break;
				}
			}

			if (internalThreats == null)
				throw new InvalidOperationException("Could not find internal threats in game string.");
			if (externalThreats == null)
				throw new InvalidOperationException("Could not find external threats in game string.");
			if (externalTracksByZone == null)
				throw new InvalidOperationException("Could not find external tracks in game string.");
			if (players == null)
				throw new InvalidOperationException("Could not find players in game string.");

			return new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone,
				internalTrackConfiguration.GetValueOrDefault());
		}

		private static IList<Player> ParsePlayers(IList<string> chunk)
		{
			if (chunk.Count == 1)
				throw new InvalidOperationException("Need at least one player");
			var players = new List<Player>();
			var playerTokens = new Queue<string>(chunk.Skip(1));
			if (playerTokens.Count % 2 != 0)
				throw new InvalidOperationException("Invalid players.");
			while (playerTokens.Any())
			{
				var idToken = ParseToken(playerTokens.Dequeue());
				if (idToken == null || idToken.Item1 != "player-index")
					throw new InvalidOperationException("Error on player #" + players.Count + 1);
				var index = TryParseInt(idToken.Item2);
				if (index == null)
					throw new InvalidOperationException("Error on player #" + players.Count + 1);
				var actionsToken = ParseToken(playerTokens.Dequeue());
				if (actionsToken == null || actionsToken.Item1 != "actions")
					throw new InvalidOperationException("Error on player #" + players.Count + 1);
				var actions = ParsePlayerActions(actionsToken.Item2);
				if (actions == null)
					throw new InvalidOperationException("Error on player #" + players.Count + 1);
				var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, actions)) { Index = index.Value };
				players.Add(player);
				//TODO: Add specializations and teleport player/destination
			}
			return players;
		}

		private static readonly IDictionary<char, PlayerActionType?> actionTypesByCode = new Dictionary<char, PlayerActionType?>
		{
			{'a', PlayerActionType.Alpha},
			{'b', PlayerActionType.Bravo},
			{'c', PlayerActionType.Charlie},
			{'x', PlayerActionType.BattleBots},
			{'<', PlayerActionType.MoveRed},
			{'>', PlayerActionType.MoveBlue},
			{'^', PlayerActionType.ChangeDeck},
			{'A', PlayerActionType.HeroicA},
			{'B', PlayerActionType.HeroicB},
			{'X', PlayerActionType.HeroicBattleBots},
			{'6', PlayerActionType.TeleportBlueUpper},
			{'3', PlayerActionType.TeleportBlueLower},
			{'5', PlayerActionType.TeleportWhiteUpper},
			{'2', PlayerActionType.TeleportWhiteLower},
			{'4', PlayerActionType.TeleportRedUpper},
			{'1', PlayerActionType.TeleportRedLower},
			{'s', PlayerActionType.BasicSpecialization},
			{'S', PlayerActionType.AdvancedSpecialization},
			{'-', null}
		};

		private static IList<PlayerActionType?> ParsePlayerActions(string actionString)
		{
			if (actionString.Any(actionCode => !actionTypesByCode.ContainsKey(actionCode)))
				throw new InvalidOperationException("Invalid player actions.");
			//TODO: Parse advanced spec ops/Medic both
			return actionString.Select(action => actionTypesByCode[action]).ToList();
		}

		private static ParseThreatsResult<ExternalThreat> ParseExternalThreats(IEnumerable<string> chunk)
		{
			var externalThreats = new List<ExternalThreat>();
			var bonusThreats = new List<Threat>();
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
						nextThreatInfo = new ExternalThreatInfo { Threat = ThreatFactory.CreateThreat<ExternalThreat>(nextToken.Item2) };
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
						bonusThreats.Add(SetExtraInternalThreat(nextToken.Item2, nextThreatInfo, externalThreats.Count + 1));
						break;
					case "extra-external-threat-id":
						bonusThreats.Add(SetExtraExternalThreat(nextToken.Item2, nextThreatInfo, externalThreats.Count + 1));
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
			return new ParseThreatsResult<ExternalThreat>
			{
				Threats = externalThreats,
				BonusThreats = bonusThreats
			};
		}

		private static ParseThreatsResult<InternalThreat> ParseInternalThreats(IEnumerable<string> chunk)
		{
			var internalThreats = new List<InternalThreat>();
			var bonusThreats = new List<Threat>();
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
						nextThreatInfo = new InternalThreatInfo { Threat = ThreatFactory.CreateThreat<InternalThreat>(nextToken.Item2) };
						if (nextThreatInfo.Threat == null)
							throw new InvalidOperationException("Error on internal threat #" + (internalThreats.Count + 1));
						break;
					case "time":
						SetTimeAppears(nextThreatInfo, nextToken.Item2, internalThreats.Count + 1);
						break;
					case "extra-internal-threat-id":
						bonusThreats.Add(SetExtraInternalThreat(nextToken.Item2, nextThreatInfo, internalThreats.Count + 1));
						break;
					case "extra-external-threat-id":
						bonusThreats.Add(SetExtraExternalThreat(nextToken.Item2, nextThreatInfo, internalThreats.Count + 1));
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
			return new ParseThreatsResult<InternalThreat>
			{
				Threats = internalThreats,
				BonusThreats = bonusThreats
			};
		}

		private static ExternalThreat SetExtraExternalThreat<T>(
			string extraExternalThreatId,
			StandardThreatInfo<T> nextThreatInfo,
			int currentThreatIndex) where T : Threat
		{
			var bonusExternalThreat = ThreatFactory.CreateThreat<ExternalThreat>(extraExternalThreatId);
			if (nextThreatInfo == null || bonusExternalThreat == null)
				throw new InvalidOperationException("Error on external threat #" + currentThreatIndex);
			AddBonusThreatInfo(nextThreatInfo, bonusExternalThreat);
			return bonusExternalThreat;
		}

		private static InternalThreat SetExtraInternalThreat<T>(
			string extraInternalThreatId,
			StandardThreatInfo<T> nextThreatInfo,
			int currentThreatIndex) where T : Threat
		{
			var bonusInternalThreat = ThreatFactory.CreateThreat<InternalThreat>(extraInternalThreatId);
			if (nextThreatInfo == null || bonusInternalThreat == null)
				throw new InvalidOperationException("Error on external threat #" + currentThreatIndex);
			AddBonusThreatInfo(nextThreatInfo, bonusInternalThreat);
			return bonusInternalThreat;
		}

		private static void SetLocation(ExternalThreatInfo nextThreatInfo, string location, int currentThreatIndex)
		{
			if (nextThreatInfo == null || nextThreatInfo.ZoneLocation.HasValue)
				throw new InvalidOperationException("Error on external threat #" + currentThreatIndex);
			nextThreatInfo.ZoneLocation = TryParseEnum<ZoneLocation>(location);
			if (!nextThreatInfo.ZoneLocation.HasValue)
				throw new InvalidOperationException("Error on external threat #" + currentThreatIndex);
		}

		private static void SetTimeAppears<T>(StandardThreatInfo<T> nextThreatInfo, string timeAppears, int currentThreatIndex) where T : Threat
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

		private static void InitializeBonusThreats<T>(ThreatInfo<T> threatInfo) where T : Threat
		{
			if (threatInfo == null)
				throw new InvalidOperationException("Missing extra threat.");
			if (threatInfo.Threat.NeedsBonusInternalThreat)
			{
				InitializeBonusThreats(threatInfo.BonusInternalThreatInfo);
				((IThreatWithBonusThreat<InternalThreat>)threatInfo.Threat).SetBonusThreat(threatInfo.BonusInternalThreatInfo.Threat);
			}
			if (threatInfo.Threat.NeedsBonusExternalThreat)
			{
				InitializeBonusThreats(threatInfo.BonusExternalThreatInfo);
				((IThreatWithBonusThreat<ExternalThreat>)threatInfo.Threat).SetBonusThreat(threatInfo.BonusExternalThreatInfo.Threat);
			}
		}

		private static void AddBonusThreatInfo<T>(ThreatInfo<T> threatInfo, ExternalThreat newThreat) where T : Threat
		{
			if (threatInfo.BonusExternalThreatInfo != null)
				AddBonusThreatInfo(threatInfo.BonusExternalThreatInfo, newThreat);
			else if (threatInfo.BonusInternalThreatInfo != null)
				AddBonusThreatInfo(threatInfo.BonusInternalThreatInfo, newThreat);
			else
				threatInfo.BonusExternalThreatInfo = new ThreatInfo<ExternalThreat> { Threat = newThreat };
		}

		private static void AddBonusThreatInfo<T>(ThreatInfo<T> threatInfo, InternalThreat newThreat) where T : Threat
		{
			if (threatInfo.BonusExternalThreatInfo != null)
				AddBonusThreatInfo(threatInfo.BonusExternalThreatInfo, newThreat);
			else if (threatInfo.BonusInternalThreatInfo != null)
				AddBonusThreatInfo(threatInfo.BonusInternalThreatInfo, newThreat);
			else
				threatInfo.BonusInternalThreatInfo = new ThreatInfo<InternalThreat> { Threat = newThreat };
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
			var pieces = token.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
			return pieces.Length != 2 ? null : Tuple.Create(pieces[0], pieces[1]);
		}

		private class ThreatInfo<T> where T : Threat
		{
			public T Threat { get; set; }
			public ThreatInfo<InternalThreat> BonusInternalThreatInfo { get; set; }
			public ThreatInfo<ExternalThreat> BonusExternalThreatInfo { get; set; }

			public virtual bool IsValid()
			{
				if (Threat == null)
					return false;
				if (Threat.NeedsBonusInternalThreat)
					if (BonusInternalThreatInfo == null || !BonusInternalThreatInfo.IsValid())
						return false;
				if (Threat.NeedsBonusExternalThreat)
					if (BonusExternalThreatInfo == null || !BonusExternalThreatInfo.IsValid())
						return false;
				return true;
			}
		}

		private class StandardThreatInfo<T> : ThreatInfo<T> where T : Threat
		{
			public int? TimeAppears { get; set; }

			public override bool IsValid()
			{
				return TimeAppears != null && base.IsValid();
			}
		}

		private class ExternalThreatInfo : StandardThreatInfo<ExternalThreat>
		{
			public ZoneLocation? ZoneLocation { get; set; }

			public override bool IsValid()
			{
				return ZoneLocation != null && base.IsValid();
			}
		}

		private class InternalThreatInfo : StandardThreatInfo<InternalThreat>
		{
		}

		private class ParseThreatsResult<T> where T : Threat
		{
			public IList<T> Threats { get; set; }
			public IList<Threat> BonusThreats { get; set; }
		}
	}
}
