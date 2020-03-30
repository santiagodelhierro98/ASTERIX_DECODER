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

        List<CAT10> listaCAT10 = new List<CAT10>();
        //List<CAT20> listaCAT20 = new List<CAT20>();
        List<CAT21> listaCAT21 = new List<CAT21>();
        public int CAT;
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

            this.Create_TrackTable();

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
        //public DataTable getTablaCAT20()
        //{
        //    return tablaCAT20;
        //}
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
                contadorGeneral++;

                if (CAT == 10)
                {
                    contadorCAT10++;
                    CAT10 C10 = new CAT10();
                    C10.Decode10(arraystring);
                    listaCAT10.Add(C10);

                    // Multiple CAT reduced table for maptrack
                    multiplecattablereducida.Rows.Add(contadorGeneral, C10.getTargetID10(), C10.getTOD10(), C10.getSIC10(), C10.getSAC10(), C10.getLAT10(), C10.getLON10(), C10.getTargetAddress10(), C10.getTrackNum10());
                    // CAT10 reduced table for maptrack
                    tablacat10reducida.Rows.Add(contadorCAT10, C10.getTargetID10(), C10.getTOD10(), C10.getSIC10(), C10.getSAC10(), C10.getLAT10(), C10.getLON10(), C10.getTargetAddress10(), C10.getTrackNum10());
                    // Complete CAT10 table
                    tablaCAT10.Rows.Add(contadorCAT10, C10.Data_Source_ID[1]+"/"+C10.Data_Source_ID[0], C10.Target_ID, C10.Track_Num, C10.Target_Rep_Descript,
                           C10.Message_Type, C10.Time_Day,"("+C10.Pos_WGS84[0] + ", " + C10.Pos_WGS84[1]+")","("+C10.Pos_PolarCoord[0] + ", " + C10.Pos_PolarCoord[1]+")",
                           "("+C10.Pos_Cartesian[0] + ", " + C10.Pos_Cartesian[1]+")","("+C10.Track_Vel_Polar[0] + ", " + C10.Track_Vel_Polar[1]+")","("+C10.Track_Vel_Cartesian[0] +
                           ", " + C10.Track_Vel_Cartesian[1]+")", C10.Track_Status, C10.Mode3A_Code, C10.Target_Add, C10.Mode_SMB, C10.Fleet_ID, C10.FL[2], C10.Height,
                           "("+C10.Target_Size_Heading[0] + ", " + C10.Target_Size_Heading[2]+")", C10.Target_Size_Heading[1], C10.Sys_Status, C10.Pre_Prog_Message,
                           "("+C10.StndrdDev_Position[0] + ", " + C10.StndrdDev_Position[1]+")", C10.StndrdDev_Position[2], C10.Presence, C10.Amplitude, "("+C10.Acceleration[0] +
                           ", " + C10.Acceleration[1] + ")");
                    // Complete Multiple CAT table
                    tablaMultipleCAT.Rows.Add(contadorGeneral, C10.Data_Source_ID[1] + "/" + C10.Data_Source_ID[0], C10.Target_ID, C10.Track_Num, C10.Target_Rep_Descript,
                           C10.Message_Type, C10.Time_Day, "(" + C10.Pos_WGS84[0] + ", " + C10.Pos_WGS84[1] + ")", "(" + C10.Pos_PolarCoord[0] + ", " + C10.Pos_PolarCoord[1] + ")",
                           "(" + C10.Pos_Cartesian[0] + ", " + C10.Pos_Cartesian[1] + ")", "(" + C10.Track_Vel_Polar[0] + ", " + C10.Track_Vel_Polar[1] + ")", "(" + C10.Track_Vel_Cartesian[0] +
                           ", " + C10.Track_Vel_Cartesian[1] + ")", C10.Track_Status, C10.Mode3A_Code, C10.Target_Add, C10.Mode_SMB, C10.Fleet_ID, C10.FL[2], C10.Height,
                           "(" + C10.Target_Size_Heading[0] + ", " + C10.Target_Size_Heading[2] + ")", C10.Target_Size_Heading[1], C10.Sys_Status, C10.Pre_Prog_Message,
                           "(" + C10.StndrdDev_Position[0] + ", " + C10.StndrdDev_Position[1] + ")", C10.StndrdDev_Position[2], C10.Presence, C10.Amplitude, "(" + C10.Acceleration[0] +
                           ", " + C10.Acceleration[1] + ")");
                }
                else if (CAT == 21)
                {
                    contadorCAT21++;
                    CAT21 C21 = new CAT21();
                    C21.Decode21(arraystring, q);
                    listaCAT21.Add(C21);

                    // Multiple CAT reduced table for maptrack
                    multiplecattablereducida.Rows.Add(contadorGeneral, C21.getTargetID21(), C21.getTOD21(), C21.getSIC21(), C21.getSAC21(), Math.Round(C21.getLAT21(),2), Math.Round(C21.getLON21(),2), C21.getTargetAddress21(), C21.getTrackNum21());
                    // CAT21 reduced table for maptrack
                    tablacat21reducida.Rows.Add(contadorCAT21, C21.getTargetID21(), C21.getTOD21(), C21.getSIC21(), C21.getSAC21(), Math.Round(C21.getLAT21()), Math.Round(C21.getLON21()), C21.getTargetAddress21(), C21.getTrackNum21());
                    // Complete CAT21 table
                    tablaCAT21.Rows.Add(contadorCAT21, C21.Data_Source_ID_SAC+"/"+C21.Data_Source_ID_SIC,C21.Target_ID,C21.Track_Num,C21.Target_Report_Desc,
                        C21.getTOD21(),"("+C21.Lat_WGS_84+", "+C21.Lon_WGS_84+")","("+C21.High_Res_Lat_WGS_84+", "+C21.High_Res_Lon_WGS_84+")",C21.FL,C21.GH,
                        C21.Op_Status,"("+C21.Air_Speed[0]+", "+C21.Air_Speed[1]+")",C21.True_Airspeed, "("+C21.GS+", "+C21.TA+")",C21.TAR, C21.SA, C21.MOPS,
                        C21.MH, C21.BVR, C21.GVR, C21.M3AC,C21.Met_Report,C21.ECAT,C21.Target_Address,C21.Target_Status,"no lo he encontrado",C21.Roll,C21.Service_ID,
                        C21.Quality_Indicators,C21.Mode_S,"no lo he encontrado","tampoco", C21.MAM,C21.RID,C21.ToA_Position, C21.ToA_Velocity, C21.TMRP,C21.TMRV, 
                        C21.TMRP_HP, C21.TMRV_HP, C21.ToART, C21.Trajectory_Intent, "tampoco", C21.Data_Ages, C21.RP);
                    // Complete Multiple CAT table
                    tablaMultipleCAT.Rows.Add(contadorGeneral, C21.Data_Source_ID_SAC + "/" + C21.Data_Source_ID_SIC, C21.Target_ID, C21.Track_Num, C21.Target_Report_Desc,
                        C21.getTOD21(), "(" + C21.Lat_WGS_84 + ", " + C21.Lon_WGS_84 + ")", "(" + C21.High_Res_Lat_WGS_84 + ", " + C21.High_Res_Lon_WGS_84 + ")", C21.FL, C21.GH,
                        C21.Op_Status, "(" + C21.Air_Speed[0] + ", " + C21.Air_Speed[1] + ")", C21.True_Airspeed, "(" + C21.GS + ", " + C21.TA + ")", C21.TAR, C21.SA, C21.MOPS,
                        C21.MH, C21.BVR, C21.GVR, C21.M3AC, C21.Met_Report, C21.ECAT, C21.Target_Address, C21.Target_Status, "no lo he encontrado", C21.Roll, C21.Service_ID,
                        C21.Quality_Indicators, C21.Mode_S, "no lo he encontrado", "tampoco", C21.MAM, C21.RID, C21.ToA_Position, C21.ToA_Velocity, C21.TMRP, C21.TMRV,
                        C21.TMRP_HP, C21.TMRV_HP, C21.ToART, C21.Trajectory_Intent, "tampoco", C21.Data_Ages, C21.RP);
                }                
            }
        }            
        public bool Checkifmulticatfile(DataTable datatable1, DataTable datatable2)
        {
            //we compare if tablacat10 or tablacat21 is equal to multiplecattable. if they are not equal, file would be multicategory
           // true means file is multycategory 
            if (datatable1.Rows[1000][1] == datatable2.Rows[1000][1]) { return false; }
            else { return true; }         
        }
        public void CreateReducedTable()
        {
            //we must declare table columns

            //multiple CAT reduced table
            multiplecattablereducida.Columns.Add(new DataColumn("#"));
            multiplecattablereducida.Columns.Add(new DataColumn("Target ID"));
            multiplecattablereducida.Columns.Add(new DataColumn("Time of Day"));
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
            tablacat21reducida.Columns.Add(new DataColumn("SIC"));
            tablacat21reducida.Columns.Add(new DataColumn("SAC"));
            tablacat21reducida.Columns.Add(new DataColumn("Latitude (º)"));
            tablacat21reducida.Columns.Add(new DataColumn("Longitude (º)"));
            tablacat21reducida.Columns.Add(new DataColumn("Target Address"));
            tablacat21reducida.Columns.Add(new DataColumn("Track Number"));
        }     
        public void Create_TrackTable() //generating each CAT columns for the tables
        {
            //CAT10
            tablaCAT10.Columns.Add(new DataColumn("#"));
            tablaCAT10.Columns.Add(new DataColumn("SIC/SAC"));
            tablaCAT10.Columns.Add(new DataColumn("Target ID"));
            tablaCAT10.Columns.Add(new DataColumn("Track Number"));
            tablaCAT10.Columns.Add(new DataColumn("Data Type"));
            tablaCAT10.Columns.Add(new DataColumn("Message type"));
            tablaCAT10.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT10.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT10.Columns.Add(new DataColumn("Position Polar Coords\n(Distance, Angle)"));
            tablaCAT10.Columns.Add(new DataColumn("Position Cartesian Coords\n(X, Y)"));
            tablaCAT10.Columns.Add(new DataColumn("Track Velocity Polar Coord\n(Ground Speed, Track Angle)"));
            tablaCAT10.Columns.Add(new DataColumn("Track Velocity Cartesian Coords\n(Vx, Vy)"));
            tablaCAT10.Columns.Add(new DataColumn("Track Status"));
            tablaCAT10.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT10.Columns.Add(new DataColumn("Target Address"));
            tablaCAT10.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT10.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT10.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT10.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT10.Columns.Add(new DataColumn("Target Size\n(Length x Width)"));
            tablaCAT10.Columns.Add(new DataColumn("Target Heading"));
            tablaCAT10.Columns.Add(new DataColumn("System Status"));
            tablaCAT10.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT10.Columns.Add(new DataColumn("Standard Deviation of Position\n(X, Y)"));
            tablaCAT10.Columns.Add(new DataColumn("Covariance of deviation"));
            tablaCAT10.Columns.Add(new DataColumn("Presence"));
            tablaCAT10.Columns.Add(new DataColumn("Amplitude of Primary Plot"));
            tablaCAT10.Columns.Add(new DataColumn("Calculated Acceleration\n(Ax, Ay)"));

            //CAT21
            tablaCAT21.Columns.Add(new DataColumn("#"));
            tablaCAT21.Columns.Add(new DataColumn("SAC/SIC"));
            tablaCAT21.Columns.Add(new DataColumn("Target ID"));
            tablaCAT21.Columns.Add(new DataColumn("Track Number"));
            tablaCAT21.Columns.Add(new DataColumn("Target Report"));
            tablaCAT21.Columns.Add(new DataColumn("Time Of Day"));
            tablaCAT21.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT21.Columns.Add(new DataColumn("High Resolution Position WGS-84\n(Latitude, Longitude)"));
            tablaCAT21.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT21.Columns.Add(new DataColumn("Geometric Height"));
            tablaCAT21.Columns.Add(new DataColumn("Operational Status"));
            tablaCAT21.Columns.Add(new DataColumn("AirSpeed\n(IAS, Mach)"));
            tablaCAT21.Columns.Add(new DataColumn("True Airspeed"));
            tablaCAT21.Columns.Add(new DataColumn("Airborne Ground Vector\n(Ground Speed, Track Angle)"));
            tablaCAT21.Columns.Add(new DataColumn("Track Angle Rate"));
            tablaCAT21.Columns.Add(new DataColumn("Intermediate Stage\nSelected Altitude"));
            tablaCAT21.Columns.Add(new DataColumn("Final Stage\nSelected Altitude"));
            tablaCAT21.Columns.Add(new DataColumn("MOPS version"));
            tablaCAT21.Columns.Add(new DataColumn("Magnetic Heading"));
            tablaCAT21.Columns.Add(new DataColumn("Barometric Vertical Rate"));
            tablaCAT21.Columns.Add(new DataColumn("Geometric Vertical Rate"));
            tablaCAT21.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT21.Columns.Add(new DataColumn("Met Report"));
            tablaCAT21.Columns.Add(new DataColumn("Emitter Category"));
            tablaCAT21.Columns.Add(new DataColumn("Target Address"));
            tablaCAT21.Columns.Add(new DataColumn("Target Status"));
            tablaCAT21.Columns.Add(new DataColumn("Rate of Angle"));
            tablaCAT21.Columns.Add(new DataColumn("Roll Angle"));
            tablaCAT21.Columns.Add(new DataColumn("Service Identification"));
            tablaCAT21.Columns.Add(new DataColumn("Quality Indicators"));
            tablaCAT21.Columns.Add(new DataColumn("Mode S"));
            tablaCAT21.Columns.Add(new DataColumn("Link Technology"));
            tablaCAT21.Columns.Add(new DataColumn("Report period"));
            tablaCAT21.Columns.Add(new DataColumn("Message amplitude"));
            tablaCAT21.Columns.Add(new DataColumn("Receiver ID"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Applicability\nfor position"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Applicability\nfor velocity"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor position"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor position - High precision"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity - High precision"));
            tablaCAT21.Columns.Add(new DataColumn("Time of ASTERIX\nReport Transmission"));
            tablaCAT21.Columns.Add(new DataColumn("Trajectory Intent Data"));
            tablaCAT21.Columns.Add(new DataColumn("Figure of merit"));
            tablaCAT21.Columns.Add(new DataColumn("Data ages"));
            tablaCAT21.Columns.Add(new DataColumn("Service Management"));

            // Multiple CAT
            tablaMultipleCAT.Columns.Add(new DataColumn("#"));
            tablaMultipleCAT.Columns.Add(new DataColumn("SIC/SAC"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Target ID"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Track Number"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Data Type"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Message type"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Position Polar Coords\n(Distance, Angle)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Position Cartesian Coords\n(X, Y)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Track Velocity Polar Coord\n(Ground Speed, Track Angle)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Track Velocity Cartesian Coords\n(Vx, Vy)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Track Status"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Target Address"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Flight Level"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Measured Height"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Target Size\n(Length x Width)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Target Heading"));
            tablaMultipleCAT.Columns.Add(new DataColumn("System Status"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Standard Deviation of Position\n(X, Y)"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Covariance of deviation"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Presence"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Amplitude of Primary Plot"));
            tablaMultipleCAT.Columns.Add(new DataColumn("Calculated Acceleration\n(Ax, Ay)"));
        }
    }
}