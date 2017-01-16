using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class RabidBeast : SeriousRedInternalThreat
	{
		private InfectedPlayers infectedPlayers;

		public RabidBeast()
			: base(2, 2, StationLocation.UpperBlue, PlayerActionType.BattleBots)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			InfectPlayers();
			MoveRed();
			MoveRed();
		}

		protected override void PerformYAction(int currentTurn)
		{
			InfectPlayers();
			ChangeDecks();
			MoveBlue();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(4);
		}

		public override string Id { get; } = "SI3-101";
		public override string DisplayName { get; } = "Rabid Beast";
		public override string FileName { get; } = "RabidBeast";

		private void InfectPlayers()
		{
			if (infectedPlayers == null)
			{
				infectedPlayers = new InfectedPlayers(ThreatType, Difficulty);
				ThreatController.AddInternalThreat(infectedPlayers, TimeAppears, Position.GetValueOrDefault());
			}
			var playersInCurrentStation = SittingDuck.GetPlayersInStation(CurrentStation);
			foreach (var player in playersInCurrentStation)
				infectedPlayers.InfectPlayer(player);
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}

		private class InfectedPlayers : InternalThreat
		{
			private readonly HashSet<Player> infectedPlayers;

			public InfectedPlayers(ThreatType threatType, ThreatDifficulty threatDifficulty)
				: base(threatType, threatDifficulty, 0, 0, new List<StationLocation>(), null)
			{
				infectedPlayers = new HashSet<Player>();
			}

			public void InfectPlayer(Player player)
			{
				infectedPlayers.Add(player);
			}

			protected override void PerformXAction(int currentTurn)
			{
			}

			protected override void PerformYAction(int currentTurn)
			{
			}

			protected override void PerformZAction(int currentTurn)
			{
				var stationsToDamage = infectedPlayers
					.Where(player => !player.IsKnockedOut)
					.Select(player => player.CurrentStation.StationLocation.ZoneLocation())
					.ToList();
				Damage(2, stationsToDamage);
			}

			public override string Id { get { throw new NotImplementedException(); } }
			public override string DisplayName { get { throw new NotImplementedException(); } }
			public override string FileName { get { throw new NotImplementedException(); } }

			public override int Points
			{
				get { return 0; }
			}

			public override bool IsDefeated
			{
				get { return false; }
			}

			public override bool IsSurvived
			{
				get { return false; }
			}

			public override bool IsDamageable
			{
				get { return false; }
			}
		}
	}
}
