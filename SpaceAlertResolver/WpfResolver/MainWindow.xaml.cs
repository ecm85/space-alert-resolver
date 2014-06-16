using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using BLL;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.External.Minor.White;
using BLL.Threats.External.Serious.White;
using BLL.Threats.Internal;
using BLL.Threats.Internal.Minor.White;
using BLL.Threats.Internal.Serious.White;
using BLL.Tracks;

namespace WpfResolver
{
	//TODO: UI: Don't allow same track multiple times
	//TODO: UI: Don't allow same threat multiple times
	//TODO: UI: Don't allow to click 'remove all threats' unless there are threats
	//TODO: UI: Don't allow to click 'remove selected threats' unless there are threats selected
	//TODO: UI: Don't allow to click 'add new threat' if haven't picked a track (if applicable), a threat, and a time appears
	//TODO: UI: Don't allow multiple internal or external threats at the same time
	//TODO: UI: Don't allow 'Resolve' unless at least one threat is picked, actions are picked, and all 4 tracks are picked
	//TODO: UI: Stop relying on the nullability of track in ThreatInGame - make two lists of threats?
	//TODO: UI: Format output better

	public partial class MainWindow
	{
		private readonly IDictionary<ThreatType, IList<Threat>> threatsByType;
		private readonly ObservableCollection<ThreatInGame> threatsInGame;
		private readonly SittingDuck sittingDuck;// = new SittingDuck();

		public MainWindow()
		{
			threatsInGame = new ObservableCollection<ThreatInGame>();
			threatsByType = new Dictionary<ThreatType, IList<Threat>>
			{
				{ThreatType.MinorExternal, GetThreats(typeof(MinorWhiteExternalThreat))},
				{ThreatType.SeriousExternal, GetThreats(typeof(SeriousWhiteExternalThreat))},
				{ThreatType.MinorInternal, GetThreats(typeof(MinorWhiteInternalThreat))},
				{ThreatType.SeriousInternal, GetThreats(typeof(SeriousWhiteInternalThreat))}
			};
			var tracks = EnumFactory.All<TrackConfiguration>()
				.Select(trackConfiguration => new Track
				{
					Name = trackConfiguration.DisplayName(),
					TrackConfiguration = trackConfiguration
				})
				.ToList();
			InitializeComponent();
			DataContext = this;
			RedTrackPicker.ItemsSource = WhiteTrackPicker.ItemsSource = BlueTrackPicker.ItemsSource = InternalTrackPicker.ItemsSource = tracks;
			NewThreatZonePicker.ItemsSource = EnumFactory.All<ZoneLocation>();
			MinorExternalRadioButton.IsChecked = true;
			SelectedThreatList.ItemsSource = threatsInGame;
		}

		private static List<Threat> GetThreats(Type threatType)
		{
			var assembly = threatType.Assembly;
			var types = assembly.GetTypes().Where(type => !type.IsAbstract && threatType.IsAssignableFrom(type)).ToList();
			return types.Select(type => new Threat
			{
				Type = type,
				Name = type.GetMethod("GetDisplayName", BindingFlags.Static | BindingFlags.Public).Invoke(null, null) as string
			}).ToList();
		}

		private void ResolveGame(object sender, RoutedEventArgs routedEventArgs)
		{
			var actionLists = new List<string> { FirstPlayerTextBox.Text, SecondPlayerTextBox.Text, ThirdPlayerTextBox.Text };
			if (FourthPlayerCheckBox.IsChecked.HasValue && FourthPlayerCheckBox.IsChecked.Value)
				actionLists.Add(FourthPlayerTextBox.Text);
			if (FifthPlayerCheckBox.IsChecked.HasValue && FifthPlayerCheckBox.IsChecked.Value)
				actionLists.Add(FifthPlayerTextBox.Text);
			var players = actionLists.Select(CreatePlayer).ToList();
			var externalThreats = threatsInGame
				.Where(threat => threat.Zone != null)
				.Select(threat => Activator.CreateInstance(
					threat.Type,
					threat.TimeAppears,
					threat.Zone.Value,
					sittingDuck))
				.Cast<ExternalThreat>()
				.ToList();
			//var externalTracks = new[]
			//{
			//	new ExternalTrack(((Track)RedTrackPicker.SelectedItem).TrackConfiguration, sittingDuck.RedZone),
			//	new ExternalTrack(((Track)WhiteTrackPicker.SelectedItem).TrackConfiguration, sittingDuck.WhiteZone),
			//	new ExternalTrack(((Track)BlueTrackPicker.SelectedItem).TrackConfiguration, sittingDuck.BlueZone)
			//};
			var internalThreats = threatsInGame
				.Where(threat => threat.Zone == null)
				.Select(threat => Activator.CreateInstance(
					threat.Type,
					threat.TimeAppears,
					sittingDuck))
				.Cast<InternalThreat>()
				.ToList();
			//var internalTrack = new InternalTrack(((Track)InternalTrackPicker.SelectedItem).TrackConfiguration);

			sittingDuck.SetPlayers(players);
			Game game = null; //new Game(sittingDuck, externalThreats, externalTracks, internalThreats, internalTrack, players);

			var currentTurn = 0;
			try
			{
				for (currentTurn = 0; currentTurn < Game.NumberOfTurns; currentTurn++)
					game.PerformTurn();
			}
			catch (LoseException loseException)
			{
				StatusTextBlock.Text += string.Format("Killed on turn {0} by: {1}", currentTurn, loseException.Threat);
			}

			StatusTextBlock.Text += string.Format("Damage Taken:\r\nBlue: {0}\r\nRed: {1}\r\nWhite: {2}",
				sittingDuck.BlueZone.TotalDamage,
				sittingDuck.RedZone.TotalDamage,
				sittingDuck.WhiteZone.TotalDamage);
			//StatusTextBlock.Text += string.Format("Threats killed: {0}. Threats survived: {1}",
			//	game.AllExternalThreats.Count(threat => threat.IsDefeated) + game.AllInternalThreats.Count(threat => threat.IsDefeated),
			//	game.AllExternalThreats.Count(threat => threat.IsSurvived) + game.AllInternalThreats.Count(threat => threat.IsSurvived));
			StatusTextBlock.Text += string.Format("Total points: {0}", game.TotalPoints);
			foreach (var zone in sittingDuck.Zones)
			{
				foreach (var token in zone.AllDamageTokensTaken)
					StatusTextBlock.Text += string.Format("{0} damage token taken in zone {1}. Still damaged: {2}", token, zone.ZoneLocation, zone.CurrentDamageTokens.Contains(token));
			}
		}

