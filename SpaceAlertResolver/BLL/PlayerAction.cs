using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class PlayerAction
	{
		public PlayerActionType? ActionType { get; set; }
		public bool HasBasicSpecializationAttached { get; set; }
		public bool HasAdvancedSpecializationAttached { get; set; }
	}
}
