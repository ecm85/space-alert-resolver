using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Ninja : MinorYellowInternalThreat
	{
		public Ninja()
			: base(3, 2, StationLocation.LowerBlue, PlayerAction.BattleBots)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			//TODO: Send drones into adjacent stations
			//Until ninja is destroyed or performs Z, anyone starting or ending is those stations is poisoned
			throw new NotImplementedException();
		}

		protected override void PerformYAction(int currentTurn)
		{
			if(!IsDefeated)
				SittingDuck.DrainReactors(CurrentZones, 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPoisonedPlayers(EnumFactory.All<StationLocation>());

			if (!IsDefeated)
			{
				var removedRocketCount = SittingDuck.GetRocketCount();
				SittingDuck.RemoveAllRockets();
				for (var i = 0; i < removedRocketCount; i++)
					SittingDuck.TakeAttack(new ThreatDamage(2, ThreatDamageType.Standard, new[] {ZoneLocation.Red}));
			}
		}

		protected override void OnHealthReducedToZero()
		{
			var anyPlayersPoisoned = SittingDuck.GetPoisonedPlayerCount(EnumFactory.All<StationLocation>()) == 0;
			if (anyPlayersPoisoned)
			{
				IsDefeated = true;
				SittingDuck.RemoveInternalThreatFromStations(CurrentStations, this);
				CurrentStations.Clear();
				ThreatController.EndOfTurn -= PerformEndOfTurn;
			}
			else
				base.OnHealthReducedToZero();
		}

		protected override void OnReachingEndOfTrack()
		{
			if (IsDefeated)
			{
				Position = null;
				ThreatController.ThreatsMove -= PerformMove;
			}
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
