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
        List<CAT10> MLATList = new List<CAT10>();
        List<CAT21_v23> ADSBList = new List<CAT21_v23>();
        Metodos M = new Metodos();

        public double ARP_lat = 41.0 + (17.0 / 60.0) + (49.0 / 3600.0) + (426.0 / 3600000.0);
        public double ARP_lon = 2.0 + (4.0 / 60.0) + (42.0 / 3600.0) + (410.0 / 3600000.0);
     
        public List<CAT10> returnMLATList(Fichero Fi) //este tendria que ser el Fi_ADSB
        { 
            for (int i=0;i<Fi.lengthlistaCAT10();i++)
            {
                // filtrar por target id (que lo tenga) en el adsb tb hay k hacer lo mismo)
                //tambien hay que filtrar por FL (los que estan volando) y por maximo 10MN del ARP 
                //ID//Diferencia de hora//Diferencia de Posicion H//Accuracy Horizontal//Diferencia de Posición V//Accuracy Vertical
                CAT10 C10 = Fi.getCAT10(i);
                bool modulo = checkdistanceMLAT(C10);
                if (C10.Target_Rep_Descript[0] == "Mode S Multilateration" && C10.Target_ID != null && C10.FL[2] != null && modulo==true)
                {
                    MLATList.Add(Fi.getCAT10(i));
                }
            }
            return MLATList;
        }
        public List<CAT21_v23> returnADSBList(Fichero Fi) //este tendria que ser el Fi_ADSB
        {
            for (int i = 0; i < Fi.lengthlistaCAT21_v23(); i++)
            {
                // tambien filtrar por target id (que lo tenga) en el adsb tb hay k hacer lo mismo) y ademas squitter version 2
                //tambien hay que filtrar por FL (los que estan volando) y por maximo 10MN del ARP 
                //ID//Diferencia de hora//Diferencia de Posicion H//Accuracy Horizontal//Diferencia de Posición V//Accuracy Vertical

                CAT21_v23 C21_v23 = Fi.getCAT21_v23(i);
                bool modulo = checkdistanceADSB(C21_v23);

                if (C21_v23.Target_ID != null && C21_v23.FL != 0 && modulo == true )
                {
                    ADSBList.Add(Fi.getCAT21_v23(i));
                }
            }
            return ADSBList;
        }
        public bool checkdistanceMLAT(CAT10 C10)
        {
            // el módulo del segmento que une la posición del ARP con la posición del avión debe ser inferior a 10 MN
            double lat_MLAT = M.cartesiantolatmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
            double lon_MLAT = M.cartesiantolonmlat(C10.Pos_Cartesian[0], C10.Pos_Cartesian[1]);
            double modulo = Math.Sqrt(Math.Pow((lat_MLAT - ARP_lat), 2) + Math.Pow((lon_MLAT - ARP_lon), 2));
            if (modulo <= 10) { return  true; }
            else { return false; }
        }
        public bool checkdistanceADSB(CAT21_v23 C21_v23)
        {
            //límite de 10 MN (1 min 1 MN)

            // el módulo del segmento que une la posición del ARP con la posición del avión debe ser inferior a 10 MN
            double modulo = Math.Sqrt(Math.Pow((C21_v23.Lat_WGS_84 - ARP_lat), 2) + Math.Pow((C21_v23.Lon_WGS_84 - ARP_lon), 2));
            if (modulo <= 10) { return true; }
            else { return false; }
        }

    }
}
