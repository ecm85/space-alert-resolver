using System;
using System.Diagnostics.CodeAnalysis;
using BLL.Threats;

namespace BLL
{
	[SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly")]
	[SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
	[Serializable]
	public class LoseException : Exception
	{
		public Threat Threat { get; private set; }
		internal LoseException(Threat threat)
		{
			Threat = threat;
		}
	}
}
