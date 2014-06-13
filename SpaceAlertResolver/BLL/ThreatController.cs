using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;

namespace BLL
{
	public class ThreatController
	{
		private IDictionary<ZoneLocation, Track> ExternalTracks { get; set; }
		private Track InternalTrack { get; set; }
		public IList<ExternalThreat> ExternalThreats { get; private set; }
		public IList<InternalThreat> InternalThreats { get; private set; }
		public event Action<int> ThreatsMove = turn => { };
		public event Action<int, int> ExternalThreatsMove = (turn, amount) => { };
		public event Action JumpingToHyperspace = () => { };
		public event Action EndOfPlayerActions = () => { };
		public event Action EndOfTurn = () => { };
		public event Action EndOfDamageResolution = () => { };

		public IEnumerable<ExternalThreat> DamageableExternalThreats
		{
			get { return ExternalThreats.Where(threat => threat.IsDamageable); }
		}

		public ThreatController(IDictionary<ZoneLocation, Track> externalTracks, Track internalTrack, IList<ExternalThreat> externalThreats, IList<InternalThreat> internalThreats)
		{
			InternalTrack = internalTrack;
			ExternalTracks = externalTracks;
			ExternalThreats = externalThreats;
			InternalThreats = internalThreats;
		}

		public void AddNewThreatsToTracks(int currentTurn)
		{
			foreach (var newThreat in ExternalThreats.Where(threat => threat.TimeAppears == currentTurn))
				newThreat.PlaceOnTrack(ExternalTracks[newThreat.CurrentZone]);

			foreach (var newThreat in InternalThreats.Where(threat => threat.TimeAppears == currentTurn))
				newThreat.PlaceOnTrack(InternalTrack);
		}

		public void MoveThreats(int currentTurn)
		{
			ThreatsMove(currentTurn);
		}

		public void MoveExternalThreats(int currentTurn, int amount)
		{
			ExternalThreatsMove(currentTurn, amount);
		}

		public void JumpToHyperspace()
		{
			JumpingToHyperspace();
		}

		public void PerformEndOfPlayerActions()
		{
			EndOfPlayerActions();
		}

		public void PerformEndOfTurn()
		{
			EndOfTurn();
		}

		public void PerformEndOfDamageResolution()
		{
			EndOfDamageResolution();
		}
	}
}
