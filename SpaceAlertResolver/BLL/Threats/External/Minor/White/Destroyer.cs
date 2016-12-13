namespace BLL.Threats.External.Minor.White
{
	public class Destroyer : MinorWhiteExternalThreat
	{
		public Destroyer()
			: base(2, 5, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(2, ThreatDamageType.DoubleDamageThroughShields);
		}
	}
}
