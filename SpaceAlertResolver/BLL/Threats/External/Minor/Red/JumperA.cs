using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Red
{
	public class JumperA : Jumper
	{
		protected override ZoneLocation JumpDestination
		{
			get { return CurrentZone.RedwardZoneLocationWithWrapping(); }
		}
	}
}
