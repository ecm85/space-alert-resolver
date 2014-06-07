using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.External;

namespace BLL.Tracks
{
	public class ExternalTrack : Track<ExternalThreat>
	{
		public Zone Zone { get; private set; }
		public ExternalTrack(TrackConfiguration trackConfiguration, Zone zone) : base(trackConfiguration)
		{
			Zone = zone;
		}
	}
}
