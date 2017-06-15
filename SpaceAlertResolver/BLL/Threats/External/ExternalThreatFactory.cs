using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Threats.External.Minor.Red;
using BLL.Threats.External.Minor.White;
using BLL.Threats.External.Minor.Yellow;
using BLL.Threats.External.Serious.Red;
using BLL.Threats.External.Serious.White;
using BLL.Threats.External.Serious.Yellow;

namespace BLL.Threats.External
{
    public static class ExternalThreatFactory
    {
        public static IEnumerable<ExternalThreat> AllExternalThreats { get; } = new ExternalThreat[]
        {
            new EnergySnake(),
            new JumperB(),
            new JumperA(),
            new MegashieldFighter(),
            new PhasingPulser(),
            new PolarizedFighter(),
            new PhasingDestroyer(),
            new PlasmaticNeedleship(),
            new SealedCapsule(),
            new Amoeba(),
            new Gunship(),
            new Jellyfish(),
            new Kamikaze(),
            new Marauder(),
            new MegashieldDestroyer(),
            new Meteoroid(),
            new MinorAsteroid(),
            new PhasingFighter(),
            new EnergyDragon(),
            new Leaper(),
            new MegashieldManOfWar(),
            new Overlord(),
            new Planetoid(),
            new PolarizedFrigate(),
            new SuperCarrier(),
            new TransmitterSatellite(),
            new Asteroid(),
            new CryoshieldFighter(),
            new CryoshieldFrigate(),
            new Destroyer(),
            new DimensionSpider(),
            new EnergyCloud(),
            new Frigate(),
            new InterstellarOctopus(),
            new LeviathanTanker(),
            new Maelstrom(),
            new ManOfWar(),
            new MiniCarrier(),
            new PhantomFighter(),
            new PlasmaticFighter(),
            new PulseBall(),
            new PulseSatellite(),
            new Scout(),
            new Behemoth(),
            new Juggernaut(),
            new MajorAsteroid(),
            new MotherSwarm(),
            new NebulaCrab(),
            new Nemesis(),
            new PhasingManOfWar(),
            new PhasingFrigate(),
            new PlasmaticFrigate(),
            new PsionicSatellite(),
            new SpacecraftCarrier(),
            new SpinningSaucer(),
            new StealthFighter(),
            new Swarm(),
            new ArmoredGrappler(),
            new Fighter()
        };

        private static IDictionary<string, Type> ThreatTypesById { get; } = AllExternalThreats.ToDictionary(threat => threat.Id, threat => threat.GetType());

        public static T CreateThreat<T>(string id) where T: ExternalThreat
        {
            if (!ThreatTypesById.ContainsKey(id))
                return null;
            return Activator.CreateInstance(ThreatTypesById[id], true) as T;
        }
    }
}
