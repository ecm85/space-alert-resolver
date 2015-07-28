﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class Frigate : SeriousWhiteExternalThreat
	{
		public Frigate()
			: base(2, 7, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(4);
		}
	}
}
