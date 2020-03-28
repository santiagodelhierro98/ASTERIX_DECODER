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
        List<CAT21> ListaCAT21;
        Fichero F;
        public MainWindow(List<CAT21> ListaCAT21)
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
        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.ShowDialog();

            Fichero fichero = new Fichero(OpenFile.FileName);
            Instructions_Label.Content = "Loading...";
            fichero.leer();
            Instructions_Label.Content = "Perfectly read!" + '\n' + "1) View the displayed data by clicking on 'Tracking Table'" +
                    '\n' + "2) Run a data simulation by clicking on 'Tracking Map'";
<<<<<<< HEAD
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
=======
            MapButton.Visibility = Visibility.Visible; ;
            TableButton.Visibility = Visibility.Visible; ;
        }    
>>>>>>> d54eb8397ec35802de314d53319a667c2e6283c1
        private void TableTrack_Click(object sender, RoutedEventArgs e)
        {
            Instructions_Label.Visibility = Visibility.Hidden;
            Track_Table.Visibility = Visibility.Visible;
            map.Visibility = Visibility.Hidden;
        }
        private void MapTrack_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            map.Visibility = 0;

            List<CAT21> pepe = F.getListCAT21();


=======
            Instructions_Label.Visibility = Visibility.Hidden;
            Track_Table.Visibility = Visibility.Hidden;
            map.Visibility = Visibility.Visible; ;            
>>>>>>> d54eb8397ec35802de314d53319a667c2e6283c1
        }
        private void Map_Load(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            map.MapProvider = OpenStreetMapProvider.Instance;
            map.Zoom = 7;
            map.MinZoom = 0;
            map.MaxZoom = 20;
            double lat = 41.0;
            double lon = 002.0;
            map.Position = new PointLatLng(lat, lon);
            map.Zoom = 5;
            map.MinZoom = 0;
            map.MaxZoom = 24;
            map.Zoom = 13;
            map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            map.CanDragMap = true;
            map.DragButton = MouseButton.Left;

                
        }
    }
}
