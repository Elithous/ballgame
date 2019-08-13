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
    public sealed partial class ScoreControl : UserControl
    {
        public ScoreControl()
        {
            this.InitializeComponent();
        }

        public void SetScores(LinescoreGame linescore)
        {
            Team1Name.Text = linescore.Away_name_abbrev;
            Team2Name.Text = linescore.Home_name_abbrev;

            if (linescore.Linescore.Count < 9)
            {
                for (int i = linescore.Linescore.Count; i < 9; i++)
                {
                    linescore.Linescore.Add(new LinescoreInning() { Inning = (i + 1).ToString() });
                }
            }

            int count = 1;
            foreach (LinescoreInning i in linescore.Linescore)
            {
                TextBlock awayText = new TextBlock();
                TextBlock homeText = new TextBlock();
                awayText.Text += i.Away_inning_runs ?? " ";
                homeText.Text += i.Home_inning_runs ?? " ";

                Root.Children.Add(awayText);
                Root.Children.Add(homeText);

                awayText.FontSize = 25;
                awayText.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(awayText, count);
                Grid.SetRow(awayText, 1);

                homeText.FontSize = 25;
                homeText.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(homeText, count);
                Grid.SetRow(homeText, 2);

                count++;
            }

            Team1Runs.Text = linescore.Away_team_runs;
            Team2Runs.Text = linescore.Home_team_runs;
            Team1Hits.Text = linescore.Away_team_hits;
            Team2Hits.Text = linescore.Home_team_hits;
            Team1Errors.Text = linescore.Away_team_errors;
            Team2Errors.Text = linescore.Home_team_errors;
        }
    }
}
