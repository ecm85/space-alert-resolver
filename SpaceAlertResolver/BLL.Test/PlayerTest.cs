using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace BLL.Test
{
	[TestFixture]
	public static class PlayerTest
	{
		//TODO: Add tests for shifting at end
		private class ActionComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				var first = x as PlayerAction;
				var second = y as PlayerAction;
				if (first == null || second == null)
					return -1;
				return first.FirstActionSegment.SegmentType == second.FirstActionSegment.SegmentType &&
					first.SecondActionSegment.SegmentType == second.SecondActionSegment.SegmentType &&
					first.BonusActionSegment.SegmentType == second.BonusActionSegment.SegmentType ?
					0 :
					-1;
			}
		}

		[Test]
		public static void Test_ShiftAfterPlayerActions_NoBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAfterPlayerActions(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.BattleBots,
				PlayerActionType.Charlie
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftAfterPlayerActions_WithBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAfterPlayerActions(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.BattleBots,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftAfterPlayerActions_AtBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAfterPlayerActions(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftAfterPlayerActions_LastBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null
			}), 0, PlayerColor.Blue);

			player.ShiftAfterPlayerActions(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftAfterPlayerActions_MultipleTimesSameTurn()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.ShiftAfterPlayerActions(2);
			player.ShiftAfterPlayerActions(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftAfterPlayerActions_MultipleTimesConsecutiveTurns()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.ShiftAfterPlayerActions(2);
			player.ShiftAfterPlayerActions(3);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				null,
				PlayerActionType.Charlie
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftFromPlayerActions_NoBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);
			player.Actions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
			player.Actions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;
			player.ShiftFromPlayerActions(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.BattleBots,
				PlayerActionType.Charlie
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftFromPlayerActions_WithBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);
			player.Actions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
			player.Actions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;

			player.ShiftFromPlayerActions(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.BattleBots,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftFromPlayerActions_AtBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);
			player.Actions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
			player.Actions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;

			player.ShiftFromPlayerActions(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftFromPlayerActions_LastBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null
			}), 0, PlayerColor.Blue);
			player.Actions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
			player.Actions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;

			player.ShiftFromPlayerActions(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftFromPlayerActions_MultipleTimesSameTurn()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);
			player.Actions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
			player.Actions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;

			player.ShiftFromPlayerActions(2);
			player.ShiftFromPlayerActions(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftDoubleAction_NotYetPerformed()
		{
			//TODO:
		}

		[Test]
		public static void Test_ShiftDoubleAction_FirstActionPerformed()
		{
			//TODO:
		}

		[Test]
		public static void Test_ShiftDoubleAction_BothPerformed()
		{
			//TODO:
		}

		[Test]
		public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_NoBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousActionAfterPlayerActions(3);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				PlayerActionType.Bravo,
				PlayerActionType.BattleBots,
				PlayerActionType.Charlie
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_WithBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousActionAfterPlayerActions(3);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				PlayerActionType.Bravo,
				PlayerActionType.BattleBots,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_AtBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousActionAfterPlayerActions(3);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				PlayerActionType.Bravo,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_LastBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousActionAfterPlayerActions(3);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				PlayerActionType.Bravo,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_MultipleTimesSameTurn()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousActionAfterPlayerActions(3);
			player.ShiftAndRepeatPreviousActionAfterPlayerActions(3);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				PlayerActionType.Bravo,
				PlayerActionType.Bravo,
				PlayerActionType.Charlie
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[Test]
		public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_MultipleTimesConsecutiveTurns()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousActionAfterPlayerActions(3);
			player.ShiftAndRepeatPreviousActionAfterPlayerActions(4);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				PlayerActionType.Bravo,
				PlayerActionType.Bravo,
				PlayerActionType.Charlie
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}
	}
}
