using System;
using System.Collections.Generic;
using System.Text;

namespace CLASSES
{
    public class CAT21_v23
    {
        // Definir los Data Items como variables, para procesar las que están en el paquete (las que son 1)
        // El tipo de cada variable depende de la precisión con la que se nos proporciona (especificado pdf CAT21 v023)
        public double Data_Source_ID_SIC = double.NaN;
        public double Data_Source_ID_SAC = double.NaN;
        public string[] Target_Report_Desc = new string[9];
        public double Time_of_Day = double.NaN;
        public double Lat_WGS_84 = double.NaN; //in degrees
        public double Lon_WGS_84 = double.NaN; //in degrees
        public string Target_Address = null;//Target address (emitter identifier) assigned uniquely to each target. en hexa
        public double GA = double.NaN;
        public string[] Fig_of_Merit = new string[4];
        public string[] Link_Tech = new string[5];
        public double Roll = double.NaN; // roll angle in degrees
        public double FL = double.NaN; //flight level in feet
        public double[] Air_Speed = new double[2];// ias and mach
        public double True_Airspeed = double.NaN; //kt
        public double MH = double.NaN; //Magnetic heading in degrees
        public string RE = null;
        public double BVR = double.NaN; // Barometric Vertical Rate in feet/minute
        public double GVR = double.NaN; // Geometric Vertical Rate in feet/minute
        public double GS = double.NaN; // ground speed in kt
        public double TA = double.NaN; // Track angle in degrees
        public string[] Rate_of_Turn = new string[2];
        public string Target_ID = null;
        public double Velocity_Acc = double.NaN;
        public double ToD_Acc = double.NaN;
        public string[] Target_Status = new string[4];
        public string ECAT = null;
        public string[] Met_Report = new string[4];
        public string SAS = null;
        public string Source = null;
        public double Interm_Selec_Alt = double.NaN; //selected altitude in ft
        public string MV = null;
        public string AH = null;
        public string AM = null;
        public double FSSA = double.NaN; //final state selected altitde in ft
        public string[] Trajectory_Intent = new string[16];

        public void Decode21(string[] paquete)
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

                if (TargetReport_Bin[0].ToString() == "0") { Target_Report_Desc[0] = "No differential correction (ADS-B)"; }
                if (TargetReport_Bin[0].ToString() == "1") { Target_Report_Desc[0] = "Differential correction (ADS-B)"; }
                if (TargetReport_Bin[1].ToString() == "0") { Target_Report_Desc[1] = "Ground Bit not set"; }
                if (TargetReport_Bin[1].ToString() == "1") { Target_Report_Desc[1] = "Ground Bit set"; }
                if (TargetReport_Bin[2].ToString() == "0") { Target_Report_Desc[2] = "Actual target report"; }
                if (TargetReport_Bin[2].ToString() == "1") { Target_Report_Desc[2] = "Simulated target report"; }
                if (TargetReport_Bin[3].ToString() == "0") { Target_Report_Desc[3] = "Default"; }
                if (TargetReport_Bin[3].ToString() == "1") { Target_Report_Desc[3] = "Test Target"; }
                if (TargetReport_Bin[4].ToString() == "0") { Target_Report_Desc[4] = "Report from target transponder"; }
                if (TargetReport_Bin[4].ToString() == "1") { Target_Report_Desc[4] = "Report from field monitor (fixed transponder)"; }
                if (TargetReport_Bin[5].ToString() == "0") { Target_Report_Desc[5] = "Equipement not capable to provide Selected Altitude"; }
                if (TargetReport_Bin[5].ToString() == "1") { Target_Report_Desc[5] = "Equipement capable to provide Selected Altitude"; }
                if (TargetReport_Bin[6].ToString() == "0") { Target_Report_Desc[6] = "Absence of SPI"; }
                if (TargetReport_Bin[6].ToString() == "1") { Target_Report_Desc[6] = "Special Position Identification"; }

                string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);

