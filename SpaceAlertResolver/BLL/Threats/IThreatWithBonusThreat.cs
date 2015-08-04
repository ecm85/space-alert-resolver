using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public interface IThreatWithBonusThreat<T> where T: Threat
	{
		T BonusThreat { get; set; }
	}
}
