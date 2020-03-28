using System;
using System.Collections.Generic;
using System.Text;

namespace CLASSES
{
    
    public class Flight
    {
        // This class groups all flights independtly of its CAT
        // Variables of this class are Data Items shared between CATs

        public double CAT;
        public double SIC;
        public double SAC;
        public double TOD;
        public double lat; //from position in WGS84 coord
        public double lon;
        public double track_num;
        public string target_address;
        public string target_ID;
        public double FL; // flight level



    }
}
