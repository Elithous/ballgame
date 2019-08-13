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
    public sealed partial class FieldControl : UserControl
    {
        public FieldControl()
        {
            this.InitializeComponent();
        }

        public void SetData(LinescoreGame linescore, GameEvents events)
        {
            string pitch="", pitchSpeed="", lastPlay = "";
            int eventInning = (events.Inning.Count == linescore.Inning.Count() ? linescore.Inning.Count() - 1 : events.Inning.Count - 1);
            if (linescore.Inning_state == "Top")
            {
                try{
                pitch = events.Inning[eventInning]?.Top.Atbat.LastOrDefault()?.Pitch?.LastOrDefault()?.Pitch_type ?? "";
                pitchSpeed = events.Inning[eventInning]?.Top.Atbat.LastOrDefault()?.Pitch?.LastOrDefault()?.Start_speed ?? "";
                lastPlay = events.Inning[eventInning]?.Top.Atbat.LastOrDefault()?.Des ?? "";}
                catch{};
            }
            else
            {
                try{
                pitch = events.Inning[eventInning]?.Bottom.Atbat.LastOrDefault()?.Pitch?.LastOrDefault()?.Pitch_type ?? "";
                pitchSpeed = events.Inning[eventInning]?.Bottom.Atbat.LastOrDefault()?.Pitch?.LastOrDefault()?.Start_speed ?? "";
                lastPlay = events.Inning[eventInning]?.Bottom.Atbat.LastOrDefault()?.Des ?? "";}
                catch{};
            }

            var grayBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
            var blackBrush = new SolidColorBrush(Windows.UI.Colors.Black);
            // Set base colors
            BaseFirst.Fill = (linescore.Runner_on_1b ?? "").Length > 0 ? grayBrush : blackBrush;
            BaseSecond.Fill = (linescore.Runner_on_2b ?? "").Length > 0 ? grayBrush : blackBrush;
            BaseThird.Fill = (linescore.Runner_on_3b ?? "").Length > 0 ? grayBrush : blackBrush;

            TextBatting.Text = string.Format("{0} ({1})",
                linescore.Current_batter.First_name + " " + linescore.Current_batter.Last_name, linescore.Current_batter.Avg);
            TextCount.Text = string.Format("{0} - {1}", linescore.Balls, linescore.Strikes);
            TextOut.Text = string.Format("{0}", linescore.Outs);
            TextDeck.Text = string.Format("{2}", (linescore.Runner_on_3b ?? "").Length > 0 ? "*" : " ", (linescore.Runner_on_1b ?? "").Length > 0 ? "*" : " ",
                     linescore.Current_ondeck.First_name + " " + linescore.Current_ondeck.Last_name);

            TextPitching.Text = string.Format("{0} ({1} ERA)", 
                linescore.Current_pitcher.First_name + " " + linescore.Current_pitcher.Last_name, linescore.Current_pitcher.Era);
            TextLastPitch.Text = string.Format("{0} {1}mph", pitch, pitchSpeed);

            TextLastPlay.Text = lastPlay;
        }
    }
}
