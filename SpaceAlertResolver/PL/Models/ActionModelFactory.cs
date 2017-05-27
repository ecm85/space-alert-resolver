using System.Linq;
using BLL.Players;

namespace PL.Models
{
	public static class ActionModelFactory
	{
		public static ActionModel Create(PlayerAction action, Player player)
		{
			var matchingStandardAction = ActionModel.AllSingleActionModels.Concat(ActionModel.AllSelectableDoubleActionModels).Concat(ActionModel.AllNonSelectableDoubleActionModels)
				.FirstOrDefault(actionModel =>
					action.FirstActionSegment.SegmentType == actionModel.FirstAction &&
					action.SecondActionSegment.SegmentType == actionModel.SecondAction);
			var matchingSpecializationAction = PlayerSpecializationActionModel.AllPlayerSpecializationActionModels
				.FirstOrDefault(actionModel =>
					action.FirstActionSegment.SegmentType == actionModel.FirstAction &&
					player.Specialization == actionModel.PlayerSpecialization);
			var matchingBonusAction = PlayerSpecializationActionModel.AllPlayerSpecializationActionModels
				.FirstOrDefault(actionModel =>
					action.BonusActionSegment.SegmentType == actionModel.FirstAction &&
					player.Specialization == actionModel.PlayerSpecialization);

			var matchingAction = (matchingStandardAction ?? matchingSpecializationAction)?.Clone();
			if (matchingAction != null && action.BonusActionSegment.SegmentType != null)
				matchingAction.BonusAction = matchingBonusAction;
			return matchingAction;
		}
	}
}
