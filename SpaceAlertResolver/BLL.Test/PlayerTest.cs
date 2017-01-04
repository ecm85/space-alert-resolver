using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLL.Test
{
	[TestClass]
	public class PlayerTest
	{
		private class ActionComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				var first = x as PlayerAction;
				var second = y as PlayerAction;
				if (first == null || second == null)
					return -1;
				return first.ActionType == second.ActionType ? 0 : -1;
			}
		}

		[TestMethod]
		public void Test_Shift_NoBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.Shift(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha,
				PlayerActionType.Bravo,
				null,
				PlayerActionType.BattleBots,
				PlayerActionType.Charlie
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_WithBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.Shift(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
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

		[TestMethod]
		public void Test_Shift_AtBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.Shift(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
				{
					PlayerActionType.Alpha,
					PlayerActionType.Bravo,
					null,
					PlayerActionType.Charlie,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_LastBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null
			}), 0, PlayerColor.Blue);

			player.Shift(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
				{
					PlayerActionType.Alpha,
					PlayerActionType.Bravo,
					null,
					PlayerActionType.Charlie,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_MultipleTimesSameTurn()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.Shift(2);
			player.Shift(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
				{
					PlayerActionType.Alpha,
					PlayerActionType.Bravo,
					null,
					PlayerActionType.Charlie,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_MultipleTimesConsecutiveTurns()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.Shift(2);
			player.Shift(3);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
				{
					PlayerActionType.Alpha,
					PlayerActionType.Bravo,
					null,
					null,
					PlayerActionType.Charlie
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_NoBlanks_RepeatPreviousAction()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
				{
					PlayerActionType.Alpha,
					PlayerActionType.Bravo,
					PlayerActionType.Bravo,
					PlayerActionType.BattleBots,
					PlayerActionType.Charlie
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_ShiftAndRepeatPreviousAction_WithBlanks()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
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

		[TestMethod]
		public void Test_ShiftAndRepeatPreviousAction_AtBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
				{
					PlayerActionType.Alpha,
					PlayerActionType.Bravo,
					PlayerActionType.Bravo,
					PlayerActionType.Charlie,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_ShiftAndRepeatPreviousAction_LastBlank()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
				{
					PlayerActionType.Alpha,
					PlayerActionType.Bravo,
					PlayerActionType.Bravo,
					PlayerActionType.Charlie,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_ShiftAndRepeatPreviousAction_MultipleTimesSameTurn()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(2);
			player.ShiftAndRepeatPreviousAction(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
				{
					PlayerActionType.Alpha,
					PlayerActionType.Bravo,
					PlayerActionType.Bravo,
					PlayerActionType.Bravo,
					PlayerActionType.Charlie
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_ShiftAndRepeatPreviousAction_MultipleTimesConsecutiveTurns()
		{
			var player = new Player(PlayerActionFactory.CreateSingleActionList(null, null, new PlayerActionType?[]
			{
				PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			}), 0, PlayerColor.Blue);

			player.ShiftAndRepeatPreviousAction(2);
			player.ShiftAndRepeatPreviousAction(3);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				null,
				null,
				new PlayerActionType?[]
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
