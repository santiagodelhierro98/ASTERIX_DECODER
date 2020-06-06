using System;

namespace CLASSES
{
    // La idea d'aquest extra points és determinar en un radi màxim de 10MN del ARP de l'Aeroport, la precissió dels tràfics a l'aire detectats per la MLAT Cat 10,
    // comparant-los amb el que el ADSB v2.1 detecta d'aquests mateixos tràfics, sempre i quan la versió MOPS dels transponders dels tràfics sigui igual a 2.

    //para definir que un avión está a máximo 10MN del ARP de LEBL: convertimos estas 10MN en su correspondiente incremento en lat lon (1 min 1 MN)(ya filtramos por FL, así evitamos aviones en tierra)

    public class Extrapoints
    {
        //primer: filtrar per MLAT Cat 10 (Target_Rep_Descript[0] == "Mode S Multilateration")
        Metodos M = new Metodos();

        double ARP_lat = 41.0 + (17.0 / 60.0) + (49.0 / 3600.0) + (426.0 / 3600000.0);
        double ARP_lon = 2.0 + (4.0 / 60.0) + (42.0 / 3600.0) + (410.0 / 3600000.0);
        public double checkdistanceMLAT_Acc(double lat, double lon)
        {
            double[] cart = M.WGStoCartesian(lat, lon);
            return Math.Sqrt(Math.Pow(cart[0], 2) + Math.Pow(cart[1], 2)) / 1851.85185185185;
        }
        public double checkdistanceMLAT(CAT10 C10)
        {
            // el módulo del segmento que une la posición del ARP con la posición del avión debe ser inferior a 10 MN
            double pos_x = C10.Pos_Cartesian[0];
            double pos_y = C10.Pos_Cartesian[1];
            return Math.Sqrt(Math.Pow(pos_x, 2) + Math.Pow(pos_y, 2)) / 1851.85185185185;
        }
        public double checkdistanceADSB(CAT21 C21)
        {
            // comparamos los módulos de dos segmentos : el que une el ARP con 10 MN y el que une el ARP con el avión
            double lat = C21.Lat_WGS_84;
            double lon = C21.Lon_WGS_84;
            // conversion de WGS a Cart
            double[] cart = M.WGStoCartesian(lat, lon);

            return Math.Sqrt(Math.Pow(cart[0], 2) + Math.Pow(cart[1], 2)) / 1851.85185185185;
        }
        public double Compute_GVA(CAT21 C21)
        {
            string GVA = C21.Quality_Indicators[5];
            if (GVA == "1") { return 150.0; }
            if (GVA == "2") { return 45.0; }
            else { return double.NaN; }
        }
        public double Horizontal_Accuracy_Pos(CAT21 list)
        {
            string NACp = list.Quality_Indicators[2];
            if (NACp == "0") { return 18520.0; }
            if (NACp == "1") { return 18520.0; }
            if (NACp == "2") { return 7408.0; }
            if (NACp == "3") { return 3704.0; }
            if (NACp == "4") { return 1852.0; }
            if (NACp == "5") { return 926.0; }
            if (NACp == "6") { return 555.6; }
            if (NACp == "7") { return 185.2; }
            if (NACp == "8") { return 92.6; }
            if (NACp == "9") { return 30.0; }
            if (NACp == "10") { return 10.0; }
            if (NACp == "11") { return 3.0; }
            else { return double.NaN; }
        }
        public double checkdistanceADSBv23(CAT21_v23 C21)
        {
            // comparamos los módulos de dos segmentos : el que une el ARP con 10 MN y el que une el ARP con el avión
            double lat = C21.Lat_WGS_84;
            double lon = C21.Lon_WGS_84;
            // conversion de WGS a Cart
            double[] cart = M.WGStoCartesian(lat, lon);

            return Math.Sqrt(Math.Pow(cart[0], 2) + Math.Pow(cart[1], 2)) / 1851.85185185185;
        }
       
    }
}
