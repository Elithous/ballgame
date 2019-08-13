using Ballgame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BallgameUWP.UserControls
{
    public sealed partial class GameOption : UserControl
    {
        public Game Game { get; set; }

        public GameOption(Game g)
        {
            this.InitializeComponent();

            SetupScreen(g);
            Game = g;
        }

        public void SetupScreen(Game g)
        {
            string status = g.Status;
            if (status == "In Progress") { status = (g.Top_inning == "Y" ? "Top" : "Btm") + " " + g.Inning; }
            if (status == "Preview") { status = g.Home_time + g.Home_time_zone; }
            GameStatus.Text = status;

            T1Name.Text = g.Away_name_abbrev;
            T1Hits.Text = g.Away_team_hits ?? "";
            T1Runs.Text = g.Away_team_runs ?? "";
            T1Errors.Text = g.Away_team_errors ?? "";

            T2Name.Text = g.Home_name_abbrev;
            T2Hits.Text = g.Home_team_hits ?? "";
            T2Runs.Text = g.Home_team_runs ?? "";
            T2Errors.Text = g.Home_team_errors ?? "";
        }
    }
}
