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
using ASTERIX_DECODER_APP;

namespace ASTERIX_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Do you want to close the app?");
            this.Close();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            ASTERIX_DECODER_APP Decoder = new ASTERIX_DECODER_APP();
            OpenFileDialog OpenFile = new OpenFileDialog();
            try
            {
                OpenFile.ShowDialog();
                MessageBox.Show(OpenFile.FileName);
                Decoder.APP(OpenFile.FileName);
            }
            catch
            {
                MessageBox.Show("Error, reboot App");
            }
        }

        private void TableTrack_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Table Tracking");
        }

        private void MapTrack_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Map Tracking");
        }
    }
}
