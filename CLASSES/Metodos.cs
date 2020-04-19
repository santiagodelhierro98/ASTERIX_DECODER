using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;

namespace CLASSES
{
    public class Metodos
    {
        // DECODING METHODS
        private static readonly Dictionary<char, string> hexCharacterToBinary = new Dictionary<char, string>
        {
            { '0', "0000" },
            { '1', "0001" },
            { '2', "0010" },
            { '3', "0011" },
            { '4', "0100" },
            { '5', "0101" },
            { '6', "0110" },
            { '7', "0111" },
            { '8', "1000" },
            { '9', "1001" },
            { 'a', "1010" },
            { 'b', "1011" },
            { 'c', "1100" },
            { 'd', "1101" },
            { 'e', "1110" },
            { 'f', "1111" }
        };
        public string HexStringToBinary(string hex)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in hex)
            {
                // Si no le pasas caracteres en Hexadecimal petara
                result.Append(hexCharacterToBinary[char.ToLower(c)]);
            }
            return result.ToString();
        }        
        public string Poner_Zeros_Delante(string octeto)
        {
            string octeto0;
            int contador = 0;
            foreach (char c in octeto)
            {
                contador++;
            }

            // Si hay un solo caracter, añadimos un 0 delante
            if (contador == 1)
            {
                octeto0 = octeto.PadLeft(2, '0');
            }
            else
            {
                octeto0 = octeto;
            }

            return octeto0;
        }
        public string Octeto_A_Bin(string Pos_Paquete)
        {
            // convertir un octeto en Hexa a un string con los dos caracteres en binario

            char[] octeto = Pos_Paquete.ToCharArray(0, 2);
            string byte1 = octeto[0].ToString();
            string byte2 = octeto[1].ToString();
            // de hexa a binario
            string first = HexStringToBinary(byte1);
            string second = HexStringToBinary(byte2);
            string bin_octeto = first + second;

            return bin_octeto;
        }
        public int Longitud_Paquete(string[] paquete)
        {
            string octeto = Poner_Zeros_Delante(paquete[2]);

            // como ningun paquete va a sobrepasar 256 de largo, solo nos fijamos en el 2o octeto de longitud
            string octeto_bin = Octeto_A_Bin(octeto);

            // de binario a decimal
            int length = Convert.ToInt32(octeto_bin, 2);

            return length;
        }
        public List<string> FSPEC(string[] paquete)
        {
            bool end_of_fspec = false;
            List<string> FSPEC = new List<string>();
            for (int i = 3; end_of_fspec != true; i++)
            {
                string octeto_bin = Octeto_A_Bin(paquete[i]);
                char[] bits = octeto_bin.ToCharArray(0, 8);

                // añadimos al vector FSPEC todos los bits menos el ultimo

                FSPEC.Add(bits[0].ToString());
                FSPEC.Add(bits[1].ToString());
                FSPEC.Add(bits[2].ToString());
                FSPEC.Add(bits[3].ToString());
                FSPEC.Add(bits[4].ToString());
                FSPEC.Add(bits[5].ToString());
                FSPEC.Add(bits[6].ToString());

                if (bits[7].ToString() == "1")
                {
                    end_of_fspec = false;
                }
                else
                {
                    // En la ultima posicion de la lista, ponemos la longitud del FSPEC
                    FSPEC.Add(i.ToString());
                    end_of_fspec = true;
                }
            }
            return FSPEC;
        }
        public double ComplementoA2(string bits)
        {
            if (bits == "1")
                return -1;
            if (bits == "0")
                return 0;
            else
            {
                if (Convert.ToString(bits[0]) == "0")
                {
                    int num = Convert.ToInt32(bits, 2);
                    return Convert.ToSingle(num);
                }
                else
                {
                    //elimino primer bit
                    string bitss = bits.Substring(1, bits.Length - 1);

                    //creo nuevo string cambiando 0s por 1s y viceversa
                    string newbits = "";
                    int i = 0;
                    while (i < bitss.Length)
                    {
                        if (Convert.ToString(bitss[i]) == "1")
                            newbits = newbits + "0";
                        if (Convert.ToString(bitss[i]) == "0")
                            newbits = newbits + "1";
                        i++;
                    }

                    //convertimos a int
                    double num = Convert.ToInt32(newbits, 2);

                    return -(num + 1);
                }
            }
        }
        public string Compare_bits(string targetbits)
        {
            int j = 0;
            string Target_ID = "";
            while (6 + j <= targetbits.Length)
            {
                char[] target_bits = targetbits.ToCharArray();
                
                string sext = target_bits[j].ToString() + target_bits[j + 1].ToString() + target_bits[j + 2].ToString() + target_bits[j + 3].ToString() + target_bits[j + 4].ToString() + target_bits[j + 5].ToString() ;
                if (sext == "000001") { Target_ID = Target_ID + "A"; }
                if (sext == "000010") { Target_ID = Target_ID + "B"; }
                if (sext == "000011") { Target_ID = Target_ID + "C"; }
                if (sext == "000100") { Target_ID = Target_ID + "D"; }
                if (sext == "000101") { Target_ID = Target_ID + "E"; }
                if (sext == "000110") { Target_ID = Target_ID + "F"; }
                if (sext == "000111") { Target_ID = Target_ID + "G"; }
                if (sext == "001000") { Target_ID = Target_ID + "H"; }
                if (sext == "001001") { Target_ID = Target_ID + "I"; }
                if (sext == "001010") { Target_ID = Target_ID + "J"; }
                if (sext == "001011") { Target_ID = Target_ID + "K"; }
                if (sext == "001100") { Target_ID = Target_ID + "L"; }
                if (sext == "001101") { Target_ID = Target_ID + "M"; }
                if (sext == "001110") { Target_ID = Target_ID + "N"; }
                if (sext == "001111") { Target_ID = Target_ID + "O"; }
                if (sext == "010000") { Target_ID = Target_ID + "P"; }
                if (sext == "010001") { Target_ID = Target_ID + "Q"; }
                if (sext == "010010") { Target_ID = Target_ID + "R"; }
                if (sext == "010011") { Target_ID = Target_ID + "S"; }
                if (sext == "010100") { Target_ID = Target_ID + "T"; }
                if (sext == "010101") { Target_ID = Target_ID + "U"; }
                if (sext == "010110") { Target_ID = Target_ID + "V"; }
                if (sext == "010111") { Target_ID = Target_ID + "W"; }
                if (sext == "011000") { Target_ID = Target_ID + "X"; }
                if (sext == "011001") { Target_ID = Target_ID + "Y"; }
                if (sext == "011010") { Target_ID = Target_ID + "Z"; }
                if (sext == "100000") { Target_ID = Target_ID + " "; }
                if (sext == "110000") { Target_ID = Target_ID + "0"; }
                if (sext == "110001") { Target_ID = Target_ID + "1"; }
                if (sext == "110010") { Target_ID = Target_ID + "2"; }
                if (sext == "110011") { Target_ID = Target_ID + "3"; }
                if (sext == "110100") { Target_ID = Target_ID + "4"; }
                if (sext == "110101") { Target_ID = Target_ID + "5"; }
                if (sext == "110110") { Target_ID = Target_ID + "6"; }
                if (sext == "110111") { Target_ID = Target_ID + "7"; }
                if (sext == "111000") { Target_ID = Target_ID + "8"; }
                if (sext == "111001") { Target_ID = Target_ID + "9"; }

                j = j + 6;               
            }
            return Target_ID;
        }

        // WPF NEEDED METHODS
        public double cartesiantolatmlat(double X, double Y)
        {
            double MLAT_lat = 41.0 + (17.0 / 60.0) + (49.0 / 3600.0) + (426.0 / 3600000.0); ;
            double R = 6371 * 1000;
            double d = Math.Sqrt((X * X) + (Y * Y));
            double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
            double phi1 = MLAT_lat * (Math.PI / 180);
            double phi2 = Math.Asin(Math.Sin(phi1) * Math.Cos(d / R) + Math.Cos(phi1) * Math.Sin(d / R) * Math.Cos(brng));
            return phi2 * (180.0 / Math.PI);
        }
        public double cartesiantolonmlat(double X, double Y)
        {
            double MLAT_lat = 41.0 + (17.0 / 60.0) + (49.0 / 3600.0) + (426.0 / 3600000.0); ;
            double MLAT_lon = 2.0 + (4.0 / 60.0) + (42.0 / 3600.0) + (410.0 / 3600000.0);
            double R = 6371 * 1000;
            double d = Math.Sqrt((X * X) + (Y * Y));
            double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
            double phi1 = MLAT_lat * (Math.PI / 180);
            double lamda1 = MLAT_lon * (Math.PI / 180);
            var phi2 = Math.Asin(Math.Sin(phi1) * Math.Cos(d / R) + Math.Cos(phi1) * Math.Sin(d / R) * Math.Cos(brng));
            double lamda2 = lamda1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(phi1), Math.Cos(d / R) - Math.Sin(phi1) * Math.Sin(phi2));
            return lamda2 * (180.0 / Math.PI);
        }
        public string convert_to_hms(double tod)
        {
            if (tod != 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(tod);
                string tiempoact = time.ToString(@"hh\:mm\:ss");
                return tiempoact;
            }
            else
            {
                string p = "NaN";
                return p;
            }
        }
        public void CreateReducedTable(DataTable multiplecattablereducida, DataTable tablacat10reducida, DataTable tablacat21reducida)
        {
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
        public void Create_TrackTable_Puras(DataTable tablaCAT10, DataTable tablaCAT21) 
        {
            //CAT10
            tablaCAT10.Columns.Add(new DataColumn("#"));
            tablaCAT10.Columns.Add(new DataColumn("CAT"));
            tablaCAT10.Columns.Add(new DataColumn("SIC"));
            tablaCAT10.Columns.Add(new DataColumn("SAC"));
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
            tablaCAT21.Columns.Add(new DataColumn("CAT"));
            tablaCAT21.Columns.Add(new DataColumn("SIC"));
            tablaCAT21.Columns.Add(new DataColumn("SAC"));
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
        public void Create_TrackTable_Multiple(DataTable tablaMultipleCAT)
        {
            //Common Items
            tablaMultipleCAT.Columns.Add(new DataColumn("#"));
            tablaMultipleCAT.Columns.Add(new DataColumn("CAT"));
            tablaMultipleCAT.Columns.Add(new DataColumn("SIC"));
            tablaMultipleCAT.Columns.Add(new DataColumn("SAC"));
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
        public DataTable getSearchTable10()
        {
            DataTable SearchTable10 = new DataTable();

            SearchTable10.Columns.Add(new DataColumn("#"));
            SearchTable10.Columns.Add(new DataColumn("SIC"));
            SearchTable10.Columns.Add(new DataColumn("SAC"));
            SearchTable10.Columns.Add(new DataColumn("Target ID"));
            SearchTable10.Columns.Add(new DataColumn("Track Number"));
            SearchTable10.Columns.Add(new DataColumn("Target Report")); //array
            SearchTable10.Columns.Add(new DataColumn("Message type"));
            SearchTable10.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            SearchTable10.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            SearchTable10.Columns.Add(new DataColumn("Position Polar\n(Distance, Angle)"));
            SearchTable10.Columns.Add(new DataColumn("Position Cartesian\n(X, Y)"));
            SearchTable10.Columns.Add(new DataColumn("Track Velocity Polar\n(Ground Speed, Track Angle)"));
            SearchTable10.Columns.Add(new DataColumn("Track Velocity Cartesian\n(Vx, Vy)"));
            SearchTable10.Columns.Add(new DataColumn("Track Status")); //array
            SearchTable10.Columns.Add(new DataColumn("Mode 3/A Code")); //array
            SearchTable10.Columns.Add(new DataColumn("Target Address"));
            SearchTable10.Columns.Add(new DataColumn("Mode S MB Data")); //array
            SearchTable10.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            SearchTable10.Columns.Add(new DataColumn("Flight Level"));
            SearchTable10.Columns.Add(new DataColumn("Measured Height"));
            SearchTable10.Columns.Add(new DataColumn("Target Size\n(Length x Width)"));
            SearchTable10.Columns.Add(new DataColumn("Target Heading"));
            SearchTable10.Columns.Add(new DataColumn("System Status")); //array
            SearchTable10.Columns.Add(new DataColumn("Pre Programmed MSG")); //array
            SearchTable10.Columns.Add(new DataColumn("Standard Deviation Position\n(X, Y)"));
            SearchTable10.Columns.Add(new DataColumn("Covariance of deviation"));
            SearchTable10.Columns.Add(new DataColumn("Presence")); //array
            SearchTable10.Columns.Add(new DataColumn("Amplitude\nPrimary Plot"));
            SearchTable10.Columns.Add(new DataColumn("Acceleration\n(Ax, Ay)"));

            return SearchTable10;
        }
        public DataTable getSearchTable21()
        {
            DataTable SearchTable21 = new DataTable();

            SearchTable21.Columns.Add(new DataColumn("#"));
            SearchTable21.Columns.Add(new DataColumn("SIC"));
            SearchTable21.Columns.Add(new DataColumn("SAC"));
            SearchTable21.Columns.Add(new DataColumn("Target ID"));
            SearchTable21.Columns.Add(new DataColumn("Track Number"));
            SearchTable21.Columns.Add(new DataColumn("Target Report")); // array
            SearchTable21.Columns.Add(new DataColumn("Time Of Day"));
            SearchTable21.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            SearchTable21.Columns.Add(new DataColumn("High Resolution\nPosition WGS-84\n(Latitude, Longitude)"));
            SearchTable21.Columns.Add(new DataColumn("Flight Level"));
            SearchTable21.Columns.Add(new DataColumn("Geometric Height"));
            SearchTable21.Columns.Add(new DataColumn("Operational Status")); //array
            SearchTable21.Columns.Add(new DataColumn("AirSpeed\n(IAS, Mach)"));
            SearchTable21.Columns.Add(new DataColumn("True Airspeed"));
            SearchTable21.Columns.Add(new DataColumn("Airborne Ground Vector\n(GS, Track Angle)"));
            SearchTable21.Columns.Add(new DataColumn("Track Angle Rate"));
            SearchTable21.Columns.Add(new DataColumn("Selected Altitude"));
            SearchTable21.Columns.Add(new DataColumn("MOPS version")); //array
            SearchTable21.Columns.Add(new DataColumn("Magnetic Heading"));
            SearchTable21.Columns.Add(new DataColumn("Barometric Vertical Rate"));
            SearchTable21.Columns.Add(new DataColumn("Geometric Vertical Rate"));
            SearchTable21.Columns.Add(new DataColumn("Mode 3A Code"));
            SearchTable21.Columns.Add(new DataColumn("Met Report")); //array
            SearchTable21.Columns.Add(new DataColumn("Emitter Category"));
            SearchTable21.Columns.Add(new DataColumn("Target Address"));
            SearchTable21.Columns.Add(new DataColumn("Target Status")); // array
            SearchTable21.Columns.Add(new DataColumn("Roll Angle"));
            SearchTable21.Columns.Add(new DataColumn("Service Identification"));
            SearchTable21.Columns.Add(new DataColumn("Quality Indicators")); //array
            SearchTable21.Columns.Add(new DataColumn("Mode S")); //array
            SearchTable21.Columns.Add(new DataColumn("Message amplitude"));
            SearchTable21.Columns.Add(new DataColumn("Receiver ID"));
            SearchTable21.Columns.Add(new DataColumn("Time Applicability\nfor position"));
            SearchTable21.Columns.Add(new DataColumn("Time Applicability\nfor velocity"));
            SearchTable21.Columns.Add(new DataColumn("Time Message Reception\nPosition"));
            SearchTable21.Columns.Add(new DataColumn("Time Message Reception\nVelocity"));
            SearchTable21.Columns.Add(new DataColumn("Time Message Reception\nPosition-High Precision")); // array
            SearchTable21.Columns.Add(new DataColumn("Time Message Reception\nVelocity-High Precision")); // array
            SearchTable21.Columns.Add(new DataColumn("Time ASTERIX\nReport Transmission"));
            SearchTable21.Columns.Add(new DataColumn("Trajectory Intent Data")); // array
            SearchTable21.Columns.Add(new DataColumn("Data ages")); // array
            SearchTable21.Columns.Add(new DataColumn("Service Management"));

            return SearchTable21;
        }
        public DataTable getSearchTableMixed()
        {
            DataTable SearchTableMixed = new DataTable();

            //Common Items
            SearchTableMixed.Columns.Add(new DataColumn("#"));
            SearchTableMixed.Columns.Add(new DataColumn("CAT"));
            SearchTableMixed.Columns.Add(new DataColumn("SIC"));
            SearchTableMixed.Columns.Add(new DataColumn("SAC"));
            SearchTableMixed.Columns.Add(new DataColumn("Target ID"));
            SearchTableMixed.Columns.Add(new DataColumn("Track Number"));
            SearchTableMixed.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            SearchTableMixed.Columns.Add(new DataColumn("Target Report"));
            SearchTableMixed.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            SearchTableMixed.Columns.Add(new DataColumn("Mode 3A Code"));
            SearchTableMixed.Columns.Add(new DataColumn("Mode S MB Data"));
            SearchTableMixed.Columns.Add(new DataColumn("Flight Level"));
            SearchTableMixed.Columns.Add(new DataColumn("Measured Height"));
            SearchTableMixed.Columns.Add(new DataColumn("Target Address"));
            SearchTableMixed.Columns.Add(new DataColumn("Amplitude"));

            //CAT10 Items
            SearchTableMixed.Columns.Add(new DataColumn("Target Size\n(Length x Width)"));
            SearchTableMixed.Columns.Add(new DataColumn("Target Heading"));
            SearchTableMixed.Columns.Add(new DataColumn("Message type"));
            SearchTableMixed.Columns.Add(new DataColumn("Position Polar\n(Distance, Angle)"));
            SearchTableMixed.Columns.Add(new DataColumn("Position Cartesian\n(X, Y)"));
            SearchTableMixed.Columns.Add(new DataColumn("Track Velocity Polar\n(GS, Track Angle)"));
            SearchTableMixed.Columns.Add(new DataColumn("Track Velocity Cartesian\n(Vx, Vy)"));
            SearchTableMixed.Columns.Add(new DataColumn("Track Status"));
            SearchTableMixed.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            SearchTableMixed.Columns.Add(new DataColumn("System Status"));
            SearchTableMixed.Columns.Add(new DataColumn("Pre Programmed MSG"));
            SearchTableMixed.Columns.Add(new DataColumn("Standard Deviation\nPosition\n(X, Y)"));
            SearchTableMixed.Columns.Add(new DataColumn("Covariance of deviation"));
            SearchTableMixed.Columns.Add(new DataColumn("Presence"));
            SearchTableMixed.Columns.Add(new DataColumn("Acceleration\n(Ax, Ay)"));

            // CAT21 Items
            SearchTableMixed.Columns.Add(new DataColumn("High Resolution\nPosition WGS-84\n(Latitude, Longitude)"));
            SearchTableMixed.Columns.Add(new DataColumn("Operational Status"));
            SearchTableMixed.Columns.Add(new DataColumn("AirSpeed\n(IAS, Mach)"));
            SearchTableMixed.Columns.Add(new DataColumn("True Airspeed"));
            SearchTableMixed.Columns.Add(new DataColumn("Airborne Ground Vector\n(GS, Track Angle)"));
            SearchTableMixed.Columns.Add(new DataColumn("Track Angle Rate"));
            SearchTableMixed.Columns.Add(new DataColumn("Selected Altitude"));
            SearchTableMixed.Columns.Add(new DataColumn("MOPS version"));
            SearchTableMixed.Columns.Add(new DataColumn("Magnetic Heading"));
            SearchTableMixed.Columns.Add(new DataColumn("Barometric Vertical Rate"));
            SearchTableMixed.Columns.Add(new DataColumn("Geometric Vertical Rate"));
            SearchTableMixed.Columns.Add(new DataColumn("Met Report"));
            SearchTableMixed.Columns.Add(new DataColumn("Emitter Category"));
            SearchTableMixed.Columns.Add(new DataColumn("Target Status"));
            SearchTableMixed.Columns.Add(new DataColumn("Roll Angle"));
            SearchTableMixed.Columns.Add(new DataColumn("Service Identification"));
            SearchTableMixed.Columns.Add(new DataColumn("Quality Indicators"));
            SearchTableMixed.Columns.Add(new DataColumn("Receiver ID"));
            SearchTableMixed.Columns.Add(new DataColumn("Time Applicability\nPosition"));
            SearchTableMixed.Columns.Add(new DataColumn("Time Applicability\nVelocity"));
            SearchTableMixed.Columns.Add(new DataColumn("Time Message Reception\nPosition"));
            SearchTableMixed.Columns.Add(new DataColumn("Time Message Reception\nVelocity"));
            SearchTableMixed.Columns.Add(new DataColumn("Time Message Reception\nPosition-High precision"));
            SearchTableMixed.Columns.Add(new DataColumn("Time Message Reception\nVelocity-High precision"));
            SearchTableMixed.Columns.Add(new DataColumn("Time ASTERIX\nReport Transmission"));
            SearchTableMixed.Columns.Add(new DataColumn("Trajectory Intent Data"));
            SearchTableMixed.Columns.Add(new DataColumn("Data ages"));
            SearchTableMixed.Columns.Add(new DataColumn("Service Management"));

            return SearchTableMixed;
        }
        public int gettimecorrectly(string[] tod)
        {
            int secabs = Convert.ToInt32(tod[0]) * 3600 + Convert.ToInt32(tod[1]) * 60 + Convert.ToInt32(tod[2]);
            return secabs;
        }
    }
}
