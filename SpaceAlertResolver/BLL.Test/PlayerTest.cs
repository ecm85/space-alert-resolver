using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace BLL.Test
{
	[TestFixture]
	public static class PlayerTest
	{
		private class ActionComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				var first = x as PlayerAction;
				var second = y as PlayerAction;
				if (first == null || second == null)
					return -1;
				return first.FirstActionType == second.FirstActionType &&
					first.SecondActionType == second.SecondActionType &&
					first.BonusActionType == second.BonusActionType ?
					0 :
					-1;
			}
		}

		[Test]
		public static void Test_Shift_NoBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.Shift(3);
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
		public static void Test_Shift_WithBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.Shift(3);
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
		public static void Test_Shift_AtBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.Shift(3);
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
		public static void Test_Shift_LastBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null
			}), 0, PlayerColor.Blue);

			player.Shift(3);
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
		public static void Test_Shift_MultipleTimesSameTurn()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.Shift(3);
			player.Shift(3);
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
		public static void Test_Shift_MultipleTimesConsecutiveTurns()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.Shift(3);
			player.Shift(4);
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
		public static void Test_Shift_NoBlanks_RepeatPreviousAction()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(3);
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
		public static void Test_ShiftAndRepeatPreviousAction_WithBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(3);
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
		public static void Test_ShiftAndRepeatPreviousAction_AtBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(3);
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
		public static void Test_ShiftAndRepeatPreviousAction_LastBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(3);
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
		public static void Test_ShiftAndRepeatPreviousAction_MultipleTimesSameTurn()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(3);
			player.ShiftAndRepeatPreviousAction(3);
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
		public static void Test_ShiftAndRepeatPreviousAction_MultipleTimesConsecutiveTurns()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(3);
			player.ShiftAndRepeatPreviousAction(4);
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