		private Player CreatePlayer(string actionList)
		{
			return new Player
			{
				Actions = actionList.Select(CreateActionFromCode).ToList()
			};
		}

		private PlayerAction CreateActionFromCode(char action)
		{
			switch (action)
			{
				case 'a':
					return PlayerAction.A;
				case 'b':
					return PlayerAction.B;
				case 'c':
					return PlayerAction.C;
				case '<':
					return PlayerAction.MoveRed;
				case '>':
					return PlayerAction.MoveBlue;
				case 'd':
					return PlayerAction.ChangeDeck;
				case 'o':
					return PlayerAction.BattleBots;
				case 'A':
					return PlayerAction.HeroicA;
				case 'B':
					return PlayerAction.HeroicB;
				case 'O':
					return PlayerAction.HeroicBattleBots;
				case '1':
					return PlayerAction.TeleportWhiteLower;
				case '2':
					return PlayerAction.TeleportWhiteLower;
				case '3':
					return PlayerAction.TeleportBlueLower;
				case '4':
					return PlayerAction.TeleportWhiteUpper;
				case '5':
					return PlayerAction.TeleportWhiteUpper;
				case '6':
					return PlayerAction.TeleportBlueUpper;
				case ' ':
					return PlayerAction.None;
			}
			return PlayerAction.None;
		}

		private void SeriousInternalRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			SetThreatPickerItemSource(ThreatType.SeriousInternal);
		}

		private void MinorInternalRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			SetThreatPickerItemSource(ThreatType.MinorInternal);
		}

		private void SeriousExternalRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			SetThreatPickerItemSource(ThreatType.SeriousExternal);
		}

		private void MinorExternalRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			SetThreatPickerItemSource(ThreatType.MinorExternal);
		}

		private void SetThreatPickerItemSource(ThreatType threatType)
		{
			NewThreatPicker.ItemsSource = threatsByType[threatType];
			switch (threatType)
			{
				case ThreatType.MinorExternal:
				case ThreatType.SeriousExternal:
					NewThreatZonePicker.IsEnabled = true;
					NewThreatZonePicker.Text = "";
					break;
				case ThreatType.MinorInternal:
				case ThreatType.SeriousInternal:
					NewThreatZonePicker.IsEnabled = false;
					NewThreatZonePicker.Text = "internal";
					break;
			}
		}

		private void RemoveAllCurrentThreats_Click(object sender, RoutedEventArgs e)
		{
			threatsInGame.Clear();
		}

		private void AddNewThreatButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedThreat = (Threat)NewThreatPicker.SelectedItem;
			var timeAppears = Int32.Parse(TimeAppearsTextBox.Text);
			var zoneLocation = NewThreatZonePicker.IsEnabled ? (ZoneLocation?)NewThreatZonePicker.SelectedItem : null;
			var zoneLocationString = zoneLocation == null ? "internal" : zoneLocation.ToString();
			threatsInGame.Add(new ThreatInGame
			{
				Name = string.Format(
					"{0} in {1} zone at time {2}",
					selectedThreat.Name,
					zoneLocationString,
					timeAppears),
				Type = selectedThreat.Type,
				TimeAppears = timeAppears,
				Zone = zoneLocation
			});
			TimeAppearsTextBox.Text = "";
			NewThreatPicker.SelectedItem = null;
			NewThreatZonePicker.SelectedItem = null;
		}

		private void RemoveSelectedThreats_Click(object sender, RoutedEventArgs e)
		{
			var selectedThreatsInGame = SelectedThreatList.SelectedItems.Cast<ThreatInGame>().ToList();
			foreach (var item in selectedThreatsInGame)
				threatsInGame.Remove(item);
		}
	}
}
