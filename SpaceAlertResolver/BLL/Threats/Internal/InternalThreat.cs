using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class InternalThreat : Threat
	{
		public Station CurrentStation { get; protected set; }

		protected InternalThreat(int pointsForSurviving, int pointsForDefeating, int shields, int health, int speed, int timeAppears, Station currentStation) :
			base(pointsForSurviving, pointsForDefeating, shields, health, speed, timeAppears)
		{
			CurrentStation = currentStation;
		}
	}
}
