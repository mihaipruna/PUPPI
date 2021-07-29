using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPPIDEBUG
{
    /// <summary>
    /// Popup Form to display debug information.
    /// </summary>
    public partial class debuglog : Form
    {
        public debuglog()
        {
            InitializeComponent();
        }

        public void writelog(string textme)
        {
            placeloghere.Text = textme;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();  
        }
    }
    /// <summary>
    /// Debugger class to store a log of actions performed and display in Debug form on exit.
    /// </summary>
    public static  class PUPPIDebugger
    {
        public static bool debugenabled = false;
    public static string logstr="";
        //number of connections redrawn
    public static int conns_Redrawn = 0;
        public static void log(string item)
    {
        StringBuilder sb = new StringBuilder(logstr);
        sb.Append("\n"); 
        sb.Append(DateTime.Now+":"+DateTime.Now.Millisecond  );
        sb.Append(": ");
        sb.Append(item);
        logstr = sb.ToString();
        try
        {
            System.Windows.Clipboard.SetData(DataFormats.Text, logstr);
        }
            catch
        {

        }
 
    }
    }
       
   
}
