using BLL.Common;

namespace BLL.ShipComponents
{
	public class Gravolift : IDamageableComponent
	{
		private bool Occupied { get; set; }

		public void Use(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			if (ShiftsPlayers && !isHeroic)
				performingPlayer.Shift(currentTurn + 1);
			SetOccupied();
		}

		private void SetOccupied()
		{
			Occupied = true;
		}

		public void PerformEndOfTurn()
		{
			Occupied = false;
		}

		private bool ShiftsPlayers { get { return Occupied || IsDamaged; } }
		private bool IsDamaged { get; set; }

		public void SetDamaged(bool isCampaignDamage)
		{
			IsDamaged = true;
		}

		public void Repair()
		{
			IsDamaged = false;
		}
	}
}
