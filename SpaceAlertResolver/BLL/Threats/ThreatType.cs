using System.Diagnostics.CodeAnalysis;

namespace BLL.Threats
{
	[SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	public enum ThreatType
	{
		SeriousInternal = 1,
		MinorInternal = 2,
		SeriousExternal = 3,
		MinorExternal = 4
	}
}
