using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class CResult
	{
		public bool VisualConfirmationPerformed { get; set; }
		public bool ComputerMaintainancePerformed { get; set; }
		public bool TakeOffInInterceptors { get; set; }
		public Rocket RocketFired { get; set; }
	}
}
