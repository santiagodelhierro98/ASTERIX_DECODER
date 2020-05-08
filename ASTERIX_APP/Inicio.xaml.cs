using System.Windows;

namespace ASTERIX_APP
{
    public partial class Inicio : Window
    {
        public Inicio()
        {
            InitializeComponent();
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var mainwind = new MainWindow();
            mainwind.Show();
            this.Close();
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            var Aboutus = new AboutUs();
            Aboutus.Show();

        }
        private void Intrepids_Click(object sender, RoutedEventArgs e)
        {
            var ExtraWork = new ExtraPoints();
            ExtraWork.Show();
            this.Close();
        }
    }
}
