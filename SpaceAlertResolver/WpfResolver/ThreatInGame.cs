using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;

namespace WpfResolver
{
	public class ThreatInGame : Threat
	{
		public int TimeAppears { get; set; }
		public ZoneLocation? Zone { get; set; }
	}
}
