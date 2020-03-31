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
using GMap.NET.WindowsPresentation;
using GMap.NET;
using GMap.NET.MapProviders;
using System.IO;
using System.Data;

namespace ASTERIX_APP
{
    public partial class MainWindow : Window
    {
        Fichero F;

        //lat and lon os cat10 files 
        double latindegrees;
        double lonindegrees;


        //lat lon os MLAT system of reference (at LEBL airport)
        double MLAT_lat = 41.29694444;
        double MLAT_lon = 2.07833333;

        public MainWindow()
        {
            InitializeComponent();        
                       
            Instructions_Label.Visibility = Visibility.Visible; ;
            Instructions_Label.FontSize = 18;
            Instructions_Label.Content = "Welcome to ASTERIX APP!" + '\n' + '\n' + "We need some file to read!" + '\n' +
                "Please, load a '.ast' format file with the 'Load File' button above.";
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Are you sure you want to exit?");
            this.Close();
        }
        public void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            Instructions_Label.Content = "Loading...";
            OpenFile.ShowDialog();            
            F = new Fichero(OpenFile.FileName);            
            F.leer();

            Instructions_Label.Content = "Perfectly read!" + '\n' + "1) View the displayed data by clicking on 'Tracking Table'" +
                    '\n' + "2) Run a data simulation by clicking on 'Tracking Map'";

            MapButton.Visibility = Visibility.Visible; ;
            TableButton.Visibility = Visibility.Visible; ;
        }
        private void TableTrack_Click(object sender, RoutedEventArgs e)
        {
            Instructions_Label.Visibility = Visibility.Hidden;
            Track_Table.Visibility = Visibility.Visible;
            map.Visibility = Visibility.Hidden;
            gridlista.Visibility = Visibility.Hidden;

            if (F.CAT_list[0] == 10)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(21);
                if (IsMultipleCAT == true) { Track_Table.ItemsSource = F.getTablaMixtCAT().DefaultView; }
                else { Track_Table.ItemsSource = F.getTablaCAT10().DefaultView; }
            }
            if (F.CAT_list[0] == 21)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(10);
                if (IsMultipleCAT == true) { Track_Table.ItemsSource = F.getTablaMixtCAT().DefaultView; }
                else { Track_Table.ItemsSource = F.getTablaCAT21().DefaultView; }
            }            
        }
        private void MapTrack_Click(object sender, RoutedEventArgs e)
        {
            Instructions_Label.Visibility = Visibility.Hidden;
            Track_Table.Visibility = Visibility.Hidden;
            map.Visibility = Visibility.Visible;
            gridlista.Visibility = Visibility.Visible;

            if (F.CAT_list[0] == 10)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(21);
                if (IsMultipleCAT == true) { gridlista.ItemsSource = F.getmultiplecattablereducida().DefaultView; }
                else { gridlista.ItemsSource = F.gettablacat10reducida().DefaultView; }
            }
            if (F.CAT_list[0] == 21)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(10);
                if (IsMultipleCAT == true) { gridlista.ItemsSource = F.getmultiplecattablereducida().DefaultView; }
                else { gridlista.ItemsSource = F.gettablacat21reducida().DefaultView; }
            }
        }
        private void Map_Load(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            map.MapProvider = OpenStreetMapProvider.Instance;
            map.MinZoom = 7;
            map.MaxZoom = 16;
            map.Zoom = 14;           
            map.Position = new PointLatLng(MLAT_lat,MLAT_lon);
            map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            map.CanDragMap = true;
            map.DragButton = MouseButton.Left;                
        }
        private PointLatLng cartesiantolatlonMLAT(double X, double Y)
        {
            double R = 6371 * 1000;
            double d = Math.Sqrt((X * X) + (Y * Y));
            double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
            double phi1 =MLAT_lat * (Math.PI / 180);
            double lamda1 = MLAT_lon * (Math.PI / 180);
            var phi2 = Math.Asin(Math.Sin(phi1) * Math.Cos(d / R) + Math.Cos(phi1) * Math.Sin(d / R) * Math.Cos(brng));
            var lamda2 = lamda1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(phi1), Math.Cos(d / R) - Math.Sin(phi1) * Math.Sin(phi2));
            double latindegrees = phi2 * (180.0 / Math.PI);
            double lonindegrees = lamda2 * (180 / Math.PI);

            PointLatLng latlonMLAT = new PointLatLng(phi2 * (180 / Math.PI), lamda2 * (180 / Math.PI));
            return latlonMLAT;
        }

       public double getlatMLAT()
        {
            return latindegrees;
        }
        public double getlonMLAT()
        {
            return lonindegrees;
        }
    }
}
