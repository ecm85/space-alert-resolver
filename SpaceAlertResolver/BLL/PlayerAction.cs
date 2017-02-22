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

		public PlayerActionType? NextActionToPerform
		{
			get
			{
				if (!BonusActionPerformed)
					return BonusActionType;
				if (!FirstActionPerformed)
					return FirstActionType;
				if (!SecondActionPerformed)
					return SecondActionType;
				return null;
			}
		}

		public void MarkNextActionPerformed()
		{
			if (!BonusActionPerformed)
				BonusActionPerformed = true;
			else if (!FirstActionPerformed)
				FirstActionPerformed = true;
			else if (!SecondActionPerformed)
				SecondActionPerformed = true;
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
