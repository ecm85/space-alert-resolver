namespace BLL.ShipComponents
{
	public class Rocket
	{
		private bool IsDoubleRocket { get; set; }

		public void SetDoubleRocket()
		{
			IsDoubleRocket = true;
		}

		public PlayerDamage PerformAttack(Player performingPlayer)
		{
			return new PlayerDamage(IsDoubleRocket ? 5 : 3, PlayerDamageType.Rocket, new [] {1, 2}, EnumFactory.All<ZoneLocation>(), performingPlayer);
		}
	}
}
