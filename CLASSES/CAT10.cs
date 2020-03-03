using System;
using System.Collections.Generic;
using System.Text;

namespace CLASSES
{
    public class CAT10
    {
        Metodos M = new Metodos();

        // Definir los Data Items como variables, para procesar las que están en el paquete (las que son 1)
        // El tipo de cada variable depende de la precisión con la que se nos proporciona (especificado pdf CAT10)

        string Message_Type;
        string Data_Source_ID;
        string Target_Rep_Descript;
        float[] Pos_PolarCoord; //rho,theta
        float[] Pos_WGS84;
        float[] Pos_Cartesian;
        string Mode3A_Code;
        string FL_Binary;
        float Height;   //ft
        float Amplitude;    //dBm
        float Time_Day; //seconds
        Int16 Track_Num;
        string Track_Status;
        float[] Track_Vel_Polar;    // (NM/s, degrees)
        float[] Track_Vel_Cartesian;    // m/s
        float Acceleration; // m/s^2
        string Target_Add;
        string Target_ID;
        string Mode_SMB;
        float[] Target_Size_Heading;    // m,degrees
        float[] Presence;   // rho,theta
        string Fleet_ID;
        string Pre_Prog_Message;
        float StndrdDev_Position; // m^2
        string Sys_Status;

        public void Decode10(string[] paquete)
        {
            Metodos M = new Metodos();
            int longitud = M.Longitud_Paquete(paquete);
            string[] paquete0 = new string[longitud];
            for (int i = 0; i < longitud; i++)
            {
                paquete0[i] = M.Poner_Zeros_Delante(paquete[i]);
            }
            List<string> FSPEC =  new List<string>(M.FSPEC(paquete0));
            
            // Posicion del vector paquete0 donde empieza la info despues del FSPEC
            int contador = Convert.ToInt32(FSPEC[FSPEC.Count -1]);
            
            if (FSPEC[0]=="1")
            {
                // Item I010/010: Data source ID                
            }
            if (FSPEC[1]=="1")
            {
                // Item I010/000: Message Type
            }
            if (FSPEC[2] == "1")
            {
                // Item I010/020: Target Report Descriptor
            }
            if (FSPEC[3] == "1")
            {
                // Item I010/140: Time of Day
            }
            if (FSPEC[4] == "1")
            {
                // Item I010/041: Position WGS-84
            }
            if (FSPEC[5] == "1")
            {
                // Item I010/040: Position Polar
            }
            if (FSPEC[6] == "1")
            {
                // Item I010/042: Position Cartesian
            }
            if (FSPEC[7] == "1")
            {
                // Item I010/200: Track Velocity Polar
            }
            if (FSPEC[8] == "1")
            {
                // Item I010/202: Track Velocity Cartesian
            }
            if (FSPEC[9] == "1")
            {
                // Item I010/161: Track Number
            }
            if (FSPEC[10] == "1")
            {
                // Item I010/170: Track Status
            }
            if (FSPEC[11] == "1")
            {
                // Item I010/060: Mode-3/A in Octal
            }
            if (FSPEC[12] == "1")
            {
                // Item I010/220: Target Address
            }
            if (FSPEC[13] == "1")
            {
                // Item I010/245: Target Identification
            }
            if (FSPEC[14] == "1")
            {
                // Item I010/250: Mode S MB Data
            }
            if (FSPEC[15] == "1")
            {
                // Item I010/300: Vehicle Fleet ID
            }
            if (FSPEC[16] == "1")
            {
                // Item I010/090: Flight Level in Binary
            }
            if (FSPEC[17] == "1")
            {
                // Item I010/091: Measured High
            }
            if (FSPEC[18] == "1")
            {
                // Item I010/270: Target Size and Orientation
            }
            if (FSPEC[19] == "1")
            {
                // Item I010/550: System Status
            }
            if (FSPEC[20] == "1")
            {
                // Item I010/310: Pre-programmed Message
            }
            if (FSPEC[21] == "1")
            {
                // Item I010/500: Standard Deviation of Position
            }
            if (FSPEC[22] == "1")
            {
                // Item I010/280: Presence
            }
            if (FSPEC[23] == "1")
            {
                // Item I010/131: Amplitude of Primary Plot
            }
            if (FSPEC[24] == "1")
            {
                // Item I010/210: Calculated Acceleration
            }

            // FSPEC[25] no tiene Data Items, es de "spare"
            
            if (FSPEC[26] == "1")
            {
                // Item I010/040: Position Polar
            }
            if (FSPEC[27] == "1")
            {
                // SP: Special Purpose Field
            }
            if (FSPEC[28] == "1")
            {
                // RE: Reserved Expansion Field
            }
        }
    }
}
