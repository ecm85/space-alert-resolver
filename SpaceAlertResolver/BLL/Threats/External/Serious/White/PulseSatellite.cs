using BLL.Players;

namespace BLL.Threats.External.Serious.White
{
	public class PulseSatellite : SeriousWhiteExternalThreat
	{
		internal PulseSatellite()
			: base(2, 4, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackAllZones(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackAllZones(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(3);
		}

		public override string Id { get; } = "SE1-04";
		public override string DisplayName { get; } = "Pulse Satellite";
		public override string FileName { get; } = "PulseSatellite";

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return DistanceToShip != 3 && base.CanBeTargetedBy(damage);
		}
	}
}
