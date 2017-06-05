using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.Red
{
	public class Leaper : SeriousRedExternalThreat
	{
		internal Leaper()
			: base(2, 7, 2)
		{
		}

		private void Jump(ZoneLocation newZone)
		{
			var newTrack = ThreatController.ExternalTracks[newZone];
			if (newTrack.StartingPosition>= Position)
				Track = newTrack;
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1);
			Jump(CurrentZone.BluewardZoneLocationWithWrapping());
			Attack(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2);
			Jump(CurrentZone.RedwardZoneLocationWithWrapping());
			Attack(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(4);
			Jump(CurrentZone.BluewardZoneLocationWithWrapping());
			Attack(1);
		}

		public override string Id { get; } = "SE3-106";
		public override string DisplayName { get; } = "Leaper";
		public override string FileName { get; } = "Leaper";
	}
}
