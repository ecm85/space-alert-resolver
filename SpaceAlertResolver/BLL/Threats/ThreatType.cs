using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public enum ThreatType
	{
		SeriousInternal,
		SeriousExternal,
		MinorInternal,
		MinorExternal
	}

	public static class ThreatTypeExtensions
	{
		public static int ThreatPoints(this ThreatType threatType)
		{
			switch (threatType)
			{
				case ThreatType.MinorExternal:
				case ThreatType.MinorInternal:
					return 1;
				case ThreatType.SeriousExternal:
				case ThreatType.SeriousInternal:
					return 2;
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
