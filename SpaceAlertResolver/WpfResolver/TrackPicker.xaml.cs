using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using BLL;
using BLL.Tracks;

namespace WpfResolver
{
	public partial class TrackPicker : Window
	{
		public InternalTrack InternalTrack { get; private set; }
		public IList<ExternalTrack> ExternalTracks { get; private set; } 

		public TrackPicker(SittingDuck sittingDuck)
		{
			InitializeComponent();
			//ExternalTracks = new List<ExternalTrack>();

			ExternalTracks = new[]
			{
				new ExternalTrack(TrackConfiguration.Track1, sittingDuck.BlueZone),
				new ExternalTrack(TrackConfiguration.Track2, sittingDuck.RedZone),
				new ExternalTrack(TrackConfiguration.Track3, sittingDuck.WhiteZone)
			};
			InternalTrack = new InternalTrack(TrackConfiguration.Track4);
		}
	}
}
