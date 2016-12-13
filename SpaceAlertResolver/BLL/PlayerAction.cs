namespace BLL
{
	public abstract class PlayerAction
	{
		public PlayerActionType? ActionType { get; private set; }
		public abstract bool HasBasicSpecializationAttached { get; }
		public abstract bool HasAdvancedSpecializationAttached { get; }

		protected PlayerAction(PlayerActionType? actionType)
		{
			ActionType = actionType;
		}

		public bool CanBeMadeHeroic()
		{
			return ActionType.CanBeMadeHeroic();
		}

		public void MakeHeroic()
		{
			ActionType = ActionType.MakeHeroic();
		}
	}
}
