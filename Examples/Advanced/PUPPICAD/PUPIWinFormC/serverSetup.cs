using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPPI
{
    public partial class PUPPICADserverSetup : Form
    {
        string ips = "";
        int prts = 0;
        string pps = "";
        bool isRunning = false;
        public PUPPICADserverSetup()
        {
            InitializeComponent();
        }

        private void lip_Click(object sender, EventArgs e)
        {

        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            ips = iptxt.Text;
            try
            {
                prts = Convert.ToInt16(portxt.Text);
            }
            catch
            {
                prts = 0;
            }
            pps = passtxt.Text;
            isRunning = runServer.Checked;

            PUPPICADBeta.Properties.Settings.Default.serverSettings = saveInSettings();
            PUPPICADBeta.Properties.Settings.Default.Save();    
            this.Close(); 
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void PUPPICADserverSetup_Load(object sender, EventArgs e)
        {
            loadFromSettings(PUPPICADBeta.Properties.Settings.Default.serverSettings);
            iptxt.Text = ips;
            portxt.Text = prts.ToString() ;
            passtxt.Text = pps;
            runServer.Checked = isRunning;  
            
        }
        int sc1 = 45;
        int sc2 = 3;
        public string saveInSettings()
        {
            byte[] PtoBytes = Encoding.ASCII.GetBytes(pps);
            byte[] TPtoBytes = new byte[PtoBytes.Length * 2];
            int j = 0;
            for (int i = 0; i < PtoBytes.Length; i++)
            {
                int newb = PtoBytes[i] + sc1 + sc2;
                if (newb > 126)
                {
                    byte tbt = Convert.ToByte(newb - 126);
                    if (tbt < 32)
                    {
                        TPtoBytes[j] = Convert.ToByte(tbt + 32);
                        j++;
                        TPtoBytes[j] = Convert.ToByte(124);
                        j++;
                    }
                    else
                    {
                        TPtoBytes[j] = Convert.ToByte(tbt);
                        j++;
                        TPtoBytes[j] = Convert.ToByte(125);
                        j++;
                    }
                }
                else
                {
                    TPtoBytes[j] = Convert.ToByte(newb);
                    j++;
                    TPtoBytes[j] = Convert.ToByte(126);
                    j++;
                }
            }
            TPtoBytes = TPtoBytes.Reverse().ToArray();
            string smps = Encoding.ASCII.GetString(TPtoBytes);

            return smps + "_|_|_" + ips + "_|_|_" + prts.ToString()+"_|_|_"+isRunning.ToString()  ;
        }

        public void loadFromSettings(string savedSettings)
        {
            try
            {
                string[] seppa = { "_|_|_" };
                string[] splitta = savedSettings.Split(seppa, StringSplitOptions.None);
                prts = Convert.ToInt16(splitta[2]);
                ips = splitta[1];
                string smps = splitta[0];
                byte[] bita = new byte[smps.Length / 2];
                byte[] sbita = Encoding.ASCII.GetBytes(smps).Reverse().ToArray();
                int j = 0;
                for (int i = 0; i < sbita.Length - 1; i += 2)
                {
                    byte b1 = sbita[i];
                    byte b2 = sbita[i + 1];
                    int it1 = b1;
                    if (b2 == 124) it1 += (126 - 32);
                    if (b2 == 125) it1 += 126;
                    if (b2 == 126) it1 += 0;
                    byte balao = Convert.ToByte(it1 - sc1 - sc2);
                    bita[j] = balao;
                    j++;
                }
                pps = Encoding.ASCII.GetString(bita);
                isRunning = Convert.ToBoolean(splitta[3]);   
            }
            catch
            {
                pps = "";
                ips = "error";
                prts = 0;
                isRunning = false;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            string pubIp = "";
            try
            {
                pubIp = new System.Net.WebClient().DownloadString("https://api.ipify.org");
            }
            catch
            {
                pubIp = "failedpublic";
            }
            MessageBox.Show("Define Server IP address, port and password for client to connect." + "\n" +
                "Client can retrieve and set node output values and run programs on server canvas using modules from Interoperability menu." + "\n"
                +
                "To connect outside your network, the PUPPICAD server and client applications need to be enabled in the firewall." + "\n" +
                "and the port needs to be open / forwarded. External clients should use the public IP " + pubIp+"\n" +
                "Steps for allowing external connections could allow unauthorized access. Use this feature at your own risk."
                );
        }
    }
}
