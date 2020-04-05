using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Linq;


namespace CLASSES
{
    public class Fichero
    {
        string path;
        int CAT;
        public List<int> CAT_list = new List<int>();

        List<CAT10> listaCAT10 = new List<CAT10>();
        //List<CAT20> listaCAT20 = new List<CAT20>();
        List<CAT21> listaCAT21 = new List<CAT21>();
        
        //Reduced table with items for display on map
        DataTable tablacat10reducida = new DataTable();
        DataTable tablacat21reducida = new DataTable();
        DataTable multiplecattablereducida = new DataTable();
        
        DataTable tablaCAT10 = new DataTable();
        //DataTable tablaCAT20 = new DataTable();
        DataTable tablaCAT21 = new DataTable();
        DataTable tablaMultipleCAT = new DataTable();

        public Fichero(string nombre)
        {
            this.path = nombre;

            this.CreateReducedTable();

            this.Create_TrackTable_Puras();

            this.Create_TrackTable_Multiple();
        }
        public List<CAT10> getListCAT10()
        {
            return listaCAT10;
        }
        public List<CAT21> getListCAT21()
        {
            return listaCAT21;
        }

        public DataTable getmultiplecattablereducida()
        {
            return multiplecattablereducida;
        }
        public DataTable gettablacat10reducida()
        {
            return tablacat10reducida;
        }
        public DataTable gettablacat21reducida()
        {
            return tablacat21reducida;
        }

        public DataTable getTablaCAT10()
        {
            return tablaCAT10;
        }
        public DataTable getTablaCAT21()
        {
            return tablaCAT21;
        }
        public DataTable getTablaMixtCAT()
        {
            return tablaMultipleCAT;
        }
 
