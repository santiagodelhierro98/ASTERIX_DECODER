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
        float[] Pos_PolarCoord; //rho,theta
        float[] Pos_WGS84;
        float[] Pos_Cartesian;
        string Mode3A_Code;
        string FL_Binary;
        float Height;   //ft
        float Amplitude;    //dBm
        float Time_Day; //seconds
        Int16 Track_Num;
        string Track_Status;
        float[] Track_Vel_Polar;    // (NM/s, degrees)
        float[] Track_Vel_Cartesian;    // m/s
        float Acceleration; // m/s^2
        string Target_Add;
        string Target_ID;
        string Mode_SMB;
        float[] Target_Size_Heading;    // m,degrees
        float[] Presence;   // rho,theta
        string Fleet_ID;
        string Pre_Prog_Message;
        float StndrdDev_Position; // m^2
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
            List<string> FSPEC =  new List<string>(M.FSPEC(paquete0));
            Console.ReadKey();

            // hola
        }
    }
}
