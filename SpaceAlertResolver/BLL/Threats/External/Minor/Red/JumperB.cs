using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Red
{
	public class JumperB : Jumper
	{
		public static string GetDisplayName()
		{
			return "Jumper E3-106";
		}

		protected override ZoneLocation JumpDestination
		{
			get { return CurrentZone.BluewardZoneLocationWithWrapping(); }
		}

		public static string GetId()
		{
			return "E3-106";
		}
	}
}
