using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Threats.Internal.Minor.Red;
using BLL.Threats.Internal.Minor.White;
using BLL.Threats.Internal.Minor.Yellow;
using BLL.Threats.Internal.Minor.Yellow.Slime;
using BLL.Threats.Internal.Serious.Red;
using BLL.Threats.Internal.Serious.White;
using BLL.Threats.Internal.Serious.Yellow;

namespace BLL.Threats.Internal
{
	public static class InternalThreatFactory
	{
		public static IEnumerable<InternalThreat> AllInternalThreats { get; } = new InternalThreat[]
		{
			new BreachedAirlock(),
			new Driller(),
			new LateralLaserJam(),
			new PhasingTroopersB(),
			new PhasingTroopersA(),
			new PulseCannonShortCircuit(),
			new ReversedShields(),
			new Ninja(),
			new OverheatedReactor(),
			new PowerPackOverload(),
			new SlimeA(),
			new SlimeB(),
			new TroopersA(),
			new TroopersB(),
			new Virus(),
			new CyberGremlin(),
			new HiddenTransmitterB(),
			new HiddenTransmitterA(),
			new Parasite(),
			new RabidBeast(),
			new Siren(),
			new SpaceTimeVortex(),
			new Alien(),
			new BattleBotUprising(),
			new CentralLaserJam(),
			new CommandosB(),
			new CommandosA(),
			new CrossedWires(),
			new Fissure(),
			new HackedShieldsA(),
			new HackedShieldsB(),
			new SaboteurA(),
			new SaboteurB(),
			new Contamination(),
			new Executioner(),
			new NuclearDevice(),
			new PhasingAnomaly(),
			new PhasingMineLayer(),
			new PowerSystemOverload(),
			new Seeker(),
			new Shambler(),
			new SkirmishersA(),
			new SkirmishersB(),
			new UnstableWarheads()
		};

		private static IDictionary<string, Type> ThreatTypesById { get; } = AllInternalThreats.ToDictionary(threat => threat.Id, threat => threat.GetType());

		public static T CreateThreat<T>(string id) where T : InternalThreat
		{
			if (!ThreatTypesById.ContainsKey(id))
				return null;
			return Activator.CreateInstance(ThreatTypesById[id]) as T;
		}
	}
}
