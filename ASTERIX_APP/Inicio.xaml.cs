using System.Windows;

namespace ASTERIX_APP
{
    /// <summary>
    /// Lógica de interacción para Inicio.xaml
    /// </summary>
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
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Working on that");
        }
    }
}
