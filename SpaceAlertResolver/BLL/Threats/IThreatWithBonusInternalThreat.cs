using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.Internal;

namespace BLL.Threats
{
	public interface IThreatWithBonusInternalThreat
	{
		void SetBonusThreat(InternalThreat threatToCallIn);
	}
}
