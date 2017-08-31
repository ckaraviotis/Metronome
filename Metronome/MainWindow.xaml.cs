using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Metronome
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TheMetronome metronome;

        public MainWindow()
        {
            InitializeComponent();
            metronome = new TheMetronome(60);
            metronome.AllowRepeats = false;
            this.DataContext = metronome;

            this.KeyDown += new KeyEventHandler(Tempo_Trigger);
        }

        private void Tempo_Trigger(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                Stop_Click(null, null);
                Start_Click(null, null);
            }
            else if (e.Key == Key.Escape)
            {
                Stop_Click(null, null);
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            int bpm = Int32.Parse(Tempo.Text);
            metronome.play(bpm);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            metronome.stop();
        }

  
    }
}
