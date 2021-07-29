using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Instrumentation;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Media.Media3D;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Instrumentation;
using System.Net.NetworkInformation;
using System.Reflection;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Instrumentation;
using System.Net.NetworkInformation;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Instrumentation;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Instrumentation;
using System.Net.NetworkInformation;
using System.Reflection;




namespace utils
{
    public static class PUPPIUtils
    {
        public static Assembly getNonPUPPIAssembly()
        {
            Assembly a = null;
            StackFrame[] s = new StackTrace().GetFrames();
            //first get where PUPPI is
            bool pf = false;
            bool nf = false;
            foreach (StackFrame ss in s)
            {
                Assembly aa = ss.GetMethod().Module.Assembly;
                if (aa.GetName().FullName.StartsWith("PUPPI,"))
                {
                    pf = true;
                }
                if (pf == true)
                {
                    //first non ;PUPPI
                    if (aa.GetName().FullName.StartsWith("PUPPI,") == false)
                    {
                        a = aa;
                        return a;
                    }
                }
            }

            return a;
        }
    }

    public static class StringConstants
    {
        public static string processdebuglog = ".process. Node GUID ";
        public static string setdefaultoutputsdebuglog = "set default outputs for ";
        public static string constructorerrordebuglog = " constructor error: ";
        public static string beginupdateconnectionsdebuglog = "begin update connections";
        public static string endupdateconnectionsdebuglog = "end update connections";
        public static string connectionsredrawndebuglog = " connections redrawn";
        public static string beginregenerateconnectionsdebuglog = "begin regenerate connections";
        public static string endregenerateconnectionsdebuglog = "end regenerate connections";
        public static string startedfullprogramrundebuglog = "started full program run ";
        public static string endedfullprogramrundebuglog = "ended full program run ";
        public static string startedopeningfiledebuglog = "started opening file ";
        public static string failedtodeepcopy = "failed deep copy trying shallow copy";
        public static string failedtomakemethodmodule = "Failed to create method module:";
        public static string startedtodeletenode = "started deleting node ";
        public static string finisheddeletingnode = "finished deleting node ";
        public static string starteddeletingnodestack = "started deleting node stack ";
        public static string finisheddeletingnodestack = "finished deleting node stack ";
        public static string startedrepnodebox = "started replacing box for node ID ";
        public static string finishedrepnodebox = "finished replacing box for node ID ";
        public static string startedfittingtoots = "started fitting roots around node with ID ";
        public static string finishedfittingroots = "finished fitting roots around node with ID ";
        public static string startedupdatingcaption = "started updating caption for node ";
        public static string finishedupdatingcaption = "finished updating caption for node ";
        public static string startedupdatinginouts = "started updating inout labels for node  GUID ";
        public static string finishedupdatinginouts = "finished updating inout labels for node  GUID ";
        public static string begindrawingconn = "begin drawing connection from node id ";
        public static string finisheddrawingconn = "finished drawing connection from node id ";
        public static string failedtodrawarrow = "Failed to draw connector arrow \n";
        public static string collapselection = "collapsing selection";
        public static string srunscri = "running script";
        public static string rnc = "resetting node captions";
        public static string reano = "rearranging nodes";
     
     

    }
}
