namespace BLL.ShipComponents
{
	public abstract class UpperStation : StandardStation
	{
		protected abstract Shield Shield { get; }

		public override IBravoComponent BravoComponent => Shield;

		protected UpperStation(
			StationLocation stationLocation,
			ThreatController threatController,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			SittingDuck sittingDuck) : base(stationLocation, threatController, gravolift, bluewardAirlock, redwardAirlock, sittingDuck)
		{
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
