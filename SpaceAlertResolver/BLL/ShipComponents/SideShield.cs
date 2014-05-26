using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class SideShield : Shield
	{
		public SideShield(Reactor source) : base(source, 2, 1)
		{
		}
	}
}
