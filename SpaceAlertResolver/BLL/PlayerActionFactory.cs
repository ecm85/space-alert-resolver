using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public static class PlayerActionFactory
	{
		public static List<PlayerAction> CreateSingleActionList(IEnumerable<PlayerActionType?> actionTypes)
		{
			return actionTypes.Select(actionType => new PlayerAction {ActionType = actionType}).ToList();
		}
	}
}
