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
using System.Windows.Data;

namespace ASTERIX_APP
{
    public partial class MainWindow : Window
    {
        Fichero F;
        bool chivato = false;
        int category;
        int i = 0;
        //lat and lon os cat10 files 
        double latindegrees;
        double lonindegrees;
        //Datatable that shows flights for each second (used at the map)
        DataTable updatedtable = new DataTable();
        //lat lon of MLAT system of reference (at LEBL airport)
        double MLAT_lat = 41.0 + (17.0 / 60.0) + (49.0 / 3600.0) + (426.0 / 3600000.0);
        double MLAT_lon = 2.0 + (4.0 / 60.0) + (42.0 / 3600.0) + (410.0 / 3600000.0);
        //lat lon of SMR system of reference (at LEBL airport)
        double SMR_lat = 41.0 + (17.0 / 60.0) + (44.0 / 3600.0) + (226.0 / 3600000.0);
        double SMR_lon = 2.0 + (5.0 / 60.0) + (42.0 / 3600.0) + (411.0 / 3600000.0);
        public MainWindow()
        {
            InitializeComponent();

            Search_Table.Visibility = Visibility.Hidden;
            SearchResult.Visibility = Visibility.Hidden;
            SearchNumButton.Visibility = Visibility.Hidden;
            SearchIDButton.Visibility = Visibility.Hidden;
            NumBox.Visibility = Visibility.Hidden;
            IDBox.Visibility = Visibility.Hidden;

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
            Application.Current.Shutdown();
        }
        public void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            map.Visibility = Visibility.Hidden;
            SearchNumButton.Visibility = Visibility.Hidden;
            SearchIDButton.Visibility = Visibility.Hidden;
            NumBox.Visibility = Visibility.Hidden;
            IDBox.Visibility = Visibility.Hidden;
            Search_Table.Visibility = Visibility.Hidden;
            SearchResult.Visibility = Visibility.Hidden;

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
            if (OpenFile.FileName == "")
            {
                chivato = false;
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
                chivato = true;
                F = new Fichero(OpenFile.FileName);
                try
                {
                    F.leer();
                    asterixPerf.Visibility = Visibility.Visible;
                    Instructions_Label.Content = "Perfectly read! Let's get started!" + '\n' + "1) View the file's data by clicking on 'Tracking Table'" +
                        '\n' + "2) Run an amazing simulation by clicking on 'Tracking Map'";
                    bubbleWord.Height = 100;
                    bubbleWord.Width = 550;
                    bubbleWord.Visibility = Visibility.Visible;
                    circle.Visibility = Visibility.Visible;
                    circle2.Visibility = Visibility.Visible;
                }
                catch { MessageBox.Show("ERROR: Make sure file containing category 21 '\n' is version 2.1 or 0.23"); }
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
            }
            else
            {
                // Table and Map Stuff
                Search_Table.Visibility = Visibility.Visible;
                SearchResult.Visibility = Visibility.Visible;
                SearchNumButton.Visibility = Visibility.Visible;
                SearchIDButton.Visibility = Visibility.Visible;
                NumBox.Visibility = Visibility.Visible;
                IDBox.Visibility = Visibility.Visible;

                Instructions_Label.Visibility = Visibility.Hidden;
                Track_Table.Visibility = Visibility.Visible;
                map.Visibility = Visibility.Hidden;
                gridlista.Visibility = Visibility.Hidden;
                StartButton.Visibility = Visibility.Hidden;
                StopButton.Visibility = Visibility.Hidden;
                timer.Visibility = Visibility.Hidden;
                x1butt.Visibility = Visibility.Hidden;
                x2butt.Visibility = Visibility.Hidden;
                x4butt.Visibility = Visibility.Hidden;
                zoomlebl.Visibility = Visibility.Hidden;
                zoombcn.Visibility = Visibility.Hidden;
                zoomcat.Visibility = Visibility.Hidden;
                updatedlista.Visibility = Visibility.Hidden;
                SearchMapbyID.Visibility = Visibility.Hidden;
                callsignbox.Visibility = Visibility.Hidden;
                // Visual Elements
                asterixPNG.Visibility = Visibility.Hidden;
                asterixPerf.Visibility = Visibility.Hidden;
                bubbleWord.Visibility = Visibility.Hidden;
                circle.Visibility = Visibility.Hidden;
                circle2.Visibility = Visibility.Hidden;
                SearchNumButton.Visibility = Visibility.Visible;
                SearchIDButton.Visibility = Visibility.Visible;
                NumBox.Visibility = Visibility.Visible;
                IDBox.Visibility = Visibility.Visible;
                SearchResult.Visibility = Visibility.Visible;

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
                if (Col_Num == 5 && pack.Target_Rep_Descript != null)
                {
                    string[] TRD = pack.Target_Rep_Descript;
                    MessageBox.Show("TYP: " + TRD[0] + "\nDCR: " + TRD[1] + "\nCHN: " + TRD[2] + "\nGBS: " + TRD[3] +
                        "\nCRT: " + TRD[4] + "\nSIM: " + TRD[5] + "\nTST: " + TRD[6] + "\nRAB" + TRD[7] + "\nLOP: " + TRD[8] + "\nTOT: " +
                        TRD[9] + "\nSPI" + TRD[10], "Target Report");
                }
                if (Col_Num == 13 && pack.Track_Status != null)
                {
                    string[] TS = pack.Track_Status;
                    MessageBox.Show("CNF: " + TS[0] + "\nTRE: " + TS[1] + "\nCST: " + TS[2] + "\nMAH: " + TS[3] +
                        "\nTCC: " + TS[4] + "\nSTH: " + TS[5] + "\nTOM: " + TS[6] + "\nDOU: " + TS[7] + "\nMRS: " +
                        TS[8] + "\nGHO: " + TS[9], "Track Status");
                }
                if (Col_Num == 14 && pack.Mode3A_Code != null)
                {
                    string[] M3A = pack.Mode3A_Code;
                    MessageBox.Show("V: " + M3A[0] + "\nG: " + M3A[1] + "\nL: " + M3A[2] + "\n Code: " + M3A[3], "Mode 3/A Code");
                }
                if (Col_Num == 16 && pack.Mode_SMB != null)
                {
                    string[] MSMB = pack.Mode_SMB;
                    MessageBox.Show("Mode S MB:\n\nREP: " + MSMB[0] + "\nMB: " + MSMB[1] + "\nBDS 1: " + MSMB[2] + "\nBDS 2: " + MSMB[3]);
                }
                if (Col_Num == 22 && pack.Sys_Status != null)
                {
                    string[] SS = pack.Sys_Status;
                    MessageBox.Show("System Status:\n\nNOGO: " + SS[0] + "\nOVL: " + SS[1] + "\nTSV: " + SS[2] + "\nDIV: " + SS[3] +
                        "\nTTF: " + SS[4]);
                }
                if (Col_Num == 23 && pack.Pre_Prog_Message != null)
                {
                    string[] PPM = pack.Pre_Prog_Message;
                    MessageBox.Show("Pre-Programmed Message:\n\nTRB: " + PPM[0] + "Message: " + PPM[1]);
                }
                if (Col_Num == 26 && pack.Presence != null)
                {
                    double[] P = pack.Presence;
                    MessageBox.Show("Presence:\n\nREP: " + P[0] + "\nDifference of Rho: " + P[1] + "\nDifference of Theta: " + P[2]);
                }
            }
            // CAT 21 case
            if (category == 21)
            {
                CAT21 pack = F.getCAT21(Row_Num);
                if (Col_Num == 5 && pack.Target_Report_Desc != null)
                {
                    string[] TRD = pack.Target_Report_Desc;
                    MessageBox.Show("Target Report:\n\nATP: " + TRD[0] + "\nARC: " + TRD[1] + "\nRC: " + TRD[2] + "\nRAB :" + TRD[3] +
                            "\nDCR: " + TRD[4] + "\nGBS: " + TRD[5] + "\nSIM: " + TRD[6] + "\nTST: " + TRD[7] + "\nSAA: " + TRD[8] + "\nCL: " +
                            TRD[9] + "\nIPC: " + TRD[10] + "\nNOGO" + TRD[11] + "\nCPR: " + TRD[12] + "\nLDPJ: " + TRD[13] + "\nRCF: " + TRD[14]);
                }
                if (Col_Num == 11 && pack.Op_Status != null)
                {
                    string[] OS = pack.Op_Status;
                    MessageBox.Show("Operational Status:\n\nRA: " + OS[0] + "\nTC: " + OS[1] + "\nTS: " + OS[2] + "\nARV: " + OS[3] +
                        "\nCDITA: " + OS[4] + "\nNot TCAS: " + OS[5] + "\nSing. Ant.: " + OS[6]);
                }
                if (Col_Num == 17 && pack.MOPS != null)
                {
                    string[] MOPS = pack.MOPS;
                    MessageBox.Show("MOPS Version:\n\nVNS: " + MOPS[0] + "\nVN: " + MOPS[1] + "\nLTT: " + MOPS[2]);
                }
                if (Col_Num == 22 && pack.Met_Report != null)
                {
                    string[] MR = pack.Met_Report;
                    MessageBox.Show("Met Report:\n\nWind Speed: " + MR[0] + "\nWind Direction: " + MR[1] + "\nTemperature: " + MR[2] +
                            "Turbulence: " + MR[3]);
                }
                if (Col_Num == 25 && pack.Target_Status != null)
                {
                    string[] TS = pack.Target_Status;
                    MessageBox.Show("Target Status:\n\nICF: " + TS[0] + "\nLNAV: " + TS[1] + "\nPS: " + TS[2] + "\nSS: " + TS[3]);
                }
                if (Col_Num == 28 && pack.Quality_Indicators != null)
                {
                    string[] QI = pack.Quality_Indicators;
                    MessageBox.Show("Quality Indicators:\n\n" + QI[0] + "\n" + QI[1] + "\n" + QI[2] + "\n" + "\nSIL Supplement: " +
                           QI[3] + "\nSDA: " + QI[4] + "\nGVA: " + QI[5] + "\nPIC: " + QI[6]);
                }
                if (Col_Num == 29 && pack.Mode_S != null)
                {
                    int[] MS = pack.Mode_S;
                    MessageBox.Show("Mode S MB Data:\n\nRep. Mode S MB Data: " + MS[0] + "\nMB Data: " + MS[1] + "\nBDS 1: " + MS[2] +
                            "\nBDS 2: " + MS[3]);
                }
                if (Col_Num == 36 && pack.TMRP_HP != null)
                {
                    string[] TMRP = pack.TMRP_HP;
                    MessageBox.Show("Time of Message Reception for Position\nHigh Precision:\n\nFull Second Indication: " + TMRP[0] +
                            "\nTMR Posiotion: " + TMRP[1]);
                }
                if (Col_Num == 37 && pack.TMRV_HP != null)
                {
                    string[] TMRV = pack.TMRV_HP;
                    MessageBox.Show("Time of Message Reception for velocity\nHigh Precision:\n\nFull Second Indication: " + TMRV[0] +
                           "\nTMR Velocity: " + TMRV[1]);
                }
                if (Col_Num == 39 && pack.Trajectory_Intent != null)
                {
                    string[] TI = pack.Trajectory_Intent;
                    MessageBox.Show("Trajectory Intent:\n\n" + TI[0] + "\nNVB: " + TI[1] + "\nNAV: " + TI[2] + "\nREP: " + TI[3] + "\nTCA: " + TI[0] +
                            "\nNC: " + TI[5] + "\nTCP Number: " + TI[6] + "\nLatitude TID: " + TI[7] + "\nLongitude TID: " + TI[8] + "\nAltitude (feet): " +
                            TI[9] + "\nPoint Type: " + TI[10] + "\nTD: " + TI[11] + "\nTRA: " + TI[12] + "\nTOA: " + TI[13] + "\nTTR (NM): " + TI[15]);
                }
                if (Col_Num == 40 && pack.Data_Ages != null)
                {
                    double[] DA = pack.Data_Ages;
                    MessageBox.Show("Data Ages:\n\nAOS: " + DA[0] + "\nTRD: " + DA[1] + "\n Mode 3A: " + DA[2] + "\nQI: " + DA[3] + "\nTI: " + DA[4] +
                            "\nMAM: " + DA[5] + "\nGH: " + DA[6] + "\nFL: " + DA[7] + "\nISA: " + DA[8] + "\nFSA: " + DA[9] + "\nAS: " + DA[10] + "\nTAS: " +
                            DA[11] + "\nMH: " + DA[12] + "\nBVR: " + DA[13] + "\nGVR: " + DA[14] + "\nGV: " + DA[15] + "\nTAR: " + DA[16] + "\nTarget ID: " +
                            DA[17] + "\nTS: " + DA[18] + "Met: " + DA[19] + "\nROA: " + DA[20] + "\nARA: " + DA[21] + "\nSCC: " + DA[22]);
                }
            }
            // Mixt category
            if (category == 1021)
            {
                DataTable tabla = F.getTablaMixtCAT();
                int cat = Convert.ToInt32(tabla.Rows[Row_Num][1]);
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
                            "\nTTF: " + SS[5]);
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
                    MessageBox.Show("Solving CAT21_v23 compatibility problems");
                    //DataTable table = F.getTablaMixtCAT();
                    //DataTable pack21 = new DataTable();
                    //pack21.ImportRow(table.Rows[Row_Num]);
                    //if (Col_Num == 7 && pack21.Target_Report_Desc != null)
                    //{
                    //    string[] TRD = pack21.Target_Report_Desc;
                    //    MessageBox.Show("Target Report:\n\nATP: " + TRD[0] + "\nARC: " + TRD[1] + "\nRC: " + TRD[2] + "\nRAB :" + TRD[3] +
                    //        "\nDCR: " + TRD[4] + "\nGBS: " + TRD[5] + "\nSIM: " + TRD[6] + "\nTST: " + TRD[7] + "\nSAA: " + TRD[8] + "\nCL: " +
                    //        TRD[9] + "\nIPC: " + TRD[10] + "\nNOGO" + TRD[11] + "\nCPR: " + TRD[12] + "\nLDPJ: " + TRD[13] + "\nRCF: " + TRD[14]);
                    //}
                    //if (Col_Num == 9 && pack21.M3AC != null)
                    //{

