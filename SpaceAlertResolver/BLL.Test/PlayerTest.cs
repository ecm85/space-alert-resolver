using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BLL.Players;
using NUnit.Framework;

namespace BLL.Test
{
    [TestFixture]
    public static class PlayerTest
    {
        //TODO: Add tests for shifting at end
        [Test]
        public static void Test_ShiftAfterPlayerActions_NoBlanks()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
            }), 0, PlayerColor.blue);

            player.ShiftAfterPlayerActions(2);
            var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha,
                PlayerActionType.Bravo,
                null,
                PlayerActionType.BattleBots,
                PlayerActionType.Charlie
            });
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAfterPlayerActions_WithBlanks()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
            }), 0, PlayerColor.blue);

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
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAfterPlayerActions_AtBlank()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
            }), 0, PlayerColor.blue);

            player.ShiftAfterPlayerActions(2);
            var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha,
                PlayerActionType.Bravo,
                null,
                PlayerActionType.Charlie,
                PlayerActionType.ChangeDeck
            });
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAfterPlayerActions_LastBlank()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null
            }), 0, PlayerColor.blue);

            player.ShiftAfterPlayerActions(2);
            var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha,
                PlayerActionType.Bravo,
                null,
                PlayerActionType.Charlie,
                PlayerActionType.ChangeDeck
            });
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAfterPlayerActions_MultipleTimesSameTurn()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
            }), 0, PlayerColor.blue);

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
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAfterPlayerActions_MultipleTimesConsecutiveTurns()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
            }), 0, PlayerColor.blue);

            player.ShiftAfterPlayerActions(2);
            player.ShiftAfterPlayerActions(3);
            var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha,
                PlayerActionType.Bravo,
                null,
                null,
                PlayerActionType.Charlie
            }).ToList();
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftFromPlayerActions_NoBlanks()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
            }), 0, PlayerColor.blue);
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
            }).ToList();
            expectedActions[0].FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions[1].FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftFromPlayerActions_WithBlanks()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
            }), 0, PlayerColor.blue);
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
            }).ToList();
            expectedActions[0].FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions[1].FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftFromPlayerActions_AtBlank()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
            }), 0, PlayerColor.blue);
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
            }).ToList();
            expectedActions[0].FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions[1].FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;

            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftFromPlayerActions_LastBlank()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null
            }), 0, PlayerColor.blue);
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
            }).ToList();
            expectedActions[0].FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions[1].FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftFromPlayerActions_MultipleTimesSameTurn()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
            }), 0, PlayerColor.blue);
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
            }).ToList();
            expectedActions[0].FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions[1].FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftDoubleAction_AfterPlayerActions()
        {
            var player = new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, null,
                PlayerActionType.MoveRed, PlayerActionType.Bravo,
                PlayerActionType.ChangeDeck, PlayerActionType.Charlie,
                PlayerActionType.BattleBots, PlayerActionType.Charlie,
                PlayerActionType.ChangeDeck, PlayerActionType.MoveBlue
            }), 0, PlayerColor.blue);

            player.Actions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            player.Actions.ElementAt(0).SecondActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            player.Actions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            player.Actions.ElementAt(1).SecondActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            player.ShiftAfterPlayerActions(2);
            var expectedActions = PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, null,
                PlayerActionType.MoveRed, PlayerActionType.Bravo,
                null, null,
                PlayerActionType.ChangeDeck, PlayerActionType.Charlie,
                PlayerActionType.BattleBots, PlayerActionType.Charlie
            }).ToList();
            expectedActions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions.ElementAt(0).SecondActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions.ElementAt(1).SecondActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftDoubleAction_FromPlayerActions_FirstActionPerforming()
        {
            var player = new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, null,
                PlayerActionType.MoveRed, PlayerActionType.Bravo,
                PlayerActionType.ChangeDeck, PlayerActionType.Charlie,
                PlayerActionType.BattleBots, PlayerActionType.Charlie,
                PlayerActionType.ChangeDeck, PlayerActionType.MoveBlue
            }), 0, PlayerColor.blue);

            player.Actions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            player.Actions.ElementAt(0).SecondActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            player.Actions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;
            player.ShiftFromPlayerActions(2);
            var expectedActions = PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, null,
                PlayerActionType.MoveRed, null,
                PlayerActionType.Bravo, null,
                PlayerActionType.ChangeDeck, PlayerActionType.Charlie,
                PlayerActionType.BattleBots, PlayerActionType.Charlie
            }).ToList();
            expectedActions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions.ElementAt(0).SecondActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performing;
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftDoubleAction_BothPerformed()
        {
            var player = new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, null,
                PlayerActionType.MoveRed, PlayerActionType.Bravo,
                PlayerActionType.ChangeDeck, PlayerActionType.Charlie,
                PlayerActionType.BattleBots, PlayerActionType.Charlie,
                PlayerActionType.ChangeDeck, PlayerActionType.MoveBlue
            }), 0, PlayerColor.blue);

            player.Actions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            player.Actions.ElementAt(0).SecondActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            player.Actions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            player.Actions.ElementAt(1).SecondActionSegment.SegmentStatus = PlayerActionStatus.Performing;
            player.ShiftFromPlayerActions(2);
            var expectedActions = PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, null,
                PlayerActionType.MoveRed, PlayerActionType.Bravo, 
                null, null,
                PlayerActionType.ChangeDeck, PlayerActionType.Charlie,
                PlayerActionType.BattleBots, PlayerActionType.Charlie
            }).ToList();
            expectedActions.ElementAt(0).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions.ElementAt(0).SecondActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions.ElementAt(1).FirstActionSegment.SegmentStatus = PlayerActionStatus.Performed;
            expectedActions.ElementAt(1).SecondActionSegment.SegmentStatus = PlayerActionStatus.Performing;
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_NoBlanks()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
            }), 0, PlayerColor.blue);

            player.ShiftAndRepeatPreviousActionAfterPlayerActions(3);
            var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha,
                PlayerActionType.Bravo,
                PlayerActionType.Bravo,
                PlayerActionType.BattleBots,
                PlayerActionType.Charlie
            });
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_WithBlanks()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.BattleBots, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
            }), 0, PlayerColor.blue);

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
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_AtBlank()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, null, PlayerActionType.Charlie, PlayerActionType.ChangeDeck
            }), 0, PlayerColor.blue);

            player.ShiftAndRepeatPreviousActionAfterPlayerActions(3);
            var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha,
                PlayerActionType.Bravo,
                PlayerActionType.Bravo,
                PlayerActionType.Charlie,
                PlayerActionType.ChangeDeck
            });
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_LastBlank()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null
            }), 0, PlayerColor.blue);

            player.ShiftAndRepeatPreviousActionAfterPlayerActions(3);
            var expectedActions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha,
                PlayerActionType.Bravo,
                PlayerActionType.Bravo,
                PlayerActionType.Charlie,
                PlayerActionType.ChangeDeck
            });
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_MultipleTimesSameTurn()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
            }), 0, PlayerColor.blue);

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
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [Test]
        public static void Test_ShiftAndRepeatPreviousActionAfterPlayerActions_MultipleTimesConsecutiveTurns()
        {
            var player = new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
            {
                PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.HeroicA
            }), 0, PlayerColor.blue);

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
            AssertActionAreEqual(expectedActions.ToList(), player.Actions.ToList());
        }

        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        private static void AssertActionAreEqual(IList<PlayerAction> expected, IList<PlayerAction> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for(var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].FirstActionSegment.SegmentType, actual[i].FirstActionSegment.SegmentType, $"Action {i}");
                Assert.AreEqual(expected[i].FirstActionSegment.SegmentStatus, actual[i].FirstActionSegment.SegmentStatus, $"Action {i}");
                Assert.AreEqual(expected[i].SecondActionSegment.SegmentType, actual[i].SecondActionSegment.SegmentType, $"Action {i}");
                Assert.AreEqual(expected[i].SecondActionSegment.SegmentStatus, actual[i].SecondActionSegment.SegmentStatus, $"Action {i}");
            }
        }
    }
}
