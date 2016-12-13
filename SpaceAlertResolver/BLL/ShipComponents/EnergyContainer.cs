namespace BLL.ShipComponents
{
	public abstract class EnergyContainer
	{
		protected int Capacity { get; private set; }

		private int energy;
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
				if (Energy > Capacity)
					Energy = Capacity;
			}
		}

		public void Repair()
		{
			var wasAlreadyDamaged = IsDamaged;
			IsDamaged = false;
			if (wasAlreadyDamaged)
				Capacity++;
		}
	}
}
