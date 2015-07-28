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
		public event Action JumpingToHyperspace = () => { };
		public event Action EndOfPlayerActions = () => { };
		public event Action EndOfTurn = () => { };
		public event Action EndOfDamageResolution = () => { };
		private IDictionary<object, ExternalThreatEffect> CurrentExternalThreatBuffsBySource { get; set; }

		public IEnumerable<ExternalThreat> DamageableExternalThreats
		{
			get { return ExternalThreats.Where(threat => threat.IsDamageable); }
		}

		public IEnumerable<InternalThreat> DamageableInternalThreats
		{
			get { return InternalThreats.Where(threat => threat.IsDamageable); }
		}

		private IEnumerable<ExternalThreat> MoveableExternalThreats
		{
			get { return ExternalThreats.Where(threat => threat.IsMoveable); }
		}

		private IEnumerable<InternalThreat> InternalThreatsOnTrack
		{
			get { return InternalThreats.Where(threat => threat.IsOnTrack); }
		}

		private IEnumerable<ExternalThreat> ExternalThreatsOnTrack
		{
			get { return ExternalThreats.Where(threat => threat.IsOnTrack); }
		}

		private IEnumerable<InternalThreat> MoveableInternalThreats
		{
			get { return InternalThreats.Where(threat => threat.IsMoveable); }
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

		public object SingleTurnThreatSource { get; private set; }

		public ThreatController(IDictionary<ZoneLocation, Track> externalTracks, Track internalTrack, IList<ExternalThreat> externalThreats, IList<InternalThreat> internalThreats)
		{
			InternalTrack = internalTrack;
			ExternalTracks = externalTracks;
			ExternalThreats = externalThreats;
			InternalThreats = internalThreats;
			CurrentExternalThreatBuffsBySource = new Dictionary<object, ExternalThreatEffect>();
			SingleTurnThreatSource = new object();
		}

		public void AddNewThreatsToTracks(int currentTurn)
		{
			foreach (var newThreat in ExternalThreats.Where(threat => threat.TimeAppears == currentTurn + 1))
				newThreat.PlaceOnBoard(ExternalTracks[newThreat.CurrentZone]);

			foreach (var newThreat in InternalThreats.Where(threat => threat.TimeAppears == currentTurn + 1))
				newThreat.PlaceOnBoard(InternalTrack);
		}

		public void MoveThreats(int currentTurn)
		{
			var allMoveableThreats = new Threat[0]
				.Concat(InternalThreatsOnTrack)
				.Concat(ExternalThreatsOnTrack)
				.OrderBy(threat => threat.TimeAppears)
				.ThenBy(threat => threat.Type)
				.ToList();
			foreach (var moveableThreat in allMoveableThreats)
				moveableThreat.Move(currentTurn);
		}

		public void MoveOtherExternalThreats(int currentTurn, int amount, ExternalThreat source)
		{
			foreach (var externalThreat in MoveableExternalThreats.Except(new[] {source}).OrderBy(threat => threat.TimeAppears))
				externalThreat.Move(currentTurn, amount);
		}

		public void MoveInternalThreats(int currentTurn, int amount)
		{
			foreach (var internalThreat in MoveableInternalThreats.OrderBy(threat => threat.TimeAppears))
				internalThreat.Move(currentTurn, amount);
		}

		public void MoveExternalThreatsInZone(int currentTurn, int amount, ZoneLocation zoneLocation)
		{
			foreach (var externalThreat in MoveableExternalThreats.Where(threat => threat.CurrentZone == zoneLocation).OrderBy(threat => threat.TimeAppears))
				externalThreat.Move(currentTurn, amount);
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
			RemoveExternalThreatEffectForSource(SingleTurnThreatSource);
			EndOfTurn();
		}

		public void PerformEndOfDamageResolution()
		{
			EndOfDamageResolution();
		}

		public IEnumerable<ExternalThreatEffect> CurrentExternalThreatBuffs()
		{
			return CurrentExternalThreatBuffsBySource.Values.ToList();
		}

		public void AddExternalThreatEffect(ExternalThreatEffect effect, object source)
		{
			CurrentExternalThreatBuffsBySource[source] = effect;
		}

		public void RemoveExternalThreatEffectForSource(object source)
		{
			CurrentExternalThreatBuffsBySource.Remove(source);
		}

		public void AddInternalThreat(InternalThreat newThreat, int timeAppears, int position)
		{
			newThreat.TimeAppears = timeAppears;
			newThreat.PlaceOnBoard(InternalTrack, position);
			InternalThreats.Add(newThreat);
		}

		public void AddInternalThreat(InternalThreat newThreat, int timeAppears)
		{
			newThreat.TimeAppears = timeAppears;
			newThreat.PlaceOnBoard(InternalTrack);
			InternalThreats.Add(newThreat);
		}

		public void AddExternalThreat(ExternalThreat newThreat, int timeAppears, ZoneLocation zoneLocation)
		{
			newThreat.CurrentZone = zoneLocation;
			newThreat.TimeAppears = timeAppears;
			newThreat.PlaceOnBoard(ExternalTracks[zoneLocation]);
			ExternalThreats.Add(newThreat);
		}
	}
}
