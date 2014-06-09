using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats;

namespace BLL.Tracks
{
	public abstract class Track<T> where T : Threat
	{
		private readonly IDictionary<int, TrackBreakpointType> breakpoints;
		private readonly IList<TrackSection> sections;

		protected Track(TrackConfiguration trackConfiguration)
		{
			breakpoints = trackConfiguration.TrackBreakpoints();
			sections = trackConfiguration.TrackSections();
		}

		public int GetStartingPosition()
		{
			return sections.Sum(section => section.Length);
		}

		public void MoveThreat(T threat, int amount, int currentTurn)
		{
			if (!threat.Position.HasValue)
				throw new InvalidOperationException("Tried to move threat not on the track.");
			for (var i = 0; i < amount && threat.IsOnTrack(); i++)
			{
				threat.Position--;
				if (breakpoints.ContainsKey(threat.Position.GetValueOrDefault()))
				{
					var crossedBreakpoint = breakpoints[threat.Position.GetValueOrDefault()];
					switch (crossedBreakpoint)
					{
						case TrackBreakpointType.X:
							threat.PerformXAction(currentTurn);
							break;
						case TrackBreakpointType.Y:
							threat.PerformYAction(currentTurn);
							break;
						case TrackBreakpointType.Z:
							threat.PerformZAction(currentTurn);
							threat.OnReachingEndOfTrack();
							break;
					}
				}
			}
		}

		public int DistanceToThreat(T threat)
		{
			var distance = threat.Position;
			foreach (var section in sections.OrderBy(section => section.DistanceFromShip))
			{
				if (section.Length >= distance)
					return section.DistanceFromShip;
				distance -= section.Length;
			}
			throw new InvalidOperationException();
		}
	}
}
