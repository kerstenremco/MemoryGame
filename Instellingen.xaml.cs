using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Instellingen class kan op 2 manieren worden aangeroepen:
    /// 1. zonder parameters, standaard instellingen worden ingeladen. Gebruik deze indien het instellingenbestand leeg is
    /// 2. Met 1 string waarin het pad instellingenbesand staat. Dit bestand wordt dan uitgelezen.
    /// </summary>
    public partial class Instellingen : Window, INotifyPropertyChanged
    {
        /// hieronder staan variabelen welke gevuld worden met instellingen. Alle aan roepen via get set methode. Ivm wijzigen op scherm dm.v. PropertyGewijzigd .
        int _breedte;
        int _lengte;
        int _aantalSets;
        string _thema;
        // pad instellingenbestand
        string padInstellingen;
        // arrat met instellingen. Wordt gevuld via uitlezen methode.
        string[] instellingen;

        /// <summary>
        /// Wijzig breedte int en voer propertyGewijzigd uit
        /// </summary>
        public int breedte
        {
            get
            {
                return _breedte;
            }
            set
            {
                _breedte = value;
                PropertyGewijzigd();
            }
        }

        /// <summary>
        /// Wijzig lengte int en voer propertyGewijzigd uit
        /// </summary>
        public int lengte
        {
            get
            {
                return _lengte;
            }
            set
            {
                _lengte = value;
                PropertyGewijzigd();
            }
        }


        /// <summary>
        /// Wijzig het aantal te raden sets
        /// Verifieer eerst of het aantal sets geldig is, anders foutmelding weergeven met verdere instructies
        /// </summary>
        public int aantalSets
        {
            get { return _aantalSets; }
            set
            {
                if (value < 1)
                    value = _aantalSets;
                if (value > (breedte * lengte) / 2)
                {
                    value = _aantalSets;
                    MessageBox.Show("Je hebt een grid van " + breedte + " * " + lengte + ". Hierin zijn maximaal " + Convert.ToString((breedte * lengte) / 2) + " combinatie's mogelijk.");
                }
                if (((breedte * lengte - (breedte * lengte % 2)) % value) != 0)
                {
                    value = _aantalSets;
                    MessageBox.Show("Dit aantal sets is niet mogelijk met het huidige grid");
                }
                _aantalSets = value;
                PropertyGewijzigd();
            }
        }

        /// <summary>
        /// String met hierin het thema
        /// </summary>
        public string thema
        {
            get
            {
                return _thema;
            }
            set
            {
                _thema = value;
                PropertyGewijzigd();
            }
        }



        /// <summary>
        /// Controleerd of instellingenbestand is ingevuld en leest deze in in instellingen stringArray.
        /// Indien bestand niet bestaat worden standaard instellingen toegepast dmv standaartInstellingen methode.
        /// </summary>
        public Instellingen(string _padInstelingen)
        {
            InitializeComponent();
            DataContext = this;
            padInstellingen = _padInstelingen;
            // probeer of instellingen bestand leesbaar is en zet in instellingen array. Indien niet mogelijk geef een foutmelding.
            try { instellingen = File.ReadAllLines(padInstellingen); }
            catch (Exception) { MessageBox.Show("Het instellingenbestand is niet beschikbaar. Probeer het over enkele seconden opnieuw of probeer het spel opnieuw te starten."); return; };
            // als instellingen bestand leeg is, dan standaard instellingen toe passen
            if (instellingen.Length == 0)
            {
                standaardInstellingen();
            }
            // als array vullen gelukt is, vul instellingen variabelen in adv uitgelezen array
            else
            {
                changeBreedteLengte(Convert.ToInt32(instellingen[0]), Convert.ToInt32(instellingen[1]));
                aantalSets = Convert.ToInt32(instellingen[2]);
                thema = instellingen[3];
            }
        }

        /// <summary>
        /// Indien geen bestanden aanwezig, geef standaard instellingen mee
        /// </summary>
        public Instellingen()
        {
            standaardInstellingen();
        }

        /// <summary>
        /// Vul standaard instellingen in
        /// </summary>
        private void standaardInstellingen()
        {
            changeBreedteLengte(4, 4);
            aantalSets = 1;
            thema = "Smiley's";
        }

        /// <summary>
        /// Genereer string met hierin de instellingen
        /// </summary>
        /// <returns>string Array in volgorde: [0] = breedte [1] = lengte [2] = aantal sets te raden [3] = thema</returns>
        public string[] ophalen()
        {
            string[] _return = new string[5];
            _return[0] = Convert.ToString(breedte);
            _return[1] = Convert.ToString(lengte);
            _return[2] = Convert.ToString(aantalSets);
            _return[3] = thema;
            return _return;
        }


        /// <summary>
        /// Verander breedte en lengte variabelen en pas string aan tbv instellingenscherm
        /// Bereken eerst of grid mogelijk is icm aantal sets
        /// </summary>
        /// <param name="_breedte">Breedte grid</param>
        /// <param name="_lengte">Lengte Grid</param>
        private void changeBreedteLengte(int _breedte, int _lengte)
        {
            if ((_breedte * _lengte) / 2 < aantalSets)
            {
                MessageBox.Show("Dit Grid kan niet gekozen worden in combinatie met het aantal minimale te raden combo's van " + Convert.ToString(aantalSets));
                return;
            }
            breedte = _breedte;
            lengte = _lengte;
        }

        /// <summary>
        /// event voor update propery indien gewijzigd
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Update property als content in variabele is veranderd
        /// </summary>
        /// <param name="propertyName">De property met een nieuwe waarde</param>
        protected void PropertyGewijzigd([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);

            }
        }

        /// <summary>
        /// Als er onder instellingen voor grid op 4*4 wordt geklikt, verander deze instellingen dmv changeBreedteLengte methode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setKlik4(object sender, RoutedEventArgs e)
        {
            changeBreedteLengte(4, 4);
        }
        /// <summary>
        /// Als er onder instellingen voor grid op 5*5 wordt geklikt, verander deze instellingen dmv changeBreedteLengte methode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setKlik5(object sender, RoutedEventArgs e)
        {
            changeBreedteLengte(5, 5);
        }
        /// <summary>
        /// Als er onder instellingen voor grid op 6*6 wordt geklikt, verander deze instellingen dmv changeBreedteLengte methode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setKlik6(object sender, RoutedEventArgs e)
        {
            changeBreedteLengte(6, 6);
        }
        /// <summary>
        /// Als er onder instellingen voor thema op smileys wordt geklikt, verander deze instellingen in array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void themaKlikSmileys(object sender, RoutedEventArgs e)
        {
            thema = "Smiley's";
        }
        /// <summary>
        /// Als er onder instellingen voor thema op dieren wordt geklikt, verander deze instellingen in array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void themaKlikDieren(object sender, RoutedEventArgs e)
        {
            thema = "Dieren";
        }
        /// <summary>
        /// Als er onder instellingen voor thema op voedsel wordt geklikt, verander deze instellingen in array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void themaKlikVoedsel(object sender, RoutedEventArgs e)
        {
            thema = "Voedsel";
        }

        /// <summary>
        /// Functie als er op instellingen opslaan knop wordt geklikt.
        /// schijf instellingen weg in volgorde: breedte, lengte, aantalSets, Thema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Opslaan(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(padInstellingen, string.Format("{0}\n{1}\n{2}\n{3}", lengte, breedte, aantalSets, thema));
            this.Close();
        }
    }
}