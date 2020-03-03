using System;
using System.Collections.Generic;
using System.Text;

namespace CLASSES
{
    public class CAT21
    {
        // Metodos M = new Metodos();

        // Definir los Data Items como variables, para procesar las que están en el paquete (las que son 1)
        // El tipo de cada variable depende de la precisión con la que se nos proporciona (especificado pdf CAT21)

        string Aircraft_Op_Stat;
        string Data_Source_ID;
        string Service_ID;
        string Service_Management;
        Int16 Emitter_Cat;
        string Target_Report_Desc;
        string Mode3A_Code;
        float ToA_Position; // seconds, ToA=Time of Aplicability
        float ToA_Velocity; // seconds
        float ToMR_Position;    // seconds, ToMR=Time of Message Reception
        float ToMR_Position_HP; // seconds, HP=High Precision
        float ToMR_Velocity;    // seconds
        float ToMR_Velocity_HP; // seconds
        float ToRT; // seconds, ToRT=Time of Report Transmission
        string Target_Add;
        string Quality_Indicators;
        string Trajectory_Intent;
        float[] Pos_WGS84;  // degrees
        float[] Pos_WGS84_HP;   // degrees
        float Amplitude;    // dBm
        float Height;   // feet
        float FL;
        Int16 Selected_Alt; // feet
        Int16 FinalState_Selected_Alt; // feet
        float Airspeed; // kt
        Int16 TAS;  // kt
        float Magnetic_Heading; // degrees
        float Barometric_VR;    // feet/min, VR=Vertical Rate
        float Geometric_VR; // feet/min
        float[] Ground_Vector;
        Int16 Track_Num;
        float Track_AR; // degrees/second, AR=Angle Rate
        string Target_ID;
        string Target_Status;
        string MOPS_Version;
        string Meteo_Info;
        float Roll; // degrees
        string Mode_SMB;
        string ACAS_Resolutuion_Advisory;
        string Surface_Capabilities_Characts;
        string Data_Ages;
        string Reciever_ID;

        public void Decode21(string[] paquete)
        {
            Metodos M = new Metodos();
            int longitud = M.Longitud_Paquete(paquete);
            string[] paquete0 = new string[longitud];
            for (int i = 0; i < longitud; i++)
            {
                
                paquete0[i] = M.Poner_Zeros_Delante(paquete[i]);
            }
            Console.WriteLine(paquete0);
            Console.ReadKey();
        }
    }
}
