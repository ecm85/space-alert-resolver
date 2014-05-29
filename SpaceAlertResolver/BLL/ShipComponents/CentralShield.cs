using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class CentralShield : Shield
	{
		public CentralShield(Reactor source) : base(source, 3, 1)
		{
		}
	}
}