        public void leer()
        {
            //StreamReader fichero = new StreamReader(path);
            //string linea = fichero.ReadLine();
            byte[] fileBytes = File.ReadAllBytes(path);
            List<byte[]> listabyte = new List<byte[]>();
            int i = 0;
            int contador = fileBytes[2];
            // int length = 0;
            
            while (i < fileBytes.Length)
            {
                byte[] array = new byte[contador];
                for (int j = 0; j < array.Length; j++)
                {
                    array[j] = fileBytes[i];
                    i++;
                }
                listabyte.Add(array);
                //length += array.Length;
                if (i + 2 < fileBytes.Length)
                {
                    contador = fileBytes[i + 2];
                }
            }

            List<string[]> listahex = new List<string[]>();
            for (int x = 0; x < listabyte.Count; x++)
            {
                byte[] buffer = listabyte[x];
                string[] arrayhex = new string[buffer.Length];
                for (int y = 0; y < buffer.Length; y++)
                {
                    arrayhex[y] = buffer[y].ToString("X");
                }
                listahex.Add(arrayhex);
            }

            int contadorGeneral = 0;
            int contadorCAT10 = 0;
            int contadorCAT21 = 0;
            for (int q = 0; q < listahex.Count; q++)
            {
                string[] arraystring = listahex[q];
                CAT = int.Parse(arraystring[0], System.Globalization.NumberStyles.HexNumber);
                CAT_list.Add(CAT); // Adds the pack category into this list
                contadorGeneral++;

                if (CAT == 10)
                {
                    contadorCAT10++;
                    CAT21 C21 = new CAT21();
                    CAT10 C10 = new CAT10();
                    C10.Decode10(arraystring);
                    listaCAT10.Add(C10);

                    // Multiple CAT reduced table for maptrack
                    multiplecattablereducida.Rows.Add(contadorGeneral, C10.getTargetID10(), C10.getTOD10(),C10.getFL10(), C10.getSIC10(), C10.getSAC10(), Math.Round(cartesiantolatmlat(C10.getX10(),C10.getY10()),2), Math.Round(cartesiantolonmlat(C10.getX10(), C10.getY10()),2), C10.getTargetAddress10(), C10.getTrackNum10());
                    // CAT10 reduced table for maptrack
                    tablacat10reducida.Rows.Add(contadorCAT10, C10.getTargetID10(), C10.getTOD10(), C10.getFL10(), C10.getSIC10(), C10.getSAC10(), Math.Round(cartesiantolatmlat(C10.getX10(), C10.getY10()),2), Math.Round(cartesiantolonmlat(C10.getX10(), C10.getY10()),2), C10.getTargetAddress10(), C10.getTrackNum10());
                    // Complete CAT10 table
                    tablaCAT10.Rows.Add(contadorCAT10, C10.Data_Source_ID[1]+"/"+C10.Data_Source_ID[0], C10.Target_ID[1], C10.Track_Num, C10.Target_Rep_Descript,
                           C10.Message_Type, C10.Time_Day,"("+C10.Pos_WGS84[0] + ", " + C10.Pos_WGS84[1]+")","("+C10.Pos_PolarCoord[0] + ", " + C10.Pos_PolarCoord[1]+")",
                           "("+C10.Pos_Cartesian[0] + ", " + C10.Pos_Cartesian[1]+")","("+C10.Track_Vel_Polar[0] + ", " + C10.Track_Vel_Polar[1]+")","("+C10.Track_Vel_Cartesian[0] +
                           ", " + C10.Track_Vel_Cartesian[1]+")", C10.Track_Status, C10.Mode3A_Code, C10.Target_Add, C10.Mode_SMB, C10.Fleet_ID, C10.FL[2], C10.Height,
                           "("+C10.Target_Size_Heading[0] + ", " + C10.Target_Size_Heading[2]+")", C10.Target_Size_Heading[1], C10.Sys_Status, C10.Pre_Prog_Message,
                           "("+C10.StndrdDev_Position[0] + ", " + C10.StndrdDev_Position[1]+")", C10.StndrdDev_Position[2], C10.Presence, C10.Amplitude, "("+C10.Acceleration[0] +
                           ", " + C10.Acceleration[1] + ")");
                    
                    // Complete Multiple CAT table
                    tablaMultipleCAT.Rows.Add(contadorGeneral, CAT, C10.Data_Source_ID[1] + "/" + C10.Data_Source_ID[0], C10.Target_ID[1], C10.Track_Num, C10.Time_Day,
                           C10.Target_Rep_Descript, "(" + C10.Pos_WGS84[0] + ", " + C10.Pos_WGS84[1] + ")", C10.Mode3A_Code, C10.Mode_SMB, C10.FL[2], C10.Height, 
                           C10.Target_Add, C10.Amplitude,


                           "(" + C10.Target_Size_Heading[0] + ", " + C10.Target_Size_Heading[2] + ")", C10.Target_Size_Heading[1], C10.Message_Type, "(" + C10.Pos_PolarCoord[0] +
                           ", " + C10.Pos_PolarCoord[1] + ")", "(" + C10.Pos_Cartesian[0] + ", " + C10.Pos_Cartesian[1] + ")", "(" + C10.Track_Vel_Polar[0] + ", " + C10.Track_Vel_Polar[1] +
                           ")", "(" + C10.Track_Vel_Cartesian[0] + ", " + C10.Track_Vel_Cartesian[1] + ")", C10.Track_Status, C10.Fleet_ID, C10.Sys_Status, C10.Pre_Prog_Message,
                           "(" + C10.StndrdDev_Position[0] + ", " + C10.StndrdDev_Position[1] + ")", C10.StndrdDev_Position[2], C10.Presence, "(" + C10.Acceleration[0] + 
                           ", " + C10.Acceleration[1] + ")", "(" + C21.High_Res_Lat_WGS_84 + ", " + C21.High_Res_Lon_WGS_84 + ")", C21.Op_Status, "(" + C21.Air_Speed[0] + ", " + C21.Air_Speed[1] + ")",
                           C21.True_Airspeed, "(" + C21.GS + ", " + C21.TA + ")", C21.TAR, C21.SA, C21.MOPS, C21.MH, C21.BVR, C21.GVR, C21.Met_Report, C21.ECAT, C21.Target_Status,
                           C21.Roll, C21.Service_ID, C21.Quality_Indicators, C21.RID, C21.ToA_Position, C21.ToA_Velocity,
                           C21.TMRP, C21.TMRV, C21.TMRP_HP, C21.TMRV_HP, C21.ToART, C21.Trajectory_Intent, C21.Data_Ages, C21.RP);
                }
                else if (CAT == 21)
                {
                    contadorCAT21++;
                    CAT10 C10 = new CAT10();
                    CAT21 C21 = new CAT21();
                    C21.Decode21(arraystring, q);
                    listaCAT21.Add(C21);

                    // Multiple CAT reduced table for maptrack
                    multiplecattablereducida.Rows.Add(contadorGeneral, C21.getTargetID21(), CheckTOD(C21), C21.getFL21(), C21.getSIC21(), C21.getSAC21(), Math.Round(C21.getLAT21(),2), Math.Round(C21.getLON21(),2), C21.getTargetAddress21(), C21.getTrackNum21());
                    // CAT21 reduced table for maptrack
                    tablacat21reducida.Rows.Add(contadorCAT21, C21.getTargetID21(), CheckTOD(C21), C21.getFL21(), C21.getSIC21(), C21.getSAC21(), Math.Round(C21.getLAT21(),2), Math.Round(C21.getLON21(),2), C21.getTargetAddress21(), C21.getTrackNum21());

                    // Complete CAT21 table
                    tablaCAT21.Rows.Add(contadorCAT21, C21.getSIC21() + "/" + C21.getSAC21(), C21.Target_ID,C21.Track_Num,C21.Target_Report_Desc,
                        CheckTOD(C21), "("+C21.Lat_WGS_84+", "+C21.Lon_WGS_84+")","("+C21.High_Res_Lat_WGS_84+", "+C21.High_Res_Lon_WGS_84+")",C21.FL,C21.GH,
                        C21.Op_Status,"("+C21.Air_Speed[0]+", "+C21.Air_Speed[1]+")",C21.True_Airspeed, "("+C21.GS+", "+C21.TA+")",C21.TAR, C21.SA, C21.MOPS,
                        C21.MH, C21.BVR, C21.GVR, C21.M3AC,C21.Met_Report,C21.ECAT,C21.Target_Address,C21.Target_Status, C21.Roll,C21.Service_ID,
                        C21.Quality_Indicators,C21.Mode_S, C21.MAM,C21.RID,C21.ToA_Position, C21.ToA_Velocity, C21.TMRP,C21.TMRV, 
                        C21.TMRP_HP, C21.TMRV_HP, C21.ToART, C21.Trajectory_Intent, C21.Data_Ages, C21.RP);
                    
                    // Complete Multiple CAT table

                    tablaMultipleCAT.Rows.Add(contadorGeneral, CAT, C21.Data_Source_ID_SAC + "/" + C21.Data_Source_ID_SIC, C21.Target_ID, C21.Track_Num, CheckTOD(C21),
                        C21.Target_Report_Desc, "(" + C21.Lat_WGS_84 + ", " + C21.Lon_WGS_84 + ")", C21.M3AC, C21.Mode_S, C21.FL, C21.GH, C21.Target_Address, C21.MAM,

                        "(" + C10.Target_Size_Heading[0] + ", " + C10.Target_Size_Heading[2] + ")", C10.Target_Size_Heading[1], C10.Message_Type, "(" + C10.Pos_PolarCoord[0] +
                        ", " + C10.Pos_PolarCoord[1] + ")", "(" + C10.Pos_Cartesian[0] + ", " + C10.Pos_Cartesian[1] + ")", "(" + C10.Track_Vel_Polar[0] + ", " + C10.Track_Vel_Polar[1] +
                        ")", "(" + C10.Track_Vel_Cartesian[0] + ", " + C10.Track_Vel_Cartesian[1] + ")", C10.Track_Status, C10.Fleet_ID, C10.Sys_Status, C10.Pre_Prog_Message,
                        "(" + C10.StndrdDev_Position[0] + ", " + C10.StndrdDev_Position[1] + ")", C10.StndrdDev_Position[2], C10.Presence, "(" + C10.Acceleration[0] +
                        ", " + C10.Acceleration[1] + ")", "(" + C21.High_Res_Lat_WGS_84 + ", " + C21.High_Res_Lon_WGS_84 + ")", C21.Op_Status, "(" + C21.Air_Speed[0] + ", " + C21.Air_Speed[1] + ")", 
                        C21.True_Airspeed, "(" + C21.GS + ", " + C21.TA + ")", C21.TAR, C21.SA, C21.MOPS, C21.MH, C21.BVR, C21.GVR, C21.Met_Report, C21.ECAT, C21.Target_Status, C21.Roll, C21.Service_ID,
                        C21.Quality_Indicators, C21.RID, C21.ToA_Position, C21.ToA_Velocity, C21.TMRP, C21.TMRV, C21.TMRP_HP, C21.TMRV_HP, C21.ToART, C21.Trajectory_Intent, C21.Data_Ages, C21.RP);
                }                
            }
        }            
        public void CreateReducedTable()
        {
            //we must declare table columns

            //multiple CAT reduced table
            multiplecattablereducida.Columns.Add(new DataColumn("#"));
            multiplecattablereducida.Columns.Add(new DataColumn("Target ID"));
            multiplecattablereducida.Columns.Add(new DataColumn("Time of Day"));
            multiplecattablereducida.Columns.Add(new DataColumn("Flight Level"));
            multiplecattablereducida.Columns.Add(new DataColumn("SIC"));
            multiplecattablereducida.Columns.Add(new DataColumn("SAC"));
            multiplecattablereducida.Columns.Add(new DataColumn("Latitude (º)"));
            multiplecattablereducida.Columns.Add(new DataColumn("Longitude (º)"));
            multiplecattablereducida.Columns.Add(new DataColumn("Target Address"));
            multiplecattablereducida.Columns.Add(new DataColumn("Track Number"));

            //CAT10 reduced table
            tablacat10reducida.Columns.Add(new DataColumn("#"));
            tablacat10reducida.Columns.Add(new DataColumn("Target ID"));
            tablacat10reducida.Columns.Add(new DataColumn("Time of Day"));
            tablacat10reducida.Columns.Add(new DataColumn("Flight Level"));
            tablacat10reducida.Columns.Add(new DataColumn("SIC"));
            tablacat10reducida.Columns.Add(new DataColumn("SAC"));
            tablacat10reducida.Columns.Add(new DataColumn("Latitude (º)"));
            tablacat10reducida.Columns.Add(new DataColumn("Longitude (º)"));
            tablacat10reducida.Columns.Add(new DataColumn("Target Address"));
            tablacat10reducida.Columns.Add(new DataColumn("Track Number"));

            //CAT21 reduced table
            tablacat21reducida.Columns.Add(new DataColumn("#"));
            tablacat21reducida.Columns.Add(new DataColumn("Target ID"));
            tablacat21reducida.Columns.Add(new DataColumn("Time of Day"));
            tablacat21reducida.Columns.Add(new DataColumn("Flight Level"));
            tablacat21reducida.Columns.Add(new DataColumn("SIC"));
            tablacat21reducida.Columns.Add(new DataColumn("SAC"));
            tablacat21reducida.Columns.Add(new DataColumn("Latitude (º)"));
            tablacat21reducida.Columns.Add(new DataColumn("Longitude (º)"));
            tablacat21reducida.Columns.Add(new DataColumn("Target Address"));
            tablacat21reducida.Columns.Add(new DataColumn("Track Number"));
        }
        public void Create_TrackTable_Puras() //generating each CAT columns for the tables
        {
            //CAT10
            tablaCAT10.Columns.Add(new DataColumn("#"));
            tablaCAT10.Columns.Add(new DataColumn("SIC/SAC"));
            tablaCAT10.Columns.Add(new DataColumn("Target ID"));
            tablaCAT10.Columns.Add(new DataColumn("Track Number"));
            tablaCAT10.Columns.Add(new DataColumn("Target Report")); //array
            tablaCAT10.Columns.Add(new DataColumn("Message type"));
            tablaCAT10.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT10.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT10.Columns.Add(new DataColumn("Position Polar Coords\n(Distance, Angle)"));
            tablaCAT10.Columns.Add(new DataColumn("Position Cartesian Coords\n(X, Y)"));
            tablaCAT10.Columns.Add(new DataColumn("Track Velocity Polar Coord\n(Ground Speed, Track Angle)"));
            tablaCAT10.Columns.Add(new DataColumn("Track Velocity Cartesian Coords\n(Vx, Vy)"));
            tablaCAT10.Columns.Add(new DataColumn("Track Status")); //array
            tablaCAT10.Columns.Add(new DataColumn("Mode 3/A Code")); //array
            tablaCAT10.Columns.Add(new DataColumn("Target Address"));
            tablaCAT10.Columns.Add(new DataColumn("Mode S MB Data")); //array
            tablaCAT10.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT10.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT10.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT10.Columns.Add(new DataColumn("Target Size\n(Length x Width)"));
            tablaCAT10.Columns.Add(new DataColumn("Target Heading"));
            tablaCAT10.Columns.Add(new DataColumn("System Status")); //array
            tablaCAT10.Columns.Add(new DataColumn("Pre Programmed MSG")); //array
            tablaCAT10.Columns.Add(new DataColumn("Standard Deviation of Position\n(X, Y)"));
            tablaCAT10.Columns.Add(new DataColumn("Covariance of deviation"));
            tablaCAT10.Columns.Add(new DataColumn("Presence")); //array
            tablaCAT10.Columns.Add(new DataColumn("Amplitude of Primary Plot"));
            tablaCAT10.Columns.Add(new DataColumn("Acceleration\n(Ax, Ay)"));

            //CAT21
            tablaCAT21.Columns.Add(new DataColumn("#"));
            tablaCAT21.Columns.Add(new DataColumn("SIC/SAC"));
            tablaCAT21.Columns.Add(new DataColumn("Target ID"));
            tablaCAT21.Columns.Add(new DataColumn("Track Number"));
            tablaCAT21.Columns.Add(new DataColumn("Target Report")); // array
            tablaCAT21.Columns.Add(new DataColumn("Time Of Day"));
            tablaCAT21.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT21.Columns.Add(new DataColumn("High Resolution Position WGS-84\n(Latitude, Longitude)"));
            tablaCAT21.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT21.Columns.Add(new DataColumn("Geometric Height"));
            tablaCAT21.Columns.Add(new DataColumn("Operational Status")); //array
            tablaCAT21.Columns.Add(new DataColumn("AirSpeed\n(IAS, Mach)"));
            tablaCAT21.Columns.Add(new DataColumn("True Airspeed"));
            tablaCAT21.Columns.Add(new DataColumn("Airborne Ground Vector\n(Ground Speed, Track Angle)"));
            tablaCAT21.Columns.Add(new DataColumn("Track Angle Rate"));
            tablaCAT21.Columns.Add(new DataColumn("Selected Altitude"));
            tablaCAT21.Columns.Add(new DataColumn("MOPS version")); //array
            tablaCAT21.Columns.Add(new DataColumn("Magnetic Heading"));
            tablaCAT21.Columns.Add(new DataColumn("Barometric Vertical Rate"));
            tablaCAT21.Columns.Add(new DataColumn("Geometric Vertical Rate"));
            tablaCAT21.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT21.Columns.Add(new DataColumn("Met Report")); //array
            tablaCAT21.Columns.Add(new DataColumn("Emitter Category"));
            tablaCAT21.Columns.Add(new DataColumn("Target Address"));
            tablaCAT21.Columns.Add(new DataColumn("Target Status")); // array
            tablaCAT21.Columns.Add(new DataColumn("Roll Angle"));
            tablaCAT21.Columns.Add(new DataColumn("Service Identification"));
            tablaCAT21.Columns.Add(new DataColumn("Quality Indicators")); //array
            tablaCAT21.Columns.Add(new DataColumn("Mode S")); //array
            tablaCAT21.Columns.Add(new DataColumn("Message amplitude"));
            tablaCAT21.Columns.Add(new DataColumn("Receiver ID"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Applicability\nfor position"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Applicability\nfor velocity"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor position"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor position - High precision")); // array
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity - High precision")); // array
            tablaCAT21.Columns.Add(new DataColumn("Time of ASTERIX\nReport Transmission"));
            tablaCAT21.Columns.Add(new DataColumn("Trajectory Intent Data")); // array
            tablaCAT21.Columns.Add(new DataColumn("Data ages")); // array
            tablaCAT21.Columns.Add(new DataColumn("Service Management"));
        }
        public void Create_TrackTable_Multiple() //generating each CAT columns for the tables
        {
            // MULTIPLE CAT
            //Common Items
            tablaMultipleCAT.Columns.Add(new DataColumn("#"));
            tablaMultipleCAT.Columns.Add(new DataColumn("CAT"));
            tablaMultipleCAT.Columns.Add(new DataColumn("SIC/SAC"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Target ID"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Track Number"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Target Report"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Flight Level"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Measured Height"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Target Address"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Amplitude"));

            //CAT10 Items
            tablaMultipleCAT.Columns.Add(new DataColumn("Target Size\n(Length x Width)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Target Heading"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Message type"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Position Polar Coords\n(Distance, Angle)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Position Cartesian Coords\n(X, Y)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Track Velocity Polar Coord\n(Ground Speed, Track Angle)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Track Velocity Cartesian Coords\n(Vx, Vy)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Track Status"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaMultipleCAT.Columns.Add(new DataColumn("System Status"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Standard Deviation of Position\n(X, Y)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Covariance of deviation"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Presence"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Acceleration\n(Ax, Ay)"));

            // CAT21 Items
            tablaMultipleCAT.Columns.Add(new DataColumn("High Resolution Position WGS-84\n(Latitude, Longitude)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Operational Status"));
            tablaMultipleCAT.Columns.Add(new DataColumn("AirSpeed\n(IAS, Mach)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("True Airspeed"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Airborne Ground Vector\n(Ground Speed, Track Angle)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Track Angle Rate"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Selected Altitude"));
            tablaMultipleCAT.Columns.Add(new DataColumn("MOPS version"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Magnetic Heading"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Barometric Vertical Rate"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Geometric Vertical Rate"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Met Report"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Emitter Category"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Target Status"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Roll Angle"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Service Identification"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Quality Indicators"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Receiver ID"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Time of Applicability\nfor position"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Time of Applicability\nfor velocity"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Time of Message Reception\nfor position"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Time of Message Reception\nfor position - High precision"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity - High precision"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Time of ASTERIX\nReport Transmission"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Trajectory Intent Data"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Data ages"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Service Management"));
        }
       
        // returns the pack in the 'num' position
        public CAT10 getCAT10(int num)
        {
            return listaCAT10[num];
        }
        public CAT21 getCAT21(int num)
        {
            return listaCAT21[num];
        }

        //to show lat and lon from cat10 files (convert from cartesian). These formulas were given in NACC lectures.
        private double cartesiantolatmlat(double X, double Y)
        {
            double MLAT_lat = 41.29694444;
            double MLAT_lon = 2.07833333;
            CAT10 C10 = new CAT10();
            double R = 6371 * 1000;
            double d = Math.Sqrt((X * X) + (Y * Y));
            double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
            double phi1 = MLAT_lat * (Math.PI / 180);
            double lamda1 = MLAT_lon * (Math.PI / 180);
            double phi2 = Math.Asin(Math.Sin(phi1) * Math.Cos(d / R) + Math.Cos(phi1) * Math.Sin(d / R) * Math.Cos(brng));
            return phi2 * (180.0/Math.PI);

        }
        private double cartesiantolonmlat(double X, double Y)
        {
            double MLAT_lat = 41.29694444;
            double MLAT_lon = 2.07833333;
            CAT10 C10 = new CAT10();
            double R = 6371 * 1000;
            double d = Math.Sqrt((X * X) + (Y * Y));
            double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
            double phi1 = MLAT_lat * (Math.PI / 180);
            double lamda1 = MLAT_lon * (Math.PI / 180);
            var phi2 = Math.Asin(Math.Sin(phi1) * Math.Cos(d / R) + Math.Cos(phi1) * Math.Sin(d / R) * Math.Cos(brng));
            double lamda2 = lamda1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(phi1), Math.Cos(d / R) - Math.Sin(phi1) * Math.Sin(phi2));
            return lamda2 * (180.0 / Math.PI);
        }
        // if time of asterix message report is not reported we will show Time of message reception for position
        
        private double CheckTOD(CAT21 C21)
        {
            double x = double.NaN;
            if (C21.getTOD21() == x) { return C21.getTOD21ADSB23(); }
            else { return C21.getTOD21(); }
        }
    }
}