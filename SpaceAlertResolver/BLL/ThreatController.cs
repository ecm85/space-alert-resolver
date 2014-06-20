using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;

namespace BLL
{
	public class ThreatController
	{
		public IDictionary<ZoneLocation, Track> ExternalTracks { get; private set; }
		private Track InternalTrack { get; set; }
		private IList<ExternalThreat> ExternalThreats { get; set; }
		private IList<InternalThreat> InternalThreats { get; set; }
		public event Action<int> ThreatsMove = turn => { };
		public event Action<int, int> ExternalThreatsMove = (turn, amount) => { };
		public event Action<int, int> InternalThreatsMove = (turn, amount) => { };
		public event Action JumpingToHyperspace = () => { };
		public event Action EndOfPlayerActions = () => { };
		public event Action EndOfTurn = () => { };
		public event Action EndOfDamageResolution = () => { };
		private IDictionary<ExternalThreat, ExternalThreatBuff> CurrentExternalThreatBuffsBySource { get; set; }
		
		public IEnumerable<ExternalThreat> DamageableExternalThreats
		{
			get { return ExternalThreats.Where(threat => threat.IsDamageable); }
		}

		public IEnumerable<InternalThreat> DamageableInternalThreats
		{
			get { return InternalThreats.Where(threat => threat.IsDamageable); }
		}

		public IEnumerable<Threat> DefeatedThreats
		{
			get { return new List<Threat>().Concat(ExternalThreats).Concat(InternalThreats).Where(threat => threat.IsDefeated); }
		}

		public IEnumerable<Threat> SurvivedThreats
		{
			get { return new List<Threat>().Concat(ExternalThreats).Concat(InternalThreats).Where(threat => threat.IsSurvived); }
		}

		public int TotalThreatPoints
		{
			get { return new List<Threat>().Concat(ExternalThreats).Concat(InternalThreats).Sum(threat => threat.Points); }
		}

		public ThreatController(IDictionary<ZoneLocation, Track> externalTracks, Track internalTrack, IList<ExternalThreat> externalThreats, IList<InternalThreat> internalThreats)
		{
			InternalTrack = internalTrack;
			ExternalTracks = externalTracks;
			ExternalThreats = externalThreats;
			InternalThreats = internalThreats;
			CurrentExternalThreatBuffsBySource = new Dictionary<ExternalThreat, ExternalThreatBuff>();
		}

		public void AddNewThreatsToTracks(int currentTurn)
		{
			foreach (var newThreat in ExternalThreats.Where(threat => threat.TimeAppears == currentTurn + 1))
				newThreat.PlaceOnTrack(ExternalTracks[newThreat.CurrentZone]);

			foreach (var newThreat in InternalThreats.Where(threat => threat.TimeAppears == currentTurn + 1))
				newThreat.PlaceOnTrack(InternalTrack);
		}

		public void MoveThreats(int currentTurn)
		{
			ThreatsMove(currentTurn);
		}

		//TODO: For all 'extra movement things' respect is damageable? or unsubscribe from those events in the threats?
		//TODO: Make sure all phasing threats unsubscribe from events they don't care about
		//TODO: Ninja and rabid beast need to not move after killed but before z if no poisoned/infected players
		public void MoveExternalThreats(int currentTurn, int amount)
		{
			ExternalThreatsMove(currentTurn, amount);
		}

		public void MoveInternalThreats(int currentTurn, int amount)
		{
			InternalThreatsMove(currentTurn, amount);
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

		public IEnumerable<ExternalThreatBuff> CurrentExternalThreatBuffs()
		{
			return CurrentExternalThreatBuffsBySource.Values.ToList();
		}

		public void AddExternalThreatBuff(ExternalThreatBuff buff, ExternalThreat source)
		{
			CurrentExternalThreatBuffsBySource[source] = buff;
		}

		public void RemoveExternalThreatBuffForSource(ExternalThreat source)
		{
			CurrentExternalThreatBuffsBySource.Remove(source);
		}

		public void AddInternalThreat(InternalThreat newThreat)
		{
			InternalThreats.Add(newThreat);
		}
	}
}
