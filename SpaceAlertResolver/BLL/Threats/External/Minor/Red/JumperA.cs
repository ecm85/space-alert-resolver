﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Red
{
	public class JumperA : Jumper
	{
		protected override ZoneLocation JumpDestination
		{
			get { return CurrentZone.RedwardZoneLocationWithWrapping(); }
		}
	}
}
