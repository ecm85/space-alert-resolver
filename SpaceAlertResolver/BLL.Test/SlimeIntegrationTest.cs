using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BLL.Players;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Threats.Internal.Minor.Yellow.Slime;
using BLL.Tracks;
using NUnit.Framework;

namespace BLL.Test
{
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "XYY")]
    [TestFixture]
    public static class SlimeIntegrationTest
    {
        /*
        
            no y's:
                \
                x
                xz

            1 y:
                \
                x
                xy \
                xy z
                xyz \
                xyz z

            2 y's:
                \
                x
                xy \
                xy y' \
                xy y' z
                xy y'z \
                xy y'z z

                xyy' \\
                xyy' \z
                xyy' y' \
                xyy' y' z
                xyy' y'z \
                xyy' y'z z

                xyy'z \\
                xyy'z \z
                xyy'z y' \
                xyy'z y' z
                xyy'z y'z \
                xyy'z y'z z
        */
        //TODO: Clone red tests to blue tests
        //TODO: double slime tests

        private static IEnumerable<PlayerActionType?> ComputerMaintenanceActions => new PlayerActionType?[]
        {
            PlayerActionType.Charlie, null, null,
            PlayerActionType.Charlie, null, null, null,
            PlayerActionType.Charlie, null, null, null, null
        };

        // /
        [Test]
        public static void RedSlimeNoYCrossesNoBreakpoints_NoDamageNoDisabledBattleBots_Defeated()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue, PlayerActionType.Charlie,
                    PlayerActionType.MoveRed, PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck, null,
                    null, null,
                    PlayerActionType.BattleBots, null,
                    PlayerActionType.BattleBots, null
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track1;
            const int timeAppears = 5;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // x
        [Test]
        public static void RedSlimeNoYCrossesX_NoDamageHasDisabledBattleBots_Defeated()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue, PlayerActionType.Charlie,
                    PlayerActionType.MoveRed, PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck, null,
                    null, null,
                    PlayerActionType.BattleBots, null,
                    PlayerActionType.BattleBots, null
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };


            const TrackConfiguration internalTrack = TrackConfiguration.Track1;
            const int timeAppears = 3;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = true;


            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // z
        [Test]
        public static void RedSlimeNoYCrossesXZ_TwoDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 0, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track1;
            const int timeAppears = 3;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 2;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }


        // /
        [Test]
        public static void RedSlimeOneYCrossesNoBreakpoints_NoDamageNoDisabledBattleBots_Defeated()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue, PlayerActionType.Charlie,
                    PlayerActionType.MoveRed, PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck, null,
                    null, null,
                    PlayerActionType.BattleBots, null,
                    PlayerActionType.BattleBots, null
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track4;
            const int timeAppears = 5;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // x
        [Test]
        public static void RedSlimeOneYCrossesX_NoDamageHasDisabledBattleBots_Defeated()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue, PlayerActionType.Charlie,
                    PlayerActionType.MoveRed, PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck, null,
                    null, null,
                    PlayerActionType.BattleBots, null,
                    PlayerActionType.BattleBots, null
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track4;
            const int timeAppears = 3;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xy \
        [Test]
        public static void RedSlimeOneYCrossesXY_ProgenyACrossesNoBreakpoints_NoDamageHasDisabledBattleBots_Defeated()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue, PlayerActionType.Charlie,
                    PlayerActionType.MoveRed, PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck, null,
                    null, null,
                    null, null,
                    null, null,
                    PlayerActionType.BattleBots, null,
                    PlayerActionType.BattleBots, null,
                    PlayerActionType.MoveBlue, PlayerActionType.BattleBots
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track5;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xy z
        [Test]
        public static void RedSlimeOneYCrossesXY_ProgenyACrossesZ_TwoWhiteDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue, PlayerActionType.Charlie,
                    PlayerActionType.MoveRed, PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck, null,
                    null, null,
                    null, null,
                    null, null,
                    PlayerActionType.BattleBots, null,
                    PlayerActionType.BattleBots, null
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track4;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 2;
            const int redDamage = 0;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyz \
        [Test]
        public static void RedSlimeOneYCrossesXYZ_ProgenyACrossesNoBreakpoints_TwoRedDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue, PlayerActionType.Charlie,
                    PlayerActionType.MoveRed, null,
                    PlayerActionType.ChangeDeck, null,
                    null, null,
                    null, null,
                    null, null,
                    null, null,
                    PlayerActionType.BattleBots, null
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track4;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 2;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyz z
        [Test]
        public static void RedSlimeOneYCrossesXYZ_ProgenyACrossesZ_TwoRedDamageTwoWhiteDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 0, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track4;
            const int timeAppears = 3;

            const int blueDamage = 0;
            const int whiteDamage = 2;
            const int redDamage = 2;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }




        // \
        [Test]
        public static void RedSlimeTwoYCrossesNoBreakpoints_NoDamageNoDisabledBattleBots_Defeated()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue, PlayerActionType.Charlie,
                    PlayerActionType.MoveRed, PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck, null,
                    null, null,
                    PlayerActionType.BattleBots, null,
                    PlayerActionType.BattleBots, null
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track6;
            const int timeAppears = 5;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // x
        [Test]
        public static void RedSlimeTwoYCrossesX_NoDamageHasDisabledBattleBots_Defeated()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue, PlayerActionType.Charlie,
                    PlayerActionType.MoveRed, PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck, null,
                    null, null,
                    PlayerActionType.BattleBots, null,
                    PlayerActionType.BattleBots, null
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track6;
            const int timeAppears = 3;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xy \
        [Test]
        public static void RedSlimeTwoYCrossesXY_ProgenyACrossesNoBreakpoints_NoDamageHasDisabledBattleBots_Defeated()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie,
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed, PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie, null,
                    null, null,
                    null, null,
                    null, null,
                    null, null,
                    PlayerActionType.BattleBots, null,
                    PlayerActionType.BattleBots, null
                }), 1, PlayerColor.Red, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 2, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track6;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xy y' \
        [Test]
        public static void RedSlimeTwoYCrossesXY_ProgenyACrossesY2_ProgenyBCrossesNoBreakpoints_NoDamageHasDisabledBattleBots_Defeated()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie,
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie,
                    PlayerActionType.BattleBots,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots,
                    PlayerActionType.MoveBlue,
                    PlayerActionType.BattleBots
                }), 1, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 2, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xy y' z
        [Test]
        public static void RedSlimeTwoYCrossesXY_ProgenyACrossesY2_ProgenyBCrossesZ_TwoBlueDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie,
                    PlayerActionType.BattleBots,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots,
                    PlayerActionType.MoveBlue,
                    PlayerActionType.BattleBots
                }), 0, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 2;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xy y'z \
        [Test]
        public static void RedSlimeTwoYCrossesXY_ProgenyACrossesY2Z_ProgenyBCrossesNoBreakpoints_TwoWhiteDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie,
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie,
                    PlayerActionType.BattleBots,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 1, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 2, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 2;
            const int redDamage = 0;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xy y'z z
        [Test]
        public static void RedSlimeTwoYCrossesXY_ProgenyACrossesY2Z_ProgenyBCrossesZ_TwoWhiteDamageTwoBlueDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie,
                    PlayerActionType.BattleBots,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 0, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 2;
            const int whiteDamage = 2;
            const int redDamage = 0;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy' \\
        [Test]
        public static void RedSlimeTwoYCrossesXYY2_ProgenyACrossesNoBreakpoints_ProgenyA2CrossesNoBreakpoints_NoDamageHasDisabledBattleBots_Defeated()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.MoveRed, 
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots,
                    null,
                    PlayerActionType.BattleBots 
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie,
                    PlayerActionType.BattleBots,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 1, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 2, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy' \z
        [Test]
        public static void RedSlimeTwoYCrossesXYY2_ProgenyACrossesNoBreakpoints_ProgenyA2CrossesZ_TwoWhiteDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.MoveRed,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots,
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie,
                    PlayerActionType.BattleBots,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 1, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 2, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 2;
            const int redDamage = 0;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy' y' \
        [Test]
        public static void RedSlimeTwoYCrossesXYY2_ProgenyACrossesY2_ProgenyBCrossesNoBreakpoints_NoDamageHasDisabledBattleBots_Defeated()
        {
            var redPlayerActions = PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
            {
                PlayerActionType.MoveRed, PlayerActionType.ChangeDeck,
                PlayerActionType.Charlie, null,
                null, null,
                PlayerActionType.BattleBots, null,
                null, null,
                null, null,
                null, null,
                null, null,
                null, null,
                PlayerActionType.BattleBots, null
            }).ToList();
            redPlayerActions.Add(new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.BattleBots, PlayerActionType.AdvancedSpecialization));
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie,
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(redPlayerActions, 1, PlayerColor.Red, PlayerSpecialization.SpecialOps),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 2, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = true;
            const bool isSurvived = false;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy' y' z
        [Test]
        public static void RedSlimeTwoYCrossesXYY2_ProgenyACrossesY2_ProgenyBCrossesZ_TwoBlueDamageHasDisabledBattleBots_Survived()
        {
            var redPlayerActions = PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
            {
                PlayerActionType.MoveRed, PlayerActionType.ChangeDeck,
                PlayerActionType.Charlie, null,
                null, null,
                PlayerActionType.BattleBots, null,
                null, null,
                null, null,
                null, null,
                null, null,
                null, null,
                PlayerActionType.BattleBots, null
            }).ToList();
            redPlayerActions.Add(new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.BattleBots, PlayerActionType.AdvancedSpecialization));
            var players = new List<Player>
            {
                new Player(redPlayerActions, 0, PlayerColor.Red, PlayerSpecialization.SpecialOps),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 2;
            const int whiteDamage = 0;
            const int redDamage = 0;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy' y'z \
        [Test]
        public static void RedSlimeTwoYCrossesXYY2_ProgenyACrossesY2Z_ProgenyBCrossesNoBreakpoints_TwoWhiteDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie,
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie,
                    PlayerActionType.BattleBots,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 1, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 2, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 2;
            const int redDamage = 0;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy' y'z z
        [Test]
        public static void RedSlimeTwoYCrossesXYY2_ProgenyACrossesY2Z_ProgenyBCrossesZ_TwoWhiteDamageTwoBlueDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie,
                    PlayerActionType.BattleBots,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 0, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 2;
            const int whiteDamage = 2;
            const int redDamage = 0;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy'z \\
        [Test]
        public static void RedSlimeTwoYCrossesXYY2Z_ProgenyACrossesNoBreakpoints_ProgenyA2CrossesNoBreakpoints_TwoRedDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie, 
                    PlayerActionType.MoveRed, 
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots,
                    null,
                    PlayerActionType.BattleBots
                }), 0, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 2;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy'z \z
        [Test]
        public static void RedSlimeTwoYCrossesXYY2Z_ProgenyACrossesNoBreakpoints_ProgenyA2CrossesZ_TwoRedDamageTwoWhiteDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie,
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots,
                }), 0, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 2;
            const int redDamage = 2;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy'z y' \
        [Test]
        public static void RedSlimeTwoYCrossesXYY2Z_ProgenyACrossesY2_ProgenyBCrossesNoBreakpoints_TwoRedDamageHasDisabledBattleBots_Survived()
        {
            var redPlayerActions = PlayerActionFactory.CreateDoubleActionList(new PlayerActionType?[]
            {
                PlayerActionType.MoveRed, PlayerActionType.ChangeDeck,
                PlayerActionType.Charlie, PlayerActionType.MoveBlue, 
                null, null,
                null, null,
                null, null,
                null, null,
                null, null,
                null, null,
                null, null,
                PlayerActionType.BattleBots, null
            }).ToList();
            redPlayerActions.Add(new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.BattleBots, PlayerActionType.AdvancedSpecialization));
            var players = new List<Player>
            {
                new Player(redPlayerActions, 1, PlayerColor.Red, PlayerSpecialization.SpecialOps),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 2, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 0;
            const int redDamage = 2;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = false;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy'z y' z
        [Test]
        public static void RedSlimeTwoYCrossesXYY2Z_ProgenyACrossesY2_ProgenyBCrossesZ_TwoRedDamageTwoBlueDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie,
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots,
                }), 0, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 2;
            const int whiteDamage = 0;
            const int redDamage = 2;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy'z y'z \
        [Test]
        public static void RedSlimeTwoYCrossesXYY2Z_ProgenyACrossesY2Z_ProgenyBCrossesNoBreakpoints_TwoRedDamageTwoWhiteDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue,
                    PlayerActionType.Charlie,
                    null,
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.BattleBots
                }), 0, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 1, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;
            const int timeAppears = 4;

            const int blueDamage = 0;
            const int whiteDamage = 2;
            const int redDamage = 2;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        // xyy'z y'z z
        [Test]
        public static void RedSlimeTwoYCrossesXYY2Z_ProgenyACrossesY2Z_ProgenyBCrossesZ_TwoRedDamageTwoWhiteDamageTwoBlueDamageHasDisabledBattleBots_Survived()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(ComputerMaintenanceActions), 0, PlayerColor.Green)
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track6;
            const int timeAppears = 3;

            const int blueDamage = 2;
            const int whiteDamage = 2;
            const int redDamage = 2;
            const bool isDefeated = false;
            const bool isSurvived = true;
            const bool battleBotsDisabled = true;

            SlimeBHelper(isDefeated, isSurvived, internalTrack, timeAppears, players, blueDamage, redDamage, whiteDamage, battleBotsDisabled);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])")]
        private static void SlimeBHelper(bool isDefeated, bool isSurvived, TrackConfiguration internalTrack, int timeAppears,
            List<Player> players, int blueDamage, int redDamage, int whiteDamage, bool battleBotsDisabled)
        {
            var totalPoints = 0;
            if (isDefeated)
                totalPoints = 6;
            if (isSurvived)
                totalPoints = 3;
            var otherTracks = EnumFactory.All<TrackConfiguration>().Except(new[] {internalTrack}).ToList();
            var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
            {
                {ZoneLocation.Blue, otherTracks.First()},
                {ZoneLocation.Red, otherTracks.Skip(1).First()},
                {ZoneLocation.White, otherTracks.Skip(2).First()}
            };
            var externalThreats = new ExternalThreat[0];

            var slimeB = new SlimeB();
            slimeB.SetInitialPlacement(timeAppears);
            var internalThreats = new InternalThreat[] {slimeB};

            var bonusThreats = new Threat[0];

            var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack, null);
            game.StartGame();
            for (var currentTurn = 0; currentTurn < game.NumberOfTurns + 1; currentTurn++)
                game.PerformTurn();
            Assert.AreEqual(GameStatus.Won, game.GameStatus);
            Assert.AreEqual(blueDamage, game.SittingDuck.BlueZone.TotalDamage);
            Assert.AreEqual(redDamage, game.SittingDuck.RedZone.TotalDamage);
            Assert.AreEqual(whiteDamage, game.SittingDuck.WhiteZone.TotalDamage);
            Assert.AreEqual(0, players.Count(player => player.IsKnockedOut));
            Assert.AreEqual(blueDamage, game.SittingDuck.BlueZone.CurrentDamageTokens.Count);
            Assert.AreEqual(redDamage, game.SittingDuck.RedZone.CurrentDamageTokens.Count);
            Assert.AreEqual(whiteDamage, game.SittingDuck.WhiteZone.CurrentDamageTokens.Count);
            var battleBotsAreDisabledInPlace = game.SittingDuck.RedZone.LowerRedStation.BattleBotsComponent.BattleBots?.IsDisabled ?? false;
            Assert.AreEqual(battleBotsDisabled, battleBotsAreDisabledInPlace);

            Assert.AreEqual(isDefeated, game.ThreatController.DefeatedThreats.Any());
            if (totalPoints != game.TotalPoints)
            {
                var errorMessage = $"Survived + points didn't match. Expected: {totalPoints} points, survived: {isSurvived}. Actual: {game.TotalPoints} points, survived: {game.ThreatController.SurvivedThreats.Any()}.";
                Assert.Fail(errorMessage);
            }
        }
    }
}
