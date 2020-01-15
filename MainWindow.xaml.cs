using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

namespace Memorygame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // bool of bestanden aanwezig zijn
        bool bestandenAanwezig;
        // bool of highscore bestand is ingevuld
        bool highscoreAanwezig;
        // bool of SAV bestand is ingevuld
        bool savAanwezig;
        // locatie MemoryMap
        string map = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/memorygame/";
        // locatie SAV bestand binnen memory map
        string padSavBestand = "save.sav";
        // locatie Highscore bestand binnen MemoryMap
        string padHighscores = "memory.txt";
        // locatie Instellingen bestand binnen MemoryMap
        string padInstellingen = "instellingen.txt";
        // array met volledige paden benodigde bestanden. Volgorde: [0]Save bestand, [1]instellingen bestand, [2]highscore bestand
        string[] paden = new string[3];

        /// <summary>
        /// Roep hoofdscherm op
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // vul bool bestandenAanwezig aan de hand van uitkomst ControleerMapenBestanden methode
            bestandenAanwezig = controleerMapenBestanden();
            // als de bestanden aanwezig zijn, controleer dan of highscore bestand en sav bestand is ingevuld. Zet volledige paden in array paden
            if (bestandenAanwezig)
            {
                highscoreAanwezig = controleerHighscoresBestand();
                savAanwezig = controleerSav();
                paden[0] = map + padSavBestand;
                paden[1] = map + padInstellingen;
                paden[2] = map + padHighscores;
            }
            
        }



        /// <summary>
        /// Controleer of map bestaat en bestanden bestaan. Zo niet, probeer eerst aan te maken
        /// </summary>
        /// <returns>Bool of map en bestanden bestaan</returns>
        public bool controleerMapenBestanden()
        {
            if (!(Directory.Exists(map)))
            {
                try { Directory.CreateDirectory(map); }
                catch (Exception) { MessageBox.Show("Kan map niet aanmaken. Zorg ervoor dat de map " + map + " bestaat en toegankelijk is. Opslaan en highscores zijn niet beschikbaar"); return false; };
                MessageBox.Show("De bestandenmap is zojuist aangemaakt. Start het spel aub opnieuw op.");
            }
            if (!(File.Exists(map + padSavBestand)))
            {
                try { File.Create(map + padSavBestand); }
                catch (Exception) { MessageBox.Show("Kan SAV bestand niet aanmaken. Zorg ervoor dat " + map + padSavBestand + " bestaat en toegankelijk is. Opslaan is niet beschikbaar"); return false; };
            }
            if (!(File.Exists(map + padHighscores)))
            {
                try { File.Create(map + padHighscores); }
                catch (Exception) { MessageBox.Show("Kan highscore bestand niet aanmaken. Zorg ervoor dat " + map + padHighscores + " bestaat en toegankelijk is. Opslaan is niet beschikbaar"); return false; };
            }
            if (!(File.Exists(map + padInstellingen)))
            {
                try { File.Create(map + padInstellingen); }
                catch (Exception) { MessageBox.Show("Kan instelingen bestand niet aanmaken. Zorg ervoor dat " + map + padInstellingen + " bestaat en toegankelijk is. Opslaan is niet beschikbaar"); return false; };
            }
            return true;
        }

        /// <summary>
        /// Controleer of highscore bestand is ingevuld
        /// </summary>
        /// <returns>True als scores aanwezig zijn, false als er nog geen scores zijn</returns>
        public bool controleerHighscoresBestand()
        {
            if (new FileInfo(map + padHighscores).Length == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Controleer of SAV bestand leeg is of is ingevuld
        /// </summary>
        /// <returns>True als ingevuld, false als leeg</returns>
        public bool controleerSav()
        {
            if (new FileInfo(map + padSavBestand).Length == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Sluit venster als er op Exit wordt geklikt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Als er op continue wordt geklikt, controleer of savbestand aanwezig is adv boolsavAanwezig
        /// Indien ja, start spel met parameter true (spel hervatten) en de benodigde paden
        /// Indien nee, geen melding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Continue(object sender, RoutedEventArgs e)
        {
            if (savAanwezig)
            {
                SpelWindow spelwindow = new SpelWindow(true, paden);
                spelwindow.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Geen opgeslagen spel gevonden!");
            }


        }

        /// <summary>
        /// Als er op nieuw spel wordt geklikt, controleer eerst of er een SAV bestand aanwezig is.
        /// Indien ja, vraag of nieuw spel starten of spel hervatten. Indien nieuw spel herstarten dan eerst SAV bestand leegmaken.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_New_Game(object sender, RoutedEventArgs e)
        {
            if (savAanwezig)
                // sav aanwezig, vragen om spel te herstarten of hervatten
            {
                MessageBoxResult m = MessageBox.Show("Wil je een nieuw spel starten? Het opgeslagen spel wordt gewist", "Opgeslagen bestand gevonden.", MessageBoxButton.YesNo);
                if (m == MessageBoxResult.Yes)
                {
                    // indien nieuw spel starten, eerste sav bestand legen en daarna nieuw spel starten
                    File.WriteAllText(map + padSavBestand, string.Empty);
                    SpelWindow spelwindow = new SpelWindow(false, paden);
                    spelwindow.Show();
                }
                else if (m == MessageBoxResult.No)
                {
                    // Indien hervatten, dan spel hervatten
                    SpelWindow spelwindow = new SpelWindow(true, paden);
                    spelwindow.Show();
                }
            } else
            // geen sav aanwezig, nieuw spel starten
            {
                // controleer of de map aanwezig is
                if (bestandenAanwezig)
                {
                    // indien ja, nieuw spel starten en paden meegeven
                    SpelWindow spelwindow = new SpelWindow(false, paden);
                    spelwindow.Show();
                } else
                {
                    // indien nee, nieuw spel starten zonder paden
                    SpelWindow spelwindow = new SpelWindow();
                    spelwindow.Show();
                }
                
            }
            // verberg main window als nieuw spel wordt herstart
            this.Close();

        }

        /// <summary>
        /// Instellingen venster oproepen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Settings(object sender, RoutedEventArgs e)
        {
            // controller of bestanden aanwezig zijn
            if (bestandenAanwezig)
            {
                // Probeer instellingen bestand te openen
                try { File.ReadAllLines(paden[1]); }
                // indien het niet lukt, geef dan een foutmelding. Dit omdat als het bestand net is aangemaakt het nog gelocked kan zijn
                catch (Exception) { MessageBox.Show("Het instellingenbestand is niet beschikbaar. Probeer het over enkele seconden opnieuw of probeer het spel opnieuw te starten."); return; };
                // maak instance aan voor instellingen en show deze
                Instellingen instellingen = new Instellingen(paden[1]);
                instellingen.Show();
            } else
            {
                MessageBox.Show("Instellingen zijn niet beschikbaar doordat de Memory map met het instellingenbestand mist");
            }
            
        }

        /// <summary>
        /// Highscore scherm oproepen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Highscores(object sender, RoutedEventArgs e)
        {
            // controleer of highscore bestand is ingevuld
            if (highscoreAanwezig)
            {
                // indien ja, maak instance van highscores en show deze.
                Highscores highscores = new Highscores(paden[2]);
                highscores.Show();
            } else
            {
                // indien nee, geef foutmelding
                MessageBox.Show("Highscores zijn niet aanwezig");
            }
            
        }

        /// <summary>
        /// Roep help scherm op
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Click(object sender, MouseButtonEventArgs e)
        {
            Help help = new Help();
            help.Show();
        }
    }
}
