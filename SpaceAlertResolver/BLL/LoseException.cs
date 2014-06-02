using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
