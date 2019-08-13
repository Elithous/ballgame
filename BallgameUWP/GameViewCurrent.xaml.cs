using Ballgame;
using BallgameUWP.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BallgameUWP
{
    public sealed partial class GameViewCurrent : Page
    {
        DataFetch data = new DataFetch();
        Game game;
        Timer updateTimer;

        public GameViewCurrent()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            game = (Game)e.Parameter;

            var autoEvent = new AutoResetEvent(false);
            updateTimer = new Timer(x => UpdateViewAsync(), autoEvent, 0, 10000 );

            UpdateView(game, data);
        }

        public async void UpdateViewAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UpdateView(game, data);
            });
        }

        public void UpdateView(Game game, DataFetch data)
        {
            LinescoreGame linescore = data.GetLinescore(game.Game_data_directory);

            TitleTeam1.Text = game.Away_team_name;
            TitleTeam2.Text = game.Home_team_name;

            ScoreControl control = new ScoreControl();
            ScoreContainer.Children.Clear();
            ScoreContainer.Children.Add(control);

            control.SetScores(linescore);

            if (game.Status == "Pre-Game" || game.Status == "Preview")
            {
                GameTime.Text = string.Format("Game Starts at {0}", linescore.Time + linescore.Time_zone);
            }
            else 
            {
                GameTime.Text = (linescore.Venue + ", " + linescore.Location + ": " + linescore.Time + linescore.Time_zone);

                FieldControl field = new FieldControl();
                FieldContainer.Children.Clear();
                FieldContainer.Children.Add(field);

                GameEvents events = data.GetGameEvents(game.Game_data_directory);
                field.SetData(linescore, events);
            }

            TextUpdated.Text = DateTime.Now.ToLocalTime().ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            updateTimer.Dispose();
            this.Frame.GoBack();
        }
    }
}
