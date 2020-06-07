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
        public double Pd;

        // 0 to 10NM
        DataTable MLAT_Table = new DataTable();
        DataTable ADSB_Table1 = new DataTable();
        DataTable ADSB_Table = new DataTable();
        DataTable acc_MLAT_Table = new DataTable();
        DataTable acc_ADSB_Table = new DataTable();
        DataTable ResultsTable = new DataTable();

        // 5 to 10 NM
        DataTable MLAT_Table_510 = new DataTable();
        DataTable ADSB_Table1_510 = new DataTable();
        DataTable ADSB_Table_510 = new DataTable();
        DataTable acc_MLAT_Table_510 = new DataTable();
        DataTable acc_ADSB_Table_510 = new DataTable();
        DataTable ResultsTable_510 = new DataTable();

        // 2.5 to 5 NM
        DataTable MLAT_Table_255 = new DataTable();
        DataTable ADSB_Table1_255 = new DataTable();
        DataTable ADSB_Table_255 = new DataTable();
        DataTable acc_MLAT_Table_255 = new DataTable();
        DataTable acc_ADSB_Table_255 = new DataTable();
        DataTable ResultsTable_255 = new DataTable();

        // 0 to 2.5 NM
        DataTable MLAT_Table_025 = new DataTable();
        DataTable ADSB_Table1_025 = new DataTable();
        DataTable ADSB_Table_025 = new DataTable();
        DataTable acc_MLAT_Table_025 = new DataTable();
        DataTable acc_ADSB_Table_025 = new DataTable();
        DataTable ResultsTable_025 = new DataTable();

        // Ground
        DataTable MLAT_Table_G = new DataTable();
        DataTable ADSB_Table1_G = new DataTable();
        DataTable ADSB_Table_G = new DataTable();
        DataTable acc_MLAT_Table_G = new DataTable();
        DataTable acc_ADSB_Table_G = new DataTable();
        DataTable ResultsTable_G = new DataTable();

        public ExtraPoints()
        {
            InitializeComponent();
            
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "AST |*.ast";
            OpenFile.ShowDialog();
            // When you click Cancel
            if (OpenFile.FileName == "")
            {
                this.Close();
            }
            else
            { 
                string path = OpenFile.FileName;
                // 0 to 10
                M.Create_ExtraTable_MLAT(MLAT_Table);
                M.Create_ExtraTable_ADSB(ADSB_Table1);
                M.Create_ResultsTable(ResultsTable);
                // 5 to 10
                M.Create_ExtraTable_MLAT(MLAT_Table_510);
                M.Create_ExtraTable_ADSB(ADSB_Table1_510);
                M.Create_ResultsTable(ResultsTable_510);
                // 2.5 to 5
                M.Create_ExtraTable_MLAT(MLAT_Table_255);
                M.Create_ExtraTable_ADSB(ADSB_Table1_255);
                M.Create_ResultsTable(ResultsTable_255);
                // 0 to 2.5
                M.Create_ExtraTable_MLAT(MLAT_Table_025);
                M.Create_ExtraTable_ADSB(ADSB_Table1_025);
                M.Create_ResultsTable(ResultsTable_025);
                // ground
                M.Create_ExtraTable_MLAT(MLAT_Table_G);
                M.Create_ExtraTable_ADSB(ADSB_Table1_G);
                M.Create_ResultsTable(ResultsTable_G);

                leerEX(path);

                ADSB_Table = ADSB_Reorganize(ADSB_Table1);
                ADSB_Table_510 = ADSB_Reorganize(ADSB_Table1_510);
                ADSB_Table_255 = ADSB_Reorganize(ADSB_Table1_255);
                ADSB_Table_025 = ADSB_Reorganize(ADSB_Table1_025);
                ADSB_Table_G = ADSB_Reorganize(ADSB_Table1_G);

                MessageBox.Show("Computing radar performances.\nPlease be patient, this will take a few seconds.", "WARNING");

                // 0 to 10
                M.Create_ExtraTable_ADSB(acc_ADSB_Table);
                M.Create_ExtraTable_MLAT(acc_MLAT_Table);
                int fila = 0;
                //comparar las tablas para tener los mismos vuelos en ambas
                for (int j = 0; j < ADSB_Table.Rows.Count; j++)
                {
                    bool x = false;
                    for (int i = fila; i < MLAT_Table.Rows.Count; i++)
                    {
                        if (ADSB_Table.Rows[j][1].ToString() == "NaN" || MLAT_Table.Rows[i][1].ToString() == "NaN") { break; }

                        string timebadmlat = Convert.ToString(MLAT_Table.Rows[i][1]);
                        string[] tiemposplitedmlat = timebadmlat.Split(':');
                        int tiempomlat = M.gettimecorrectly(tiemposplitedmlat);
                        string timebadadsb = Convert.ToString(ADSB_Table.Rows[j][1]);
                        string[] tiemposplitedadsb = timebadadsb.Split(':');
                        int tiempoadsb = M.gettimecorrectly(tiemposplitedadsb);

                        if (x == false && tiempoadsb == tiempomlat) { fila = i; x = true; }
                        double TimeDiff = Convert.ToDouble(ADSB_Table.Rows[j][9]) - Convert.ToDouble(MLAT_Table.Rows[i][6]);
                        double[] ext = Extrapolation(TimeDiff, Convert.ToDouble(MLAT_Table.Rows[i][7]), Convert.ToDouble(MLAT_Table.Rows[i][8]));

                        //si callsign y hora son iguales
                        if (MLAT_Table.Rows[i][0].ToString() == ADSB_Table.Rows[j][0].ToString() && tiempoadsb == tiempomlat)
                        {
                            acc_ADSB_Table.Rows.Add(ADSB_Table.Rows[j][0], ADSB_Table.Rows[j][1], ADSB_Table.Rows[j][2], ADSB_Table.Rows[j][3],
                                ADSB_Table.Rows[j][4], ADSB_Table.Rows[j][5], ADSB_Table.Rows[j][6], ADSB_Table.Rows[j][7], ADSB_Table.Rows[j][8],
                                ADSB_Table.Rows[j][9]);

                            acc_MLAT_Table.Rows.Add(MLAT_Table.Rows[i][0], MLAT_Table.Rows[i][1], Math.Round(Convert.ToDouble(MLAT_Table.Rows[i][2]) + ext[0], 8),
                                Math.Round(Convert.ToDouble(MLAT_Table.Rows[i][3]) + ext[1], 8), Math.Round(E.checkdistanceMLAT_Acc(Convert.ToDouble(MLAT_Table.Rows[i][2]) + ext[0],
                                Convert.ToDouble(MLAT_Table.Rows[i][3]) + ext[1]), 8), MLAT_Table.Rows[i][5], MLAT_Table.Rows[i][6], MLAT_Table.Rows[i][7],
                                MLAT_Table.Rows[i][8]);
                        }
                        if (tiempoadsb + 2 == tiempomlat) { break; }
                        else { }
                    }
                }
                MessageBox.Show("From 0 to 10NM: DONE\n" +
                    "From 5 to 10NM:\nFrom 2.5 to 5NM:\nFrom 0 to 2.5NM:\nGround:\n", "PROCESS");

                // 5 to 10
                M.Create_ExtraTable_ADSB(acc_ADSB_Table_510);
                M.Create_ExtraTable_MLAT(acc_MLAT_Table_510);
                fila = 0;
                //comparar las tablas para tener los mismos vuelos en ambas
                for (int j = 0; j < ADSB_Table_510.Rows.Count; j++)
                {
                    bool x = false;
                    for (int i = fila; i < MLAT_Table_510.Rows.Count; i++)
                    {
                        if (ADSB_Table_510.Rows[j][1].ToString() == "NaN" || MLAT_Table_510.Rows[i][1].ToString() == "NaN") { break; }

                        string timebadmlat = Convert.ToString(MLAT_Table_510.Rows[i][1]);
                        string[] tiemposplitedmlat = timebadmlat.Split(':');
                        int tiempomlat = M.gettimecorrectly(tiemposplitedmlat);
                        string timebadadsb = Convert.ToString(ADSB_Table_510.Rows[j][1]);
                        string[] tiemposplitedadsb = timebadadsb.Split(':');
                        int tiempoadsb = M.gettimecorrectly(tiemposplitedadsb);

                        if (x == false && tiempoadsb == tiempomlat) { fila = i; x = true; }
                        double TimeDiff = Convert.ToDouble(ADSB_Table_510.Rows[j][9]) - Convert.ToDouble(MLAT_Table_510.Rows[i][6]);
                        double[] ext = Extrapolation(TimeDiff, Convert.ToDouble(MLAT_Table_510.Rows[i][7]), Convert.ToDouble(MLAT_Table_510.Rows[i][8]));

                        //si callsign y hora son iguales
                        if (MLAT_Table_510.Rows[i][0].ToString() == ADSB_Table_510.Rows[j][0].ToString() && tiempoadsb == tiempomlat)
                        {
                            acc_ADSB_Table_510.Rows.Add(ADSB_Table_510.Rows[j][0], ADSB_Table_510.Rows[j][1], ADSB_Table_510.Rows[j][2], ADSB_Table_510.Rows[j][3],
                                ADSB_Table_510.Rows[j][4], ADSB_Table_510.Rows[j][5], ADSB_Table_510.Rows[j][6], ADSB_Table_510.Rows[j][7], ADSB_Table_510.Rows[j][8],
                                ADSB_Table_510.Rows[j][9]);

                            acc_MLAT_Table_510.Rows.Add(MLAT_Table_510.Rows[i][0], MLAT_Table_510.Rows[i][1], Math.Round(Convert.ToDouble(MLAT_Table_510.Rows[i][2]) + ext[0], 8),
                                Math.Round(Convert.ToDouble(MLAT_Table_510.Rows[i][3]) + ext[1], 8), Math.Round(E.checkdistanceMLAT_Acc(Convert.ToDouble(MLAT_Table_510.Rows[i][2]) + ext[0],
                                Convert.ToDouble(MLAT_Table_510.Rows[i][3]) + ext[1]), 8), MLAT_Table_510.Rows[i][5], MLAT_Table_510.Rows[i][6], MLAT_Table_510.Rows[i][7],
                                MLAT_Table_510.Rows[i][8]);
                        }
                        if (tiempoadsb + 2 == tiempomlat) { break; }
                        else { }
                    }
                }
                MessageBox.Show("From 0 to 10NM: DONE\n" +
                    "From 5 to 10NM: DONE\nFrom 2.5 to 5NM:\nFrom 0 to 2.5NM:\nGround:\n", "PROCESS");

                // 2.5 to 5
                M.Create_ExtraTable_ADSB(acc_ADSB_Table_255);
                M.Create_ExtraTable_MLAT(acc_MLAT_Table_255);
                fila = 0;
                //comparar las tablas para tener los mismos vuelos en ambas
                for (int j = 0; j < ADSB_Table_255.Rows.Count; j++)
                {
                    bool x = false;
                    for (int i = fila; i < MLAT_Table_255.Rows.Count; i++)
                    {
                        if (ADSB_Table_255.Rows[j][1].ToString() == "NaN" || MLAT_Table_255.Rows[i][1].ToString() == "NaN") { break; }

                        string timebadmlat = Convert.ToString(MLAT_Table_255.Rows[i][1]);
                        string[] tiemposplitedmlat = timebadmlat.Split(':');
                        int tiempomlat = M.gettimecorrectly(tiemposplitedmlat);
                        string timebadadsb = Convert.ToString(ADSB_Table_255.Rows[j][1]);
                        string[] tiemposplitedadsb = timebadadsb.Split(':');
                        int tiempoadsb = M.gettimecorrectly(tiemposplitedadsb);

                        if (x == false && tiempoadsb == tiempomlat) { fila = i; x = true; }
                        double TimeDiff = Convert.ToDouble(ADSB_Table_255.Rows[j][9]) - Convert.ToDouble(MLAT_Table_255.Rows[i][6]);
                        double[] ext = Extrapolation(TimeDiff, Convert.ToDouble(MLAT_Table_255.Rows[i][7]), Convert.ToDouble(MLAT_Table_255.Rows[i][8]));

                        //si callsign y hora son iguales
                        if (MLAT_Table_255.Rows[i][0].ToString() == ADSB_Table_255.Rows[j][0].ToString() && tiempoadsb == tiempomlat)
                        {
                            acc_ADSB_Table_255.Rows.Add(ADSB_Table_255.Rows[j][0], ADSB_Table_255.Rows[j][1], ADSB_Table_255.Rows[j][2], ADSB_Table_255.Rows[j][3],
                                ADSB_Table_255.Rows[j][4], ADSB_Table_255.Rows[j][5], ADSB_Table_255.Rows[j][6], ADSB_Table_255.Rows[j][7], ADSB_Table_255.Rows[j][8],
                                ADSB_Table_255.Rows[j][9]);

                            acc_MLAT_Table_255.Rows.Add(MLAT_Table_255.Rows[i][0], MLAT_Table_255.Rows[i][1], Math.Round(Convert.ToDouble(MLAT_Table_255.Rows[i][2]) + ext[0], 8),
                                Math.Round(Convert.ToDouble(MLAT_Table_255.Rows[i][3]) + ext[1], 8), Math.Round(E.checkdistanceMLAT_Acc(Convert.ToDouble(MLAT_Table_255.Rows[i][2]) + ext[0],
                                Convert.ToDouble(MLAT_Table_255.Rows[i][3]) + ext[1]), 8), MLAT_Table_255.Rows[i][5], MLAT_Table_255.Rows[i][6], MLAT_Table_255.Rows[i][7],
                                MLAT_Table_255.Rows[i][8]);
                        }
                        if (tiempoadsb + 2 == tiempomlat) { break; }
                        else { }
                    }
                }
                MessageBox.Show("From 0 to 10NM: DONE\n" +
                    "From 5 to 10NM: DONE\nFrom 2.5 to 5NM: DONE \nFrom 0 to 2.5NM:\nGround:\n", "PROCESS");


                // 0 to 2.5
                M.Create_ExtraTable_ADSB(acc_ADSB_Table_025);
                M.Create_ExtraTable_MLAT(acc_MLAT_Table_025);
                fila = 0;
                //comparar las tablas para tener los mismos vuelos en ambas
                for (int j = 0; j < ADSB_Table_025.Rows.Count; j++)
                {
                    bool x = false;
                    for (int i = fila; i < MLAT_Table_025.Rows.Count; i++)
                    {
                        if (ADSB_Table_025.Rows[j][1].ToString() == "NaN" || MLAT_Table_025.Rows[i][1].ToString() == "NaN") { break; }

                        string timebadmlat = Convert.ToString(MLAT_Table_025.Rows[i][1]);
                        string[] tiemposplitedmlat = timebadmlat.Split(':');
                        int tiempomlat = M.gettimecorrectly(tiemposplitedmlat);
                        string timebadadsb = Convert.ToString(ADSB_Table_025.Rows[j][1]);
                        string[] tiemposplitedadsb = timebadadsb.Split(':');
                        int tiempoadsb = M.gettimecorrectly(tiemposplitedadsb);

                        if (x == false && tiempoadsb == tiempomlat) { fila = i; x = true; }
                        double TimeDiff = Convert.ToDouble(ADSB_Table_025.Rows[j][9]) - Convert.ToDouble(MLAT_Table_025.Rows[i][6]);
                        double[] ext = Extrapolation(TimeDiff, Convert.ToDouble(MLAT_Table_025.Rows[i][7]), Convert.ToDouble(MLAT_Table_025.Rows[i][8]));

                        //si callsign y hora son iguales
                        if (MLAT_Table_025.Rows[i][0].ToString() == ADSB_Table_025.Rows[j][0].ToString() && tiempoadsb == tiempomlat)
                        {
                            acc_ADSB_Table_025.Rows.Add(ADSB_Table_025.Rows[j][0], ADSB_Table_025.Rows[j][1], ADSB_Table_025.Rows[j][2], ADSB_Table_025.Rows[j][3],
                                ADSB_Table_025.Rows[j][4], ADSB_Table_025.Rows[j][5], ADSB_Table_025.Rows[j][6], ADSB_Table_025.Rows[j][7], ADSB_Table_025.Rows[j][8],
                                ADSB_Table_025.Rows[j][9]);

                            acc_MLAT_Table_025.Rows.Add(MLAT_Table_025.Rows[i][0], MLAT_Table_025.Rows[i][1], Math.Round(Convert.ToDouble(MLAT_Table_025.Rows[i][2]) + ext[0], 8),
                                Math.Round(Convert.ToDouble(MLAT_Table_025.Rows[i][3]) + ext[1], 8), Math.Round(E.checkdistanceMLAT_Acc(Convert.ToDouble(MLAT_Table_025.Rows[i][2]) + ext[0],
                                Convert.ToDouble(MLAT_Table_025.Rows[i][3]) + ext[1]), 8), MLAT_Table_025.Rows[i][5], MLAT_Table_025.Rows[i][6], MLAT_Table_025.Rows[i][7],
                                MLAT_Table_025.Rows[i][8]);
                        }
                        if (tiempoadsb + 2 == tiempomlat) { break; }
                        else { }
                    }
                }
                MessageBox.Show("From 0 to 10NM: DONE\n" +
                    "From 5 to 10NM: DONE\nFrom 2.5 to 5NM: DONE \nFrom 0 to 2.5NM: DONE\nGround:\n", "PROCESS");

                // Ground
                M.Create_ExtraTable_ADSB(acc_ADSB_Table_G);
                M.Create_ExtraTable_MLAT(acc_MLAT_Table_G);
                fila = 0;
                //comparar las tablas para tener los mismos vuelos en ambas         
                for (int j = 0; j < ADSB_Table_G.Rows.Count; j++)
                {
                    bool x = false;
                    for (int i = fila; i < MLAT_Table_G.Rows.Count; i++)
                    {
                        if (ADSB_Table_G.Rows[j][1].ToString() == "NaN" || MLAT_Table_G.Rows[i][1].ToString() == "NaN") { break; }

                        string timebadmlat = Convert.ToString(MLAT_Table_G.Rows[i][1]);
                        string[] tiemposplitedmlat = timebadmlat.Split(':');
                        int tiempomlat = M.gettimecorrectly(tiemposplitedmlat);
                        string timebadadsb = Convert.ToString(ADSB_Table_G.Rows[j][1]);
                        string[] tiemposplitedadsb = timebadadsb.Split(':');
                        int tiempoadsb = M.gettimecorrectly(tiemposplitedadsb);

                        if (x == false && tiempoadsb == tiempomlat) { fila = i; x = true; }
                        double TimeDiff = Convert.ToDouble(ADSB_Table_G.Rows[j][9]) - Convert.ToDouble(MLAT_Table_G.Rows[i][6]);
                        double[] ext = Extrapolation(TimeDiff, Convert.ToDouble(MLAT_Table_G.Rows[i][7]), Convert.ToDouble(MLAT_Table_G.Rows[i][8]));

                        string MLATid = MLAT_Table_G.Rows[i][0].ToString();
                        string ADSBid = ADSB_Table_G.Rows[j][0].ToString();
                        //si callsign y hora son iguales
                        if (MLATid == ADSBid && tiempoadsb == tiempomlat)
                        {
                            acc_ADSB_Table_G.Rows.Add(ADSB_Table_G.Rows[j][0], ADSB_Table_G.Rows[j][1], ADSB_Table_G.Rows[j][2], ADSB_Table_G.Rows[j][3],
                                ADSB_Table_G.Rows[j][4], ADSB_Table_G.Rows[j][5], ADSB_Table_G.Rows[j][6], ADSB_Table_G.Rows[j][7], ADSB_Table_G.Rows[j][8],
                                ADSB_Table_G.Rows[j][9]);

                            acc_MLAT_Table_G.Rows.Add(MLAT_Table_G.Rows[i][0], MLAT_Table_G.Rows[i][1], Math.Round(Convert.ToDouble(MLAT_Table_G.Rows[i][2]) + ext[0], 8),
                                Math.Round(Convert.ToDouble(MLAT_Table_G.Rows[i][3]) + ext[1], 8), Math.Round(E.checkdistanceMLAT_Acc(Convert.ToDouble(MLAT_Table_G.Rows[i][2]) + ext[0],
                                Convert.ToDouble(MLAT_Table_G.Rows[i][3]) + ext[1]), 8), MLAT_Table_G.Rows[i][5], MLAT_Table_G.Rows[i][6], MLAT_Table_G.Rows[i][7],
                                MLAT_Table_G.Rows[i][8]);
                        }
                        if (tiempoadsb + 2 == tiempomlat) { break; }
                        else { }
                    }
                }
                MessageBox.Show("From 0 to 10NM: DONE\n" +
                    "From 5 to 10NM: DONE\nFrom 2.5 to 5NM: DONE \nFrom 0 to 2.5NM: DONE\nGround: DONE\n", "PROCESS");
            }
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
                    double modulo = E.checkdistanceMLAT(C10);
                    double FL;
                    double lat = M.cartesiantolatmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
                    double lon = M.cartesiantolonmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
                    if ( C10.Time_Day.ToString() == "NaN") { C10.Time_Day = 0.0; }

                    // 0 to 10NM
                    if (C10.Target_Rep_Descript[0] == "Mode S Multilateration" && C10.Target_ID != null && Convert.ToDouble(C10.FL[2]) > 0.0 && Convert.ToDouble(C10.FL[2]) < 500.0 && C10.FL[2] != null && modulo < 10)
                    {
                        MLAT_Table.Rows.Add(C10.Target_ID, M.convert_to_hms(Math.Floor(C10.Time_Day)), Math.Round(lat, 8), Math.Round(lon, 8), Math.Round(modulo, 8), C10.FL[2], C10.Time_Day, C10.Track_Vel_Cartesian[0], C10.Track_Vel_Cartesian[1]);
                    }
                    // 5 to 10NM
                    if (C10.Target_Rep_Descript[0] == "Mode S Multilateration" && C10.Target_ID != null && Convert.ToDouble(C10.FL[2]) > 0.0 && Convert.ToDouble(C10.FL[2]) < 500.0 && C10.FL[2] != null && modulo < 10 && modulo > 5)
                    {
                        MLAT_Table_510.Rows.Add(C10.Target_ID, M.convert_to_hms(Math.Floor(C10.Time_Day)), Math.Round(lat, 8), Math.Round(lon, 8), Math.Round(modulo, 8), C10.FL[2], C10.Time_Day, C10.Track_Vel_Cartesian[0], C10.Track_Vel_Cartesian[1]);
                    }
                    // 2.5 to 5NM
                    if (C10.Target_Rep_Descript[0] == "Mode S Multilateration" && C10.Target_ID != null && Convert.ToDouble(C10.FL[2]) > 0.0 && Convert.ToDouble(C10.FL[2]) < 500.0 && C10.FL[2] != null && modulo < 5 && modulo > 2.5)
                    {
                        MLAT_Table_255.Rows.Add(C10.Target_ID, M.convert_to_hms(Math.Floor(C10.Time_Day)), Math.Round(lat, 8), Math.Round(lon, 8), Math.Round(modulo, 8), C10.FL[2], C10.Time_Day, C10.Track_Vel_Cartesian[0], C10.Track_Vel_Cartesian[1]);
                    }
                    // 0 to 2.5NM
                    if (C10.Target_Rep_Descript[0] == "Mode S Multilateration" && C10.Target_ID != null && Convert.ToDouble(C10.FL[2]) > 0.0 && Convert.ToDouble(C10.FL[2]) < 500.0 && C10.FL[2] != null && modulo < 2.5 && modulo > 0)
                    {
                        MLAT_Table_025.Rows.Add(C10.Target_ID, M.convert_to_hms(Math.Floor(C10.Time_Day)), Math.Round(lat, 8), Math.Round(lon, 8), Math.Round(modulo, 8), C10.FL[2], C10.Time_Day, C10.Track_Vel_Cartesian[0], C10.Track_Vel_Cartesian[1]);
                    }
                    // Ground
                    if (C10.Target_Rep_Descript[0] == "Mode S Multilateration" && C10.Target_ID != null && Convert.ToDouble(C10.FL[2]) <= 5.0)
                    {
                        if (C10.FL[2] == null) { C10.FL[2] = Convert.ToString(0.0); }
                        FL = Convert.ToDouble(C10.FL[2]);
                      
                        MLAT_Table_G.Rows.Add(C10.Target_ID, M.convert_to_hms(Math.Floor(C10.Time_Day)), Math.Round(lat, 8), Math.Round(lon, 8), Math.Round(modulo, 8), FL, C10.Time_Day, C10.Track_Vel_Cartesian[0], C10.Track_Vel_Cartesian[1]);
                        
                    }
                    else { }
                }
                if (CAT == 21)
                {
                    SICSAC = M.getSIC_SAC(arraystring);
                    // Check if its version 23
                    if (SICSAC[0] == 107 && SICSAC[1] == 0)
                    {
                        CAT21_v23 C21 = new CAT21_v23();
                        C21.Decode21_23(arraystring);
                        double modulo21 = E.checkdistanceADSBv23(C21);
                        if (C21.Time_of_Day.ToString() == "NaN") { C21.Time_of_Day = 0.0; }

                        // 0 to 10NM
                        if (C21.Target_ID != null && C21.FL != 0 && C21.FL <= 150.0  && modulo21 < 10)
                        {
                            ADSB_Table1.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.Time_of_Day)), Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, "N/A", "N/A", "N/A", C21.Time_of_Day);
                        }
                        // 5 to 10NM
                        if (C21.Target_ID != null && C21.FL != 0 && C21.FL <= 150.0  && modulo21 < 10 && modulo21 > 5)
                        {
                            ADSB_Table1_510.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.Time_of_Day)), Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, "N/A", "N/A", "N/A", C21.Time_of_Day);
                        }
                        // 2.5 to 5NM
                        if (C21.Target_ID != null && C21.FL != 0 && C21.FL <= 150.0  && modulo21 < 5 && modulo21 > 2.5)
                        {
                            ADSB_Table1_255.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.Time_of_Day)), Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, "N/A", "N/A", "N/A", C21.Time_of_Day);
                        }
                        // 0 to 2.5NM
                        if (C21.Target_ID != null && C21.FL != 0 && C21.FL <= 150.0  && modulo21 < 2.5 && modulo21 > 0)
                        {
                            ADSB_Table1_025.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.Time_of_Day)), Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, "N/A", "N/A", "N/A", C21.Time_of_Day);
                        }
                        // Ground
                        if (C21.Target_ID != null && C21.FL <= 5 )
                        {
                            ADSB_Table1_G.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.Time_of_Day)), Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, "N/A", "N/A", "N/A", C21.Time_of_Day);
                        }
                        else { }
                    }
                    else
                    {
                        CAT21 C21 = new CAT21();
                        C21.Decode21(arraystring);
                        if (C21.TMRP.ToString() == "NaN") { C21.TMRP = 0.0; }

                        double modulo21 = E.checkdistanceADSB(C21);
                        double EPU = E.Horizontal_Accuracy_Pos(C21); // Horizonatl Accuracy (NACp)
                        double RC = Convert.ToDouble(C21.Quality_Indicators[6]); // Radius of Containments (NIC)
                        double GVA = E.Compute_GVA(C21); // Altitude Accuracy
                        // 0 to 10NM
                        if (C21.Target_ID != null && C21.FL != 0 && C21.FL <= 150.0 && C21.MOPS[1] == "ED102A/DO-260B [Ref. 11]" && modulo21 < 10)
                        {  
                            ADSB_Table1.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.TMRP)), Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, EPU, RC, GVA, C21.TMRP);
                        }
                        // 5 to 10NM
                        if (C21.Target_ID != null && C21.FL != 0 && C21.FL <= 150.0 && C21.MOPS[1] == "ED102A/DO-260B [Ref. 11]" && modulo21 < 10 && modulo21 > 5)
                        {
                            ADSB_Table1_510.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.TMRP)), Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, EPU, RC, GVA, C21.TMRP);
                        }
                        // 2.5 to 5NM
                        if (C21.Target_ID != null && C21.FL != 0 && C21.FL <= 150.0 && C21.MOPS[1] == "ED102A/DO-260B [Ref. 11]" && modulo21 < 5 && modulo21 > 2.5)
                        {
                            ADSB_Table1_255.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.TMRP)), Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, EPU, RC, GVA, C21.TMRP);
                        }
                        // 0 to 2.5NM
                        if (C21.Target_ID != null && C21.FL != 0 && C21.FL <= 150.0 && C21.MOPS[1] == "ED102A/DO-260B [Ref. 11]" && modulo21 < 2.5 && modulo21 > 0)
                        {
                            ADSB_Table1_025.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.TMRP)), Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, EPU, RC, GVA, C21.TMRP);
                        }
                        // Ground
                        if (C21.Target_ID != null && C21.FL <= 5 && C21.MOPS[1] == "ED102A/DO-260B [Ref. 11]")
                        {
                            string hora = M.convert_to_hms(Math.Floor(C21.TMRP));
                            if (hora == "NaN")
                            { hora = M.convert_to_hms(Math.Floor(C21.Time_Rep_Transm)); }

                            ADSB_Table1_G.Rows.Add(C21.Target_ID, hora, Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, EPU, RC, GVA, C21.TMRP);
                        }
                        else { }
                    }
                   
                }
            }
        }
        private DataTable ADSB_Reorganize(DataTable table)
        {
            DataTable new_table = new DataTable();
            M.Create_ExtraTable_ADSB(new_table);

            for (int i = 0; i<table.Rows.Count; i++)
            {
                string callsign = table.Rows[i][0].ToString();
                string hora = table.Rows[i][1].ToString();        
                
                int n = i + 1;
                // SI NO ESTAN DUPLICAOS NO LE DECIMOS Q LO AÑADA GGGGGGGILIPOLLAS
                while (n < table.Rows.Count && table.Rows[n][1].ToString() == hora)
                {
                    if (table.Rows[n][0].ToString() == callsign)
                    {
                        double lat = Math.Round((Convert.ToDouble(table.Rows[i][2]) + Convert.ToDouble(table.Rows[n][2])) / 2, 8);
                        double lon = Math.Round((Convert.ToDouble(table.Rows[i][3]) + Convert.ToDouble(table.Rows[n][3])) / 2, 8);
                        double dist = Math.Round((Convert.ToDouble(table.Rows[i][4]) + Convert.ToDouble(table.Rows[n][4])) / 2, 8);
                        double H = Math.Round((Convert.ToDouble(table.Rows[i][5]) + Convert.ToDouble(table.Rows[n][5])) / 2, 8);
                        double sec = Math.Round((Convert.ToDouble(table.Rows[i][9]) + Convert.ToDouble(table.Rows[n][9])) / 2, 8);

                        new_table.Rows.Add(table.Rows[i][0], table.Rows[i][1], lat, lon, dist, H, table.Rows[i][6], table.Rows[i][7], table.Rows[i][8], sec);
                        break;
                    }
                    else { n++; }
                }
            }
            return new_table;
        }
        private void getresults_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Uploaded file has " + acc_MLAT_Table.Rows.Count + " packages within this range.\nPlease be patient, this may take several seconds.", "WARNING");
            progressbar.Visibility = Visibility.Visible;

            ResultsTable.Clear();
            double mean_error_lat = new double();
            double mean_error_lon = new double();
            double mean_error_alt = new double();
            double mean_error_R = new double();

            TableMLAT.ItemsSource = acc_MLAT_Table.DefaultView;
            TableADSB.ItemsSource = acc_ADSB_Table.DefaultView;
            //rellenamos la tabla de resultados con la resta de posiciones entre adsb y mlat más el quality indicator
            for (int i = 0; i < acc_ADSB_Table.Rows.Count; i++)
            {
                string callsign = acc_ADSB_Table.Rows[i][0].ToString();
                string time = acc_ADSB_Table.Rows[i][1].ToString();

                double precision_lat = Convert.ToDouble(acc_ADSB_Table.Rows[i][2]) - (Convert.ToDouble(acc_MLAT_Table.Rows[i][2]));
                double precision_lon = Convert.ToDouble(acc_ADSB_Table.Rows[i][3]) - (Convert.ToDouble(acc_MLAT_Table.Rows[i][3]));
                mean_error_lat += precision_lat;
                mean_error_lon += precision_lon;
                // R [m]
                double precision_R = 1852 * (Convert.ToDouble(acc_ADSB_Table.Rows[i][4]) - Convert.ToDouble(acc_MLAT_Table.Rows[i][4]));
                mean_error_R += precision_R;
                // FL*100 (feet) --> *0.3048 (to meters)
                double altitude_precision = 100 * (Convert.ToDouble(acc_ADSB_Table.Rows[i][5]) - Convert.ToDouble(acc_MLAT_Table.Rows[i][5]));
                mean_error_alt += altitude_precision;

                ResultsTable.Rows.Add(callsign, time, Math.Round(precision_lat, 8), Math.Round(precision_lon, 8), Math.Round(precision_R, 8), Math.Round(altitude_precision, 8));
            }
            Res_Table.ItemsSource = ResultsTable.DefaultView;

            if (ADSB_Table.Rows.Count == 0 || MLAT_Table.Rows.Count == 0) { MessageBox.Show("Any flight within this range interval.","WARNING"); }
            else
            {
                Pd = Probability_of_Detection(ADSB_Table, MLAT_Table);
                double lat_percentil = Percentile(2, 0.95, ResultsTable);
                double lon_percentil = Percentile(3, 0.95, ResultsTable);
                double dist_percentil = Percentile(4, 0.95, ResultsTable);
                double alt_percentil = Percentile(5, 0.95, ResultsTable);

                Lat_Av.Content = "Latitude: " + Math.Round(mean_error_lat / acc_ADSB_Table.Rows.Count, 5) + " º / " + Math.Round(lat_percentil, 5) + " º";
                Lon_Av.Content = "Longitude: " + Math.Round(mean_error_lon / acc_ADSB_Table.Rows.Count, 5) + " º / " + Math.Round(lon_percentil, 5) + " º";
                Dist_Av.Content = "Distance: " + Math.Round(mean_error_R / acc_ADSB_Table.Rows.Count, 5) + " m / " + Math.Round(dist_percentil, 6) + " m";
                Alt_Av.Content = "Altitude: " + Math.Round(mean_error_alt / acc_ADSB_Table.Rows.Count, 5) + " ft / " + Math.Round(alt_percentil, 5) + " ft";
                Pd_10NM.Content = "Pd: " + Math.Round(Pd, 5) + " %";
            }            

            progressbar.Visibility = Visibility.Hidden;
        }
        private void getresults510_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Uploaded file has " + acc_MLAT_Table_510.Rows.Count + " packages within this range.\nPlease be patient, this may take several seconds.", "WARNING");
            progressbar.Visibility = Visibility.Visible;

            ResultsTable_510.Clear();
            double mean_error_lat = new double();
            double mean_error_lon = new double();
            double mean_error_alt = new double();
            double mean_error_R = new double();

            TableMLAT.ItemsSource = acc_MLAT_Table_510.DefaultView;
            TableADSB.ItemsSource = acc_ADSB_Table_510.DefaultView;
            //rellenamos la tabla de resultados con la resta de posiciones entre adsb y mlat más el quality indicator
            for (int i = 0; i < acc_ADSB_Table_510.Rows.Count; i++)
            {
                string callsign = acc_ADSB_Table_510.Rows[i][0].ToString();
                string time = acc_ADSB_Table_510.Rows[i][1].ToString();

                double precision_lat = Convert.ToDouble(acc_ADSB_Table_510.Rows[i][2]) - (Convert.ToDouble(acc_MLAT_Table_510.Rows[i][2]));
                double precision_lon = Convert.ToDouble(acc_ADSB_Table_510.Rows[i][3]) - (Convert.ToDouble(acc_MLAT_Table_510.Rows[i][3]));
                mean_error_lat += precision_lat;
                mean_error_lon += precision_lon;
                // R [m]
                double precision_R = 1852 * (Convert.ToDouble(acc_ADSB_Table_510.Rows[i][4]) - Convert.ToDouble(acc_MLAT_Table_510.Rows[i][4]));
                mean_error_R += precision_R;
                // FL*100 (feet) --> *0.3048 (to meters)
                double altitude_precision = 100 * (Convert.ToDouble(acc_ADSB_Table_510.Rows[i][5]) - Convert.ToDouble(acc_MLAT_Table_510.Rows[i][5]));
                mean_error_alt += altitude_precision;

                ResultsTable_510.Rows.Add(callsign, time, Math.Round(precision_lat, 8), Math.Round(precision_lon, 8), Math.Round(precision_R, 8), Math.Round(altitude_precision, 8));
            }
                  
            Res_Table.ItemsSource = ResultsTable_510.DefaultView;

            if (ADSB_Table_510.Rows.Count == 0 || MLAT_Table.Rows.Count == 0) { MessageBox.Show("Any flight within this range interval.", "WARNING"); }
            else
            {
                Pd = Probability_of_Detection(ADSB_Table_510, MLAT_Table);
                double lat_percentil = Percentile(2, 0.95, ResultsTable_510);
                double lon_percentil = Percentile(3, 0.95, ResultsTable_510);
                double dist_percentil = Percentile(4, 0.95, ResultsTable_510);
                double alt_percentil = Percentile(5, 0.95, ResultsTable_510);

                Lat_Av.Content = "Latitude: " + Math.Round(mean_error_lat / acc_ADSB_Table_510.Rows.Count, 5) + " º / " + Math.Round(lat_percentil, 5) + " º";
                Lon_Av.Content = "Longitude: " + Math.Round(mean_error_lon / acc_ADSB_Table_510.Rows.Count, 5) + " º / " + Math.Round(lon_percentil, 5) + " º";
                Dist_Av.Content = "Distance: " + Math.Round(mean_error_R / acc_ADSB_Table_510.Rows.Count, 5) + " m / " + Math.Round(dist_percentil, 6) + " m";
                Alt_Av.Content = "Altitude: " + Math.Round(mean_error_alt / acc_ADSB_Table_510.Rows.Count, 5) + " ft / " + Math.Round(alt_percentil, 5) + " ft";
                Pd_10NM.Content = "Pd: " + Math.Round(Pd, 5) + " %";
            }
            progressbar.Visibility = Visibility.Hidden;
        }
        private void getresults255_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Uploaded file has " + acc_MLAT_Table_255.Rows.Count + " packages within this range.\nPlease be patient, this may take several seconds.", "WARNING");
            progressbar.Visibility = Visibility.Visible;
            ResultsTable_255.Clear();
            double mean_error_lat = new double();
            double mean_error_lon = new double();
            double mean_error_alt = new double();
            double mean_error_R = new double();

            TableMLAT.ItemsSource = acc_MLAT_Table_255.DefaultView;
            TableADSB.ItemsSource = acc_ADSB_Table_255.DefaultView;
            //rellenamos la tabla de resultados con la resta de posiciones entre adsb y mlat más el quality indicator
            for (int i = 0; i < acc_ADSB_Table_255.Rows.Count; i++)
            {
                string callsign = acc_ADSB_Table_255.Rows[i][0].ToString();
                string time = acc_ADSB_Table_255.Rows[i][1].ToString();

                double precision_lat = Convert.ToDouble(acc_ADSB_Table_255.Rows[i][2]) - (Convert.ToDouble(acc_MLAT_Table_255.Rows[i][2]));
                double precision_lon = Convert.ToDouble(acc_ADSB_Table_255.Rows[i][3]) - (Convert.ToDouble(acc_MLAT_Table_255.Rows[i][3]));
                mean_error_lat += precision_lat;
                mean_error_lon += precision_lon;
                // R [m]
                double precision_R = 1852 * (Convert.ToDouble(acc_ADSB_Table_255.Rows[i][4]) - Convert.ToDouble(acc_MLAT_Table_255.Rows[i][4]));
                mean_error_R += precision_R;
                // FL*100 (feet) --> *0.3048 (to meters)
                double altitude_precision = 100 * (Convert.ToDouble(acc_ADSB_Table_255.Rows[i][5]) - Convert.ToDouble(acc_MLAT_Table_255.Rows[i][5]));
                mean_error_alt += altitude_precision;

                ResultsTable_255.Rows.Add(callsign, time, Math.Round(precision_lat, 8), Math.Round(precision_lon, 8), Math.Round(precision_R, 8), Math.Round(altitude_precision, 8));
            }
            Res_Table.ItemsSource = ResultsTable_255.DefaultView;

            if (ADSB_Table_255.Rows.Count == 0 || MLAT_Table.Rows.Count == 0) { MessageBox.Show("Any flight within this range interval.", "WARNING"); }
            else
            {
                Pd = Probability_of_Detection(ADSB_Table_255, MLAT_Table);
                double lat_percentil = Percentile(2, 0.95, ResultsTable_255);
                double lon_percentil = Percentile(3, 0.95, ResultsTable_255);
                double dist_percentil = Percentile(4, 0.95, ResultsTable_255);
                double alt_percentil = Percentile(5, 0.95, ResultsTable_255);

                Lat_Av.Content = "Latitude: " + Math.Round(mean_error_lat / acc_ADSB_Table_255.Rows.Count, 5) + " º / " + Math.Round(lat_percentil, 5) + " º";
                Lon_Av.Content = "Longitude: " + Math.Round(mean_error_lon / acc_ADSB_Table_255.Rows.Count, 5) + " º / " + Math.Round(lon_percentil, 5) + " º";
                Dist_Av.Content = "Distance: " + Math.Round(mean_error_R / acc_ADSB_Table_255.Rows.Count, 5) + " m / " + Math.Round(dist_percentil, 6) + " m";
                Alt_Av.Content = "Altitude: " + Math.Round(mean_error_alt / acc_ADSB_Table_255.Rows.Count, 5) + " ft / " + Math.Round(alt_percentil, 5) + " ft";
                Pd_10NM.Content = "Pd: " + Math.Round(Pd, 5) + " %";
            }
            progressbar.Visibility = Visibility.Hidden;
        }
        private void getresults025_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Uploaded file has " + acc_MLAT_Table_025.Rows.Count + " packages within this range.\nPlease be patient, this may take several seconds.", "WARNING");
            progressbar.Visibility = Visibility.Visible;

            ResultsTable_025.Clear();
            double mean_error_lat = new double();
            double mean_error_lon = new double();
            double mean_error_alt = new double();
            double mean_error_R = new double();

            TableMLAT.ItemsSource = acc_MLAT_Table_025.DefaultView;
            TableADSB.ItemsSource = acc_ADSB_Table_025.DefaultView;
            //rellenamos la tabla de resultados con la resta de posiciones entre adsb y mlat más el quality indicator
            for (int i = 0; i < acc_ADSB_Table_025.Rows.Count; i++)
            {
                string callsign = acc_ADSB_Table_025.Rows[i][0].ToString();
                string time = acc_ADSB_Table_025.Rows[i][1].ToString();

                double precision_lat = Convert.ToDouble(acc_ADSB_Table_025.Rows[i][2]) - (Convert.ToDouble(acc_MLAT_Table_025.Rows[i][2]));
                double precision_lon = Convert.ToDouble(acc_ADSB_Table_025.Rows[i][3]) - (Convert.ToDouble(acc_MLAT_Table_025.Rows[i][3]));
                mean_error_lat += precision_lat;
                mean_error_lon += precision_lon;
                // R [m]
                double precision_R = 1852 * (Convert.ToDouble(acc_ADSB_Table_025.Rows[i][4]) - Convert.ToDouble(acc_MLAT_Table_025.Rows[i][4]));
                mean_error_R += precision_R;
                // FL*100 (feet) --> *0.3048 (to meters)
                double altitude_precision = 100 * (Convert.ToDouble(acc_ADSB_Table_025.Rows[i][5]) - Convert.ToDouble(acc_MLAT_Table_025.Rows[i][5]));
                mean_error_alt += altitude_precision;

                ResultsTable_025.Rows.Add(callsign, time, Math.Round(precision_lat, 8), Math.Round(precision_lon, 8), Math.Round(precision_R, 8), Math.Round(altitude_precision, 8));
            }
            Res_Table.ItemsSource = ResultsTable_025.DefaultView;

            if (ADSB_Table_025.Rows.Count == 0 || MLAT_Table.Rows.Count == 0) { MessageBox.Show("Any flight within this range interval.", "WARNING"); }
            else
            {
                Pd = Probability_of_Detection(ADSB_Table_025, MLAT_Table);
                double lat_percentil = Percentile(2, 0.95, ResultsTable_025);
                double lon_percentil = Percentile(3, 0.95, ResultsTable_025);
                double dist_percentil = Percentile(4, 0.95, ResultsTable_025);
                double alt_percentil = Percentile(5, 0.95, ResultsTable_025);

                Lat_Av.Content = "Latitude: " + Math.Round(mean_error_lat / acc_ADSB_Table_025.Rows.Count, 5) + " º / " + Math.Round(lat_percentil, 5) + " º";
                Lon_Av.Content = "Longitude: " + Math.Round(mean_error_lon / acc_ADSB_Table_025.Rows.Count, 5) + " º / " + Math.Round(lon_percentil, 5) + " º";
                Dist_Av.Content = "Distance: " + Math.Round(mean_error_R / acc_ADSB_Table_025.Rows.Count, 5) + " m / " + Math.Round(dist_percentil, 6) + " m";
                Alt_Av.Content = "Altitude: " + Math.Round(mean_error_alt / acc_ADSB_Table_025.Rows.Count, 5) + " ft / " + Math.Round(alt_percentil, 5) + " ft";
                Pd_10NM.Content = "Pd: " + Math.Round(Pd, 5) + " %";
            }            

            progressbar.Visibility = Visibility.Collapsed;
        }
        private void getresults0_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Uploaded file has " + acc_MLAT_Table_G.Rows.Count + " packages within this range.\nPlease be patient, this may take several seconds.", "WARNING");
            progressbar.Visibility = Visibility.Visible;

            ResultsTable_G.Clear();
            double mean_error_lat = new double();
            double mean_error_lon = new double();
            double mean_error_alt = new double();
            double mean_error_R = new double();

            TableMLAT.ItemsSource = acc_MLAT_Table_G.DefaultView;
            TableADSB.ItemsSource = acc_ADSB_Table_G.DefaultView;
            //rellenamos la tabla de resultados con la resta de posiciones entre adsb y mlat más el quality indicator
            for (int i = 0; i < acc_ADSB_Table_G.Rows.Count; i++)
            {
                string callsign = acc_ADSB_Table_G.Rows[i][0].ToString();
                string time = acc_ADSB_Table_G.Rows[i][1].ToString();

                double precision_lat = Convert.ToDouble(acc_ADSB_Table_G.Rows[i][2]) - (Convert.ToDouble(acc_MLAT_Table_G.Rows[i][2]));
                double precision_lon = Convert.ToDouble(acc_ADSB_Table_G.Rows[i][3]) - (Convert.ToDouble(acc_MLAT_Table_G.Rows[i][3]));
                mean_error_lat += precision_lat;
                mean_error_lon += precision_lon;
                // R [m]
                double precision_R = 1852 * (Convert.ToDouble(acc_ADSB_Table_G.Rows[i][4]) - Convert.ToDouble(acc_MLAT_Table_G.Rows[i][4]));
                mean_error_R += precision_R;
                // FL*100 (feet) --> *0.3048 (to meters)
                double altitude_precision = 100 * (Convert.ToDouble(acc_ADSB_Table_G.Rows[i][5]) - Convert.ToDouble(acc_MLAT_Table_G.Rows[i][5]));
                mean_error_alt += altitude_precision;

                ResultsTable_G.Rows.Add(callsign, time, Math.Round(precision_lat, 8), Math.Round(precision_lon, 8), Math.Round(precision_R, 8), Math.Round(altitude_precision, 8));
            }
            Res_Table.ItemsSource = ResultsTable_G.DefaultView;
            if (ADSB_Table_G.Rows.Count == 0 || MLAT_Table.Rows.Count == 0) { MessageBox.Show("Any flight within this range interval.", "WARNING"); }
            else
            {
                Pd = Probability_of_Detection(ADSB_Table_G, MLAT_Table);
                double lat_percentil = Percentile(2, 0.95, ResultsTable_G);
                double lon_percentil = Percentile(3, 0.95, ResultsTable_G);
                double dist_percentil = Percentile(4, 0.95, ResultsTable_G);
                double alt_percentil = Percentile(5, 0.95, ResultsTable_G);

                Lat_Av.Content = "Latitude: " + Math.Round(mean_error_lat / acc_ADSB_Table_G.Rows.Count, 5) + " º / " + Math.Round(lat_percentil, 5) + " º";
                Lon_Av.Content = "Longitude: " + Math.Round(mean_error_lon / acc_ADSB_Table_G.Rows.Count, 5) + " º / " + Math.Round(lon_percentil, 5) + " º";
                Dist_Av.Content = "Distance: " + Math.Round(mean_error_R / acc_ADSB_Table_G.Rows.Count, 5) + " m / " + Math.Round(dist_percentil, 6) + " m";
                Alt_Av.Content = "Altitude: " + Math.Round(mean_error_alt / acc_ADSB_Table_G.Rows.Count, 5) + " ft / " + Math.Round(alt_percentil, 5) + " ft";
                Pd_10NM.Content = "Pd: " + Math.Round(Pd, 5) + " %";
            }
            progressbar.Visibility = Visibility.Collapsed;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }      
        private double Probability_of_Detection(DataTable tableADSB, DataTable tableMLAT)
        {
            int count = 0;
            string time = tableADSB.Rows[0][1].ToString();
            string[] time_0_v = time.Split(':');
            int time_0_s = M.gettimecorrectly(time_0_v);

            string horaUlt = tableADSB.Rows[tableADSB.Rows.Count - 1][1].ToString();
            string[] horaUlt_V = horaUlt.Split(':');

            List<string> reportsADSB = new List<string>();
            List<string> reportsMLAT = new List<string>();
            for (int i = 0; i < tableADSB.Rows.Count; i++)
            {
                reportsADSB.Add(tableADSB.Rows[i][1].ToString());
            }
            for (int i = 0; i < tableMLAT.Rows.Count; i++)
            {
                reportsMLAT.Add(tableMLAT.Rows[i][1].ToString());
            }
            for (int i = 0; i < reportsADSB.Count; i++)
            {
                string[] time_v = reportsADSB[i].Split(':');
                int time_s = M.gettimecorrectly(time_v);
                time_s++;
                string time_1 = M.convert_to_hms(time_s);

                if (reportsMLAT.Contains(reportsADSB[i]) || reportsMLAT.Contains(time_1))
                {
                    count++;
                }
                time_0_s = time_0_s + 4;
            }
            return 100 * Convert.ToDouble(count) / reportsADSB.Count;
        }
        private double Percentile(int col, double percentile, DataTable results)
        {
            int len = results.Rows.Count;
            double[] columna = new double[len];
            for (int n = 0; n < len; n++)
            {
                columna[n] = Math.Abs(Convert.ToDouble(results.Rows[n][col]));
            }
            Array.Sort(columna);
            double realIndex = percentile * (columna.Length - 1);
            int index = (int)realIndex;
            double frac = realIndex - index;
            if (index + 1 < columna.Length)
                return columna[index] * (1 - frac) + columna[index + 1] * frac;
            else
                return columna[index];
        }
        private double[] Extrapolation(double dT, double Vx, double Vy)
        {
            double[] Extr = new double[2];
            double Dx = Vx * dT;
            double Dy = Vy * dT;
            double MLAT_lat = 41.0 + (17.0 / 60.0) + (49.0 / 3600.0) + (426.0 / 3600000.0);
            double MLAT_lon = 2.0 + (4.0 / 60.0) + (42.0 / 3600.0) + (410.0 / 3600000.0);
            // 1m * (1 NM/ 1852 m) * (1 º / 60 NM)
            Extr[0] = M.cartesiantolatmlat(Dx, Dy) - MLAT_lat; // LAT [º]
            Extr[1] = M.cartesiantolonmlat(Dx, Dy) - MLAT_lon; // LON [º]

            return Extr;
        }


    }
}