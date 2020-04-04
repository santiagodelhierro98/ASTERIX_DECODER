using System;
using System.Collections.Generic;
using System.Text;

namespace CLASSES
{
    public class CAT21
    {
        Metodos Met = new Metodos();

        // Definir los Data Items como variables, para procesar las que están en el paquete (las que son 1)
        // El tipo de cada variable depende de la precisión con la que se nos proporciona (especificado pdf CAT21)

        public double Data_Source_ID_SIC = double.NaN;
        public double Data_Source_ID_SAC = double.NaN;
        public string[] Target_Report_Desc = new string[15];
        public double Track_Num = double.NaN;
        public string Service_ID = null;
        public double ToA_Position = double.NaN; // seconds, ToA=Time of Aplicability
        public double Lat_WGS_84 = double.NaN; //in degrees
        public double Lon_WGS_84 = double.NaN; //in degrees
        public double High_Res_Lat_WGS_84 = double.NaN; //high resolution Lat WGS 84 in degrees
        public double High_Res_Lon_WGS_84 = double.NaN; //high resolution Lon WGS 84 in degrees
        public double ToA_Velocity = double.NaN; // seconds
        public double[] Air_Speed = new double[2];// ias and mach
        public double True_Airspeed = double.NaN; //kt
        public string Target_Address = null;//Target address (emitter identifier) assigned uniquely to each target. en hexa
        public double TMRP = double.NaN; // s
        public string[] TMRP_HP = new string[2]; // high precision TMRP
        public double TMRV = double.NaN; // s
        public string[] TMRV_HP = new string[2];// high precision TMRV
        public double GH = double.NaN; // ft
        public string M3AC = null; //Mode 3/A Code
        public double Roll = double.NaN; // roll angle in degrees
        public double FL = double.NaN; //flight level in feet
        public double MH = double.NaN; //Magnetic heading in degrees
        public string RE = null;
        public double BVR = double.NaN; // Barometric Vertical Rate in feet/minute
        public double GVR = double.NaN; // Geometric Vertical Rate in feet/minute
        public double GS = double.NaN; // ground speed in kt
        public double TA = double.NaN; // Track angle in degrees
        public double TAR = double.NaN; // Track Angle Rate in degees/s
        public double ToART = double.NaN; // Time of ASTERIX Report Transmission in s
        public string Target_ID = null;
        public string ECAT = null;
        public string SAS = null;
        public string Source = null;
        public double SA = double.NaN; //selected altitude in ft
        public string MV = null;
        public string AH = null;
        public string AM = null;
        public double FSSA = double.NaN; //final state selected altitde in ft
        public string TIS = null;
        public string TID = null;
        public string NAV = null;
        public double RP = double.NaN; //in seconds
        public string POA = null;
        public string CDTIS = null;
        public string B2LOW = null;
        public string RAS =  null;
        public string IDENT = null;
        public string LenWidth = null; //length and width of the aircraft in meters
        public double MAM = double.NaN; //in dBm;
        public double TYP = double.NaN;
        public double STYP = double.NaN;
        public double ARA = double.NaN;
        public double RAC = double.NaN;
        public double RAT = double.NaN;
        public double MTE = double.NaN;
        public double TTI = double.NaN;
        public double TID_ACAS = double.NaN;
        public double RID = double.NaN;
        public string[] Op_Status = new string[7];
        public string[] MOPS = new string[3];
        public string[] Met_Report = new string[4];
        public string[] Target_Status = new string[4];
        public string[] Quality_Indicators = new string[7];
        public int[] Mode_S = new int[4];
        public string[] Trajectory_Intent = new string[16];
        public double[] Data_Ages = new double[23];


