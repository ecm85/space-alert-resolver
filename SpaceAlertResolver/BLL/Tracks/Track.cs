using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats;
using BLL.Threats.External;

namespace BLL.Tracks
{
	public abstract class Track
	{
		private IDictionary<ExternalThreat, int> ThreatPositions { get; set; }
		public IEnumerable<ExternalThreat> ThreatsOnTrack { get { return ThreatPositions.Keys.ToList(); } }
		public Zone Zone { get; private set; }
		private IDictionary<int, TrackBreakpoint> Breakpoints { get; set; }
		private IList<TrackSection> sections;

		protected Track(IEnumerable<TrackBreakpoint> breakpoints, Zone zone, IList<TrackSection> sections)
		{
			Zone = zone;
			ThreatPositions = new Dictionary<ExternalThreat, int>();
			Breakpoints = breakpoints.ToDictionary(breakpoint => breakpoint.Position);
			this.sections = sections;
		}

		public void AddThreat(ExternalThreat threat)
		{
			ThreatPositions[threat] = 0;
		}

		public ExternalThreat ClosestThreat()
		{
			if (!ThreatPositions.Any())
				return null;
			var closestThreatPosition = ThreatPositions.Max(threat => threat.Value);
			var closestThreats = ThreatPositions.Where(threat => threat.Value == closestThreatPosition);
			return closestThreats.OrderBy(threat => threat.Key.TimeAppears).First().Key;
		}

		public int DistanceToThreat(ExternalThreat threat)
		{
			var distance = ThreatPositions[threat];
			foreach (var section in sections)
			{
				if (section.Length < distance)
					distance -= section.Length;
				else
					return section.DistanceFromShip;
			}
			throw new InvalidOperationException(); 
		}

		public IEnumerable<ExternalThreat> RemoveDefeatedThreats()
		{
			var defeatedThreats = new List<ExternalThreat>();
			foreach (var threat in ThreatsOnTrack.Where(threat => threat.RemainingHealth <= 0))
			{
				defeatedThreats.Add(threat);
				ThreatPositions.Remove(threat);
			}
			return defeatedThreats;
		}

		public IEnumerable<ExternalThreat> GetThreatsWithinDistance(int distance)
		{
			return ThreatsOnTrack.Where(threat => DistanceToThreat(threat) <= distance);
		}

		public void MoveThreat(ExternalThreat externalThreat, SittingDuck sittingDuck)
		{
			for (var i = 0; i < externalThreat.Speed; i++)
			{
				ThreatPositions[externalThreat]++;
				var newLocationOnTrack = ThreatPositions[externalThreat];
				if (Breakpoints.ContainsKey(newLocationOnTrack))
				{
					var crossedBreakpoint = Breakpoints[newLocationOnTrack];
					switch (crossedBreakpoint.Type)
					{
						case TrackBreakpointType.X:
							externalThreat.PeformXAction(sittingDuck);
							break;
						case TrackBreakpointType.Y:
							externalThreat.PerformYAction(sittingDuck);
							break;
						case TrackBreakpointType.Z:
							externalThreat.PerformZAction(sittingDuck);
							//TODO: Handle survived case (score)
							break;
					}
				}
			}
			if (ThreatPositions[externalThreat] >= sections.Sum(section => section.Length) - 1)
				ThreatPositions.Remove(externalThreat);
		}
	}
}
