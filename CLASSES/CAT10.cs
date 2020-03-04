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
            
            if (FSPEC[0] == "1")
            {
                // Item I010/010: Data source ID 
                string SAC_Bin = M.Octeto_A_Bin(paquete0[contador]);                
                string SIC_Bin = M.Octeto_A_Bin(paquete0[contador + 1]);

                string SAC = (Convert.ToInt32(SAC_Bin, 2)).ToString();
                string SIC = (Convert.ToInt32(SAC_Bin, 2)).ToString();
                Data_Source_ID = SIC + "/" + SAC;

                contador = contador + 2;
            }
            if (FSPEC[1] == "1")
            {
                // Item I010/000: Message Type
                string MessType_Bin = M.Octeto_A_Bin(paquete[contador]);
                Int32 MessType = Convert.ToInt32(MessType_Bin, 2);
                if (MessType == 1)
                {
                    Message_Type = "Target Report";
                }
                if (MessType == 2)
                {
                    Message_Type = "Start of Update Cycle";
                }
                if (MessType == 3)
                {
                    Message_Type = "Periodic Status Message";
                }
                if (MessType == 4)
                {
                    Message_Type = "Event-triggered Status Message";
                }
                else { Console.WriteLine("ERROR: Wrong Message Type Format"); }
                contador = contador + 1;
            }
            if (FSPEC[2] == "1")
            {
                // Item I010/020: Target Report Descriptor
                string TargetReport_Bin = M.Octeto_A_Bin(paquete0[contador]);
                char[] Target_Bits = TargetReport_Bin.ToCharArray();
                
                string TYP_Bin = (Target_Bits[0] + Target_Bits[1] + Target_Bits[2]).ToString();
                Int32 TYP = Convert.ToInt32(TYP_Bin, 2);                
                if (TYP == 0) { Target_Rep_Descript = "SSR Multilateration"; }
                if (TYP == 1) { Target_Rep_Descript = "Mode S Multilateration"; }
                if (TYP == 2) { Target_Rep_Descript = "ADS-B"; }
                if (TYP == 3) { Target_Rep_Descript = "PSR"; }
                if (TYP == 4) { Target_Rep_Descript = "Magnetic Loop System"; }
                if (TYP == 5) { Target_Rep_Descript = "HF Multilateration"; }
                if (TYP == 6) { Target_Rep_Descript = "Not defined"; }
                if (TYP == 7) { Target_Rep_Descript = "Other types"; }

                if (Target_Bits[3] == 1) { Target_Rep_Descript = Target_Rep_Descript + "/Differential Correction (ADS-B)"; }
                if (Target_Bits[3] == 0) { Target_Rep_Descript = Target_Rep_Descript + "/No Differential Correction (ADS-B)"; }

                if (Target_Bits[4] == 1) { Target_Rep_Descript = Target_Rep_Descript + "/Chain 2"; }
                if (Target_Bits[4] == 0) { Target_Rep_Descript = Target_Rep_Descript + "/Chain 1"; }

                if (Target_Bits[5] == 1) { Target_Rep_Descript = Target_Rep_Descript + "/Transponder Ground bit set"; }
                if (Target_Bits[5] == 0) { Target_Rep_Descript = Target_Rep_Descript + "/Transponder Ground bit not set"; }

                if (Target_Bits[6] == 1) { Target_Rep_Descript = Target_Rep_Descript + "/Corrupted reply in multilateration"; }
                if (Target_Bits[6] == 0) { Target_Rep_Descript = Target_Rep_Descript + "/No Corrupted replies in multilateration"; }

                if (Target_Bits[7] == 1)
                {
                    contador = contador + 1;
                    // First extent
                    string TargetReport_Bin1 = M.Octeto_A_Bin(paquete0[contador]);
                    char[] Target_Bits1 = TargetReport_Bin.ToCharArray();

                    if (Target_Bits1[0] == 1) { Target_Rep_Descript = Target_Rep_Descript + "/Simulated Target Report"; }
                    if (Target_Bits1[0] == 0) { Target_Rep_Descript = Target_Rep_Descript + "/Actual Target Report"; }

                    if (Target_Bits1[1] == 1) { Target_Rep_Descript = Target_Rep_Descript + "/Test Target"; }
                    if (Target_Bits1[1] == 0) { Target_Rep_Descript = Target_Rep_Descript + "/Default"; }

                    if (Target_Bits1[2] == 1) { Target_Rep_Descript = Target_Rep_Descript + "/ Report from field monitor(fixed transponder)"; }
                    if (Target_Bits1[2] == 0) { Target_Rep_Descript = Target_Rep_Descript + "/ Report from target transponder"; }

                    string RAB_Bin = (Target_Bits1[3] + Target_Bits1[4]).ToString();
                    Int32 RAB = Convert.ToInt32(RAB_Bin, 2);

                    // aqui!!!!!!
                }   
                if (Target_Bits[7] == 0)
                {
                    contador = contador + 1;
                }
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