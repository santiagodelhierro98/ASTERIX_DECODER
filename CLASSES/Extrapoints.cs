using System;
using System.Collections.Generic;
using System.Text;

namespace CLASSES
{
    // La idea d'aquest extra points és determinar en un radi màxim de 10MN del ARP de l'Aeroport, la precissió dels tràfics a l'aire detectats per la MLAT Cat 10,
    // comparant-los amb el que el ADSB v2.1 detecta d'aquests mateixos tràfics, sempre i quan la versió MOPS dels transponders dels tràfics sigui igual a 2.


    public class Extrapoints
    {
        Fichero F;
        //primer: filtrar per MLAT Cat 10 (Target_Rep_Descript[0] == "Mode S Multilateration")
        List<CAT10> MLATList = new List<CAT10>();
        public List<CAT10> returnMLATList()
        {
            
            for (int i=0;i<=F.lengthlistaCAT10();i++)
            {
                CAT10 C10 = F.getCAT10(i);
                if (C10.Target_Rep_Descript[0] == "Mode S Multilateration")
                {
                    MLATList.Add(F.getCAT10(i));
                }
                // tambien filtrat por target id (que lo tenga) en el adsb tb hay k hacer lo mismo)
                //tambien hay fitlrar por FL (los que estan volando)
                //ID//Diferencia de hora//Diferencia de Posicion H//Accuracy Horizontal//Diferencia de Posición V//Accuracy Vertical

            }
            return MLATList;
        }

        //segon: definir el límit máxim de 10MN del ARP 
        public double ARP_lat= 41.0 + (17.0 / 60.0) + (49.0 / 3600.0) + (426.0 / 3600000.0);
        public double ARP_lon = 2.0 + (4.0 / 60.0) + (42.0 / 3600.0) + (410.0 / 3600000.0);


    }
}
