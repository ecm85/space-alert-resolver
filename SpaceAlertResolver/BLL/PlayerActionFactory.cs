using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BLL
{
	public static class PlayerActionFactory
	{
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static IList<PlayerAction> CreateSingleActionList(PlayerSpecialization? basicSpecialization, PlayerSpecialization? advancedSpecialization, IEnumerable<PlayerActionType?> actionTypes)
		{
			return actionTypes
				.Select(actionType => CreateSingleAction(basicSpecialization, advancedSpecialization, actionType))
				.ToList();
		}

		public static PlayerAction CreateEmptyAction()
		{
			return new StandardPlayerAction(null);
		}

		public static PlayerAction CreateSingleAction(PlayerSpecialization? basicSpecialization, PlayerSpecialization? advancedSpecialization, PlayerActionType? actionType, PlayerActionType? extraAction = null)
		{
			var firstAction = actionType ?? extraAction;
			var secondAction = actionType == null ? null : extraAction;
			if ((secondAction == PlayerActionType.BasicSpecialization || secondAction == PlayerActionType.AdvancedSpecialization))
				Swap(ref firstAction, ref secondAction);
			switch (firstAction)
			{
				case PlayerActionType.BasicSpecialization:
					switch (basicSpecialization)
					{
						case PlayerSpecialization.Medic:
							if (secondAction == null || secondAction.IsBasicMovement())
								return new MedicBasicPlayerAction(secondAction);
							return new StandardPlayerAction(secondAction);
					}
					break;
				case PlayerActionType.AdvancedSpecialization:
					switch (advancedSpecialization)
					{
						case PlayerSpecialization.Medic:
							if(secondAction.IsBasicMovement())
								return new MedicAdvancedPlayerAction(secondAction);
							return new StandardPlayerAction(secondAction);
						case PlayerSpecialization.SpecialOps:
							if (secondAction == null || secondAction.IsBasicMovement())
								return new SpecialOpsAdvancedPlayerAction (secondAction);
							//This will ignore the special-ops action if there is no second action selected
							return new StandardPlayerAction(null);
					}
					break;
			}
			//This will ignore the second action if:
			//  Neither is a specialization
			//  The first is a non-Medic basic specialization
			//  The first is a non-Medic, non-SpecialOps specialization
			return new StandardPlayerAction(firstAction);
		}

		private static void Swap(ref PlayerActionType? firstAction, ref PlayerActionType? secondAction)
		{
			var temp = firstAction;
			firstAction = secondAction;
			secondAction = temp;
		}

		private class StandardPlayerAction : PlayerAction
		{
			public StandardPlayerAction(PlayerActionType? actionType) : base(actionType)
			{
			}

			public override bool HasBasicSpecializationAttached
			{
				get { return false; }
			}

			public override bool HasAdvancedSpecializationAttached
			{
				get { return false; }
			}
		}

		private class SpecialOpsAdvancedPlayerAction : PlayerAction
		{
			public SpecialOpsAdvancedPlayerAction(PlayerActionType? actionType) : base(actionType)
			{
			}

			public override bool HasBasicSpecializationAttached
			{
				get { return false; }
			}

			public override bool HasAdvancedSpecializationAttached
			{
				get { return true; }
			}
		}

		private class MedicBasicPlayerAction : PlayerAction
		{
			public MedicBasicPlayerAction(PlayerActionType? actionType) : base(actionType)
			{
			}

			public override bool HasBasicSpecializationAttached
			{
				get { return true; }
			}

			public override bool HasAdvancedSpecializationAttached
			{
				get { return false; }
			}
		}

		private class MedicAdvancedPlayerAction : PlayerAction
		{
			public MedicAdvancedPlayerAction(PlayerActionType? actionType) : base(actionType)
			{
			}

			public override bool HasBasicSpecializationAttached
			{
				get { return false; }
			}

			public override bool HasAdvancedSpecializationAttached
			{
				get { return true; }
			}
		}
	}
}
