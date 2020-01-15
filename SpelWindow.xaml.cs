using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Memorygame
{
    /// <summary>
    /// Interaction logic for SpelWindow.xaml
    /// Class kan worden aangeroepen met 2 construters
    /// 1. Indien bestandmap aanwezig is, geef dan een bool mee of het een hervat spel betreft, en een array met daarin de paden
    /// 2. Indien de map niet aanwezig is, start dan via de cunstructor zonder parameters
    /// </summary>
    public partial class SpelWindow : Window
    {
        bool mapAanwezig = false;
        string[] paden;
        private string _Speler1 = "Speler 1";
        private string _Speler2 = "Speler 2";
        /// <summary>
        /// Spel starten met aanwezige bestandenmap
        /// </summary>
        /// <param name="_spelHerstarten">Spel herstarten? Geef TRUE mee</param>
        /// <param name="_paden">Array met paden, in volgorde [0]Save bestand, [1]instellingen bestand, [2]highscore bestand</param>
        public SpelWindow(bool _spelHerstarten, string[] _paden)
        {
            InitializeComponent();
            DataContext = this;
            paden = _paden;
            // mappen zijn meegegeven, dus zet mapAanwezig op true
            mapAanwezig = true;
            // als _spelHerstarten true is, start spel direct door
            if (_spelHerstarten)
            {
                Spel spel = new Spel(paden);
                this.Content = spel;
            }
        }

        /// <summary>
        /// SpelWindow zonder bestandenmap
        /// </summary>
        public SpelWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Naam speler 1
        /// </summary>
        public string Speler1
        {
            get
            {
                return _Speler1;
            }
            set
            {
                _Speler1 = value;
            }
        }
        /// <summary>
        /// Naam Speler 2
        /// </summary>
        public string Speler2
        {
            get
            {
                return _Speler2;
            }
            set
            {
                _Speler2 = value;
            }
        }
        /// <summary>
        /// Als er op spel starten wordt geklikt, start spel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spel_starten(object sender, RoutedEventArgs e)
        {
            // conroleer of map aanwezig is
            if (mapAanwezig)
            {
                // start een nieuw spel met paden en spelers namen
                Spel spel = new Spel(paden, Speler1, Speler2);
                this.Content = spel;
            } else
            {
                // start spel zonder paden met alleen namen
                Spel spel = new Spel(Speler1, Speler2);
                this.Content = spel;
            }
            
        }
        /// <summary>
        /// Als er op sluiten wordt geklikt, sluit dit venster en roep Main window terug.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void schermSluiten (object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
        }
    }
}
