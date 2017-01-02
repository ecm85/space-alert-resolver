using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Threats.Internal.Minor.Red;
using BLL.Threats.Internal.Minor.White;
using BLL.Threats.Internal.Minor.Yellow;
using BLL.Threats.Internal.Serious.Red;
using BLL.Threats.Internal.Serious.White;
using BLL.Threats.Internal.Serious.Yellow;

namespace BLL.Threats.Internal
{
	public static class ThreatFactory
	{
		//private static readonly Dictionary<Type, string> ThreatTypesByDisplayName = new Dictionary<Type, string>
		//{
		//	{typeof (EnergySnake), "Energy Snake"},
		//	{typeof (JumperB), "Jumper E3-106"},
		//	{typeof (JumperA), "Jumper E3-105"},
		//	{typeof (MegashieldFighter), "Megashield Fighter"},
		//	{typeof (PhasingPulser), "Phasing Pulser"},
		//	{typeof (PolarizedFighter), "Polarized Fighter"},
		//	{typeof (PhasingDestroyer), "Phasing Destroyer"},
		//	{typeof (PlasmaticNeedleship), "Plasmatic Needleship"},
		//	{typeof (SealedCapsule), "Sealed Capsule"},
		//	{typeof (Amoeba), "Amoeba"},
		//	{typeof (Gunship), "Gunship"},
		//	{typeof (Jellyfish), "Jellyfish"},
		//	{typeof (Kamikaze), "Kamikaze"},
		//	{typeof (Marauder), "Marauder"},
		//	{typeof (MegashieldDestroyer), "Megashield Destroyer"},
		//	{typeof (Meteoroid), "Meteoroid"},
		//	{typeof (MinorAsteroid), "Minor Asteroid"},
		//	{typeof (PhasingFighter), "Phasing Fighter"},
		//	{typeof (EnergyDragon), "Energy Dragon"},
		//	{typeof (Leaper), "Leaper"},
		//	{typeof (MegashieldManOfWar), "Megashield Man-Of-War"},
		//	{typeof (Overlord), "Overlord"},
		//	{typeof (Planetoid), "Planetoid"},
		//	{typeof (PolarizedFrigate), "Polarized Frigate"},
		//	{typeof (SuperCarrier), "Super-Carrier"},
		//	{typeof (TransmitterSatellite), "Transmitter Satellite"},
		//	{typeof (Asteroid), "Asteroid"},
		//	{typeof (CryoshieldFighter), "Cryoshield Fighter"},
		//	{typeof (CryoshieldFrigate), "Cryoshield Frigate"},
		//	{typeof (Destroyer), "Destroyer"},
		//	{typeof (DimensionSpider), "Dimension Spider"},
		//	{typeof (EnergyCloud), "Energy Cloud"},
		//	{typeof (Frigate), "Frigate"},
		//	{typeof (InterstellarOctopus), "Interstellar Octopus"},
		//	{typeof (LeviathanTanker), "Leviathan Tanker"},
		//	{typeof (Maelstrom), "Maelstrom"},
		//	{typeof (ManOfWar), "Man-Of-War"},
		//	{typeof (MiniCarrier), "Mini-Carrier"},
		//	{typeof (PhantomFighter), "Phantom Fighter"},
		//	{typeof (PlasmaticFighter), "Plasmatic Fighter"},
		//	{typeof (PulseBall), "Pulse Ball"},
		//	{typeof (PulseSatellite), "Pulse Satellite"},
		//	{typeof (Scout), "Scout"},
		//	{typeof (Behemoth), "Behemoth"},
		//	{typeof (Juggernaut), "Juggernaut"},
		//	{typeof (MajorAsteroid), "Major Asteroid"},
		//	{typeof (MotherSwarm), "Mother Swarm"},
		//	{typeof (NebulaCrab), "Nebula Crab"},
		//	{typeof (Nemesis), "Nemesis"},
		//	{typeof (PhasingManOfWar), "Phasing Man-Of-War"},
		//	{typeof (PhasingFrigate), "Phasing Frigate"},
		//	{typeof (PlasmaticFrigate), "Plasmatic Frigate"},
		//	{typeof (PsionicSatellite), "Psionic Satellite"},
		//	{typeof (SpacecraftCarrier), "Spacecraft Carrier"},
		//	{typeof (SpinningSaucer), "Spinning Saucer"},
		//	{typeof (StealthFighter), "Stealth Fighter"},
		//	{typeof (Swarm), "Swarm"},
		//	{typeof (BreachedAirlock), "Breached Airlock"},
		//	{typeof (Driller), "Driller"},
		//	{typeof (LateralLaserJam), "Lateral Laser Jam"},
		//	{typeof (PhasingTroopersB), "Phasing Troopers I3-105"},
		//	{typeof (PhasingTroopersA), "Phasing Troopers I3-106"},
		//	{typeof (PulseCannonShortCircuit), "Pulse Cannon Short Circuit"},
		//	{typeof (ReversedShields), "Reversed Shields"},
		//	{typeof (Ninja), "Ninja"},
		//	{typeof (OverheatedReactor), "Overheated Reactor"},
		//	{typeof (PowerPackOverload), "Power Pack Overload"},
		//	{typeof (SlimeA), "Slime I2-01"},
		//	{typeof (SlimeB), "Slime I2-02"},
		//	{typeof (TroopersA), "Troopers I2-04"},
		//	{typeof (TroopersB), "Troopers I2-03"},
		//	{typeof (Virus), "Virus"},
		//	{typeof (CyberGremlin), "Cyber Gremlin"},
		//	{typeof (HiddenTransmitterB), "Hidden Transmitter SI3-102"},
		//	{typeof (HiddenTransmitterA), "Hidden Transmitter SI3-103"},
		//	{typeof (Parasite), "Cyber Gremlin"},
		//	{typeof (RabidBeast), "Rabid Beast"},
		//	{typeof (Siren), "Siren"},
		//	{typeof (SpaceTimeVortex), "Space Time Vortex"},
		//	{typeof (Alien), "Alien"},
		//	{typeof (BattleBotUprising), "BattleBot Uprising"},
		//	{typeof (CentralLaserJam), "Central Laser Jam"},
		//	{typeof (CommandosB), "Commandos SI1-02"},
		//	{typeof (CommandosA), "Commandos SI1-01"},
		//	{typeof (CrossedWires), "Crossed Wires"},
		//	{typeof (Fissure), "Fissure"},
		//	{typeof (HackedShieldsA), "Hacked Shields I1-06"},
		//	{typeof (HackedShieldsB), "Hacked Shields I1-05"},
		//	{typeof (SaboteurA), "Saboteur I1-04"},
		//	{typeof (SaboteurB), "Saboteur I1-03"},
		//	{typeof (Contamination), "Contamination"},
		//	{typeof (Executioner), "Executioner"},
		//	{typeof (NuclearDevice), "Nuclear Device"},
		//	{typeof (PhasingAnomaly), "Phasing Anomaly"},
		//	{typeof (PhasingMineLayer), "Phasing Mine Layer"},
		//	{typeof (PowerSystemOverload), "Power System Overload"},
		//	{typeof (Seeker), "Seeker"},
		//	{typeof (Shambler), "Shambler"},
		//	{typeof (SkirmishersA), "Skirmishers I1-01"},
		//	{typeof (SkirmishersB), "Skirmishers I1-02"},
		//	{typeof (UnstableWarheads), "Unstable Warheads"},
		//	{typeof (ArmoredGrappler), "Armored Grappler"},
		//	{typeof (Fighter), "Fighter"}
		//};

