﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class SaboteurA : Saboteur
	{
		protected override void PerformXAction(int currentTurn)
		{
			MoveRed();
		}
	}
}
