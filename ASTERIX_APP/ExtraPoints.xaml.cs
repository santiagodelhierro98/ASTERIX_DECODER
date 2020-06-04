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
        DataTable ADSB_Table1 = new DataTable();
        DataTable ADSB_Table = new DataTable();
        DataTable acc_MLAT_Table = new DataTable();
        DataTable acc_ADSB_Table = new DataTable();


        DataTable ResultsTable = new DataTable();
        DataTable AverageTable = new DataTable();

        public ExtraPoints()
        {
            InitializeComponent();
            
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "AST |*.ast";
            OpenFile.ShowDialog();
            string path = OpenFile.FileName;            

            M.Create_ExtraTable_MLAT(MLAT_Table);
            M.Create_ExtraTable_ADSB(ADSB_Table1);            
            leerEX(path);

            ADSB_Table = ADSB_Reorganize(ADSB_Table1);
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
                    double modulo = E.checkdistanceMLAT(C10);                    
                    if (C10.Target_Rep_Descript[0] == "Mode S Multilateration" && C10.Target_ID != null && Convert.ToDouble(C10.FL[2]) > 0.0 && Convert.ToDouble(C10.FL[2]) < 500.0 && C10.FL[2] != null && modulo < 10)
                    {
                        double lat = M.cartesiantolatmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
                        double lon = M.cartesiantolonmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);

                        MLAT_Table.Rows.Add(C10.Target_ID, M.convert_to_hms(Math.Floor(C10.Time_Day)), Math.Round(lat, 8), Math.Round(lon, 8), Math.Round(modulo, 8), C10.FL[2], C10.Time_Day, C10.Track_Vel_Cartesian[0], C10.Track_Vel_Cartesian[1]);
                    }
                    else { }
                }
                if (CAT == 21)
                {
                    CAT21 C21 = new CAT21();
                    C21.Decode21(arraystring);
                    double modulo21 = E.checkdistanceADSB(C21);
                    if (C21.Target_ID != null && C21.FL != 0 && C21.FL <= 150.0 && C21.MOPS[1] == "ED102A/DO-260B [Ref. 11]" && modulo21 < 10)
                    {
                        double EPU = E.Horizontal_Accuracy_Pos(C21); // Horizonatl Accuracy (NACp)
                        double RC = Convert.ToDouble(C21.Quality_Indicators[6]); // Radius of Containments (NIC)
                        double GVA = E.Compute_GVA(C21); // Altitude Accuracy

                        ADSB_Table1.Rows.Add(C21.Target_ID, M.convert_to_hms(Math.Floor(C21.TMRP)), Math.Round(C21.Lat_WGS_84, 8), Math.Round(C21.Lon_WGS_84, 8), Math.Round(modulo21, 8), C21.FL, EPU, RC, GVA, C21.TMRP);
                    }
                    else { }
                }
            }
        }
        private DataTable ADSB_Reorganize(DataTable table)
        {
            DataTable new_table = new DataTable();
            M.Create_ExtraTable_ADSB(new_table);

            for (int i = 1; i<table.Rows.Count; i++)
            {
                if (table.Rows[i][0] == table.Rows[i -1][0] && table.Rows[i][1].ToString() == table.Rows[i - 1][1].ToString())
                {
                    double lat = (Convert.ToDouble(table.Rows[i][2]) + Convert.ToDouble(table.Rows[i - 1][2])) / 2;
                    double lon = (Convert.ToDouble(table.Rows[i][3]) + Convert.ToDouble(table.Rows[i - 1][3])) / 2;
                    double dist = (Convert.ToDouble(table.Rows[i][4]) + Convert.ToDouble(table.Rows[i - 1][4])) / 2;
                    double H = (Convert.ToDouble(table.Rows[i][5]) + Convert.ToDouble(table.Rows[i - 1][5])) / 2;
                    double sec = (Convert.ToDouble(table.Rows[i][9]) + Convert.ToDouble(table.Rows[i - 1][9])) / 2;

                    new_table.Rows.Add(table.Rows[i][0], table.Rows[i][1], lat, lon, dist, H, table.Rows[i][6], table.Rows[i][7], table.Rows[i][8], sec);
                }
                else
                {
                    new_table.ImportRow(table.Rows[i - 1]);
                }
            }
            return new_table;
        }

        double mean_error_lat;
        double mean_error_lon;
        double mean_error_alt;
        double mean_error_R;

        private async void getresults_Click(object sender, RoutedEventArgs e)
        {
            progressbar.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                M.Create_ExtraTable_ADSB(acc_ADSB_Table);
                M.Create_ExtraTable_MLAT(acc_MLAT_Table);
                M.Create_AverageTable(AverageTable);
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
                        else { }
                    }
                }
            });
            TableMLAT.ItemsSource = acc_MLAT_Table.DefaultView;
            TableADSB.ItemsSource = acc_ADSB_Table.DefaultView;
            await Task.Run(() =>
            {
                double Pd = Probability_of_Detection(ADSB_Table, MLAT_Table);
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
                    double precision_R = 1852*(Convert.ToDouble(acc_ADSB_Table.Rows[i][4]) - Convert.ToDouble(acc_MLAT_Table.Rows[i][4]));
                    mean_error_R += precision_R;
                    // FL*100 (feet) --> *0.3048 (to meters)
                    double altitude_precision = 100*(Convert.ToDouble(acc_ADSB_Table.Rows[i][5]) - Convert.ToDouble(acc_MLAT_Table.Rows[i][5]));
                    mean_error_alt += altitude_precision;                    

                    ResultsTable.Rows.Add(callsign, time, Math.Round(precision_lat, 8), Math.Round(precision_lon, 8), Math.Round(precision_R, 8), Math.Round(altitude_precision, 8));
                }
                double lat_percentil = Percentile(2, 0.95);
                double lon_percentil = Percentile(3, 0.95);
                double dist_percentil = Percentile(4, 0.95);
                double alt_percentil = Percentile(5, 0.95);

                AverageTable.Rows.Add(Math.Round(mean_error_lat / acc_ADSB_Table.Rows.Count, 5), Math.Round(mean_error_lon /acc_ADSB_Table.Rows.Count, 5), Math.Round(mean_error_R/acc_ADSB_Table.Rows.Count, 5), Math.Round(mean_error_alt / acc_ADSB_Table.Rows.Count, 5), Math.Round(Pd, 5));
                AverageTable.Rows.Add(Math.Round(lat_percentil, 5), Math.Round(lon_percentil, 5), Math.Round(dist_percentil, 6), Math.Round(alt_percentil, 5), "");
            });
            Res_Table.ItemsSource = ResultsTable.DefaultView;
            Av_Table.ItemsSource = AverageTable.DefaultView;

            progressbar.Visibility = Visibility.Hidden;
        }    
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private double Probability_of_Detection(DataTable tableADSB, DataTable tableMLAT)
        {
            int count = 0;
            string time = tableMLAT.Rows[0][1].ToString();
            string[] time_0_v = time.Split(':');
            int time_0_s = M.gettimecorrectly(time_0_v);

            string horaUlt = tableMLAT.Rows[tableADSB.Rows.Count - 1][1].ToString();
            string[] horaUlt_V = horaUlt.Split(':');
            int secs = M.gettimecorrectly(horaUlt_V) - time_0_s;

            List<string> reportsADSB = new List<string>();
            List<string> reportsMLAT = new List<string>();
            for (int i = 0; i<tableADSB.Rows.Count; i++)
            {
                reportsADSB.Add(tableADSB.Rows[i][1].ToString());                
            }
            for (int i = 0; i < tableMLAT.Rows.Count; i++)
            {
                reportsADSB.Add(tableMLAT.Rows[i][1].ToString());
            }
            while (time_0_s <= M.gettimecorrectly(horaUlt_V))
            {
                string clock = M.convert_to_hms(time_0_s);
                string clock_1 = M.convert_to_hms(time_0_s + 1);
                if ((reportsADSB.Contains(clock) || reportsADSB.Contains(clock_1)))
                {
                    count++;
                }
                time_0_s++;
            }
            return 100*Convert.ToDouble(count) / secs;
        }
        //private double Probability_of_Detection(DataTable tableMLAT, DataTable tableADSB)
        //{
        //    int count = 0;
        //    for(int i = 0; i<tableADSB.Rows.Count; i++)
        //    {
        //        string timeADSB = tableADSB.Rows[i][1].ToString();
        //        string[] timeADSB_v = timeADSB.Split(':');
        //        int timeADSB_s = M.gettimecorrectly(timeADSB_v);

        //        string callsignADSB = tableADSB.Rows[i][0].ToString();

        //        for(int j = 0; j<tableMLAT.Rows.Count; j++)
        //        {
        //            string timeMLAT = tableMLAT.Rows[j][1].ToString();
        //            string[] timeMLAT_v = timeMLAT.Split(':');
        //            int timeMLAT_s = M.gettimecorrectly(timeMLAT_v);

        //            string callsignMLAT = tableMLAT.Rows[j][0].ToString();
        //            if(timeADSB_s == timeMLAT_s && callsignADSB == callsignMLAT)
        //            {
        //                count++;
        //                break;
        //            }
        //            if(timeMLAT_s > timeADSB_s) { break; }
        //        }
        //    }
        //    double Pd = 100*Convert.ToDouble(count)/Convert.ToDouble(tableADSB.Rows.Count);
        //    return Pd;
        //}
        private double Percentile(int col, double percentile)
        {
            int len = ResultsTable.Rows.Count;
            double[] columna = new double[len];
            for (int n = 0; n < len; n++)
            {
                columna[n] = Math.Abs(Convert.ToDouble(ResultsTable.Rows[n][col]));
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
            double[] Extr = new double[4];
            double Dx = Vx * dT;
            double Dy = Vy * dT;
            double MLAT_lat = 41.0 + (17.0 / 60.0) + (49.0 / 3600.0) + (426.0 / 3600000.0);
            double MLAT_lon = 2.0 + (4.0 / 60.0) + (42.0 / 3600.0) + (410.0 / 3600000.0);
            // 1m * (1 NM/ 1852 m) * (1 º / 60 NM)
            Extr[0] = M.cartesiantolatmlat(Dx, Dy) - MLAT_lat; // LAT [º]
            Extr[1] = M.cartesiantolonmlat(Dx, Dy) - MLAT_lon; // LON [º]

            return Extr;
        }

        // SMR MLAT:

        // Position error = sqrt((x-x0)^2 + (y-y0)^2)
        // V Suplly = 230V
        // Tau = 40e-9 s
        // wR = 40 RPM
        // f = [9, 9.5]GHz or [15.4, 16.9]GHz
        // BeamWidth = 0.4º
        // Rmax = 2500 m
        // 250 targets min in 360º
        // P of False detection = 1e-4
        // P of False Identification = 1e-6

        // PRI = 2*Rmax/c
        // t_obs = BeamWidth/6*wR
        // n = t_obs/PRI
        // A = ln(0.62/P_False_Det)
        // SNRn = -5*log10(n) + (6.2 + 4.54/sqrt(n + 0.44))*log10(A + 0.12*A*B + 1.7*B)
        // B = (10^((SNRn + 5*log10(n))/(10*(6.2 + 4.54/sqrt(n + 0.44)))) - A)/(0.12*A + 1.7)
        // B = ln(Pd/(Pd-1)) --> Pd = -e^B/(1 - e^B)
    }
}