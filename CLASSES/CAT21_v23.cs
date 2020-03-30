using System;
using System.Collections.Generic;
using System.Text;

namespace CLASSES
{
    public class CAT21_v23
    {
        Metodos Met = new Metodos();

        // Definir los Data Items como variables, para procesar las que están en el paquete (las que son 1)
        // El tipo de cada variable depende de la precisión con la que se nos proporciona (especificado pdf CAT21)









        public void Decode21_v23(string[] paquete, int q)
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
            // definicion de cada data item segun ADS-B v23 Reports UAP

            if (FSPEC[0] == "1")
            {
            }
        }
    }
}
