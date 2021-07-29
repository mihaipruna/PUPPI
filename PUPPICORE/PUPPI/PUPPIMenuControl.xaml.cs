using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PUPPIGUI
{
    /// <summary>
    /// User Control for PUPPI Menu where Modules are stored. Inherited by WPF controls and Formcontrols. Cannot be used directly.
    /// </summary>
    public partial class PUPPIMenuControl : UserControl
    {
        internal PUPPIMenuControl()
        {
            InitializeComponent();
        }
    }
}
