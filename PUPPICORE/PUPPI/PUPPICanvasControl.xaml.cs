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
using System.ComponentModel; 


namespace PUPPIGUI
{
    //[LicenseProvider(typeof(LicFileLicenseProvider ) ) ]
  /// <summary>
  /// User Control for PUPPI Canvas. Inherited by WPF controls and Formcontrols. Cannot be used directly.
  /// </summary>
     partial class PUPPICanvasControl : UserControl
    {
        //public bool isInDesignMode1
        //{
        //    get
        //    {
        //        System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
        //        bool res = process.ProcessName == "devenv";
        //        process.Dispose();
        //        return res;
        //    }
        //}

        //public static bool IsInDesignMode2()
        //{
        //    if (System.Reflection.Assembly.GetExecutingAssembly().Location.Contains("VisualStudio"))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        

     //   private License license = null;
        internal PUPPICanvasControl()
        {
           // if (isInDesignMode)
           // {
           //     System.Windows.Forms.MessageBox.Show("Invalid License");     
           // }

           // if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
          //  {
          //      System.Windows.Forms.MessageBox.Show("Invalid License"); 
          //  }
            InitializeComponent();
            //if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            //{
            //    System.Windows.Forms.MessageBox.Show("Invalid License"); 
            //}
          //  if (IsInDesignMode2() )
          //  {
         //       System.Windows.Forms.MessageBox.Show("Invalid License"); 
         //   }
           
        //    license = LicenseManager.Validate(typeof(PUPPICanvasControl),this);
            
   
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (license != null)
        //        {
        //            license.Dispose();
        //            license = null;
        //        }
        //    }
        //}
    }
}
