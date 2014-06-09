using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Ninja : MinorYellowInternalThreat
	{
		public Ninja()
			: base(3, 2, StationLocation.LowerBlue, PlayerAction.BattleBots)
		{
		}

		public override void PerformXAction()
		{
			//TODO: Send drones into adjacent stations
			//Until ninja is destroyed or performs Z, anyone starting or ending is those stations is poisoned
			throw new NotImplementedException();
		}

		public override void PerformYAction()
		{
			SittingDuck.DrainReactors(CurrentZones, 1);
		}

		public override void PerformZAction()
		{
			SittingDuck.KnockOutPoisonedPlayers(EnumFactory.All<StationLocation>());

			if (RemainingHealth != 0)
			{
				var removedRocketCount = SittingDuck.RemoveAllRockets();
				for (var i = 0; i < removedRocketCount; i++)
					SittingDuck.TakeAttack(new ThreatDamage(2, ThreatDamageType.Standard, new[] {ZoneLocation.Red}));
			}
			else
				isDefeated = true;
			
		}

		protected override void OnHealthReducedToZero()
		{
			var anyPlayersPoisoned = SittingDuck.GetPoisonedPlayerCount(EnumFactory.All<StationLocation>()) == 0;
			base.OnHealthReducedToZero(!anyPlayersPoisoned);
		}

		public override void OnReachingEndOfTrack()
		{
			if (isDefeated)
				Position = null;
			else
				base.OnReachingEndOfTrack();
			//TODO: Remove drones
		}

		public static string GetDisplayName()
		{
			return "Ninja";
		}
	}
}
