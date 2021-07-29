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
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Collections;
using System.Collections.ObjectModel;
using PUPPIModel;
using System.Reflection;


namespace PUPPIGUI
{
    //menu with drag buttons for each module
    internal class PUPPIMenu
    {
        //the actual control
        internal Border maindisplay;

        internal Grid PUPPIMenuGrid;
        //scroll view needs to be in stack panel
        internal StackPanel msp;
        internal ScrollViewer menuwindow;
        //height of the elemnt
        internal double height;

        //width of the element
        internal double width;
        //width of the window to display in
        internal double winwidth;
        //height of the window to display in
        internal double winheight;
        internal int index;
        internal int maxelementsperrow;


        public PUPPIMenu(double left, double top, string menutitle = "Menu", double mywidth = 80, double myheight = 20, double menuwidth = 200, double menuheight = 100, int maxperrow = 5,string menuToolTip="")
        {

            if (menutitle == "")
            {
                menutitle = "NoAddMenuTitle";
            }
            if (myheight <= 0)
                height = PUPPIGUISettings.defaultMenuElementHeight;
            else height = myheight;
            if (mywidth <= 0)
                width = PUPPIGUISettings.defaultMenuElementWidth;
            else
                width = mywidth;

            if (menuheight <= 0)
                winheight = PUPPIGUISettings.defaultMenuWindowHeight;
            else winheight = menuheight;
            if (menuwidth <= 0)
                winwidth = PUPPIGUISettings.defaultMenuWindowWidth;
            else
                winwidth = menuwidth;
            if (maxperrow <= 0)
                maxelementsperrow = PUPPIGUISettings.defaultMenuMaxPerRow;
            else
                maxelementsperrow = maxperrow;
            index = 0;
            maindisplay = new Border();
            //maindisplay.Height = winheight;
            //maindisplay.Width = winwidth;
            //maindisplay.Margin = new Thickness(left, top, 0, 0);

            maindisplay.VerticalAlignment = VerticalAlignment.Stretch;
            maindisplay.HorizontalAlignment = HorizontalAlignment.Stretch;    

            maindisplay.BorderThickness = new Thickness(PUPPIGUISettings.moduleMenuBorderWidth);
            //set border colors with default colors
            double border_Red = PUPPIGUISettings.menuBorderRed;
            double border_Green = PUPPIGUISettings.menuBorderGreen;
            double border_Blue = PUPPIGUISettings.menuBorderBlue;
            double border_Alpha = PUPPIGUISettings.menuBorderAlpha;
            maindisplay.BorderBrush = new SolidColorBrush(Color.FromArgb((byte)(border_Alpha * 255), (byte)(border_Red * 255), (byte)(border_Green * 255), (byte)(border_Blue * 255)));

            //scroll view
            menuwindow = new ScrollViewer();
            if (menuToolTip != "")
                menuwindow.ToolTip = menuToolTip; 
            //menuwindow.Margin = new Thickness(0, 0, 0, 0);
            //menuwindow.Width = winwidth;
            //menuwindow.Height = winheight;

            menuwindow.HorizontalAlignment = HorizontalAlignment.Stretch;
            menuwindow.VerticalAlignment = VerticalAlignment.Stretch;   

            menuwindow.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            menuwindow.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;


            


            //define stackpanel
            msp = new StackPanel();
            //msp.Height = menuwindow.Height;// -10;


            //menu title
            TextBlock tb = new TextBlock();
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.FontFamily = PUPPIGUISettings.bumeFoFa;   
            tb.TextAlignment = TextAlignment.Center;
            tb.Text = menutitle;
            if (menutitle != "NoAddMenuTitle")
                msp.Children.Add(tb);
            PUPPIMenuGrid = new Grid();
            //place it
            PUPPIMenuGrid.Margin = new Thickness(0, 0, 0, 0);
            // PUPPIMenuGrid.Width = width;
            PUPPIMenuGrid.Height = height;
            PUPPIMenuGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            PUPPIMenuGrid.VerticalAlignment = VerticalAlignment.Stretch;
            PUPPIMenuGrid.ShowGridLines = false;
            SolidColorBrush iobro = new SolidColorBrush();
            iobro.Color = Color.FromArgb(Convert.ToByte(255 * PUPPIGUISettings.menuBackgroundAlpha), Convert.ToByte(255 * PUPPIGUISettings.menuBackgroundRed), Convert.ToByte(255 * PUPPIGUISettings.menuBackgroundGreen), Convert.ToByte(255 * PUPPIGUISettings.menuBackgroundBlue));
            msp.Background = iobro;
            iobro.Freeze();  
            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            rowDef1.Height = new GridLength(height  + PUPPIGUISettings.moduleButtonSpacing);
            PUPPIMenuGrid.RowDefinitions.Add(rowDef1);


            //add this to the stackpanel
            // msp.Width = PUPPIMenuGrid.Width;
            msp.Children.Add(PUPPIMenuGrid);




            PUPPIMenuGrid.Width = maxelementsperrow * width;
         //   msp.Width = maxelementsperrow * width;

            msp.HorizontalAlignment = HorizontalAlignment.Stretch;
            msp.VerticalAlignment = VerticalAlignment.Stretch;   

            menuwindow.Content = msp;

            maindisplay.Child = menuwindow;


            //this.BorderThickness = new Thickness(2.0);


        }
        //sets the menu border and background colors
        public void setmenubordercolor(double border_Red = -1, double border_Green = -1, double border_Blue = -1, double border_Alpha = -1)
        {
            if (border_Red < 0)
            {
                border_Red = PUPPIGUISettings.menuBorderRed;
            }
            if (border_Green < 0)
            {

                border_Green = PUPPIGUISettings.menuBorderGreen;
            }
            if (border_Blue < 0)
            {
                border_Blue = PUPPIGUISettings.menuBorderBlue;
            }
            if (border_Alpha < 0)
            {
                border_Alpha = PUPPIGUISettings.menuBorderAlpha;
            }
            maindisplay.BorderBrush = new SolidColorBrush(Color.FromArgb((byte)(border_Alpha * 255), (byte)(border_Red * 255), (byte)(border_Green * 255), (byte)(border_Blue * 255)));
        }

