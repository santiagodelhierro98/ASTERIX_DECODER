using System;
using System.Collections.Generic;

namespace CLASSES
{
    public class CAT10
    {
        // Definir los Data Items como variables, para procesar las que están en el paquete (las que son 1)
        // El tipo de cada variable depende de la precisión con la que se nos proporciona (especificado pdf CAT10)

        public string Message_Type = null;
        public int[] Data_Source_ID = new int[2];
        public string[] Target_Rep_Descript = new string[11];
        public double[] Pos_PolarCoord = new double[2];   //rho,theta
        public double[] Pos_WGS84 = new double[2];
        public double[] Pos_Cartesian = new double[2];
        public string[] Mode3A_Code = new string[4];
        public string[] FL = new string[3];
        public double Height = double.NaN;   //ft
        public double Amplitude = double.NaN;    //dBm
        public double Time_Day; //seconds
        public int? Track_Num = null;
        public string[] Track_Status = new string[10];
        public double[] Track_Vel_Polar = new double[2];    // (NM/s, degrees)
        public double[] Track_Vel_Cartesian = new double[2];    // m/s
        public double[] Acceleration = new double[2]; // m/s^2
        public string Target_Add;
        public string Target_ID = null;
        public string[] Mode_SMB = new string[4];
        public double[] Target_Size_Heading = new double[3];    // m,degrees
        public double[] Presence = new double[3];   // rho,theta
        public string Fleet_ID = null;
        public string[] Pre_Prog_Message = new string[2];
        public double[] StndrdDev_Position = new double[3]; // m^2
        public string[] Sys_Status = new string[5];
        public string STI = null;

        Metodos M = new Metodos();

