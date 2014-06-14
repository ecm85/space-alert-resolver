using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public class UnstableWarheads : MinorWhiteInternalThreat
	{
		public UnstableWarheads()
			: base(3, 3, StationLocation.LowerBlue, PlayerAction.C)
		{
		}

		public override void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears)
		{
			base.Initialize(sittingDuck, threatController, timeAppears);
			SetHealthToRemainingRockets();
			SittingDuck.RocketsModified += SetHealthToRemainingRockets;
		}

		private void SetHealthToRemainingRockets()
		{
			RemainingHealth = SittingDuck.GetRocketCount();
		}

		protected override void PerformXAction(int currentTurn)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(RemainingHealth * 3);
		}

		protected override void OnHealthReducedToZero()
		{
			SittingDuck.RocketsModified -= SetHealthToRemainingRockets;
			base.OnHealthReducedToZero();
		}

		protected override void OnReachingEndOfTrack()
		{
			SittingDuck.RocketsModified -= SetHealthToRemainingRockets;
			base.OnReachingEndOfTrack();
		}

		public static string GetDisplayName()
		{
			return "Unstable Warheads";
		}
	}
}
