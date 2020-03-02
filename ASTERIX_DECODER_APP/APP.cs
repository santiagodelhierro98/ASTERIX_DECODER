using System;
using System.Collections.Generic;
using System.Text;
using CLASSES;

namespace ASTERIX_DECODER_APP
{
    class APP
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\santi\Desktop\ASTERIX_DECODER\ASTERIX_DECODER_APP\smr_160510-lebl-220001.ast";
            Fichero ast = new Fichero(path);
            ast.leer();
        }
    }
}
