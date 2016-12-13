using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Red
{
	public class JumperB : Jumper
	{
		protected override ZoneLocation JumpDestination
		{
			get { return CurrentZone.BluewardZoneLocationWithWrapping(); }
		}
	}
}
