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
        double[] Pos_PolarCoord; //rho,theta
        double[] Pos_WGS84;
        double[] Pos_Cartesian;
        string Mode3A_Code;
        string FL_Binary;
        double Height;   //ft
        double Amplitude;    //dBm
        double Time_Day; //seconds
        int Track_Num;
        string Track_Status;
        double[] Track_Vel_Polar;    // (NM/s, degrees)
        double[] Track_Vel_Cartesian;    // m/s
        double Acceleration; // m/s^2
        int Target_Add;
        string Target_ID;
        string Mode_SMB;
        double[] Target_Size_Heading;    // m,degrees
        double[] Presence;   // rho,theta
        string Fleet_ID;
        string Pre_Prog_Message;
        double StndrdDev_Position; // m^2
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
            List<string> FSPEC = new List<string>(M.FSPEC(paquete0));

            // Posicion del vector paquete0 donde empieza la info despues del FSPEC
            int contador = Convert.ToInt32(FSPEC[FSPEC.Count - 1]) + 1;

            if (FSPEC[0] == "1")
            {
                // Item I010/010: Data source ID 
                string SAC_Bin = M.Octeto_A_Bin(paquete0[contador]);
                string SIC_Bin = M.Octeto_A_Bin(paquete0[contador + 1]);

                string SAC = (Convert.ToInt32(SAC_Bin, 2)).ToString();
                string SIC = (Convert.ToInt32(SIC_Bin, 2)).ToString();
                Data_Source_ID = "SIC: " + SIC + "/SAC: " + SAC;

                contador += 2;
            }
            if (FSPEC[1] == "1")
            {
                // Item I010/000: Message Type
                string MessType_Bin = M.Octeto_A_Bin(paquete0[contador]);
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
                contador += 1;
            }
            if (FSPEC[2] == "1")
            {
                // Item I010/020: Target Report Descriptor
                string TargetReport_Bin = M.Octeto_A_Bin(paquete0[contador]);
                char[] Target_Bits = TargetReport_Bin.ToCharArray();

                string TYP_Bin = Target_Bits[0].ToString() + Target_Bits[1].ToString() + Target_Bits[2].ToString();
                Int32 TYP = Convert.ToInt32(TYP_Bin, 2);
                if (TYP == 0) { Target_Rep_Descript = "TYP: SSR Multilateration"; }
                if (TYP == 1) { Target_Rep_Descript = "TYP: Mode S Multilateration"; }
                if (TYP == 2) { Target_Rep_Descript = "TYP: ADS-B"; }
                if (TYP == 3) { Target_Rep_Descript = "TYP: PSR"; }
                if (TYP == 4) { Target_Rep_Descript = "TYP: Magnetic Loop System"; }
                if (TYP == 5) { Target_Rep_Descript = "TYP: HF Multilateration"; }
                if (TYP == 6) { Target_Rep_Descript = "TYP: Not defined"; }
                if (TYP == 7) { Target_Rep_Descript = "TYP: Other types"; }

                if (Target_Bits[3].ToString() == "1") { Target_Rep_Descript = Target_Rep_Descript + "/DCR: Differential Correction (ADS-B)"; }
                if (Target_Bits[3].ToString() == "0") { Target_Rep_Descript = Target_Rep_Descript + "/DCR: No Differential Correction (ADS-B)"; }

                if (Target_Bits[4].ToString() == "1") { Target_Rep_Descript = Target_Rep_Descript + "/CHN: Chain 2"; }
                if (Target_Bits[4].ToString() == "0") { Target_Rep_Descript = Target_Rep_Descript + "/CHN: Chain 1"; }

                if (Target_Bits[5].ToString() == "1") { Target_Rep_Descript = Target_Rep_Descript + "/GBS: Transponder Ground bit set"; }
                if (Target_Bits[5].ToString() == "0") { Target_Rep_Descript = Target_Rep_Descript + "/GBS: Transponder Ground bit not set"; }

                if (Target_Bits[6].ToString() == "1") { Target_Rep_Descript = Target_Rep_Descript + "/CRT: Corrupted reply in multilateration"; }
                if (Target_Bits[6].ToString() == "0") { Target_Rep_Descript = Target_Rep_Descript + "/CRT: No Corrupted replies in multilateration"; }

                if (Target_Bits[7].ToString() == "1")
                {
                    contador += 1;
                    // First extent
                    string TargetReport_Bin1 = M.Octeto_A_Bin(paquete0[contador]);
                    char[] Target_Bits1 = TargetReport_Bin1.ToCharArray();

                    if (Target_Bits1[0].ToString() == "1") { Target_Rep_Descript = Target_Rep_Descript + "/SIM: Simulated Target Report"; }
                    if (Target_Bits1[0].ToString() == "0") { Target_Rep_Descript = Target_Rep_Descript + "/SIM: Actual Target Report"; }

                    if (Target_Bits1[1].ToString() == "1") { Target_Rep_Descript = Target_Rep_Descript + "/TST: Test Target"; }
                    if (Target_Bits1[1].ToString() == "0") { Target_Rep_Descript = Target_Rep_Descript + "/TST: Default"; }

                    if (Target_Bits1[2].ToString() == "1") { Target_Rep_Descript = Target_Rep_Descript + "/RAB: Report from field monitor(fixed transponder)"; }
                    if (Target_Bits1[2].ToString() == "0") { Target_Rep_Descript = Target_Rep_Descript + "/RAB: Report from target transponder"; }

                    string LOP_Bin = Target_Bits1[3].ToString() + Target_Bits1[4].ToString();
                    Int32 LOP = Convert.ToInt32(LOP_Bin, 2);
                    if (LOP == 0) { Target_Rep_Descript = Target_Rep_Descript + "/LOP: Undetermined"; }
                    if (LOP == 1) { Target_Rep_Descript = Target_Rep_Descript + "/LOP: Loop Start"; }
                    if (LOP == 2) { Target_Rep_Descript = Target_Rep_Descript + "/LOP: Loop Finish"; }

                    string TOT_Bin = Target_Bits1[5].ToString() + Target_Bits1[6].ToString();
                    Int32 TOT = Convert.ToInt32(TOT_Bin, 2);
                    if (TOT == 0) { Target_Rep_Descript = Target_Rep_Descript + "/TOT: Undetermined"; }
                    if (TOT == 1) { Target_Rep_Descript = Target_Rep_Descript + "/TOT: Aircraft"; }
                    if (TOT == 2) { Target_Rep_Descript = Target_Rep_Descript + "/TOT: Ground Vehicle"; }
                    if (TOT == 3) { Target_Rep_Descript = Target_Rep_Descript + "/TOT: Helicopter"; }

                    if (Target_Bits1[7] == 1)
                    {
                        contador += 1;
                        // Second Extent
                        string TargetReport_Bin2 = M.Octeto_A_Bin(paquete0[contador]);
                        char[] Target_Bits2 = TargetReport_Bin2.ToCharArray();

                        if (Target_Bits2[0].ToString() == "1") { Target_Rep_Descript = Target_Rep_Descript + "/SPI: Special Position Identification"; }
                        if (Target_Bits2[0].ToString() == "0") { Target_Rep_Descript = Target_Rep_Descript + "/SPI: Abcense of SPI"; }

                        // los siguientes 6 bits deberian ser 0s
                        string Spare_Bin = Target_Bits2[1].ToString() + Target_Bits2[2].ToString() + Target_Bits2[3].ToString() + Target_Bits2[4].ToString() + Target_Bits2[5].ToString() + Target_Bits2[6].ToString();
                        Int32 Spare = Convert.ToInt32(Spare_Bin, 2);
                    }
                    if (Target_Bits1[7].ToString() == "0")
                    {
                        contador += 1;
                    }
                }
                if (Target_Bits[7].ToString() == "0")
                {
                    contador += 1;
                }
            }
            if (FSPEC[3] == "1")
            {
                // Item I010/140: Time of Day
                string ToD1 = M.Octeto_A_Bin(paquete0[contador]);
                string ToD2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                string ToD3 = M.Octeto_A_Bin(paquete0[contador + 2]);

                string ToD_Invertido = ToD1 + ToD2 + ToD3;
                char[] ToD_bin = ToD_Invertido.ToCharArray();
                Array.Reverse(ToD_bin);

                for (int i = 0; i < ToD_bin.Length; i++)
                {
                    if (ToD_bin[i].ToString() == "1") { Time_Day = Time_Day + Math.Pow(2, -7 + i); }
                    else { }
                }
                contador += 3;
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
                string TN_1 = M.Octeto_A_Bin(paquete0[contador]);
                string TN_2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                string TN = TN_1 + TN_2;
                Track_Num = Convert.ToInt32(TN, 2);

                contador += 2;
            }
            if (FSPEC[10] == "1")
            {
                // Item I010/170: Track Status
                string Status_Bin = M.Octeto_A_Bin(paquete0[contador]);
                char[] Status = Status_Bin.ToCharArray();

                if (Status[0].ToString() == "1") { Track_Status = "CNF: Track in initalisation phase"; }
                if (Status[0].ToString() == "0") { Track_Status = "CNF: Confirmed Track"; }

                if (Status[1].ToString() == "1") { Track_Status = Track_Status + "/TRE: Last Report for a Track"; }
                if (Status[1].ToString() == "0") { Track_Status = Track_Status + "/TRE: Default"; }

                string CST_bin = Status[2].ToString() + Status[3].ToString();
                Int32 CST = Convert.ToInt32(CST_bin, 2);
                if (CST == 0) { Track_Status = Track_Status + "/CST: No Extrapolation"; }
                if (CST == 1) { Track_Status = Track_Status + "/CST: Predictable extrapolation due to sensor refresh period"; }
                if (CST == 2) { Track_Status = Track_Status + "/CST: Predictable extrapolation in masked area"; }
                if (CST == 3) { Track_Status = Track_Status + "/CST: Extrapolation due to unpredictable absence of detection"; }

                if (Status[4].ToString() == "1") { Track_Status = Track_Status + "/MAH: Horizontal manouvre"; }
                if (Status[4].ToString() == "0") { Track_Status = Track_Status + "/MAH: Default"; }

                if (Status[5].ToString() == "1") { Track_Status = Track_Status + "/TCC: Horizontal manouvre"; }
                if (Status[5].ToString() == "0") { Track_Status = Track_Status + "/TCC: Default"; }

                if (Status[6].ToString() == "1") { Track_Status = Track_Status + "/STH: Smoothed Position"; }
                if (Status[6].ToString() == "0") { Track_Status = Track_Status + "/STH: Measured Position"; }

                if (Status[7].ToString() == "1")
                {
                    contador += 1;
                    string Status_Bin1 = M.Octeto_A_Bin(paquete0[contador]);
                    char[] Status1 = Status_Bin1.ToCharArray();

                    string TOM_bin = Status1[0].ToString() + Status1[1].ToString();
                    Int32 TOM = Convert.ToInt32(TOM_bin, 2);
                    if (TOM == 0) { Track_Status = Track_Status + "/TOM: Unknown Type of Movement"; }
                    if (TOM == 1) { Track_Status = Track_Status + "/TOM: Taking-off"; }
                    if (TOM == 2) { Track_Status = Track_Status + "/TOM: Landing"; }
                    if (TOM == 3) { Track_Status = Track_Status + "/TOM: Other Types of Movements"; }

                    string DOU_bin = Status1[2].ToString() + Status1[3].ToString() + Status1[4].ToString();
                    Int32 DOU = Convert.ToInt32(DOU_bin, 2);
                    if (DOU == 0) { Track_Status = Track_Status + "/DOU: No doubt"; }
                    if (DOU == 1) { Track_Status = Track_Status + "/DOU: Doubtful correlation"; }
                    if (DOU == 2) { Track_Status = Track_Status + "/DOU: Doubtful correlation in clutter"; }
                    if (DOU == 3) { Track_Status = Track_Status + "/DOU: Loss of accuracy"; }
                    if (DOU == 4) { Track_Status = Track_Status + "/DOU: Loss of accuracy in clutter"; }
                    if (DOU == 5) { Track_Status = Track_Status + "/DOU: Unstable track"; }
                    if (DOU == 6) { Track_Status = Track_Status + "/DOU: Previously Cosated"; }

                    contador += 1;
                    string MRS_bin = Status1[5].ToString() + Status1[6].ToString();
                    Int32 MRS = Convert.ToInt32(MRS_bin, 2);
                    if (MRS == 0) { Track_Status = Track_Status + "/MRS: Merge or split indication undetermined"; }
                    if (MRS == 1) { Track_Status = Track_Status + "/MRS: Track merged by association to plot"; }
                    if (MRS == 2) { Track_Status = Track_Status + "/MRS: Track merged by non-association to plot"; }
                    if (MRS == 3) { Track_Status = Track_Status + "/MRS: Split track"; }

                    if (Status1[7].ToString() == "1")
                    {
                        contador += 1;
                        string Status_Bin2 = M.Octeto_A_Bin(paquete0[contador]);
                        char[] Status2 = Status_Bin2.ToCharArray();

                        if (Status2[0].ToString() == "1") { Track_Status = Track_Status + "/GHO: Ghost Track"; }
                        if (Status2[0].ToString() == "0") { Track_Status = Track_Status + "/GHO: Default"; }
                    }
                    if (Status1[7].ToString() == "0")
                    {
                        contador += 1;
                    }
                }
                if (Status[7].ToString() == "0")
                {
                    contador += 1;
                }
            }
            if (FSPEC[11] == "1")
            {
                // Item I010/060: Mode-3/A in Octal
                string Mode3A_Bin1 = M.Octeto_A_Bin(paquete0[contador]);
                string Mode3A_Bin2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                string Mode3A_Bin = Mode3A_Bin1 + Mode3A_Bin2;
                char[] Mode3A_v = Mode3A_Bin.ToCharArray();

                if (Mode3A_v[0].ToString() == "0") { Mode3A_Code = "V: Code validated"; }
                if (Mode3A_v[0].ToString() == "1") { Mode3A_Code = "V: Code not validated"; }

                if (Mode3A_v[1].ToString() == "0") { Mode3A_Code = Mode3A_Code + "/G: Default"; }
                if (Mode3A_v[1].ToString() == "1") { Mode3A_Code = Mode3A_Code + "/G: Garbled Code"; }

                if (Mode3A_v[2].ToString() == "0") { Mode3A_Code = Mode3A_Code + "/L: Mode-3/A code derived from the reply of the transponder"; }
                if (Mode3A_v[2].ToString() == "0") { Mode3A_Code = Mode3A_Code + "/L: Mode-3/A code not extracted during the last scan"; }

                Mode3A_Code = Mode3A_Code + "/" + Mode3A_v[4].ToString() + Mode3A_v[5].ToString() + Mode3A_v[6].ToString() + Mode3A_v[7].ToString() +
                    Mode3A_v[8].ToString() + Mode3A_v[9].ToString() + Mode3A_v[10].ToString() + Mode3A_v[11].ToString() + Mode3A_v[12].ToString() +
                    Mode3A_v[13].ToString() + Mode3A_v[14].ToString() + Mode3A_v[15].ToString();

                contador += 2;
            }
            if (FSPEC[12] == "1")
            {
                // Item I010/220: Target Address
                string TA_Bin1 = M.Octeto_A_Bin(paquete0[contador]);
                string TA_Bin2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                string TA_Bin3 = M.Octeto_A_Bin(paquete0[contador + 2]);
                Target_Add = Convert.ToInt32(TA_Bin1 + TA_Bin2 + TA_Bin3, 2);

                contador += 3;
            }
            if (FSPEC[13] == "1")
            {
                // Item I010/245: Target Identification
                string TI_Bin1 = M.Octeto_A_Bin(paquete0[contador]);
                string TI_Bin2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                string TI_Bin3 = M.Octeto_A_Bin(paquete0[contador + 2]);
                string TI_Bin4 = M.Octeto_A_Bin(paquete0[contador + 3]);
                string TI_Bin5 = M.Octeto_A_Bin(paquete0[contador + 4]);
                string TI_Bin6 = M.Octeto_A_Bin(paquete0[contador + 5]);
                string TI_Bin7 = M.Octeto_A_Bin(paquete0[contador + 6]);

                string TI_Bin = TI_Bin1 + TI_Bin2 + TI_Bin3 + TI_Bin4 + TI_Bin5 + TI_Bin6 + TI_Bin7;
                char[] TI_v = TI_Bin.ToCharArray();

                Int32 STI = Convert.ToInt32(TI_v[0].ToString() + TI_v[1].ToString(), 2);
                if (STI == 0) { Target_ID = "STI: Callsign or registration downlinked from transponder"; }
                if (STI == 1) { Target_ID = "STI: Callsign not downlinked from transponder"; }
                if (STI == 2) { Target_ID = "STI: Registration not downlinked from transponder"; }

                Target_ID = new string(TI_v, 8, 55);

                contador += 7;
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