using BLL.Tracks;

namespace BLL.Threats.External.Serious.Red
{
	public class PhasingManOfWar : SeriousRedExternalThreat
	{
		private PhasingThreatCore phasingThreatCore;

		public PhasingManOfWar()
			: base(2, 9, 1)
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
			Speed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 2 : 3);
			Shields++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 4 : 5);
		}

		public override bool IsDamageable => base.IsDamageable && phasingThreatCore.IsDamageable;

		public override bool IsMoveable => base.IsDamageable && phasingThreatCore.IsDamageable;

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			phasingThreatCore.ThreatTerminated();
		}
	}
}
