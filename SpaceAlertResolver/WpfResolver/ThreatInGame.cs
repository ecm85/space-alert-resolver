using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace WpfResolver
{
	public class ThreatInGame : Threat
	{
		public int TimeAppears { get; set; }
		public ZoneLocation? Zone { get; set; }
	}
}
