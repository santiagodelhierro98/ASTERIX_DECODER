using Microsoft.Win32;
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
using CLASSES;

namespace ASTERIX_APP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Instructions_Label.FontSize = 18;
            Instructions_Label.Content = "Welcome to ASTERIX APP!" + '\n' + '\n' + "We need some file to read!" + '\n' +
                "Please, load a '.ast' format file with the 'Load File' button above.";
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Are you sure you want to exit?");
            this.Close();
        }
        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.ShowDialog();

            try
            {
                Fichero fichero = new Fichero(OpenFile.FileName);
                fichero.leer();

                Instructions_Label.Content = "Perfectly read!" + '\n' + "1) View the displayed data by clicking on 'Tracking Table'" +
                    '\n' + "2) Run a data simulation by clicking on 'Tracking Map'";
                MapButton.Visibility = 0;
                TableButton.Visibility = 0;

                if (fichero.CAT == 10) { Track_Table.ItemsSource = fichero.getListCAT10(); }
                if (fichero.CAT == 21) { Track_Table.ItemsSource = fichero.getListCAT21(); }
                else { MessageBox.Show("SLOW MOTION" + '\n' + "We have not solve this Asterix category yet"); }
            }
            catch
            {
                MessageBox.Show("There was an error. Incorrect format/Something went wrong in CLASSES");
                // Hay un archivo q me da errores en el cat21
            }
        }
        private void TableTrack_Click(object sender, RoutedEventArgs e)
        {
            Track_Table.Visibility = 0;

        }
        private void MapTrack_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Map Tracking");
        }
    }
}
