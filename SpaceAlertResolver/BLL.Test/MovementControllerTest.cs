using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLL.Test
{
	[TestClass]
	public class MovementControllerTest
	{
		[TestMethod]
		public void Test_Path_Corners()
		{
			Test_Path_Helper(StationLocation.LowerBlue, StationLocation.UpperRed, new List<StationLocation>
			{
				StationLocation.LowerWhite,
				StationLocation.LowerRed,
				StationLocation.UpperRed
			});
			Test_Path_Helper(StationLocation.UpperBlue, StationLocation.LowerRed, new List<StationLocation>
			{
				StationLocation.UpperWhite,
				StationLocation.UpperRed,
				StationLocation.LowerRed
			});
		}

		[TestMethod]
		public void Test_Path_ChangeDecks()
		{
			Test_Path_Helper(StationLocation.UpperBlue, StationLocation.LowerBlue, new List<StationLocation> { StationLocation.LowerBlue });
			Test_Path_Helper(StationLocation.LowerWhite, StationLocation.UpperWhite, new List<StationLocation> { StationLocation.UpperWhite });
		}

		[TestMethod]
		public void Test_Path_Move()
		{
			Test_Path_Helper(StationLocation.UpperBlue, StationLocation.UpperWhite, new List<StationLocation>
			{
				StationLocation.UpperWhite
			});
			Test_Path_Helper(StationLocation.UpperWhite, StationLocation.UpperBlue, new List<StationLocation>
			{
				StationLocation.UpperBlue
			});
		}

		[TestMethod]
		public void Test_Path_MoveAndChangeDecks()
		{
			Test_Path_Helper(StationLocation.LowerWhite, StationLocation.UpperBlue, new List<StationLocation>
			{
				StationLocation.LowerBlue,
				StationLocation.UpperBlue
			});

			Test_Path_Helper(StationLocation.UpperWhite, StationLocation.LowerRed, new List<StationLocation>
			{
				StationLocation.UpperRed,
				StationLocation.LowerRed
			});
		}

		private void Test_Path_Helper(StationLocation toStation, StationLocation fromStation, List<StationLocation> expectedPath)
		{
			var path = MovementController.Path(toStation, fromStation).ToList();
			CollectionAssert.AreEqual(expectedPath, path);
		}
	}
}
