﻿using System;
using System.Collections;
using System.Collections.Generic;
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
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, PlayerActionType.BattleBots, PlayerActionType.C, PlayerActionType.ChangeDeck
			});

			player.Shift(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A,
				PlayerActionType.B,
				null,
				PlayerActionType.BattleBots,
				PlayerActionType.C
			});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_WithBlanks()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, PlayerActionType.BattleBots, null, PlayerActionType.C, PlayerActionType.ChangeDeck
			});

			player.Shift(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					null,
					PlayerActionType.BattleBots,
					PlayerActionType.C,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_AtBlank()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, null, PlayerActionType.C, PlayerActionType.ChangeDeck
			});

			player.Shift(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					null,
					PlayerActionType.C,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_LastBlank()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, PlayerActionType.C, PlayerActionType.ChangeDeck, null
			});

			player.Shift(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					null,
					PlayerActionType.C,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_MultipleTimesSameTurn()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, PlayerActionType.C, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			});

			player.Shift(2);
			player.Shift(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					null,
					PlayerActionType.C,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_MultipleTimesConsecutiveTurns()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, PlayerActionType.C, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			});

			player.Shift(2);
			player.Shift(3);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					null,
					null,
					PlayerActionType.C
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_Shift_NoBlanks_RepeatPreviousAction()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, PlayerActionType.BattleBots, PlayerActionType.C, PlayerActionType.ChangeDeck
			});

			player.ShiftAndRepeatPreviousAction(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					PlayerActionType.B,
					PlayerActionType.BattleBots,
					PlayerActionType.C
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_ShiftAndRepeatPreviousAction_WithBlanks()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, PlayerActionType.BattleBots, null, PlayerActionType.C, PlayerActionType.ChangeDeck
			});

			player.ShiftAndRepeatPreviousAction(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					PlayerActionType.B,
					PlayerActionType.BattleBots,
					PlayerActionType.C,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_ShiftAndRepeatPreviousAction_AtBlank()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, null, PlayerActionType.C, PlayerActionType.ChangeDeck
			});

			player.ShiftAndRepeatPreviousAction(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					PlayerActionType.B,
					PlayerActionType.C,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_ShiftAndRepeatPreviousAction_LastBlank()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, PlayerActionType.C, PlayerActionType.ChangeDeck, null
			});

			player.ShiftAndRepeatPreviousAction(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					PlayerActionType.B,
					PlayerActionType.C,
					PlayerActionType.ChangeDeck
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_ShiftAndRepeatPreviousAction_MultipleTimesSameTurn()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, PlayerActionType.C, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			});

			player.ShiftAndRepeatPreviousAction(2);
			player.ShiftAndRepeatPreviousAction(2);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					PlayerActionType.B,
					PlayerActionType.B,
					PlayerActionType.C
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}

		[TestMethod]
		public void Test_ShiftAndRepeatPreviousAction_MultipleTimesConsecutiveTurns()
		{
			var player = new Player();
			player.Actions = PlayerActionFactory.CreateSingleActionList(player, new PlayerActionType?[]
			{
				PlayerActionType.A, PlayerActionType.B, PlayerActionType.C, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
			});

			player.ShiftAndRepeatPreviousAction(2);
			player.ShiftAndRepeatPreviousAction(3);
			var expectedActions = PlayerActionFactory.CreateSingleActionList(
				player,
				new PlayerActionType?[]
				{
					PlayerActionType.A,
					PlayerActionType.B,
					PlayerActionType.B,
					PlayerActionType.B,
					PlayerActionType.C
				});
			CollectionAssert.AreEqual(expectedActions.ToList(), player.Actions.ToList(), new ActionComparer());
		}
	}
}
