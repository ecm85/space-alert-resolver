using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats;

namespace BLL.Tracks
{
	public abstract class Track<T> where T : Threat
	{
		public readonly IDictionary<T, int> threatPositions;
		public readonly IDictionary<int, TrackBreakpoint> breakpoints;
		protected readonly IList<TrackSection> sections;

		protected Track(TrackConfiguration trackConfiguration)
		{
			threatPositions = new Dictionary<T, int>();
			breakpoints = trackConfiguration.TrackBreakpoints().ToDictionary(breakpoint => breakpoint.Position);
			sections = trackConfiguration.TrackSections();
		}

		public void AddThreat(T threat)
		{
			threatPositions[threat] = sections.Sum(section => section.Length);
		}

		

		public void RemoveThreats(IEnumerable<T> threats )
		{
			foreach (var threat in threats)
				threatPositions.Remove(threat);
		}

		public IList<T> ThreatsSurvived
		{
			get { return threatPositions.Keys.Where(threat => threatPositions[threat] <= 0).ToList(); }
		}

		public void MoveThreat(T threat)
		{
			for (var i = 0; i < threat.Speed; i++)
			{
				threatPositions[threat]--;
				var newLocationOnTrack = threatPositions[threat];
				if (breakpoints.ContainsKey(newLocationOnTrack))
				{
					var crossedBreakpoint = breakpoints[newLocationOnTrack];
					switch (crossedBreakpoint.Type)
					{
						case TrackBreakpointType.X:
							threat.PeformXAction();
							break;
						case TrackBreakpointType.Y:
							threat.PerformYAction();
							break;
						case TrackBreakpointType.Z:
							threat.PerformZAction();
							//TODO: Handle survived case (score)
							break;
					}
				}
			}
		}
	}
}
