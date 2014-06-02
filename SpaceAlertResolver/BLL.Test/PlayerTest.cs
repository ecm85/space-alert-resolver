using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLL.Test
{
	[TestClass]
	public class PlayerTest
	{
		[TestMethod]
		public void TestShiftNoBlanks()
		{
			var player = new Player
			{
				Actions = new List<PlayerAction>
				{
					PlayerAction.A, PlayerAction.B, PlayerAction.BattleBots, PlayerAction.C, PlayerAction.ChangeDeck
				}
			};

			player.Shift(2);
			var expectedActions = new[] {PlayerAction.A, PlayerAction.B, PlayerAction.None, PlayerAction.BattleBots, PlayerAction.C};
			CollectionAssert.AreEqual(expectedActions, player.Actions);
		}

		[TestMethod]
		public void TestShiftWithBlanks()
		{
			var player = new Player
			{
				Actions = new List<PlayerAction>
				{
					PlayerAction.A, PlayerAction.B, PlayerAction.BattleBots, PlayerAction.None, PlayerAction.C, PlayerAction.ChangeDeck
				}
			};

			player.Shift(2);
			var expectedActions = new[] { PlayerAction.A, PlayerAction.B, PlayerAction.None, PlayerAction.BattleBots, PlayerAction.C, PlayerAction.ChangeDeck };
			CollectionAssert.AreEqual(expectedActions, player.Actions);
		}

		[TestMethod]
		public void TestShiftAtBlank()
		{
			var player = new Player
			{
				Actions = new List<PlayerAction>
				{
					PlayerAction.A, PlayerAction.B, PlayerAction.None, PlayerAction.C, PlayerAction.ChangeDeck
				}
			};

			player.Shift(2);
			var expectedActions = new[] { PlayerAction.A, PlayerAction.B, PlayerAction.None, PlayerAction.C, PlayerAction.ChangeDeck };
			CollectionAssert.AreEqual(expectedActions, player.Actions);
		}

		[TestMethod]
		public void TestShiftLastBlank()
		{
			var player = new Player
			{
				Actions = new List<PlayerAction>
				{
					PlayerAction.A, PlayerAction.B, PlayerAction.C, PlayerAction.ChangeDeck, PlayerAction.None
				}
			};

			player.Shift(2);
			var expectedActions = new[] { PlayerAction.A, PlayerAction.B, PlayerAction.None, PlayerAction.C, PlayerAction.ChangeDeck };
			CollectionAssert.AreEqual(expectedActions, player.Actions);
		}
	}
}
