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
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerWhite, 2 }, { StationLocation.UpperBlue, 3 } },
				StationLocation.LowerBlue,
				StationLocation.UpperBlue);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerBlueMovesRed()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerWhite, 3 }, { StationLocation.UpperBlue, 2 } },
				StationLocation.LowerBlue,
				StationLocation.LowerWhite);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerBlueStays()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerWhite, 2 }, { StationLocation.UpperBlue, 2 } },
				StationLocation.LowerBlue,
				StationLocation.LowerBlue);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerWhiteMovesUp()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 2 }, { StationLocation.UpperWhite, 3 } , {StationLocation.LowerRed, 2}},
				StationLocation.LowerWhite,
				StationLocation.UpperWhite);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerWhiteMovesRed()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 2 }, { StationLocation.UpperWhite, 3 }, { StationLocation.LowerRed, 4 } },
				StationLocation.LowerWhite,
				StationLocation.LowerRed);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerWhiteMovesBlue()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 5 }, { StationLocation.UpperWhite, 4 }, { StationLocation.LowerRed, 4 } },
				StationLocation.LowerWhite,
				StationLocation.LowerBlue);
		}

		[TestMethod]
		public void Test_MoveToMostPlayers_LowerWhiteStays()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 3 }, { StationLocation.UpperWhite, 4 }, { StationLocation.LowerRed, 4 } },
				StationLocation.LowerWhite,
				StationLocation.LowerWhite);
		}

		private void Test_MoveToMostPlayers_Helper(Dictionary<StationLocation, int> countsByLocation, StationLocation currentStation, StationLocation expectedNewStation)
		{
			var seeker = new Seeker{CurrentStation = currentStation, TimeAppears = 4};
			var mockSittingDuck = new Mock<SittingDuck>(MockBehavior.Strict, (ThreatController)null, (Game)null);
			foreach (var countByLocation in countsByLocation)
			{
				var count = countByLocation.Value;
				var location = countByLocation.Key;
				mockSittingDuck
					.Setup(f => f.GetPlayerCount(location))
					.Returns(count)
					.Verifiable();
				mockSittingDuck
					.Setup(f => f.RedAirlockIsBreached)
					.Returns(false);
				mockSittingDuck
					.Setup(f => f.BlueAirlockIsBreached)
					.Returns(false);
			}
			seeker.Initialize(mockSittingDuck.Object, null);
			seeker.MoveToMostPlayers();
			Assert.AreEqual(expectedNewStation, seeker.CurrentStation);
		}
	}
}
