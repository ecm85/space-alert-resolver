using BLL.Tracks;

namespace BLL.Threats.External.Minor.Red
{
	public class PhasingDestroyer : MinorRedExternalThreat
	{
		private PhasingThreatCore phasingThreatCore;

		internal PhasingDestroyer()
			: base(2, 5, 2)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			phasingThreatCore = new PhasingThreatCore(this);
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 1 : 2, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 2 : 3, ThreatDamageType.DoubleDamageThroughShields);
		}

		public override bool IsDamageable => base.IsDamageable && phasingThreatCore.IsDamageable;

		public override bool IsMoveable => base.IsDamageable && phasingThreatCore.IsDamageable;

		protected override void OnThreatTerminated()
		{
			phasingThreatCore.ThreatTerminated();
			base.OnThreatTerminated();
		}

		public override string Id { get; } = "E3-103";
		public override string DisplayName { get; } = "Phasing Destroyer";
		public override string FileName { get; } = "PhasingDestroyer";
	}
}
