using System.Collections.Generic;
using BLL.ShipComponents;
using BLL.Threats.Internal.Minor.Red;
using Moq;
using NUnit.Framework;

namespace BLL.Test
{
	[TestFixture]
	public static class DrillerTest
	{
		[Test]
		public static void Test_MoveTowardsMostDamagedZone_InBlue_DamageAdjacent_MovesRed()
		{
			Test_MoveTowardsMostDamagedZone_Helper(
				new Dictionary<ZoneLocation, int>{{ZoneLocation.Blue, 1}, {ZoneLocation.White, 2}, {ZoneLocation.Red, 1}},
				StationLocation.LowerBlue,
				StationLocation.LowerWhite);
		}

		[Test]
		public static void Test_MoveTowardsMostDamagedZone_InRed_DamageNotAdjacent_MovesBlue()
		{
			Test_MoveTowardsMostDamagedZone_Helper(
				new Dictionary<ZoneLocation, int> { { ZoneLocation.Blue, 2 }, { ZoneLocation.White, 1 }, { ZoneLocation.Red, 1 } },
				StationLocation.LowerRed,
				StationLocation.LowerWhite);
		}

		[Test]
		public static void Test_MoveTowardsMostDamagedZone_InBlue_RedAndWhiteTied_MovesRed()
		{
			Test_MoveTowardsMostDamagedZone_Helper(
				new Dictionary<ZoneLocation, int> { { ZoneLocation.Blue, 1 }, { ZoneLocation.White, 2 }, { ZoneLocation.Red, 2 } },
				StationLocation.UpperBlue,
				StationLocation.UpperWhite);
		}

		[Test]
		public static void Test_MoveTowardsMostDamagedZone_InBlue_BlueAndWhiteTied_MovesRed()
		{
			Test_MoveTowardsMostDamagedZone_Helper(
				new Dictionary<ZoneLocation, int> { { ZoneLocation.Blue, 2 }, { ZoneLocation.White, 2 }, { ZoneLocation.Red, 1 } },
				StationLocation.UpperBlue,
				StationLocation.UpperWhite);
		}

		[Test]
		public static void Test_MoveTowardsMostDamagedZone_InBlue_BlueAndRedTied_MovesRed()
		{
			Test_MoveTowardsMostDamagedZone_Helper(
				new Dictionary<ZoneLocation, int> { { ZoneLocation.Blue, 1 }, { ZoneLocation.White, 0 }, { ZoneLocation.Red, 1 } },
				StationLocation.UpperBlue,
				StationLocation.UpperWhite);
		}

		[Test]
		public static void Test_MoveTowardsMostDamagedZone_InWhite_BlueAndWhiteTied_DoesNotMove()
		{
			Test_MoveTowardsMostDamagedZone_Helper(
				new Dictionary<ZoneLocation, int> { { ZoneLocation.Blue, 1 }, { ZoneLocation.White, 1 }, { ZoneLocation.Red, 0 } },
				StationLocation.UpperWhite,
				StationLocation.UpperWhite);
		}

		[Test]
		public static void Test_MoveTowardsMostDamagedZone_InWhite_BlueAndRedTied_DoesNotMove()
		{
			Test_MoveTowardsMostDamagedZone_Helper(
				new Dictionary<ZoneLocation, int> { { ZoneLocation.Blue, 1 }, { ZoneLocation.White, 0 }, { ZoneLocation.Red, 1 } },
				StationLocation.UpperWhite,
				StationLocation.UpperWhite);
		}

		[Test]
		public static void Test_MoveTowardsMostDamagedZone_InBlue_BlueHighest_DoesNotMove()
		{
			Test_MoveTowardsMostDamagedZone_Helper(
				new Dictionary<ZoneLocation, int> { { ZoneLocation.Blue, 3 }, { ZoneLocation.White, 1 }, { ZoneLocation.Red, 2 } },
				StationLocation.UpperBlue,
				StationLocation.UpperBlue);
		}

		[Test]
		public static void Test_MoveTowardsMostDamagedZone_InWhite_WhiteHighest_DoesNotMove()
		{
			Test_MoveTowardsMostDamagedZone_Helper(
				new Dictionary<ZoneLocation, int> { { ZoneLocation.Blue, 1 }, { ZoneLocation.White, 3 }, { ZoneLocation.Red, 2 } },
				StationLocation.UpperWhite,
				StationLocation.UpperWhite);
		}

		private static void Test_MoveTowardsMostDamagedZone_Helper(Dictionary<ZoneLocation, int> countsByLocation, StationLocation currentStation, StationLocation expectedNewStation)
		{
			var driller = new Driller {CurrentStation = currentStation, TimeAppears = 3};
			var mockSittingDuck = new Mock<SittingDuck>(MockBehavior.Strict, null, null);
			foreach (var countByLocation in countsByLocation)
			{
				var count = countByLocation.Value;
				var location = countByLocation.Key;
				mockSittingDuck
					.Setup(f => f.GetDamageToZone(location))
					.Returns(count)
					.Verifiable();
				mockSittingDuck
					.Setup(f => f.RedDoorIsSealed)
					.Returns(false);
				mockSittingDuck
					.Setup(f => f.BlueDoorIsSealed)
					.Returns(false);
			}
			driller.Initialize(mockSittingDuck.Object, null);
			driller.MoveTowardsMostDamagedZone();
			Assert.AreEqual(expectedNewStation, driller.CurrentStation);
		}
	}
}