                string ARC_Bin = octeto2[0].ToString() + octeto2[1].ToString() + octeto2[2].ToString();
                Int32 ARC = Convert.ToInt32(ARC_Bin, 2);
                if (ARC == 0) { Target_Report_Desc[7] = "Non unique address"; }
                if (ARC == 1) { Target_Report_Desc[7] = "24-Bit ICAO address"; }
                if (ARC == 2) { Target_Report_Desc[7] = "Surface vehicle address"; }
                if (ARC == 3) { Target_Report_Desc[7] = "Anonymous address"; }
                if (ARC == 4 || ARC == 5 || ARC == 6 || ARC == 7) { Target_Report_Desc[7] = "Reserved for future use"; }
                string bits2 = octeto2[3].ToString() + octeto2[4].ToString();
                Int32 bb = Convert.ToInt32(bits2, 2);

                if (bb == 0) { Target_Report_Desc[8] = "Unknown"; }
                if (bb == 1) { Target_Report_Desc[8] = "25 ft"; }
                if (bb == 2) { Target_Report_Desc[8] = "100 ft"; }
                contador = contador + 2;

            }
            
        
            if (FSPEC[2] == "1")
            {
                //Data Item I021/030, Time of Day
                string TODbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]) + Met.Octeto_A_Bin(paquete0[contador + 2]);
                Time_of_Day = Math.Round(Convert.ToInt32(TODbits, 2) * (1.0 / 128.0), 3);
                contador = contador + 3;
            }
            if (FSPEC[3] == "1")
            {
                // I021/130: Position in WGS-84 Co-ordinates
                string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                string octeto3 = Met.Octeto_A_Bin(paquete0[contador + 2]);
                string octeto_total = octeto1 + octeto2 + octeto3;
                Lat_WGS_84 = Math.Round(Met.ComplementoA2(octeto_total) * (180.0 / Math.Pow(2, 23)), 3);
                string octeto4 = Met.Octeto_A_Bin(paquete0[contador + 3]);
                string octeto5 = Met.Octeto_A_Bin(paquete0[contador + 4]);
                string octeto6 = Met.Octeto_A_Bin(paquete0[contador + 5]);
                string octeto_total1 = octeto4 + octeto5 + octeto6;
                Lon_WGS_84 = Math.Round(Met.ComplementoA2(octeto_total1) * (180.0 / Math.Pow(2, 23)), 3);
                contador = contador + 6;
            }
            if (FSPEC[4] == "1")
            {
                // I021/080 Target Address
                Target_Address = Convert.ToString(paquete0[contador]) + Convert.ToString(paquete0[contador + 1]) + Convert.ToString(paquete0[contador + 2]);
                contador = contador + 3;
            }
            if (FSPEC[5] == "1")
            {
                //I021/140: Geometric Altitude
                string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                string octeto_total = octeto1 + octeto2;
                GA = Math.Round(Met.ComplementoA2(octeto_total) * 6.25, 3);

                contador = contador + 2;
            }
            if (FSPEC[6] == "1")
            {
                //Data Item I021/090, Figure of Merit
                string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                string acbits = octeto1[0].ToString() + octeto1[1].ToString();
                if (acbits == "00") { Fig_of_Merit[0] = "Unknown"; }
                if (acbits == "01") { Fig_of_Merit[0] = "ACAS not operational"; }
                if (acbits == "10") { Fig_of_Merit[0] = "ACAS operational"; }
                if (acbits == "11") { Fig_of_Merit[0] = "invalid"; }
                string mnbits = octeto1[2].ToString() + octeto1[3].ToString();
                if (mnbits == "00") { Fig_of_Merit[1] = "Unknown"; }
                if (mnbits == "01") { Fig_of_Merit[1] = "Multiple navigational aids not operating"; }
                if (mnbits == "10") { Fig_of_Merit[1] = "Multiple navigational aids operating"; }
                if (mnbits == "11") { Fig_of_Merit[1] = "invalid"; }
                string dcbits = octeto1[4].ToString() + octeto1[5].ToString();
                if (mnbits == "00") { Fig_of_Merit[2] = "Unknown"; }
                if (mnbits == "01") { Fig_of_Merit[2] = "Differential correction"; }
                if (mnbits == "10") { Fig_of_Merit[2] = "No Differential correction"; }
                if (mnbits == "11") { Fig_of_Merit[2] = "invalid"; }
                string octeto2 = Met.Octeto_A_Bin(paquete0[contador+1]);
                string pabits = octeto2[4].ToString() + octeto2[5].ToString() + octeto2[6].ToString() + octeto2[7].ToString();
                Fig_of_Merit[3] = pabits;
                contador = contador + 2;
            }
            if (FSPEC.Count > 7)
            {

                if (FSPEC[7] == "1")
                {
                    //Data Item I021/210, Link Technology Indicator
                    string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                    if (octeto[3].ToString() == "0") { Link_Tech[0] = "Unknown"; }
                    if (octeto[3].ToString() == "1") { Link_Tech[0] = "Aircraft Equiped with CDTI"; }
                    if (octeto[4].ToString() == "0") { Link_Tech[1] = "Not Used"; }
                    if (octeto[4].ToString() == "1") { Link_Tech[1] = "Used"; }
                    if (octeto[5].ToString() == "0") { Link_Tech[2] = "Not Used"; }
                    if (octeto[5].ToString() == "1") { Link_Tech[2] = "Used"; }
                    if (octeto[6].ToString() == "0") { Link_Tech[3] = "Not Used"; }
                    if (octeto[6].ToString() == "1") { Link_Tech[3] = "Used"; }
                    if (octeto[7].ToString() == "0") { Link_Tech[4] = "Not Used"; }
                    if (octeto[7].ToString() == "1") { Link_Tech[4] = "Used"; }
                    contador = contador + 1;
                }
                if (FSPEC[8] == "1")
                {
                    // I021/230 Roll Angle

                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                    string octeto_total = octeto1.ToString() + octeto2.ToString();
                    Roll = Met.ComplementoA2(octeto_total) * 0.01;
                    contador = contador + 2;
                }
                if (FSPEC[9] == "1")
                {
                    //I021/145: Flight Level

                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                    string octeto_total = octeto1.ToString() + octeto2.ToString();
                    FL = Math.Round(Met.ComplementoA2(octeto_total) * 0.25, 3);
                    contador = contador + 2;
                }
                if (FSPEC[10] == "1")
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
                if (FSPEC[11] == "1")
                {
                    //I021/151 True Airspeed
                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                    string octeto_total = octeto1 + octeto2;
                    True_Airspeed = Met.ComplementoA2(octeto_total);// knots
                    contador = contador + 2;
                }
                if (FSPEC[12] == "1")
                {
                    //I021/152: Magnetic Heading

                    string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                    string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                    string octeto_total = octeto1.ToString() + octeto2.ToString();
                    MH = Math.Round(Met.ComplementoA2(octeto_total) * (360.0 / Math.Pow(2, 16)), 3);

                    contador = contador + 2;
                }
                if (FSPEC[13] == "1")
                {
                    // I021/155: Barometric Vertical Rate

                    string totalbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                    BVR = Math.Round(Met.ComplementoA2(totalbits) * 6.25, 3);
                    contador = contador + 2;
                }
                if (FSPEC.Count > 14)
                {

                    if (FSPEC[14] == "1")
                    {
                        //021/157: Geometric Vertical Rate

                        string totalbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                        GVR = Math.Round(Met.ComplementoA2(totalbits) * 6.25, 3);

                        contador = contador + 2;
                    }
                    if (FSPEC[15] == "1")
                    {
                        //I021/160: Airborne Ground Vector

                        string GSbits = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]);
                        GS = Math.Round(Met.ComplementoA2(GSbits) * 0.22, 3);
                        string TAbits = Met.Octeto_A_Bin(paquete0[contador + 2]) + Met.Octeto_A_Bin(paquete0[contador + 3]);
                        TA = Math.Round(Met.ComplementoA2(TAbits) * (360.0 / Math.Pow(2, 16)), 3);
                        contador = contador + 4;
                    }
                    if (FSPEC[16] == "1")
                    {
                        //Data Item I021/165, Rate Of Turn
                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        if (octeto1[0].ToString() + octeto1[1].ToString() == "00") { Rate_of_Turn[0] = "Not available"; }
                        if (octeto1[0].ToString() + octeto1[1].ToString() == "01") { Rate_of_Turn[0] = "Left"; }
                        if (octeto1[0].ToString() + octeto1[1].ToString() == "10") { Rate_of_Turn[0] = "Right"; }
                        if (octeto1[0].ToString() + octeto1[1].ToString() == "11") { Rate_of_Turn[0] = "Straight"; }
                        if (octeto1[7].ToString() == "0") { contador = contador + 1; }
                        if (octeto1[7].ToString() == "1")
                        {
                            string octeto2 = Met.Octeto_A_Bin(paquete0[contador + 1]);
                            string rotbits = octeto2.Remove(7);
                            Rate_of_Turn[1] = (Met.ComplementoA2(rotbits) * (1.0 / 4.0)).ToString();
                            contador = contador + 1;
                        }
                    }
                    if (FSPEC[17] == "1")
                    {
                        //I021/170 Target Identification (found in table 3.8 annex 10 ICAO) see 3.1.2.9
                        string octetototal = Met.Octeto_A_Bin(paquete0[contador]) + Met.Octeto_A_Bin(paquete0[contador + 1]) + Met.Octeto_A_Bin(paquete0[contador + 2]) + Met.Octeto_A_Bin(paquete0[contador + 3]) + Met.Octeto_A_Bin(paquete0[contador + 4]) + Met.Octeto_A_Bin(paquete0[contador + 5]);
                        Target_ID = Met.Compare_bits(octetototal);
                        contador = contador + 6;
                    }
                    if (FSPEC[18] == "1")
                    {
                        //Data Item I021/095, Velocity Accuracy
                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        Velocity_Acc = Met.ComplementoA2(octeto1);
                        contador = contador + 1;
                    }
                    if (FSPEC[19] == "1")
                    {
                        //Data Item I021/032, Time of Day Accuracy
                        string octeto1 = Met.Octeto_A_Bin(paquete0[contador]);
                        ToD_Acc = Met.ComplementoA2(octeto1) * (1.0 / 256.0);
                        contador = contador + 1;

                    }
                    if (FSPEC[20] == "1")
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
                    if (FSPEC.Count > 21)
                    {

                        if (FSPEC[21] == "1")
                        {
                            // I021/020 Emitter Category
                            string octeto = Met.Octeto_A_Bin(paquete0[contador]);
                            if (Convert.ToInt32(octeto, 2) == 1) { ECAT = "light aircraft <= 7000 kg"; }
                            if (Convert.ToInt32(octeto, 2) == 2) { ECAT = "reserved"; }
                            if (Convert.ToInt32(octeto, 2) == 3) { ECAT = "7000 kg < medium aircraft < 136000 kg"; }
                            if (Convert.ToInt32(octeto, 2) == 4) { ECAT = "reserved"; }
                            if (Convert.ToInt32(octeto, 2) == 5) { ECAT = "136000 kg <= heavy aircraft"; }
                            if (Convert.ToInt32(octeto, 2) == 6) { ECAT = "highly manoeuvrable (5g acceleration capability) and high speed (>400knots cruise)"; }
                            if (Convert.ToInt32(octeto, 2) == 7) { ECAT = "reserved"; }
                            if (Convert.ToInt32(octeto, 2) == 8) { ECAT = "reserved"; }
                            if (Convert.ToInt32(octeto, 2) == 9) { ECAT = "reserved"; }
                            if (Convert.ToInt32(octeto, 2) == 10) { ECAT = "rotocraft"; }
                            if (Convert.ToInt32(octeto, 2) == 11) { ECAT = "glider / sailplane"; }
                            if (Convert.ToInt32(octeto, 2) == 12) { ECAT = "lighter-than-air"; }
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
                            if (Convert.ToInt32(octeto, 2) == 23) { ECAT = "reserved"; }
                            if (Convert.ToInt32(octeto, 2) == 24) { ECAT = "reserved"; }
                            contador = contador + 1;
                        }
                        if (FSPEC[22] == "1")
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
                        if (FSPEC[23] == "1")
                        {
                            //021/146 Intermediate State Selected Altitude

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
                            Interm_Selec_Alt = Math.Round(Met.ComplementoA2(altitudebits) * 25.0, 3);
                            contador = contador + 2;
                        }
                        if (FSPEC[24] == "1")
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
                        if (FSPEC[25] == "1")
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
    }
}


