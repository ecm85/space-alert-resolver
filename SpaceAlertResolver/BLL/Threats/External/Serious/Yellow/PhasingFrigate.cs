using BLL.Tracks;

namespace BLL.Threats.External.Serious.Yellow
{
	public class PhasingFrigate : SeriousYellowExternalThreat
	{
		private PhasingThreatCore phasingThreatCore;

		public PhasingFrigate()
			: base(2, 7, 2)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			phasingThreatCore = new PhasingThreatCore(this);
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 1 : 2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 2 : 3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 3 : 4);
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
