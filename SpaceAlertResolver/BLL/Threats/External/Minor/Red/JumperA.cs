using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Red
{
	public class JumperA : Jumper
	{
		protected override ZoneLocation JumpDestination
		{
			get { return CurrentZone.RedwardZoneLocationWithWrapping(); }
		}

		public override string Id { get; } = "E3-105";
		public override string DisplayName { get; } = "Jumper";
		public override string FileName { get; } = "JumperA";
	}
}
