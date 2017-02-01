namespace BLL
{
	public class PlayerAction
	{
		public PlayerActionType? FirstActionType { get; private set; }
		public bool FirstActionPerformed { get; set; }
		public PlayerActionType? SecondActionType { get; set; }
		public bool SecondActionPerformed { get; set; }
		public PlayerActionType? BonusActionType { get; private set; }
		public bool BonusActionPerformed { get; set; }

		public PlayerActionType? MarkNextActionPerformed()
		{
			if (!BonusActionPerformed)
			{
				BonusActionPerformed = true;
				return BonusActionType;
			}
			if (!FirstActionPerformed)
			{
				FirstActionPerformed = true;
				return FirstActionType;
			}
			if (!SecondActionPerformed)
			{
				SecondActionPerformed = true;
				return SecondActionType;
			}
			return null;
		}

		public PlayerAction(PlayerActionType? firstActionType, PlayerActionType? secondActionType, PlayerActionType? bonusActionType)
		{
			FirstActionType = firstActionType;
			SecondActionType = secondActionType;
			BonusActionType = bonusActionType;
		}

		public bool CanBeMadeHeroic()
		{
			return FirstActionType.CanBeMadeHeroic() || SecondActionType.CanBeMadeHeroic();
		}

		public void MakeHeroic()
		{
			if (FirstActionType.CanBeMadeHeroic())
				FirstActionType = FirstActionType.MakeHeroic();
			else if (SecondActionType.CanBeMadeHeroic())
				SecondActionType = SecondActionType.MakeHeroic();
		}
	}
}