        //public void addMenuButtonIconImage(PUPPIModule pm)
        //{


        //    ////icon test
        //    //Image img = new Image();
        //    ////img.Source = new BitmapImage(new Uri("PUPPImenuIcon.png",UriKind.Relative ) ); //  BitmapToBitmapSource(Resources.image_name)


        //    //BitmapImage bi3 = new BitmapImage();
        //    //bi3.BeginInit();
        //    //bi3.UriSource = new Uri("pack://application:,,,/WpfApplication2;component/Resources/PUPPImenuIcon.png");
        //    //bi3.EndInit();
        //    //// img.Stretch = Stretch.Fill;
        //    //img.Source = bi3;
        //    //img.HorizontalAlignment = HorizontalAlignment.Left;
        //    //img.Height = PUPPIGUISettings.defaultMenuElementHeight;
        //    ////new BitmapImage(new Uri("/Resources/PUPPImenuIcon.jpg"));
        //    //spg.Children.Add(img);  
        //}


        //adds draggble button to menu
        public void additem(PUPPIModule pm, bool cleanuptitle = true, double red = -1, double green = -1, double blue = -1, double alpha = -1)
        {
            if (red < 0)
                red = PUPPIGUISettings.menuButtonRed;
            if (green < 0)
                green = PUPPIGUISettings.menuButtonGreen;
            if (blue < 0) blue = PUPPIGUISettings.menuButtonBlue;
            if (alpha < 0)
                alpha = PUPPIGUISettings.menuButtonAlpha;
            StackPanel sp = new StackPanel();
            Grid spg = new Grid();
            Rectangle dragger = new Rectangle();
            //spg.Margin = new Thickness(10);
            //Ellipse dragger = new Ellipse();
            SolidColorBrush sobro = new SolidColorBrush();
            sobro.Color = Color.FromArgb(Convert.ToByte(255 * Math.Abs(alpha)), Convert.ToByte(255 * Math.Abs(red)), Convert.ToByte(255 * Math.Abs(green)), Convert.ToByte(255 * Math.Abs(blue)));
            
            dragger.Height = height * 5 / 6;//PUPPIGUISettings.defaultMenuElementHeight;
            dragger.Width = width * 5 / 6;//PUPPIGUISettings.defaultMenuElementWidth;
            dragger.Fill = sobro;
            //quarter radius
            dragger.RadiusX = Math.Min(height / 4, width / 4);
            dragger.RadiusY = dragger.RadiusX;
            sobro.Freeze();  
            TextBlock tb = new TextBlock();
             
            //checks the caption and removes the last numbers
            tb.Text = pm.name;
            tb.FontFamily = PUPPIGUISettings.bumeFoFa;   
            if (cleanuptitle == true)
            {
                try
                {

                    string[] stringSeparators = new string[] { "__" };
                    string[] typeSeparators = new string[] { "." };
                    string[] result = pm.name.Split(stringSeparators, StringSplitOptions.None);
                    if (result[0] != null && result[0] != "")
                    {
                        string[] tS = result[0].Split(typeSeparators, StringSplitOptions.None);
                        string ltS = tS[tS.GetLength(0) - 1];
                        tb.Text = ltS;//.Replace("Mthd:", "").Replace("New ", "");
                        //update caption for nodes
                        pm.cleancap = tb.Text;
                    }
                }
                catch
                {
                    tb.Text = pm.name;
                    pm.cleancap = "";
                }
            }


            tb.TextAlignment = TextAlignment.Center;
            tb.Height = height * 4 / 5;
            try
            {
                tb.Width = width - height;
            }
            //if it gets too small
            catch
            {
                tb.Width = PUPPIGUISettings.defaultMenuElementWidth;
            }


            //tb.FontSize = 10;
            tb.FontSize = height * 0.5;

            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            if (PUPPIGUISettings.highlightConstructorsInMenus)
            {
                if (pm.isconstructor || pm.isparameterlessconstructor)
                {

                    tb.FontWeight = FontWeights.DemiBold;
                }
            }
            

            spg.Children.Add(dragger);
            spg.Children.Add(tb);

            //spg.HorizontalAlignment = HorizontalAlignment.Center;
            //spg.VerticalAlignment = VerticalAlignment.Center;   

            sp.Children.Add(spg);
            sp.Height = dragger.Height;// +PUPPIGUISettings.moduleButtonSpacing;
            sp.Width = dragger.Width;
            
            //sp.HorizontalAlignment = HorizontalAlignment.Center;
            //sp.VerticalAlignment = VerticalAlignment.Center;   
            sp.MouseMove += dragitem;

            sp.MouseDown += sp_MouseDown;
            sp.DataContext = pm;
            //tooltip
            if (pm.description != "")
            {
                sp.ToolTip = pm.description;
            }
            else
            {
                sp.ToolTip = pm.name;
            }
            long mmm = 0;
            long quot = Math.DivRem(index, maxelementsperrow, out mmm);
            //sets the column definitions only the first row
            if (quot == 0)
            {
                ColumnDefinition colDef1 = new ColumnDefinition();

                PUPPIMenuGrid.ColumnDefinitions.Add(colDef1);
                //grows just enough to leave some space
                //  PUPPIMenuGrid.Width += width/2+1;
                //msp.Width += width/2+1;

            }
            //new row formed

            if (mmm == 0 && index > 0)
            {
                PUPPIMenuGrid.Height += height + PUPPIGUISettings.moduleButtonSpacing;
                if (PUPPIMenuGrid.Height >= msp.Height)
                {
                    msp.Height += height + PUPPIGUISettings.moduleButtonSpacing;
                }
                // Define new row
                RowDefinition rowDef1 = new RowDefinition();
                rowDef1.Height = new System.Windows.GridLength(height+PUPPIGUISettings.moduleButtonSpacing);
                PUPPIMenuGrid.RowDefinitions.Add(rowDef1);

            }

            Grid.SetRow(sp, (int)quot);
            Grid.SetColumn(sp, (int)mmm);
            PUPPIMenuGrid.Children.Add(sp);
            index = index + 1;






        }

