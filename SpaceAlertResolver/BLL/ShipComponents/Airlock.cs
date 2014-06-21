using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class Airlock
	{
		public bool CanUse { get { return !Breached; }}
		public bool Breached { get; set; }
	}
}
