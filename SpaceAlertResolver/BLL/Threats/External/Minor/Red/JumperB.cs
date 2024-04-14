using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Red
{
	public class JumperB : Jumper
	{
		protected override ZoneLocation JumpDestination
		{
			get { return CurrentZone.BluewardZoneLocationWithWrapping(); }
		}

		public override string Id { get; } = "E3-106";
		public override string DisplayName { get; } = "Jumper";
		public override string FileName { get; } = "JumperB";
	}
}
