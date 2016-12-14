using System;
using System.Collections.Generic;

namespace BLL.ShipComponents
{
	public interface IAlphaComponent : IDamageableComponent
	{
		void SetOpticsDisrupted(bool opticsDisrupted);
		event EventHandler CannonFired;
		IEnumerable<PlayerDamage> CurrentPlayerDamage { get; }
		void PerformEndOfTurn();
		void PerformAAction(bool isHeroic, Player performingPlayer, bool isAdvanced);
		bool CanFire();
		void RemoveMechanicBuff();
		void AddMechanicBuff();
	}
}
