using BLL;

namespace PL.Models
{
	public class ActionModel
	{
		public string SerializationText { get; set; }
		public string DisplayText { get; set; }
		public string EntryText { get; set; }
		public string Description { get; set; }
		public PlayerActionType? Action { get; set; }
	}
}
