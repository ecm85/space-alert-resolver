using System;
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

		public static string GetDisplayName()
		{
			return "Phasing Troopers I3-106";
		}

		public static string GetId()
		{
			return "I3-106";
		}
	}
}
