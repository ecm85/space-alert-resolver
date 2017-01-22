using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;

namespace BLL
{
	public class ThreatController
	{
		public IDictionary<ZoneLocation, Track> ExternalTracks { get; }
		public Track InternalTrack { get; }
		private IList<ExternalThreat> ExternalThreats { get; }
		private IList<InternalThreat> InternalThreats { get; }
		public event EventHandler JumpingToHyperspace = (sender, args) => { };
		public event EventHandler PlayerActionsEnding = (sender, args) => { };
		public event EventHandler TurnEnding = (sender, args) => { };
		public event EventHandler DamageResolutionEnding = (sender, args) => { };
		private readonly IList<ThreatStatus> externalThreatStatusEffects = new List<ThreatStatus>();
		private readonly IList<ThreatStatus> singleTurnExternalThreatStatusEffects = new List<ThreatStatus>();

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

		public IEnumerable<InternalThreat> InternalThreatsOnTrack
		{
			get { return InternalThreats.Where(threat => threat.IsOnTrack); }
		}

		public IEnumerable<ExternalThreat> ExternalThreatsOnTrack
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

		public IEnumerable<InternalThreat> InternalThreatsOnShip { get {return InternalThreats.Where(threat => threat.IsOnShip);} }

		public ThreatController(IDictionary<ZoneLocation, Track> externalTracks, Track internalTrack, IList<ExternalThreat> externalThreats, IList<InternalThreat> internalThreats)
		{
			InternalTrack = internalTrack;
			ExternalTracks = externalTracks;
			ExternalThreats = externalThreats;
			InternalThreats = internalThreats;
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
			var allMoveableThreats = new Threat[0]
				.Concat(InternalThreatsOnTrack)
				.Concat(ExternalThreatsOnTrack)
				.OrderBy(threat => threat.TimeAppears)
				.ThenBy(threat => threat.ThreatType)
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

		public void OnJumpingToHyperspace()
		{
			JumpingToHyperspace(null, null);
		}

		public void OnPlayerActionsEnded()
		{
			PlayerActionsEnding(null, null);
		}

		public void PerformEndOfTurn()
		{
			TurnEnding(null, null);
			foreach (var singleTurnExternalThreatStatusEffect in singleTurnExternalThreatStatusEffects)
				foreach (var externalThreat in ExternalThreats)
					externalThreat.SetThreatStatus(singleTurnExternalThreatStatusEffect, false);
		}

		public void PerformEndOfDamageResolution()
		{
			DamageResolutionEnding(null, null);
		}

		public void AddExternalThreatEffect(ThreatStatus threatStatus)
		{
			foreach (var externalThreat in ExternalThreats)
				externalThreat.SetThreatStatus(threatStatus, true);
			externalThreatStatusEffects.Add(threatStatus);
		}

		public void RemoveExternalThreatEffect(ThreatStatus threatStatus)
		{
			foreach (var externalThreat in ExternalThreats)
				externalThreat.SetThreatStatus(threatStatus, false);
			externalThreatStatusEffects.Remove(threatStatus);
		}

		public void AddSingleTurnExternalThreatEffect(ThreatStatus threatStatus)
		{
			foreach (var externalThreat in ExternalThreats)
				externalThreat.SetThreatStatus(threatStatus, true);
			singleTurnExternalThreatStatusEffects.Add(threatStatus);
		}

		public void AddInternalThreat(InternalThreat newThreat, int timeAppears, int position)
		{
			newThreat.TimeAppears = timeAppears;
			newThreat.PlaceOnTrack(InternalTrack, position);
			InternalThreats.Add(newThreat);
		}

		public void AddInternalThreat(InternalThreat newThreat, int timeAppears)
		{
			newThreat.TimeAppears = timeAppears;
			newThreat.PlaceOnTrack(InternalTrack);
			InternalThreats.Add(newThreat);
		}

		public void AddInternalTracklessThreat(InternalThreat newThreat)
		{
			InternalThreats.Add(newThreat);
		}

		public void AddExternalThreat(ExternalThreat newThreat, int timeAppears, ZoneLocation zoneLocation)
		{
			newThreat.CurrentZone = zoneLocation;
			newThreat.TimeAppears = timeAppears;
			newThreat.PlaceOnTrack(ExternalTracks[zoneLocation]);
			foreach (var threat in externalThreatStatusEffects.Concat(singleTurnExternalThreatStatusEffects))
				newThreat.SetThreatStatus(threat, true);
			ExternalThreats.Add(newThreat);
		}
	}
}
