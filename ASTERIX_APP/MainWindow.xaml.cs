using CLASSES;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Microsoft.Win32;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ASTERIX_APP
{
    public partial class MainWindow : Window
    {
        Fichero F;
        //Extrapoints E = new Extrapoints();
        Metodos M = new Metodos();
        DispatcherTimer dt_Timer = new DispatcherTimer();

        bool chivato = false;
        int category;
        int i = 0;
        //Datatable that shows flights for each second (used at the map)
        DataTable updatedtable = new DataTable();
        //lat lon of MLAT system of reference (at LEBL airport)
        double MLAT_lat = 41.0 + (17.0 / 60.0) + (49.0 / 3600.0) + (426.0 / 3600000.0);
        double MLAT_lon = 2.0 + (4.0 / 60.0) + (42.0 / 3600.0) + (410.0 / 3600000.0);
        //lat lon of SMR system of reference (at LEBL airport)
        double SMR_lat = 41.0 + (17.0 / 60.0) + (44.0 / 3600.0) + (226.0 / 3600000.0);
        double SMR_lon = 2.0 + (5.0 / 60.0) + (42.0 / 3600.0) + (411.0 / 3600000.0);

        string searchedcallsign;
        bool idbuttonclicked = false;
        double start;
        double tiempo;
        double s = 0;
        int n = 0;

        public MainWindow()
        {
            InitializeComponent();
            progressbar.Visibility = Visibility.Collapsed;
            bubbleWord.Visibility = Visibility.Visible;
            circle.Visibility = Visibility.Visible;
            circle2.Visibility = Visibility.Visible;
            asterixPNG.Visibility = Visibility.Visible;
            Instructions_Label.Visibility = Visibility.Visible; ;
            arrow.Visibility = Visibility.Visible;
            Instructions_Label.FontSize = 18;
            Instructions_Label.Content = "Hi I'm Asterix, welcome to my App!" + '\n' + '\n' + "I need some file to read." + '\n' +
                "Please, load a '.ast' file using the 'Load File' button above.";
            bubbleWord.Height = 150;
            bubbleWord.Width = 550;
        }

        // MAIN APP BUTTONS
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    var inicio = new Inicio();
                    inicio.Show();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        public async void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            STOP_TRACK();
            Track_Table.ItemsSource = null;
            Track_Table.Items.Clear();
            gridlista.ItemsSource = null;

            progressbar.Visibility = Visibility.Collapsed;
            map.Visibility = Visibility.Collapsed;
            SearchNumButton.Visibility = Visibility.Collapsed;
            SearchIDButton.Visibility = Visibility.Collapsed;
            SearchAddButton.Visibility = Visibility.Collapsed;
            ClearSearch.Visibility = Visibility.Collapsed;
            NumBox.Visibility = Visibility.Collapsed;
            IDBox.Visibility = Visibility.Collapsed;
            AddBox.Visibility = Visibility.Collapsed;
            Search_Table.Visibility = Visibility.Collapsed;

            // HIDE AND CLEAN TABLES
            checktrail.Visibility = Visibility.Collapsed;
            map.Visibility = Visibility.Collapsed;
            StartButton.Visibility = Visibility.Collapsed;
            StopButton.Visibility = Visibility.Collapsed;
            timer.Visibility = Visibility.Collapsed;
            x1butt.Visibility = Visibility.Collapsed;
            x2butt.Visibility = Visibility.Collapsed;
            x4butt.Visibility = Visibility.Collapsed;
            zoomlebl.Visibility = Visibility.Collapsed;
            zoombcn.Visibility = Visibility.Collapsed;
            SearchMapbyID.Visibility = Visibility.Collapsed;
            callsignbox.Visibility = Visibility.Collapsed;
            StopSearchbytarget.Visibility = Visibility.Collapsed;
            ZOOM.Visibility = Visibility.Collapsed;
            Timer.Visibility = Visibility.Collapsed;
            speedlabel.Visibility = Visibility.Collapsed;

            RestartButton.Visibility = Visibility.Collapsed;
            Track_Table.Visibility = Visibility.Collapsed;
            gridlista.Visibility = Visibility.Collapsed;
            updatedlista.Visibility = Visibility.Collapsed;
            // HIDE VISUAL ELEMENTS
            bubbleWord.Visibility = Visibility.Collapsed;
            circle.Visibility = Visibility.Collapsed;
            circle2.Visibility = Visibility.Collapsed;
            asterixPNG.Visibility = Visibility.Collapsed;
            asterixPerf.Visibility = Visibility.Collapsed;
            arrow.Visibility = Visibility.Collapsed;
            Instructions_Label.Visibility = Visibility.Collapsed;

            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "AST |*.ast";
            //var progress = new Progress<int>(value => progressbar.Value = value);


            //Instructions_Label.Visibility = Visibility.Visible;
            //Instructions_Label.Content = "Loading...";
            OpenFile.ShowDialog();
            //Children.Add(progressbar);

            // When you click Cancel
            if (OpenFile.FileName == "")
            {
                chivato = false;
                asterixPNG.Visibility = Visibility.Visible;
                Instructions_Label.Visibility = Visibility.Visible;
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
                progressbar.Visibility = Visibility.Visible;
                await Task.Run(() =>
                {
                    chivato = true;
                    F = new Fichero(OpenFile.FileName);
                    //((IProgress<int>)progress).Report(20);
                    F.leer();
                    //computelistMLAT(F);
                    //computelistADSB(F);

                    //((IProgress<int>)progress).Report(100);
                });


                asterixPerf.Visibility = Visibility.Visible;
                progressbar.Visibility = Visibility.Collapsed;

                Instructions_Label.Visibility = Visibility.Visible;
                Instructions_Label.Content = "Perfectly read! Let's get started!" + '\n' + "1) View the file's data by clicking on 'Tracking Table'" +
                    '\n' + "2) Run an amazing simulation by clicking on 'Tracking Map'";
                bubbleWord.Height = 100;
                bubbleWord.Width = 550;
                bubbleWord.Visibility = Visibility.Visible;
                circle.Visibility = Visibility.Visible;
                circle2.Visibility = Visibility.Visible;
                Help.Visibility = Visibility.Visible;
                CheckCAT();

            }
        }
        private void TableTrack_Click(object sender, RoutedEventArgs e)
        {
            // When clicking button before loading a file
            if (chivato == false)
            {
                asterixPNG.Visibility = Visibility.Visible;
                Instructions_Label.Content = "Any file was loaded,\nPlease select a '.ast' file";
                bubbleWord.Height = 100;
                bubbleWord.Width = 550;
                bubbleWord.Visibility = Visibility.Visible;
                circle.Visibility = Visibility.Visible;
                circle2.Visibility = Visibility.Visible;
                arrow.Visibility = Visibility.Visible;

            }
            else
            {
                STOP_TRACK();
                // Table and Map Stuff
                progressbar.Visibility = Visibility.Hidden;
                Search_Table.Visibility = Visibility.Collapsed;
                SearchNumButton.Visibility = Visibility.Visible;
                SearchIDButton.Visibility = Visibility.Visible;
                SearchAddButton.Visibility = Visibility.Visible;
                ClearSearch.Visibility = Visibility.Visible;
                NumBox.Visibility = Visibility.Visible;
                IDBox.Visibility = Visibility.Visible;
                AddBox.Visibility = Visibility.Visible;

                Instructions_Label.Visibility = Visibility.Collapsed;
                Track_Table.Visibility = Visibility.Visible;
                map.Visibility = Visibility.Collapsed;
                gridlista.Visibility = Visibility.Collapsed;
                updatedlista.Visibility = Visibility.Collapsed;
                StartButton.Visibility = Visibility.Collapsed;
                StopButton.Visibility = Visibility.Collapsed;
                timer.Visibility = Visibility.Collapsed;
                x1butt.Visibility = Visibility.Collapsed;
                x2butt.Visibility = Visibility.Collapsed;
                x4butt.Visibility = Visibility.Collapsed;
                zoomlebl.Visibility = Visibility.Collapsed;
                zoombcn.Visibility = Visibility.Collapsed;
                updatedlista.Visibility = Visibility.Collapsed;
                SearchMapbyID.Visibility = Visibility.Collapsed;
                callsignbox.Visibility = Visibility.Collapsed;
                StopSearchbytarget.Visibility = Visibility.Collapsed;
                ZOOM.Visibility = Visibility.Collapsed;
                speedlabel.Visibility = Visibility.Collapsed;
                Timer.Visibility = Visibility.Collapsed;
                RestartButton.Visibility = Visibility.Collapsed;
                Help.Visibility = Visibility.Visible;
                // Visual Elements
                arrow.Visibility = Visibility.Collapsed;
                asterixPNG.Visibility = Visibility.Collapsed;
                asterixPerf.Visibility = Visibility.Collapsed;
                bubbleWord.Visibility = Visibility.Collapsed;
                circle.Visibility = Visibility.Collapsed;
                circle2.Visibility = Visibility.Collapsed;
                SearchNumButton.Visibility = Visibility.Visible;
                SearchIDButton.Visibility = Visibility.Visible;
                NumBox.Visibility = Visibility.Visible;
                IDBox.Visibility = Visibility.Visible;
                checktrail.Visibility = Visibility.Hidden;
                gridlista.Visibility = Visibility.Collapsed;


            }
        }
        private void MapTrack_Click(object sender, RoutedEventArgs e)
        {
            // When clicking button before loading a file
            if (chivato == false)
            {
                asterixPNG.Visibility = Visibility.Visible;
                Instructions_Label.Content = "Any file was loaded,\nPlease select a '.ast' file";
                bubbleWord.Height = 100;
                bubbleWord.Width = 550;
                bubbleWord.Visibility = Visibility.Visible;
                circle.Visibility = Visibility.Visible;
                circle2.Visibility = Visibility.Visible;
                arrow.Visibility = Visibility.Visible;
            }
            else
            {
                // Table and Map Stuff
                progressbar.Visibility = Visibility.Hidden;
                Search_Table.Visibility = Visibility.Collapsed;
                SearchNumButton.Visibility = Visibility.Collapsed;
                SearchIDButton.Visibility = Visibility.Collapsed;
                SearchAddButton.Visibility = Visibility.Collapsed;
                ClearSearch.Visibility = Visibility.Collapsed;
                NumBox.Visibility = Visibility.Collapsed;
                IDBox.Visibility = Visibility.Collapsed;
                AddBox.Visibility = Visibility.Collapsed;
                Instructions_Label.Visibility = Visibility.Collapsed;
                Track_Table.Visibility = Visibility.Collapsed;
                map.Visibility = Visibility.Visible;
                gridlista.Visibility = Visibility.Visible;
                StartButton.Visibility = Visibility.Visible;
                StopButton.Visibility = Visibility.Visible;
                timer.Visibility = Visibility.Visible;
                x1butt.Visibility = Visibility.Visible;
                x2butt.Visibility = Visibility.Visible;
                x4butt.Visibility = Visibility.Visible;
                zoomlebl.Visibility = Visibility.Visible;
                zoombcn.Visibility = Visibility.Visible;
                SearchMapbyID.Visibility = Visibility.Visible;
                callsignbox.Visibility = Visibility.Visible;
                StopSearchbytarget.Visibility = Visibility.Visible;
                ZOOM.Visibility = Visibility.Visible;
                Timer.Visibility = Visibility.Visible;
                RestartButton.Visibility = Visibility.Visible;
                speedlabel.Visibility = Visibility.Visible;
                Help.Visibility = Visibility.Visible;
                // Visual Stuff
                arrow.Visibility = Visibility.Collapsed;
                asterixPNG.Visibility = Visibility.Collapsed;
                asterixPerf.Visibility = Visibility.Collapsed;
                bubbleWord.Visibility = Visibility.Collapsed;
                circle.Visibility = Visibility.Collapsed;
                circle2.Visibility = Visibility.Collapsed;
                SearchNumButton.Visibility = Visibility.Collapsed;
                SearchIDButton.Visibility = Visibility.Collapsed;
                NumBox.Visibility = Visibility.Collapsed;
                IDBox.Visibility = Visibility.Collapsed;
                checktrail.Visibility = Visibility.Visible;

                if (Math.Floor(F.CAT_list[0]) == 10)
                {
                    bool IsMultipleCAT = F.CAT_list.Contains(21);
                    if (IsMultipleCAT == true) { gridlista.ItemsSource = F.multiplecattablereducida.DefaultView; }
                    else { gridlista.ItemsSource = F.tablacat10reducida.DefaultView; }
                }
                if (Math.Floor(F.CAT_list[0]) == 21)
                {
                    bool IsMultipleCAT = F.CAT_list.Contains(10);
                    if (IsMultipleCAT == true) { gridlista.ItemsSource = F.multiplecattablereducida.DefaultView; }
                    else { gridlista.ItemsSource = F.tablacat21reducida.DefaultView; }
                }
            }
        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var HelpWin = new HelpWindow();
            HelpWin.Show();
        }

        // TRACK TABLE METHODS
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
                if (Col_Num == 6 && pack.Target_Rep_Descript != null)
                {
                    string[] TRD = pack.Target_Rep_Descript;
                    MessageBox.Show("TYP: " + TRD[0] + "\nDCR: " + TRD[1] + "\nCHN: " + TRD[2] + "\nGBS: " + TRD[3] +
                        "\nCRT: " + TRD[4] + "\nSIM: " + TRD[5] + "\nTST: " + TRD[6] + "\nRAB" + TRD[7] + "\nLOP: " + TRD[8] + "\nTOT: " +
                        TRD[9] + "\nSPI" + TRD[10], "Target Report");
                }
                if (Col_Num == 14 && pack.Track_Status != null)
                {
                    string[] TS = pack.Track_Status;
                    MessageBox.Show("CNF: " + TS[0] + "\nTRE: " + TS[1] + "\nCST: " + TS[2] + "\nMAH: " + TS[3] +
                        "\nTCC: " + TS[4] + "\nSTH: " + TS[5] + "\nTOM: " + TS[6] + "\nDOU: " + TS[7] + "\nMRS: " +
                        TS[8] + "\nGHO: " + TS[9], "Track Status");
                }
                if (Col_Num == 15 && pack.Mode3A_Code != null)
                {
                    string[] M3A = pack.Mode3A_Code;
                    MessageBox.Show("V: " + M3A[0] + "\nG: " + M3A[1] + "\nL: " + M3A[2] + "\n Code: " + M3A[3],
                        "Mode 3/A Code");
                }
                if (Col_Num == 17 && pack.Mode_SMB != null)
                {
                    string[] MSMB = pack.Mode_SMB;
                    MessageBox.Show("REP: " + MSMB[0] + "\nMB: " + MSMB[1] + "\nBDS 1: " + MSMB[2] + "\nBDS 2: " + MSMB[3],
                        "Mode S MB");
                }
                if (Col_Num == 23 && pack.Sys_Status != null)
                {
                    string[] SS = pack.Sys_Status;
                    MessageBox.Show("NOGO: " + SS[0] + "\nOVL: " + SS[1] + "\nTSV: " + SS[2] + "\nDIV: " + SS[3] +
                        "\nTTF: " + SS[4], "System Status");
                }
                if (Col_Num == 24 && pack.Pre_Prog_Message != null)
                {
                    string[] PPM = pack.Pre_Prog_Message;
                    MessageBox.Show("TRB: " + PPM[0] + "Message: " + PPM[1], "Pre-Programmed Message");
                }
                if (Col_Num == 27 && pack.Presence != null)
                {
                    double[] P = pack.Presence;
                    MessageBox.Show("REP: " + P[0] + "\nDifference of Rho: " + P[1] + "\nDifference of Theta: " + P[2],
                        "Presence");
                }
            }
            // CAT 21 case
            if (category == 21)
            {
                DataTable tabla = F.tablaCAT21;
                double cat = Convert.ToDouble(tabla.Rows[Row_Num][1]);
                if (cat == 21)
                {
                    CAT21 pack = F.getCAT21(Row_Num);
                    if (Col_Num == 6 && pack.Target_Report_Desc != null)
                    {
                        string[] TRD = pack.Target_Report_Desc;
                        MessageBox.Show("ATP: " + TRD[0] + "\nARC: " + TRD[1] + "\nRC: " + TRD[2] + "\nRAB :" + TRD[3] +
                                "\nDCR: " + TRD[4] + "\nGBS: " + TRD[5] + "\nSIM: " + TRD[6] + "\nTST: " + TRD[7] + "\nSAA: " + TRD[8] + "\nCL: " +
                                TRD[9] + "\nIPC: " + TRD[10] + "\nNOGO" + TRD[11] + "\nCPR: " + TRD[12] + "\nLDPJ: " + TRD[13] + "\nRCF: " + TRD[14],
                                "Target Report");
                    }
                    if (Col_Num == 12 && pack.Op_Status != null)
                    {
                        string[] OS = pack.Op_Status;
                        MessageBox.Show("RA: " + OS[0] + "\nTC: " + OS[1] + "\nTS: " + OS[2] + "\nARV: " + OS[3] +
                            "\nCDITA: " + OS[4] + "\nNot TCAS: " + OS[5] + "\nSing. Ant.: " + OS[6], "Operational Status");
                    }
                    if (Col_Num == 18 && pack.MOPS != null)
                    {
                        string[] MOPS = pack.MOPS;
                        MessageBox.Show("VNS: " + MOPS[0] + "\nVN: " + MOPS[1] + "\nLTT: " + MOPS[2], "MOPS Version");
                    }
                    if (Col_Num == 23 && pack.Met_Report != null)
                    {
                        string[] MR = pack.Met_Report;
                        MessageBox.Show("Wind Speed: " + MR[0] + "\nWind Direction: " + MR[1] + "\nTemperature: " + MR[2] +
                                "Turbulence: " + MR[3], "Met Report");
                    }
                    if (Col_Num == 26 && pack.Target_Status != null)
                    {
                        string[] TS = pack.Target_Status;
                        MessageBox.Show("ICF: " + TS[0] + "\nLNAV: " + TS[1] + "\nPS: " + TS[2] + "\nSS: " + TS[3],
                            "Target Status");
                    }
                    if (Col_Num == 29 && pack.Quality_Indicators != null)
                    {
                        string[] QI = pack.Quality_Indicators;
                        MessageBox.Show("" + QI[0] + "\n" + QI[1] + "\n" + QI[2] + "\n" + "\nSIL Supplement: " +
                               QI[3] + "\nSDA: " + QI[4] + "\nGVA: " + QI[5] + "\nPIC: " + QI[6], "Quality Indicators");
                    }
                    if (Col_Num == 30 && pack.Mode_S != null)
                    {
                        int[] MS = pack.Mode_S;
                        MessageBox.Show("Rep. Mode S MB Data: " + MS[0] + "\nMB Data: " + MS[1] + "\nBDS 1: " + MS[2] +
                                "\nBDS 2: " + MS[3], "Mode S MB Data");
                    }
                    if (Col_Num == 37 && pack.TMRP_HP != null)
                    {
                        string[] TMRP = pack.TMRP_HP;
                        MessageBox.Show("Full Second Indication: " + TMRP[0] +
                                "\nTMR Posiotion: " + TMRP[1], "Time of Message Reception for Position\nHigh Precision");
                    }
                    if (Col_Num == 38 && pack.TMRV_HP != null)
                    {
                        string[] TMRV = pack.TMRV_HP;
                        MessageBox.Show("Full Second Indication: " + TMRV[0] +
                               "\nTMR Velocity: " + TMRV[1], "Time of Message Reception for velocity\nHigh Precision");
                    }
                    if (Col_Num == 40 && pack.Trajectory_Intent != null)
                    {
                        string[] TI = pack.Trajectory_Intent;
                        MessageBox.Show("" + TI[0] + "\nNVB: " + TI[1] + "\nNAV: " + TI[2] + "\nREP: " + TI[3] + "\nTCA: " + TI[0] +
                                "\nNC: " + TI[5] + "\nTCP Number: " + TI[6] + "\nLatitude TID: " + TI[7] + "\nLongitude TID: " + TI[8] + "\nAltitude (feet): " +
                                TI[9] + "\nPoint Type: " + TI[10] + "\nTD: " + TI[11] + "\nTRA: " + TI[12] + "\nTOA: " + TI[13] + "\nTTR (NM): " + TI[15],
                                "Trajectory Intent");
                    }
                    if (Col_Num == 41 && pack.Data_Ages != null)
                    {
                        double[] DA = pack.Data_Ages;
                        MessageBox.Show("AOS: " + DA[0] + "\nTRD: " + DA[1] + "\n Mode 3A: " + DA[2] + "\nQI: " + DA[3] + "\nTI: " + DA[4] +
                                "\nMAM: " + DA[5] + "\nGH: " + DA[6] + "\nFL: " + DA[7] + "\nISA: " + DA[8] + "\nFSA: " + DA[9] + "\nAS: " + DA[10] + "\nTAS: " +
                                DA[11] + "\nMH: " + DA[12] + "\nBVR: " + DA[13] + "\nGVR: " + DA[14] + "\nGV: " + DA[15] + "\nTAR: " + DA[16] + "\nTarget ID: " +
                                DA[17] + "\nTS: " + DA[18] + "Met: " + DA[19] + "\nROA: " + DA[20] + "\nARA: " + DA[21] + "\nSCC: " + DA[22], "Data Ages");
                    }
                }
                if (cat == 21.23)
                {
                    CAT21_v23 pack = F.getCAT21_v23(Row_Num);
                    if (Col_Num == 6 && pack.Target_Report_Desc != null)
                    {
                        string[] TRD = pack.Target_Report_Desc;
                        MessageBox.Show(TRD[0] + "\n" + TRD[1] + "\n" + TRD[2] + "\n" + TRD[3] +
                            "\n" + TRD[4] + "\n" + TRD[5] + "\n" + TRD[6] + "\n" + TRD[7] + "\n" + TRD[8],
                            "Target Report");
                    }
                    if (Col_Num == 27 && pack.Met_Report != null)
                    {
                        string[] MR = pack.Met_Report;
                        MessageBox.Show("Wind Speed: " + MR[0] + "\nWind Direction: " + MR[1] + "\nTemperature: " + MR[2] +
                            "Turbulence: " + MR[3], "Met Report");
                    }
                    if (Col_Num == 29 && pack.Target_Status != null)
                    {
                        string[] TS = pack.Target_Status;
                        MessageBox.Show("ICF: " + TS[0] + "\nLNAV: " + TS[1] + "\nPS: " + TS[2] + "\nSS: " + TS[3],
                            "Target Status");
                    }
                    if (Col_Num == 44 && pack.Trajectory_Intent != null)
                    {
                        string[] TI = pack.Trajectory_Intent;
                        MessageBox.Show(TI[0] + "\nNVB: " + TI[1] + "\nNAV: " + TI[2] + "\nREP: " + TI[3] + "\nTCA: " + TI[0] +
                            "\nNC: " + TI[5] + "\nTCP Number: " + TI[6] + "\nLatitude TID: " + TI[7] + "\nLongitude TID: " + TI[8] + "\nAltitude (feet): " +
                            TI[9] + "\nPoint Type: " + TI[10] + "\nTD: " + TI[11] + "\nTRA: " + TI[12] + "\nTOA: " + TI[13] + "\nTTR (NM): " + TI[15],
                            "Trajectory Intnet");
                    }
                    if (Col_Num == 47 && pack.Fig_of_Merit != null)
                    {
                        string[] FoM = pack.Fig_of_Merit;
                        MessageBox.Show("AC: " + FoM[0] + "\nMN: " + FoM[1] + "\nDC: " + FoM[2] + "\nPA: " + FoM[3], "Figure of Merit");
                    }
                    if (Col_Num == 48 && pack.Link_Tech != null)
                    {
                        string[] LT = pack.Link_Tech;
                        MessageBox.Show(LT[0] + "\n" + LT[1] + "\n" + LT[2] + "\n" + LT[3] + "\n" + LT[4], "Link Technology");
                    }
                }
            }
            // Mixt category
            if (category == 1021)
            {
                DataTable tabla = F.tablaMultipleCAT;
                double cat = Convert.ToDouble(tabla.Rows[Row_Num][1]);
                if (cat == 10)
                {
                    CAT10 pack10 = F.getCAT10(Row_Num);
                    if (Col_Num == 7 && pack10.Target_Rep_Descript != null)
                    {
                        string[] TRD = pack10.Target_Rep_Descript;
                        MessageBox.Show("Target Report:\n\nTYP: " + TRD[0] + "\nDCR: " + TRD[1] + "\nCHN: " + TRD[2] + "\nGBS: " + TRD[3] +
                            "\nCRT: " + TRD[4] + "\nSIM: " + TRD[5] + "\nTST: " + TRD[6] + "\nRAB" + TRD[7] + "\nLOP: " + TRD[8] + "\nTOT: " +
                            TRD[9] + "\nSPI" + TRD[10]);
                    }
                    if (Col_Num == 9 && pack10.Mode3A_Code != null)
                    {
                        string[] M3A = pack10.Mode3A_Code;
                        MessageBox.Show("Mode 3/A Code:\n\nV: " + M3A[0] + "\nG: " + M3A[1] + "\nL: " + M3A[2] + "\n Code: " + M3A[3]);
                    }
                    if (Col_Num == 10 && pack10.Mode_SMB != null)
                    {
                        string[] MSMB = pack10.Mode_SMB;
                        MessageBox.Show("Mode S MB:\n\nREP: " + MSMB[0] + "\nMB: " + MSMB[1] + "\nBDS 1: " + MSMB[2] + "\nBDS 2: " + MSMB[3]);
                    }
                    if (Col_Num == 22 && pack10.Track_Status != null)
                    {
                        string[] TS = pack10.Track_Status;
                        MessageBox.Show("Track Status:\n\nCNF: " + TS[0] + "\nTRE: " + TS[1] + "\nCST: " + TS[2] + "\nMAH: " + TS[3] +
                            "\nTCC: " + TS[4] + "\nSTH: " + TS[5] + "\nTOM: " + TS[6] + "\nDOU: " + TS[7] + "\nMRS: " + TS[8] + "\nGHO: " + TS[9]);
                    }
                    if (Col_Num == 24 && pack10.Sys_Status != null)
                    {
                        string[] SS = pack10.Sys_Status;
                        MessageBox.Show("System Status:\n\nNOGO: " + SS[0] + "\nOVL: " + SS[1] + "\nTSV: " + SS[2] + "\nDIV: " + SS[3] +
                        "\nTTF: " + SS[4]);
                    }
                    if (Col_Num == 25 && pack10.Pre_Prog_Message != null)
                    {
                        string[] PPM = pack10.Pre_Prog_Message;
                        MessageBox.Show("Pre-Programmed Message:\n\nTRB: " + PPM[0] + "Message: " + PPM[1]);
                    }
                    if (Col_Num == 28 && pack10.Presence != null)
                    {
                        double[] P = pack10.Presence;
                        MessageBox.Show("Presence:\n\nREP: " + P[0] + "\nDifference of Rho: " + P[1] + "\nDifference of Theta: " + P[2]);
                    }
                }
                if (cat == 21)
                {
                    CAT21 pack21 = F.getCAT21(Row_Num);
                    if (Col_Num == 7)
                    {
                        string[] TRD = pack21.Target_Report_Desc;
                        MessageBox.Show("Target Report:\n\nATP: " + TRD[0] + "\nARC: " + TRD[1] + "\nRC: " + TRD[2] + "\nRAB :" + TRD[3] +
                            "\nDCR: " + TRD[4] + "\nGBS: " + TRD[5] + "\nSIM: " + TRD[6] + "\nTST: " + TRD[7] + "\nSAA: " + TRD[8] + "\nCL: " +
                            TRD[9] + "\nIPC: " + TRD[10] + "\nNOGO" + TRD[11] + "\nCPR: " + TRD[12] + "\nLDPJ: " + TRD[13] + "\nRCF: " + TRD[14]);
                    }
                    if (Col_Num == 10 && pack21.Mode_S != null)
                    {
                        int[] MS = pack21.Mode_S;
                        MessageBox.Show("Mode S MB Data:\n\nRep. Mode S MB Data: " + MS[0] + "\nMB Data: " + MS[1] + "\nBDS 1: " + MS[2] +
                            "\nBDS 2: " + MS[3]);
                    }
                    if (Col_Num == 31 && pack21.Op_Status != null)
                    {
                        string[] OS = pack21.Op_Status;
                        MessageBox.Show("Operational Status:\n\nRA: " + OS[0] + "\nTC: " + OS[1] + "\nTS: " + OS[2] + "\nARV: " + OS[3] +
                            "\nCDITA: " + OS[4] + "\nNot TCAS: " + OS[5] + "\nSing. Ant.: " + OS[6]);
                    }
                    if (Col_Num == 41 && pack21.MOPS != null)
                    {
                        string[] MOPS = pack21.MOPS;
                        MessageBox.Show("MOPS Version:\n\nVNS: " + MOPS[0] + "\nVN: " + MOPS[1] + "\nLTT: " + MOPS[2]);
                    }
                    if (Col_Num == 45 && pack21.Met_Report != null)
                    {
                        string[] MR = pack21.Met_Report;
                        MessageBox.Show("Met Report:\n\nWind Speed: " + MR[0] + "\nWind Direction: " + MR[1] + "\nTemperature: " + MR[2] +
                            "Turbulence: " + MR[3]);
                    }
                    if (Col_Num == 47 && pack21.Target_Status != null)
                    {
                        string[] TS = pack21.Target_Status;
                        MessageBox.Show("Target Status:\n\nICF: " + TS[0] + "\nLNAV: " + TS[1] + "\nPS: " + TS[2] + "\nSS: " + TS[3]);
                    }
                    if (Col_Num == 50 && pack21.Quality_Indicators != null)
                    {
                        string[] QI = pack21.Quality_Indicators;
                        MessageBox.Show("Quality Indicators:\n\n" + QI[0] + "\n" + QI[1] + "\n" + QI[2] + "\n" + "\nSIL Supplement: " +
                            QI[3] + "\nSDA: " + QI[4] + "\nGVA: " + QI[5] + "\nPIC: " + QI[6]);
                    }
                    if (Col_Num == 56 && pack21.TMRP_HP != null)
                    {
                        string[] TMRP = pack21.TMRP_HP;
                        MessageBox.Show("Time of Message Reception for Position\nHigh Precision:\n\nFull Second Indication: " + TMRP[0] +
                            "\nTMR Posiotion: " + TMRP[1]);
                    }
                    if (Col_Num == 57 && pack21.TMRV_HP != null)
                    {
                        string[] TMRV = pack21.TMRV_HP;
                        MessageBox.Show("Time of Message Reception for velocity\nHigh Precision:\n\nFull Second Indication: " + TMRV[0] +
                            "\nTMR Velocity: " + TMRV[1]);
                    }
                    if (Col_Num == 59 && pack21.Trajectory_Intent != null)
                    {
                        string[] TI = pack21.Trajectory_Intent;
                        MessageBox.Show("Trajectory Intent:\n\n" + TI[0] + "\nNVB: " + TI[1] + "\nNAV: " + TI[2] + "\nREP: " + TI[3] + "\nTCA: " + TI[0] +
                            "\nNC: " + TI[5] + "\nTCP Number: " + TI[6] + "\nLatitude TID: " + TI[7] + "\nLongitude TID: " + TI[8] + "\nAltitude (feet): " +
                            TI[9] + "\nPoint Type: " + TI[10] + "\nTD: " + TI[11] + "\nTRA: " + TI[12] + "\nTOA: " + TI[13] + "\nTTR (NM): " + TI[15]);
                    }
                    if (Col_Num == 60 && pack21.Data_Ages != null)
                    {
                        double[] DA = pack21.Data_Ages;
                        MessageBox.Show("Data Ages:\n\nAOS: " + DA[0] + "\nTRD: " + DA[1] + "\n Mode 3A: " + DA[2] + "\nQI: " + DA[3] + "\nTI: " + DA[4] +
                            "\nMAM: " + DA[5] + "\nGH: " + DA[6] + "\nFL: " + DA[7] + "\nISA: " + DA[8] + "\nFSA: " + DA[9] + "\nAS: " + DA[10] + "\nTAS: " +
                            DA[11] + "\nMH: " + DA[12] + "\nBVR: " + DA[13] + "\nGVR: " + DA[14] + "\nGV: " + DA[15] + "\nTAR: " + DA[16] + "\nTarget ID: " +
                            DA[17] + "\nTS: " + DA[18] + "Met: " + DA[19] + "\nROA: " + DA[20] + "\nARA: " + DA[21] + "\nSCC: " + DA[22]);
                    }
                }
                if (cat == 21.23)
                {
                    CAT21_v23 pack21 = F.getCAT21_v23(Row_Num);
                    if (Col_Num == 7)
                    {
                        string[] TRD = pack21.Target_Report_Desc;
                        MessageBox.Show(TRD[0] + "\n" + TRD[1] + "\n" + TRD[2] + "\n" + TRD[3] +
                            "\n" + TRD[4] + "\n" + TRD[5] + "\n" + TRD[6] + "\n" + TRD[7] + "\n" + TRD[8],
                            "Target Report");
                    }
                    if (Col_Num == 45 && pack21.Met_Report != null)
                    {
                        string[] MR = pack21.Met_Report;
                        MessageBox.Show("Wind Speed: " + MR[0] + "\nWind Direction: " + MR[1] + "\nTemperature: " + MR[2] +
                            "Turbulence: " + MR[3], "Met Report");
                    }
                    if (Col_Num == 47 && pack21.Target_Status != null)
                    {
                        string[] TS = pack21.Target_Status;
                        MessageBox.Show("ICF: " + TS[0] + "\nLNAV: " + TS[1] + "\nPS: " + TS[2] + "\nSS: " + TS[3],
                            "Target Status");
                    }
                    if (Col_Num == 59 && pack21.Trajectory_Intent != null)
                    {
                        string[] TI = pack21.Trajectory_Intent;
                        MessageBox.Show(TI[0] + "\nNVB: " + TI[1] + "\nNAV: " + TI[2] + "\nREP: " + TI[3] + "\nTCA: " + TI[0] +
                            "\nNC: " + TI[5] + "\nTCP Number: " + TI[6] + "\nLatitude TID: " + TI[7] + "\nLongitude TID: " + TI[8] + "\nAltitude (feet): " +
                            TI[9] + "\nPoint Type: " + TI[10] + "\nTD: " + TI[11] + "\nTRA: " + TI[12] + "\nTOA: " + TI[13] + "\nTTR (NM): " + TI[15],
                            "Trajectory Intnet");
                    }
                    if (Col_Num == 62 && pack21.Fig_of_Merit != null)
                    {
                        string[] FoM = pack21.Fig_of_Merit;
                        MessageBox.Show("AC: " + FoM[0] + "\nMN: " + FoM[1] + "\nDC: " + FoM[2] + "\nPA: " + FoM[3], "Figure of Merit");
                    }
                    if (Col_Num == 63 && pack21.Link_Tech != null)
                    {
                        string[] LT = pack21.Link_Tech;
                        MessageBox.Show(LT[0] + "\n" + LT[1] + "\n" + LT[2] + "\n" + LT[3] + "\n" + LT[4], "Link Technology");
                    }
                }
            }
        }
        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            Track_Table.Visibility = Visibility.Visible;
            Search_Table.Visibility = Visibility.Collapsed;

            SearchNumButton.Visibility = Visibility.Visible;
            NumBox.Visibility = Visibility.Visible;
            SearchIDButton.Visibility = Visibility.Visible;
            IDBox.Visibility = Visibility.Visible;
            SearchAddButton.Visibility = Visibility.Visible;
            AddBox.Visibility = Visibility.Visible;
        }
        private void SearchNum_Click(object sender, RoutedEventArgs e)
        {
            Track_Table.Visibility = Visibility.Hidden;
            Search_Table.Visibility = Visibility.Visible;
            SearchIDButton.Visibility = Visibility.Collapsed;
            SearchNumButton.Visibility = Visibility.Collapsed;
            SearchAddButton.Visibility = Visibility.Collapsed;
            IDBox.Visibility = Visibility.Collapsed;
            AddBox.Visibility = Visibility.Collapsed;

            Search_Table.ItemsSource = null;
            Search_Table.Items.Clear();
            try
            {
                // Searching
                string search = NumBox.Text;
                if (category == 10)
                {
                    try
                    {
                        DataTable tableC10 = F.tablaCAT10;
                        DataTable table = M.getSearchTable10();
                        for (int i = 0; i < tableC10.Rows.Count; i++)
                        {
                            if (tableC10.Rows[i][5].ToString() == search)
                            {
                                table.ImportRow(tableC10.Rows[i]);
                            }
                        }
                        Search_Table.ItemsSource = table.DefaultView;
                    }
                    catch { MessageBox.Show("Any Track number " + search + " in this file"); }
                }
                if (category == 21)
                {
                    try
                    {
                        DataTable tableC21 = F.tablaCAT21;
                        DataTable table = M.getSearchTable21();
                        for (int i = 0; i < tableC21.Rows.Count; i++)
                        {
                            if (tableC21.Rows[i][5].ToString() == search)
                            {
                                table.ImportRow(tableC21.Rows[i]);
                            }
                        }
                        Search_Table.ItemsSource = table.DefaultView;
                    }                    
                    catch { MessageBox.Show("There is no item number " + search + " in this file"); }
                }
                if (category == 1021)
                {
                    try
                    {
                        DataTable tableMix = F.tablaMultipleCAT;
                        DataTable table = M.getSearchTableMixed();
                        for (int i = 0; i < tableMix.Rows.Count; i++)
                        {
                            if (tableMix.Rows[i][5].ToString() == search)
                            {
                                table.ImportRow(tableMix.Rows[i]);
                            }                            
                        }
                        Search_Table.ItemsSource = table.DefaultView;
                    }
                    catch { MessageBox.Show("There is no item number " + search + " in this file"); }
                }
            }
            catch { MessageBox.Show("Wrong format", "ERROR"); }
        }
        private void SearchID_Click(object sender, RoutedEventArgs e)
        {
            Track_Table.Visibility = Visibility.Hidden;
            Search_Table.Visibility = Visibility.Visible;
            SearchNumButton.Visibility = Visibility.Collapsed;
            SearchIDButton.Visibility = Visibility.Collapsed;
            SearchAddButton.Visibility = Visibility.Collapsed;
            NumBox.Visibility = Visibility.Collapsed;
            AddBox.Visibility = Visibility.Collapsed;

            Search_Table.ItemsSource = null;
            Search_Table.Items.Clear();

            string search = IDBox.Text;
            if (category == 10)
            {
                DataTable table10 = F.tablaCAT10;
                DataTable searchtable = M.getSearchTable10();
                for (int i = 0; i < table10.Rows.Count; i++)
                {
                    CAT10 C10 = F.getCAT10(i);
                    if (C10.Target_ID != null && C10.Target_ID.Contains(search))
                    {
                        searchtable.ImportRow(table10.Rows[i]);
                    }
                }
                if (searchtable.Rows.Count == 0)
                {
                    MessageBox.Show("There is no flight with callsign" + search);
                }
                else { Search_Table.ItemsSource = searchtable.DefaultView; }
            }
            if (category == 21)
            {
                DataTable table21 = F.tablaCAT21;
                DataTable searchtable = M.getSearchTable21();
                if (Convert.ToDouble(table21.Rows[0][1]) == 21)
                {
                    for (int i = 0; i < table21.Rows.Count; i++)
                    {
                        CAT21 C21 = F.getCAT21(i);
                        if (C21.Target_ID != null && C21.Target_ID.Contains(search))
                        {
                            searchtable.ImportRow(table21.Rows[i]);
                        }
                    }
                    if (searchtable.Rows.Count == 0)
                    {
                        MessageBox.Show("There is no flight with callsign" + search);
                    }
                    else { Search_Table.ItemsSource = searchtable.DefaultView; }
                }
                if (Convert.ToDouble(table21.Rows[0][1]) == 21.23)
                {
                    for (int i = 0; i < table21.Rows.Count; i++)
                    {
                        CAT21_v23 C21 = F.getCAT21_v23(i);
                        if (C21.Target_ID != null && C21.Target_ID.Contains(search))
                        {
                            searchtable.ImportRow(table21.Rows[i]);
                        }
                    }
                    if (searchtable.Rows.Count == 0)
                    {
                        MessageBox.Show("There is no flight with callsign" + search);
                    }
                    else { Search_Table.ItemsSource = searchtable.DefaultView; }
                }
            }
            if (category == 1021)
            {
                DataTable tableMix = F.tablaMultipleCAT;
                DataTable searchtable = M.getSearchTableMixed();
                for (int i = 0; i < tableMix.Rows.Count; i++)
                {
                    string ID = tableMix.Rows[i][4].ToString();
                    if (ID != null && ID.Contains(search))
                    {
                        searchtable.ImportRow(tableMix.Rows[i]);
                    }
                }
                if (searchtable.Rows.Count == 0)
                {
                    MessageBox.Show("There is no flight with callsign" + search);
                }
                else { Search_Table.ItemsSource = searchtable.DefaultView; }
            }
        }
        private void SearchAdd_Click(object sender, RoutedEventArgs e)
        {
            Track_Table.Visibility = Visibility.Hidden;
            Search_Table.Visibility = Visibility.Visible;
            SearchNumButton.Visibility = Visibility.Collapsed;
            SearchIDButton.Visibility = Visibility.Collapsed;
            SearchAddButton.Visibility = Visibility.Collapsed;
            NumBox.Visibility = Visibility.Collapsed;
            IDBox.Visibility = Visibility.Collapsed;

            Search_Table.ItemsSource = null;
            Search_Table.Items.Clear();

            string search = AddBox.Text;
            if (category == 10)
            {
                DataTable table10 = F.tablaCAT10;
                DataTable searchtable = M.getSearchTable10();
                for (int i = 0; i < table10.Rows.Count; i++)
                {
                    if (table10.Rows[i][16].ToString() == search)
                    {
                        searchtable.ImportRow(table10.Rows[i]);
                    }
                }
                Search_Table.ItemsSource = searchtable.DefaultView;
            }
            if (category == 21)
            {
                DataTable table21 = F.tablaCAT21;
                DataTable searchtable = M.getSearchTable21();
                if (Convert.ToDouble(table21.Rows[0][1]) == 21)
                {
                    for (int i = 0; i < table21.Rows.Count; i++)
                    {
                        if (table21.Rows[i][29].ToString() == search)
                        {
                            searchtable.ImportRow(table21.Rows[i]);
                        }
                    }
                    Search_Table.ItemsSource = searchtable.DefaultView;
                }
                if (Convert.ToDouble(table21.Rows[0][1]) == 21.23)
                {
                    for (int i = 0; i < table21.Rows.Count; i++)
                    {
                        if (table21.Rows[i][29].ToString() == search)
                        {
                            searchtable.ImportRow(table21.Rows[i]);
                        }
                    }
                    Search_Table.ItemsSource = searchtable.DefaultView;
                }
            }
            if (category == 1021)
            {
                DataTable tableMix = F.tablaMultipleCAT;
                DataTable searchtable = M.getSearchTableMixed();
                for (int i = 0; i < tableMix.Rows.Count; i++)
                {
                    if (tableMix.Rows[i][13].ToString() == search)
                    {
                        searchtable.ImportRow(tableMix.Rows[i]);
                    }
                }
                Search_Table.ItemsSource = searchtable.DefaultView;
            }
        }

        // TRACK MAP METHODS
        private void Map_Load(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            map.MapProvider = OpenStreetMapProvider.Instance;
            map.MinZoom = 1;
            map.MaxZoom = 18;
            map.Zoom = 14;
            map.Position = new PointLatLng(MLAT_lat, MLAT_lon);
            map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            map.CanDragMap = true;
            map.DragButton = MouseButton.Left;
        }
        public void addMarker_Click(object sender, RoutedEventArgs e)
        {
            timer.Visibility = Visibility.Visible;
            if (category == 10)
            {
                dt_Timer.Tick += dt_Timer_TickC10;
                updatedtable = F.tablacat10reducida.Clone();
            }
            if (category == 21)
            {
                dt_Timer.Tick += dt_Timer_TickC21;
                updatedtable = F.tablacat21reducida.Clone();
            }
            if (category == 1021)
            {
                dt_Timer.Tick += dt_Timer_TickMULTICAT;
                updatedtable = F.multiplecattablereducida.Clone();
            }
            dt_Timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            dt_Timer.Start();
        }
        public void AddMarkerMLAT(double latitude, double longitude, string targetid)
        {
            PointLatLng point = fromXYtoLatLongMLAT(latitude, longitude);
            GMapMarker marker = new GMapMarker(point);
            if (targetid != null && targetid!= "Not available")
            {
                marker.Shape = new Image
                {
                    Width = 15,
                    Height = 15,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Images/airplaneMLAT.png"))
                };
            }
            if (targetid == "Not available")
            {
                marker.Shape = new Image
                {
                    Width = 15,
                    Height = 15,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Images/purpledot.png"))
                };
            }
            marker.Offset = new Point(-10, -10);
            map.Markers.Add(marker);
        }
        public void AddMarkerSMR(double latitude, double longitude, string targetid)
        {
            PointLatLng point = fromXYtoLatLongSMR(latitude, longitude);
            GMapMarker marker = new GMapMarker(point);
            if (targetid != null && targetid != "Not available")
            {
                marker.Shape = new Image
                {
                    Width = 15,
                    Height = 15,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Images/airplane.png"))
                };
            }
            if (targetid == "Not available")
            {
                marker.Shape = new Image
                {
                    Width = 15,
                    Height = 15,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Images/reddot.png"))
                };
            }
            marker.Offset = new Point(-10, -10);
            map.Markers.Add(marker);
        }
        public void AddMarkerC21(double latitude, double longitude, string targetid)
        {
            PointLatLng point = new PointLatLng(latitude, longitude);
            GMapMarker marker = new GMapMarker(point);
            if (targetid != null && targetid != "Not available")
            {
                marker.Shape = new Image
                {
                    Width = 15,
                    Height = 15,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Images/airplane.png"))
                };
            }
            if (targetid == "Not available")
            {
                marker.Shape = new Image
                {
                    Width = 15,
                    Height = 15,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Images/reddot.png"))
                };
            }
            marker.Offset = new Point(-10, -10);
            map.Markers.Add(marker);
        }
        private void dt_Timer_TickC10(object sender, EventArgs e)
        {
            if (checktrail.IsChecked == true)
            {
                showonlyoneairplane();
            }
            else { }
            Boolean x = true;
            while (x == true)
            {
                CAT10 C10 = F.getCAT10(i);
                start = Math.Floor(F.getCAT10(0).Time_Day) + s;
                tiempo =  Math.Floor(C10.Time_Day);
                if (tiempo < start) { tiempo = tiempo + 1; }

                if (C10.Target_ID == null) { C10.Target_ID = "Not available"; }
                if (searchedcallsign == null) { searchedcallsign = "Not available"; }
                if (idbuttonclicked == true)
                {
                    if (C10.Target_ID.Contains(searchedcallsign))
                    {
                        if (tiempo == start)
                        {
                            if (C10.Target_Rep_Descript[0] == "PSR")
                            {
                                AddMarkerSMR(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1], C10.Target_ID);
                            }
                            if (C10.Target_Rep_Descript[0] == "Mode S Multilateration")
                            {
                                AddMarkerMLAT(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1], C10.Target_ID);
                            }
                            else { }
                            i++;
                            if (map.Markers.Count >= 200)
                            {
                                map.Markers[map.Markers.Count - 200].Clear();
                            }
                            rellenartablaCAT10(i);
                        }
                        else
                        {
                            x = false;
                            s++;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
                else
                {
                    if (tiempo == start)
                    {
                        if (C10.Target_Rep_Descript[0] == "PSR")
                        {
                            AddMarkerSMR(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1], C10.Target_ID);
                        }
                        if (C10.Target_Rep_Descript[0] == "Mode S Multilateration")
                        {
                            AddMarkerMLAT(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1], C10.Target_ID);
                        }
                        else { }
                        i++;
                        if (map.Markers.Count >= 200)
                        {
                            map.Markers[map.Markers.Count - 200].Clear();
                        }
                        rellenartablaCAT10(i);
                    }
                    else
                    {
                        x = false;
                        s++;
                    }
                }
                clock(tiempo - 1);
                gridlista.Visibility = Visibility.Collapsed;
                updatedlista.Visibility = Visibility.Visible;
            }
        }
        private void dt_Timer_TickC21(object sender, EventArgs e)
        {
            if (checktrail.IsChecked == true)
            {
                showonlyoneairplane();

            }
            else { }

            Boolean x = true;
            while (x == true)
            {
                if (F.SICSAC[0] == 107 && F.SICSAC[1] == 0)
                {
                    CAT21_v23 C21_v23 = F.getCAT21_v23(i);
                    start = Math.Floor(F.getCAT21_v23(0).Time_of_Day) + s;
                    tiempo = Math.Floor(C21_v23.Time_of_Day);
                    if (C21_v23.Target_ID == null) { C21_v23.Target_ID = "Not available"; }
                    if (searchedcallsign == null) { searchedcallsign = "Not available"; }
                    if (idbuttonclicked == true)
                    {
                        if (C21_v23.Target_ID.Contains(searchedcallsign))
                        {
                            if (tiempo == start)
                            {
                                AddMarkerC21(C21_v23.Lat_WGS_84, C21_v23.Lon_WGS_84, C21_v23.Target_ID);
                                i++;
                                if (map.Markers.Count >= 200)
                                {
                                    map.Markers[map.Markers.Count - 200].Clear();
                                }
                                rellenartablaCAT21(i);
                            }
                            else
                            {
                                x = false;
                                s++;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        if (tiempo == start)
                        {
                            AddMarkerC21(C21_v23.Lat_WGS_84, C21_v23.Lon_WGS_84, C21_v23.Target_ID);
                            i++;
                            if (map.Markers.Count >= 200)
                            {
                                map.Markers[map.Markers.Count - 200].Clear();
                            }
                            rellenartablaCAT21(i);
                        }
                        else
                        {
                            x = false;
                            s++;
                        }
                    }
                }
                // version 2.1
                if (F.SICSAC[0] != 107 || F.SICSAC[1] != 0)
                {
                    CAT21 C21 = F.getCAT21(i);
                    start = Math.Floor(F.getCAT21(0).Time_Rep_Transm) + s;
                    tiempo = Math.Floor(C21.Time_Rep_Transm);
                    if (C21.Target_ID == null) { C21.Target_ID = "Not available"; }

                    if (searchedcallsign == null) { searchedcallsign = "Not available"; }
                    if (idbuttonclicked == true)
                    {
                        if (C21.Target_ID.Contains(searchedcallsign))
                        {
                            if (tiempo == start)
                            {
                                AddMarkerC21(C21.High_Res_Lat_WGS_84, C21.High_Res_Lon_WGS_84, C21.Target_ID);
                                i++;
                                if (map.Markers.Count >= 200)
                                {
                                    map.Markers[map.Markers.Count - 200].Clear();
                                }
                                rellenartablaCAT21(i);
                            }
                            else
                            {
                                x = false;
                                s++;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }

                    else
                    {
                        if (tiempo == start)
                        {
                            AddMarkerC21(C21.High_Res_Lat_WGS_84, C21.High_Res_Lon_WGS_84, C21.Target_ID);
                            i++;
                            if (map.Markers.Count >= 200)
                            {
                                map.Markers[map.Markers.Count - 200].Clear();
                            }
                            rellenartablaCAT21(i);
                        }
                        else
                        {
                            x = false;
                            s++;
                        }
                    }
                }
                clock(tiempo - 1);
                gridlista.Visibility = Visibility.Collapsed;
                updatedlista.Visibility = Visibility.Visible;
            }
        }
        
        DataTable tabla;
        private void dt_Timer_TickMULTICAT(object sender, EventArgs e)
        {
            if (checktrail.IsChecked == true)
            {
                showonlyoneairplane();
            }
            else { }
            Boolean x = true;
            tabla = F.multiplecattablereducida;
            
            while (x == true)
            {               
                for (int i = 0; i < tabla.Rows.Count; i++)
                {                   
                    string tiempomal = Convert.ToString(tabla.Rows[i][2]);
                    string starttiempo = Convert.ToString(tabla.Rows[0][2]);
                    string[] tiemposplited = tiempomal.Split(':');
                    string[] tiemposplitedstart = starttiempo.Split(':');

                    int tiempo = M.gettimecorrectly(tiemposplited);
                    int start1 = M.gettimecorrectly(tiemposplitedstart) + n;
                    if (tiempo == start1 + 2) { i = tabla.Rows.Count; }
                    else
                    {
                        string targetid = Convert.ToString(tabla.Rows[i][1]);

                        if (searchedcallsign == null) { searchedcallsign = "Not available"; }
                        if (targetid == "") { targetid = "Not available"; }
                        if (idbuttonclicked == true && tiempo == start1)
                        {
                            if (targetid.Contains(searchedcallsign))
                            {
                                if (F.CAT_list[i] == 10)
                                {
                                    double poscartx = Convert.ToDouble(tabla.Rows[i][6]);
                                    double poscarty = Convert.ToDouble(tabla.Rows[i][7]);
                                        
                                    if(tabla.Rows[i][10].ToString() == "PSR")
                                    {
                                        AddMarkerSMR(poscartx, poscarty, targetid);
                                    }
                                    else
                                    {
                                        AddMarkerMLAT(poscartx, poscarty, targetid);
                                    }

                                    if (map.Markers.Count >= 200)
                                    {
                                        map.Markers[map.Markers.Count - 200].Clear();
                                    }
                                    rellenartablaMULTICAT(i);
                                    clock(tiempo - 1);
                                }
                                if (F.CAT_list[i] == 21.23)
                                {
                                    double poscartx = Convert.ToDouble(tabla.Rows[i][6]);
                                    double poscarty = Convert.ToDouble(tabla.Rows[i][7]);
                                    AddMarkerC21(poscartx, poscarty, targetid);

                                    if (map.Markers.Count >= 200)
                                    {
                                        map.Markers[map.Markers.Count - 200].Clear();
                                    }
                                    rellenartablaMULTICAT(i);
                                    clock(tiempo - 1);
                                }
                                if (F.CAT_list[i] == 21)
                                {
                                    double poscartx = Convert.ToDouble(tabla.Rows[i][6]);
                                    double poscarty = Convert.ToDouble(tabla.Rows[i][7]);
                                    AddMarkerC21(poscartx, poscarty, targetid);
                                    if (map.Markers.Count >= 200)
                                    {
                                        map.Markers[map.Markers.Count - 200].Clear();
                                    }
                                    rellenartablaMULTICAT(i);
                                    clock(tiempo - 1);
                                }
                            }
                            else
                            {
                                i++;
                            }
                        }
                        else
                        {
                            if (F.CAT_list[i] == 10 && tiempo == start1)
                            {
                                double poscartx = Convert.ToDouble(tabla.Rows[i][6]);
                                double poscarty = Convert.ToDouble(tabla.Rows[i][7]);
                                if (tabla.Rows[i][10].ToString() == "PSR")
                                {
                                    AddMarkerSMR(poscartx, poscarty, targetid);
                                }
                                else
                                {
                                    AddMarkerMLAT(poscartx, poscarty, targetid);
                                }

                                if (map.Markers.Count >= 200)
                                {
                                    map.Markers[map.Markers.Count - 200].Clear();
                                }
                                rellenartablaMULTICAT(i);
                                clock(tiempo - 1);
                            }
                            if (F.CAT_list[i] == 21.23 && tiempo == start1)
                            {
                                double poscartx = Convert.ToDouble(tabla.Rows[i][6]);
                                double poscarty = Convert.ToDouble(tabla.Rows[i][7]);
                                AddMarkerC21(poscartx, poscarty, targetid);
                                if (map.Markers.Count >= 200)
                                {
                                    map.Markers[map.Markers.Count - 200].Clear();
                                }
                                rellenartablaMULTICAT(i);
                                clock(tiempo - 1);
                            }
                            if (F.CAT_list[i] == 21 && tiempo == start1)
                            {
                                double poscartx = Convert.ToDouble(tabla.Rows[i][6]);
                                double poscarty = Convert.ToDouble(tabla.Rows[i][7]);
                                AddMarkerC21(poscartx, poscarty, targetid);
                                if (map.Markers.Count >= 200)
                                {
                                    map.Markers[map.Markers.Count - 200].Clear();
                                }
                                rellenartablaMULTICAT(i);
                                clock(tiempo - 1);
                            }
                            else { }
                        }
                    }             
                   
                }
                x = false;
                n++;
            }
            gridlista.Visibility = Visibility.Collapsed;
            updatedlista.Visibility = Visibility.Visible;
        }
        private void clock(double tiempo)
        {
            TimeSpan time = TimeSpan.FromSeconds(tiempo);
            string tiempoact = time.ToString(@"hh\:mm\:ss");
            timer.Text = tiempoact;
        }
        private void stop_Click(object sender, RoutedEventArgs e)
        {
            if (category == 10)
            {
                dt_Timer.Tick -= dt_Timer_TickC10;
            }
            if (category == 21)
            {
                dt_Timer.Tick -= dt_Timer_TickC21;
            }
            if (category == 1021)
            {
                dt_Timer.Tick -= dt_Timer_TickMULTICAT;
            }
            dt_Timer.Stop();
        }
        private void STOP_TRACK()
        {
            if (category == 10)
            {
                dt_Timer.Tick -= dt_Timer_TickC10;
            }
            if (category == 21)
            {
                dt_Timer.Tick -= dt_Timer_TickC21;
            }
            if (category == 1021)
            {
                dt_Timer.Tick -= dt_Timer_TickMULTICAT;
            }
            dt_Timer.Interval = new TimeSpan(0, 0, 0, 1000);
            map.Markers.Clear();
            updatedtable.Clear();
            i = 0;
            s = 0;
            n = 0;
            //if (category == 10) { gridlista.ItemsSource = F.tablacat10reducida.DefaultView; }
            //if (category == 21) { gridlista.ItemsSource = F.tablacat21reducida.DefaultView; }
            //if (category == 1021) { gridlista.ItemsSource = F.multiplecattablereducida.DefaultView; }
            gridlista.Visibility = Visibility.Visible;
            updatedlista.Visibility = Visibility.Collapsed;
            timer.Visibility = Visibility.Collapsed;
            dt_Timer.Stop();
        }
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            STOP_TRACK();
        }

        // MAP VIEWING OPTIONS        
        // Change the refreshing speed of the map files
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
        // Change map zoom
        private void zoomin_Click(object sender, RoutedEventArgs e)
        {
            map.Zoom = map.Zoom + 2;
        }
        private void zoomout_Click(object sender, RoutedEventArgs e)
        {
            map.Zoom = map.Zoom - 2;
        }

        // Plot only airplanes with searched callsign
        private void SearchIDMAP_Click(object sender, RoutedEventArgs e)
        {
            idbuttonclicked = true;
            if (callsignbox.Text == "")
            {
                idbuttonclicked = false;
                MessageBox.Show("Make sure textbox is not empty!");

            }
            else
            {
                if (category == 10)
                {
                    bool fuera = false;
                    for (int q = 0; q < F.tablacat10reducida.Rows.Count; q++)
                    {
                        string targetID = Convert.ToString(F.tablacat10reducida.Rows[q][1]);
                        if (targetID.Contains(callsignbox.Text))
                        {
                            fuera = true;
                            q = F.tablacat10reducida.Rows.Count;
                            map.Markers.Clear();
                            searchedcallsign = callsignbox.Text;
                        }
                    }
                    if (fuera == false)
                    {
                        idbuttonclicked = false;
                        MessageBox.Show("Target ID: " + callsignbox.Text + " is not available! \n Make sure Target ID is written in capital letters (e.g. VLG)");

                    }
                }
                if (category == 21)
                {
                    bool fuera = false;
                    for (int q = 0; q < F.tablacat21reducida.Rows.Count; q++)
                    {
                        string targetID = Convert.ToString(F.tablacat21reducida.Rows[q][1]);

                        if (targetID.Contains(callsignbox.Text))
                        {
                            fuera = true;
                            q = F.tablacat21reducida.Rows.Count;
                            map.Markers.Clear();
                            searchedcallsign = callsignbox.Text;
                        }
                    }
                    if (fuera == false)
                    {
                        idbuttonclicked = false;
                        MessageBox.Show("Target ID: " + callsignbox.Text + " is not available! \n Make sure Target ID is written in capital letters (e.g. VLG)");

                    }
                }
                if (category == 1021)
                {
                    bool fuera = false;
                    for (int q = 0; q < F.multiplecattablereducida.Rows.Count; q++)
                    {
                        string targetID = Convert.ToString(F.multiplecattablereducida.Rows[q][1]);

                        if (targetID.Contains(callsignbox.Text))
                        {
                            fuera = true;
                            q = F.multiplecattablereducida.Rows.Count;
                            map.Markers.Clear();
                            searchedcallsign = callsignbox.Text;
                        }
                    }
                    if (fuera == false)
                    {
                        idbuttonclicked = false;
                        MessageBox.Show("Target ID: " + callsignbox.Text + " is not available! " +
                            "\n Make sure Target ID is written in capital letters (e.g. VLG)");

                    }
                }
                else { }
            }
        }
        private void Stopsearchtarget(object sender, RoutedEventArgs e)
        {
            idbuttonclicked = false;
        }
        private void rellenartablaCAT10(int i)
        {
            //we copy/paste all data from that specific flight
            updatedtable.ImportRow(F.tablacat10reducida.Rows[i - 1]);
            updatedlista.ItemsSource = updatedtable.DefaultView;
            updatedlista.ScrollIntoView(updatedlista.Items.GetItemAt(updatedlista.Items.Count - 1));
        }
        private void rellenartablaCAT21(int i)
        {
            //we copy/paste all data from that specific flight
            updatedtable.ImportRow(F.tablacat21reducida.Rows[i - 1]);
            updatedlista.ItemsSource = updatedtable.DefaultView;
            updatedlista.ScrollIntoView(updatedlista.Items.GetItemAt(updatedlista.Items.Count - 1));
        }
        private void rellenartablaMULTICAT(int i)
        {
            //we copy/paste all data from that specific flight
            updatedtable.ImportRow(F.multiplecattablereducida.Rows[i]);
            updatedlista.ItemsSource = updatedtable.DefaultView;
            updatedlista.ScrollIntoView(updatedlista.Items.GetItemAt(updatedlista.Items.Count - 1));
        }
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
        public PointLatLng fromXYtoLatLongSMR(double X, double Y)
        {
            double R = 6371 * 1000;
            double d = Math.Sqrt((X * X) + (Y * Y));
            double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
            double φ1 = SMR_lat * (Math.PI / 180);
            double λ1 = SMR_lon * (Math.PI / 180);
            var φ2 = Math.Asin(Math.Sin(φ1) * Math.Cos(d / R) + Math.Cos(φ1) * Math.Sin(d / R) * Math.Cos(brng));
            var λ2 = λ1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(φ1), Math.Cos(d / R) - Math.Sin(φ1) * Math.Sin(φ2));

            PointLatLng coordinates = new PointLatLng(φ2 * (180 / Math.PI), λ2 * (180 / Math.PI));
            return coordinates;
        }
       
        private void showonlyoneairplane()
        {  
            
            for (int i = 0; i < map.Markers.Count; i++)
            {
                if (map.Markers[i].Shape != null)
                {
                    map.Markers[i].Shape.Visibility = Visibility.Collapsed;
                }
            }

        }
        public void CheckCAT()
        {
            if (Math.Floor(F.CAT_list[0]) == 10)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(21);
                if (IsMultipleCAT == true)
                {
                    Track_Table.ItemsSource = F.tablaMultipleCAT.DefaultView;
                    category = 1021;
                }
                else
                {
                    Track_Table.ItemsSource = F.tablaCAT10.DefaultView;
                    category = 10;
                }
            }
            if (Math.Floor(F.CAT_list[0]) == 21)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(10);
                if (IsMultipleCAT == true)
                {
                    Track_Table.ItemsSource = F.tablaMultipleCAT.DefaultView;
                    category = 1021;
                }
                else
                {
                    Track_Table.ItemsSource = F.tablaCAT21.DefaultView;
                    category = 21;
                }
            }
        }
    }
}
