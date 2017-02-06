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
			var machingSpecialiaztionAction = PlayerSpecializationActionModel.AllPlayerSpecializationActionModels
				.FirstOrDefault(actionModel =>
					action.FirstActionType == actionModel.FirstAction &&
					player.Specialization == actionModel.PlayerSpecialization);

			return matchingStandardAction ?? machingSpecialiaztionAction;
		}
	}
}