        public bool changeitemtt(PUPPIModule pm,string newToolTip)
        {
           
            for (int i=0;i<PUPPIMenuGrid.Children.Count;i++)
            {
                try
                {
                    StackPanel sp = PUPPIMenuGrid.Children[i] as StackPanel;
                    if (sp.DataContext.GetType() .Equals(pm.GetType() )  )
                    {
                        sp.ToolTip = newToolTip;
                        return true;
                    }
                }
                catch
                {

                }
            }

          return false;

        }



        void sp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PUPPIGUISettings.addNodeOnModuleDoubleClick)
            {
                StackPanel sp = sender as StackPanel;


                if (sp != null && e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
                {



                    PUPPIGUI.PUPPIruntimesettings.savedAddNodeModule = sp.DataContext as PUPPIModule;
                    PUPPIGUI.PUPPIruntimesettings.fireUpModuleDblClicked();

                }
            }
        }

        //when object is dragged to canvas
        void dragitem(object sender, MouseEventArgs e)
        {
            StackPanel sp = sender as StackPanel;


            if (sp != null && e.LeftButton == MouseButtonState.Pressed)
            {


                DataObject dragData = new DataObject("PUPPIModule", sp.DataContext);

                DragDrop.DoDragDrop(sp, dragData, DragDropEffects.Move);


            }
        }
    }
    //internal class PUPPIMainMenu : Menu
    //{
    //    PUPPICanvas mycanvas;
    //    //if running from another form there should not be an exit command
    //    internal PUPPIMainMenu(double left, double top,PUPPICanvas pc, bool exitcommand)
    //    {
    //        // Make the main menu.

    //        mycanvas=pc;
    //        Margin = new Thickness(left, top, 0, 0); 
    //        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
    //        VerticalAlignment = System.Windows.VerticalAlignment.Top;

    //        // Make the File menu.
    //        MenuItem fileMenuItem = new MenuItem();
    //        fileMenuItem.Header = "_File";
    //        Items.Add(fileMenuItem);

    //        // Make the File menu's items.


















    //    }





    //}
    //help menu
    //internal class PUPPIHelpMenu:Menu
    //{
    //    PUPPICanvas mycanvas;
    //    string hlppath="";
    //    //relative path to executable
    //    internal PUPPIHelpMenu(double left, double top, PUPPICanvas pc, string helpfilepath)
    //    {
    //        // Make the help  menu.
    //        hlppath = helpfilepath;
    //        mycanvas = pc;
    //        Margin = new Thickness(left, top, 0, 0);
    //        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
    //        VerticalAlignment = System.Windows.VerticalAlignment.Top;

    //        // Make the help menu.
    //        MenuItem helpMenuItem = new MenuItem();
    //        helpMenuItem.Header = "_Help";
    //        Items.Add(helpMenuItem);

    //        // Make the help menu's items.


    //        // Make the help menu's items.


    //    }


    //}
    //generic dropdown menu, should accommodate everything
    internal class PUPPIGeneralDropDownMenu : Menu
    {
        internal PUPPICanvas mycanvas;
        internal MenuItem dropTopMenuItem;
        internal string hlppath;
        //items disabled on locked canvas
        internal List<bool> lon;
        //if running from another form there should not be an exit command
        internal PUPPIGeneralDropDownMenu(PUPPICanvas pc, string name = "Drop Down Menu")
        {
            // Make the main menu.

            mycanvas = pc;


            // Make the File menu.
            dropTopMenuItem = new MenuItem();
            dropTopMenuItem.Header = "_" + name.Replace("_", "");
            dropTopMenuItem.FontSize = PUPPIGUISettings.dropDownMenuFontSize;
            Items.Add(dropTopMenuItem);
            Margin = new Thickness(0, 0, 0, 0);
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
            dropTopMenuItem.SubmenuOpened += dropTopMenuItem_SubmenuOpened;
            lon = new List<bool>();

        }

        void dropTopMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            for (int i=0;i<dropTopMenuItem.Items.Count;i++   )
            {
                MenuItem mee = dropTopMenuItem.Items[i] as MenuItem;
                if (lon[i]==true && mycanvas.thiscanvasdefinitelylocked  )
                {
                    mee.IsEnabled = false; 
                }
                else
                {
                    mee.IsEnabled = true; 
                }
            }
        }
        //adds an item with associated method

        internal void addNewCanvasMenuOption()
        {
            MenuItem newMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(newMenuItem);
            newMenuItem.Header = "_New";
            newMenuItem.Click += newMenuItem_Click;
            ToolTip newToolTip = new ToolTip();
            newMenuItem.ToolTip = newToolTip;
            newToolTip.Content = "Start a new canvas";
            lon.Add(true); 
        }

        internal void addCustomCommandMenuOption(string n, Action d, string t,bool loh=false)
        {
            MenuItem newMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(newMenuItem);
            newMenuItem.Header = n;
            newMenuItem.Click += delegate { d(); };
            ToolTip newToolTip = new ToolTip();
            newMenuItem.ToolTip = newToolTip;
            newToolTip.Content = t;
            lon.Add(loh); 
        }

        internal void addLockCanvasMenuOption()
        {
            MenuItem lomeMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(lomeMenuItem);
            lomeMenuItem.Header = "_Lock Canvas";
            lomeMenuItem.Click += lomeMenuItem_Click;
            ToolTip loToolTip = new ToolTip();
            lomeMenuItem.ToolTip = loToolTip;
            loToolTip.Content = "Switch to canvas read-only presentation and interaction mode";
            lon.Add(true); 
        }

        internal void addunLockCanvasMenuOption()
        {
            MenuItem ulomeMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(ulomeMenuItem);
            ulomeMenuItem.Header = "_Unlock Canvas";
            ulomeMenuItem.Click += ulomeMenuItem_Click;
            ToolTip loToolTip = new ToolTip();
            ulomeMenuItem.ToolTip = loToolTip;
            loToolTip.Content = "Switch to canvas visual programming mode";
            lon.Add(false);
        }

        void ulomeMenuItem_Click(object sender, RoutedEventArgs e)
        {
          //  System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("All unsaved changes will be lost, proceed?", "Locking Canvas", System.Windows.Forms.MessageBoxButtons.YesNo);
           // if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            //{
                mycanvas.setthiscanvaslockstatus(false); 
            //}
            //else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                //do something else
            }
           
        }


        void lomeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (mycanvas.thiscanvasdefinitelylocked == false)
            {

                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("All unsaved changes will be lost, proceed?", "Locking Canvas", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    mycanvas.setthiscanvaslockstatus(true);
                }
                else if (dialogResult == System.Windows.Forms.DialogResult.No)
                {
                    //do something else
                }
               
            }
        }


        internal void addOpenCanvasMenuOption()
        {
            MenuItem openMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(openMenuItem);
            openMenuItem.Header = "_Open";
            openMenuItem.Click += openMenuItem_Click;
            ToolTip openToolTip = new ToolTip();
            openMenuItem.ToolTip = openToolTip;
            openToolTip.Content = "Open a new PUPPI Canvas file";
            lon.Add(false); 
        }

        internal void addImportCanvasMenuOption()
        {
            MenuItem importMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(importMenuItem);
            importMenuItem.Header = "_Import";
            importMenuItem.Click += importMenuItem_Click;
            ToolTip importToolTip = new ToolTip();
            importMenuItem.ToolTip = importToolTip;
            importToolTip.Content = "Import canvas file into current canvas";
            lon.Add(true); 

        }


        internal void addImportCanvasACMenuOption()
        {
            MenuItem importCMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(importCMenuItem);
            importCMenuItem.Header = "_Import As Container";
            importCMenuItem.Click += importCMenuItem_Click;
            ToolTip importToolTip = new ToolTip();
            importCMenuItem.ToolTip = importToolTip;
            importToolTip.Content = "Import canvas file into current canvas as a single container node";
            lon.Add(true); 

        }

        void importCMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.importfileAC();
            }
        }

        internal void addSaveCanvasMenuOption()
        {
            MenuItem saveMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(saveMenuItem);
            saveMenuItem.Header = "_Save";
            saveMenuItem.Click += saveMenuItem_Click;
            ToolTip saveToolTip = new ToolTip();
            saveMenuItem.ToolTip = saveToolTip;
            saveToolTip.Content = "Save file";
            lon.Add(true); 
        }

        internal void addExportSnapshotMenuOption()
        {
            MenuItem expMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(expMenuItem);
            expMenuItem.Header = "_Export Snapshot";
            expMenuItem.Click += expMenuItem_Click;
            ToolTip expToolTip = new ToolTip();
            expMenuItem.ToolTip = expToolTip;
            expToolTip.Content = "Exports a 3D model of the current state of the program.";
            lon.Add(false); 
        }

        void expMenuItem_Click(object sender, RoutedEventArgs e)
        {
            mycanvas.exportcanvassnapshot();
        }

        internal void addUndoMenuOption()
        {
            MenuItem undoMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(undoMenuItem);
            undoMenuItem.Header = "_Undo";
            undoMenuItem.Click += undoMenuItem_Click;
            ToolTip undoToolTip = new ToolTip();
            undoMenuItem.ToolTip = undoToolTip;
            undoToolTip.Content = "Undo";
            lon.Add(true); 
        }
        internal void addRedoMenuOption()
        {
            MenuItem redoMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(redoMenuItem);
            redoMenuItem.Header = "_Redo";
            redoMenuItem.Click += redoMenuItem_Click;
            ToolTip redoToolTip = new ToolTip();
            redoMenuItem.ToolTip = redoToolTip;
            redoToolTip.Content = "Redo";
            lon.Add(true); 
        }

        internal void addPaste00MenuOption()
        {
            MenuItem Paste00MenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(Paste00MenuItem);
            Paste00MenuItem.Header = "_Paste";
            Paste00MenuItem.Click += Paste00MenuItem_Click;
            ToolTip Paste00ToolTip = new ToolTip();
            Paste00MenuItem.ToolTip = Paste00ToolTip;
            Paste00ToolTip.Content = "Paste selection at 0,0";
            lon.Add(true); 
        }

        void Paste00MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.canvasabouttochange();
                mycanvas.pastefromclip(new Point3D(0, 0, 0));
            }
        }
        internal void addNbNMenuOption()
        {
            MenuItem NbNMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(NbNMenuItem);
            NbNMenuItem.Header = "Add node by _module name";
            NbNMenuItem.Click += NbNMenuItem_Click;
            ToolTip NbNToolTip = new ToolTip();
            NbNMenuItem.ToolTip = NbNToolTip;
            NbNToolTip.Content = "Adds a node at 0,0 from specified module name";
            lon.Add(true); 
        }

        void NbNMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                PUPPI.inputModulePicker pi = new PUPPI.inputModulePicker();
                foreach (PUPPIModule p in mycanvas.cvavailablePUPPIModules.Values)
                {

                    pi.modNames.Add(p.name);


                    pi.modest.Add(p.description);
                }
                pi.ShowDialog();
                PUPPIModule pl = null;
                if (pi.selectedModule != "")
                {
                    foreach (PUPPIModule ppi in mycanvas.cvavailablePUPPIModules.Values)
                    {
                        if (ppi.name == pi.selectedModule)
                        {
                            pl = ppi;
                            break;
                        }
                    }
                    if (pl != null)
                    {
                        mycanvas.canvasabouttochange();
                        int i = mycanvas.addANodeToTheCanvas(new Point3D(0, 0, 0), pl);
                        if (i == -1)
                        {
                            MessageBox.Show("Failed to add node!");
                            mycanvas.undome();
                        }

                    }
                }
            }
        }

        internal void addResetAllNodeCaptionsOption()
        {
            MenuItem resetNodeCaptionsMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(resetNodeCaptionsMenuItem);
            resetNodeCaptionsMenuItem.Header = "Reset _Node Captions";
            resetNodeCaptionsMenuItem.Click += resetNodeCaptionsMenuItem_Click;
            ToolTip redoToolTip = new ToolTip();
            resetNodeCaptionsMenuItem.ToolTip = redoToolTip;
            redoToolTip.Content = "Reset All Node Captions";
            lon.Add(true); 
        }

        void resetNodeCaptionsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.resetAllCanvasNodeCaptions();
            }
        }

        internal void addSelectAllMenuOption()
        {
            MenuItem selectAllMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(selectAllMenuItem);
            selectAllMenuItem.Header = "Select _All";
            selectAllMenuItem.Click += selectAllMenuItem_Click;
            ToolTip selectAllToolTip = new ToolTip();
            selectAllMenuItem.ToolTip = selectAllToolTip;
            selectAllToolTip.Content = "Selects all the stacks on the canvas.";
            lon.Add(true); 
        }

        internal void addHideSelectedMenuOption()
        {
            MenuItem HideSelectedMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(HideSelectedMenuItem);
            HideSelectedMenuItem.Header = "_Hide Selected";
            HideSelectedMenuItem.Click += HideSelectedMenuItem_Click;
            ToolTip HideSelectedToolTip = new ToolTip();
            HideSelectedMenuItem.ToolTip = HideSelectedToolTip;
            HideSelectedToolTip.Content = "Hide selected node stacks on locked canvases";
            lon.Add(true);
        }


        internal void addShowSelectedMenuOption()
        {
            MenuItem ShowSelectedMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(ShowSelectedMenuItem);
            ShowSelectedMenuItem.Header = "Show Se_lected";
            ShowSelectedMenuItem.Click += ShowSelectedMenuItem_Click;
            ToolTip ShowSelectedToolTip = new ToolTip();
            ShowSelectedMenuItem.ToolTip = ShowSelectedToolTip;
            ShowSelectedToolTip.Content = "Show selected node stacks on locked canvases";
            lon.Add(true);
        }

        void ShowSelectedMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked) mycanvas.set_hidden_selected_nodes_on_locked_canvas(false);  
        }

        void HideSelectedMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked) mycanvas.set_hidden_selected_nodes_on_locked_canvas(true);  
        }


        internal void addCollapseSelectionMenuOption()
        {
            MenuItem cMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(cMenuItem);
            cMenuItem.Header = "C_ollapse Selection";
            cMenuItem.Click += cMenuItem_Click;
            ToolTip cToolTip = new ToolTip();
            cMenuItem.ToolTip = cToolTip;
            cToolTip.Content = "Collapses selected nodes into a single container node";
            lon.Add(true); 
        }

        void cMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                string msg = "";
                bool bial = PUPPIruntimesettings.processyesno;
                PUPPIruntimesettings.processyesno = false;
                int iuw = 0;
                bool b = mycanvas.collapseSelectionToContainerNode(out msg, out iuw);
                PUPPIruntimesettings.processyesno = bial;
                if (b)
                {
                    mycanvas.runPUPPIprogram();
                }
                else
                {
                    if (!msg.StartsWith("There is at least one output which connects"))
                    {
                        MessageBox.Show("Failed to collapse nodes: " + msg);
                        //mycanvas.undome();
                    }
                    else
                    {
                        MessageBox.Show(msg);
                    }
                }
            }
        }

        void selectAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.selectallnodes();
            }
        }
        internal void addSelectNoneMenuOption()
        {
            MenuItem SelectNoneMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(SelectNoneMenuItem);
            SelectNoneMenuItem.Header = "Select _None";
            SelectNoneMenuItem.Click += SelectNoneMenuItem_Click;
            ToolTip SelectNoneToolTip = new ToolTip();
            SelectNoneMenuItem.ToolTip = SelectNoneToolTip;
            SelectNoneToolTip.Content = "De-selects all the selected nodes on the canvas.";
            lon.Add(true); 
        }

        void SelectNoneMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.deselectallnodes();
            }
        }


        internal void addCopySelectionMenuOption()
        {
            MenuItem CopySelectionMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(CopySelectionMenuItem);
            CopySelectionMenuItem.Header = "_Copy Selection";
            CopySelectionMenuItem.Click += CopySelectionMenuItem_Click;
            ToolTip CopySelectionToolTip = new ToolTip();
            CopySelectionMenuItem.ToolTip = CopySelectionToolTip;
            CopySelectionToolTip.Content = "Copies selected nodes to clipboard.";
            lon.Add(true); 
        }

        void CopySelectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.copyselectiontoclipboard();
            }
        }

        internal void addExportSelectionMenuOption()
        {

            MenuItem exportSelectionMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(exportSelectionMenuItem);
            exportSelectionMenuItem.Header = "_Export Selection";
            exportSelectionMenuItem.Click += exportSelectionMenuItem_Click;
            ToolTip exportSelectionToolTip = new ToolTip();
            exportSelectionMenuItem.ToolTip = exportSelectionToolTip;
            exportSelectionToolTip.Content = "Exports selected nodes to a program file.";
            lon.Add(true); 
        }

        void exportSelectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.export_selected_nodes();
            }
        }

        internal void adddeleteSelectionMenuOption()
        {

            MenuItem deleteSelectionMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(deleteSelectionMenuItem);
            deleteSelectionMenuItem.Header = "_Delete Selected Nodes";
            deleteSelectionMenuItem.Click += deleteSelectionMenuItem_Click;
            ToolTip deleteSelectionToolTip = new ToolTip();
            deleteSelectionMenuItem.ToolTip = deleteSelectionToolTip;
            deleteSelectionToolTip.Content = "Deletes the selection from the canvas";
            lon.Add(true); 
        }

        void deleteSelectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.delete_selected_nodes();
            }
        }

        internal void addTreeViewMenuOption()
        {
            MenuItem treeMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(treeMenuItem);
            treeMenuItem.Header = "_Tree View";
            treeMenuItem.Click += treeMenuItem_Click;
            ToolTip treeToolTip = new ToolTip();
            treeMenuItem.ToolTip = treeToolTip;
            treeToolTip.Content = "Show Tree View of Nodes (Bases and children)";
            lon.Add(false); 
        }

        internal void addSetCameraPositionMenuOption()
        {
            MenuItem setposMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(setposMenuItem);
            setposMenuItem.Header = "_Set Camera Position";
            setposMenuItem.Click += setposMenuItem_Click;
            ToolTip setposToolTip = new ToolTip();
            setposMenuItem.ToolTip = setposToolTip;
            setposToolTip.Content = "Sets the locastion of the camera. If 3D, it's lookingn at the origin, if 2D it is looking straight down.";
            lon.Add(false); 
        }

        internal void addRARnodesMenuOption()
        {
            MenuItem rarNodesMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(rarNodesMenuItem);
            rarNodesMenuItem.Header = "_Rearrange Nodes";
            rarNodesMenuItem.Click += rarNodesMenuItem_Click;
            ToolTip setposToolTip = new ToolTip();
            rarNodesMenuItem.ToolTip = setposToolTip;
            setposToolTip.Content = "Rearranges node stacks based on connections and user input";
            lon.Add(true); 
        }



        void rarNodesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                string s = "1";
                System.Windows.Forms.DialogResult d = PUPPIFormUtils.formutils.InputBox("Node reorganization...", "Enter number of colums", ref s);
                if (d == System.Windows.Forms.DialogResult.Cancel) return;
                int v = 0;
                try
                {
                    v = Convert.ToInt32(s);
                }
                catch
                {
                    v = 0;
                }
                if (v <= 0)
                {
                    MessageBox.Show("Invalid number of rows!");
                    return;
                }
                mycanvas.rearrangenice(v);
            }
        }


        void setposMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string ps = "";
            if (PUPPIFormUtils.formutils.InputBox("Enter new position", "Enter in format x,y,z", ref ps) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    char[] s = { ',' };
                    string[] words = ps.Split(s);
                    if (words.Length == 3)
                    {
                        double xx = Convert.ToDouble(words[0]);
                        double yy = Convert.ToDouble(words[1]);
                        double zz = Convert.ToDouble(words[2]);
                        mycanvas.setcameraposition(new Point3D(xx, yy, zz));
                    }
                }
                catch
                {
                    mycanvas.setcameraposition(new Point3D(10, 10, 10));
                }
            }

        }

        internal void addMapViewMenuOption()
        {
            //only for 3d code, otherwise no point
            if ((PUPPIGUISettings.canvasMode != PUPPIGUISettings.CanvasModeEnum.TwoDimensional))
            {
                MenuItem mapMenuItem = new MenuItem();
                dropTopMenuItem.Items.Add(mapMenuItem);
                mapMenuItem.Header = "_Map View";
                mapMenuItem.Click += mapMenuItem_Click;
                ToolTip mapToolTip = new ToolTip();
                mapMenuItem.ToolTip = mapToolTip;
                mapToolTip.Content = "Show Map View of Nodes (Bases and simplified connections)";
                lon.Add(false); 
            }
        }

        internal void addRefrConnMenuOption()
        {
            MenuItem regenConnMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(regenConnMenuItem);
            regenConnMenuItem.Header = "R_egenerate Connections";
            regenConnMenuItem.Click += regenConnMenuItem_Click;
            ToolTip regenConnToolTip = new ToolTip();
            regenConnMenuItem.ToolTip = regenConnToolTip;
            regenConnToolTip.Content = "Redraws connections between nodes";
            lon.Add(true); 
        }

        void regenConnMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                string os = mycanvas.my_old_pc_status;
                mycanvas.updatemyPCStatus("Please wait,regenerating connections...");

                mycanvas.regenerate_connections(true);
                mycanvas.updatemyPCStatus(os);
            }
        }

        internal void addExitMenuOption()
        {
            MenuItem exitMenuItem = new MenuItem();
            dropTopMenuItem.Items.Add(exitMenuItem);
            exitMenuItem.Header = "E_xit";
            exitMenuItem.Click += exitMenuItem_Click;
            ToolTip exitToolTip = new ToolTip();
            exitMenuItem.ToolTip = exitToolTip;
            exitToolTip.Content = "End the program";
            lon.Add(false); 
        }

        internal void addHelpMenuOption(string helppath)
        {
            MenuItem hlpshowMenuItem = new MenuItem();
            hlppath = helppath;
            dropTopMenuItem.Items.Add(hlpshowMenuItem);


            hlpshowMenuItem.Header = "PUPPI VPL&GUI _Help";

            hlpshowMenuItem.Click += hlpshowMenuItem_Click;
            ToolTip hlpToolTip = new ToolTip();
            hlpshowMenuItem.ToolTip = hlpToolTip;
            hlpToolTip.Content = "Help on programming using the PUPPI Visual Programming Language and GUI.";
            lon.Add(false); 
        }
        internal void addAboutPUPPIMenuOption()
        {
            MenuItem aboutshowMenuItem = new MenuItem();

            dropTopMenuItem.Items.Add(aboutshowMenuItem);

            aboutshowMenuItem.Header = "_About PUPPI";
            aboutshowMenuItem.Click += aboutshowMenuItem_Click;
            ToolTip aboutToolTip = new ToolTip();
            aboutshowMenuItem.ToolTip = aboutToolTip;
            aboutToolTip.Content = "About PUPPI:Licenses, et";
            lon.Add(false); 
        }

        //standard methods

        void mapMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PUPPIMapViewForm pmv = new PUPPIMapViewForm();
            pmv.pc = mycanvas;
            pmv.ShowDialog();
        }

        void treeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PUPPITreeViewForm ptv = new PUPPITreeViewForm();
            ptv.mycanvas = mycanvas;
            ptv.ShowDialog();
        }


        private void undoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.stopMouseEv();
                mycanvas.undome();
                mycanvas.resumeMouseEv();
            }
        }


        private void importMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.stopMouseEv();
                mycanvas.importfile();
                mycanvas.resumeMouseEv();
            }  
        }
        private void redoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.stopMouseEv();
                mycanvas.redome();
                mycanvas.resumeMouseEv();
            }
        }

        private void newMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mycanvas.thiscanvasdefinitelylocked)
            {
                mycanvas.newcanvas();
            }
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
            mycanvas.stopMouseEv();  
            mycanvas.openfile();
            mycanvas.resumeMouseEv();  
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (!mycanvas.thiscanvasdefinitelylocked)
                mycanvas.stopMouseEv();
                mycanvas.saveas(mycanvas.stacks.Values.ToList<PUPPICanvas.ModViz3DNode>());
                mycanvas.resumeMouseEv(); 
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {

            mycanvas.exitapplication();
        }

        void aboutshowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PUPPI.aboutlicense abb = new PUPPI.aboutlicense();
            abb.Show();
        }

        void hlpshowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, hlppath);


        }
    }

}
