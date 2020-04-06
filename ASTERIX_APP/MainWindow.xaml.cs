using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CLASSES;
using Microsoft.Win32;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Windows.Threading;
using System.Data;

namespace ASTERIX_APP
{
    public partial class MainWindow : Window
    {
        Fichero F;
        int category;
        int i = 0;
        //lat and lon os cat10 files 
        double latindegrees;
        double lonindegrees;
        //Datatable that shows flights for each second (used at the map)
        DataTable updatedtable = new DataTable();
        //lat lon of MLAT system of reference (at LEBL airport)
        double MLAT_lat = 41.0 + (17.0/60.0)+(49.0/3600.0)+(426.0/3600000.0);
        double MLAT_lon = 2.0 + (4.0 / 60.0) + (42.0 / 3600.0) + (410.0 / 3600000.0);

        public MainWindow()
        {
            InitializeComponent();

            bubbleWord.Visibility = Visibility.Visible;
            circle.Visibility = Visibility.Visible;
            circle2.Visibility = Visibility.Visible;
            asterixPNG.Visibility = Visibility.Visible;
            Instructions_Label.Visibility = Visibility.Visible; ;
            Instructions_Label.FontSize = 18;
            Instructions_Label.Content = "Hi I'm Asterix, welcome to my App!" + '\n' + '\n' + "I need some file to read." + '\n' +
                "Please, load a '.ast' file using the 'Load File' button above.";
            bubbleWord.Height = 150;
            bubbleWord.Width = 550;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Are you sure you want to exit?");
            this.Close();
        }
        public void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            // HIDE BUTTONS
            map.Visibility = Visibility.Hidden;
            MapButton.Visibility = Visibility.Hidden;
            TableButton.Visibility = Visibility.Hidden;

            // HIDE AND CLEAN TABLES
            Track_Table.Visibility = Visibility.Hidden;
            gridlista.Visibility = Visibility.Hidden;
            Track_Table.ItemsSource = null;
            Track_Table.Items.Clear();

            // HIDE VISUAL ELEMENTS
            bubbleWord.Visibility = Visibility.Hidden;
            circle.Visibility = Visibility.Hidden;
            circle2.Visibility = Visibility.Hidden;
            asterixPNG.Visibility = Visibility.Hidden;
            asterixPerf.Visibility = Visibility.Hidden;

            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "AST |*.ast";

            Instructions_Label.Visibility = Visibility.Visible;
            Instructions_Label.Content = "Loading...";
            OpenFile.ShowDialog();

