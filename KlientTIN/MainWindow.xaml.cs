using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KlientTIN
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Stream stream;

        private static Boolean listSuccess = false;

        private static Timer realTimer;

        private static Boolean fileEnd = false;
        private static double playHead = 0;
        private static double playMax = 10;

        public MainWindow()
        {

            InitializeComponent();

            var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            var options = new string[]
            {
                // VLC options can be given here. Please refer to the VLC command line documentation.
            };

            this.VlcControl.SourceProvider.CreatePlayer(vlcLibDirectory, options);

            
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            updateFilmList();
        }

        private void ButtonPlayPause_Click(object sender, RoutedEventArgs e)
        {
            switch(ButtonPlayPause.Content)
            {
                case "▶":
                    ButtonPlayPause.Content = "⏸";
                    this.VlcControl.SourceProvider.MediaPlayer.Play();
                    realTimer.Enabled = true;
                    break;
                case "⏸":
                    ButtonPlayPause.Content = "▶";
                    this.VlcControl.SourceProvider.MediaPlayer.Pause();
                    realTimer.Enabled = false;
                    break;
            }


        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            //this.VlcControl.SourceProvider.MediaPlayer.Stop();
        }

        private async void ComboBoxSelectFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StreamerService.StreamerServiceClient client = new StreamerService.StreamerServiceClient();

            try
            {
                if(listSuccess)
                {
                    double max = client.GetFilmLen((string)ComboBoxSelectFile.SelectedItem);
                    SliderPlayBar.Maximum = max;
                    LabelTimeMax.Content = TimeSpan.FromSeconds(max).ToString("hh':'mm':'ss");
                    stream = await client.GetStreamAsync((string)ComboBoxSelectFile.SelectedItem);

                    string path = @"D:\Videos\vid\down\" + ComboBoxSelectFile.Text;

                    if (File.Exists(path))
                        File.Delete(path);

                    using (FileStream fs = File.Create(path))
                    {
                        stream.CopyTo(fs);
                    }

                    Uri src = new Uri(path);
                    
                    //this.VlcControl.SourceProvider.MediaPlayer.Play(stream);
                    this.VlcControl.SourceProvider.MediaPlayer.SetMedia(src);
                    this.VlcControl.SourceProvider.MediaPlayer.Play();
                    this.VlcControl.SourceProvider.MediaPlayer.EndReached += MediaPlayer_EndReached;

                    SetTimer();

                    ButtonPlayPause.Content = "⏸";
                } else
                {
                    SliderPlayBar.Maximum = 1;
                    LabelTimeMax.Content = "Błąd!";
                }
            }
            catch
            {
                SliderPlayBar.Maximum = 1;
                LabelTimeMax.Content = "--:--:--";
            } finally
            {
                LabelTimeCurrent.Content = "00:00:00";
                SliderPlayBar.Value = 0;
                ButtonPlayPause.Content = "▶";
            }
        }

        private void MediaPlayer_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
            realTimer.Enabled = false;
        }

        private void SliderPlayBar_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            //LabelTimeCurrent.Content = TimeSpan.FromSeconds(SliderPlayBar.Value).ToString("hh':'mm':'ss");
            //LabelTimeCurrent.Content = SliderPlayBar.Value;
        }

        private void SliderPlayBar_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            //LabelTimeCurrent.Content = SliderPlayBar.Value;
        }

        private void SliderPlayBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //LabelTimeCurrent.Content = SliderPlayBar.Value;
            LabelTimeCurrent.Content = TimeSpan.FromSeconds(SliderPlayBar.Value).ToString("hh':'mm':'ss");
            this.VlcControl.SourceProvider.MediaPlayer.Time = (long)SliderPlayBar.Value*1000;
        }

        /* SHARED METHODS */

        public async void updateFilmList()
        {
            StreamerService.StreamerServiceClient client = new StreamerService.StreamerServiceClient();

            List<String> filmList;

            try
            {
                String[] films = await client.GetFilmsAsync();
                filmList = films.ToList<String>();
                listSuccess = true;
            }
            catch
            {
                filmList = new List<String>();
                filmList.Add("BŁĄD");
                filmList.Add("POBIERANIA");
                filmList.Add("LISTY");
                listSuccess = false;
                LabelTimeMax.Content = "Błąd!";
            }

            ComboBoxSelectFile.ItemsSource = filmList;
        }

        private void SetTimer()
        {
            realTimer = new Timer(1000);
            realTimer.AutoReset = true;
            realTimer.Elapsed += RealTimer_Elapsed;
            realTimer.Enabled = true;
        }

        private void RealTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //
        }
    }
}
