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



namespace ASTERIX_APP
{
    public partial class MainWindow : Window
    {
        Fichero F;
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
            MessageBox.Show(F.getListCAT10().Count().ToString());

            Instructions_Label.Visibility = Visibility.Hidden;
            Track_Table.Visibility = Visibility.Visible;
            map.Visibility = Visibility.Hidden;
        }
        private void MapTrack_Click(object sender, RoutedEventArgs e)
        { 
            Instructions_Label.Visibility = Visibility.Hidden;
            Track_Table.Visibility = Visibility.Hidden;
            map.Visibility = Visibility.Visible;
            gridlista.Visibility = Visibility.Visible;
            gridlista.ItemsSource = F.gettablacat21reducida().DefaultView;
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
        
    }
}
