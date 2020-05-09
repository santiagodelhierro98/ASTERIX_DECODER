using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CLASSES
{
    public class Fichero
    {
        Metodos M = new Metodos();

        string path;
        public int CAT;
        public double[] SICSAC;
        public string filename;

        public List<double> CAT_list = new List<double>();

        List<CAT10> listaCAT10 = new List<CAT10>();
        List<CAT21> listaCAT21 = new List<CAT21>();
        List<CAT21_v23> listaCAT21_v23 = new List<CAT21_v23>();

        //Reduced table with items for display on map
        public DataTable tablacat10reducida = new DataTable();
        public DataTable tablacat21reducida = new DataTable();
        public DataTable multiplecattablereducida = new DataTable();

        public DataTable tablaCAT10 = new DataTable();
        public DataTable tablaCAT21 = new DataTable();
        public DataTable tablaMultipleCAT = new DataTable();

        public Fichero(string nombre)
        {
            this.path = nombre;
            M.CreateReducedTable(multiplecattablereducida, tablacat10reducida, tablacat21reducida);
            M.Create_TrackTable_Puras(tablaCAT10, tablaCAT21);
            M.Create_TrackTable_Multiple(tablaMultipleCAT);
        }
        public void leer()
        {
            //StreamReader fichero = new StreamReader(path);
            //string linea = fichero.ReadLine();
            byte[] fileBytes = File.ReadAllBytes(path);
            filename = path;

            List<string[]> listahex = M.File_to_HexaList(fileBytes, path);

            int contadorGeneral = 0;
            int contadorCAT10 = 0;
            int contadorCAT21 = 0;

            bool multicat = M.IsAMulticatFile(listahex);
            
            for (int q = 0; q < listahex.Count; q++)
            {
                string[] arraystring = listahex[q];
                CAT = int.Parse(arraystring[0], System.Globalization.NumberStyles.HexNumber);
                contadorGeneral++;

                if (CAT == 10)
                {
                    if (multicat == false)
                    {
                        CAT10 C10 = new CAT10();
                        CAT_list.Add(CAT); // Adds the pack category into this list
                        contadorCAT10++;
                        C10.Decode10(arraystring);
                        listaCAT10.Add(C10);

                        // CAT10 reduced table for maptrack
                        tablacat10reducida.Rows.Add(contadorCAT10, C10.Target_ID, M.convert_to_hms(Math.Floor(C10.Time_Day)), C10.FL[2], C10.Data_Source_ID[0], C10.Data_Source_ID[1],
                            Math.Round(M.cartesiantolatmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]), 2), Math.Round(M.cartesiantolonmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]), 2),
                            C10.Target_Add, C10.Track_Num);

                        // Complete CAT10 table
                        tablaCAT10.Rows.Add(contadorCAT10, CAT, C10.Data_Source_ID[0], C10.Data_Source_ID[1], C10.Target_ID, C10.Track_Num, "Click to View Data",
                               C10.Message_Type, M.convert_to_hms(Math.Floor(C10.Time_Day)), "(" + C10.Pos_WGS84[0] + ", " + C10.Pos_WGS84[1] + ")", "(" + C10.Pos_PolarCoord[0] + ", " + C10.Pos_PolarCoord[1] + ")",
                               "(" + C10.Pos_Cartesian[0] + ", " + C10.Pos_Cartesian[1] + ")", "(" + C10.Track_Vel_Polar[0] + ", " + C10.Track_Vel_Polar[1] + ")", "(" + C10.Track_Vel_Cartesian[0] +
                               ", " + C10.Track_Vel_Cartesian[1] + ")", "Click to View Data", "Click to View Data", C10.Target_Add, "Click to View Data", C10.Fleet_ID, C10.FL[2], C10.Height,
                               "(" + C10.Target_Size_Heading[0] + ", " + C10.Target_Size_Heading[2] + ")", C10.Target_Size_Heading[1], "Click to View Data", "Click to View Data",
                               "(" + C10.StndrdDev_Position[0] + ", " + C10.StndrdDev_Position[1] + ")", C10.StndrdDev_Position[2], "Click to View Data", C10.Amplitude, "(" + C10.Acceleration[0] +
                               ", " + C10.Acceleration[1] + ")");                        
                    }
                    if (multicat == true)
                    {
                        CAT10 C10 = new CAT10();
                        CAT_list.Add(CAT); // Adds the pack category into this list
                        contadorCAT10++;
                        C10.Decode10(arraystring);
                        listaCAT10.Add(C10);

                        // Multiple CAT reduced table for maptrack
                        multiplecattablereducida.Rows.Add(contadorGeneral, C10.Target_ID, M.convert_to_hms(Math.Floor(C10.Time_Day)), C10.FL[2], C10.Data_Source_ID[0], C10.Data_Source_ID[1],
                            Math.Round(M.cartesiantolatmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]), 2), Math.Round(M.cartesiantolonmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]), 2),
                            C10.Target_Add, C10.Track_Num);

                        // Complete Multiple CAT table
                        tablaMultipleCAT.Rows.Add(contadorGeneral, CAT, C10.Data_Source_ID[0], C10.Data_Source_ID[1], C10.Target_ID, C10.Track_Num, M.convert_to_hms(Math.Floor(C10.Time_Day)),
                               "Click to View Data", "(" + C10.Pos_WGS84[0] + ", " + C10.Pos_WGS84[1] + ")", "Click to View Data", "Click to View Data", C10.FL[2], C10.Height,
                               C10.Target_Add, C10.Amplitude,

                               "(" + C10.Target_Size_Heading[0] + ", " + C10.Target_Size_Heading[2] + ")", C10.Target_Size_Heading[1], C10.Message_Type, "(" + C10.Pos_PolarCoord[0] +
                               ", " + C10.Pos_PolarCoord[1] + ")", "(" + C10.Pos_Cartesian[0] + ", " + C10.Pos_Cartesian[1] + ")", "(" + C10.Track_Vel_Polar[0] + ", " + C10.Track_Vel_Polar[1] +
                               ")", "(" + C10.Track_Vel_Cartesian[0] + ", " + C10.Track_Vel_Cartesian[1] + ")", "Click To View Data", C10.Fleet_ID, "Click to View Data", "Click to View Data",
                               "(" + C10.StndrdDev_Position[0] + ", " + C10.StndrdDev_Position[1] + ")", C10.StndrdDev_Position[2], "Click to View Data", "(" + C10.Acceleration[0] +
                               ", " + C10.Acceleration[1] + ")", "", "", "", "",
                               "", "", "", "", "", "", "", "", "", "", "",
                               "", "", "", "", "", "", "", "", "", "", "", "");
                    }
                }
                if (CAT == 21)
                {
                    SICSAC = M.getSIC_SAC(arraystring);
                    contadorCAT21++;
                    // Check if its version 23
                    if (multicat == false && SICSAC[0] == 107 && SICSAC[1] == 0)
                    {
                        CAT21_v23 C21_v23 = new CAT21_v23();
                        CAT_list.Add(CAT + 0.23); // Adds the pack category into this list
                        C21_v23.Decode21_23(arraystring);
                        listaCAT21_v23.Add(C21_v23);
                        
                        // CAT21 reduced table for maptrack
                        tablacat21reducida.Rows.Add(contadorCAT21, C21_v23.Target_ID, M.convert_to_hms(Math.Floor((C21_v23.Time_of_Day))), C21_v23.FL, C21_v23.Data_Source_ID_SIC, C21_v23.Data_Source_ID_SAC,
                            Math.Round(C21_v23.Lat_WGS_84, 3), Math.Round(C21_v23.Lon_WGS_84, 3), C21_v23.Target_Address, "Null");

                        // Complete CAT21 table
                        tablaCAT21.Rows.Add(contadorCAT21, CAT + 0.23, C21_v23.Data_Source_ID_SIC, C21_v23.Data_Source_ID_SAC, C21_v23.Target_ID, " ", "Click to View Data",
                            M.convert_to_hms(Math.Floor((C21_v23.Time_of_Day))), "(" + Math.Round(C21_v23.Lat_WGS_84, 3) + ", " + Math.Round(C21_v23.Lon_WGS_84, 3) + ")", "(" + Math.Round(C21_v23.Lat_WGS_84, 3) + ", " + Math.Round(C21_v23.Lon_WGS_84, 3) + ")", C21_v23.FL, C21_v23.GA,
                            "", "(" + C21_v23.Air_Speed[0] + ", " + C21_v23.Air_Speed[1] + ")", C21_v23.True_Airspeed, "(" + C21_v23.GS + ", " + C21_v23.TA + ")", "", "", "",
                            C21_v23.MH, C21_v23.BVR, C21_v23.GVR, "", "Click to View Data", C21_v23.ECAT, C21_v23.Target_Address, "Click to View Data", C21_v23.Roll, "",
                            "", "", "", "", "", "", "", "", "", "", "", "Click To View Data", "", "");
                    }
                    // Check if its version 21
                    if (multicat == false && (SICSAC[0] != 107 || SICSAC[1] != 0))
                    {
                        CAT21 C21 = new CAT21();
                        CAT_list.Add(CAT); // Adds the pack category into this list
                        C21.Decode21(arraystring);
                        listaCAT21.Add(C21);

                        // CAT21 reduced table for maptrack
                        tablacat21reducida.Rows.Add(contadorCAT21, C21.Target_ID, M.convert_to_hms(Math.Floor(C21.Time_Rep_Transm)), C21.FL, C21.Data_Source_ID_SIC, C21.Data_Source_ID_SAC,
                           Math.Round(C21.High_Res_Lat_WGS_84, 3), Math.Round(C21.High_Res_Lon_WGS_84, 3), C21.Target_Address, C21.Track_Num);

                        // Complete CAT21 table
                        tablaCAT21.Rows.Add(contadorCAT21, CAT, C21.Data_Source_ID_SIC, C21.Data_Source_ID_SAC, C21.Target_ID, C21.Track_Num, "Click to View Data",
                            M.convert_to_hms(Math.Floor(C21.Time_Rep_Transm)), "(" + C21.Lat_WGS_84 + ", " + C21.Lon_WGS_84 + ")", "(" + Math.Round(C21.High_Res_Lat_WGS_84, 3) + ", " + Math.Round(C21.High_Res_Lon_WGS_84, 3) + ")", C21.FL, C21.GH,
                            "Click to View Data", "(" + C21.Air_Speed[0] + ", " + C21.Air_Speed[1] + ")", C21.True_Airspeed, "(" + C21.GS + ", " + C21.TA + ")", C21.TAR, C21.SA, "Click to View Data",
                            C21.MH, C21.BVR, C21.GVR, C21.M3AC, "Click to View Data", C21.ECAT, C21.Target_Address, "Click to View Data", C21.Roll, C21.Service_ID,
                            "Click to View Data", "Click to View Data", C21.MAM, C21.RID, C21.ToA_Position, C21.ToA_Velocity, C21.TMRP, C21.TMRV, "(" + C21.TMRP_HP[0] + ", " + C21.TMRV_HP[1] + ")", "(" + C21.TMRV_HP[0] + ", " + C21.TMRP_HP[1] + ")", C21.Time_Rep_Transm, "Click to View Data", "Click to View Data", C21.RP);
                    }
                    if (multicat == true && (SICSAC[0] == 107 && SICSAC[1] == 0))
                    {
                        CAT21_v23 C21_v23 = new CAT21_v23();
                        CAT_list.Add(CAT + 0.23); // Adds the pack category into this list
                        C21_v23.Decode21_23(arraystring);
                        listaCAT21_v23.Add(C21_v23);

                        // Multiple CAT reduced table for maptrack
                        multiplecattablereducida.Rows.Add(contadorGeneral, C21_v23.Target_ID, M.convert_to_hms(Math.Floor((C21_v23.Time_of_Day))), C21_v23.FL, C21_v23.Data_Source_ID_SIC, C21_v23.Data_Source_ID_SAC,
                        Math.Round(C21_v23.Lat_WGS_84, 3), Math.Round(C21_v23.Lon_WGS_84, 3), C21_v23.Target_Address, "Null");

                        // Complete Multiple CAT table
                        tablaMultipleCAT.Rows.Add(contadorGeneral, CAT + 0.23, C21_v23.Data_Source_ID_SIC, C21_v23.Data_Source_ID_SAC, C21_v23.Target_ID, "", M.convert_to_hms(Math.Floor((C21_v23.Time_of_Day))),
                            "Click to View Data", "(" + Math.Round(C21_v23.Lat_WGS_84, 3) + ", " + Math.Round(C21_v23.Lon_WGS_84, 3) + ")", "", "", C21_v23.FL, C21_v23.GA, C21_v23.Target_Address, "",

                            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                            "(" + Math.Round(C21_v23.Lat_WGS_84, 3) + ", " + Math.Round(C21_v23.Lon_WGS_84, 3) + ")", "", "(" + C21_v23.Air_Speed[0] + ", " + C21_v23.Air_Speed[1] + ")",
                            C21_v23.True_Airspeed, "(" + C21_v23.GS + ", " + C21_v23.TA + ")", "", "", "", C21_v23.MH, C21_v23.BVR, C21_v23.GVR, "Click to View Data", C21_v23.ECAT, "", C21_v23.Roll, "",
                            "", "", "", "", "", "", "", "", "", "Click to View Data", "", "");
                    }
                    if (multicat == true && (SICSAC[0] != 107 || SICSAC[1] != 0))
                    {
                        CAT21 C21 = new CAT21();
                        CAT_list.Add(CAT); // Adds the pack category into this list
                        C21.Decode21(arraystring);
                        listaCAT21.Add(C21);

                        // Multiple CAT reduced table for maptrack
                        multiplecattablereducida.Rows.Add(contadorGeneral, C21.Target_ID, M.convert_to_hms(Math.Floor((C21.Time_Rep_Transm))), C21.FL, C21.Data_Source_ID_SIC, C21.Data_Source_ID_SAC,
                        Math.Round(C21.Lat_WGS_84, 3), Math.Round(C21.Lon_WGS_84, 3), C21.Target_Address, "Null");

                        // Complete Multiple CAT table
                        tablaMultipleCAT.Rows.Add(contadorGeneral, CAT, C21.Data_Source_ID_SIC, C21.Data_Source_ID_SAC, C21.Target_ID, "", M.convert_to_hms(Math.Floor((C21.Time_Rep_Transm))),
                            "Click to View Data", "(" + Math.Round(C21.Lat_WGS_84, 3) + ", " + Math.Round(C21.Lon_WGS_84, 3) + ")", "", "", C21.FL, C21.GH, C21.Target_Address, "",

                            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                            "(" + Math.Round(C21.Lat_WGS_84, 3) + ", " + Math.Round(C21.Lon_WGS_84, 3) + ")", "", "(" + C21.Air_Speed[0] + ", " + C21.Air_Speed[1] + ")",
                            C21.True_Airspeed, "(" + C21.GS + ", " + C21.TA + ")", "", "", "", C21.MH, C21.BVR, C21.GVR, "Click to View Data", C21.ECAT, "", C21.Roll, "",
                            "", "", "", "", "", "", "", "", "", "Click to View Data", "", "");
                    }
                }
            }
        }
        public CAT10 getCAT10(int num)
        {
            return listaCAT10[num];
        }
        public CAT21 getCAT21(int num)
        {
            return listaCAT21[num];
        }
        public CAT21_v23 getCAT21_v23(int num)
        {
            return listaCAT21_v23[num];
        }
        public int lengthlistaCAT10()
        {
            return listaCAT10.Count;
        }
        public int lengthlistaCAT21()
        {
            return listaCAT21.Count;
        }
        public int lengthlistaCAT21_v23()
        {
            return listaCAT21_v23.Count;
        }

    }
}