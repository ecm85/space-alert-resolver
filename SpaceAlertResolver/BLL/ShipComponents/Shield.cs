using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class Shield : EnergyContainer
	{
		private EnergyContainer Source { get; set; }

		protected Shield(Reactor source, int capacity, int energy) : base(capacity, energy)
		{
			Source = source;
		}

		public override void PerformBAction(bool isHeroic)
		{
			FillToCapacity(isHeroic);
		}

		public override int Energy
		{
			get { return base.Energy + BonusShield; }
			set
			{
				if (Energy <= value)
					energy = value - BonusShield;
				else if (value <= 0)
				{
					BonusShield = 0;
					energy = 0;
				}
				else
				{
					var energyToDrain = Energy - value;
					var oldBonusShield = BonusShield;
					BonusShield -= energyToDrain;
					var newBonusShield = BonusShield;
					var bonusEnergyDrained = oldBonusShield - newBonusShield;
					energyToDrain -= bonusEnergyDrained;
					energy -= energyToDrain;
				}
			}
		}

		public void FillToCapacity(bool isHeroic)
		{
			var energyToPull = Capacity - energy;
			var oldSourceEnergy = Source.Energy;
			Source.Energy -= energyToPull;
			var newSourceEnergy = Source.Energy;
			var energyPulled = oldSourceEnergy - newSourceEnergy;
			energy += energyPulled;
			if (energyPulled > 0 && isHeroic)
				energy++;
		}

		public void PerformEndOfTurn()
		{
			BonusShield = 0;
		}

		private int bonusShield;
		public int BonusShield
		{
			get { return bonusShield; }
			set { bonusShield = value > 0 ? value : 0; }
		}

		private bool IneffectiveShields { get; set; }

		public void SetIneffectiveShields(bool ineffectiveShields)
		{
			IneffectiveShields = ineffectiveShields;
		}

		private bool ReversedShields { get; set; }

		public void SetReversedShields(bool reversedShields)
		{
			ReversedShields = reversedShields;
		}

		public int ShieldThroughAttack(int amount)
		{
			var amountUnshielded = amount;
			var oldBonusShield = BonusShield;
			BonusShield -= amount;
			var newBonusShield = BonusShield;
			var amountShielded = oldBonusShield - newBonusShield;
			amountUnshielded -= amountShielded;
			if (ReversedShields)
			{
				var energyAddedToAttack = energy;
				energy = 0;
				return amountShielded - energyAddedToAttack;
			}
			if (!IneffectiveShields)
			{
				var oldShields = Energy;
				Energy -= amountUnshielded;
				var newShields = Energy;
				return (oldShields - newShields) + amountShielded;
			}
			return amountShielded;
		}
	}
}