        public void Decode21(string[] paquete, int q)
        {
            Metodos Met = new Metodos();
            int longitud = Met.Longitud_Paquete(paquete);
            string[] paquete0 = new string[longitud];
            
            for (int i = 0; i < longitud; i++)
            {
                paquete0[i] = Met.Poner_Zeros_Delante(paquete[i]);
                string bitscat = Convert.ToString(Convert.ToInt32(paquete0[0], 16), 2);
                double CAT = Convert.ToInt32(bitscat, 2);
                if (CAT != 21) { i = i + 1; }
            }
            List<string> FSPEC = new List<string>(Met.FSPEC(paquete0));
            // Posicion del vector paquete0 donde empieza la info despues del FSPEC
            int contador = Convert.ToInt32(FSPEC[FSPEC.Count - 1]) + 1;
            FSPEC.RemoveAt(FSPEC.Count - 1);
            // definicion de cada data item segun ADS-B Reports UAP

            if (FSPEC[0] == "1")
            {
                // Item I021/010 : Data Source Identification
                string SAC_Bin = Met.Octeto_A_Bin(paquete0[contador]);
                string SIC_Bin = Met.Octeto_A_Bin(paquete0[contador + 1]);
                double SAC = (Convert.ToInt32(SAC_Bin, 2));
                double SIC = (Convert.ToInt32(SIC_Bin, 2));

                //decodificamos , solo utilizaremos estos dos 
                Data_Source_ID_SIC = SIC;
                Data_Source_ID_SAC = SAC;

                contador = contador + 2;
            }
            if (FSPEC[1] == "1")
            {
                // I021/040 : Target Report Descriptor
                string TargetReport_Bin = Met.Octeto_A_Bin(paquete0[contador]);
                char[] Target_Bits = TargetReport_Bin.ToCharArray();

                string ATP_Bin = Target_Bits[0].ToString() + Target_Bits[1].ToString() + Target_Bits[2].ToString();
                Int32 ATP = Convert.ToInt32(ATP_Bin, 2);
                if (ATP == 0) { Target_Report_Desc[0] = "24-Bit ICAO address"; }
                if (ATP == 1) { Target_Report_Desc[0] = "Duplicate address"; }
                if (ATP == 2) { Target_Report_Desc[0] = "Surface vehicle address"; }
                if (ATP == 3) { Target_Report_Desc[0] = "Anonymous address"; }
                if (ATP == 4 || ATP == 5 || ATP == 6 || ATP == 7) { Target_Report_Desc[0] = "Reserved for future use"; }

                string ARC_Bin = Target_Bits[3].ToString() + Target_Bits[4].ToString();
                Int32 ARC = Convert.ToInt32(ARC_Bin, 2);
                if (ARC == 0) { Target_Report_Desc[1] = "25 ft"; }
                if (ARC == 1) { Target_Report_Desc[1] = "100 ft"; }
                if (ARC == 2) { Target_Report_Desc[1] = "Unknown"; }
                if (ARC == 3) { Target_Report_Desc[1] = "Invalid"; }

                if (Convert.ToInt32(Target_Bits[5].ToString()) == 0) { Target_Report_Desc[2] = "Default"; }
                if (Convert.ToInt32(Target_Bits[5].ToString()) == 1) { Target_Report_Desc[2] = "Range Check passed, CPR Validation pending"; }

                if (Convert.ToInt32(Target_Bits[6].ToString()) == 0) { Target_Report_Desc[3] = "Report from target transponder"; }
                if (Convert.ToInt32(Target_Bits[6].ToString()) == 1) { Target_Report_Desc[3] = "Report from field monitor (fixed transponder)"; }

                if (Convert.ToInt32(Target_Bits[7].ToString()) == 0) { contador = contador + 1; }
                if (Convert.ToInt32(Target_Bits[7].ToString()) == 1)
                {
                    //I021/040 - First Extension
                    contador = contador + 1;
                    string TargetReport1_Bin = Met.Octeto_A_Bin(paquete0[contador]);
                    char[] Target1_Bits = TargetReport1_Bin.ToCharArray();

                    if (Convert.ToInt32(Target1_Bits[0].ToString()) == 0) { Target_Report_Desc[4] = "No Differential Correction(ADS-B)"; }
                    if (Convert.ToInt32(Target1_Bits[0].ToString()) == 1) { Target_Report_Desc[4] = "Differential correction (ADS-B)"; }

                    if (Convert.ToInt32(Target1_Bits[1].ToString()) == 0) { Target_Report_Desc[5] = "Ground Bit not set"; }
                    if (Convert.ToInt32(Target1_Bits[1].ToString()) == 1) { Target_Report_Desc[5] = "Ground Bit set"; }

                    if (Convert.ToInt32(Target1_Bits[2].ToString()) == 0) { Target_Report_Desc[6] = "Actual target report"; }
                    if (Convert.ToInt32(Target1_Bits[2].ToString()) == 1) { Target_Report_Desc[6] = "Simulated target report"; }

                    if (Convert.ToInt32(Target1_Bits[3].ToString()) == 0) { Target_Report_Desc[7] = "Default"; }
                    if (Convert.ToInt32(Target1_Bits[3].ToString()) == 1) { Target_Report_Desc[7] = "Test Target"; }

                    if (Convert.ToInt32(Target1_Bits[4].ToString()) == 0) { Target_Report_Desc[8] = "Equipment capable to provide Selected Altitude"; }
                    if (Convert.ToInt32(Target1_Bits[4].ToString()) == 1) { Target_Report_Desc[8] = "Equipment not capable to provide Selected Altitude"; }

                    string CL_Bin = Target1_Bits[5].ToString() + Target1_Bits[6].ToString();
                    Int32 CL = Convert.ToInt32(ARC_Bin, 2);
                    if (CL == 0) { Target_Report_Desc[9] = "Report Valid"; }
                    if (CL == 1) { Target_Report_Desc[9] = "Report suspect"; }
                    if (CL == 2) { Target_Report_Desc[9] = "No information"; }
                    if (CL == 3) { Target_Report_Desc[9] = "Reserved for future use"; }

                    if (Convert.ToInt32(Target1_Bits[7].ToString()) == 0) { contador = contador + 1; }
                    if (Convert.ToInt32(Target1_Bits[7].ToString()) == 1)
                    {
                        //I021/040 - Second Extension : Error Conditions
                        contador = contador + 1;
                        string TargetReport2_Bin = Met.Octeto_A_Bin(paquete0[contador]);
                        char[] Target2_Bits = TargetReport2_Bin.ToCharArray();

                        if (Convert.ToInt32(Target2_Bits[2].ToString()) == 0) { Target_Report_Desc[10] = "Independent Position Check default"; }
                        if (Convert.ToInt32(Target2_Bits[2].ToString()) == 1) { Target_Report_Desc[10] = "Independent Position Check failed"; }

                        if (Convert.ToInt32(Target2_Bits[3].ToString()) == 0) { Target_Report_Desc[11] = "NOGO-bit not set"; }
                        if (Convert.ToInt32(Target2_Bits[3].ToString()) == 1) { Target_Report_Desc[11] = "NOGO-bit set"; }

                        if (Convert.ToInt32(Target2_Bits[4].ToString()) == 0) { Target_Report_Desc[12] = "CPR Validation correct"; }
                        if (Convert.ToInt32(Target2_Bits[4].ToString()) == 1) { Target_Report_Desc[12] = "CPR Validation failed"; }

                        if (Convert.ToInt32(Target2_Bits[5].ToString()) == 0) { Target_Report_Desc[13] = "LDPJ not detected"; }
                        if (Convert.ToInt32(Target2_Bits[5].ToString()) == 1) { Target_Report_Desc[13] = "LDPJ detected"; }

                        if (Convert.ToInt32(Target2_Bits[6].ToString()) == 0) { Target_Report_Desc[14] = "Range Check default"; }
                        if (Convert.ToInt32(Target2_Bits[6].ToString()) == 1) { Target_Report_Desc[14] = "Range Check failed"; }

                        contador = contador + 1;
                    }
                }
            }
            if (FSPEC[2] == "1")
            {
                // I021/161: Track Number
                string Track_Num_Bin1 = Met.Octeto_A_Bin(paquete0[contador]);
                string Track_Num_Bin2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                string Track_Num_oct1 = Track_Num_Bin1[4].ToString() + Track_Num_Bin1[5].ToString() + Track_Num_Bin1[6].ToString() + Track_Num_Bin1[7].ToString();

                string Track_Num_Bin = Track_Num_oct1 + Track_Num_Bin2;
                Track_Num = Convert.ToInt32(Track_Num_Bin, 2);
                contador = contador + 2;
            }
            if (FSPEC[3] == "1")
            {
                // I021/015: Service Identification

                Service_ID = Met.Octeto_A_Bin(paquete0[contador]);
                contador = contador + 1;
            }
            if (FSPEC[4] == "1")
            {
                // I021/071: Time of Applicability for Position
                string toa1 = Met.Octeto_A_Bin(paquete0[contador]);
                string toa2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                string toa3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                string toa_total = toa1 + toa2 + toa3;
                ToA_Position = Math.Round(Convert.ToInt32(toa_total, 2) * (1.0 / 128.0), 3);
                contador = contador + 3;
            }
            if (FSPEC[5] == "1")
            {
                // I021/130: Position in WGS-84 Co-ordinates
                string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                string octeto_total = octeto1 + octeto2 + octeto3;
                Lat_WGS_84 = Math.Round(Met.ComplementoA2(octeto_total) * (180 / Math.Pow(2, 23)), 3);
                string octeto4 = Met.Octeto_A_Bin(paquete0[contador + 3]);
                string octeto5 = Met.Octeto_A_Bin(paquete0[contador + 4]);
                string octeto6 = Met.Octeto_A_Bin(paquete0[contador + 5]);
                string octeto_total1 = octeto4 + octeto5 + octeto6;
                Lon_WGS_84 = Math.Round(Met.ComplementoA2(octeto_total1) * (180 / Math.Pow(2, 23)), 3);
                contador = contador + 6;
            }
            if (FSPEC[6] == "1")
            {
                // I021/131: High-Resolution Position in WGS-84 Co-ordinates
                string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                string octeto4 = Met.Octeto_A_Bin(paquete0[contador + 3]);
                string octeto_total = octeto1 + octeto2 + octeto3 + octeto4;
                High_Res_Lat_WGS_84 = Math.Round(Met.ComplementoA2(octeto_total) * (180 / Math.Pow(2, 30)), 3);
                string octeto5 = Met.Octeto_A_Bin(paquete0[contador + 4]);
                string octeto6 = Met.Octeto_A_Bin(paquete0[contador + 5]);
                string octeto7 = Met.Octeto_A_Bin(paquete0[contador + 6]);
                string octeto8 = Met.Octeto_A_Bin(paquete0[contador + 7]);
                string octeto_total1 = octeto5 + octeto6 + octeto7 + octeto8;
                High_Res_Lon_WGS_84 = Math.Round(Met.ComplementoA2(octeto_total1) * (180 / Math.Pow(2, 30)), 3);
                contador = contador + 8;
            }

            if (FSPEC.Count > 7)
            {
                if (FSPEC[7] == "1")
                {
                    // I021/072: Time of Applicability for Velocity
                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                    string octeto_total = octeto1 + octeto2 + octeto3;
                    ToA_Velocity = Math.Round(Met.ComplementoA2(octeto_total) * (1.0/128.0), 3);
                    contador = contador + 3;
                }
                if (FSPEC[8] == "1")
                {
                    // I021/150: Air Speed
                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                    string octeto_total = octeto1 + octeto2;
                    double num1 = 0.001;
                    if (Convert.ToInt32(octeto_total[0].ToString()) == 0)
                    {
                        double ias = Met.ComplementoA2(octeto_total.Remove(0, 1)) * (Math.Pow(2, -14));
                        // IAS
                        Air_Speed[0] = Math.Round(ias, 3);// NM/s
                    }
                    else
                    {
                        double Mach = Met.ComplementoA2(octeto_total.Remove(0, 1)) * num1;
                        // Mach Num
                        Air_Speed[1] = Math.Round(Mach, 3);
                    }
                    contador = contador + 2;
                }
                if (FSPEC[9] == "1")
                {
                    //I021/151 True Airspeed
                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    try
                    {
                        string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                        string octeto_total = octeto1 + octeto2;
                        if (Convert.ToInt32(octeto_total[0].ToString()) == 0)
                        {
                            True_Airspeed = Met.ComplementoA2(octeto_total.Remove(0, 1));// knots
                        }
                        else
                        {
                            True_Airspeed = 0;
                        }
                    }
                    catch
                    {
                        True_Airspeed = 0;
                    }
                    contador = contador + 2;
                }
                if (FSPEC[10] == "1")
                {
                    // I021/080 Target Address
                    Target_Address = Convert.ToString(paquete0[contador]) + Convert.ToString(paquete0[contador + 1]) + Convert.ToString(paquete0[contador + 2]);
                    contador = contador + 3;
                }
                if (FSPEC[11] == "1")
                {
                    // I021/073 Time of Message Reception of Position
                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                    string octeto_total = octeto1 + octeto2 + octeto3;
                    double num1 = 1;
                    double num2 = 128;
                    TMRP = Math.Round(Met.ComplementoA2(octeto_total) * (num1 / num2), 3);
                    contador = contador + 3;
                }
                if (FSPEC[12] == "1")
                {
                    // I021/074: Time of Message Reception of Position-High Precison
                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                    string octeto4 = Met.Octeto_A_Bin(paquete0[contador + 3]);
                    string octeto_total = octeto1 + octeto2 + octeto3 + octeto4;
                    // TMRP FSI
                    // first two bits defining parameters of TMRP (FSI: Full Second Indication)
                    if (Convert.ToInt32(octeto1[0].ToString()) == 1 & Convert.ToInt32(octeto1[1].ToString()) == 1)
                    {
                        TMRP_HP[0] = "Reserved";
                    }
                    if (Convert.ToInt32(octeto1[0].ToString()) == 1 & Convert.ToInt32(octeto1[1].ToString()) == 0)
                    {
                        TMRP_HP[0] = (TMRP - 1.0).ToString();
                    }
                    if (Convert.ToInt32(octeto1[0].ToString()) == 0 & Convert.ToInt32(octeto1[1].ToString()) == 1)
                    {
                        TMRP_HP[0] = (TMRP + 1.0).ToString();
                    }
                    if (Convert.ToInt32(octeto1[0].ToString()) == 0 & Convert.ToInt32(octeto1[1].ToString()) == 0)
                    {
                        TMRP_HP[0] = TMRP.ToString();
                    }
                    TMRP_HP[1] = Math.Round(Met.ComplementoA2(octeto_total.Remove(0, 2)) * Math.Pow(2, -30), 3).ToString();

                    contador = contador + 4;
                }
                if (FSPEC[13] == "1")
                {
                    // I021/075: Time of Message Reception of Velocity 
                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                    string octeto_total = octeto1 + octeto2 + octeto3;
                    double num1 = 1;
                    double num2 = 128;
                    TMRV = Math.Round(Met.ComplementoA2(octeto_total) * (num1 / num2), 3);

                    contador = contador + 3;
                }

                if (FSPEC.Count > 14)
                {
                    if (FSPEC[14] == "1")
                    {
                        //I021/076:Time of Message Reception of Velocity-High Precision
                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                        string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                        string octeto4 = Met.Octeto_A_Bin(paquete0[contador + 3]);
                        string octeto_total = octeto1 + octeto2 + octeto3 + octeto4;
                        // TMRV FSI
                        // corrections of TMRV in s
                        if (Convert.ToInt32(octeto1[0].ToString(),2) == 1 & Convert.ToInt32(octeto1[1].ToString(),2) == 1)
                        {
                            TMRV_HP[0]= "Reserved";
                        }
                        if (Convert.ToInt32(octeto1[0].ToString(),2) == 1 & Convert.ToInt32(octeto1[1].ToString(),2) == 0)
                        {
                            TMRV_HP[0] = (TMRV - 1.0).ToString();
                        }
                        if (Convert.ToInt32(octeto1[0].ToString(),2) == 0 & Convert.ToInt32(octeto1[1].ToString(),2) == 1)
                        {
                            TMRV_HP[0] = (TMRV + 1.0).ToString();
                        }
                        if (Convert.ToInt32(octeto1[0].ToString(),2) == 0 & Convert.ToInt32(octeto1[1].ToString(),2) == 0)
                        {
                            TMRV_HP[0] = TMRV.ToString();
                        }
                        TMRV_HP[1] = Math.Round(Met.ComplementoA2(octeto_total.Remove(0, 2)) * Math.Pow(2, -30), 3).ToString();

                        contador = contador + 4;
                    }
                    if (FSPEC[15] == "1")
                    {
                        //I021/140: Geometric Height
                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                        string octeto_total = octeto1 + octeto2;
                        GH = Math.Round(Met.ComplementoA2(octeto_total) * 6.25, 3);
                        
                        contador = contador + 2;
                    }
                    if (FSPEC[16] == "1")
                    {
                        // I021/090: Quality Indicators
                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        char[] NUCR_Bits = octeto1.ToCharArray();
                        string NUCR_Bin = NUCR_Bits[0].ToString() + NUCR_Bits[1].ToString() + NUCR_Bits[2].ToString();
                        Int32 NUCR = Convert.ToInt32(NUCR_Bin, 2);
                        // NUCr_or_NACv
                        Quality_Indicators[0] = "Navigation Uncertainty Category for velocity NUCr or the Navigation Accuracy Category for Velocity NACv:" + NUCR;
                        string NUC_Bin = NUCR_Bits[3].ToString() + NUCR_Bits[4].ToString() + NUCR_Bits[5].ToString() + NUCR_Bits[6].ToString();
                        Int32 NUC = Convert.ToInt32(NUC_Bin, 2);
                        // NUCp or NIC
                        Quality_Indicators[0] = "Navigation Uncertainty Category for Position NUCp or Navigation Integrity Category NIC:" + NUC;
                        
                        if (Convert.ToInt32(NUCR_Bits[7].ToString(),2) == 0)
                        { 
                            contador = contador + 1; 
                        }
                        if (Convert.ToInt32(NUCR_Bits[7].ToString(),2) == 1)
                        {
                            contador = contador + 1;
                            string octeto2 = Met.Octeto_A_Bin(paquete0[contador]);
                            char[] octeto2Bits = octeto2.ToCharArray();
                            string p = octeto2Bits[0].ToString();
                            Int32 NIC_baro_mostrar = Convert.ToInt32(p, 2);
                            // NIC baro
                            Quality_Indicators[0] = "Navigation Integrity Category for Barometric Altitude: " + NIC_baro_mostrar;
                            string SIL1 = octeto2Bits[1].ToString() + octeto2Bits[2].ToString();
                            Int32 SIL11 = Convert.ToInt32(SIL1, 2);
                            // SIL
                            Quality_Indicators[1] = "Surveillance (version 1) or Source (version 2) Integrity Level: " + SIL11;
                            string NACP1 = octeto2Bits[3].ToString() + octeto2Bits[4].ToString() + octeto2Bits[5].ToString() + octeto2Bits[6].ToString();
                            Int32 NACP11 = Convert.ToInt32(NACP1, 2);
                            // NACp
                            Quality_Indicators[2] = "Navigation Accuracy Category for Position: " + NACP11;
                           
                            if (Convert.ToInt32(octeto2Bits[7].ToString(),2) == 0) 
                            { 
                                contador = contador + 1; 
                            }
                            if (Convert.ToInt32(octeto2Bits[7].ToString(),2) == 1)
                            {
                                contador = contador + 1;
                                string octeto3 = Met.Octeto_A_Bin(paquete0[contador]);
                                char[] octeto3Bits = octeto3.ToCharArray();
                                string SIL_supp = octeto3Bits[2].ToString();
                                // SIL Supplement
                                if (SIL_supp == "0") { Quality_Indicators[3] = "measured per flight-hour"; }
                                else { Quality_Indicators[3] = "measured per sample"; }
                                string SDA1 = octeto3Bits[3].ToString() + octeto3Bits[4].ToString();
                                Int32 SDA11 = Convert.ToInt32(SDA1, 2);
                                // SDA
                                Quality_Indicators[4] = "Horizontal Position System Design Assurance Level (as defined in version 2): " + SDA11;
                                string GVA1 = octeto3Bits[5].ToString() + octeto3Bits[6].ToString();
                                Int32 GVA11 = Convert.ToInt32(GVA1, 2);
                                // GVA
                                Quality_Indicators[5] = "Geometric Altitude Accuracy: " + GVA11;

                                if (Convert.ToInt32(octeto3Bits[7].ToString(),2) == 0)
                                { 
                                    contador = contador + 1; 
                                }
                                if (Convert.ToInt32(octeto3Bits[7].ToString(),2) == 1)
                                {
                                    contador = contador + 1;
                                    string octeto4 = Met.Octeto_A_Bin(paquete0[contador]);
                                    char[] octeto4Bits = octeto4.ToCharArray();
                                    string total = octeto4Bits[0].ToString() + octeto4Bits[1].ToString() + octeto4Bits[2].ToString() + octeto4Bits[3].ToString();
                                    int PIC1 = Convert.ToInt32(total, 2);
                                    // PIC
                                    if (PIC1 == 0) { Quality_Indicators[6] = "No integrity (or > 20.0 NM)"; }
                                    if (PIC1 == 1) { Quality_Indicators[6] = "< 20.0 NM"; }
                                    if (PIC1 == 2) { Quality_Indicators[6] = "< 10.0 NM"; }
                                    if (PIC1 == 3) { Quality_Indicators[6] = "< 8.0 NM"; }
                                    if (PIC1 == 4) { Quality_Indicators[6] = "< 4.0 NM"; }
                                    if (PIC1 == 5) { Quality_Indicators[6] = "< 2.0 NM"; }
                                    if (PIC1 == 6) { Quality_Indicators[6] = "< 1.0 NM"; }
                                    if (PIC1 == 7) { Quality_Indicators[6] = "< 0.6 NM"; }
                                    if (PIC1 == 8) { Quality_Indicators[6] = "< 0.5 NM"; }
                                    if (PIC1 == 9) { Quality_Indicators[6] = "< 0.3 NM"; }
                                    if (PIC1 == 10) { Quality_Indicators[6] = "< 0.2 NM"; }
                                    if (PIC1 == 11) { Quality_Indicators[6] = "< 0.1 NM"; }
                                    if (PIC1 == 2) { Quality_Indicators[6] = "< 0.04 NM"; }
                                    if (PIC1 == 13) { Quality_Indicators[6] = "< 0.013 NM"; }
                                    if (PIC1 == 14) { Quality_Indicators[6] = "< 0.004 NM"; }
                                    if (PIC1 == 15) { Quality_Indicators[6] = "No defined"; }
                                    contador = contador + 1;
                                }
                            }
                        }
                        
                    }
                    if (FSPEC[17] == "1")
                    {
                        // I021/210: MOPS Version

                        string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                        // VNS
                        if (Convert.ToInt32(octeto[1].ToString(), 2) == 0) { MOPS[0] = "The MOPS Version is supported by the GS"; }
                        else { MOPS[0] = "The MOPS Version is not supported by the GS"; }
                        // VN
                        if (Convert.ToInt32(octeto[2].ToString() + octeto[3].ToString() + octeto[4].ToString(), 2) == 0) { MOPS[1] = "ED102/DO-260 [Ref. 8]"; }
                        if (Convert.ToInt32(octeto[2].ToString() + octeto[3].ToString() + octeto[4].ToString(), 2) == 1) { MOPS[1] = "DO-260A [Ref. 9]"; }
                        if (Convert.ToInt32(octeto[2].ToString() + octeto[3].ToString() + octeto[4].ToString(), 2) == 2) { MOPS[1] = "ED102A/DO-260B [Ref. 11]"; }
                        //LTT
                        if (Convert.ToInt32(octeto[5].ToString() + octeto[6].ToString() + octeto[7].ToString(), 2) == 0) { MOPS[2] = "Other"; }
                        if (Convert.ToInt32(octeto[5].ToString() + octeto[6].ToString() + octeto[7].ToString(), 2) == 1) { MOPS[2] = "UAT"; }
                        if (Convert.ToInt32(octeto[5].ToString() + octeto[6].ToString() + octeto[7].ToString(), 2) == 2) { MOPS[2] = "1090 ES"; }
                        if (Convert.ToInt32(octeto[5].ToString() + octeto[6].ToString() + octeto[7].ToString(), 2) == 3) { MOPS[2] = "VDL 4"; }
                        else { MOPS[2] = "Not assigned"; }
                        
                        contador = contador + 1;
                    }
                    if (FSPEC[18] == "1")
                    {
                        //I021/070: Mode 3/A Code in Octal Representation

                        try
                        {
                            string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                            string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                            string octeto_total = octeto1[4].ToString() + octeto1[5].ToString() + octeto1[6].ToString() + octeto1[7].ToString() + octeto2.ToString();
                            M3AC = "Mode-3/A reply in octal representation: " + Convert.ToString(Convert.ToInt32(Convert.ToInt32(octeto_total, 2)), 8);
                        }
                        catch
                        {
                            M3AC = null;
                        }
                        contador = contador + 2;
                    }
                    if (FSPEC[19] == "1")
                    {
                        // I021/230 Roll Angle

                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                        string octeto_total = octeto1.ToString() + octeto2.ToString();
                        Roll = Met.ComplementoA2(octeto_total) * 0.01;
                        contador = contador + 2;
                    }
                    if (FSPEC[20] == "1")
                    {
                        //I021/145: Flight Level

                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                        string octeto_total = octeto1.ToString() + octeto2.ToString();
                        FL = Math.Round(Met.ComplementoA2(octeto_total) * 0.25, 3);

                        contador = contador + 2;
                    }

                    if (FSPEC.Count > 21)
                    {
                        if (FSPEC[21] == "1")
                        {
                            //I021/152: Magnetic Heading

                            string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                            string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                            string octeto_total = octeto1.ToString() + octeto2.ToString();
                            MH = Math.Round(Met.ComplementoA2(octeto_total) * (360.0 / Math.Pow(2, 16)), 3);

                            contador = contador + 2;
                        }
                        if (FSPEC[22] == "1")
                        {
                            //I021/200: Target Status

                            string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                            // ICF
                            if (Convert.ToInt32(octeto[0].ToString(), 2) == 0) { Target_Status[0] = "No intent change active"; }
                            else { Target_Status[0] = "Intent change flag raised "; }
                            // LNAV
                            if (Convert.ToInt32(octeto[1].ToString(), 2) == 0) { Target_Status[1] = "LNAV Mode engaged"; }
                            else { Target_Status[1] = "LNAV Mode not engaged "; }
                            string octetoPS = octeto[3].ToString() + octeto[4].ToString() + octeto[5].ToString();
                            // PS
                            if (Convert.ToInt32(octetoPS, 2) == 0) { Target_Status[2] = "No emergency / not reported"; }
                            if (Convert.ToInt32(octetoPS, 2) == 1) { Target_Status[2] = "General emergency"; }
                            if (Convert.ToInt32(octetoPS, 2) == 2) { Target_Status[2] = "Lifeguard / medical emergency"; }
                            if (Convert.ToInt32(octetoPS, 2) == 3) { Target_Status[2] = "Minimum fuel"; }
                            if (Convert.ToInt32(octetoPS, 2) == 4) { Target_Status[2] = "No communications"; }
                            if (Convert.ToInt32(octetoPS, 2) == 5) { Target_Status[2] = "Unlawful interference"; }
                            if (Convert.ToInt32(octetoPS, 2) == 6) { Target_Status[2] = "Downed aircraft"; }
                            string octetoSS = octeto[6].ToString() + octeto[7].ToString();
                            //SS
                            if (Convert.ToInt32(octetoSS, 2) == 0) { Target_Status[3] = "No condition reported"; }
                            if (Convert.ToInt32(octetoSS, 2) == 1) { Target_Status[3] = "Permanent Alert (Emergency condition)"; }
                            if (Convert.ToInt32(octetoSS, 2) == 2) { Target_Status[3] = "Temporary Alert (change in Mode 3/A Code other than emergency"; }
                            if (Convert.ToInt32(octetoSS, 2) == 3) { Target_Status[3] = "SPI set"; }
                            
                            contador = contador + 1;
                        }
                        if (FSPEC[23] == "1")
                        {
                            // I021/155: Barometric Vertical Rate

                            string REbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                            if (Convert.ToInt32(REbits[0].ToString()) == 0) { RE = "Value in defined range"; }
                            else { RE = "Value exceeds defined range"; }
                            string BVRbits = REbits.Remove(0, 1);
                            BVR = Math.Round(Met.ComplementoA2(BVRbits) * 6.25, 3);

                            contador = contador + 2;
                        }
                        if (FSPEC[24] == "1")
                        {
                            //021/157: Geometric Vertical Rate

                            string REbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                            if (Convert.ToInt32(REbits[0].ToString()) == 0) { RE = "Value in defined range"; }
                            else { RE = "Value exceeds defined range"; }
                            string BVRbits = REbits.Remove(0, 1);
                            GVR = Math.Round(Met.ComplementoA2(BVRbits) * 6.25, 3);

                            contador = contador + 2;
                        }
                        if (FSPEC[25] == "1")
                        {
                            //I021/160: Airborne Ground Vector

                            string GSbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                            if (Convert.ToInt32(GSbits[0].ToString()) == 0) { RE = "Value in defined range"; }
                            else { RE = "Value exceeds defined range"; }
                            string GSBITS = GSbits.Remove(0, 1);
                            GS = Math.Round(Met.ComplementoA2(GSBITS) * 0.22, 3);
                            string TAbits = Met.Octeto_A_Bin(paquete0[contador + 2]) + Met.Octeto_A_Bin(paquete0[contador + 3]);
                            TA = Math.Round(Met.ComplementoA2(TAbits) * (360.0 / Math.Pow(2, 16)), 3);
                            contador = contador + 4;
                        }
                        if (FSPEC[26] == "1")
                        {
                            // I021/165: Track Angle Rate

                            string TARbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                            string TARBITS = TARbits.Remove(0, 6);
                            TAR = Math.Round(Met.ComplementoA2(TARBITS) * (1.0 / 32.0), 3);
                            contador = contador + 2;
                        }
                        if (FSPEC[27] == "1")
                        {
                            // I021/077 Time of Report Transmission

                            string ToARTbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]) + Met.Octeto_A_Bin(paquete0[contador + 2]);
                            ToART = Math.Round(Met.ComplementoA2(ToARTbits) * (1.0 / 128.0), 3);
                            contador = contador + 3;

                        }

                        if (FSPEC.Count > 28)
                        {
                            if (FSPEC[28] == "1")
                            {
                                //I021/170 Target Identification (found in table 3.8 annex 10 ICAO) see 3.1.2.9
                                string octetototal = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]) + Met.Octeto_A_Bin(paquete0[contador + 2]) + Met.Octeto_A_Bin(paquete0[contador + 3]) + Met.Octeto_A_Bin(paquete0[contador + 4]) + Met.Octeto_A_Bin(paquete0[contador + 5]);
                                Target_ID = Met.Compare_bits(octetototal);
                                contador = contador + 6;
                            }
                            if (FSPEC[29] == "1")
                            {
                                // I021/020 Emitter Category
                                string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                if (Convert.ToInt32(octeto, 2) == 0) { ECAT = "No ADS-B Emitter Category Information"; }
                                if (Convert.ToInt32(octeto, 2) == 1) { ECAT = "light aircraft <= 15500 lbs"; }
                                if (Convert.ToInt32(octeto, 2) == 2) { ECAT = "15500 lbs < small aircraft < 75000 lbs"; }
                                if (Convert.ToInt32(octeto, 2) == 3) { ECAT = "75000 lbs < medium a/c < 300000 lbs"; }
                                if (Convert.ToInt32(octeto, 2) == 4) { ECAT = "High Vortex Large"; }
                                if (Convert.ToInt32(octeto, 2) == 5) { ECAT = "300000 lbs <= heavy aircraft"; }
                                if (Convert.ToInt32(octeto, 2) == 6) { ECAT = "highly manoeuvrable (5g acceleration capability) and high speed (>400knots cruise)"; }
                                if (Convert.ToInt32(octeto, 2) == 7) { ECAT = "reserved"; }
                                if (Convert.ToInt32(octeto, 2) == 8) { ECAT = "reserved"; }
                                if (Convert.ToInt32(octeto, 2) == 9) { ECAT = "reserved"; }
                                if (Convert.ToInt32(octeto, 2) == 10) { ECAT = "rotocraft"; }
                                if (Convert.ToInt32(octeto, 2) == 11) { ECAT = "glider / sailplane"; }
                                if (Convert.ToInt32(octeto, 2) == 12) { ECAT = "ighter-than-air"; }
                                if (Convert.ToInt32(octeto, 2) == 13) { ECAT = "unmanned aerial vehicle"; }
                                if (Convert.ToInt32(octeto, 2) == 14) { ECAT = "space / transatmospheric vehicle"; }
                                if (Convert.ToInt32(octeto, 2) == 15) { ECAT = "ultralight / handglider / paraglider"; }
                                if (Convert.ToInt32(octeto, 2) == 16) { ECAT = "parachutist / skydiver"; }
                                if (Convert.ToInt32(octeto, 2) == 17) { ECAT = "reserved"; }
                                if (Convert.ToInt32(octeto, 2) == 18) { ECAT = "reserved"; }
                                if (Convert.ToInt32(octeto, 2) == 19) { ECAT = "reserved"; }
                                if (Convert.ToInt32(octeto, 2) == 20) { ECAT = "surface emergency vehicle"; }
                                if (Convert.ToInt32(octeto, 2) == 21) { ECAT = "surface service vehicle"; }
                                if (Convert.ToInt32(octeto, 2) == 22) { ECAT = "fixed ground or tethered obstruction"; }
                                if (Convert.ToInt32(octeto, 2) == 23) { ECAT = "cluster obstacle"; }
                                if (Convert.ToInt32(octeto, 2) == 24) { ECAT = "line obstacle"; }
                                contador = contador + 1;
                            }
                            if (FSPEC[30] == "1")
                            {
                                // I021/220 Met Information

                                string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                // Wind Speed in knots
                                if (octeto[0].ToString() == "0") { Met_Report[0] = "No Wind Speed reported"; }
                                else
                                {
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                                    string windspeedbits = octeto2 + octeto3;
                                    Met_Report[0] = Convert.ToString(Met.ComplementoA2(windspeedbits));
                                    
                                    contador = contador + 2;
                                }
                                // Wind Direction in degrees
                                if (octeto[1].ToString() == "0") { Met_Report[1] = "No Wind Speed reported"; }
                                else
                                {
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                                    string windspeedbits = octeto2 + octeto3;
                                    Met_Report[1] = Convert.ToString(Met.ComplementoA2(windspeedbits));
                                    
                                    contador = contador + 2;
                                }
                                // Temperature in Celsius
                                if (octeto[2].ToString() == "0") { Met_Report[2] = "No Temperature reported"; }
                                else
                                {
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                                    string windspeedbits = octeto2 + octeto3;
                                    Met_Report[2] = Convert.ToString(Math.Round((Met.ComplementoA2(windspeedbits) * 0.25), 2));
                                    
                                    contador = contador + 2;
                                }
                                // Turbulence
                                if (octeto[3].ToString() == "0") { Met_Report[3] = "No Turbulence reported"; }
                                else
                                {
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    Met_Report[3] = Convert.ToString(Convert.ToInt32(Met.ComplementoA2(octeto2)), 2);
                                    
                                    contador = contador + 1;
                                }
                                contador = contador + 1;
                            }
                            if (FSPEC[31] == "1")
                            {
                                //021/146 Selected Altitude

                                string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                string total = octeto + octeto2;
                                if (total[0].ToString() == "0") { SAS = "No source information provided"; }
                                else { SAS = "Source information provided"; }
                                string sourcebits = total[1].ToString() + total[2].ToString();
                                if (sourcebits == "00") { Source = "Unknown"; }
                                if (sourcebits == "01") { Source = "Aircraft Altitude (Holding Altitude"; }
                                if (sourcebits == "10") { Source = "MCP/FCU Selected Altitude"; }
                                if (sourcebits == "11") { Source = "FMS Selected Altitude"; }
                                string altitudebits = total.Remove(0, 2);
                                SA = Math.Round(Met.ComplementoA2(altitudebits) * 25.0, 3);
                                contador = contador + 2;
                            }
                            if (FSPEC[32] == "1")
                            {
                                //I021/148 Final State Selected Altitude

                                string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                string total = octeto + octeto2;
                                if (total[0].ToString() == "0") { MV = "Not active or unknown"; }
                                else { MV = "Active"; }
                                if (total[1].ToString() == "0") { AH = "Not active or unknown"; }
                                else { AH = "Active"; }
                                if (total[2].ToString() == "0") { AM = "Not active or unknown"; }
                                else { AM = "Active"; }
                                string altitudebits = total.Remove(0, 2);
                                FSSA = Math.Round(Met.ComplementoA2(altitudebits) * 25.0, 3);
                                contador = contador + 2;
                            }
                            if (FSPEC[33] == "1")
                            {
                                //I021/110 Trajectory Intent

                                string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                if (octeto[0].ToString() == "0") { Trajectory_Intent[0] = "Abscence of Subfield #1"; }
                                else
                                {
                                    contador = contador + 1;
                                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                                    // NVB
                                    if (octeto1[1].ToString() == "0") { Trajectory_Intent[1] = "Trajectory Intent Data is valid"; }
                                    else { Trajectory_Intent[1] = "Trajectory Intent Data is not valid"; }
                                    // NAV
                                    if (octeto1[0].ToString() == "1") { Trajectory_Intent[2] = "Trajectory Intent Data is not available for this aircraft"; }
                                    else
                                    {
                                        Trajectory_Intent[2] = "Trajectory Intent Data available";
                                        contador = contador + 1;
                                        string octeto2 = Met.Octeto_A_Bin(paquete0[contador]);
                                        //REP
                                        Trajectory_Intent[3] = Convert.ToInt32(octeto2, 2).ToString();
                                        contador = contador + 1;
                                        string octeto3 = Met.Octeto_A_Bin(paquete0[contador]);
                                        //TCA
                                        if (octeto3[0].ToString() == "0") { Trajectory_Intent[4] = "TCP number available"; }
                                        else { Trajectory_Intent[4] = "TCP number not available"; }
                                        // NC
                                        if (octeto3[1].ToString() == "0") { Trajectory_Intent[5] = "TCP compliance"; }
                                        else { Trajectory_Intent[5] = "TCP non-compliance"; }
                                        // TCP num
                                        Trajectory_Intent[6] = Convert.ToInt32(octeto3.Remove(0, 1), 2).ToString();
                                        string octetoaltitude = Met.Octeto_A_Bin(paquete0[contador + 1]) + Met.Octeto_A_Bin(paquete0[contador + 2]);
                                        // Alt in ft
                                        Trajectory_Intent[9] = Math.Round(Convert.ToInt32(octetoaltitude, 2) * 10.0, 3).ToString();
                                        string octetolatitude = Met.Octeto_A_Bin(paquete0[contador + 3]) + Met.Octeto_A_Bin(paquete0[contador + 4]) + Met.Octeto_A_Bin(paquete0[contador + 5]);
                                        // Lat TID in degrees
                                        Trajectory_Intent[7] = Math.Round(Convert.ToInt32(octetolatitude, 2) * (180.0 / Math.Pow(2, 23)), 3).ToString();
                                        string octetolong = Met.Octeto_A_Bin(paquete0[contador + 6]) + Met.Octeto_A_Bin(paquete0[contador + 7]) + Met.Octeto_A_Bin(paquete0[contador + 8]);
                                        // Lon TID in degrees
                                        Trajectory_Intent[8] = Math.Round(Convert.ToInt32(octetolong, 2) * (180.0 / Math.Pow(2, 23)), 3).ToString();
                                        string octeto11 = Met.Octeto_A_Bin(paquete0[contador + 9]);
                                        string pointtyopebits = octeto11[0].ToString() + octeto11[1].ToString() + octeto11[2].ToString() + octeto11[3].ToString();
                                        // Point Type
                                        if (Convert.ToInt32(pointtyopebits, 2) == 0) { Trajectory_Intent[10] = "Unknown"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 1) { Trajectory_Intent[10] = "Fly by waypoint (LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 2) { Trajectory_Intent[10] = "Fly over waypoint (LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 3) { Trajectory_Intent[10] = "Hold pattern (LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 4) { Trajectory_Intent[10] = "Procedure hold(LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 5) { Trajectory_Intent[10] = "Procedure turn (LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 6) { Trajectory_Intent[10] = "RF leg (LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 7) { Trajectory_Intent[10] = "Top of climb (VT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 8) { Trajectory_Intent[10] = "Top of descent (VT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 9) { Trajectory_Intent[10] = "Start of level (VT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 10) { Trajectory_Intent[10] = "Cross-over altitude (VT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 11) { Trajectory_Intent[10] = "Transition altitude (VT)"; }
                                        string TDbits = octeto11[4].ToString() + octeto11[5].ToString();
                                        // TD
                                        if (TDbits == "00") { Trajectory_Intent[11] = "N/A"; }
                                        if (TDbits == "01") { Trajectory_Intent[11] = "Turn right"; }
                                        if (TDbits == "10") { Trajectory_Intent[11] = "Turn left"; }
                                        if (TDbits == "11") { Trajectory_Intent[11] = "No turn"; }
                                        // TRA
                                        if (octeto11[6].ToString() == "0") { Trajectory_Intent[12] = "TTR not available"; }
                                        if (octeto11[6].ToString() == "1") { Trajectory_Intent[12] = "TTR available"; }
                                        // TOA
                                        if (octeto11[7].ToString() == "0") { Trajectory_Intent[13] = "TOV available"; }
                                        if (octeto11[7].ToString() == "1") { Trajectory_Intent[13] = "TOV not available"; }
                                        string TOVbits = Met.Octeto_A_Bin(paquete0[contador + 10]) + Met.Octeto_A_Bin(paquete0[contador + 11]) + Met.Octeto_A_Bin(paquete0[contador + 12]);
                                        // TOV in seconds
                                        Trajectory_Intent[14] = Convert.ToInt32(TOVbits, 2).ToString();
                                        string TTRbits = Met.Octeto_A_Bin(paquete0[contador + 13]) + Met.Octeto_A_Bin(paquete0[contador + 14]);
                                        // TTR in Nm
                                        Trajectory_Intent[15] = Math.Round(Convert.ToInt32(TTRbits, 2) * 0.01, 3).ToString();
                                        contador = contador + 14;
                                    }
                                }
                                contador = contador + 1;
                            }
                            if (FSPEC[34] == "1")
                            {
                                //I021/016 Service Management

                                string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                RP = Math.Round(Convert.ToInt32(octeto, 2) * 0.5, 3);
                                contador = contador + 1;
                            }

                            if (FSPEC.Count > 35)
                            {
                                if (FSPEC[35] == "1")
                                {
                                    //I021/008 Aircraft Operational Status

                                    string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                    // RA
                                    if (octeto[0].ToString() == "0") { Op_Status[0] = "TCAS II or ACAS RA not active"; }
                                    else { Op_Status[0] = "TCAS RA active"; }
                                    string tcbits = octeto[1].ToString() + octeto[2].ToString();
                                    //TC
                                    if (Convert.ToInt32(tcbits, 2) == 0) { Op_Status[1] = "no capability for Trajectory Change Reports"; }
                                    if (Convert.ToInt32(tcbits, 2) == 1) { Op_Status[1] = "support for TC+0 reports only"; }
                                    if (Convert.ToInt32(tcbits, 2) == 2) { Op_Status[1] = "support for multiple TC reports"; }
                                    if (Convert.ToInt32(tcbits, 2) == 3) { Op_Status[1] = "Reserved"; }
                                    //TS
                                    if (octeto[3].ToString() == "0") { Op_Status[2] = "no capability to support Target State Reports"; }
                                    else { Op_Status[2] = "capable of supporting target State Reports"; }
                                    // ARV
                                    if (octeto[4].ToString() == "0") { Op_Status[3] = "no capability to generate ARV-reports"; }
                                    else { Op_Status[3] = "capable of generate ARV-reports"; }
                                    //CDTIA
                                    if (octeto[5].ToString() == "0") { Op_Status[4] = "CDTI not operational"; }
                                    else { Op_Status[4] = "CDTI operational"; }
                                    //NotTACS
                                    if (octeto[6].ToString() == "0") { Op_Status[5] = "TCAS operational"; }
                                    else { Op_Status[5] = "TCAS not operational"; }
                                    //SingAnt
                                    if (octeto[7].ToString() == "0") { Op_Status[6] = "Antenna Diversity"; }
                                    else { Op_Status[6] = "Single Antenna only "; }
                                    
                                    contador = contador + 1;
                                }
                                if (FSPEC[36] == "1")
                                {
                                    //I021/271 Surface Capabilities and Characteristics

                                    string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                    if (octeto[2].ToString() == "0") { POA = "Position transmitted is not ADS-B position reference point"; }
                                    else { POA = "Position transmitted is the ADS-B position reference point"; }
                                    if (octeto[3].ToString() == "0") { CDTIS = "CDTI not operational"; }
                                    else { CDTIS = "CDTI operational"; }
                                    if (octeto[4].ToString() == "0") { B2LOW = ">=70 Watts"; }
                                    else { B2LOW = "<70 Watts"; }
                                    if (octeto[5].ToString() == "0") { RAS = "Aircraft not receiving ATC-services"; }
                                    else { RAS = "Aircraft receiving ATC services"; }
                                    if (octeto[6].ToString() == "0") { IDENT = "IDENT switch not active"; }
                                    else { IDENT = "IDENT switch active"; }
                                    if (octeto[7].ToString() == "0") { contador = contador + 1; }
                                    else
                                    {
                                        string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                        string lenwidthbits = octeto2[4].ToString() + octeto2[5].ToString() + octeto2[6].ToString() + octeto2[7].ToString();
                                        if (Convert.ToInt32(lenwidthbits, 2) == 0) { LenWidth = "Length < 15 m ; Width < 11.5 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 1) { LenWidth = "Length < 15 m ; Width < 23 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 2) { LenWidth = "Length < 25 m ; Width < 28.5 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 3) { LenWidth = "Length < 25 m ; Width < 34 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 4) { LenWidth = "Length < 35 m ; Width < 33 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 5) { LenWidth = "Length < 35 m ; Width < 38 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 6) { LenWidth = "Length < 45 m ; Width < 39.5 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 7) { LenWidth = "Length < 45 m ; Width < 45 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 8) { LenWidth = "Length < 55 m ; Width < 45 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 9) { LenWidth = "Length < 55 m ; Width < 52 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 10) { LenWidth = "Length < 65 m ; Width < 59.5 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 11) { LenWidth = "Length < 65 m ; Width < 67 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 12) { LenWidth = "Length < 75 m ; Width < 72.5 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 13) { LenWidth = "Length < 75 m ; Width < 80 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 14) { LenWidth = "Length < 85 m ; Width < 80 m"; }
                                        if (Convert.ToInt32(lenwidthbits, 2) == 15) { LenWidth = "Length < 85 m ; Width > 80 m"; }
                                        
                                        contador = contador + 1;
                                    }
                                }
                                if (FSPEC[37] == "1")
                                {
                                    //I021/132 Message Amplitude

                                    string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                    MAM = Met.ComplementoA2(octeto);
                                    contador = contador + 1;
                                }
                                if (FSPEC[38] == "1")
                                {
                                    //I021/250 Mode S MB Data

                                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                                    // Rep Mode SMB Data
                                    Mode_S[0] = Convert.ToInt32(octeto1, 2);
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                                    string octeto4 = Met.Octeto_A_Bin(paquete0[contador + 3]);
                                    string octeto5 = Met.Octeto_A_Bin(paquete0[contador + 4]);
                                    string octeto6 = Met.Octeto_A_Bin(paquete0[contador + 5]);
                                    string octeto7 = Met.Octeto_A_Bin(paquete0[contador + 6]);
                                    string octeto8 = Met.Octeto_A_Bin(paquete0[contador + 7]);
                                    // MB Data
                                    Mode_S[1] = Convert.ToInt32(octeto2.ToString() + octeto3.ToString() + octeto4.ToString() + octeto5.ToString() + octeto6.ToString() + octeto7.ToString() + octeto8.ToString(), 2);
                                    string octeto9 = Met.Octeto_A_Bin(paquete0[contador + 8]);
                                    // BDS 1
                                    Mode_S[2] = Convert.ToInt32(octeto9.Remove(4, 4), 2);
                                    // BDS 2
                                    Mode_S[3] = Convert.ToInt32(octeto9.Remove(0, 4), 2);
                                    
                                    contador = contador + 9;
                                }
                                if (FSPEC[39] == "1")
                                {
                                    // I021/260 ACAS Resolution Advisory Report

                                    string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                    string TYP_BITS = octeto.Remove(5, 3);
                                    TYP = Convert.ToInt32(TYP_BITS, 2);
                                    string STYP_BITS = octeto.Remove(0, 5);
                                    STYP = Convert.ToInt32(STYP_BITS, 2);
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                                    ARA = Convert.ToInt32(octeto2.ToString() + octeto3.ToString().Remove(6, 2), 2);
                                    string octeto4 = Met.Octeto_A_Bin(paquete0[contador + 3]);
                                    string RAC_BITS = octeto3[6].ToString() + octeto3[7].ToString() + octeto4[0].ToString() + octeto4[1].ToString();
                                    RAC = Convert.ToInt32(RAC_BITS, 2);
                                    RAT = Convert.ToInt32(octeto4[2].ToString(), 2);
                                    MTE = Convert.ToInt32(octeto4[3].ToString(), 2);
                                    TTI = Convert.ToInt32(octeto4[4].ToString() + octeto4[5].ToString(), 2);
                                    string octeto5 = Met.Octeto_A_Bin(paquete0[contador + 4]);
                                    string octeto6 = Met.Octeto_A_Bin(paquete0[contador + 5]);
                                    string octeto7 = Met.Octeto_A_Bin(paquete0[contador + 6]);
                                    TID_ACAS = Convert.ToInt32(octeto4[6].ToString() + octeto4[7].ToString() + octeto5.ToString() + octeto6.ToString() + octeto7.ToString(), 2);
                                    contador = contador + 7;
                                }
                                if (FSPEC[40] == "1")
                                {
                                    //I021/400 Receiver ID

                                    string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                    RID = Convert.ToInt32(octeto, 2);
                                    contador = contador + 1;
                                }
                                if (FSPEC[41] == "1")
                                {
                                    //I021/295 Data Ages

                                    string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                    Int32 contadordeoctetos = contador;
                                    // AOS age
                                    if (octeto[0].ToString() == "1")
                                    {
                                        contador = contador + 4;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        Data_Ages[0] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                    }
                                    // TRD age
                                    if (octeto[1].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string TRD_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        Data_Ages[1] = Math.Round(Convert.ToInt32(TRD_BITS, 2) * 0.1, 3);
                                    }
                                    // Mode 3A age
                                    if (octeto[2].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        Data_Ages[2] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                    }
                                    // QI age
                                    if (octeto[3].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        Data_Ages[3] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                    }
                                    // TI age
                                    if (octeto[4].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        Data_Ages[4] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                    }
                                    // MAM age
                                    if (octeto[5].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        Data_Ages[5] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                    }
                                    // GH age
                                    if (octeto[6].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        Data_Ages[6] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                    }
                                    if (octeto[7].ToString() == "1")
                                    {
                                        string octeto2 = Met.Octeto_A_Bin(paquete0[contadordeoctetos + 1]);
                                        // FL age
                                        if (octeto2[0].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            Data_Ages[7] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                        }
                                        // ISA age
                                        if (octeto2[1].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            Data_Ages[8] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                        }
                                        // FSA age
                                        if (octeto2[2].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            Data_Ages[9] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                        }
                                        // AS age
                                        if (octeto2[3].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            Data_Ages[10] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                        }
                                        // TAS age
                                        if (octeto2[4].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            Data_Ages[11] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                        }
                                        // MH age
                                        if (octeto2[5].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            Data_Ages[12] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                        }
                                        // BVR age
                                        if (octeto2[6].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            Data_Ages[13] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                        }
                                        if (octeto2[7].ToString() == "1")
                                        {
                                            string octeto3 = Met.Octeto_A_Bin(paquete0[contadordeoctetos + 2]);
                                            // GVR age
                                            if (octeto3[0].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                Data_Ages[14] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                            }
                                            // GV age
                                            if (octeto3[1].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                Data_Ages[15] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                            }
                                            // TAR age
                                            if (octeto3[2].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                Data_Ages[16] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                            }
                                            // Target ID age
                                            if (octeto3[3].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                Data_Ages[17] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                            }
                                            // TS age
                                            if (octeto3[4].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                Data_Ages[18] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                            }
                                            // MET age
                                            if (octeto3[5].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                Data_Ages[19] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                            }
                                            // ROA age
                                            if (octeto3[6].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                Data_Ages[20] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                            }
                                            if (octeto3[7].ToString() == "1")
                                            {
                                                string octeto4 = Met.Octeto_A_Bin(paquete0[contadordeoctetos + 3]);
                                                // ARA age
                                                if (octeto4[0].ToString() == "1")
                                                {
                                                    contador = contador + 1;
                                                    string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                    Data_Ages[21] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                                }
                                                // SCC age
                                                if (octeto4[1].ToString() == "1")
                                                {
                                                    contador = contador + 1;
                                                    string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                    Data_Ages[22] = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 3);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else { }
                        }
                        else { }
                    }
                    else { }
                }
                else { }
            }
            else { }
        }

        // getters for flight class
        public double getSIC21()
        {
            return Data_Source_ID_SIC;
        }
        public double getSAC21()
        {
            return Data_Source_ID_SAC;
        }
        public double getTOD21()
        {
            return ToART;
        }
        public double getLAT21()
        {
            return High_Res_Lat_WGS_84;
        }
        public double getLON21()
        {
            return High_Res_Lon_WGS_84;
        }
        public double getTrackNum21()
        {
            return Track_Num;
        }
        public string getTargetAddress21()
        {
            return Target_Address;
        }
        public string getTargetID21()
        {
            return Target_ID;
        }
        public double getFL21()
        {
            return FL;
        }
        //file that does not report Time of Asterix Report Time will report time of message report for position (ADSB-v023)
        public double getTOD21ADSB23()
        {
            return ToA_Position;
        }
    }
}

