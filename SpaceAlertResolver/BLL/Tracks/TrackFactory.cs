using System.Collections.Generic;
using System.Linq;

namespace BLL.Tracks
{
	public static class TrackFactory
	{
		public static IEnumerable<Track> CreateAllTracks()
		{
			return EnumFactory.All<TrackConfiguration>().Select(trackConfiguration => new Track(trackConfiguration));
		}
	}
}
