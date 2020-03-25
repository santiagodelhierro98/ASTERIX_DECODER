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

        public string Data_Source_ID;
        public string Target_Report_Desc;
        public string Track_Num;
        public string Service_ID;
        public double ToA_Position; // seconds, ToA=Time of Aplicability
        public double Lat_WGS_84; //in degrees
        public double Lon_WGS_84; //in degrees
        public double High_Res_Lat_WGS_84; //high resolution Lat WGS 84 in degrees
        public double High_Res_Lon_WGS_84; //high resolution Lon WGS 84 in degrees
        public double ToA_Velocity; // seconds
        public string Air_Speed;// ias and mach
        public string True_Airspeed; //kt
        public string Target_Address;//Target address (emitter identifier) assigned uniquely to each target. en hexa
        public double TMRP; // s
        public double TMRP_HP; // high precision TMRP
        public string TMRP_FSI; // first two bits defining parameters of TMRP (FSI: Full Second Indication)
        public double TMRV; // s
        public double TMRV_HP;// high precision TMRV
        public string TMRV_FSI; // corrections of TMRV in s
        public double GH; // ft
        public string NUCr_or_NACv;
        public string NUCp_or_NIC;
        public string NIC_baro;
        public string SIL;
        public string NACp;
        public string SIL_Supplement;
        public string SDA;
        public string GVA;
        public string PIC;
        public string VNS;
        public string VN;
        public string LTT;
        public string M3AC; //Mode 3/A Code
        public double Roll; // roll angle in degrees
        public double FL; //flight level in feet
        public double MH; //Magnetic heading in degrees
        public string ICF;
        public string LNAV;
        public string PS;
        public string SS;
        public string RE;
        public double BVR; // Barometric Vertical Rate in feet/minute
        public double GVR; // Geometric Vertical Rate in feet/minute
        public double GS; // ground speed in kt
        public double TA; // Track angle in degrees
        public double TAR; // In degees/s
        public double ToART; // Time of ASTERIX Report Transmission in s
        public string Target_ID;
        public string ECAT;
        public string WS; //wind speed in knots
        public string WD; //wind direction in degrees
        public string TMP; //temperature in celcius
        public string TRB; //turbulence
        public string SAS;
        public string Source;
        public double SA; //selected altitude in ft
        public string MV;
        public string AH;
        public string AM;
        public double FSSA; //final state selected altitde in ft
        public string TIS;
        public string TID;
        public string NAV;
        public string NVB;
        public double REP;
        public string TCA;
        public string NC;
        public double TCP_Num;
        public double Alt_TID; // altitude trajectory intent data in ft
        public double Lat_TID; // latitude trajectory intent data in degrees
        public double Lon_TID; // longitude trajectory intent data in degrees
        public string Point_Type;
        public string TD; 
        public string TRA;
        public string TOA;
        public double TOV; //in seconds
        public double TTR; // in Nm
        public double RP; //in seconds
        public string RA;
        public string TC;
        public string TS;
        public string ARV;
        public string CDTIA;
        public string NotTCAS;
        public string SingAnt;
        public string POA;
        public string CDTIS;
        public string B2LOW;
        public string RAS;
        public string IDENT;
        public string LenWidth; //length and width of the aircraft in meters
        public double MAM; //in dBm;
        public double REP_MODESMBDAATA;
        public double MB_data;
        public double BDS1;
        public double BDS2;
        public double TYP;
        public double STYP;
        public double ARA;
        public double RAC;
        public double RAT;
        public double MTE;
        public double TTI;
        public double TID_ACAS;
        public double RID;
        public double AOS_age;
        public double TRD_age;
        public double M3A_age;
        public double QI_age;
        public double TI_age;
        public double MAM_age;
        public double GH_age;
        public double FL_age;
        public double ISA_age;
        public double FSA_age;
        public double AS_age;
        public double TAS_age;
        public double MH_age;
        public double BVR_age;
        public double GVR_age;
        public double GV_age;
        public double TAR_age;
        public double Target_ID_age;
        public double TS_age;
        public double MET_age;
        public double ROA_age;
        public double ARA_age;
        public double SCC_age;
       
        public void Decode21(string[] paquete)
        {
            Metodos Met = new Metodos();
            int longitud = Met.Longitud_Paquete(paquete);
            string[] paquete0 = new string[longitud];
            for (int i = 0; i < longitud; i++)
            {

                paquete0[i] = Met.Poner_Zeros_Delante(paquete[i]);
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
                string SAC = (Convert.ToInt32(SAC_Bin, 2)).ToString();
                string SIC = (Convert.ToInt32(SIC_Bin, 2)).ToString();

                //decodificamos , solo utilizaremos estos dos 
                Data_Source_ID = "SIC: " + SIC + "/ SAC: " + SAC;
                if (SAC == "0" && SIC == "107")
                    Data_Source_ID = "Data flow local to the airport: Barcelona - LEBL ; SIC: 107 , SAC: 0";
                if (SAC == "0" && SIC == "7")
                    Data_Source_ID = "Data flow local to the airport: Barcelona - LEBL ; SIC: 7 , SAC: 0";

                contador = contador + 2;
            }
            if (FSPEC[1] == "1")
            {
                // I021/040 : Target Report Descriptor
                string TargetReport_Bin = Met.Octeto_A_Bin(paquete0[contador]);
                char[] Target_Bits = TargetReport_Bin.ToCharArray();


                string ATP_Bin = Target_Bits[0].ToString() + Target_Bits[1].ToString() + Target_Bits[2].ToString();
                Int32 ATP = Convert.ToInt32(ATP_Bin, 2);
                if (ATP == 0) { Target_Report_Desc = "24-Bit ICAO address"; }
                if (ATP == 1) { Target_Report_Desc = "Duplicate address"; }
                if (ATP == 2) { Target_Report_Desc = "Surface vehicle address"; }
                if (ATP == 3) { Target_Report_Desc = "Anonymous address"; }
                if (ATP == 4 || ATP == 5 || ATP == 6 || ATP == 7) { Target_Report_Desc = "Reserved for future use"; }

                string ARC_Bin = Target_Bits[3].ToString() + Target_Bits[4].ToString();
                Int32 ARC = Convert.ToInt32(ARC_Bin, 2);
                if (ARC == 0) { Target_Report_Desc = Target_Report_Desc + "/25 ft"; }
                if (ARC == 1) { Target_Report_Desc = Target_Report_Desc + "/100 ft"; }
                if (ARC == 2) { Target_Report_Desc = Target_Report_Desc + "/Unknown"; }
                if (ARC == 3) { Target_Report_Desc = Target_Report_Desc + "/Invalid"; }

                if (Convert.ToInt32(Target_Bits[5].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/Default"; }
                if (Convert.ToInt32(Target_Bits[5].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/Range Check passed, CPR Validation pending"; }

                if (Convert.ToInt32(Target_Bits[6].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/Report from target transponder"; }
                if (Convert.ToInt32(Target_Bits[6].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/Report from field monitor (fixed transponder)"; }

                if (Convert.ToInt32(Target_Bits[7].ToString()) == 0) { contador = contador + 1; }
                if (Convert.ToInt32(Target_Bits[7].ToString()) == 1)
                {
                    //I021/040 - First Extension
                    contador = contador + 1;
                    string TargetReport1_Bin = Met.Octeto_A_Bin(paquete0[contador]);
                    char[] Target1_Bits = TargetReport1_Bin.ToCharArray();

                    if (Convert.ToInt32(Target1_Bits[0].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/No Differential Correction(ADS-B)"; }
                    if (Convert.ToInt32(Target1_Bits[0].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/Differential correction (ADS-B)"; }

                    if (Convert.ToInt32(Target1_Bits[1].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/Ground Bit not set"; }
                    if (Convert.ToInt32(Target1_Bits[1].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/Ground Bit set"; }

                    if (Convert.ToInt32(Target1_Bits[2].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/Actual target report"; }
                    if (Convert.ToInt32(Target1_Bits[2].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/Simulated target report"; }

                    if (Convert.ToInt32(Target1_Bits[3].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/Default"; }
                    if (Convert.ToInt32(Target1_Bits[3].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/Test Target"; }

                    if (Convert.ToInt32(Target1_Bits[4].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/Equipment capable to provide Selected Altitude"; }
                    if (Convert.ToInt32(Target1_Bits[4].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/Equipment not capable to provide Selected Altitude"; }

                    string CL_Bin = Target1_Bits[5].ToString() + Target1_Bits[6].ToString();
                    Int32 CL = Convert.ToInt32(ARC_Bin, 2);
                    if (CL == 0) { Target_Report_Desc = Target_Report_Desc + "/Report Valid"; }
                    if (CL == 1) { Target_Report_Desc = Target_Report_Desc + "/Report suspect"; }
                    if (CL == 2) { Target_Report_Desc = Target_Report_Desc + "/No information"; }
                    if (CL == 3) { Target_Report_Desc = Target_Report_Desc + "/Reserved for future use"; }

                    if (Convert.ToInt32(Target1_Bits[7].ToString()) == 0) { contador = contador + 1; }
                    if (Convert.ToInt32(Target1_Bits[7].ToString()) == 1)
                    {
                        //I021/040 - Second Extension : Error Conditions
                        contador = contador + 1;
                        string TargetReport2_Bin = Met.Octeto_A_Bin(paquete0[contador]);
                        char[] Target2_Bits = TargetReport2_Bin.ToCharArray();

                        if (Convert.ToInt32(Target2_Bits[2].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/Independent Position Check default"; }
                        if (Convert.ToInt32(Target2_Bits[2].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/Independent Position Check failed"; }

                        if (Convert.ToInt32(Target2_Bits[3].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/NOGO-bit not set"; }
                        if (Convert.ToInt32(Target2_Bits[3].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/NOGO-bit set"; }

                        if (Convert.ToInt32(Target2_Bits[4].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/CPR Validation correct"; }
                        if (Convert.ToInt32(Target2_Bits[4].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/CPR Validation failed"; }

                        if (Convert.ToInt32(Target2_Bits[5].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/LDPJ not detected"; }
                        if (Convert.ToInt32(Target2_Bits[5].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/LDPJ detected"; }

                        if (Convert.ToInt32(Target2_Bits[6].ToString()) == 0) { Target_Report_Desc = Target_Report_Desc + "/Range Check default"; }
                        if (Convert.ToInt32(Target2_Bits[6].ToString()) == 1) { Target_Report_Desc = Target_Report_Desc + "/Range Check failed"; }

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
                Track_Num = Convert.ToInt32(Track_Num_Bin, 2).ToString();
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
                string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                string octeto_total = octeto1 + octeto2 + octeto3;
                ToA_Position = Met.ComplementoA2(octeto_total) * (1 / 128);
                contador = contador + 3;


            }
            if (FSPEC[5] == "1")
            {
                // I021/130: Position in WGS-84 Co-ordinates
                string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                string octeto_total = octeto1 + octeto2 + octeto3;
                Lat_WGS_84 = Met.ComplementoA2(octeto_total) * (180 / Math.Pow(2, 23));
                string octeto4 = Met.Octeto_A_Bin(paquete0[contador + 3]);
                string octeto5 = Met.Octeto_A_Bin(paquete0[contador + 4]);
                string octeto6 = Met.Octeto_A_Bin(paquete0[contador + 5]);
                string octeto_total1 = octeto4 + octeto5 + octeto6;
                Lon_WGS_84 = Met.ComplementoA2(octeto_total1) * (180 / Math.Pow(2, 23));
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
                High_Res_Lat_WGS_84 = Met.ComplementoA2(octeto_total) * (180 / Math.Pow(2, 30));
                string octeto5 = Met.Octeto_A_Bin(paquete0[contador + 4]);
                string octeto6 = Met.Octeto_A_Bin(paquete0[contador + 5]);
                string octeto7 = Met.Octeto_A_Bin(paquete0[contador + 6]);
                string octeto8 = Met.Octeto_A_Bin(paquete0[contador + 7]);
                string octeto_total1 = octeto5 + octeto6 + octeto7 + octeto8;
                High_Res_Lon_WGS_84 = Met.ComplementoA2(octeto_total1) * (180 / Math.Pow(2, 30));
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
                    double num1 = 1;
                    double num2 = 128;
                    ToA_Velocity = Met.ComplementoA2(octeto_total) * (num1 / num2);
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
                        Air_Speed = "IAS:" + ias.ToString();// NM/s
                    }
                    else
                    {
                        double Mach = Met.ComplementoA2(octeto_total.Remove(0, 1)) * num1;
                        Air_Speed = "MACH:" + Mach.ToString();
                    }
                    contador = contador + 2;


                }
                if (FSPEC[9] == "1")
                {
                    //I021/151 True Airspeed
                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                    string octeto_total = octeto1 + octeto2;
                    if (Convert.ToInt32(octeto_total[0].ToString()) == 0)
                    {
                        double tas = Met.ComplementoA2(octeto_total.Remove(0, 1));
                        True_Airspeed = "TAS:" + tas.ToString();// knots
                    }
                    else
                    {
                        True_Airspeed = "Value exceeds defined range";
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
                    TMRP = Math.Round(Met.ComplementoA2(octeto_total) * (num1 / num2), 2);
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
                    double num1 = 1;
                    if (Convert.ToInt32(octeto1[0].ToString()) == 1 & Convert.ToInt32(octeto1[1].ToString()) == 1)
                    {
                        TMRP_FSI = "Reserved";
                    }
                    if (Convert.ToInt32(octeto1[0].ToString()) == 1 & Convert.ToInt32(octeto1[1].ToString()) == 0)
                    {
                        TMRP_FSI = "TMRP:" + (TMRP - num1);
                    }
                    if (Convert.ToInt32(octeto1[0].ToString()) == 0 & Convert.ToInt32(octeto1[1].ToString()) == 1)
                    {
                        TMRP_FSI = "TMRP:" + (TMRP + num1);
                    }
                    if (Convert.ToInt32(octeto1[0].ToString()) == 0 & Convert.ToInt32(octeto1[1].ToString()) == 0)
                    {
                        TMRP_FSI = "TMRP:" + TMRP;
                    }
                    TMRP_HP = Met.ComplementoA2(octeto_total.Remove(0, 2)) * Math.Pow(2, -30);
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
                    TMRV = Math.Round(Met.ComplementoA2(octeto_total) * (num1 / num2), 2);
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
                        double num1 = 1;
                        if (Convert.ToInt32(octeto1[0].ToString()) == 1 & Convert.ToInt32(octeto1[1].ToString()) == 1)
                        {
                            TMRV_FSI = "Reserved";
                        }
                        if (Convert.ToInt32(octeto1[0].ToString()) == 1 & Convert.ToInt32(octeto1[1].ToString()) == 0)
                        {
                            TMRV_FSI = "TMRV:" + (TMRV - num1);
                        }
                        if (Convert.ToInt32(octeto1[0].ToString()) == 0 & Convert.ToInt32(octeto1[1].ToString()) == 1)
                        {
                            TMRV_FSI = "TMRV:" + (TMRV + num1);
                        }
                        if (Convert.ToInt32(octeto1[0].ToString()) == 0 & Convert.ToInt32(octeto1[1].ToString()) == 0)
                        {
                            TMRV_FSI = "TMRV:" + TMRV;
                        }
                        TMRV_HP = Met.ComplementoA2(octeto_total.Remove(0, 2)) * Math.Pow(2, -30);
                        contador = contador + 4;
                    }
                    if (FSPEC[15] == "1")
                    {
                        //I021/140: Geometric Height
                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                        string octeto_total = octeto1 + octeto2;
                        GH = Met.ComplementoA2(octeto_total) * 6.25;
                        contador = contador + 2;

                    }
                    if (FSPEC[16] == "1")
                    {
                        // I021/090: Quality Indicators
                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        char[] NUCR_Bits = octeto1.ToCharArray();
                        string NUCR_Bin = NUCR_Bits[0].ToString() + NUCR_Bits[1].ToString() + NUCR_Bits[2].ToString();
                        Int32 NUCR = Convert.ToInt32(NUCR_Bin, 2);
                        NUCr_or_NACv = "Navigation Uncertainty Category for velocity NUCr or the Navigation Accuracy Category for Velocity NACv:" + NUCR;
                        string NUC_Bin = NUCR_Bits[3].ToString() + NUCR_Bits[4].ToString() + NUCR_Bits[5].ToString() + NUCR_Bits[6].ToString();
                        Int32 NUC = Convert.ToInt32(NUC_Bin, 2);
                        NUCp_or_NIC = "Navigation Uncertainty Category for Position NUCp or Navigation Integrity Category NIC:" + NUC;
                        if (Convert.ToInt32(NUCR_Bits[7].ToString()) == 0) { contador = contador + 1; }
                        if (Convert.ToInt32(NUCR_Bits[7].ToString()) == 1)
                        {
                            contador = contador + 1;
                            string octeto2 = Met.Octeto_A_Bin(paquete0[contador]);
                            char[] octeto2Bits = octeto2.ToCharArray();
                            string p = octeto2Bits[0].ToString();
                            Int32 NIC_baro_mostrar = Convert.ToInt32(p, 2);
                            NIC_baro = "Navigation Integrity Category for Barometric Altitude: " + NIC_baro_mostrar;
                            string SIL1 = octeto2Bits[1].ToString() + octeto2Bits[2].ToString();
                            Int32 SIL11 = Convert.ToInt32(SIL1, 2);
                            SIL = "Surveillance (version 1) or Source (version 2) Integrity Level: " + SIL11;
                            string NACP1 = octeto2Bits[3].ToString() + octeto2Bits[4].ToString() + octeto2Bits[5].ToString() + octeto2Bits[6].ToString();
                            Int32 NACP11 = Convert.ToInt32(NACP1, 2);
                            NACp = "Navigation Accuracy Category for Position: " + NACP11;
                            if (Convert.ToInt32(octeto2Bits[7].ToString()) == 0) { contador = contador + 1; }
                            if (Convert.ToInt32(octeto2Bits[7].ToString()) == 1)
                            {
                                contador = contador + 1;
                                string octeto3 = Met.Octeto_A_Bin(paquete0[contador]);
                                char[] octeto3Bits = octeto3.ToCharArray();
                                string SIL_supp = octeto3Bits[2].ToString();
                                if (SIL_supp == "0") { SIL_Supplement = "measured per flight-hour"; }
                                else { SIL_Supplement = "measured per sample"; }
                                string SDA1 = octeto3Bits[3].ToString() + octeto3Bits[4].ToString();
                                Int32 SDA11 = Convert.ToInt32(SDA1, 2);
                                SDA = "Horizontal Position System Design Assurance Level (as defined in version 2): " + SDA11;
                                string GVA1 = octeto3Bits[5].ToString() + octeto3Bits[6].ToString();
                                Int32 GVA11 = Convert.ToInt32(GVA1, 2);
                                GVA = "Geometric Altitude Accuracy: " + GVA11;

                                if (Convert.ToInt32(octeto3Bits[7].ToString()) == 0) { contador = contador + 1; }
                                if (Convert.ToInt32(octeto3Bits[7].ToString()) == 1)
                                {
                                    contador = contador + 1;
                                    string octeto4 = Met.Octeto_A_Bin(paquete0[contador]);
                                    char[] octeto4Bits = octeto4.ToCharArray();
                                    string total = octeto4Bits[0].ToString() + octeto4Bits[1].ToString() + octeto4Bits[2].ToString() + octeto4Bits[3].ToString();
                                    int PIC1 = Convert.ToInt32(total, 2);
                                    if (PIC1 == 0) { PIC = "No integrity (or > 20.0 NM)"; }
                                    if (PIC1 == 1) { PIC = "< 20.0 NM"; }
                                    if (PIC1 == 2) { PIC = "< 10.0 NM"; }
                                    if (PIC1 == 3) { PIC = "< 8.0 NM"; }
                                    if (PIC1 == 4) { PIC = "< 4.0 NM"; }
                                    if (PIC1 == 5) { PIC = "< 2.0 NM"; }
                                    if (PIC1 == 6) { PIC = "< 1.0 NM"; }
                                    if (PIC1 == 7) { PIC = "< 0.6 NM"; }
                                    if (PIC1 == 8) { PIC = "< 0.5 NM"; }
                                    if (PIC1 == 9) { PIC = "< 0.3 NM"; }
                                    if (PIC1 == 10) { PIC = "< 0.2 NM"; }
                                    if (PIC1 == 11) { PIC = "< 0.1 NM"; }
                                    if (PIC1 == 2) { PIC = "< 0.04 NM"; }
                                    if (PIC1 == 13) { PIC = "< 0.013 NM"; }
                                    if (PIC1 == 14) { PIC = "< 0.004 NM"; }
                                    if (PIC1 == 15) { PIC = "No defined"; }

                                }
                            }

                        }
                        contador = contador + 1;
                    }
                    if (FSPEC[17] == "1")
                    {
                        // I021/210: MOPS Version

                        string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                        if (Convert.ToInt32(octeto[1].ToString(), 2) == 0) { VNS = "The MOPS Version is supported by the GS"; }
                        else { VNS = "The MOPS Version is not supported by the GS"; }
                        if (Convert.ToInt32(octeto[2].ToString() + octeto[3].ToString() + octeto[4].ToString(), 2) == 0) { VN = "ED102/DO-260 [Ref. 8]"; }
                        if (Convert.ToInt32(octeto[2].ToString() + octeto[3].ToString() + octeto[4].ToString(), 2) == 1) { VN = "DO-260A [Ref. 9]"; }
                        if (Convert.ToInt32(octeto[2].ToString() + octeto[3].ToString() + octeto[4].ToString(), 2) == 2) { VN = "ED102A/DO-260B [Ref. 11]"; }
                        if (Convert.ToInt32(octeto[5].ToString() + octeto[6].ToString() + octeto[7].ToString(), 2) == 0) { LTT = "Other"; }
                        if (Convert.ToInt32(octeto[5].ToString() + octeto[6].ToString() + octeto[7].ToString(), 2) == 1) { LTT = "UAT"; }
                        if (Convert.ToInt32(octeto[5].ToString() + octeto[6].ToString() + octeto[7].ToString(), 2) == 2) { LTT = "1090 ES"; }
                        if (Convert.ToInt32(octeto[5].ToString() + octeto[6].ToString() + octeto[7].ToString(), 2) == 3) { LTT = "VDL 4"; }
                        else { LTT = "Not assigned"; }
                        contador = contador + 1;


                    }
                    if (FSPEC[18] == "1")
                    {
                        //I021/070: Mode 3/A Code in Octal Representation

                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                        string octeto_total = octeto1[4].ToString() + octeto1[5].ToString() + octeto1[6].ToString() + octeto1[7].ToString() + octeto2.ToString();
                        M3AC = "Mode-3/A reply in octal representation: " + Convert.ToString(Convert.ToInt32(Convert.ToInt32(octeto_total, 2)), 8);
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
                        FL = Met.ComplementoA2(octeto_total) * 0.25;
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
                            MH = Met.ComplementoA2(octeto_total) * (360.0 / Math.Pow(2, 16));
                            contador = contador + 2;
                        }
                        if (FSPEC[22] == "1")
                        {
                            //I021/200: Target Status

                            string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                            if (Convert.ToInt32(octeto[0].ToString(), 2) == 0) { ICF = "No intent change active"; }
                            else { ICF = "Intent change flag raised "; }
                            if (Convert.ToInt32(octeto[1].ToString(), 2) == 0) { LNAV = "LNAV Mode engaged"; }
                            else { LNAV = "LNAV Mode not engaged "; }
                            string octetoPS = octeto[3].ToString() + octeto[4].ToString() + octeto[5].ToString();
                            if (Convert.ToInt32(octetoPS, 2) == 0) { PS = "No emergency / not reported"; }
                            if (Convert.ToInt32(octetoPS, 2) == 1) { PS = "General emergency"; }
                            if (Convert.ToInt32(octetoPS, 2) == 2) { PS = "Lifeguard / medical emergency"; }
                            if (Convert.ToInt32(octetoPS, 2) == 3) { PS = "Minimum fuel"; }
                            if (Convert.ToInt32(octetoPS, 2) == 4) { PS = "No communications"; }
                            if (Convert.ToInt32(octetoPS, 2) == 5) { PS = "Unlawful interference"; }
                            if (Convert.ToInt32(octetoPS, 2) == 6) { PS = "Downed aircraft"; }
                            string octetoSS = octeto[6].ToString() + octeto[7].ToString();
                            if (Convert.ToInt32(octetoSS, 2) == 0) { SS = "No condition reported"; }
                            if (Convert.ToInt32(octetoSS, 2) == 1) { SS = "Permanent Alert (Emergency condition)"; }
                            if (Convert.ToInt32(octetoSS, 2) == 2) { SS = "Temporary Alert (change in Mode 3/A Code other than emergency"; }
                            if (Convert.ToInt32(octetoSS, 2) == 3) { SS = "SPI set"; }
                            contador = contador + 1;

                        }
                        if (FSPEC[23] == "1")
                        {
                            // I021/155: Barometric Vertical Rate

                            string REbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                            if (Convert.ToInt32(REbits[0].ToString()) == 0) { RE = "Value in defined range"; }
                            else { RE = "Value exceeds defined range"; }
                            string BVRbits = REbits.Remove(0, 1);
                            BVR = Met.ComplementoA2(BVRbits) * 6.25;
                            contador = contador + 2;
                        }
                        if (FSPEC[24] == "1")
                        {
                            //021/157: Geometric Vertical Rate

                            string REbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                            if (Convert.ToInt32(REbits[0].ToString()) == 0) { RE = "Value in defined range"; }
                            else { RE = "Value exceeds defined range"; }
                            string BVRbits = REbits.Remove(0, 1);
                            GVR = Met.ComplementoA2(BVRbits) * 6.25;
                            contador = contador + 2;
                        }
                        if (FSPEC[25] == "1")
                        {
                            //I021/160: Airborne Ground Vector

                            string GSbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                            if (Convert.ToInt32(GSbits[0].ToString()) == 0) { RE = "Value in defined range"; }
                            else { RE = "Value exceeds defined range"; }
                            string GSBITS = GSbits.Remove(0, 1);
                            GS = Met.ComplementoA2(GSBITS) * 0.22;
                            string TAbits = Met.Octeto_A_Bin(paquete0[contador + 2]) + Met.Octeto_A_Bin(paquete0[contador + 3]);
                            TA = Math.Round(Met.ComplementoA2(TAbits) * (360.0 / Math.Pow(2, 16)), 2);
                            contador = contador + 4;
                        }
                        if (FSPEC[26] == "1")
                        {

                            // I021/165: Track Angle Rate
                            string TARbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                            string TARBITS = TARbits.Remove(0, 6);
                            TAR = Met.ComplementoA2(TARBITS) * (1.0 / 32.0);
                            contador = contador + 2;
                        }
                        if (FSPEC[27] == "1")
                        {
                            // I021/077 Time of Report Transmission
                            string ToARTbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]) + Met.Octeto_A_Bin(paquete0[contador + 2]);
                            ToART = Math.Round(Met.ComplementoA2(ToARTbits) * (1.0 / 128.0), 2);
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
                                if (Convert.ToInt32(octeto, 2) == 2) { ECAT = "15500 lbs < small aircraft <75000 lbs"; }
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
                                if (octeto[0].ToString() == "0") { WS = "No Wind Speed reported"; }
                                else
                                {
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                                    string windspeedbits = octeto2 + octeto3;
                                    WS = Convert.ToString(Met.ComplementoA2(windspeedbits));
                                    contador = contador + 2;
                                }
                                if (octeto[1].ToString() == "0") { WD = "No Wind Speed reported"; }
                                else
                                {
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                                    string windspeedbits = octeto2 + octeto3;
                                    WD = Convert.ToString(Met.ComplementoA2(windspeedbits));
                                    contador = contador + 2;

                                }
                                if (octeto[2].ToString() == "0") { TMP = "No Temperature reported"; }
                                else
                                {
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                                    string windspeedbits = octeto2 + octeto3;
                                    TMP = Convert.ToString(Math.Round((Met.ComplementoA2(windspeedbits) * 0.25), 2));
                                    contador = contador + 2;
                                }
                                if (octeto[3].ToString() == "0") { TRB = "No Turbulence reported"; }
                                else
                                {
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    TRB = Convert.ToString(Convert.ToInt32(Met.ComplementoA2(octeto2)), 2);
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
                                SA = Met.ComplementoA2(altitudebits) * 25.0;
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
                                FSSA = Met.ComplementoA2(altitudebits) * 25.0;
                                contador = contador + 2;
                            }
                            if (FSPEC[33] == "1")
                            {
                                //I021/110 Trajectory Intent
                                string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                if (octeto[0].ToString() == "0") { TIS = "Abscence of Subfield #1"; }
                                else
                                {
                                    contador = contador + 1;
                                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                                    if (octeto1[1].ToString() == "0") { NVB = "Trajectory Intent Data is valid"; }
                                    else { NVB = "Trajectory Intent Data is not valid"; }
                                    if (octeto1[0].ToString() == "1") { NAV = "Trajectory Intent Data is not available for this aircraft"; }
                                    else
                                    {
                                        contador = contador + 1;
                                        string octeto2 = Met.Octeto_A_Bin(paquete0[contador]);
                                        REP = Convert.ToInt32(octeto2, 2);
                                        contador = contador + 1;
                                        string octeto3 = Met.Octeto_A_Bin(paquete0[contador]);
                                        if (octeto3[0].ToString() == "0") { TCA = "TCP number available"; }
                                        else { TCA = "TCP number not available"; }
                                        if (octeto3[1].ToString() == "0") { NC = "TCP compliance"; }
                                        else { NC = "TCP non-compliance"; }
                                        TCP_Num = Convert.ToInt32(octeto3.Remove(0, 1), 2);
                                        string octetoaltitude = Met.Octeto_A_Bin(paquete0[contador + 1]) + Met.Octeto_A_Bin(paquete0[contador + 2]);
                                        Alt_TID = Math.Round(Convert.ToInt32(octetoaltitude, 2) * 10.0, 2);
                                        string octetolatitude = Met.Octeto_A_Bin(paquete0[contador + 3]) + Met.Octeto_A_Bin(paquete0[contador + 4]) + Met.Octeto_A_Bin(paquete0[contador + 5]);
                                        Lat_TID = Math.Round(Convert.ToInt32(octetolatitude, 2) * (180.0 / Math.Pow(2, 23)), 2);
                                        string octetolong = Met.Octeto_A_Bin(paquete0[contador + 6]) + Met.Octeto_A_Bin(paquete0[contador + 7]) + Met.Octeto_A_Bin(paquete0[contador + 8]);
                                        Lon_TID = Math.Round(Convert.ToInt32(octetolong, 2) * (180.0 / Math.Pow(2, 23)), 2);
                                        string octeto11 = Met.Octeto_A_Bin(paquete0[contador + 9]);
                                        string pointtyopebits = octeto11[0].ToString() + octeto11[1].ToString() + octeto11[2].ToString() + octeto11[3].ToString();
                                        if (Convert.ToInt32(pointtyopebits, 2) == 0) { Point_Type = "Unknown"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 1) { Point_Type = "Fly by waypoint (LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 2) { Point_Type = "Fly over waypoint (LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 3) { Point_Type = "Hold pattern (LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 4) { Point_Type = "Procedure hold(LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 5) { Point_Type = "Procedure turn (LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 6) { Point_Type = "RF leg (LT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 7) { Point_Type = "Top of climb (VT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 8) { Point_Type = "Top of descent (VT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 9) { Point_Type = "Start of level (VT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 10) { Point_Type = "Cross-over altitude (VT)"; }
                                        if (Convert.ToInt32(pointtyopebits, 2) == 11) { Point_Type = "Transition altitude (VT)"; }
                                        string TDbits = octeto11[4].ToString() + octeto11[5].ToString();
                                        if (TDbits == "00") { TD = "N/A"; }
                                        if (TDbits == "01") { TD = "Turn right"; }
                                        if (TDbits == "10") { TD = "Turn left"; }
                                        if (TDbits == "11") { TD = "No turn"; }
                                        if (octeto11[6].ToString() == "0") { TRA = "TTR not available"; }
                                        if (octeto11[6].ToString() == "1") { TRA = "TTR available"; }
                                        if (octeto11[7].ToString() == "0") { TOA = "TOV available"; }
                                        if (octeto11[7].ToString() == "1") { TOA = "TOV not available"; }
                                        string TOVbits = Met.Octeto_A_Bin(paquete0[contador + 10]) + Met.Octeto_A_Bin(paquete0[contador + 11]) + Met.Octeto_A_Bin(paquete0[contador + 12]);
                                        TOV = Convert.ToInt32(TOVbits, 2);
                                        string TTRbits = Met.Octeto_A_Bin(paquete0[contador + 13]) + Met.Octeto_A_Bin(paquete0[contador + 14]);
                                        TTR = Convert.ToInt32(TTRbits, 2) * 0.01;
                                        contador = contador + 14;
                                    }

                                }
                                contador = contador + 1;


                            }
                            if (FSPEC[34] == "1")
                            {
                                //I021/016 Service Management
                                string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                RP = Convert.ToInt32(octeto, 2) * 0.5;
                                contador = contador + 1;
                            }

                            if (FSPEC.Count > 35)
                            {
                                if (FSPEC[35] == "1")
                                {
                                    //I021/008 Aircraft Operational Status
                                    string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                                    if (octeto[0].ToString() == "0") { RA = "TCAS II or ACAS RA not active"; }
                                    else { RA = "TCAS RA active"; }
                                    string tcbits = octeto[1].ToString() + octeto[2].ToString();
                                    if (Convert.ToInt32(tcbits, 2) == 0) { TC = "no capability for Trajectory Change Reports"; }
                                    if (Convert.ToInt32(tcbits, 2) == 1) { TC = "support for TC+0 reports only"; }
                                    if (Convert.ToInt32(tcbits, 2) == 2) { TC = "support for multiple TC reports"; }
                                    if (Convert.ToInt32(tcbits, 2) == 3) { TC = "Reserved"; }
                                    if (octeto[3].ToString() == "0") { TS = "no capability to support Target State Reports"; }
                                    else { TS = "capable of supporting target State Reports"; }
                                    if (octeto[4].ToString() == "0") { ARV = "no capability to generate ARV-reports"; }
                                    else { ARV = "capable of generate ARV-reports"; }
                                    if (octeto[5].ToString() == "0") { CDTIA = "CDTI not operational"; }
                                    else { CDTIA = "CDTI operational"; }
                                    if (octeto[6].ToString() == "0") { NotTCAS = "TCAS operational"; }
                                    else { NotTCAS = "TCAS not operational"; }
                                    if (octeto[7].ToString() == "0") { SingAnt = "Antenna Diversity"; }
                                    else { SingAnt = "Single Antenna only "; }
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
                                    REP_MODESMBDAATA = Convert.ToInt32(octeto1, 2);
                                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                                    string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                                    string octeto4 = Met.Octeto_A_Bin(paquete0[contador + 3]);
                                    string octeto5 = Met.Octeto_A_Bin(paquete0[contador + 4]);
                                    string octeto6 = Met.Octeto_A_Bin(paquete0[contador + 5]);
                                    string octeto7 = Met.Octeto_A_Bin(paquete0[contador + 6]);
                                    string octeto8 = Met.Octeto_A_Bin(paquete0[contador + 7]);
                                    MB_data = Convert.ToInt32(octeto2.ToString() + octeto3.ToString() + octeto4.ToString() + octeto5.ToString() + octeto6.ToString() + octeto7.ToString() + octeto8.ToString(), 2);
                                    string octeto9 = Met.Octeto_A_Bin(paquete0[contador + 8]);
                                    BDS1 = Convert.ToInt32(octeto9.Remove(4, 4), 2);
                                    BDS2 = Convert.ToInt32(octeto9.Remove(0, 4), 2);
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
                                    if (octeto[0].ToString() == "1")
                                    {
                                        contador = contador + 4;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        AOS_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                    }
                                    if (octeto[1].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string TRD_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        TRD_age = Convert.ToInt32(TRD_BITS, 2) * 0.1;

                                    }
                                    if (octeto[2].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        M3A_age = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 2);

                                    }
                                    if (octeto[3].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        QI_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                    }
                                    if (octeto[4].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        TI_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                    }
                                    if (octeto[5].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        MAM_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                    }
                                    if (octeto[6].ToString() == "1")
                                    {
                                        contador = contador + 1;
                                        string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                        GH_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                    }
                                    if (octeto[7].ToString() == "1")
                                    {
                                        string octeto2 = Met.Octeto_A_Bin(paquete0[contadordeoctetos + 1]);
                                        if (octeto2[0].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            FL_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                        }
                                        if (octeto2[1].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            ISA_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                        }
                                        if (octeto2[2].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            FSA_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                        }
                                        if (octeto2[3].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            AS_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                        }
                                        if (octeto2[4].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            TAS_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                        }
                                        if (octeto2[5].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            MH_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                        }
                                        if (octeto2[6].ToString() == "1")
                                        {
                                            contador = contador + 1;
                                            string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                            BVR_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                        }
                                        if (octeto2[7].ToString() == "1")
                                        {
                                            string octeto3 = Met.Octeto_A_Bin(paquete0[contadordeoctetos + 2]);
                                            if (octeto3[0].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                GVR_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                            }
                                            if (octeto3[1].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                GV_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                            }
                                            if (octeto3[2].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                TAR_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                            }
                                            if (octeto3[3].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                Target_ID_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                            }
                                            if (octeto3[4].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                TS_age = Math.Round(Convert.ToInt32(AOS_BITS, 2) * 0.1, 2);

                                            }
                                            if (octeto3[5].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                MET_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                            }
                                            if (octeto3[6].ToString() == "1")
                                            {
                                                contador = contador + 1;
                                                string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                ROA_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                            }
                                            if (octeto3[7].ToString() == "1")
                                            {
                                                string octeto4 = Met.Octeto_A_Bin(paquete0[contadordeoctetos + 3]);
                                                if (octeto4[0].ToString() == "1")
                                                {
                                                    contador = contador + 1;
                                                    string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                    ARA_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

                                                }
                                                if (octeto4[1].ToString() == "1")
                                                {
                                                    contador = contador + 1;
                                                    string AOS_BITS = Met.Octeto_A_Bin(paquete0[contador]);
                                                    SCC_age = Convert.ToInt32(AOS_BITS, 2) * 0.1;

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
    }
}
