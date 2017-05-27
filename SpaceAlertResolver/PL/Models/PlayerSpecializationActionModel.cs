using System;
using System.Collections.Generic;
using BLL.Players;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class PlayerSpecializationActionModel : ActionModel
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public PlayerSpecialization PlayerSpecialization { get; set; }
		public bool CanBeBonusAction { get; set; }

		public override ActionModel Clone()
		{
			return new PlayerSpecializationActionModel
			{
				DisplayText = DisplayText,
				Description = Description,
				FirstAction = FirstAction,
				SecondAction = SecondAction,
				BonusAction = BonusAction,
				PlayerSpecialization = PlayerSpecialization,
				CanBeBonusAction = CanBeBonusAction
			};
		}

		public override string Hotkey
		{
			get
			{
				switch (FirstAction)
				{
					case PlayerActionType.BasicSpecialization:
						return "s";
					case PlayerActionType.AdvancedSpecialization:
						return "S";
					default:
						throw new InvalidOperationException();
				}
			}
		}

		public static IEnumerable<PlayerSpecializationActionModel> AllPlayerSpecializationActionModels { get; } = new[]
		{
			new PlayerSpecializationActionModel
			{
				Description="BasicRocketeer", FirstAction = PlayerActionType.BasicSpecialization, DisplayText = "Basic Rocketeer", PlayerSpecialization = PlayerSpecialization.Rocketeer
			},
			new PlayerSpecializationActionModel
			{
				Description="BasicDataAnalyst", FirstAction = PlayerActionType.BasicSpecialization, DisplayText = "Basic DataAnalyst", PlayerSpecialization = PlayerSpecialization.DataAnalyst
			},
			new PlayerSpecializationActionModel
			{
				Description="BasicEnergyTechnician", FirstAction = PlayerActionType.BasicSpecialization, DisplayText = "Basic EnergyTechnician", PlayerSpecialization = PlayerSpecialization.EnergyTechnician
			},
			new PlayerSpecializationActionModel
			{
				Description="BasicPulseGunner", FirstAction = PlayerActionType.BasicSpecialization, DisplayText = "Basic PulseGunner", PlayerSpecialization = PlayerSpecialization.PulseGunner
			},
			new PlayerSpecializationActionModel
			{
				Description="BasicMedic", FirstAction = PlayerActionType.BasicSpecialization, DisplayText = "Basic Medic", PlayerSpecialization = PlayerSpecialization.Medic, CanBeBonusAction = true
			},
			new PlayerSpecializationActionModel
			{
				Description="BasicTeleporter", FirstAction = PlayerActionType.BasicSpecialization, DisplayText = "Basic Teleporter", PlayerSpecialization = PlayerSpecialization.Teleporter
			},
			new PlayerSpecializationActionModel
			{
				Description="BasicHypernavigator", FirstAction = PlayerActionType.BasicSpecialization, DisplayText = "Basic Hypernavigator", PlayerSpecialization = PlayerSpecialization.Hypernavigator
			},
			new PlayerSpecializationActionModel
			{
				Description="BasicSpecialOps", FirstAction = PlayerActionType.BasicSpecialization, DisplayText = "Basic SpecialOps", PlayerSpecialization = PlayerSpecialization.SpecialOps
			},
			new PlayerSpecializationActionModel
			{
				Description="BasicSquadLeader", FirstAction = PlayerActionType.BasicSpecialization, DisplayText = "Basic SquadLeader", PlayerSpecialization = PlayerSpecialization.SquadLeader
			},
			new PlayerSpecializationActionModel
			{
				Description="BasicMechanic", FirstAction = PlayerActionType.BasicSpecialization, DisplayText = "Basic Mechanic", PlayerSpecialization = PlayerSpecialization.Mechanic
			},

			new PlayerSpecializationActionModel
			{
				Description="AdvancedRocketeer", FirstAction = PlayerActionType.AdvancedSpecialization, DisplayText = "Advanced Rocketeer", PlayerSpecialization = PlayerSpecialization.Rocketeer
			},
			new PlayerSpecializationActionModel
			{
				Description="AdvancedDataAnalyst", FirstAction = PlayerActionType.AdvancedSpecialization, DisplayText = "Advanced DataAnalyst", PlayerSpecialization = PlayerSpecialization.DataAnalyst
			},
			new PlayerSpecializationActionModel
			{
				Description="AdvancedEnergyTechnician", FirstAction = PlayerActionType.AdvancedSpecialization, DisplayText = "Advanced EnergyTechnician", PlayerSpecialization = PlayerSpecialization.EnergyTechnician
			},
			new PlayerSpecializationActionModel
			{
				Description="AdvancedPulseGunner", FirstAction = PlayerActionType.AdvancedSpecialization, DisplayText = "Advanced PulseGunner", PlayerSpecialization = PlayerSpecialization.PulseGunner
			},
			new PlayerSpecializationActionModel
			{
				Description="AdvancedMedic", FirstAction = PlayerActionType.AdvancedSpecialization, DisplayText = "Advanced Medic", PlayerSpecialization = PlayerSpecialization.Medic, CanBeBonusAction = true
			},
			new PlayerSpecializationActionModel
			{
				Description="AdvancedTeleporter", FirstAction = PlayerActionType.AdvancedSpecialization, DisplayText = "Advanced Teleporter", PlayerSpecialization = PlayerSpecialization.Teleporter
			},
			new PlayerSpecializationActionModel
			{
				Description="AdvancedHypernavigator", FirstAction = PlayerActionType.AdvancedSpecialization, DisplayText = "Advanced Hypernavigator", PlayerSpecialization = PlayerSpecialization.Hypernavigator
			},
			new PlayerSpecializationActionModel
			{
				Description="AdvancedSpecialOps", FirstAction = PlayerActionType.AdvancedSpecialization, DisplayText = "Advanced SpecialOps", PlayerSpecialization = PlayerSpecialization.SpecialOps, CanBeBonusAction = true
			},
			new PlayerSpecializationActionModel
			{
				Description="AdvancedSquadLeader", FirstAction = PlayerActionType.AdvancedSpecialization, DisplayText = "Advanced SquadLeader", PlayerSpecialization = PlayerSpecialization.SquadLeader
			},
			new PlayerSpecializationActionModel
			{
				Description="AdvancedMechanic", FirstAction = PlayerActionType.AdvancedSpecialization, DisplayText = "Advanced Mechanic", PlayerSpecialization = PlayerSpecialization.Mechanic
			}
		};
	}
}
