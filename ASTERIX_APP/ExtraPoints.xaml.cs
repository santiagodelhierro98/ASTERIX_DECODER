using CLASSES;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                        ADSB_Table.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.Time_Rep_Transm)), C21.Lat_WGS_84 , C21.Lon_WGS_84 , C21.FL);
                    }
                    else { }
                }
            }
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
            if (list[n].Quality_Indicators[2] == "0") { EPU = ">= 10 NM\nUnknown Accuracy"; }
            if (list[n].Quality_Indicators[2] == "1") { EPU = "< 10 NM\nRNP-10 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "2") { EPU = "< 4 NM\nRNP-4 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "3") { EPU = "< 2 NM\nRNP-2 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "4") { EPU = "< 1 NM\nRNP-1 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "5") { EPU = "< 0.5 NM\nRNP-0.5 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "6") { EPU = "< 0.3 NM\nRNP-0.3 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "7") { EPU = "< 0.1 NM\nRNP-0.1 Accuracy"; }
            if (list[n].Quality_Indicators[2] == "8") { EPU = "< 0.05 NM\nGPS (SA on)"; }
            if (list[n].Quality_Indicators[2] == "9") { EPU = "< 30 m\nGPS (SA off)"; }
            if (list[n].Quality_Indicators[2] == "10") { EPU = "< 10 m\nWAAS"; }
            if (list[n].Quality_Indicators[2] == "11") { EPU = "< 3 m\nLAAS"; }
            else { EPU = "Reserved"; }

            return EPU;
        }

        private void getresults_Click(object sender, RoutedEventArgs e)
        {
            M.Create_ExtraTable_ADSB(acc_ADSB_Table);
            M.Create_ExtraTable_MLAT(acc_MLAT_Table);
            //comparar las tablas para tener los mismos vuelos en ambas
            for (int j = 0;j < ADSB_Table.Rows.Count; j++)
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

            TableMLAT.ItemsSource = acc_MLAT_Table.DefaultView;
            TableADSB.ItemsSource = acc_ADSB_Table.DefaultView;
            //rellenamos la tabla de resultados con la resta de posiciones entre adsb y mlat más el quality indicator
            for (int i = 0; i < acc_ADSB_Table.Rows.Count; i++)
            {
                string callsign = acc_ADSB_Table.Rows[i][0].ToString();
                string time = acc_ADSB_Table.Rows[i][1].ToString();
                // 1º = 60', 1' = 1NM --> precision[º]*(60'/1º)*(1NM/1') = precision [NM]
                double precision_lat = 60*(Convert.ToDouble(acc_ADSB_Table.Rows[i][2]) - Convert.ToDouble(acc_MLAT_Table.Rows[i][2]));
                double precision_lon = 60*(Convert.ToDouble(acc_ADSB_Table.Rows[i][3]) - Convert.ToDouble(acc_MLAT_Table.Rows[i][3]));
                
                double altitude_precision = Convert.ToDouble(acc_ADSB_Table.Rows[i][4]) - Convert.ToDouble(acc_MLAT_Table.Rows[i][4]);
                ResultsTable.Rows.Add(callsign,time,Math.Round(precision_lat,5), Math.Round(precision_lon,5), altitude_precision, "");
            }
            Res_Table.ItemsSource = ResultsTable.DefaultView;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var inicio = new Inicio();
            inicio.Show();
        }
    }
}