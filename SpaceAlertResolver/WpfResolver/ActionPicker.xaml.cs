using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using BLL;

namespace WpfResolver
{
	public partial class ActionPicker : Window
	{
		public IList<Player> Players { get; private set; }

		public ActionPicker()
		{
			InitializeComponent();
			//Players = new List<Player>();

			Players = new[]
			{
				new Player
				{
					Actions =
						new List<PlayerAction>
						{
							PlayerAction.None,
							PlayerAction.ChangeDeck,
							PlayerAction.B,
							PlayerAction.ChangeDeck,
							PlayerAction.A,
							PlayerAction.A,
							PlayerAction.A,
							PlayerAction.A,
							PlayerAction.A,
							PlayerAction.A
						},
					Index = 1
				},
				new Player
				{
					Actions =
						new List<PlayerAction>
						{
							PlayerAction.MoveRed,
							PlayerAction.ChangeDeck,
							PlayerAction.C,
							PlayerAction.ChangeDeck,
							PlayerAction.None,
							PlayerAction.None,
							PlayerAction.C,
							PlayerAction.BattleBots
						},
					Index = 2
				},
				new Player
				{
					Actions =
						new List<PlayerAction> {PlayerAction.None, PlayerAction.C, PlayerAction.None, PlayerAction.None, PlayerAction.C}
				}
			};
		}

		private void FifthPlayerCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			FifthPlayerTextBox.IsEnabled = FifthPlayerCheckBox.IsChecked ?? false;
		}

		private void FourthPlayerCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			FourthPlayerTextBox.IsEnabled = FourthPlayerCheckBox.IsChecked ?? false;
		}
	}
}
