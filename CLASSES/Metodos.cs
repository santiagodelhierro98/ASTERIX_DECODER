using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace CLASSES
{
    public class Metodos
    {
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
    }
}
