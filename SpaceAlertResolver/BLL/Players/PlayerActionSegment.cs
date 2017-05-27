namespace BLL.Players
{
	public class PlayerActionSegment
	{
		public PlayerActionStatus SegmentStatus { get; set; }
		public PlayerActionType? SegmentType { get; set; }

		public bool CanBeMadeHeroic()
		{
			return SegmentType.CanBeMadeHeroic() && SegmentStatus == PlayerActionStatus.NotPerformed;
		}

		public void MakeHeroic()
		{
			SegmentType = SegmentType.MakeHeroic();
		}
	}
}
