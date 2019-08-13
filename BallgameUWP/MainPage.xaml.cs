using Ballgame;
using BallgameUWP.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BallgameUWP
{
    public sealed partial class MainPage : Page
    {
        DataFetch df = new DataFetch();
        DateTime date;

        public MainPage()
        {
            this.InitializeComponent();

            date = DateTime.Today;
            List<Game> games = df.GetGames(date);
            DisplayGames(games, date);
        }

        public void DisplayGames(List<Game> games, DateTime date)
        {
            if (date == DateTime.Today) { DayTitle.Text = "Todays Games"; }
            else { DayTitle.Text = "Games of " + date.Date.ToString("MM/dd/yyyy"); }

            GameGrid.Children.Clear();
            for (int i = 0; i < games.Count; i++)
            {
                GameOption option = new GameOption(games[i]);
                GameGrid.Children.Add(option);

                Grid.SetRow(option, i / 4);
                Grid.SetColumn(option, i % 4);

                option.PointerPressed += Option_PointerPressed;
            }
        }

        private void Option_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender.GetType() == typeof(GameOption))
            {
                GameOption option = (GameOption)sender;
                Game game = option.Game;

                if (date == DateTime.Today && game.Status != "Final" && game.Status != "Preview")
                {

                    //Display.DisplayGameData(game, df.GetLinescore(game.Game_data_directory),
                    //                                                            df.GetBoxscore(game.Game_data_directory),
                    //                                                            df.GetGameEvents(game.Game_data_directory),
                    //                                                            df.GetGameCenterGame(game.Game_data_directory));

                    //GameViewCurrent window = new GameViewCurrent();
                    this.Frame.Navigate(typeof(GameViewCurrent), game);
                }
                else if (game.Status == "Final")
                {
                    // These are broken on the main app so I'm not sure how they should be displayed. Display.cs is complex
                    //Display.DisplayFinal(game, df.GetBoxscore(game.Game_data_directory), df.GetLinescore(game.Game_data_directory), df.GetGameCenterGame(game.Game_data_directory));
                }
                else if (game.Status == "Preview")
                {
                    //Display.DisplayPreview(df.GetLinescore(game.Game_data_directory), df.GetGameCenterGame(game.Game_data_directory));
                }
            }
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            date = date.Subtract(TimeSpan.FromDays(1));
            List<Game> games = df.GetGames(date);
            DisplayGames(games, date);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            date = date.Add(TimeSpan.FromDays(1));
            List<Game> games = df.GetGames(date);
            DisplayGames(games, date);
        }
    }
}
