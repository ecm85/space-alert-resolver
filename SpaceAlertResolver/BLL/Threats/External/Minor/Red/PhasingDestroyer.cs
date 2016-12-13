using BLL.Tracks;

namespace BLL.Threats.External.Minor.Red
{
	public class PhasingDestroyer : MinorRedExternalThreat
	{
		private PhasingThreatCore phasingThreatCore;

		public PhasingDestroyer()
			: base(2, 5, 2)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			phasingThreatCore = new PhasingThreatCore(this);
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 1 : 2, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 2 : 3, ThreatDamageType.DoubleDamageThroughShields);
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
