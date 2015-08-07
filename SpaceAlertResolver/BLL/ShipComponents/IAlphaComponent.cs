using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public interface IAlphaComponent : IDamageableComponent
	{
		void PerformAAction(bool isHeroic, Player performingPlayer, bool isAdvanced);
	}
}
