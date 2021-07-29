using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPPIGUI
{
    public partial class ListForm : Form
    {
        public ListForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListForm_Load(object sender, EventArgs e)
        {

        }

        public void addListItem(string newItem)
        {
            listBox1.Items.Add(newItem);

        }
        public void setTitle(string newTitle)
        {
            Text = newTitle;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
