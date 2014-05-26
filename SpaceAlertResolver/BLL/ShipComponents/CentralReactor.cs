using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class CentralReactor : Reactor
	{
		private int storageCapsules = 3;

		public CentralReactor() : base(5, 3)
		{
		}

		public override void PerformBAction()
		{
			if (storageCapsules <= 0)
				return;
			storageCapsules--;
			Energy = capacity;
		}
	}
}
