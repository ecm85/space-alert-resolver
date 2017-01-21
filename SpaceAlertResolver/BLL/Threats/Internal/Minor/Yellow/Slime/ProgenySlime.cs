using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow.Slime
{
	public abstract class ProgenySlime : BaseSlime
	{
		protected ProgenySlime(NormalSlime parentSlime, StationLocation currentStation) : base(1, currentStation)
		{
			Parent = ParentSlime = parentSlime;
		}

		protected override bool IsDefeatedWhenHealthReachesZero => false;
		protected override bool IsSurvivedWhenReachingEndOfTrack => false;

		protected override void PerformXAction(int currentTurn)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			ParentSlime.SpreadFrom(CurrentStation, Position);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			ParentSlime.OnProgenyKilled();
		}

		private NormalSlime ParentSlime { get; }
	}
}
