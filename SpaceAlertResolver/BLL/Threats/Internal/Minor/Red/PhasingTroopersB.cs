using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
	public class PhasingTroopersB : PhasingTroopers
	{
		public PhasingTroopersB() : base(StationLocation.LowerRed)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveBlue();
		}

		public static string GetDisplayName()
		{
			return "Phasing Troopers I3-105";
		}
	}
}
