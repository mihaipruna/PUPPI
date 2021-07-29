using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PUPPICADBeta
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //to embed resources
            //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Form1());
            }
            catch (Exception exy)
            {
                MessageBox.Show("Critical error. Clearing list of dll files to load for runtime PUPPIModule creation from settings. Please restart PUPPICAD. See errorlog.txt for details.If error persists, remove any newly added files from the PluginPUPPIModules folder.");
                using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@".\errorlog.txt"))
                {
                    
                    file.WriteLine(DateTime.Now.ToString()   );
                    file.WriteLine(exy.ToString()); 
                }
                Properties.Settings.Default.generateModulesFrom.Clear();

                Properties.Settings.Default.Save();

            }
        }
        //internal static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

        //    dllName = dllName.Replace(".", "_");

        //    if (dllName.EndsWith("_resources")) return null;

        //    System.Resources.ResourceManager rm = new System.Resources.ResourceManager(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name    + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

        //    byte[] bytes = (byte[])rm.GetObject(dllName);

        //   if (bytes!=null && bytes.Length>0)  return System.Reflection.Assembly.Load(bytes); else return null;
        //}
    }
}
