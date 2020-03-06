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
            foreach(char c in octeto)
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
            
            char[] octeto = Pos_Paquete.ToCharArray(0,2);
            string byte1 = octeto[0].ToString();
            string byte2 = octeto[1].ToString();
            // de hexa a binario
            string first = HexStringToBinary(byte1);
            string second = HexStringToBinary(byte2);
            string bin_octeto = first+second;
           
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
                char[] bits = octeto_bin.ToCharArray(0,8);

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
        public List<double> sumar_num_base_2(double exponente_LSB,string mensaje_bin_entero)
        {
            // devuelve un vector (lista) llamado vector en que cada posicion tiene el valor del complemento a dos
            List<double> vector = new List<double>();
            int longitud_entera = mensaje_bin_entero.Length;
            int j= mensaje_bin_entero.Length;
            for (int i=0;i<=longitud_entera;i++)
            {
                int x = 0;
                if (mensaje_bin_entero[j]==1)
                {
                    vector[x] = Math.Pow(2, exponente_LSB + Convert.ToDouble(i));
                    x = x + 1;
                    j = j - 1;
                }
                else { j = j - 1;  }
            }
            return vector;
            

        }
    }
}
