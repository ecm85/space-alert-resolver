using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.External.Minor.White;
using BLL.Threats.Internal;
using BLL.Tracks;
using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.Threats;
using BLL.Threats.External.Serious.White;
using BLL.Threats.Internal.Minor.White;
using BLL.Threats.Internal.Serious.White;
using BLL.Threats.Internal.Serious.Yellow;
using NUnit.Framework;

namespace BLL.Test
{
    [TestFixture]
    public static class GameIntegrationTest
    {
        [Test]
        public static void JustAFighterNoActions()
        {
            var players = Enumerable.Range(0, 1).Select(index => new Player (new List<PlayerAction>(), 0, PlayerColor.Blue)).ToList();

            var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
            {
                {ZoneLocation.Blue, TrackConfiguration.Track1},
                {ZoneLocation.Red, TrackConfiguration.Track5},
                {ZoneLocation.White, TrackConfiguration.Track3}
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track4;

            var fighter = new Fighter();
            fighter.SetInitialPlacement(5, ZoneLocation.Red);
            var externalThreats = new ExternalThreat[] { fighter };

            var internalThreats = new InternalThreat[0];
            var bonusThreats = new Threat[0];
            var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack, null);
            game.StartGame();
            for (var currentTurn = 0; currentTurn < game.NumberOfTurns + 1; currentTurn++)
                game.PerformTurn();

            Assert.AreEqual(GameStatus.Won, game.GameStatus);
            Assert.AreEqual(0, game.SittingDuck.BlueZone.TotalDamage);
            Assert.AreEqual(5, game.SittingDuck.RedZone.TotalDamage);
            Assert.AreEqual(0, game.SittingDuck.WhiteZone.TotalDamage);
            Assert.AreEqual(0, game.ThreatController.DefeatedThreats.Count());
            Assert.AreEqual(1, game.ThreatController.SurvivedThreats.Count());
            Assert.AreEqual(2, game.TotalPoints);
            Assert.AreEqual(5, game.SittingDuck.Zones.ElementAt(0).CurrentDamageTokens.Count);
            Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(1).CurrentDamageTokens.Count);
            Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(2).CurrentDamageTokens.Count);
        }

        [Test]
        public static void SixBasicThreats()
        {
            var players = GetPlayers();

            var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
            {
                {ZoneLocation.Blue, TrackConfiguration.Track1},
                {ZoneLocation.Red, TrackConfiguration.Track2},
                {ZoneLocation.White, TrackConfiguration.Track3},
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track4;

            var destroyer = new Destroyer();
            destroyer.SetInitialPlacement(4, ZoneLocation.Blue);
            var fighter1 = new Fighter();
            fighter1.SetInitialPlacement(5, ZoneLocation.Red);
            var fighter2 = new Fighter();
            fighter2.SetInitialPlacement(6, ZoneLocation.White);
            var externalThreats = new ExternalThreat[] { destroyer, fighter1, fighter2 };

            var skirmishers = new SkirmishersA();
            skirmishers.SetInitialPlacement(4);
            var fissure = new Fissure();
            fissure.SetInitialPlacement(2);
            var nuclearDevice = new NuclearDevice();
            nuclearDevice.SetInitialPlacement(6);
            var internalThreats = new InternalThreat[] { skirmishers, fissure, nuclearDevice };

            var bonusThreats = new Threat[0];

            var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack, null);
            game.StartGame();
            for (var currentTurn = 0; currentTurn < game.NumberOfTurns + 1; currentTurn++)
                game.PerformTurn();

            Assert.AreEqual(GameStatus.Won, game.GameStatus);
            Assert.AreEqual(4, game.SittingDuck.BlueZone.TotalDamage);
            Assert.AreEqual(3, game.SittingDuck.RedZone.TotalDamage);
            Assert.AreEqual(3, game.SittingDuck.WhiteZone.TotalDamage);
            Assert.AreEqual(3, game.ThreatController.DefeatedThreats.Count());
            Assert.AreEqual(3, game.ThreatController.SurvivedThreats.Count());
            Assert.AreEqual(30, game.TotalPoints);
            Assert.AreEqual(3, game.SittingDuck.Zones.ElementAt(0).CurrentDamageTokens.Count);
            Assert.AreEqual(3, game.SittingDuck.Zones.ElementAt(1).CurrentDamageTokens.Count);
            Assert.AreEqual(4, game.SittingDuck.Zones.ElementAt(2).CurrentDamageTokens.Count);
        }

        private static IList<Player> GetPlayers()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    null,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Bravo,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Alpha,
                    PlayerActionType.Alpha,
                    PlayerActionType.Alpha,
                    PlayerActionType.Alpha,
                    PlayerActionType.Alpha,
                    PlayerActionType.Alpha
                }), 0, PlayerColor.Blue),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie,
                    PlayerActionType.ChangeDeck,
                    null,
                    PlayerActionType.Charlie,
                    PlayerActionType.BattleBots,
                    PlayerActionType.Charlie,
                    PlayerActionType.BattleBots,
                    PlayerActionType.Alpha
                }), 1, PlayerColor.Green),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    null,
                    PlayerActionType.Charlie,
                    null,
                    null,
                    PlayerActionType.Charlie,
                    PlayerActionType.ChangeDeck,
                    PlayerActionType.Charlie
                }), 2, PlayerColor.Purple),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.Charlie
                }), 3, PlayerColor.Red),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    null,
                    PlayerActionType.ChangeDeck,
                    null,
                    null,
                    null,
                    null,
                    PlayerActionType.Charlie
                }), 4, PlayerColor.Yellow),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.TeleportBlueLower,
                    PlayerActionType.TeleportRedUpper,
                    PlayerActionType.TeleportWhiteLower,
                    PlayerActionType.TeleportWhiteUpper
                }), 5, PlayerColor.Blue)
            };
            return players;
        }

        [Test]
        public static void EzraCampaign1Mission1()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed, null, PlayerActionType.BasicSpecialization,
                    PlayerActionType.Alpha, PlayerActionType.Alpha, PlayerActionType.Alpha, PlayerActionType.MoveBlue,
                    PlayerActionType.MoveBlue, PlayerActionType.Alpha, PlayerActionType.Alpha, PlayerActionType.Alpha, null
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed, PlayerActionType.ChangeDeck, PlayerActionType.Alpha,
                    PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.MoveBlue, PlayerActionType.MoveBlue,
                    null, PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Alpha, null
                }), 1, PlayerColor.Green),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    null, null, null,
                    PlayerActionType.MoveBlue, PlayerActionType.Charlie, PlayerActionType.ChangeDeck, PlayerActionType.BattleBots,
                    PlayerActionType.BattleBots,
                    null, PlayerActionType.MoveRed, null, PlayerActionType.Charlie
                }), 2, PlayerColor.Purple),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.Charlie, null, null,
                    PlayerActionType.BasicSpecialization, null, PlayerActionType.ChangeDeck, null,
                    PlayerActionType.BasicSpecialization, null, null, null, PlayerActionType.Charlie
                }), 3, PlayerColor.Red, PlayerSpecialization.DataAnalyst),
            };

            var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
            {
                {ZoneLocation.Blue, TrackConfiguration.Track3},
                {ZoneLocation.Red, TrackConfiguration.Track2},
                {ZoneLocation.White, TrackConfiguration.Track7},
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track4;

            var dimensionSpider = new DimensionSpider();
            dimensionSpider.SetInitialPlacement(6, ZoneLocation.Blue);
            var asteroid = new Asteroid();
            asteroid.SetInitialPlacement(4, ZoneLocation.Red);
            var cryoshieldFighter = new CryoshieldFighter();
            cryoshieldFighter.SetInitialPlacement(1, ZoneLocation.Red);
            var externalThreats = new ExternalThreat[] { dimensionSpider, asteroid, cryoshieldFighter };

            var shambler = new Shambler();
            shambler.SetInitialPlacement(5);
            var internalThreats = new InternalThreat[] { shambler };

            var bonusThreats = new Threat[0];

            var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack, null);
            game.StartGame();
            for (var currentTurn = 0; currentTurn < game.NumberOfTurns + 1; currentTurn++)
                game.PerformTurn();
            Assert.AreEqual(GameStatus.Won, game.GameStatus);
            Assert.AreEqual(0, game.SittingDuck.BlueZone.TotalDamage);
            Assert.AreEqual(1, game.SittingDuck.RedZone.TotalDamage);
            Assert.AreEqual(0, game.SittingDuck.WhiteZone.TotalDamage);
            Assert.AreEqual(3, game.ThreatController.DefeatedThreats.Count());
            Assert.AreEqual(1, game.ThreatController.SurvivedThreats.Count());
            Assert.AreEqual(20 + 4 + 2 + 2, game.TotalPoints);
            Assert.AreEqual(1, game.SittingDuck.Zones.ElementAt(0).CurrentDamageTokens.Count);
            Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(1).CurrentDamageTokens.Count);
            Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(2).CurrentDamageTokens.Count);
        }

        [Test]
        public static void EzraCampaign1Mission2()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    null, PlayerActionType.MoveBlue, PlayerActionType.BasicSpecialization,
                    null, PlayerActionType.AdvancedSpecialization, PlayerActionType.ChangeDeck, PlayerActionType.Alpha, 
                    PlayerActionType.Alpha, null, null, null, null
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed, PlayerActionType.BasicSpecialization, null,
                    null, PlayerActionType.Alpha, PlayerActionType.Alpha, PlayerActionType.MoveBlue,
                    PlayerActionType.BasicSpecialization, null, null, null, null
                }), 1, PlayerColor.Green, PlayerSpecialization.PulseGunner),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveRed, PlayerActionType.ChangeDeck, PlayerActionType.Bravo,
                    PlayerActionType.Alpha, PlayerActionType.Alpha, PlayerActionType.Alpha, null,
                    null, null, null, null, null
                }), 2, PlayerColor.Purple),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.Charlie, null, null,
                    PlayerActionType.BasicSpecialization, PlayerActionType.MoveBlue, PlayerActionType.Alpha, PlayerActionType.Alpha,
                    PlayerActionType.MoveRed, PlayerActionType.BasicSpecialization, null, null, null
                }), 3, PlayerColor.Red, PlayerSpecialization.DataAnalyst)
            };

            var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
            {
                {ZoneLocation.Blue, TrackConfiguration.Track1},
                {ZoneLocation.Red, TrackConfiguration.Track2},
                {ZoneLocation.White, TrackConfiguration.Track7},
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track4;

            var dimensionSpider = new DimensionSpider();
            dimensionSpider.SetInitialPlacement(6, ZoneLocation.Blue);
            var cryoshieldFrigate = new CryoshieldFrigate();
            cryoshieldFrigate.SetInitialPlacement(4, ZoneLocation.Red);
            var energyCloud = new EnergyCloud();
            energyCloud.SetInitialPlacement(1, ZoneLocation.Red);
            var externalThreats = new ExternalThreat[] { dimensionSpider, cryoshieldFrigate, energyCloud };

            var battleBotUprising = new BattleBotUprising();
            battleBotUprising.SetInitialPlacement(5);
            var internalThreats = new InternalThreat[] { battleBotUprising };

            var bonusThreats = new Threat[0];

            var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack, null);
            game.StartGame();
            for (var currentTurn = 0; currentTurn < game.NumberOfTurns + 1; currentTurn++)
                game.PerformTurn();
            Assert.AreEqual(GameStatus.Won, game.GameStatus);
            Assert.AreEqual(0, game.SittingDuck.BlueZone.TotalDamage);
            Assert.AreEqual(0, game.SittingDuck.RedZone.TotalDamage);
            Assert.AreEqual(0, game.SittingDuck.WhiteZone.TotalDamage);
            Assert.AreEqual(3, game.ThreatController.DefeatedThreats.Count());
            Assert.AreEqual(1, game.ThreatController.SurvivedThreats.Count());
            Assert.AreEqual(20 + 4 + 2, game.TotalPoints);
            Assert.AreEqual(2, players.Count(player => player.IsKnockedOut));
            Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(0).CurrentDamageTokens.Count);
            Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(1).CurrentDamageTokens.Count);
            Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(2).CurrentDamageTokens.Count);
        }

        [Test]
        public static void EzraCampaign1Mission3()
        {
            var players = new List<Player>
            {
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.MoveBlue, null, PlayerActionType.BasicSpecialization,
                    PlayerActionType.Alpha, PlayerActionType.Alpha, PlayerActionType.MoveRed, null,
                    PlayerActionType.ChangeDeck, null, null, null, PlayerActionType.Charlie
                }), 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    null, PlayerActionType.MoveBlue, PlayerActionType.ChangeDeck,
                    PlayerActionType.Alpha, PlayerActionType.Alpha, PlayerActionType.MoveRed, PlayerActionType.MoveRed,
                    PlayerActionType.Alpha, PlayerActionType.MoveBlue, null, null, PlayerActionType.Charlie
                }), 1, PlayerColor.Green, PlayerSpecialization.PulseGunner),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    null, null, PlayerActionType.AdvancedSpecialization,
                    PlayerActionType.MoveRed, PlayerActionType.BasicSpecialization, null, PlayerActionType.Alpha,
                    PlayerActionType.Alpha, PlayerActionType.MoveBlue, PlayerActionType.ChangeDeck, null, PlayerActionType.Charlie
                }), 2, PlayerColor.Purple, PlayerSpecialization.Mechanic),
                new Player(PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
                {
                    PlayerActionType.Charlie, null, PlayerActionType.Alpha,
                    PlayerActionType.BasicSpecialization, PlayerActionType.Alpha, PlayerActionType.Alpha, PlayerActionType.Alpha,
                    PlayerActionType.Charlie, PlayerActionType.ChangeDeck, null, null, PlayerActionType.AdvancedSpecialization
                }), 3, PlayerColor.Red, PlayerSpecialization.DataAnalyst)
            };

            var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
            {
                {ZoneLocation.Blue, TrackConfiguration.Track6},
                {ZoneLocation.Red, TrackConfiguration.Track2},
                {ZoneLocation.White, TrackConfiguration.Track5},
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track3;

            var spacecraftCarrier = new SpacecraftCarrier();
            spacecraftCarrier.SetInitialPlacement(4, ZoneLocation.Blue);
            var manOfWar = new ManOfWar();
            manOfWar.SetInitialPlacement(5, ZoneLocation.White);
            var frigate = new Frigate();
            frigate.SetInitialPlacement(7, ZoneLocation.Red);
            var externalThreats = new ExternalThreat[] { spacecraftCarrier, manOfWar, frigate };

            var centralLaserJam = new CentralLaserJam();
            centralLaserJam.SetInitialPlacement(3);
            var internalThreats = new InternalThreat[] { centralLaserJam };

            var bonusThreats = new Threat[0];

            var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack, null);
            game.StartGame();
            for (var currentTurn = 0; currentTurn < game.NumberOfTurns + 1; currentTurn++)
                game.PerformTurn();
            Assert.AreEqual(GameStatus.Won, game.GameStatus);
            Assert.AreEqual(0, game.SittingDuck.BlueZone.TotalDamage);
            Assert.AreEqual(0, game.SittingDuck.RedZone.TotalDamage);
            Assert.AreEqual(0, game.SittingDuck.WhiteZone.TotalDamage);
            Assert.AreEqual(4, game.ThreatController.DefeatedThreats.Count());
            Assert.AreEqual(0, game.ThreatController.SurvivedThreats.Count());
            Assert.AreEqual(28 + 1 + 9, game.TotalPoints);
            Assert.AreEqual(0, players.Count(player => player.IsKnockedOut));
            Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(0).CurrentDamageTokens.Count);
            Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(1).CurrentDamageTokens.Count);
            Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(2).CurrentDamageTokens.Count);
        }

        [Test]
        public static void EzraDoubleActionsCampaignMission1()
        {
            var players = new List<Player>
            {
                CreateBluePlayerDoubleActionsCampaignMission1(),
                CreateGreenPlayerDoubleActionsCampaignMission1(),
                CreateYellowPlayerDoubleActionsCampaignMission1(),
                CreateRedPlayerDoubleActionsCampaignMission1()
            };

            var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
            {
                {ZoneLocation.Blue, TrackConfiguration.Track6},
                {ZoneLocation.White, TrackConfiguration.Track2},
                {ZoneLocation.Red, TrackConfiguration.Track5},
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track7;

            var armoredGrappler = new ArmoredGrappler();
            armoredGrappler.SetInitialPlacement(1, ZoneLocation.Blue);
            var meteoroid = new Meteoroid();
            meteoroid.SetInitialPlacement(4, ZoneLocation.Blue);
            var dimensionSpider = new DimensionSpider();
            dimensionSpider.SetInitialPlacement(2, ZoneLocation.White);
            var gunship = new Gunship();
            gunship.SetInitialPlacement(6, ZoneLocation.White);
            var asteroid = new Asteroid();
            asteroid.SetInitialPlacement(5, ZoneLocation.Red);
            var destroyer = new Destroyer();
            destroyer.SetInitialPlacement(8, ZoneLocation.Red);
            var externalThreats = new ExternalThreat[] { armoredGrappler, meteoroid, dimensionSpider, gunship, asteroid, destroyer };

            var shambler = new Shambler();
            shambler.SetInitialPlacement(4);
            var internalThreats = new InternalThreat[] { shambler };

            var bonusThreats = new Threat[0];

            var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack, null);
            game.StartGame();
            try
            {
                for (var currentTurn = 0; currentTurn < game.NumberOfTurns + 1; currentTurn++)
                    game.PerformTurn();
            }
            catch (LoseException loseException)
            {
                Assert.AreEqual(destroyer, loseException.Threat);
                Assert.AreEqual(GameStatus.Lost, game.GameStatus);
                Assert.AreEqual(0, game.SittingDuck.BlueZone.TotalDamage);
                Assert.AreEqual(9, game.SittingDuck.RedZone.TotalDamage);
                Assert.AreEqual(0, game.SittingDuck.WhiteZone.TotalDamage);
                Assert.AreEqual(6, game.ThreatController.DefeatedThreats.Count());
                Assert.AreEqual(0, game.ThreatController.SurvivedThreats.Count());
                Assert.AreEqual(0, players.Count(player => player.IsKnockedOut));
                Assert.AreEqual(0, game.SittingDuck.BlueZone.CurrentDamageTokens.Count);
                Assert.AreEqual(6, game.SittingDuck.RedZone.CurrentDamageTokens.Count);
                Assert.AreEqual(0, game.SittingDuck.WhiteZone.CurrentDamageTokens.Count);
            }
        }

        private static Player CreateGreenPlayerDoubleActionsCampaignMission1()
        {
            var greenActions = new[]
            {
                new PlayerAction(PlayerActionType.MoveBlue, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Charlie, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.Alpha, null),
                new PlayerAction(null, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.MoveRed, PlayerActionType.ChangeDeck, null),
                new PlayerAction(null, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Charlie, null, null)
            };
            var greenPlayer = new Player(greenActions, 1, PlayerColor.Green);
            return greenPlayer;
        }

        private static Player CreateRedPlayerDoubleActionsCampaignMission1()
        {
            var redActions = new[]
            {
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Alpha, PlayerActionType.Charlie, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.Charlie, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.MoveRed, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.ChangeDeck, null),
                new PlayerAction(PlayerActionType.Charlie, null, null)
            };
            var redPlayer = new Player(redActions, 3, PlayerColor.Red);
            return redPlayer;
        }

        private static Player CreateYellowPlayerDoubleActionsCampaignMission1()
        {
            var yellowActions = new[]
            {
                new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.ChangeDeck, null),
                new PlayerAction(PlayerActionType.BasicSpecialization, null, null),
                new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.MoveRed, PlayerActionType.MoveRed, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Bravo, null, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.Charlie, null)
            };
            var yellowPlayer = new Player(yellowActions, 2, PlayerColor.Yellow, PlayerSpecialization.SpecialOps);
            return yellowPlayer;
        }

        private static Player CreateBluePlayerDoubleActionsCampaignMission1()
        {
            var blueActions = new[]
            {
                new PlayerAction(PlayerActionType.MoveRed, PlayerActionType.ChangeDeck, null),
                new PlayerAction(PlayerActionType.Charlie, PlayerActionType.MoveBlue, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.BattleBots, PlayerActionType.Bravo, null),
                new PlayerAction(PlayerActionType.BattleBots, PlayerActionType.Bravo, null),
                new PlayerAction(PlayerActionType.ChangeDeck, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.Charlie, null, null),
                new PlayerAction(PlayerActionType.ChangeDeck, null, null),
                new PlayerAction(PlayerActionType.Charlie, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(null, null, null)
            };
            var bluePlayer = new Player(blueActions, 0, PlayerColor.Blue);
            return bluePlayer;
        }

        [Test]
        public static void EzraDoubleActionsCampaignMission2()
        {
            var players = new List<Player>
            {
                CreateBluePlayerDoubleActionsCampaignMission2(),
                CreateGreenPlayerDoubleActionsCampaignMission2(),
                CreateYellowPlayerDoubleActionsCampaignMission2(),
                CreateRedPlayerDoubleActionsCampaignMission2()
            };

            var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
            {
                {ZoneLocation.Blue, TrackConfiguration.Track7},
                {ZoneLocation.White, TrackConfiguration.Track2},
                {ZoneLocation.Red, TrackConfiguration.Track5}
            };
            const TrackConfiguration internalTrack = TrackConfiguration.Track4;

            var interstellarOctopus = new InterstellarOctopus();
            interstellarOctopus.SetInitialPlacement(3, ZoneLocation.Blue);
            var meteoroid = new Meteoroid();
            meteoroid.SetInitialPlacement(8, ZoneLocation.Blue);
            var amoeba = new Amoeba();
            amoeba.SetInitialPlacement(2, ZoneLocation.White);
            var stealthFighter = new StealthFighter();
            stealthFighter.SetInitialPlacement(4, ZoneLocation.White);
            var spinningSaucer = new SpinningSaucer();
            spinningSaucer.SetInitialPlacement(7, ZoneLocation.White);
            var armoredGrappler = new ArmoredGrappler();
            armoredGrappler.SetInitialPlacement(1, ZoneLocation.Red);
            var externalThreats = new ExternalThreat[] { interstellarOctopus, meteoroid, amoeba, stealthFighter, spinningSaucer, armoredGrappler };

            var commandosB = new CommandosB();
            commandosB.SetInitialPlacement(5);
            var hackedShieldsB = new HackedShieldsB();
            hackedShieldsB.SetInitialPlacement(8);
            var internalThreats = new InternalThreat[] { commandosB, hackedShieldsB };

            var bonusThreats = new Threat[0];

            var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack, null);
            game.StartGame();
            for (var currentTurn = 0; currentTurn < game.NumberOfTurns + 1; currentTurn++)
                game.PerformTurn();
            Assert.AreEqual(GameStatus.Won, game.GameStatus);
            Assert.AreEqual(2, game.SittingDuck.BlueZone.TotalDamage);
            Assert.AreEqual(0, game.SittingDuck.RedZone.TotalDamage);
            Assert.AreEqual(5, game.SittingDuck.WhiteZone.TotalDamage);
            Assert.AreEqual(6, game.ThreatController.DefeatedThreats.Count());
            Assert.AreEqual(2, game.ThreatController.SurvivedThreats.Count());
            Assert.AreEqual(0, players.Count(player => player.IsKnockedOut));
            Assert.AreEqual(2, game.SittingDuck.BlueZone.CurrentDamageTokens.Count);
            Assert.AreEqual(0, game.SittingDuck.RedZone.CurrentDamageTokens.Count);
            Assert.AreEqual(5, game.SittingDuck.WhiteZone.CurrentDamageTokens.Count);
        }

        private static Player CreateGreenPlayerDoubleActionsCampaignMission2()
        {
            var greenActions = new[]
            {
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Charlie, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Alpha, PlayerActionType.Charlie, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.BasicSpecialization, null, null),
                new PlayerAction(PlayerActionType.ChangeDeck, PlayerActionType.Bravo, null),
                new PlayerAction(PlayerActionType.ChangeDeck, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.ChangeDeck, null, null),
                new PlayerAction(PlayerActionType.Charlie, null, null)
            };
            var greenPlayer = new Player(greenActions, 1, PlayerColor.Green, PlayerSpecialization.Mechanic);
            return greenPlayer;
        }

        private static Player CreateRedPlayerDoubleActionsCampaignMission2()
        {
            var redActions = new[]
            {
                new PlayerAction(PlayerActionType.MoveRed, PlayerActionType.ChangeDeck, null),
                new PlayerAction(PlayerActionType.Bravo, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.MoveBlue, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Bravo, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.BattleBots, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.BasicSpecialization, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.MoveRed, null, null),
                new PlayerAction(PlayerActionType.AdvancedSpecialization, null, null)
            };
            var redPlayer = new Player(redActions, 3, PlayerColor.Red, PlayerSpecialization.DataAnalyst);
            return redPlayer;
        }

        private static Player CreateYellowPlayerDoubleActionsCampaignMission2()
        {
            var yellowActions = new[]
            {
                new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.Charlie, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.BattleBots, PlayerActionType.Charlie, null),
                new PlayerAction(PlayerActionType.BattleBots, PlayerActionType.Charlie, null),
                new PlayerAction(PlayerActionType.BasicSpecialization, null, null),
                new PlayerAction(PlayerActionType.BattleBots, PlayerActionType.Bravo, null),
                new PlayerAction(PlayerActionType.BattleBots, PlayerActionType.Bravo, null),
                new PlayerAction(PlayerActionType.MoveRed, PlayerActionType.ChangeDeck, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Charlie, null, null)
            };
            var yellowPlayer = new Player(yellowActions, 2, PlayerColor.Yellow, PlayerSpecialization.SpecialOps);
            return yellowPlayer;
        }

        private static Player CreateBluePlayerDoubleActionsCampaignMission2()
        {
            var blueActions = new[]
            {
                new PlayerAction(PlayerActionType.MoveRed, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.Alpha, null, null),
                new PlayerAction(PlayerActionType.BasicSpecialization, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.BasicSpecialization, null, null),
                new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.MoveBlue, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.MoveBlue, PlayerActionType.Alpha, null),
                new PlayerAction(PlayerActionType.MoveRed, PlayerActionType.ChangeDeck, null),
                new PlayerAction(null, null, null),
                new PlayerAction(null, null, null),
                new PlayerAction(PlayerActionType.Charlie, null, null)
            };
            var bluePlayer = new Player(blueActions, 0, PlayerColor.Blue, PlayerSpecialization.EnergyTechnician);
            return bluePlayer;
        }
    }
}
