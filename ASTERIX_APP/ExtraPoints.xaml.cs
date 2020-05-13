using CLASSES;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace ASTERIX_APP
{
    public partial class ExtraPoints : Window
    {
        Extrapoints E = new Extrapoints();
        Metodos M = new Metodos();

        public int CAT;
        public double[] SICSAC;
        public string filename;

        DataTable MLAT_Table = new DataTable();
        DataTable ADSB_Table = new DataTable();
        DataTable acc_MLAT_Table = new DataTable();
        DataTable acc_ADSB_Table = new DataTable();

        DataTable ResultsTable = new DataTable();
        public ExtraPoints()
        {
            InitializeComponent();
            
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "AST |*.ast";
            OpenFile.ShowDialog();
            string path = OpenFile.FileName;

            M.Create_ExtraTable_MLAT(MLAT_Table);
            M.Create_ExtraTable_ADSB(ADSB_Table);
            leerEX(path);

            M.Create_ResultsTable(ResultsTable);

            TableMLAT.ItemsSource = MLAT_Table.DefaultView;
            TableADSB.ItemsSource = ADSB_Table.DefaultView;
        }
        public void leerEX(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);

            int contadorGeneral = 0;
            List<string[]> listahex = M.File_to_HexaList(fileBytes, path);
            for (int q = 0; q < listahex.Count; q++)
            {
                // filtrar por: callsign, FL y distancia al ARP
                string[] arraystring = listahex[q];
                CAT = int.Parse(arraystring[0], System.Globalization.NumberStyles.HexNumber);
                contadorGeneral++;
                if (CAT == 10)
                {
                    CAT10 C10 = new CAT10();
                    C10.Decode10(arraystring);
                    bool modulo = E.checkdistanceMLAT(C10);
                    if (C10.Target_Rep_Descript[0] == "Mode S Multilateration" && C10.Target_ID != null && Convert.ToDouble(C10.FL[2]) > 0.0 && Convert.ToDouble(C10.FL[2]) < 500.0 && C10.FL[2] != null && modulo == true)
                    {
                        double lat = M.cartesiantolatmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
                        double lon = M.cartesiantolonmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
                        MLAT_Table.Rows.Add(C10.Target_ID, M.convert_to_hms(Math.Floor(C10.Time_Day)), lat, lon , C10.FL[2]);
                    }
                    else { }
                }
                if (CAT == 21)
                {
                    CAT21 C21 = new CAT21();
                    C21.Decode21(arraystring);
                    bool modulo21 = E.checkdistanceADSB(C21);
                    if (C21.Target_ID != null && C21.FL != 0 && C21.FL <= 150.0 && C21.MOPS[1] == "ED102A/DO-260B [Ref. 11]" && modulo21 == true)
                    {
                        double EPU = Horizontal_Accuracy_Pos(C21); // Horizonatl Accuracy (NACp)
                        double RC = Convert.ToDouble(C21.Quality_Indicators[6]); // Radius of Containments (NIC)
                        double GVA = Compute_GVA(C21); // Altitude Accuracy
                        ADSB_Table.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.Time_Rep_Transm)), C21.Lat_WGS_84 , C21.Lon_WGS_84 , C21.FL, EPU, RC, GVA);
                    }
                    else { }
                }
            }
        }
        private double Compute_GVA(CAT21 C21)
        {
            string GVA = C21.Quality_Indicators[5];
            if (GVA == "1") { return 150.0; }
            if (GVA == "2") { return 45.0; }
            else { return double.NaN; }
        }
        private double Horizontal_Accuracy_Pos(CAT21 list)
        {
            string NACp = list.Quality_Indicators[2];
            if (NACp == "0") { return 18520.0; }
            if (NACp == "1") { return 18520.0; }
            if (NACp == "2") { return 7408.0; }
            if (NACp == "3") { return 3704.0; }
            if (NACp == "4") { return 1852.0; }
            if (NACp == "5") { return 926.0; }
            if (NACp == "6") { return 555.6; }
            if (NACp == "7") { return 185.2; }
            if (NACp == "8") { return 92.6; }
            if (NACp == "9") { return 30.0; }
            if (NACp == "10") { return 10.0; }
            if (NACp == "11") { return 3.0; }
            else { return double.NaN; }
        }

        private async void getresults_Click(object sender, RoutedEventArgs e)
        {
            progressbar.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                M.Create_ExtraTable_ADSB(acc_ADSB_Table);
                M.Create_ExtraTable_MLAT(acc_MLAT_Table);
                //comparar las tablas para tener los mismos vuelos en ambas
                for (int j = 0; j < ADSB_Table.Rows.Count; j++)
                {
                    for (int i = 0; i < MLAT_Table.Rows.Count; i++)
                    {
                        string timebadmlat = Convert.ToString(MLAT_Table.Rows[i][1]);
                        string[] tiemposplitedmlat = timebadmlat.Split(':');
                        int tiempomlat = M.gettimecorrectly(tiemposplitedmlat);
                        string timebadadsb = Convert.ToString(ADSB_Table.Rows[j][1]);
                        string[] tiemposplitedadsb = timebadadsb.Split(':');
                        int tiempoadsb = M.gettimecorrectly(tiemposplitedadsb);
                        //si callsign y hora son iguales
                        if (MLAT_Table.Rows[i][0].ToString() == ADSB_Table.Rows[j][0].ToString() && tiempoadsb == tiempomlat)
                        {
                            acc_ADSB_Table.ImportRow(ADSB_Table.Rows[j]);
                            acc_MLAT_Table.ImportRow(MLAT_Table.Rows[i]);
                        }
                        else { }
                    }
                }
            });
            TableMLAT.ItemsSource = acc_MLAT_Table.DefaultView;
            TableADSB.ItemsSource = acc_ADSB_Table.DefaultView;
            await Task.Run(() =>
            {
                //rellenamos la tabla de resultados con la resta de posiciones entre adsb y mlat más el quality indicator
                for (int i = 0; i < acc_ADSB_Table.Rows.Count; i++)
                {
                    string callsign = acc_ADSB_Table.Rows[i][0].ToString();
                    string time = acc_ADSB_Table.Rows[i][1].ToString();
                    // 1º = 60', 1' = 1NM --> precision[º]*(60'/1º)*(1NM/1')*(1852m/1NM) = precision [m]
                    double precision_lat = Convert.ToDouble(acc_ADSB_Table.Rows[i][5]) + Convert.ToDouble(acc_ADSB_Table.Rows[i][6]) + 1852 * 60 * (Convert.ToDouble(acc_ADSB_Table.Rows[i][2]) - Convert.ToDouble(acc_MLAT_Table.Rows[i][2]));
                    double precision_lon = Convert.ToDouble(acc_ADSB_Table.Rows[i][5]) + Convert.ToDouble(acc_ADSB_Table.Rows[i][6]) + 1852 * 60 * (Convert.ToDouble(acc_ADSB_Table.Rows[i][3]) - Convert.ToDouble(acc_MLAT_Table.Rows[i][3]));

                    // FL*100 (feet) --> *0.3048 (to meters)
                    double altitude_precision = Convert.ToDouble(acc_ADSB_Table.Rows[i][7]) + 30.48 * Convert.ToDouble(acc_ADSB_Table.Rows[i][4]) - 30.48 * Convert.ToDouble(acc_MLAT_Table.Rows[i][4]);
                    ResultsTable.Rows.Add(callsign, time, Math.Round(precision_lat, 5), Math.Round(precision_lon, 5), Math.Round(altitude_precision, 5), "");
                }                
            });
            Res_Table.ItemsSource = ResultsTable.DefaultView;
            progressbar.Visibility = Visibility.Hidden;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var inicio = new Inicio();
            inicio.Show();
        }
    }
}