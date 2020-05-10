using System;
using System.Collections.Generic;
using System.Text;

namespace CLASSES
{
    // La idea d'aquest extra points és determinar en un radi màxim de 10MN del ARP de l'Aeroport, la precissió dels tràfics a l'aire detectats per la MLAT Cat 10,
    // comparant-los amb el que el ADSB v2.1 detecta d'aquests mateixos tràfics, sempre i quan la versió MOPS dels transponders dels tràfics sigui igual a 2.

    //para definir que un avión está a máximo 10MN del ARP de LEBL: convertimos estas 10MN en su correspondiente incremento en lat lon (1 min 1 MN)(ya filtramos por FL, así evitamos aviones en tierra)

    public class Extrapoints
    {
        //primer: filtrar per MLAT Cat 10 (Target_Rep_Descript[0] == "Mode S Multilateration")
        Metodos M = new Metodos();

        public double ARP_lat = 41.0 + (17.0 / 60.0) + (49.0 / 3600.0) + (426.0 / 3600000.0);
        public double ARP_lon = 2.0 + (4.0 / 60.0) + (42.0 / 3600.0) + (410.0 / 3600000.0);
     
        public bool checkdistanceMLAT(CAT10 C10)
        {
            // el módulo del segmento que une la posición del ARP con la posición del avión debe ser inferior a 10 MN
            double pos_x = C10.Pos_Cartesian[0];
            double pos_y = C10.Pos_Cartesian[0];
            double modulo = Math.Sqrt(Math.Pow(pos_x, 2) + Math.Pow(pos_y, 2));
            if (modulo <= 10 * 1852) { return  true; }
            else { return false; }
        }
        public bool checkdistanceADSB(CAT21 C21,double x,double y)
        {

            // el módulo del segmento que une la posición del ARP con la posición del avión debe ser inferior a 10 MN
            double modulo = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            if (modulo <= 10 * 1852) { return true; }
            else { return false; }
        }
        double x;
        double y;
        double z;
        public void fromlatlontocartesian(double lat,double lon,double h)
        {
            //las coordenas latitud y longitud estan referenciadas al sistema elipsoidal WGS-84
            //excentricidad
            double a = 6378137.0; //in m
            double b = 6356752.3142; //in m
            double e = Math.Sqrt(1 - (Math.Pow(b, 2) / Math.Pow(a, 2)));
            x = (a * Math.Cos(lat) / Math.Sqrt(1 + (1 - Math.Pow(e, 2) * Math.Pow(Math.Tan(lon), 2)))) + h * Math.Cos(lat) * Math.Cos(lon);
            y = (a * Math.Sin(lat) / Math.Sqrt(1 + (1 - Math.Pow(e, 2) * Math.Pow(Math.Tan(lon), 2)))) + h * Math.Sin(lat) * Math.Cos(lon);
            z = (a * (1 - Math.Pow(e, 2) * Math.Sin(lon) / Math.Sqrt(1 - Math.Pow(e, 2) * Math.Pow(Math.Sin(lon), 2)))) + h * Math.Sin(lon);
        }
        public double x_adsb()
        { return x; }
        public double y_adsb()
        { return y; }

    }
}
