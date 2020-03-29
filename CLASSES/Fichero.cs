using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Linq;


namespace CLASSES
{
    public class Fichero
    {
        string path;
        public int CAT;

        List<CAT10> listaCAT10 = new List<CAT10>();
        //List<CAT20> listaCAT20 = new List<CAT20>();
        List<CAT21> listaCAT21 = new List<CAT21>();

        //Reduced table with items for display on map
        DataTable tablacat10reducida = new DataTable();
        DataTable tablacat21reducida = new DataTable();
        DataTable multiplecattablereducida = new DataTable();


        DataTable tablaCAT10 = new DataTable();
        //DataTable tablaCAT20 = new DataTable();
        DataTable tablaCAT21 = new DataTable();

        public Fichero(string nombre)
        {
            this.path = nombre;
            this.createtable();
        }
        public List<CAT10> getListCAT10()
        {
            return listaCAT10;
        }

        public List<CAT21> getListCAT21()
        {
            return listaCAT21;
        }
        public DataTable getmultiplecattablereducida()
        {
            return multiplecattablereducida;
        }
        public DataTable gettablacat10reducida()
        {
            return tablacat10reducida;
        }
        public DataTable gettablacat21reducida()
        {
            return tablacat21reducida;
        }
        public void leer()
        {
            //StreamReader fichero = new StreamReader(path);
            //string linea = fichero.ReadLine();
            byte[] fileBytes = File.ReadAllBytes(path);
            List<byte[]> listabyte = new List<byte[]>();
            int i = 0;
            int contador = fileBytes[2];
            // int length = 0;

            while (i < fileBytes.Length)
            {
                byte[] array = new byte[contador];
                for (int j = 0; j < array.Length; j++)
                {
                    array[j] = fileBytes[i];
                    i++;
                }
                listabyte.Add(array);
                //length += array.Length;
                if (i + 2 < fileBytes.Length)
                {
                    contador = fileBytes[i + 2];
                }
            }

            List<string[]> listahex = new List<string[]>();
            for (int x = 0; x < listabyte.Count; x++)
            {
                byte[] buffer = listabyte[x];
                string[] arrayhex = new string[buffer.Length];
                for (int y = 0; y < buffer.Length; y++)
                {
                    arrayhex[y] = buffer[y].ToString("X");
                }
                listahex.Add(arrayhex);
            }

            //int p = 0;
            for (int q = 0; q < listahex.Count; q++)
            {
                string[] arraystring = listahex[q];
                CAT = int.Parse(arraystring[0], System.Globalization.NumberStyles.HexNumber);

                if (CAT == 10)
                {
                    CAT10 C10 = new CAT10();
                    C10.Decode10(arraystring);
                    listaCAT10.Add(C10);
                    //Flight vuelo = new Flight();
                    //vuelo.getcat10(C10);
                    multiplecattablereducida.Rows.Add(C10.getTargetID10(), C10.getTOD10(), C10.getSIC10(), C10.getSAC10(), C10.getLAT10(), C10.getLON10(), C10.getTargetAddress10(), C10.getTrackNum10());
                    tablacat10reducida.Rows.Add(C10.getTargetID10(), C10.getTOD10(), C10.getSIC10(), C10.getSAC10(), C10.getLAT10(), C10.getLON10(), C10.getTargetAddress10(), C10.getTrackNum10());
                }

                else if (CAT == 21)
                {
                    CAT21 C21 = new CAT21();
                    C21.Decode21(arraystring, q);
                    listaCAT21.Add(C21);
                    multiplecattablereducida.Rows.Add(C21.getTargetID21(), C21.getTOD21(), C21.getSIC21(), C21.getSAC21(), C21.getLAT21(), C21.getLON21(), C21.getTargetAddress21(), C21.getTrackNum21());
                    tablacat21reducida.Rows.Add(C21.getTargetID21(), C21.getTOD21(), C21.getSIC21(), C21.getSAC21(), C21.getLAT21(), C21.getLON21(), C21.getTargetAddress21(), C21.getTrackNum21());

                }
            }
        }
        public void createtable()
        {
            //we must declare table columns

            //multiple CAT table
            multiplecattablereducida.Columns.Add(new DataColumn("Target ID"));
            multiplecattablereducida.Columns.Add(new DataColumn("Time of Day"));
            multiplecattablereducida.Columns.Add(new DataColumn("SIC"));
            multiplecattablereducida.Columns.Add(new DataColumn("SAC"));
            multiplecattablereducida.Columns.Add(new DataColumn("Latitude (º)"));
            multiplecattablereducida.Columns.Add(new DataColumn("Longitude (º)"));
            multiplecattablereducida.Columns.Add(new DataColumn("Target Address"));
            multiplecattablereducida.Columns.Add(new DataColumn("Track Number"));

            //CAT10 table
            tablacat10reducida.Columns.Add(new DataColumn("Target ID"));
            tablacat10reducida.Columns.Add(new DataColumn("Time of Day"));
            tablacat10reducida.Columns.Add(new DataColumn("SIC"));
            tablacat10reducida.Columns.Add(new DataColumn("SAC"));
            tablacat10reducida.Columns.Add(new DataColumn("Latitude (º)"));
            tablacat10reducida.Columns.Add(new DataColumn("Longitude (º)"));
            tablacat10reducida.Columns.Add(new DataColumn("Target Address"));
            tablacat10reducida.Columns.Add(new DataColumn("Track Number"));

            //CAT21 table
            tablacat21reducida.Columns.Add(new DataColumn("Target ID"));
            tablacat21reducida.Columns.Add(new DataColumn("Time of Day"));
            tablacat21reducida.Columns.Add(new DataColumn("SIC"));
            tablacat21reducida.Columns.Add(new DataColumn("SAC"));
            tablacat21reducida.Columns.Add(new DataColumn("Latitude (º)"));
            tablacat21reducida.Columns.Add(new DataColumn("Longitude (º)"));
            tablacat21reducida.Columns.Add(new DataColumn("Target Address"));
            tablacat21reducida.Columns.Add(new DataColumn("Track Number"));
        }
    }
}