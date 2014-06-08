using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Ninja : MinorYellowInternalThreat
	{
		public Ninja(int timeAppears, ISittingDuck sittingDuck)
			: base(3, 2, timeAppears, StationLocation.LowerBlue, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			//TODO: Send drones into adjacent stations
			//Until ninja is destroyed or performs Z, anyone starting or ending is those stations is poisoned
			throw new NotImplementedException();
		}

		public override void PerformYAction()
		{
			sittingDuck.StationByLocation[CurrentStation].EnergyContainer.Energy--;
		}

		public override void PerformZAction()
		{
			if (RemainingHealth != 0)
			{
				var rockets = sittingDuck.RocketsComponent.Rockets;
				foreach (var rocket in rockets)
					sittingDuck.TakeAttack(new ThreatDamage(2, ThreatDamageType.Standard, new[] {ZoneLocation.Red}));
				rockets.Clear();
			}
			else
				OnDestroyed();
			sittingDuck.KnockOutPoisonedPlayers(EnumFactory.All<StationLocation>());
			//TODO: Remove drones
		}

		public override void CheckForDestroyed()
		{
			if (RemainingHealth > 0)
				return;
			if (sittingDuck.GetPoisonedPlayerCount(EnumFactory.All<StationLocation>()) == 0)
				OnDestroyed();
			else
				RemoveFromStation(CurrentStation);
		}

		protected override void OnDestroyed()
		{
			//TODO: Remove drones
			base.OnDestroyed();
		}

		public static string GetDisplayName()
		{
			return "Ninja";
		}
	}
}
