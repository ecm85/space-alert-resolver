using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using BLL;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;

namespace WpfResolver
{
	public partial class ThreatPicker : Window
	{
		public IList<InternalThreat> InternalThreats { get; private set; }
		public IList<ExternalThreat> ExternalThreats { get; private set; }

		public ThreatPicker(SittingDuck sittingDuck)
		{
			InitializeComponent();
			//InternalThreats = new List<InternalThreat>();
			//ExternalThreats = new List<ExternalThreat>();

			ExternalThreats = new ExternalThreat[]
			{
				new Destroyer(3, sittingDuck.BlueZone, sittingDuck),
				new Fighter(4, sittingDuck.RedZone, sittingDuck),
				new Fighter(5, sittingDuck.WhiteZone, sittingDuck)
			};
			InternalThreats = new InternalThreat[]
			{
				new SkirmishersA(3, sittingDuck),
				new Fissure(2, sittingDuck)
				//new Alien(1, sittingDuck)
			};
		}
	}
}
