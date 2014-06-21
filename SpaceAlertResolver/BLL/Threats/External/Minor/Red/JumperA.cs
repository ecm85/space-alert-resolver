﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Red
{
	public class JumperA : Jumper
	{
		public static string GetDisplayName()
		{
			return "Jumper E3-105";
		}

		protected override ZoneLocation JumpDestination
		{
			get { return CurrentZone.RedwardZoneLocationWithWrapping(); }
		}
	}
}