            // When you click Cancel
            if(OpenFile.FileName == "")
            {
                asterixPNG.Visibility = Visibility.Visible;
                Instructions_Label.Content = "Any file was loaded,\nPlease select a '.ast' file";
                bubbleWord.Height = 100;
                bubbleWord.Width = 550;
                bubbleWord.Visibility = Visibility.Visible;
                circle.Visibility = Visibility.Visible;
                circle2.Visibility = Visibility.Visible;
            }
            // When slecting a correct file
            else
            {                
                F = new Fichero(OpenFile.FileName);
                F.leer();

                asterixPerf.Visibility = Visibility.Visible;
                Instructions_Label.Content = "Perfectly read! Let's get started!" + '\n' + "1) View the file's data by clicking on 'Tracking Table'" +
                        '\n' + "2) Run an amazing simulation by clicking on 'Tracking Map'";
                bubbleWord.Height = 100;
                bubbleWord.Width = 550;
                bubbleWord.Visibility = Visibility.Visible;
                circle.Visibility = Visibility.Visible;
                circle2.Visibility = Visibility.Visible;

                MapButton.Visibility = Visibility.Visible; ;
                TableButton.Visibility = Visibility.Visible;
            }
        }
        private void TableTrack_Click(object sender, RoutedEventArgs e)
        {
            Instructions_Label.Visibility = Visibility.Hidden;
            Track_Table.Visibility = Visibility.Visible;
            map.Visibility = Visibility.Hidden;
            gridlista.Visibility = Visibility.Hidden;
            asterixPNG.Visibility = Visibility.Hidden;
            asterixPerf.Visibility = Visibility.Hidden;
            bubbleWord.Visibility = Visibility.Hidden;
            circle.Visibility = Visibility.Hidden;
            circle2.Visibility = Visibility.Hidden;
            StartButton.Visibility = Visibility.Hidden;
            StopButton.Visibility = Visibility.Hidden;
            timer.Visibility = Visibility.Hidden;
            x1butt.Visibility = Visibility.Hidden;
            x2butt.Visibility = Visibility.Hidden;
            x4butt.Visibility = Visibility.Hidden;
            zoomlebl.Visibility = Visibility.Hidden;
            zoombcn.Visibility = Visibility.Hidden;
            zoomcat.Visibility = Visibility.Hidden;

            if (F.CAT_list[0] == 10)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(21);
                if (IsMultipleCAT == true)
                {
                    Track_Table.ItemsSource = F.getTablaMixtCAT().DefaultView;
                    category = 1021;
                }
                else
                {
                    Track_Table.ItemsSource = F.getTablaCAT10().DefaultView;
                    category = 10;
                }
            }
            if (F.CAT_list[0] == 21)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(10);
                if (IsMultipleCAT == true)
                {
                    Track_Table.ItemsSource = F.getTablaMixtCAT().DefaultView;
                    category = 1021;
                }
                else
                {
                    Track_Table.ItemsSource = F.getTablaCAT21().DefaultView;
                    category = 21;
                }
            }
        }
        private void MapTrack_Click(object sender, RoutedEventArgs e)
        {
            Instructions_Label.Visibility = Visibility.Hidden;
            Track_Table.Visibility = Visibility.Hidden;
            map.Visibility = Visibility.Visible;
            gridlista.Visibility = Visibility.Visible;
            asterixPNG.Visibility = Visibility.Hidden;
            asterixPerf.Visibility = Visibility.Hidden;
            bubbleWord.Visibility = Visibility.Hidden;
            circle.Visibility = Visibility.Hidden;
            circle2.Visibility = Visibility.Hidden;
            StartButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Visible;
            timer.Visibility = Visibility.Visible;
            x1butt.Visibility = Visibility.Visible;
            x2butt.Visibility = Visibility.Visible;
            x4butt.Visibility = Visibility.Visible;
            zoomlebl.Visibility = Visibility.Visible;
            zoombcn.Visibility = Visibility.Visible;
            zoomcat.Visibility = Visibility.Visible;

            if (F.CAT_list[0] == 10)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(21);
                if (IsMultipleCAT == true) { gridlista.ItemsSource = F.getmultiplecattablereducida().DefaultView; }
                else { gridlista.ItemsSource = F.gettablacat10reducida().DefaultView; }
            }
            if (F.CAT_list[0] == 21)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(10);
                if (IsMultipleCAT == true) { gridlista.ItemsSource = F.getmultiplecattablereducida().DefaultView; }
                else { gridlista.ItemsSource = F.gettablacat21reducida().DefaultView; }
            }
        }

        private void Map_Load(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            map.MapProvider = OpenStreetMapProvider.Instance;
            map.MinZoom = 8;
            map.MaxZoom = 16;
            map.Zoom = 14;           
            map.Position = new PointLatLng(MLAT_lat,MLAT_lon);
            map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            map.CanDragMap = true;
            map.DragButton = MouseButton.Left;
        }
        public void addMarker_Click(object sender, RoutedEventArgs e)
        {
            if (F.CAT_list[0] == 10)
            {
                dt_Timer.Tick += dt_Timer_Tick;
                   
            }
            dt_Timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            dt_Timer.Start();
            updatedtable = F.gettablacat10reducida().Clone();

        }

        public void AddMarker(double latitude, double longitude)
        {
            PointLatLng point = fromXYtoLatLongMLAT(latitude , longitude);
            GMapMarker marker = new GMapMarker(point);
            marker.Shape = new Image
            {
                Width = 15,
                Height = 15,
                Source = new BitmapImage(new Uri("pack://application:,,,/Images/airplane.png"))
            };
            map.Markers.Add(marker);
        }
        DispatcherTimer dt_Timer = new DispatcherTimer();

        double s = 0;
        private void dt_Timer_Tick(object sender,EventArgs e)
        {
            Boolean x = true;
            while (x == true)
            {
                CAT10 C10 = F.getCAT10(i);
                double start = Math.Floor(F.getCAT10(0).Time_Day)+s;
                double tiempo = Math.Floor(C10.Time_Day);
                if (tiempo == start)
                {
                    AddMarker(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
                    i++;
                    if (map.Markers.Count >= 200)
                    {
                        map.Markers[map.Markers.Count - 200].Clear();
                    }
                }
                else
                {
                    x = false;
                    s++;
                }
                clock(tiempo);
                gridlista.Visibility = Visibility.Hidden;
                updatedlista.Visibility = Visibility.Visible;
                rellenartablaCAT10(i);
            }
        }
        private void rellenartablaCAT10(int i)
        {
            //we copy/paste all data from that specific flight
            updatedtable.ImportRow(F.gettablacat10reducida().Rows[i]);
            updatedlista.ItemsSource = updatedtable.DefaultView;
         
        }
      
        private void clock(double tiempo)
        {
            TimeSpan time = TimeSpan.FromSeconds(tiempo);
            string tiempoact = time.ToString(@"hh\:mm\:ss");
            timer.Text = tiempoact;
        }
        private void stop_Click (object sender, RoutedEventArgs e)
        { dt_Timer.Stop(); }
        private PointLatLng fromXYtoLatLongMLAT(double X, double Y)
        {
            double R = 6371 * 1000;
            double d = Math.Sqrt((X * X) + (Y * Y));
            double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
            double φ1 = MLAT_lat * (Math.PI / 180);
            double λ1 = MLAT_lon * (Math.PI / 180);
            var φ2 = Math.Asin(Math.Sin(φ1) * Math.Cos(d / R) + Math.Cos(φ1) * Math.Sin(d / R) * Math.Cos(brng));
            var λ2 = λ1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(φ1), Math.Cos(d / R) - Math.Sin(φ1) * Math.Sin(φ2));

            PointLatLng coordinates = new PointLatLng(φ2 * (180 / Math.PI), λ2 * (180 / Math.PI));
            return coordinates;
        }
      
        public double getlatMLAT()
        {
            return latindegrees;
        }
        public double getlonMLAT()
        {
            return lonindegrees;
        }

        //to change the refreshing speed of the map files
        private void x1_Click(object sender, RoutedEventArgs e)
        {
            dt_Timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
        }
        private void x2_Click(object sender, RoutedEventArgs e)
        {
            dt_Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }
        private void x4_Click(object sender, RoutedEventArgs e)
        {
            dt_Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
        }

        //to change map zoom
        private void zoomlebl_Click(object sender, RoutedEventArgs e)
        {
            map.Zoom = 14;
        }
        private void zoombcn_Click(object sender, RoutedEventArgs e)
        {
            map.Zoom = 11;
        }
        private void zoomcat_Click(object sender, RoutedEventArgs e)
        {
            map.Zoom = 7;
        }

    
        void ClickDataGrid(object sender, RoutedEventArgs e) // When we click over a clickable cell
        {
            DataGridCell cell = (DataGridCell)sender;
            // pick the column number of the clicked cell
            int Col_Num = cell.Column.DisplayIndex;
            // pick the row number of the clicked cell
            DataGridRow row = DataGridRow.GetRowContainingElement(cell);
            int Row_Num = row.GetIndex();

            // CAT 10 case
            if (category == 10)
            {
                CAT10 pack = F.getCAT10(Row_Num);
                if (Col_Num == 1 && pack.Target_ID!= null)
                {
                    MessageBox.Show(pack.Target_ID[0]+"/"+pack.Target_ID[1]);
                }
                if (Col_Num == 4 && pack.Target_Rep_Descript != null)
                {
                    string[] TRD = pack.Target_Rep_Descript;
                    MessageBox.Show("Target Report:\n\nTYP: " + TRD[0] + "\nDCR: " + TRD[1] + "\nCHN: " + TRD[2] + "\nGBS: " + TRD[3] +
                        "\nCRT: " + TRD[4] + "\nSIM: " + TRD[5] + "\nTST: " + TRD[6] + "\nRAB" + TRD[7] + "\nLOP: " + TRD[8] + "\nTOT: " +
                        TRD[9] + "\nSPI" + TRD[10]);
                }
                if (Col_Num == 12 && pack.Track_Status != null)
                {
                    string[] TS = pack.Track_Status;
                    MessageBox.Show("Track Status:\n\nCNF: " + TS[0] + "\nTRE: " + TS[1] + "\nCST: " + TS[2] + "\nMAH: " + TS[3] +
                        "\nTCC: " + TS[4] + "\nSTH: " + TS[5] + "\nTOM: " + TS[6] + "\nDOU: " + TS[7] + "\nMRS: " + TS[8] + "\nGHO: " + TS[9]);
                }
                if (Col_Num == 13 && pack.Mode3A_Code != null)
                {
                    string[] M3A = pack.Mode3A_Code;
                    MessageBox.Show("Mode 3/A Code:\n\nV: " + M3A[0] + "\nG: " + M3A[1] + "\nL: " + M3A[2] + "\n Code: " + M3A[3]);
                }
                if (Col_Num == 15 && pack.Mode_SMB != null)
                {
                    string[] MSMB = pack.Mode_SMB;
                    MessageBox.Show("Mode S MB:\n\nREP: " + MSMB[0] + "\nMB: " + MSMB[1] + "\nBDS 1: " + MSMB[2] + "\nBDS 2: " + MSMB[3]);
                }
                if (Col_Num == 21 && pack.Sys_Status != null)
                {
                    string[] SS = pack.Sys_Status;
                    MessageBox.Show("System Status:\n\nNOGO: " + SS[0] + "\nOVL: " + SS[1] + "\nTSV: " + SS[2] + "\nDIV: " + SS[3] +
                        "\nTTF: " + SS[5]);
                }
                if (Col_Num == 22 && pack.Pre_Prog_Message != null)
                {
                    string[] PPM = pack.Pre_Prog_Message;
                    MessageBox.Show("Pre-Programmed Message:\n\nTRB: " + PPM[0] + "Message: " + PPM[1]);
                }
                if (Col_Num == 25 && pack.Presence != null)
                {
                    double[] P = pack.Presence;
                    MessageBox.Show("Presence:\n\nREP: " + P[0] + "\nDifference of Rho: " + P[1] + "\nDifference of Theta: " + P[2]);
                }
            }
            // CAT 21 case
            if (category == 21)
            {
                CAT21 pack = F.getCAT21(Row_Num);
                if (Col_Num == 4 && pack.Target_Report_Desc != null)
                {
                    string[] TRD = pack.Target_Report_Desc;
                }
                if (Col_Num == 10 && pack.Op_Status != null)
                {
                    string[] OS = pack.Op_Status;
                }
                if (Col_Num == 16 && pack.MOPS != null)
                {
                    string[] MOPS = pack.MOPS;
                }
                if (Col_Num == 21 && pack.Met_Report != null)
                {
                    string[] MR = pack.Met_Report;
                }
                if (Col_Num == 24 && pack.Target_Status != null)
                {
                    string[] TS = pack.Target_Status;
                }
                if (Col_Num == 27 && pack.Quality_Indicators != null)
                {
                    string[] QI = pack.Quality_Indicators;
                }
                if (Col_Num == 28 && pack.Mode_S != null)
                {
                    int[] MS = pack.Mode_S;
                }
                if (Col_Num == 35 && pack.TMRP_HP != null)
                {
                    string[] TMRP = pack.TMRP_HP;
                }
                if (Col_Num == 36 && pack.TMRV_HP != null)
                {
                    string[] TMRV = pack.TMRV_HP;
                }
                if (Col_Num == 38 && pack.Trajectory_Intent != null)
                {
                    string[] TI = pack.Trajectory_Intent;
                }
                if (Col_Num == 39 && pack.Data_Ages != null)
                {
                    double[] DA = pack.Data_Ages;
                }
            }
            // Mixt category
            if (category == 1021)
            {
                CAT10 pack10 = F.getCAT10(Row_Num);
                CAT21 pack21 = F.getCAT21(Row_Num);
                DataTable tabla = F.getTablaMixtCAT();
                int cat = Convert.ToInt32(tabla.Rows[Row_Num][1]);

                if (cat == 10 && Col_Num == 6 && pack10.Target_Rep_Descript != null)
                {
                    string[] TRD = pack10.Target_Rep_Descript;
                    MessageBox.Show("Target Report:\n\nTYP: " + TRD[0] + "\nDCR: " + TRD[1] + "\nCHN: " + TRD[2] + "\nGBS: " + TRD[3] +
                        "\nCRT: " + TRD[4] + "\nSIM: " + TRD[5] + "\nTST: " + TRD[6] + "\nRAB" + TRD[7] + "\nLOP: " + TRD[8] + "\nTOT: " +
                        TRD[9] + "\nSPI" + TRD[10]);
                }
                if (cat == 21 && Col_Num == 6 && pack21.Target_Report_Desc != null)
                {
                    string[] TRD = pack21.Target_Report_Desc;
                    MessageBox.Show("Target Report:\n\nATP: " + TRD[0] + "\nARC: " + TRD[1] + "\nRC: " + TRD[2] + "\nRAB :" + TRD[3] +
                        "\nDCR: " + TRD[4] + "\nGBS: " + TRD[5] + "\nSIM: " + TRD[6] + "\nTST: " + TRD[7] + "\nSAA: " + TRD[8] + "\nCL: " +
                        TRD[9] + "\nIPC: " + TRD[10] + "\nNOGO" + TRD[11] + "\nCPR: " + TRD[12] + "\nLDPJ: " + TRD[13] + "\nRCF: " + TRD[14]);
                }
                if (cat == 10 && Col_Num == 8 && pack10.Mode3A_Code != null)
                {
                    string[] M3A = pack10.Mode3A_Code;
                    MessageBox.Show("Mode 3/A Code:\n\nV: " + M3A[0] + "\nG: " + M3A[1] + "\nL: " + M3A[2] + "\n Code: " + M3A[3]);
                }
                if (cat == 21 && Col_Num == 8 && pack21.M3AC != null)
                {

                }
                if (cat == 10 && Col_Num == 9 && pack10.Mode_SMB != null)
                {
                    string[] MSMB = pack10.Mode_SMB;
                    MessageBox.Show("Mode S MB:\n\nREP: " + MSMB[0] + "\nMB: " + MSMB[1] + "\nBDS 1: " + MSMB[2] + "\nBDS 2: " + MSMB[3]);
                }
                if (cat == 21 && Col_Num == 9 && pack21.Mode_S != null)
                {
                    int[] MS = pack21.Mode_S;
                    MessageBox.Show("Mode S MB Data:\n\nRep. Mode S MB Data: " + MS[0] + "\nMB Data: " + MS[1] + "\nBDS 1: " + MS[2] +
                        "\nBDS 2: " + MS[3]);
                }
                if (Col_Num == 21 && pack10.Track_Status != null)
                {
                    string[] TS = pack10.Track_Status;
                    MessageBox.Show("Track Status:\n\nCNF: " + TS[0] + "\nTRE: " + TS[1] + "\nCST: " + TS[2] + "\nMAH: " + TS[3] +
                        "\nTCC: " + TS[4] + "\nSTH: " + TS[5] + "\nTOM: " + TS[6] + "\nDOU: " + TS[7] + "\nMRS: " + TS[8] + "\nGHO: " + TS[9]);
                }
                if (Col_Num == 23 && pack10.Sys_Status != null)
                {
                    string[] SS = pack10.Sys_Status;
                    MessageBox.Show("System Status:\n\nNOGO: " + SS[0] + "\nOVL: " + SS[1] + "\nTSV: " + SS[2] + "\nDIV: " + SS[3] +
                        "\nTTF: " + SS[5]);
                }
                if (Col_Num == 24 && pack10.Pre_Prog_Message != null)
                {
                    string[] PPM = pack10.Pre_Prog_Message;
                    MessageBox.Show("Pre-Programmed Message:\n\nTRB: " + PPM[0] + "Message: " + PPM[1]);
                }
                if (Col_Num == 27 && pack10.Presence != null)
                {
                    double[] P = pack10.Presence;
                    MessageBox.Show("Presence:\n\nREP: " + P[0] + "\nDifference of Rho: " + P[1] + "\nDifference of Theta: " + P[2]);
                }
                if (Col_Num == 30 && pack21.Op_Status != null)
                {
                    string[] OS = pack21.Op_Status;
                    MessageBox.Show("Operational Status:\n\nRA: " + OS[0] + "\nTC: " + OS[1] + "\nTS: " + OS[2] + "\nARV: " + OS[3] +
                        "\nCDITA: " + OS[4] + "\nNot TCAS: " + OS[5] + "\nSing. Ant.: " + OS[6]);
                }
                if (Col_Num == 36 && pack21.MOPS != null)
                {
                    string[] MOPS = pack21.MOPS;
                    MessageBox.Show("MOPS Version:\n\nVNS: " + MOPS[0] + "\nVN: " + MOPS[1] + "\nLTT: " + MOPS[2]);
                }
                if (Col_Num == 40 && pack21.Met_Report != null)
                {
                    string[] MR = pack21.Met_Report;
                    MessageBox.Show("Met Report:\n\nWind Speed: " + MR[0] + "\nWind Direction: " + MR[1] + "\nTemperature: " + MR[2] +
                        "Turbulence: " + MR[3]);
                }
                if (Col_Num == 42 && pack21.Target_Status != null)
                {
                    string[] TS = pack21.Target_Status;
                    MessageBox.Show("Target Status:\n\nICF: " + TS[0] + "\nLNAV: " + TS[1] + "\nPS: " + TS[2] + "\nSS: " + TS[3]);
                }
                if (Col_Num == 45 && pack21.Quality_Indicators != null)
                {
                    string[] QI = pack21.Quality_Indicators;
                    MessageBox.Show("Quality Indicators:\n\n" + QI[0] + "\n" + QI[1] + "\n" + QI[2] + "\n" + "\nSIL Supplement: " +
                        QI[3] + "\nSDA: " + QI[4] + "\nGVA: " + QI[5] + "\nPIC: " + QI[6]);
                }
                if (Col_Num == 51 && pack21.TMRP_HP != null)
                {
                    string[] TMRP = pack21.TMRP_HP;
                    MessageBox.Show("Time of Message Reception for Position\nHigh Precision:\n\nFull Second Indication: " + TMRP[0] +
                        "\nTMR Posiotion: " + TMRP[1]);
                }
                if (Col_Num == 52 && pack21.TMRV_HP != null)
                {
                    string[] TMRV = pack21.TMRV_HP;
                    MessageBox.Show("Time of Message Reception for velocity\nHigh Precision:\n\nFull Second Indication: " + TMRV[0] +
                        "\nTMR Velocity: " + TMRV[1]);
                }
                if (Col_Num == 54 && pack21.Trajectory_Intent != null)
                {
                    string[] TI = pack21.Trajectory_Intent;
                    MessageBox.Show("Trajectory Intent:\n\n" + TI[0] + "\nNVB: " + TI[1] + "\nNAV: " + TI[2] + "\nREP: " + TI[3] + "\nTCA: " + TI[0] +
                        "\nNC: " + TI[5] + "\nTCP Number: " + TI[6] + "\nLatitude TID: " + TI[7] + "\nLongitude TID: " + TI[8] + "\nAltitude (feet): " +
                        TI[9] + "\nPoint Type: " + TI[10] + "\nTD: " + TI[11] + "\nTRA: " + TI[12] + "\nTOA: " + TI[13] + "\nTTR (NM): " + TI[15]);
                }
                if (Col_Num == 55 && pack21.Data_Ages != null)
                {
                    double[] DA = pack21.Data_Ages;
                    MessageBox.Show("Data Ages:\n\nAOS: " + DA[0] + "\nTRD: " + DA[1] + "\n Mode 3A: " + DA[2] + "\nQI: " + DA[3] + "\nTI: " + DA[4] +
                        "\nMAM: " + DA[5] + "\nGH: " + DA[6] + "\nFL: " + DA[7] + "\nISA: " + DA[8] + "\nFSA: " + DA[9] + "\nAS: " + DA[10] + "\nTAS: " +
                        DA[11] + "\nMH: " + DA[12] + "\nBVR: " + DA[13] + "\nGVR: " + DA[14] + "\nGV: " + DA[15] + "\nTAR: " + DA[16] + "\nTarget ID: " +
                        DA[17] + "\nTS: " + DA[18] + "Met: " + DA[19] + "\nROA: " + DA[20] + "\nARA: " + DA[21] + "\nSCC: " + DA[22]);
                }
            }
        }
    }
}
