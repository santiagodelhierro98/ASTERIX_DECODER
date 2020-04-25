using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ASTERIX_APP
{
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();

            HelpText1.Content = "Consult and learn here all about ASTERIX APP";
            HelpText2.Content = "Tracking Table:\n\n" +
                "This table shows you ALL the asterix file content.\n" +
                "Whith the search tools above you can filter the information\n" +
                "shown in the table.\n\n" +
                "You can search by 'Target Identification' (Callsign),\n" +
                "or simply by the desired row number.\n\n" +
                "Try also to double-click the 'Click to View' cells, they have\n" +
                "a lot of information to show!";

            HelpText3.Content = "Tracking Map:\n\n" +
                "Here you can run a simulation of the loaded file's information.\n\n" +
                "Click on START to run the simualtion, you may see some airplanes\n" +
                "displayed on the map. If the duration of a second is too large to you,\n" +
                "you can speed-up the simulation by clicking on the SPEED buttons.\n\n" +
                "Remember that the table shown is a subset of the Tracking Table,\n";
        }
    }
}
