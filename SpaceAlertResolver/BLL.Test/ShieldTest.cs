using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLL.Test
{
	[TestClass]
	public class ShieldTest
	{
		private readonly Reactor reactor;
		private readonly Shield shield;

		public ShieldTest()
		{
			reactor = new CentralReactor();
			shield = new CentralShield(reactor);
		}

		[TestMethod]
		public void Test_Energy_Get_NoBonusEnergy()
		{
			shield.Energy = 3;
			shield.BonusShield = 0;
			Assert.AreEqual(3, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
		}

		[TestMethod]
		public void Test_Energy_Get_IncludeBonusEnergy()
		{
			shield.Energy = 3;
			shield.BonusShield = 1;
			Assert.AreEqual(4, shield.Energy);
			Assert.AreEqual(1, shield.BonusShield);
		}

		[TestMethod]
		public void Test_Energy_Add_NoBonusEnergy()
		{
			shield.Energy = 3;
			shield.BonusShield = 0;
			Assert.AreEqual(3, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
			shield.Energy += 1;
			Assert.AreEqual(4, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
		}

		[TestMethod]
		public void Test_Energy_Add_IncludeBonusEnergy()
		{
			shield.Energy = 3;
			shield.BonusShield = 2;
			Assert.AreEqual(5, shield.Energy);
			Assert.AreEqual(2, shield.BonusShield);
			shield.Energy += 1;
			Assert.AreEqual(6, shield.Energy);
			Assert.AreEqual(2, shield.BonusShield);
		}

		[TestMethod]
		public void Test_Energy_SetToZero_HadBonusEnergy()
		{
			shield.Energy = 3;
			shield.BonusShield = 1;
			Assert.AreEqual(4, shield.Energy);
			Assert.AreEqual(1, shield.BonusShield);
			shield.Energy = 0;
			Assert.AreEqual(0, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
		}

		[TestMethod]
		public void Test_Energy_SetToZero_NoBonusEnergy()
		{
			shield.Energy = 3;
			shield.BonusShield = 0;
			Assert.AreEqual(3, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
			shield.Energy = 0;
			Assert.AreEqual(0, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
		}

		[TestMethod]
		public void Test_Energy_SubtractedLessThanBonusEnergy()
		{
			shield.Energy = 4;
			shield.BonusShield = 2;
			Assert.AreEqual(6, shield.Energy);
			Assert.AreEqual(2, shield.BonusShield);
			shield.Energy -= 1;
			Assert.AreEqual(5, shield.Energy);
			Assert.AreEqual(1, shield.BonusShield);
		}

		[TestMethod]
		public void Test_Energy_SubtractedMoreThanBonusEnergy()
		{
			shield.Energy = 4;
			shield.BonusShield = 2;
			Assert.AreEqual(6, shield.Energy);
			Assert.AreEqual(2, shield.BonusShield);
			shield.Energy -= 3;
			Assert.AreEqual(3, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
		}

		[TestMethod]
		public void Test_Energy_Subtracted_NoBonusEnergy()
		{
			shield.Energy = 4;
			shield.BonusShield = 0;
			Assert.AreEqual(4, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
			shield.Energy -= 3;
			Assert.AreEqual(1, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
		}

		[TestMethod]
		public void Test_Energy_SubtractedMoreThanTotal_NoBonusEnergy()
		{
			shield.Energy = 3;
			shield.BonusShield = 0;
			Assert.AreEqual(3, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
			shield.Energy -= 5;
			Assert.AreEqual(0, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
		}

		[TestMethod]
		public void Test_Energy_SubtractedMoreThanTotal_HasBonusEnergy()
		{
			shield.Energy = 3;
			shield.BonusShield = 1;
			Assert.AreEqual(4, shield.Energy);
			Assert.AreEqual(1, shield.BonusShield);
			shield.Energy -= 5;
			Assert.AreEqual(0, shield.Energy);
			Assert.AreEqual(0, shield.BonusShield);
		}

		[TestMethod]
		public void Test_FillToCapacity_RemainingCapacityGreaterThanSource()
		{
			reactor.Energy = 2;
			shield.Energy = 0;
			shield.FillToCapacity(false);
			Assert.AreEqual(0, reactor.Energy);
			Assert.AreEqual(2, shield.Energy);
		}

		[TestMethod]
		public void Test_FillToCapacity_RemainingCapacityLessThanSource()
		{
			reactor.Energy = 3;
			shield.Energy = 2;
			shield.FillToCapacity(false);
			Assert.AreEqual(2, reactor.Energy);
			Assert.AreEqual(3, shield.Energy);
		}

		[TestMethod]
		public void Test_FillToCapacity_SourceEmpty()
		{
			reactor.Energy = 0;
			shield.Energy = 2;
			shield.FillToCapacity(false);
			Assert.AreEqual(0, reactor.Energy);
			Assert.AreEqual(2, shield.Energy);
		}

		[TestMethod]
		public void Test_FillToCapacity_AtCapacity()
		{
			reactor.Energy = 2;
			shield.Energy = 3;
			shield.FillToCapacity(false);
			Assert.AreEqual(2, reactor.Energy);
			Assert.AreEqual(3, shield.Energy);
		}

		[TestMethod]
		public void Test_FillToCapacity_RemainingCapacityGreaterThanSource_Heroic()
		{
			reactor.Energy = 2;
			shield.Energy = 0;
			shield.FillToCapacity(true);
			Assert.AreEqual(0, reactor.Energy);
			Assert.AreEqual(3, shield.Energy);
		}

		[TestMethod]
		public void Test_FillToCapacity_RemainingCapacityLessThanSource_Heroic()
		{
			reactor.Energy = 3;
			shield.Energy = 2;
			shield.FillToCapacity(true);
			Assert.AreEqual(2, reactor.Energy);
			Assert.AreEqual(4, shield.Energy);
		}

		[TestMethod]
		public void Test_FillToCapacity_SourceEmpty_Heroic()
		{
			reactor.Energy = 0;
			shield.Energy = 2;
			shield.FillToCapacity(true);
			Assert.AreEqual(0, reactor.Energy);
			Assert.AreEqual(2, shield.Energy);
		}

		[TestMethod]
		public void Test_FillToCapacity_AtCapacity_Heroic()
		{
			reactor.Energy = 2;
			shield.Energy = 3;
			shield.FillToCapacity(true);
			Assert.AreEqual(2, reactor.Energy);
			Assert.AreEqual(3, shield.Energy);
		}

		[TestMethod]
		public void Test_ShieldThroughAttack_IneffectiveShields_DamageGreaterThanBonusShield()
		{
			shield.IneffectiveShields = true;
			shield.Energy = 5;
			shield.BonusShield = 2;
			var result = shield.ShieldThroughAttack(3);
			Assert.AreEqual(2, result);
			Assert.AreEqual(0, shield.BonusShield);
			Assert.AreEqual(5, shield.Energy);
		}

		[TestMethod]
		public void Test_ShieldThroughAttack_IneffectiveShields_DamageLessThanBonusShield()
		{
			shield.IneffectiveShields = true;
			shield.Energy = 5;
			shield.BonusShield = 2;
			var result = shield.ShieldThroughAttack(1);
			Assert.AreEqual(1, result);
			Assert.AreEqual(1, shield.BonusShield);
			Assert.AreEqual(6, shield.Energy);
		}

		[TestMethod]
		public void Test_ShieldThroughAttack_ReversedShields_HasBonusShields()
		{
			shield.ReversedShields = true;
			shield.Energy = 3;
			shield.BonusShield = 2;
			var result = shield.ShieldThroughAttack(6);
			Assert.AreEqual(-1, result);
			Assert.AreEqual(0, shield.BonusShield);
			Assert.AreEqual(0, shield.Energy);

		}

		[TestMethod]
		public void Test_ShieldThroughAttack_ReversedShields_NoBonusShields()
		{
			shield.ReversedShields = true;
			shield.Energy = 3;
			shield.BonusShield = 0;
			var result = shield.ShieldThroughAttack(6);
			Assert.AreEqual(-3, result);
			Assert.AreEqual(0, shield.BonusShield);
			Assert.AreEqual(0, shield.Energy);
		}

		[TestMethod]
		public void Test_ShieldThroughAttack_WorkingShieldsGreaterThanAttack_NoBonusShields()
		{
			shield.Energy = 3;
			shield.BonusShield = 0;
			var result = shield.ShieldThroughAttack(2);
			Assert.AreEqual(2, result);
			Assert.AreEqual(0, shield.BonusShield);
			Assert.AreEqual(1, shield.Energy);
		}

		[TestMethod]
		public void Test_ShieldThroughAttack_WorkingShieldsLessThanAttack_NoBonusShields()
		{
			shield.Energy = 3;
			shield.BonusShield = 0;
			var result = shield.ShieldThroughAttack(5);
			Assert.AreEqual(3, result);
			Assert.AreEqual(0, shield.BonusShield);
			Assert.AreEqual(0, shield.Energy);
		}

		[TestMethod]
		public void Test_ShieldThroughAttack_WorkingShieldsGreaterThanAttack_HasBonusShields()
		{
			shield.Energy = 4;
			shield.BonusShield = 2;
			var result = shield.ShieldThroughAttack(5);
			Assert.AreEqual(5, result);
			Assert.AreEqual(0, shield.BonusShield);
			Assert.AreEqual(1, shield.Energy);
		}

		[TestMethod]
		public void Test_ShieldThroughAttack_WorkingShieldsLessThanAttack_HasBonusShields()
		{
			shield.Energy = 4;
			shield.BonusShield = 2;
			var result = shield.ShieldThroughAttack(7);
			Assert.AreEqual(6, result);
			Assert.AreEqual(0, shield.BonusShield);
			Assert.AreEqual(0, shield.Energy);
		}
	}
}
