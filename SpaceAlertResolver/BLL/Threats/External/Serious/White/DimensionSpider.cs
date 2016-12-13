using BLL.Common;
using BLL.Tracks;

namespace BLL.Threats.External.Serious.White
{
	public class DimensionSpider : SeriousWhiteExternalThreat
	{
		public DimensionSpider()
			: base(0, 13, 1)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			ThreatController.JumpingToHyperspaceEventHandler += HandleJumpingToHyperspace;
		}

		protected override void PerformXAction(int currentTurn)
		{
			Shields = 1;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Shields++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(4);
		}

		private void HandleJumpingToHyperspace()
		{
			PerformZAction(-1);
			OnReachingEndOfTrack();
		}
		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			ThreatController.JumpingToHyperspaceEventHandler -= HandleJumpingToHyperspace;
		}
	}
}
