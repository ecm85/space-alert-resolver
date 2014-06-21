﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
	public class Driller : MinorRedInternalThreat
	{
		public Driller()
			: base(2, 3, StationLocation.LowerBlue, PlayerAction.BattleBots, 1)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			totalInaccessibility = 0;
			MoveTowardsMostDamagedZone();
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveTowardsMostDamagedZone();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(4);
		}

		internal void MoveTowardsMostDamagedZone()
		{
			var currentZone = (ZoneLocation?)CurrentZone;

			var zones =
				new []
				{
					new {NewZone = currentZone, MoveCommand = new Action(() => { })},
					new {NewZone = currentZone.BluewardZoneLocation(), MoveCommand = new Action(MoveBlue)},
					new {NewZone = currentZone.BluewardZoneLocation().BluewardZoneLocation(), MoveCommand = new Action(MoveBlue)},
					new {NewZone = currentZone.RedwardZoneLocation(), MoveCommand = new Action(MoveRed)},
					new {NewZone = currentZone.RedwardZoneLocation().RedwardZoneLocation(), MoveCommand = new Action(MoveRed)}
				}
				.Where(zone => zone.NewZone != null)
				.ToList();
			
			var mostDamagedZoneGroup = zones
				.Select(zone => new { Zone = zone, DamageTaken = SittingDuck.GetDamageToZone(zone.NewZone.Value) })
				.GroupBy(zone => zone.DamageTaken)
				.OrderByDescending(group => group.Key)
				.First();

			if (mostDamagedZoneGroup.Count() == 1)
				mostDamagedZoneGroup.Single().Zone.MoveCommand();
			else
				zones.Single(zone => zone.NewZone.Value == ZoneLocation.White).MoveCommand();
		}

		public static string GetDisplayName()
		{
			return "Driller";
		}
	}
}