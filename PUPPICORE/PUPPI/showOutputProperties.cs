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
    public partial class showOutputProperties : Form
    {
        public List<string> proNamesValues;
        public showOutputProperties()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showOutputProperties_Load(object sender, EventArgs e)
        {
            if (proNamesValues!=null )
            {
                foreach (string lp in proNamesValues)
                {
                    listBox1.Items.Add(lp);   
                }
            }
        }
    }
}
