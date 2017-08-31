using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Metronome
{
    class TheMetronome : ViewModelBase
    {
        private SoundPlayer click;
        private int tempo;
        private Timer metronome;
        private Random random;

        // Notes
        private string[] naturals = { "A", "B", "C", "D", "E", "F", "G" };
        private string[] flats = { "A\u266D", "B\u266D", "D\u266D", "E\u266D", "G\u266D" };
        private string[] sharps = { "A\u266F", "C\u266F", "D\u266F", "F\u266F", "G\u266F" };

        List<string> validNotes;

        #region Property variables

        private string _note;
        private bool _includeSharps;
        private bool _includeFlats;
        private bool _allowRepeats;

        #endregion

        public TheMetronome(int tempo)
        {
            random = new Random();

            try
            {
                Assembly a = Assembly.GetExecutingAssembly();
                Stream s = a.GetManifestResourceStream("Metronome.Assets.4d.wav");
                
                click = new SoundPlayer(s);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            this.tempo = tempo;

            metronome = new Timer(1000);
            metronome.Elapsed += MetronomeClick;
            metronome.AutoReset = true;

            validNotes = new List<string>(naturals);
        }

        #region Bindable Properties

        public string Note
        {
            get { return _note; }
            set { Set(ref _note, value); }
        }

        public bool IncludeSharps
        {
            get { return _includeSharps; }
            set
            {
                Set(ref _includeSharps, value);
                if (value)
                {
                    foreach (string s in sharps)
                        validNotes.Add(s);
                }
                else
                {
                    foreach (string s in sharps)
                        validNotes.Remove(s);
                }

            }
        }

        public bool IncludeFlats
        {
            get { return _includeFlats; }
            set
            {
                Set(ref _includeFlats, value);
                if (value)
                {
                    foreach (string s in flats)
                        validNotes.Add(s);
                }
                else
                {
                    foreach (string s in flats)
                        validNotes.Remove(s);
                }
            }
        }

        public bool AllowRepeats
        {
            get { return _allowRepeats; }
            set
            {
                Set(ref _allowRepeats, value);  
            }
        }

        #endregion

        public void play(int tempo)
        {
            this.tempo = tempo;

            double beat = (60000.0 / this.tempo);

            metronome.Interval = beat;
            metronome.Start();
        }

        public void stop()
        {
            metronome.Stop();
        }

        /// <summary>
        /// Event handler for metronome tick. Play our sound and update the note
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void MetronomeClick(Object source, System.Timers.ElapsedEventArgs e)
        {
            int r = random.Next(0, (validNotes.Count - 1));
            string n = validNotes[r];

            if (!AllowRepeats)
            {
                while (n.Equals(Note))
                {
                    r = random.Next(0, (validNotes.Count - 1));
                    n = validNotes[r];
                }
            }
            
            Note = n;

            click.Play();
        }

    }
}
