using BLL.Tracks;

namespace BLL.Threats.External.Serious.Red
{
	public class PhasingManOfWar : SeriousRedExternalThreat
	{
		private PhasingThreatCore phasingThreatCore;

		internal PhasingManOfWar()
			: base(2, 9, 1)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			phasingThreatCore = new PhasingThreatCore(this);
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 1 : 2);
			Speed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 2 : 3);
			Shields++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 4 : 5);
		}

		public override bool IsDamageable => base.IsDamageable && phasingThreatCore.IsDamageable;

		public override bool IsMoveable => base.IsDamageable && phasingThreatCore.IsDamageable;

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			phasingThreatCore.ThreatTerminated();
		}

		public override string Id { get; } = "SE3-101";
		public override string DisplayName { get; } = "Phasing Man-Of-War";
		public override string FileName { get; } = "PhasingManOfWar";
	}
}
