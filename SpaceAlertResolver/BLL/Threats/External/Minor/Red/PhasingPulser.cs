using BLL.Tracks;

namespace BLL.Threats.External.Minor.Red
{
	public class PhasingPulser : MinorRedExternalThreat
	{
		private PhasingThreatCore phasingThreatCore;

		public PhasingPulser()
			: base(1, 6, 2)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			phasingThreatCore = new PhasingThreatCore(this);
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackAllZones(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 0 : 1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackAllZones(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 2 : 3);
		}

		public override bool IsDamageable => base.IsDamageable && phasingThreatCore.IsDamageable;

		public override bool IsMoveable => base.IsDamageable && phasingThreatCore.IsDamageable;

		protected override void OnThreatTerminated()
		{
			phasingThreatCore.ThreatTerminated();
			base.OnThreatTerminated();
		}

		public override string Id { get; } = "E3-102";
		public override string DisplayName { get; } = "Phasing Pulser";
		public override string FileName { get; } = "PhasingPulser";
	}
}
