using BLL.Tracks;

namespace BLL.Threats.External.Minor.Yellow
{
	public class PhasingFighter : MinorYellowExternalThreat
	{
		private PhasingThreatCore phasingThreatCore;

		public PhasingFighter()
			: base(2, 4, 3)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			phasingThreatCore = new PhasingThreatCore(this);
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 1 : 2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 2 : 3);
		}

		public override bool IsDamageable => base.IsDamageable && phasingThreatCore.IsDamageable;

		public override bool IsMoveable => base.IsDamageable && phasingThreatCore.IsDamageable;

		protected override void OnThreatTerminated()
		{
			phasingThreatCore.ThreatTerminated();
			base.OnThreatTerminated();
		}
	}
}
