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
	public static class InternalThreatFactory
	{
		public static Dictionary<Type, string> ThreatNamesByType { get; } = new Dictionary<Type, string>
		{
			{typeof (BreachedAirlock), "Breached Airlock"},
			{typeof (Driller), "Driller"},
			{typeof (LateralLaserJam), "Lateral Laser Jam"},
			{typeof (PhasingTroopersB), "Phasing Troopers"},
			{typeof (PhasingTroopersA), "Phasing Troopers"},
			{typeof (PulseCannonShortCircuit), "Pulse Cannon Short Circuit"},
			{typeof (ReversedShields), "Reversed Shields"},
			{typeof (Ninja), "Ninja"},
			{typeof (OverheatedReactor), "Overheated Reactor"},
			{typeof (PowerPackOverload), "Power Pack Overload"},
			{typeof (SlimeA), "Slime"},
			{typeof (SlimeB), "Slime"},
			{typeof (TroopersA), "Troopers"},
			{typeof (TroopersB), "Troopers"},
			{typeof (Virus), "Virus"},
			{typeof (CyberGremlin), "Cyber Gremlin"},
			{typeof (HiddenTransmitterB), "Hidden Transmitter"},
			{typeof (HiddenTransmitterA), "Hidden Transmitter"},
			{typeof (Parasite), "Cyber Gremlin"},
			{typeof (RabidBeast), "Rabid Beast"},
			{typeof (Siren), "Siren"},
			{typeof (SpaceTimeVortex), "Space Time Vortex"},
			{typeof (Alien), "Alien"},
			{typeof (BattleBotUprising), "BattleBot Uprising"},
			{typeof (CentralLaserJam), "Central Laser Jam"},
			{typeof (CommandosB), "Commandos"},
			{typeof (CommandosA), "Commandos"},
			{typeof (CrossedWires), "Crossed Wires"},
			{typeof (Fissure), "Fissure"},
			{typeof (HackedShieldsA), "Hacked Shields"},
			{typeof (HackedShieldsB), "Hacked Shields"},
			{typeof (SaboteurA), "Saboteur"},
			{typeof (SaboteurB), "Saboteur"},
			{typeof (Contamination), "Contamination"},
			{typeof (Executioner), "Executioner"},
			{typeof (NuclearDevice), "Nuclear Device"},
			{typeof (PhasingAnomaly), "Phasing Anomaly"},
			{typeof (PhasingMineLayer), "Phasing Mine Layer"},
			{typeof (PowerSystemOverload), "Power System Overload"},
			{typeof (Seeker), "Seeker"},
			{typeof (Shambler), "Shambler"},
			{typeof (SkirmishersA), "Skirmishers"},
			{typeof (SkirmishersB), "Skirmishers"},
			{typeof (UnstableWarheads), "Unstable Warheads"}
		};

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
