using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.External;

namespace BLL.Threats
{
	public interface IThreatWithBonusExternalThreat
	{
		void SetBonusThreat(ExternalThreat threatToCallIn);
	}
}
