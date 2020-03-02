//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace CLASSES
//{
//    public class Metodos
//    {
//        private static readonly Dictionary<char, string> hexCharacterToBinary = new Dictionary<char, string>
//        {
//            { '0', "0000" },
//            { '1', "0001" },
//            { '2', "0010" },
//            { '3', "0011" },
//            { '4', "0100" },
//            { '5', "0101" },
//            { '6', "0110" },
//            { '7', "0111" },
//            { '8', "1000" },
//            { '9', "1001" },
//            { 'a', "1010" },
//            { 'b', "1011" },
//            { 'c', "1100" },
//            { 'd', "1101" },
//            { 'e', "1110" },
//            { 'f', "1111" }
//        };
//        public string HexStringToBinary(string hex)
//        {
//            StringBuilder result = new StringBuilder();
//            foreach (char c in hex)
//            {
//                // Si no le pasas caracteres en Hexadecimal petara
//                result.Append(hexCharacterToBinary[char.ToLower(c)]);
//            }
//            return result.ToString();
//        }
//        public string Poner_Zeros_Delante(string octeto)
//        {
//            // Si hay un solo caracter, añadimos un 0 delante
//            if (octeto.Length == 1)
//            {
//                octeto.PadLeft(2, '0');
//            }
//            else { }

//            return octeto;
//        }
//        public string[] Octeto_A_Bin(string Pos_Paquete)
//        {
//            // convertir un octeto en Hexa a un string con los dos caracteres en binario
//            string[] octeto = Pos_Paquete.Split();
//            string byte1 = octeto[0];
//            string byte2 = octeto[1];
//            // de hexa a binario
//            string first = HexStringToBinary(byte1);
//            string second = HexStringToBinary(byte2);
//            string[] bin_octeto = new string[first, second]();

//            return bin_octeto;
//        }
//        public int Longitud_Paquete(string[] paquete)
//        {
//            string octeto = Poner_Zeros_Delante(paquete[2]);

//            // como ningun paquete va a sobrepasar 256 de largo, solo nos fijamos en el 2o octeto de longitud
//            string[] octeto_bin = Octeto_A_Bin(octeto);

//            // de binario a decimal
//            Convert.ToInt32(octeto_bin[0], 2).ToString();
//            Convert.ToInt32(octeto_bin[1], 2).ToString();
//            Int32 length = Convert.ToInt32(octeto_bin[0] + octeto_bin[1]);

//            return length;
//        }
//        public List<string> FSPEC(string[] paquete)
//        {
//            bool end_of_fspec = false;
//            List<string> FSPEC = new List<string>();
//            for (int i = 0; end_of_fspec == true; i++)
//            {
//                string octeto = Poner_Zeros_Delante(paquete[i]);
//                string[] octeto_bin = Octeto_A_Bin(octeto);

//                FSPEC.Add(octeto_bin[0]);
//                FSPEC.Add(octeto_bin[1]);
//                FSPEC.Add(octeto_bin[2]);
//                FSPEC.Add(octeto_bin[3]);
//                // añadimos al vector FSPEC todos los bits menos el ultimo
//                FSPEC.Add(octeto_bin[4]);
//                FSPEC.Add(octeto_bin[5]);
//                FSPEC.Add(octeto_bin[6]);

//                if (octeto_bin[7] == "1")
//                {
//                    end_of_fspec = false;
//                }
//                else
//                {
//                    end_of_fspec = true;
//                }
//            }
//            return FSPEC;
//        }
//    }
//}
