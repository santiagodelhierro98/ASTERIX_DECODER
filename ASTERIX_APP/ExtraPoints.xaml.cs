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
    public partial class ExtraPoints : Window
    {
        Extrapoints E = new Extrapoints();
        public ExtraPoints()
        {
            InitializeComponent();
            Fichero F_MLAT = new Fichero(@"C:\Users\joanh\Desktop\ASTERIX_DECODER\01PGTA_Fitxers_Asterix_de_prova-20200302\mlat_160510-lebl-220001.ast");
            Fichero F_ADSB = new Fichero(@"C:\Users\joanh\Desktop\ASTERIX_DECODER\01PGTA_Fitxers_Asterix_de_prova-20200302\adsb_v21_bcn.ast");
            F_MLAT.leer();
            F_ADSB.leer();

            Metodos M = new Metodos();

            List<CAT10> MLAT_List = E.returnMLATList(F_MLAT);
            List<CAT21> ADSB_List = E.returnADSBList(F_ADSB);

            DataTable ExtraPoints = new DataTable();
            M.Create_ExtraTable(ExtraPoints);
            for (int n = 0; n <= MLAT_List.Count; n++) // Ambas listas igual de largas
            {
                string Callsign = ADSB_List[n].Target_ID; // Cogemos de una de las dos listas porque son iguales
                double lat = M.cartesiantolatmlat(MLAT_List[n].Pos_Cartesian[0], MLAT_List[n].Pos_Cartesian[1]);
                double lon = M.cartesiantolonmlat(MLAT_List[n].Pos_Cartesian[0], MLAT_List[n].Pos_Cartesian[1]);
                double MLAT_H_D = ComputeDistance(lat, lon);
                double ADSB_H_D = ComputeDistance(ADSB_List[n].Lat_WGS_84, ADSB_List[n].Lon_WGS_84);
                string MLAT_V_D = MLAT_List[n].FL[2];
                double ADSB_V_D = ADSB_List[n].FL;
                string VN = Version_Number_Subfield(ADSB_List, n);
                string EPU = Horizontal_Accuracy_Pos(ADSB_List, n);

                ExtraPoints.Rows.Add(n, Callsign, MLAT_H_D, ADSB_H_D, MLAT_V_D, ADSB_V_D, VN, EPU);
            }
            Extras_Table.ItemsSource = ExtraPoints.DefaultView;
        }
        private double ComputeDistance(double lat, double lon)
        {            
            double modulo = Math.Sqrt(Math.Pow((lat - E.ARP_lat), 2) + Math.Pow((lon - E.ARP_lon), 2));
            return modulo;
        }
        void ClickExtraGrid(object sender, RoutedEventArgs e)
        {
            
        }
        private string Version_Number_Subfield(List<CAT21> list, int n)
        {
            string VN;
            if (list[n].MOPS[1] == "0") { VN = "Conformant to\nDO-260/ED-102\nand DO-242"; }
            if (list[n].MOPS[1] == "1") { VN = "Conformant to\nDO-260/A\nand DO-242A"; }
            if (list[n].MOPS[1] == "2") { VN = "Conformant to\nDO-260B/ED-102A\nand DO-242B"; }
            else { VN = "Reserved"; }
            return VN;
        }
        private string Horizontal_Accuracy_Pos(List<CAT21> list, int n)
        {
            string EPU;
            if( list[n].Quality_Indicators[2] == "0") { EPU = ">= 10 NM\nUnknown Accuracy"; }
            if (list[n].Quality_Indicators[2] == "1") { EPU = "< 10 NM\nRNP-10 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "2") { EPU = "< 4 NM\nRNP-4 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "3") { EPU = "< 2 NM\nRNP-2 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "4") { EPU = "< 1 NM\nRNP-1 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "5") { EPU = "< 0.5 NM\nRNP-0.5 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "6") { EPU = "< 0.3 NM\nRNP-0.3 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "7") { EPU = "< 0.1 NM\nRNP-0.1 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "8") { EPU = "< 0.05 NM\nGPS (SA on)"; }
            if (list[n].Quality_Indicators[2] == "8") { EPU = "< 30 m\nGPS (SA off)"; }
            if (list[n].Quality_Indicators[2] == "8") { EPU = "< 10 m\nWAAS"; }
            if (list[n].Quality_Indicators[2] == "8") { EPU = "< 3 m\nLAAS"; }
            else { EPU = "Reserved"; }

            return EPU;
        }

    }
}