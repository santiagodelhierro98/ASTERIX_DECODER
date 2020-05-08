using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using CLASSES;

namespace ASTERIX_APP
{
    /// <summary>
    /// Lógica de interacción para ExtraPoints.xaml
    /// </summary>
    public partial class ExtraPoints : Window
    {
        public ExtraPoints()
        {
            InitializeComponent();
            Fichero F_MLAT = new Fichero(@"C:\Users\joanh\Desktop\ASTERIX_DECODER\01PGTA_Fitxers_Asterix_de_prova-20200302\mlat_160510-lebl-220001.ast");
            Fichero F_ADSB = new Fichero(@"C:\Users\joanh\Desktop\ASTERIX_DECODER\01PGTA_Fitxers_Asterix_de_prova-20200302\adsb_v21_bcn.ast");

            Metodos M = new Metodos();
            int CAT_1 = CheckCAT(F_MLAT);
            int CAT_2 = CheckCAT(F_ADSB);

            F_MLAT.

            DataTable ExtraPoints = new DataTable();
            M.Create_ExtraTable(ExtraPoints);
            ExtraPoints.Rows.Add();
        }
        public int CheckCAT(Fichero F)
        {
            int category = new int();
            if (Math.Floor(F.CAT_list[0]) == 10)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(21);
                if (IsMultipleCAT == true)
                {
                    Extras_Table.ItemsSource = F.tablaMultipleCAT.DefaultView;
                    category = 1021;
                }
                else
                {
                    Extras_Table.ItemsSource = F.tablaCAT10.DefaultView;
                    category = 10;
                }
            }
            if (Math.Floor(F.CAT_list[0]) == 21)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(10);
                if (IsMultipleCAT == true)
                {
                    Extras_Table.ItemsSource = F.tablaMultipleCAT.DefaultView;
                    category = 1021;
                }
                else
                {
                    Extras_Table.ItemsSource = F.tablaCAT21.DefaultView;
                    category = 21;
                }                
            }
            return category;
        }

        void ClickExtraGrid(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
