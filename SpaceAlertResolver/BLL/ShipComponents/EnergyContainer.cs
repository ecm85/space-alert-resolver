using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class EnergyContainer : IDamageableComponent
	{
		protected int Capacity { get; private set; }
		
		protected int energy;
		public virtual int Energy
		{
			get { return energy; }
			set { energy = value > 0 ? value : 0; }
		}

		protected EnergyContainer(int capacity, int energy)
		{
			Capacity = capacity;
			this.energy = energy;
		}

		private bool IsDamaged { get; set; }

		public void SetDamaged()
		{
			var wasAlreadyDamaged = IsDamaged;
			IsDamaged = true;
			if (!wasAlreadyDamaged)
			{
				Capacity--;
				if (energy > Capacity)
					energy = Capacity;
			}
		}

		public void Repair()
		{
			var wasAlreadyDamaged = IsDamaged;
			IsDamaged = false;
			if (wasAlreadyDamaged)
				Capacity++;
		}

		public abstract void PerformBAction(bool isHeroic);
	}
}
