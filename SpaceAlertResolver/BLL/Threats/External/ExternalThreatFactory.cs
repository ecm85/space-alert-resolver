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
		public static Dictionary<Type, string> ThreatNamesByType { get; } = new Dictionary<Type, string>
		{
			{typeof (EnergySnake), "Energy Snake"},
			{typeof (JumperB), "Jumper"},
			{typeof (JumperA), "Jumper"},
			{typeof (MegashieldFighter), "Megashield Fighter"},
			{typeof (PhasingPulser), "Phasing Pulser"},
			{typeof (PolarizedFighter), "Polarized Fighter"},
			{typeof (PhasingDestroyer), "Phasing Destroyer"},
			{typeof (PlasmaticNeedleship), "Plasmatic Needleship"},
			{typeof (SealedCapsule), "Sealed Capsule"},
			{typeof (Amoeba), "Amoeba"},
			{typeof (Gunship), "Gunship"},
			{typeof (Jellyfish), "Jellyfish"},
			{typeof (Kamikaze), "Kamikaze"},
			{typeof (Marauder), "Marauder"},
			{typeof (MegashieldDestroyer), "Megashield Destroyer"},
			{typeof (Meteoroid), "Meteoroid"},
			{typeof (MinorAsteroid), "Minor Asteroid"},
			{typeof (PhasingFighter), "Phasing Fighter"},
			{typeof (EnergyDragon), "Energy Dragon"},
			{typeof (Leaper), "Leaper"},
			{typeof (MegashieldManOfWar), "Megashield Man-Of-War"},
			{typeof (Overlord), "Overlord"},
			{typeof (Planetoid), "Planetoid"},
			{typeof (PolarizedFrigate), "Polarized Frigate"},
			{typeof (SuperCarrier), "Super-Carrier"},
			{typeof (TransmitterSatellite), "Transmitter Satellite"},
			{typeof (Asteroid), "Asteroid"},
			{typeof (CryoshieldFighter), "Cryoshield Fighter"},
			{typeof (CryoshieldFrigate), "Cryoshield Frigate"},
			{typeof (Destroyer), "Destroyer"},
			{typeof (DimensionSpider), "Dimension Spider"},
			{typeof (EnergyCloud), "Energy Cloud"},
			{typeof (Frigate), "Frigate"},
			{typeof (InterstellarOctopus), "Interstellar Octopus"},
			{typeof (LeviathanTanker), "Leviathan Tanker"},
			{typeof (Maelstrom), "Maelstrom"},
			{typeof (ManOfWar), "Man-Of-War"},
			{typeof (MiniCarrier), "Mini-Carrier"},
			{typeof (PhantomFighter), "Phantom Fighter"},
			{typeof (PlasmaticFighter), "Plasmatic Fighter"},
			{typeof (PulseBall), "Pulse Ball"},
			{typeof (PulseSatellite), "Pulse Satellite"},
			{typeof (Scout), "Scout"},
			{typeof (Behemoth), "Behemoth"},
			{typeof (Juggernaut), "Juggernaut"},
			{typeof (MajorAsteroid), "Major Asteroid"},
			{typeof (MotherSwarm), "Mother Swarm"},
			{typeof (NebulaCrab), "Nebula Crab"},
			{typeof (Nemesis), "Nemesis"},
			{typeof (PhasingManOfWar), "Phasing Man-Of-War"},
			{typeof (PhasingFrigate), "Phasing Frigate"},
			{typeof (PlasmaticFrigate), "Plasmatic Frigate"},
			{typeof (PsionicSatellite), "Psionic Satellite"},
			{typeof (SpacecraftCarrier), "Spacecraft Carrier"},
			{typeof (SpinningSaucer), "Spinning Saucer"},
			{typeof (StealthFighter), "Stealth Fighter"},
			{typeof (Swarm), "Swarm"},
			{typeof (ArmoredGrappler), "Armored Grappler"},
			{typeof (Fighter), "Fighter"}
		};

		public static IDictionary<string, Type> ThreatTypesById { get; } = new Dictionary<string, Type>
		{
			{"E3-109", typeof(EnergySnake)},
			{"E3-106", typeof(JumperB)},
			{"E3-105", typeof(JumperA)},
			{"E3-104", typeof(MegashieldFighter)},
			{"E3-102", typeof(PhasingPulser)},
			{"E3-108", typeof(PolarizedFighter)},
			{"E3-103", typeof(PhasingDestroyer)},
			{"E3-101", typeof(PlasmaticNeedleship)},
			{"E3-107", typeof(SealedCapsule)},
			{"E1-09", typeof(Amoeba)},
			{"E1-05", typeof(Gunship)},
			{"E2-05", typeof(Jellyfish)},
			{"E2-01", typeof(Kamikaze)},
			{"E2-06", typeof(Marauder)},
			{"E2-103", typeof(MegashieldDestroyer)},
			{"E1-10", typeof(Meteoroid)},
			{"E2-07", typeof(MinorAsteroid)},
			{"E2-102", typeof(PhasingFighter)},
			{"SE3-108", typeof(EnergyDragon)},
			{"SE3-106", typeof(Leaper)},
			{"SE3-102", typeof(MegashieldManOfWar)},
			{"SE3-105", typeof(Overlord)},
			{"SE3-107", typeof(Planetoid)},
			{"SE3-109", typeof(PolarizedFrigate)},
			{"SE3-103", typeof(SuperCarrier)},
			{"SE3-104", typeof(TransmitterSatellite)},
			{"SE1-08", typeof(Asteroid)},
			{"E1-06", typeof(CryoshieldFighter)},
			{"SE1-05", typeof(CryoshieldFrigate)},
			{"E1-02", typeof(Destroyer)},
			{"SE1-102", typeof(DimensionSpider)},
			{"E1-04", typeof(EnergyCloud)},
			{"SE1-01", typeof(Frigate)},
			{"SE1-06", typeof(InterstellarOctopus)},
			{"SE1-03", typeof(LeviathanTanker)},
			{"SE1-07", typeof(Maelstrom)},
			{"SE1-02", typeof(ManOfWar)},
			{"E2-101", typeof(MiniCarrier)},
			{"E2-03", typeof(PhantomFighter)},
			{"E1-101", typeof(PlasmaticFighter)},
			{"E1-01", typeof(PulseBall)},
			{"SE1-04", typeof(PulseSatellite)},
			{"E2-02", typeof(Scout)},
			{"SE2-01", typeof(Behemoth)},
			{"SE2-02", typeof(Juggernaut)},
			{"SE2-06", typeof(MajorAsteroid)},
			{"SE2-103", typeof(MotherSwarm)},
			{"SE2-04", typeof(NebulaCrab)},
			{"SE2-05", typeof(Nemesis)},
			{"SE3-101", typeof(PhasingManOfWar)},
			{"SE2-102", typeof(PhasingFrigate)},
			{"SE2-101", typeof(PlasmaticFrigate)},
			{"SE2-03", typeof(PsionicSatellite)},
			{"SE1-101", typeof(SpacecraftCarrier)},
			{"E1-102", typeof(SpinningSaucer)},
			{"E1-03", typeof(StealthFighter)},
			{"E2-04", typeof(Swarm)},
			{"E1-08", typeof(ArmoredGrappler)},
			{"E1-07", typeof(Fighter)}
		};

		public static IDictionary<Type, string> ThreatIdsByType { get; } = ThreatTypesById.ToDictionary(typeById => typeById.Value, typeById => typeById.Key);

		public static T CreateThreat<T>(string id) where T: ExternalThreat
		{
			if (!ThreatTypesById.ContainsKey(id))
				return null;
			return Activator.CreateInstance(ThreatTypesById[id]) as T;
		}
	}
}
