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
        int Runde = 0;
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
            NeueZeile();
            Btn_Start.IsEnabled = false;
            Btn_Spiel.IsEnabled = true;
        }

        
        /// <summary>
        /// Dem Spielfeld wird eine neue zeile hinzugrfügt.
        /// </summary>
        private void NeueZeile()
        {
            //Neue zeile im Grid einfügen
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(100,GridUnitType.Pixel);
            Spielfeld.RowDefinitions.Add(rowDefinition);

            //Ellipsen
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
                menuItemBlack.Header = "Schwarz";
                menuItemBlack.Click += new RoutedEventHandler(EllipseSchwarzFärben);
                contextMenu.Items.Add(menuItemBlack);


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
    }
}
