using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.Internal;

namespace BLL.Threats.External.Serious.Red
{
	public class TransmitterSatellite : SeriousRedExternalThreat, IThreatWithBonusInternalThreat
	{
		private InternalThreat threatToCallIn;
		private bool calledInThreat;

		public TransmitterSatellite()
			: base(2, 5, 3)
		{
		}

		public void SetBonusThreat(InternalThreat threatToCallIn)
		{
			this.threatToCallIn = threatToCallIn;
		}

		public override bool NeedsBonusInternalThreat { get { return true; } }

		protected override void PerformXAction(int currentTurn)
		{
			CallInInternalThreat(currentTurn);
		}

		protected override void PerformYAction(int currentTurn)
		{
			ThreatController.MoveInternalThreats(currentTurn, 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutCaptain();
			SittingDuck.ShiftPlayers(EnumFactory.All<StationLocation>(), currentTurn + 1);
		}

		public override int GetPointsForDefeating()
		{
			return 8 + (calledInThreat ? 0 : threatToCallIn.GetPointsForDefeating());
		}

		protected override int GetPointsForSurviving()
		{
			return 4;
		}

		private void CallInInternalThreat(int currentTurn)
		{
			ThreatController.AddInternalThreat(threatToCallIn, 1000 + currentTurn);
			calledInThreat = true;
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return DistanceToShip != 3 && base.CanBeTargetedBy(damage);
		}
	}
}