                    //}
                    //if (Col_Num == 10 && pack21.Mode_S != null)
                    //{
                    //    int[] MS = pack21.Mode_S;
                    //    MessageBox.Show("Mode S MB Data:\n\nRep. Mode S MB Data: " + MS[0] + "\nMB Data: " + MS[1] + "\nBDS 1: " + MS[2] +
                    //        "\nBDS 2: " + MS[3]);
                    //}
                    //if (Col_Num == 31 && pack21.Op_Status != null)
                    //{
                    //    string[] OS = pack21.Op_Status;
                    //    MessageBox.Show("Operational Status:\n\nRA: " + OS[0] + "\nTC: " + OS[1] + "\nTS: " + OS[2] + "\nARV: " + OS[3] +
                    //        "\nCDITA: " + OS[4] + "\nNot TCAS: " + OS[5] + "\nSing. Ant.: " + OS[6]);
                    //}
                    //if (Col_Num == 37 && pack21.MOPS != null)
                    //{
                    //    string[] MOPS = pack21.MOPS;
                    //    MessageBox.Show("MOPS Version:\n\nVNS: " + MOPS[0] + "\nVN: " + MOPS[1] + "\nLTT: " + MOPS[2]);
                    //}
                    //if (Col_Num == 41 && pack21.Met_Report != null)
                    //{
                    //    string[] MR = pack21.Met_Report;
                    //    MessageBox.Show("Met Report:\n\nWind Speed: " + MR[0] + "\nWind Direction: " + MR[1] + "\nTemperature: " + MR[2] +
                    //        "Turbulence: " + MR[3]);
                    //}
                    //if (Col_Num == 43 && pack21.Target_Status != null)
                    //{
                    //    string[] TS = pack21.Target_Status;
                    //    MessageBox.Show("Target Status:\n\nICF: " + TS[0] + "\nLNAV: " + TS[1] + "\nPS: " + TS[2] + "\nSS: " + TS[3]);
                    //}
                    //if (Col_Num == 46 && pack21.Quality_Indicators != null)
                    //{
                    //    string[] QI = pack21.Quality_Indicators;
                    //    MessageBox.Show("Quality Indicators:\n\n" + QI[0] + "\n" + QI[1] + "\n" + QI[2] + "\n" + "\nSIL Supplement: " +
                    //        QI[3] + "\nSDA: " + QI[4] + "\nGVA: " + QI[5] + "\nPIC: " + QI[6]);
                    //}
                    //if (Col_Num == 52 && pack21.TMRP_HP != null)
                    //{
                    //    string[] TMRP = pack21.TMRP_HP;
                    //    MessageBox.Show("Time of Message Reception for Position\nHigh Precision:\n\nFull Second Indication: " + TMRP[0] +
                    //        "\nTMR Posiotion: " + TMRP[1]);
                    //}
                    //if (Col_Num == 53 && pack21.TMRV_HP != null)
                    //{
                    //    string[] TMRV = pack21.TMRV_HP;
                    //    MessageBox.Show("Time of Message Reception for velocity\nHigh Precision:\n\nFull Second Indication: " + TMRV[0] +
                    //        "\nTMR Velocity: " + TMRV[1]);
                    //}
                    //if (Col_Num == 55 && pack21.Trajectory_Intent != null)
                    //{
                    //    string[] TI = pack21.Trajectory_Intent;
                    //    MessageBox.Show("Trajectory Intent:\n\n" + TI[0] + "\nNVB: " + TI[1] + "\nNAV: " + TI[2] + "\nREP: " + TI[3] + "\nTCA: " + TI[0] +
                    //        "\nNC: " + TI[5] + "\nTCP Number: " + TI[6] + "\nLatitude TID: " + TI[7] + "\nLongitude TID: " + TI[8] + "\nAltitude (feet): " +
                    //        TI[9] + "\nPoint Type: " + TI[10] + "\nTD: " + TI[11] + "\nTRA: " + TI[12] + "\nTOA: " + TI[13] + "\nTTR (NM): " + TI[15]);
                    //}
                    //if (Col_Num == 56 && pack21.Data_Ages != null)
                    //{
                    //    double[] DA = pack21.Data_Ages;
                    //    MessageBox.Show("Data Ages:\n\nAOS: " + DA[0] + "\nTRD: " + DA[1] + "\n Mode 3A: " + DA[2] + "\nQI: " + DA[3] + "\nTI: " + DA[4] +
                    //        "\nMAM: " + DA[5] + "\nGH: " + DA[6] + "\nFL: " + DA[7] + "\nISA: " + DA[8] + "\nFSA: " + DA[9] + "\nAS: " + DA[10] + "\nTAS: " +
                    //        DA[11] + "\nMH: " + DA[12] + "\nBVR: " + DA[13] + "\nGVR: " + DA[14] + "\nGV: " + DA[15] + "\nTAR: " + DA[16] + "\nTarget ID: " +
                    //        DA[17] + "\nTS: " + DA[18] + "Met: " + DA[19] + "\nROA: " + DA[20] + "\nARA: " + DA[21] + "\nSCC: " + DA[22]);
                    //}
                }
            }
        }
        private void SearchNum_Click(object sender, RoutedEventArgs e)
        {
            Search_Table.ItemsSource = null;
            Search_Table.Items.Clear();
            try
            {
                // Searching
                int search = Convert.ToInt32(NumBox.Text);
                if (category == 10)
                {
                    try
                    {
                        DataTable C10 = F.getTablaCAT10();
                        DataTable table = F.getSearchTable10();
                        table.ImportRow(C10.Rows[search - 1]);

                        Search_Table.ItemsSource = table.DefaultView;
                    }
                    catch { MessageBox.Show("There is no item number " + search + " in this file"); }
                }
                if (category == 21)
                {
                    try
                    {
                        DataTable C21 = F.getTablaCAT21();
                        DataTable table = F.getSearchTable21();
                        table.ImportRow(C21.Rows[search - 1]);

                        Search_Table.ItemsSource = table.DefaultView;
                    }
                    catch { MessageBox.Show("There is no item number " + search + " in this file"); }
                }
                if (category == 1021)
                {
                    try
                    {
                        DataTable table = F.getTablaMixtCAT();
                        DataTable searchtable = F.getSearchTableMixed();
                        searchtable.ImportRow(table.Rows[search - 1]);

                        Search_Table.ItemsSource = searchtable.DefaultView;
                    }
                    catch { MessageBox.Show("There is no item number " + search + " in this file"); }
                }
            }
            catch { MessageBox.Show("It must be an integer"); }
        }
        private void SearchID_Click(object sender, RoutedEventArgs e)
        {
            Search_Table.ItemsSource = null;
            Search_Table.Items.Clear();

            string search = IDBox.Text;
            if (category == 10)
            {
                DataTable table10 = F.getTablaCAT10();
                DataTable searchtable = F.getSearchTable10();
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
                DataTable table21 = F.getTablaCAT21();
                DataTable searchtable = F.getSearchTable21();
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
            if (category == 1021)
            {
                DataTable tableMix = F.getTablaMixtCAT();
                DataTable searchtable = F.getSearchTableMixed();
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
            }
            else
            {
                // Table and Map Stuff
                Search_Table.Visibility = Visibility.Hidden;
                SearchResult.Visibility = Visibility.Hidden;
                SearchNumButton.Visibility = Visibility.Hidden;
                SearchIDButton.Visibility = Visibility.Hidden;
                NumBox.Visibility = Visibility.Hidden;
                IDBox.Visibility = Visibility.Hidden;
                Instructions_Label.Visibility = Visibility.Hidden;
                Track_Table.Visibility = Visibility.Hidden;
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
                zoomcat.Visibility = Visibility.Visible;
                SearchMapbyID.Visibility = Visibility.Visible;
                callsignbox.Visibility = Visibility.Visible;


                // Visual Stuff
                asterixPNG.Visibility = Visibility.Hidden;
                asterixPerf.Visibility = Visibility.Hidden;
                bubbleWord.Visibility = Visibility.Hidden;
                circle.Visibility = Visibility.Hidden;
                circle2.Visibility = Visibility.Hidden;
                SearchNumButton.Visibility = Visibility.Hidden;
                SearchIDButton.Visibility = Visibility.Hidden;
                NumBox.Visibility = Visibility.Hidden;
                IDBox.Visibility = Visibility.Hidden;
                SearchResult.Visibility = Visibility.Hidden;

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
        }
        private void Map_Load(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            map.MapProvider = OpenStreetMapProvider.Instance;
            map.MinZoom = 8;
            map.MaxZoom = 16;
            map.Zoom = 14;
            map.Position = new PointLatLng(MLAT_lat, MLAT_lon);
            map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            map.CanDragMap = true;
            map.DragButton = MouseButton.Left;
        }
        int categoria;
        public int comprobarcat()
        {
            if (F.CAT_list[0] == 10)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(21);
                if (IsMultipleCAT == true)
                {
                    return categoria = 1021;
                }
                else
                {
                    return categoria = 10;
                }
            }
            if (F.CAT_list[0] == 21)
            {
                bool IsMultipleCAT = F.CAT_list.Contains(10);
                if (IsMultipleCAT == true)
                {
                    return categoria = 1021;
                }
                else
                {
                    return categoria = 21;
                }

            }
            else { return 0; }
        }
        public void addMarker_Click(object sender, RoutedEventArgs e)
        {
            if (comprobarcat() == 10)
            {
                dt_Timer.Tick += dt_Timer_TickC10;
                updatedtable = F.gettablacat10reducida().Clone();
            }
            if (comprobarcat() == 21)
            {
                dt_Timer.Tick += dt_Timer_TickC21;
                updatedtable = F.gettablacat21reducida().Clone();
            }
            if (comprobarcat() == 1021)
            {
                dt_Timer.Tick += dt_Timer_TickMULTICAT;
                updatedtable = F.getmultiplecattablereducida().Clone();
            }
            dt_Timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            dt_Timer.Start();
        }
        public void AddMarkerMLAT(double latitude, double longitude)
        {
            PointLatLng point = fromXYtoLatLongMLAT(latitude, longitude);
            GMapMarker marker = new GMapMarker(point);
            marker.Shape = new Image
            {
                Width = 15,
                Height = 15,
                Source = new BitmapImage(new Uri("pack://application:,,,/Images/airplane.png"))
            };
            map.Markers.Add(marker);
        }
        public void AddMarkerSMR(double latitude, double longitude)
        {
            PointLatLng point = fromXYtoLatLongSMR(latitude, longitude);
            GMapMarker marker = new GMapMarker(point);
            marker.Shape = new Image
            {
                Width = 15,
                Height = 15,
                Source = new BitmapImage(new Uri("pack://application:,,,/Images/airplane.png"))
            };
            map.Markers.Add(marker);
        }
        public void AddMarkerC21(double latitude, double longitude)
        {
            PointLatLng point = new PointLatLng(latitude, longitude);
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
        private void dt_Timer_TickC10(object sender, EventArgs e)
        {
            Boolean x = true;
            while (x == true)
            {
                
                CAT10 C10 = F.getCAT10(i);
                double start = Math.Floor(F.getCAT10(0).Time_Day) + s;
                double tiempo = Math.Floor(C10.Time_Day);
                if (C10.Target_ID.Contains(searchedcallsign))
                {
                    if (tiempo == start)
                    {
                        if (C10.Target_Rep_Descript[0] == "PSR")
                        {
                            AddMarkerSMR(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
                        }
                        if (C10.Target_Rep_Descript[0] == "Mode S Multilateration")
                        {
                            AddMarkerMLAT(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
                        }
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
                }
                else
                {
                    if (tiempo == start)
                    {
                        if (C10.Target_Rep_Descript[0] == "PSR")
                        {
                            AddMarkerSMR(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
                        }
                        if (C10.Target_Rep_Descript[0] == "Mode S Multilateration")
                        {
                            AddMarkerMLAT(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
                        }
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
                }
                clock(tiempo);
                gridlista.Visibility = Visibility.Hidden;
                updatedlista.Visibility = Visibility.Visible;
                rellenartablaCAT10(i);
            }
        }
        double start;
        double tiempo;
        private void dt_Timer_TickC21(object sender, EventArgs e)
        {
            Boolean x = true;
            while (x == true)
            {
                if (F.getFileName().Contains("v023") == true || F.getFileName().Contains("v23") == true)
                {
                    CAT21_v23 C21_v23 = F.getCAT21_v23(i);
                    start = Math.Floor(F.getCAT21_v23(0).Time_of_Day) + s;
                    tiempo = Math.Floor(C21_v23.Time_of_Day);
                    if (tiempo == start)
                    {
                        AddMarkerC21(C21_v23.Lat_WGS_84, C21_v23.Lon_WGS_84);
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
                }
                if (F.getFileName().Contains("v021") == true || F.getFileName().Contains("v21") == true)
                {
                    CAT21 C21 = F.getCAT21(i);
                    start = Math.Floor(F.getCAT21(0).Time_Rep_Transm) + s;
                    tiempo = Math.Floor(C21.Time_Rep_Transm);
                    if (searchedcallsign == null) { searchedcallsign = "Nada"; }
                    if (idbuttonclicked==true)
                    {
                        //HACER UNA FOR QUE PLOTEE SOLAMENTE LOS PAQUETES CON TARGET ID IGUAL A SEARCHEDCALLSIGN Y SOLAMENTE RELLENE EN RELLENAR TABLA CAT21 ESOS PAQUETES. TENER EM CUENTA QUE EL TIMER TIENE K SEGUIR RULANDO
                        if (C21.Target_ID.Contains(searchedcallsign))
                        {
                            if (tiempo == start)
                            {
                                AddMarkerC21(C21.High_Res_Lat_WGS_84, C21.High_Res_Lon_WGS_84);
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
                        }
                        else { i++; }
                    }
                    
                    else
                    {
                        if (tiempo == start)
                        {
                            AddMarkerC21(C21.High_Res_Lat_WGS_84, C21.High_Res_Lon_WGS_84);
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
                    }
               }
                clock(tiempo);
                gridlista.Visibility = Visibility.Hidden;
                updatedlista.Visibility = Visibility.Visible;
                rellenartablaCAT21(i);
            }
        }
        int n = 0;
        private void dt_Timer_TickMULTICAT(object sender, EventArgs e)
        {
            Boolean x = true;
            DataTable tabla = F.gettablamixtareducida();
            while (x == true)
            {
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    try
                    {
                        string tiempomal = Convert.ToString(tabla.Rows[i][2]);
                        string starttiempo = Convert.ToString(tabla.Rows[0][2]);
                        string[] tiemposplited = tiempomal.Split(':');
                        string[] tiemposplitedstart = starttiempo.Split(':');

                        int tiempo = gettimecorrectly(tiemposplited);
                        int start = gettimecorrectly(tiemposplitedstart) + n;

                        if (F.CAT_list[i] == 10 && tiempo == start)
                        {
                            double poscartx = Convert.ToDouble(tabla.Rows[i][6]);
                            double poscarty = Convert.ToDouble(tabla.Rows[i][7]);
                            AddMarkerMLAT(poscartx, poscarty);
                            if (map.Markers.Count >= 200)
                            {
                                map.Markers[map.Markers.Count - 200].Clear();
                            }
                            rellenartablaMULTICAT(i);
                            clock(tiempo);

                        }
                        if (F.CAT_list[i] == 21 && tiempo == start)
                        {
                            double poscartx = Convert.ToDouble(tabla.Rows[i][6]);
                            double poscarty = Convert.ToDouble(tabla.Rows[i][7]);
                            AddMarkerC21(poscartx, poscarty);
                            if (map.Markers.Count >= 200)
                            {
                                map.Markers[map.Markers.Count - 200].Clear();
                            }
                            rellenartablaMULTICAT(i);
                            clock(tiempo);

                        }
                    }
                    catch
                    {
                        i++;
                    }


                }
                x = false;
                n++;
            }
            gridlista.Visibility = Visibility.Hidden;
            updatedlista.Visibility = Visibility.Visible;
        }

        public int gettimecorrectly(string[] tod)
        {
            int secabs = Convert.ToInt32(tod[0]) * 3600 + Convert.ToInt32(tod[1]) * 60 + Convert.ToInt32(tod[2]);
            return secabs;

        }
        private void rellenartablaCAT10(int i)
        {
            //we copy/paste all data from that specific flight
            updatedtable.ImportRow(F.gettablacat10reducida().Rows[i - 1]);
            updatedlista.ItemsSource = updatedtable.DefaultView;
            updatedlista.ScrollIntoView(updatedlista.Items.GetItemAt(updatedlista.Items.Count - 1));

        }
        private void rellenartablaCAT21(int i)
        {
            //we copy/paste all data from that specific flight
            updatedtable.ImportRow(F.gettablacat21reducida().Rows[i - 1]);
            updatedlista.ItemsSource = updatedtable.DefaultView;
            updatedlista.ScrollIntoView(updatedlista.Items.GetItemAt(updatedlista.Items.Count - 1));
        }
        private void rellenartablaMULTICAT(int i)
        {
            //we copy/paste all data from that specific flight
            updatedtable.ImportRow(F.gettablamixtareducida().Rows[i]);
            updatedlista.ItemsSource = updatedtable.DefaultView;
            updatedlista.ScrollIntoView(updatedlista.Items.GetItemAt(updatedlista.Items.Count - 1));

        }

        private void clock(double tiempo)
        {
            TimeSpan time = TimeSpan.FromSeconds(tiempo);
            string tiempoact = time.ToString(@"hh\:mm\:ss");
            timer.Text = tiempoact;
        }
        private void stop_Click(object sender, RoutedEventArgs e)
        {
            dt_Timer.Stop();
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
        private PointLatLng fromXYtoLatLongSMR(double X, double Y)
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
            map.MinZoom = 1;
        }
        string searchedcallsign;
        bool idbuttonclicked = false;
        //plot only airplanes with searched callsign
        private void SearchIDMAP_Click(object sender, RoutedEventArgs e)
        {
            idbuttonclicked = true;
            string id = Convert.ToString(updatedtable.Rows[i-1][4]);
            if (id != null)
            {
                searchedcallsign = callsignbox.Text;
            }
            else { MessageBox.Show("Make sure Target ID exists"); }


        }
    }
}
