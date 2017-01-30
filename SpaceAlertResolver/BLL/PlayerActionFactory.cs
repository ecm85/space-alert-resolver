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

		public static PlayerAction CreateEmptyAction()
		{
			return new PlayerAction(null, null, null);
		}
	}
}
