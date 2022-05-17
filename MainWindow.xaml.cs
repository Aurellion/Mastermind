using System;
using System.Collections.Generic;
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

namespace Mastermind
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Ellipse> ellipses = new List<Ellipse>();
        List<TextBox> textBoxes = new List<TextBox>();
        int[] code = new int[4];//0=Schwarz, 1=Blau, 2=Rot, 3=Grün
        Random rnd = new Random();
        int Runde;
        public MainWindow()
        {
            InitializeComponent();

            //Spalten im Spielfeld erzeugen
            for (int i = 0; i < 5; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(1,GridUnitType.Star);
                Spielfeld.ColumnDefinitions.Add(column);
            }
            Spielregeln.Text = "Spielregeln:\n" +
                               "Ein Code aus vier Farben soll erraten werden.\n" +
                               "1. Farben auswählen\n" +
                               "2. Raten Button betätigen\n" +
                               "3. Auswertung:\n" +
                               "x bedeutet richtige Farbe und richtiger Ort,\n" +
                               "o bedeutet richtige Farbe.\n" +
                               "Die Reihenfolge der Auswertung spielt keine Rolle.\n";
        }

        /// <summary>
        /// Startet das Spiel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            ellipses.Clear();
            textBoxes.Clear();
            Spielfeld.Children.Clear();
            Runde = 0;
            NeueZeile();
            for(int i=0; i<4; i++)
            {
                code[i] = rnd.Next(0,4);
            }
            //Spielregeln.Text += "\n";
            //for (int i = 0; i < 4; i++)
            //{
            //    Spielregeln.Text += code[i];
            //}
            
            Btn_Start.IsEnabled = false;
            Btn_Spiel.IsEnabled = true;

        }

        
        /// <summary>
        /// Dem Spielfeld wird eine neue Zeile hinzugrfügt.
        /// </summary>
        private void NeueZeile()
        {
            //Neue zeile im Grid einfügen
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(100,GridUnitType.Pixel);
            Spielfeld.RowDefinitions.Add(rowDefinition);

            //Ellipsen

            //Ellipsen der vorigen Runde deaktivieren
            if (Runde > 0)
            {
                ellipses.ForEach(x => x.IsEnabled = false); ;
            }

            //Liste leeren
            ellipses.Clear();

            //neue Ellipsen erstellen
            for (int i = 0; i < 4; i++)
            {
                //Ellipse definieren
                Ellipse ellipse = new Ellipse
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Gray
                };

                //Rechtklickmenü für Farbwahl
                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItemBlack = new MenuItem();
                menuItemBlack.Header = "0: Schwarz";
                menuItemBlack.Click += new RoutedEventHandler(EllipseSchwarzFärben);
                contextMenu.Items.Add(menuItemBlack);

                MenuItem menuItemBlue = new MenuItem();
                menuItemBlue.Header = "1: Blau";
                menuItemBlue.Click += new RoutedEventHandler(EllipseBlauFärben);
                contextMenu.Items.Add(menuItemBlue);

                MenuItem menuItemRed = new MenuItem();
                menuItemRed.Header = "2: Rot";
                menuItemRed.Click += new RoutedEventHandler(EllipseRotFärben);
                contextMenu.Items.Add(menuItemRed);

                MenuItem menuItemGreen = new MenuItem();
                menuItemGreen.Header = "3: Grün";
                menuItemGreen.Click += new RoutedEventHandler(EllipseGrünFärben);
                contextMenu.Items.Add(menuItemGreen);


                ellipse.ContextMenu = contextMenu;

                //Ellipse dem Grid hinzufügen
                Spielfeld.Children.Add(ellipse);
                Grid.SetRow(ellipse, Runde);
                Grid.SetColumn(ellipse, i);
                ellipses.Add(ellipse);
            }


            //Textbox
            TextBox textBox = new TextBox
            {
                Background = Brushes.LightGray,
                IsReadOnly = true,
                FontSize = 40,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };
            Spielfeld.Children.Add(textBox);
            Grid.SetRow(textBox, Runde);
            Grid.SetColumn(textBox, 4);
            textBoxes.Add(textBox);           

            Runde++;
        }

        private void EllipseSchwarzFärben(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;
            ContextMenu cm = mi.Parent as ContextMenu;
            Ellipse el = cm.PlacementTarget as Ellipse;
            el.Fill = Brushes.Black;
        }

        private void EllipseBlauFärben(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;            
            Ellipse el = ((ContextMenu)mi.Parent).PlacementTarget as Ellipse;
            el.Fill = Brushes.Blue;
        }

        private void EllipseRotFärben(object sender, RoutedEventArgs e)
        {           
            Ellipse el = ((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget as Ellipse;
            el.Fill = Brushes.Red;
        }

        private void EllipseGrünFärben(object sender, RoutedEventArgs e)
        {
            ((Ellipse)((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget).Fill = Brushes.Green;
        }

        private void Btn_Spiel_Click(object sender, RoutedEventArgs e)
        {
            int[] geraten = new int[4];
            for (int i = 0; i < geraten.Length; i++)
            {
                geraten[i] = FarbeZuZahl(ellipses[i].Fill);
            }
            if (geraten.Contains(-1))
            {
                MessageBox.Show("Bitte Farben für alle Kreise wählen.", "Fehler!");
            }
            else Raten(geraten);
        }

        private void Raten(int[] geraten)
        {
            List<string> ausgabe = new List<string>();
            
            for (int i = 0; i < geraten.Length; i++)
            {
                Console.Write(code[i]);
                if (geraten[i] == code[i]) ausgabe.Add("X");
                else if (geraten.Contains(code[i])) ausgabe.Add("O");
                else ausgabe.Add(" ");
            }
            //Mischen(ausgabe);
            ausgabe = ausgabe.OrderBy(i => rnd.Next()).ToList();
            string ausgabetext="";
            ausgabe.ForEach(x => ausgabetext += x+" ");
            textBoxes[Runde - 1].Text = ausgabetext;

            //Spielregeln.Text += "\n";
            //for (int i = 0; i < 4; i++)
            //{
            //    Spielregeln.Text += geraten[i];
            //}
            //Spielregeln.Text += "\t";
            //for (int i = 0; i < 4; i++)
            //{
            //    Spielregeln.Text += code[i];
            //}

            if (ausgabetext == "X X X X ")
            {
                MessageBox.Show("Sie haben den Code geknackt!", "Gewonnen!");
                Btn_Start.IsEnabled = true;
                Btn_Spiel.IsEnabled = false;
            }

            NeueZeile();
        }

        public void Mischen(string[] stringArray) 
        {
            //stringArray[2] = "5";//automatisch -> ausgabe[2]="5"
            int n = stringArray.Length;
            while (n > 1)
            {
                int k = rnd.Next(n--);//n-- -> n = n-1;
                //string temp = stringArray[n];
                //stringArray[n] = stringArray[k];
                //stringArray[k] = temp;
                (stringArray[n], stringArray[k]) = (stringArray[k], stringArray[n]);
            }
        }

        public int FarbeZuZahl(Brush color)
        {
            int zahl=-1;
            if (color == Brushes.Black) zahl = 0;
            else if (color == Brushes.Blue) zahl = 1;
            else if (color == Brushes.Red) zahl = 2;
            else if (color == Brushes.Green)zahl = 3;
            return zahl;
        }
    }
}
