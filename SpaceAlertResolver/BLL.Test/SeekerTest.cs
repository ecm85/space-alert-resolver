using System.Collections.Generic;
using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Threats.Internal.Serious.Yellow;
using Moq;
using NUnit.Framework;

namespace BLL.Test
{
	[TestFixture]
	public static class SeekerTest
	{
		[Test]
		public static void Test_MoveToMostPlayers_LowerBlueMovesUp()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerWhite, 2 }, { StationLocation.UpperBlue, 3 } },
				StationLocation.LowerBlue,
				StationLocation.UpperBlue);
		}

		[Test]
		public static void Test_MoveToMostPlayers_LowerBlueMovesRed()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerWhite, 3 }, { StationLocation.UpperBlue, 2 } },
				StationLocation.LowerBlue,
				StationLocation.LowerWhite);
		}

		[Test]
		public static void Test_MoveToMostPlayers_LowerBlueStays()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerWhite, 2 }, { StationLocation.UpperBlue, 2 } },
				StationLocation.LowerBlue,
				StationLocation.LowerBlue);
		}

		[Test]
		public static void Test_MoveToMostPlayers_LowerWhiteMovesUp()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 2 }, { StationLocation.UpperWhite, 3 } , {StationLocation.LowerRed, 2}},
				StationLocation.LowerWhite,
				StationLocation.UpperWhite);
		}

		[Test]
		public static void Test_MoveToMostPlayers_LowerWhiteMovesRed()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 2 }, { StationLocation.UpperWhite, 3 }, { StationLocation.LowerRed, 4 } },
				StationLocation.LowerWhite,
				StationLocation.LowerRed);
		}

		[Test]
		public static void Test_MoveToMostPlayers_LowerWhiteMovesBlue()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 5 }, { StationLocation.UpperWhite, 4 }, { StationLocation.LowerRed, 4 } },
				StationLocation.LowerWhite,
				StationLocation.LowerBlue);
		}

		[Test]
		public static void Test_MoveToMostPlayers_LowerWhiteStays()
		{
			Test_MoveToMostPlayers_Helper(
				new Dictionary<StationLocation, int> { { StationLocation.LowerBlue, 3 }, { StationLocation.UpperWhite, 4 }, { StationLocation.LowerRed, 4 } },
				StationLocation.LowerWhite,
				StationLocation.LowerWhite);
		}

		private static void Test_MoveToMostPlayers_Helper(Dictionary<StationLocation, int> countsByLocation, StationLocation currentStation, StationLocation expectedNewStation)
		{
			var seeker = new Seeker{CurrentStation = currentStation};
			seeker.SetInitialPlacement(4);
			var threatController = new ThreatController(null, null, new List<ExternalThreat>(), new List<InternalThreat>());
			var mockSittingDuck = new Mock<SittingDuck>(MockBehavior.Strict, threatController, null, null);
			foreach (var countByLocation in countsByLocation)
			{
				var count = countByLocation.Value;
				var location = countByLocation.Key;
				mockSittingDuck
					.Setup(f => f.GetPlayerCount(location))
					.Returns(count)
					.Verifiable();
				mockSittingDuck
					.Setup(f => f.RedDoorIsSealed)
					.Returns(false);
				mockSittingDuck
					.Setup(f => f.BlueDoorIsSealed)
					.Returns(false);
			}
			seeker.Initialize(mockSittingDuck.Object, null, null);
			seeker.MoveToMostPlayers();
			Assert.AreEqual(expectedNewStation, seeker.CurrentStation);
		}
	}
}
