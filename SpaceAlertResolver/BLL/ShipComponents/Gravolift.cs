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
				performingPlayer.ShiftFromPlayerActions(currentTurn);
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

		private bool ShiftsPlayers => Occupied || IsDamaged;
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
