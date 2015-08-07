using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public interface IThreatWithBonusThreat<in T> where T: Threat
	{
		void SetBonusThreat(T bonusThreat);
	}
}
