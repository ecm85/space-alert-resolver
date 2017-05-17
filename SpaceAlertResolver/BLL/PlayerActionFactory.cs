using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BLL
{
	public static class PlayerActionFactory
	{
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static IEnumerable<PlayerAction> CreateSingleActionList(IEnumerable<PlayerActionType?> actionTypes)
		{
			return actionTypes
				.Select(actionType => new PlayerAction(actionType, null, null))
				.ToList();
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static IEnumerable<PlayerAction> CreateDoubleActionList(IList<PlayerActionType?> actionTypes)
		{
			var firstActions = actionTypes.Where((actionType, index) => index%2 == 0);
			var secondActions = actionTypes.Where((actionType, index) => index%2 != 0);
			return firstActions.Zip(secondActions, (firstAction, secondAction) => new PlayerAction(firstAction, secondAction, null));
		}

		public static PlayerAction CreateEmptyAction()
		{
			return new PlayerAction(null, null, null);
		}
	}
}
