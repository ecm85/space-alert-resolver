using BLL.ShipComponents;

namespace API.Models
{
	public class CannonModel
	{
		public EnergyType? Energy { get; set; }

		public CannonModel(IAlphaComponent cannon)
		{
			Energy = cannon.EnergyInCannon;
		}
	}
}
