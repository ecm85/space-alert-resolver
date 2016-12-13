using System;
using BLL.Threats;

namespace BLL
{
	public class LoseException : Exception
	{
		public Threat Threat { get; private set; }
		public LoseException(Threat threat)
		{
			Threat = threat;
		}
	}
}
