﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
	public class PhasingTroopersA : PhasingTroopers
	{
		public PhasingTroopersA() : base(StationLocation.LowerBlue)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveRed();
		}
	}
}
