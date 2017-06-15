using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Tracks
{
    public class Track
    {
        public IDictionary<int, TrackBreakpointType> Breakpoints { get; }
        public IList<TrackSection> Sections { get; }
        public TrackConfiguration TrackConfiguration { get; }

        internal Track(TrackConfiguration trackConfiguration)
        {
            Breakpoints = trackConfiguration.TrackBreakpoints();
            Sections = trackConfiguration.TrackSections();
            TrackConfiguration = trackConfiguration;
        }

        public int StartingPosition => Sections.Sum(section => section.Length);

        public int DistanceToThreat(int position)
        {
            var distance = position;
            foreach (var section in Sections.OrderBy(section => section.DistanceFromShip))
            {
                if (section.Length >= distance)
                    return section.DistanceFromShip;
                distance -= section.Length;
            }
            throw new InvalidOperationException();
        }

        public IEnumerable<TrackBreakpointType> GetCrossedBreakpoints(int oldPosition, int newPosition)
        {
            var crossedBreakpoints = new List<TrackBreakpointType>();
            for(var i = oldPosition - 1; i >= newPosition; i--)
                if(Breakpoints.ContainsKey(i))
                    crossedBreakpoints.Add(Breakpoints[i]);
            return crossedBreakpoints;
        }
    }
}
