namespace BLL.ShipComponents
{
	public class UpperStation : StandardStation
	{
		private Shield Shield { get; set; }

		public UpperStation(
			StationLocation stationLocation,
			ThreatController threatController,
			ICharlieComponent charlieComponent,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			Cannon cannon,
			SittingDuck sittingDuck,
			Shield shield) : base(stationLocation, threatController, shield, charlieComponent, gravolift, bluewardAirlock, redwardAirlock, cannon, sittingDuck)
		{
			Shield = shield;
		}

		public int ShieldThroughAttack(int amount)
		{
			return Shield.ShieldThroughAttack(amount);
		}

		public override void DrainEnergy(int amount)
		{
			DrainShield(amount);
		}

		public override void PerformEndOfTurn()
		{
			base.PerformEndOfTurn();
			Shield.PerformEndOfTurn();
		}

		public int DrainShield()
		{
			var oldEnergy = Shield.Energy;
			Shield.Energy = 0;
			return oldEnergy;
		}

		public void DrainShield(int amount)
		{
			Shield.Energy -= amount;
		}

		public void AddBonusShield(int amount)
		{
			Shield.BonusShield += amount;
		}

		public void FillToCapacity()
		{
			Shield.FillToCapacity(false);
		}

		public void SetIneffectiveShields(bool ineffectiveShields)
		{
			Shield.SetIneffectiveShields(ineffectiveShields);
		}

		public void SetReversedShields(bool reversedShields)
		{
			Shield.SetReversedShields(reversedShields);
		}
	}
}
