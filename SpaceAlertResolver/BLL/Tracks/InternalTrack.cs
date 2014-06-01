using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.Internal;

namespace BLL.Tracks
{
	public class InternalTrack : Track<InternalThreat>
	{
		public InternalTrack(TrackConfiguration trackConfiguration) : base(trackConfiguration)
		{
		}
	}
}
