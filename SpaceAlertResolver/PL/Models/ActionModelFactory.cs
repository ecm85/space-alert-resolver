using System.Linq;
using BLL;

namespace PL.Models
{
	public static class ActionModelFactory
	{
		public static ActionModel Create(PlayerAction action, Player player)
		{
			var matchingStandardAction = ActionModel.AllSingleActionModels.Concat(ActionModel.AllDoubleActionModels)
				.FirstOrDefault(actionModel =>
					action.FirstActionType == actionModel.FirstAction &&
					action.SecondActionType == actionModel.SecondAction);
			var matchingSpecializationAction = PlayerSpecializationActionModel.AllPlayerSpecializationActionModels
				.FirstOrDefault(actionModel =>
					action.FirstActionType == actionModel.FirstAction &&
					player.Specialization == actionModel.PlayerSpecialization);
			var matchingBonusAction = PlayerSpecializationActionModel.AllPlayerSpecializationActionModels
				.FirstOrDefault(actionModel =>
					action.BonusActionType == actionModel.FirstAction &&
					player.Specialization == actionModel.PlayerSpecialization);

			var matchingAction = (matchingStandardAction ?? matchingSpecializationAction)?.Clone();
			if (matchingAction != null && action.BonusActionType != null)
				matchingAction.BonusAction = matchingBonusAction;
			return matchingAction;
		}
	}
}
