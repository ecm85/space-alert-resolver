using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			SetHealthToRemainingRockets(null, null);
			SittingDuck.RocketsModified += SetHealthToRemainingRockets;
		}

		private void SetHealthToRemainingRockets(object sender, EventArgs eventArgs)
		{
			RemainingHealth = SittingDuck.GetRocketCount();
		}

		public override void PerformXAction(int currentTurn)
		{
		}

		public override void PerformYAction(int currentTurn)
		{
		}

		public override void PerformZAction(int currentTurn)
		{
			Damage(RemainingHealth * 3);
		}

		protected override void OnHealthReducedToZero()
		{
			SittingDuck.RocketsModified -= SetHealthToRemainingRockets;
			base.OnHealthReducedToZero();
		}

		public override void OnReachingEndOfTrack()
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
