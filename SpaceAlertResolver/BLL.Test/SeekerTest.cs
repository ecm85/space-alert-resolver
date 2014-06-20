using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.Internal.Serious.Yellow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BLL.Test
{
	[TestClass]
	public class SeekerTest
	{
		[TestMethod]
		public void Test_MoveToMostPlayers_LowerBlueMovesUp()
		{
			const StationLocation currentLocation = StationLocation.LowerBlue;
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerWhite, 2 }, { StationLocation.UpperBlue, 3 } },
				currentLocation,
				StationLocation.UpperBlue);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerBlueMovesRed()
		{
			const StationLocation currentLocation = StationLocation.LowerBlue;
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerWhite, 3 }, { StationLocation.UpperBlue, 2 } },
				currentLocation,
				StationLocation.LowerWhite);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerBlueStays()
		{
			const StationLocation currentLocation = StationLocation.LowerBlue;
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerWhite, 2 }, { StationLocation.UpperBlue, 2 } },
				currentLocation,
				StationLocation.LowerBlue);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerWhiteMovesUp()
		{
			const StationLocation currentLocation = StationLocation.LowerWhite;
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 2 }, { StationLocation.UpperWhite, 3 } , {StationLocation.LowerRed, 2}},
				currentLocation,
				StationLocation.UpperWhite);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerWhiteMovesRed()
		{
			const StationLocation currentLocation = StationLocation.LowerWhite;
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 2 }, { StationLocation.UpperWhite, 3 }, { StationLocation.LowerRed, 4 } },
				currentLocation,
				StationLocation.LowerRed);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerWhiteMovesBlue()
		{
			const StationLocation currentLocation = StationLocation.LowerWhite;
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 5 }, { StationLocation.UpperWhite, 4 }, { StationLocation.LowerRed, 4 } },
				currentLocation,
				StationLocation.LowerBlue);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerWhiteStays()
		{
			const StationLocation currentLocation = StationLocation.LowerWhite;
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 3 }, { StationLocation.UpperWhite, 4 }, { StationLocation.LowerRed, 4 } },
				currentLocation,
				StationLocation.LowerWhite);
		}

		private void Test_MoveToMostPlayers_Helper(Dictionary<StationLocation, int> countsByLocation, StationLocation currentStation, StationLocation expectedNewStation)
		{
			var seeker = new Seeker{CurrentStation = currentStation};
			var mockSittingDuck = new Mock<SittingDuck>(MockBehavior.Strict, (ThreatController)null);
			foreach (var countByLocation in countsByLocation)
			{
				var count = countByLocation.Value;
				var location = countByLocation.Key;
				mockSittingDuck
					.Setup(f => f.GetPlayerCount(location))
					.Returns(count)
					.Verifiable();
			}
			seeker.Initialize(mockSittingDuck.Object, null, 4);
			seeker.MoveToMostPlayers();
			Assert.AreEqual(expectedNewStation, seeker.CurrentStation);
		}
	}
}
