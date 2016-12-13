using System;
using System.Collections.Generic;

namespace BLL.Threats
{
	public static class ThreatPoints
	{
		private static readonly Dictionary<Tuple<ThreatType, ThreatDifficulty>, int> PointsForSurviving = new Dictionary<Tuple<ThreatType, ThreatDifficulty>, int>
		{
			{Tuple.Create(ThreatType.MinorExternal, ThreatDifficulty.White), 2},
			{Tuple.Create(ThreatType.MinorExternal, ThreatDifficulty.Yellow), 3},
			{Tuple.Create(ThreatType.MinorExternal, ThreatDifficulty.Red), 4},

			{Tuple.Create(ThreatType.MinorInternal, ThreatDifficulty.White), 2},
			{Tuple.Create(ThreatType.MinorInternal, ThreatDifficulty.Yellow), 3},
			{Tuple.Create(ThreatType.MinorInternal, ThreatDifficulty.Red), 4},

			{Tuple.Create(ThreatType.SeriousExternal, ThreatDifficulty.White), 4},
			{Tuple.Create(ThreatType.SeriousExternal, ThreatDifficulty.Yellow), 6},
			{Tuple.Create(ThreatType.SeriousExternal, ThreatDifficulty.Red), 8},

			{Tuple.Create(ThreatType.SeriousInternal, ThreatDifficulty.White), 4},
			{Tuple.Create(ThreatType.SeriousInternal, ThreatDifficulty.Yellow), 6},
			{Tuple.Create(ThreatType.SeriousInternal, ThreatDifficulty.Red), 8}
		};

		public static int GetPointsForSurviving(ThreatType type, ThreatDifficulty difficulty)
		{
			return PointsForSurviving[Tuple.Create(type, difficulty)];
		}

		public static int GetPointsForDefeating(ThreatType type, ThreatDifficulty difficulty)
		{
			return GetPointsForSurviving(type, difficulty) * 2;
		}

		public static int GetPointsForDefeatingSeeker => 15;
	}
}