        public void Decode10(string[] paquete)
        {
            Metodos M = new Metodos();
            int longitud = M.Longitud_Paquete(paquete);
            string[] paquete0 = new string[longitud];
            for (int i = 0; i < longitud; i++)
            {
                paquete0[i] = M.Poner_Zeros_Delante(paquete[i]);
                string bitscat = Convert.ToString(Convert.ToInt32(paquete0[0], 16), 2);
                double CAT = Convert.ToInt32(bitscat, 2);
                if (CAT != 10) { i = i + 1; }
            }
            List<string> FSPEC = new List<string>(M.FSPEC(paquete0));

            // Posicion del vector paquete0 donde empieza la info despues del FSPEC
            int contador = Convert.ToInt32(FSPEC[FSPEC.Count - 1]) + 1;
            FSPEC.RemoveAt(FSPEC.Count - 1);

            if (FSPEC[0] == "1")
            {
                // Item I010/010: Data source ID 
                string SAC_Bin = M.Octeto_A_Bin(paquete0[contador]);
                string SIC_Bin = M.Octeto_A_Bin(paquete0[contador + 1]);

                double SAC = Convert.ToInt32(SAC_Bin, 2);
                double SIC = Convert.ToInt32(SIC_Bin, 2);

                Data_Source_ID[0] = Convert.ToInt32(SIC);
                Data_Source_ID[1] = Convert.ToInt32(SAC);

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
                contador += 1;
            }
            if (FSPEC[2] == "1")
            {
                // Item I010/020: Target Report Descriptor
                string TargetReport_Bin = M.Octeto_A_Bin(paquete0[contador]);
                char[] Target_Bits = TargetReport_Bin.ToCharArray();
                string TYP_Bin = Target_Bits[0].ToString() + Target_Bits[1].ToString() + Target_Bits[2].ToString();
                Int32 TYP = Convert.ToInt32(TYP_Bin, 2);
                // TYP
                if (TYP == 0) { Target_Rep_Descript[0] = "SSR Multilateration"; }
                if (TYP == 1) { Target_Rep_Descript[0] = "Mode S Multilateration"; }
                if (TYP == 2) { Target_Rep_Descript[0] = "ADS-B"; }
                if (TYP == 3) { Target_Rep_Descript[0] = "PSR"; }
                if (TYP == 4) { Target_Rep_Descript[0] = "Magnetic Loop System"; }
                if (TYP == 5) { Target_Rep_Descript[0] = "HF Multilateration"; }
                if (TYP == 6) { Target_Rep_Descript[0] = "Not defined"; }
                if (TYP == 7) { Target_Rep_Descript[0] = "Other types"; }
                //DCR
                if (Target_Bits[3].ToString() == "1") { Target_Rep_Descript[1] = "Differential Correction (ADS-B)"; }
                if (Target_Bits[3].ToString() == "0") { Target_Rep_Descript[1] = "No Differential Correction (ADS-B)"; }
                //CHN
                if (Target_Bits[4].ToString() == "1") { Target_Rep_Descript[2] = "Chain 2"; }
                if (Target_Bits[4].ToString() == "0") { Target_Rep_Descript[2] = "Chain 1"; }
                //GBS
                if (Target_Bits[5].ToString() == "1") { Target_Rep_Descript[3] = "Transponder Ground bit set"; }
                if (Target_Bits[5].ToString() == "0") { Target_Rep_Descript[3] = "Transponder Ground bit not set"; }
                //CRT
                if (Target_Bits[6].ToString() == "1") { Target_Rep_Descript[4] = "Corrupted reply in multilateration"; }
                if (Target_Bits[6].ToString() == "0") { Target_Rep_Descript[4] = "No Corrupted replies in multilateration"; }

                if (Target_Bits[7].ToString() == "1")
                {
                    contador += 1;
                    // First extent
                    string TargetReport_Bin1 = M.Octeto_A_Bin(paquete0[contador]);
                    char[] Target_Bits1 = TargetReport_Bin1.ToCharArray();
                    //SIM
                    if (Target_Bits1[0].ToString() == "1") { Target_Rep_Descript[5] = "Simulated Target Report"; }
                    if (Target_Bits1[0].ToString() == "0") { Target_Rep_Descript[5] = "Actual Target Report"; }
                    //TST
                    if (Target_Bits1[1].ToString() == "1") { Target_Rep_Descript[6] = "Test Target"; }
                    if (Target_Bits1[1].ToString() == "0") { Target_Rep_Descript[6] = "Default"; }
                    //RAB
                    if (Target_Bits1[2].ToString() == "1") { Target_Rep_Descript[7] = "Report from field monitor(fixed transponder)"; }
                    if (Target_Bits1[2].ToString() == "0") { Target_Rep_Descript[7] = "Report from target transponder"; }

                    string LOP_Bin = Target_Bits1[3].ToString() + Target_Bits1[4].ToString();
                    Int32 LOP = Convert.ToInt32(LOP_Bin, 2);
                    //LOP
                    if (LOP == 0) { Target_Rep_Descript[8] = "Undetermined"; }
                    if (LOP == 1) { Target_Rep_Descript[8] = "Loop Start"; }
                    if (LOP == 2) { Target_Rep_Descript[8] = "Loop Finish"; }

                    string TOT_Bin = Target_Bits1[5].ToString() + Target_Bits1[6].ToString();
                    Int32 TOT = Convert.ToInt32(TOT_Bin, 2);
                    //TOT
                    if (TOT == 0) { Target_Rep_Descript[9] = "Undetermined"; }
                    if (TOT == 1) { Target_Rep_Descript[9] = "Aircraft"; }
                    if (TOT == 2) { Target_Rep_Descript[9] = "Ground Vehicle"; }
                    if (TOT == 3) { Target_Rep_Descript[9] = "Helicopter"; }

                    if (Target_Bits1[7].ToString() == "1")
                    {
                        contador += 1;
                        // Second Extent
                        string TargetReport_Bin2 = M.Octeto_A_Bin(paquete0[contador]);
                        char[] Target_Bits2 = TargetReport_Bin2.ToCharArray();
                        //SPI
                        if (Target_Bits2[0].ToString() == "1") { Target_Rep_Descript[10] = "Special Position Identification"; }
                        if (Target_Bits2[0].ToString() == "0") { Target_Rep_Descript[10] = "Abcense of SPI"; }

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

                string ToD = ToD1 + ToD2 + ToD3;

                Time_Day = 12 * 3600 + Math.Round(Convert.ToInt32(ToD, 2) / 128.0, 3);
                contador += 3;
            }
            if (FSPEC[4] == "1")
            {
                // Item I010/041: Position WGS-84
                string position1 = M.Octeto_A_Bin(paquete0[contador]);
                string position2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                string position3 = M.Octeto_A_Bin(paquete0[contador + 2]);
                string position4 = M.Octeto_A_Bin(paquete0[contador + 3]);
                string position5 = M.Octeto_A_Bin(paquete0[contador + 4]);
                string position6 = M.Octeto_A_Bin(paquete0[contador + 5]);
                string position7 = M.Octeto_A_Bin(paquete0[contador + 6]);
                string position8 = M.Octeto_A_Bin(paquete0[contador + 7]);

                string lat_bin = position1 + position2 + position3 + position4;
                string lon_bin = position5 + position6 + position7 + position8;

                Pos_WGS84[0] = Math.Round(M.ComplementoA2(lat_bin) * (180 / Math.Pow(2, 31)), 3);
                Pos_WGS84[1] = Math.Round(M.ComplementoA2(lon_bin) * (180 / Math.Pow(2, 31)), 3);

                contador += 8;
            }
            if (FSPEC[5] == "1")
            {
                // Item I010/040: Position Polar
                string PolarR1 = M.Octeto_A_Bin(paquete0[contador]);
                string PolarR2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                string PolarT1 = M.Octeto_A_Bin(paquete0[contador + 2]);
                string PolarT2 = M.Octeto_A_Bin(paquete0[contador + 3]);

                string PolarR = PolarR1 + PolarR2;
                string PolarT = PolarT1 + PolarT2;

                Pos_PolarCoord[0] = Convert.ToInt32(PolarR, 2);
                Pos_PolarCoord[1] = Math.Round(Convert.ToInt32(PolarT, 2) * (360.0 / Math.Pow(2, 16)), 3);

                contador += 4;
            }
            if (FSPEC[6] == "1")
            {
                // Item I010/042: Position Cartesian
                string Cartx1 = M.Octeto_A_Bin(paquete0[contador]);
                string Cartx2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                string Carty1 = M.Octeto_A_Bin(paquete0[contador + 2]);
                string Carty2 = M.Octeto_A_Bin(paquete0[contador + 3]);

                string CartX = Cartx1 + Cartx2;
                string CartY = Carty1 + Carty2;

                Pos_Cartesian[0] = Convert.ToInt32(M.ComplementoA2(CartX));
                Pos_Cartesian[1] = Convert.ToInt32(M.ComplementoA2(CartY));

                contador += 4;
            }

            if (FSPEC.Count > 7)
            {
                if (FSPEC[7] == "1")
                {
                    // Item I010/200: Track Velocity Polar
                    string polar_vel1 = M.Octeto_A_Bin(paquete0[contador]);
                    string polar_vel2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                    string polar_vel3 = M.Octeto_A_Bin(paquete0[contador + 2]);
                    string polar_vel4 = M.Octeto_A_Bin(paquete0[contador + 3]);

                    string track_gs = polar_vel1 + polar_vel2;
                    string track_ta = polar_vel3 + polar_vel4;

                    Track_Vel_Cartesian[0] = Math.Round(Convert.ToInt32(track_gs, 2) * Math.Pow(2, -14), 3);
                    Track_Vel_Cartesian[1] = Math.Round(Convert.ToInt32(track_ta, 2) * (360.0 / Math.Pow(2, 16)), 3);
                    contador += 4;
                }
                if (FSPEC[8] == "1")
                {
                    // Item I010/202: Track Velocity Cartesian
                    string track_vel1 = M.Octeto_A_Bin(paquete0[contador]);
                    string track_vel2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                    string track_vel3 = M.Octeto_A_Bin(paquete0[contador + 2]);
                    string track_vel4 = M.Octeto_A_Bin(paquete0[contador + 3]);

                    string track_vx = track_vel1 + track_vel2;
                    string track_vy = track_vel3 + track_vel4;

                    Track_Vel_Cartesian[0] = Math.Round(M.ComplementoA2(track_vx) * 0.25, 3);
                    Track_Vel_Cartesian[1] = Math.Round(M.ComplementoA2(track_vy) * 0.25, 3);
                    contador += 4;
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
                    //CNF
                    if (Status[0].ToString() == "1") { Track_Status[0] = "Track in initalisation phase"; }
                    if (Status[0].ToString() == "0") { Track_Status[0] = "Confirmed Track"; }
                    //TRE
                    if (Status[1].ToString() == "1") { Track_Status[1] = "Last Report for a Track"; }
                    if (Status[1].ToString() == "0") { Track_Status[1] = "Default"; }

                    string CST_bin = Status[2].ToString() + Status[3].ToString();
                    Int32 CST = Convert.ToInt32(CST_bin, 2);
                    //CST
                    if (CST == 0) { Track_Status[2] = "No Extrapolation"; }
                    if (CST == 1) { Track_Status[2] = "Predictable extrapolation due to sensor refresh period"; }
                    if (CST == 2) { Track_Status[2] = "Predictable extrapolation in masked area"; }
                    if (CST == 3) { Track_Status[2] = "Extrapolation due to unpredictable absence of detection"; }
                    //MAH
                    if (Status[4].ToString() == "1") { Track_Status[3] = "Horizontal manouvre"; }
                    if (Status[4].ToString() == "0") { Track_Status[3] = "Default"; }
                    //TCC
                    if (Status[5].ToString() == "1") { Track_Status[4] = "Horizontal manouvre"; }
                    if (Status[5].ToString() == "0") { Track_Status[4] = "Default"; }
                    //STH
                    if (Status[6].ToString() == "1") { Track_Status[5] = "Smoothed Position"; }
                    if (Status[6].ToString() == "0") { Track_Status[5] = "Measured Position"; }

                    if (Status[7].ToString() == "1")
                    {
                        contador += 1;
                        string Status_Bin1 = M.Octeto_A_Bin(paquete0[contador]);
                        char[] Status1 = Status_Bin1.ToCharArray();

                        string TOM_bin = Status1[0].ToString() + Status1[1].ToString();
                        Int32 TOM = Convert.ToInt32(TOM_bin, 2);
                        //TOM
                        if (TOM == 0) { Track_Status[6] = "Unknown Type of Movement"; }
                        if (TOM == 1) { Track_Status[6] = "Taking-off"; }
                        if (TOM == 2) { Track_Status[6] = "Landing"; }
                        if (TOM == 3) { Track_Status[6] = "Other Types of Movements"; }

                        string DOU_bin = Status1[2].ToString() + Status1[3].ToString() + Status1[4].ToString();
                        Int32 DOU = Convert.ToInt32(DOU_bin, 2);
                        //DOU
                        if (DOU == 0) { Track_Status[7] = "No doubt"; }
                        if (DOU == 1) { Track_Status[7] = "Doubtful correlation"; }
                        if (DOU == 2) { Track_Status[7] = "Doubtful correlation in clutter"; }
                        if (DOU == 3) { Track_Status[7] = "Loss of accuracy"; }
                        if (DOU == 4) { Track_Status[7] = "Loss of accuracy in clutter"; }
                        if (DOU == 5) { Track_Status[7] = "Unstable track"; }
                        if (DOU == 6) { Track_Status[7] = "Previously Cosated"; }

                        contador += 1;
                        string MRS_bin = Status1[5].ToString() + Status1[6].ToString();
                        Int32 MRS = Convert.ToInt32(MRS_bin, 2);
                        //MRS
                        if (MRS == 0) { Track_Status[8] = "Merge or split indication undetermined"; }
                        if (MRS == 1) { Track_Status[8] = "Track merged by association to plot"; }
                        if (MRS == 2) { Track_Status[8] = "Track merged by non-association to plot"; }
                        if (MRS == 3) { Track_Status[8] = "Split track"; }

                        if (Status1[7].ToString() == "1")
                        {
                            contador += 1;
                            string Status_Bin2 = M.Octeto_A_Bin(paquete0[contador]);
                            char[] Status2 = Status_Bin2.ToCharArray();
                            //GHO
                            if (Status2[0].ToString() == "1") { Track_Status[9] = "Ghost Track"; }
                            if (Status2[0].ToString() == "0") { Track_Status[9] = "Default"; }
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
                    // V
                    if (Mode3A_v[0].ToString() == "0") { Mode3A_Code[0] = "Code validated"; }
                    if (Mode3A_v[0].ToString() == "1") { Mode3A_Code[0] = "Code not validated"; }
                    // G
                    if (Mode3A_v[1].ToString() == "0") { Mode3A_Code[1] = "Default"; }
                    if (Mode3A_v[1].ToString() == "1") { Mode3A_Code[1] = "Garbled Code"; }
                    // L
                    if (Mode3A_v[2].ToString() == "0") { Mode3A_Code[2] = "Mode-3/A code derived from the reply of the transponder"; }
                    if (Mode3A_v[2].ToString() == "0") { Mode3A_Code[2] = "Mode-3/A code not extracted during the last scan"; }

                    Mode3A_Code[3] = Mode3A_v[4].ToString() + Mode3A_v[5].ToString() + Mode3A_v[6].ToString() + Mode3A_v[7].ToString() +
                        Mode3A_v[8].ToString() + Mode3A_v[9].ToString() + Mode3A_v[10].ToString() + Mode3A_v[11].ToString() + Mode3A_v[12].ToString() +
                        Mode3A_v[13].ToString() + Mode3A_v[14].ToString() + Mode3A_v[15].ToString();

                    contador += 2;
                }
                if (FSPEC[12] == "1")
                {
                    // Item I010/220: Target Address
                    Target_Add = Convert.ToString(paquete0[contador]) + Convert.ToString(paquete0[contador + 1]) + Convert.ToString(paquete0[contador + 2]);

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

                    Int32 sti = Convert.ToInt32(TI_v[0].ToString() + TI_v[1].ToString(), 2);
                    // STI
                    if (sti == 0) { STI = "Callsign or registration downlinked from transponder"; }
                    if (sti == 1) { STI = "Callsign not downlinked from transponder"; }
                    if (sti == 2) { STI = "Registration not downlinked from transponder"; }

                    for (int i = 8; i < TI_v.Length; i++) { Target_ID += TI_v[i].ToString(); }
                    Target_ID = M.Compare_bits(Target_ID);

                    contador += 7;
                }

                if (FSPEC.Count > 14)
                {
                    if (FSPEC[14] == "1")
                    {
                        // Item I010/250: Mode S MB Data
                        string REP = M.Octeto_A_Bin(paquete0[contador]);
                        string SMB2 = M.Octeto_A_Bin(paquete0[contador + 1]);
                        string SMB3 = M.Octeto_A_Bin(paquete0[contador + 2]);
                        string SMB4 = M.Octeto_A_Bin(paquete0[contador + 3]);
                        string SMB5 = M.Octeto_A_Bin(paquete0[contador + 4]);
                        string SMB6 = M.Octeto_A_Bin(paquete0[contador + 5]);
                        string SMB7 = M.Octeto_A_Bin(paquete0[contador + 6]);
                        string SMB8 = M.Octeto_A_Bin(paquete0[contador + 7]);
                        string BDS = M.Octeto_A_Bin(paquete0[contador + 8]);
                        char[] BDS_bin = BDS.ToCharArray();

                        Mode_SMB[0] = REP;
                        //MB
                        Mode_SMB[1] = (Convert.ToInt32(SMB2 + SMB3 + SMB4 + SMB5 + SMB6 + SMB7 + SMB8, 2)).ToString();
                        // BDS 1
                        Mode_SMB[2] = BDS_bin[0].ToString() + BDS_bin[1].ToString() + BDS_bin[2].ToString() + BDS_bin[3].ToString();
                        // BDS 2
                        Mode_SMB[3] = BDS_bin[4].ToString() + BDS_bin[5].ToString() + BDS_bin[6].ToString() + BDS_bin[7].ToString();

                        contador += 9;
                    }
                    if (FSPEC[15] == "1")
                    {
                        // Item I010/300: Vehicle Fleet ID
                        string fleetID = M.Octeto_A_Bin(paquete0[contador]);
                        int VFI = Convert.ToInt32(fleetID, 2);
                        if (VFI == 0) { Fleet_ID = "Unknown"; }
                        if (VFI == 1) { Fleet_ID = "ATC Equipment maintainance"; }
                        if (VFI == 2) { Fleet_ID = "Airport maintainance"; }
                        if (VFI == 3) { Fleet_ID = "Fire"; }
                        if (VFI == 4) { Fleet_ID = "Bird scarer"; }
                        if (VFI == 5) { Fleet_ID = "Snow plough"; }
                        if (VFI == 6) { Fleet_ID = "Runway sweeper"; }
                        if (VFI == 7) { Fleet_ID = "Emergency"; }
                        if (VFI == 8) { Fleet_ID = "Police"; }
                        if (VFI == 9) { Fleet_ID = "Bus"; }
                        if (VFI == 10) { Fleet_ID = "Tug (push/tow)"; }
                        if (VFI == 11) { Fleet_ID = "Grass cutter"; }
                        if (VFI == 12) { Fleet_ID = "Fuel"; }
                        if (VFI == 13) { Fleet_ID = "Baggage"; }
                        if (VFI == 14) { Fleet_ID = "Catering"; }
                        if (VFI == 15) { Fleet_ID = "Aircraft maintainance"; }
                        if (VFI == 16) { Fleet_ID = "Flyco (follow me)"; }

                        contador += 1;
                    }
                    if (FSPEC[16] == "1")
                    {
                        // Item I010/090: Flight Level
                        string FL1 = M.Octeto_A_Bin(paquete0[contador]);
                        string FL2 = M.Octeto_A_Bin(paquete0[contador + 1]);

                        string FL_ = FL1 + FL2;
                        char[] FL_bin = FL_.ToCharArray();
                        //V
                        if (FL_bin[0].ToString() == "0") { FL[0] = "Code Validated"; }
                        if (FL_bin[0].ToString() == "1") { FL[0] = "Code Not Validated"; }
                        //G
                        if (FL_bin[1].ToString() == "0") { FL[1] = "Default"; }
                        if (FL_bin[1].ToString() == "1") { FL[1] = "Garbled Code"; }

                        string FL3 = FL_bin[2].ToString();
                        for (int i = 3; i < FL_bin.Length; i++) { FL3 += FL_bin[i].ToString(); }
                        FL[2] = (M.ComplementoA2(FL3) * 0.25).ToString();
                        contador += 2;
                    }
                    if (FSPEC[17] == "1")
                    {
                        // Item I010/091: Measured Height
                        string height1 = M.Octeto_A_Bin(paquete0[contador]);
                        string height2 = M.Octeto_A_Bin(paquete0[contador + 1]);

                        string height = height1 + height2;
                        Height = Math.Round(Convert.ToInt32(height) * 6.25, 3);

                        contador += 2;
                    }
                    if (FSPEC[18] == "1")
                    {
                        // Item I010/270: Target Size and Orientation
                        string length = M.Octeto_A_Bin(paquete0[contador]);
                        char[] length_v = length.ToCharArray();
                        string length_bin = new string(length_v, 0, 6);
                        Target_Size_Heading[0] = Convert.ToInt32(length_bin, 2);

                        if (length_v[7].ToString() == "1")
                        {
                            string length1 = M.Octeto_A_Bin(paquete0[contador + 1]);
                            char[] length1_v = length1.ToCharArray();
                            string lenght1_bin = new string(length1_v, 0, 6);
                            Target_Size_Heading[1] = Math.Round(Convert.ToInt32(lenght1_bin, 2) * (360.0 / 128.0), 3);

                            if (length1_v[7].ToString() == "1")
                            {
                                string length2 = M.Octeto_A_Bin(paquete0[contador + 2]);
                                char[] length2_v = length2.ToCharArray();
                                string lenght2_bin = new string(length2_v, 0, 6);
                                Target_Size_Heading[2] = Convert.ToInt32(lenght2_bin, 2);

                                contador += 3;
                            }
                            if (length1_v[7].ToString() == "0") { contador += 2; }
                        }
                        if (length_v[7].ToString() == "0") { contador += 1; }
                    }
                    if (FSPEC[19] == "1")
                    {
                        // Item I010/550: System Status
                        string stat = M.Octeto_A_Bin(paquete0[contador]);
                        char[] stat_v = stat.ToCharArray();

                        int NOGO = Convert.ToInt32(stat_v[0].ToString() + stat_v[1].ToString(), 2);
                        // NOGO
                        if (NOGO == 0) { Sys_Status[0] = "Operational"; }
                        if (NOGO == 1) { Sys_Status[0] = "Degraded"; }
                        if (NOGO == 2) { Sys_Status[0] = "NOGO"; }
                        // OVL
                        if (stat_v[2].ToString() == "0") { Sys_Status[1] = "No overload"; }
                        if (stat_v[2].ToString() == "1") { Sys_Status[1] = "Overload"; }
                        // TSV
                        if (stat_v[3].ToString() == "0") { Sys_Status[2] = "Valid"; }
                        if (stat_v[3].ToString() == "1") { Sys_Status[2] = "Invalid"; }
                        // DIV
                        if (stat_v[4].ToString() == "0") { Sys_Status[3] = "Normal Operation"; }
                        if (stat_v[4].ToString() == "1") { Sys_Status[3] = "Diversity degraded"; }
                        // TTF
                        if (stat_v[5].ToString() == "0") { Sys_Status[4] = "Test Target Operative"; }
                        if (stat_v[5].ToString() == "1") { Sys_Status[4] = "Test Target Failure"; }

                        contador += 1;
                    }
                    if (FSPEC[20] == "1")
                    {
                        // Item I010/310: Pre-programmed Message
                        string messg = M.Octeto_A_Bin(paquete0[contador]);
                        char[] messg_v = messg.ToCharArray();
                        // TRB
                        if (messg_v[0].ToString() == "0") { Pre_Prog_Message[0] = "Default"; }
                        if (messg_v[0].ToString() == "1") { Pre_Prog_Message[0] = "In Trouble"; }

                        string MSGbin = new string(messg_v, 1, 7);
                        int MSG = Convert.ToInt32(MSGbin, 2);
                        // MSG
                        if (MSG == 1) { Pre_Prog_Message[1] = "Towing Aircraft"; }
                        if (MSG == 2) { Pre_Prog_Message[1] = "'Follow Me' operation"; }
                        if (MSG == 3) { Pre_Prog_Message[1] = "Runway Check"; }
                        if (MSG == 4) { Pre_Prog_Message[1] = "Emergency Operation"; }
                        if (MSG == 5) { Pre_Prog_Message[1] = "Work in progress"; }

                        contador += 1;
                    }

                    if (FSPEC.Count > 21)
                    {
                        if (FSPEC[21] == "1")
                        {
                            // Item I010/500: Standard Deviation of Position
                            string SDx = M.Octeto_A_Bin(paquete0[contador]);
                            string SDy = M.Octeto_A_Bin(paquete0[contador + 1]);
                            string SDxy1 = M.Octeto_A_Bin(paquete0[contador + 2]);
                            string SDxy2 = M.Octeto_A_Bin(paquete0[contador + 3]);

                            StndrdDev_Position[0] = Math.Round(Convert.ToInt32(SDx, 2) * 0.25, 3);
                            StndrdDev_Position[1] = Math.Round(Convert.ToInt32(SDy, 2) * 0.25, 3);

                            string SDxy = SDxy1 + SDxy2;
                            StndrdDev_Position[2] = Math.Round(Convert.ToInt32(SDxy, 2) * 0.25, 3);

                            contador += 4;
                        }
                        if (FSPEC[22] == "1")
                        {
                            // Item I010/280: Presence
                            string REP = M.Octeto_A_Bin(paquete0[contador]);
                            string DRHO = M.Octeto_A_Bin(paquete0[contador + 1]);
                            string DTHETA = M.Octeto_A_Bin(paquete0[contador + 2]);

                            Presence[0] = Convert.ToInt32(REP, 2);
                            Presence[1] = Convert.ToInt32(DRHO, 2);
                            Presence[2] = Math.Round(Convert.ToInt32(DTHETA, 2) * 0.15, 3);

                            contador += 3;
                        }
                        if (FSPEC[23] == "1")
                        {
                            // Item I010/131: Amplitude of Primary Plot
                            string amplitude = M.Octeto_A_Bin(paquete0[contador]);
                            Amplitude = Convert.ToInt32(amplitude, 2);

                            contador += 1;
                        }
                        if (FSPEC[24] == "1")
                        {
                            // Item I010/210: Calculated Acceleration
                            string Ax = M.Octeto_A_Bin(paquete0[contador]);
                            string Ay = M.Octeto_A_Bin(paquete0[contador + 1]);

                            Acceleration[0] = Math.Round(M.ComplementoA2(Ax) * 0.25, 3);
                            Acceleration[1] = Math.Round(M.ComplementoA2(Ay) * 0.25, 3);

                            contador += 2;
                        }
                    }
                    else { }
                }
                else { }
            }
            else { }
        }
    }
}