using System;

namespace BLL
{
	public class PhaseEventArgs : EventArgs
	{
		public string PhaseHeader { get; set; }
		public string PhaseSubHeader { get; set; }
	}
}
