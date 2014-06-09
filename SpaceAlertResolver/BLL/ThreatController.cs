using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;

namespace BLL
{
	public class ThreatController
	{
		private IDictionary<ZoneLocation, ExternalTrack> ExternalTracks { get; set; }
		private InternalTrack InternalTrack { get; set; }
		public IList<ExternalThreat> ExternalThreats { get; private set; }
		public IList<InternalThreat> InternalThreats { get; private set; }

		public ThreatController(IDictionary<ZoneLocation, ExternalTrack> externalTracks, InternalTrack internalTrack, IList<ExternalThreat> externalThreats, IList<InternalThreat> internalThreats)
		{
			InternalTrack = internalTrack;
			ExternalTracks = externalTracks;
			ExternalThreats = externalThreats;
			InternalThreats = internalThreats;
		}

		public void JumpingToHyperspace()
		{
			foreach (var threat in ExternalThreats)
				threat.OnJumpingToHyperspace();
			foreach (var threat in InternalThreats)
				threat.OnJumpingToHyperspace();
		}

		public void MoveAllThreats(int currentTurn)
		{
			var allCurrentThreats = new List<Threat>()
				.Concat(ExternalThreats)
				.Concat(InternalThreats)
				.OrderBy(threat => threat.TimeAppears);
			foreach (var threat in allCurrentThreats)
				threat.Move(currentTurn);
		}

		public void MoveExternalThreatsExcept(IEnumerable<ExternalThreat> threatsToNotMove, int amount, int currentTurn)
		{
			var threatsToMove = ExternalThreats
				.Except(threatsToNotMove)
				.OrderBy(threat => threat.TimeAppears);
			foreach (var threat in threatsToMove)
				threat.Move(amount, currentTurn);
		}

		public void PerformEndOfPlayerActions()
		{
			foreach (var threat in InternalThreats)
				threat.PerformEndOfPlayerActions();
		}

		public void PerformEndOfTurn()
		{
			foreach (var threat in ExternalThreats)
				threat.PerformEndOfTurn();
			foreach (var threat in InternalThreats)
				threat.PerformEndOfTurn();
		}

		public void PerformEndOfDamageResolution()
		{
			foreach (var threat in ExternalThreats)
				threat.PerformEndOfDamageResolution();
		}

		public void AddNewThreatsToTracks(int currentTurn)
		{
			foreach (var newThreat in ExternalThreats.Where(threat => threat.TimeAppears == currentTurn))
				newThreat.PlaceOnTrack(ExternalTracks[newThreat.CurrentZone]);

			foreach (var newThreat in InternalThreats.Where(threat => threat.TimeAppears == currentTurn))
				newThreat.PlaceOnTrack(InternalTrack);
		}
	}
}
