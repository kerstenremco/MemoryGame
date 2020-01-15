using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Memorygame
{
    /// <summary>
    /// Interaction logic for Highscores.xaml
    /// Highscore scherm
    /// </summary>
    public partial class Highscores : Window
    {
        string padHighscores;
        /// <summary>
        /// Constructor voor Highscores class.
        /// Leest highscore bestand uit, en vult het scherm met highscores
        /// </summary>
        /// <param name="_padHighscores">Pad naar highscore bestand</param>
        public Highscores(string _padHighscores)
        {
            InitializeComponent();
            padHighscores = _padHighscores;
            // haal scores op en sla op in dictionary _scores
            Dictionary<String, Int32> _scores = highscoreUitlezen();
            // int voor margin boven, wordt per rij verhoogd
            int marginboven = 0;
            int positie = 0;
            // int met laatst uitgelezen score. Wordt gebruikt om positie (medaille) te bepalen.
            int laatsteScrore = 0;
            // loop dic _scores af
            foreach (KeyValuePair<String, Int32> _item in _scores)
            {
                // als score voorgaande speler niet gelijk is, dan 1 positie verhogen.
                if (_item.Value != laatsteScrore)
                    positie++;
                // Vul scherm met rijen met hierin 2 labels. Label 1 voor naam (key) en Label2 voor score (value)
                laatsteScrore = _item.Value;
                Label _label = new Label();
                Label _label2 = new Label();
                _label.Content = _item.Key;
                _label2.Content = _item.Value;
                _label.Margin = new Thickness(40, marginboven, 0,0);
                _label2.Margin = new Thickness(550, marginboven, 0, 0);
                // als positie op 1, 2 of 3 staat geef dan een breedte van 400, anders 500. Dit om ruimte te maken voor medaille welke voor label1 wordt geplaatst.
                _label.Width = positie < 4 ? 400 : 500;
                _label2.Width = 100;
                // pas kleur toe aan de hand van positie
                _label.Foreground = positie == 1 ? new SolidColorBrush(Colors.Gold) : positie == 2 ? new SolidColorBrush(Colors.Silver) : positie == 3 ? new SolidColorBrush(Colors.SaddleBrown) : new SolidColorBrush(Colors.White);
                _label2.Foreground = positie == 1 ? new SolidColorBrush(Colors.Gold) : positie == 2 ? new SolidColorBrush(Colors.Silver) : positie == 3 ? new SolidColorBrush(Colors.SaddleBrown) : new SolidColorBrush(Colors.White);
                _label.FontSize = 30;
                _label2.FontSize = 30;
                _label.HorizontalAlignment = HorizontalAlignment.Left;
                _label2.HorizontalContentAlignment = HorizontalAlignment.Right;
                if (positie < 4)
                { 
                    // voor positie 1,2 en 3 pas medaille toe
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("/medaille/" + positie + ".png", UriKind.Relative));
                    image.Height = 40;
                    image.Margin = new Thickness(0, marginboven, 0, 0);
                    image.HorizontalAlignment = HorizontalAlignment.Left;
                    image.VerticalAlignment = VerticalAlignment.Top;
                    lijst.Children.Add(image);
                }
                // voeg gegenereerde labels toe aan lijst in XAML bestand
                lijst.Children.Add(_label);
                lijst.Children.Add(_label2);
                // verhoog marginboven met 40 zodat volgende rij er netjes onder komt te staan
                marginboven += 40;
            }
        }

        /// <summary>
        /// Lees Highscores uit
        /// </summary>
        /// <returns>Dictionary met naam(key) en score(value)</returns>
        public Dictionary<string, int> highscoreUitlezen()
        {
            string _naam = string.Empty;
            Dictionary<string, int> _highScores = new Dictionary<string, int>();
            // lees highscore bestand uit, en voer per lijn actie uit
            foreach (string line in File.ReadLines(padHighscores, Encoding.UTF8))
            {
                // als string naam leeg is, zet naam is string en voer volgende lijn uit
                if (_naam == string.Empty)
                {
                    _naam = line;
                    continue;
                }
                // als er een naam bekend is, dan zitten we nu op een score lijn. Lees deze uit en voeg naam + highscore toe aan dic
                else
                {
                    _highScores.Add(_naam, Convert.ToInt32(line));
                    // maak string naam weer leeg
                    _naam = string.Empty;
                }
            }
            return _highScores;
        }
        /// <summary>
        /// Sluit scherm als op exit wordt geklikt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