		public static IDictionary<string, Type> ThreatTypesById { get; } = new Dictionary<string, Type>
		{
			{"I3-104", typeof(BreachedAirlock)},
			{"I3-107", typeof(Driller)},
			{"I3-101", typeof(LateralLaserJam)},
			{"I3-105", typeof(PhasingTroopersB)},
			{"I3-106", typeof(PhasingTroopersA)},
			{"I3-102", typeof(PulseCannonShortCircuit)},
			{"I3-103", typeof(ReversedShields)},
			{"I2-102", typeof(Ninja)},
			{"I2-06", typeof(OverheatedReactor)},
			{"I2-101", typeof(PowerPackOverload)},
			{"I2-01", typeof(SlimeA)},
			{"I2-02", typeof(SlimeB)},
			{"I2-04", typeof(TroopersA)},
			{"I2-03", typeof(TroopersB)},
			{"I2-05", typeof(Virus)},
			{"SI3-106", typeof(CyberGremlin)},
			{"SI3-102", typeof(HiddenTransmitterB)},
			{"SI3-103", typeof(HiddenTransmitterA)},
			{"SI3-107", typeof(Parasite)},
			{"SI3-101", typeof(RabidBeast)},
			{"SI3-105", typeof(Siren)},
			{"SI3-104", typeof(SpaceTimeVortex)},
			{"SI1-03", typeof(Alien)},
			{"SI1-06", typeof(BattleBotUprising)},
			{"I1-101", typeof(CentralLaserJam)},
			{"SI1-02", typeof(CommandosB)},
			{"SI1-01", typeof(CommandosA)},
			{"SI1-05", typeof(CrossedWires)},
			{"SI1-04", typeof(Fissure)},
			{"I1-06", typeof(HackedShieldsA)},
			{"I1-05", typeof(HackedShieldsB)},
			{"I1-04", typeof(SaboteurA)},
			{"I1-03", typeof(SaboteurB)},
			{"SI2-04", typeof(Contamination)},
			{"SI2-01", typeof(Executioner)},
			{"SI2-05", typeof(NuclearDevice)},
			{"SI2-101", typeof(PhasingAnomaly)},
			{"SI2-102", typeof(PhasingMineLayer)},
			{"SI2-03", typeof(PowerSystemOverload)},
			{"SI2-02", typeof(Seeker)},
			{"SI1-101", typeof(Shambler)},
			{"I1-01", typeof(SkirmishersA)},
			{"I1-02", typeof(SkirmishersB)},
			{"I1-07", typeof(UnstableWarheads)}
		};

		public static IDictionary<Type, string> ThreatIdsByType
		{
			get { return ThreatTypesById.ToDictionary(typeById => typeById.Value, typeById => typeById.Key); }
		}

		public static T CreateThreat<T>(string id) where T : InternalThreat
		{
			if (!ThreatTypesById.ContainsKey(id))
				return null;
			return Activator.CreateInstance(ThreatTypesById[id]) as T;
		}
	}
}
