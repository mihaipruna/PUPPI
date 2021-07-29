namespace PUPPICADBeta
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
            this.viewswitcher = new System.Windows.Forms.TabControl();
            this.PUPPIcanvaspage = new System.Windows.Forms.TabPage();
            this.PUPPIcad3dviewpage = new System.Windows.Forms.TabPage();
            this.tabPUPPICADBasic = new System.Windows.Forms.TabPage();
            this.basicCADLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.menuTabControl = new System.Windows.Forms.TabControl();
            this.tabPUPPICADPrimitives = new System.Windows.Forms.TabPage();
            this.primitivesLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tabPUPPICADCurves = new System.Windows.Forms.TabPage();
            this.curvesLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.advancedWPF = new System.Windows.Forms.TabPage();
            this.aWPFtabControl = new System.Windows.Forms.TabControl();
            this.aWPFtabPage1 = new System.Windows.Forms.TabPage();
            this.advancedHelix3D = new System.Windows.Forms.TabPage();
            this.aHelix3DtabControl = new System.Windows.Forms.TabControl();
            this.aHelix3Dtabpage1 = new System.Windows.Forms.TabPage();
            this.pluginsTabControl = new System.Windows.Forms.TabPage();
            this.plugTabControl = new System.Windows.Forms.TabControl();
            this.dllModules = new System.Windows.Forms.TabPage();
            this.dllTabControl = new System.Windows.Forms.TabControl();
            this.label1 = new System.Windows.Forms.Label();
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.menuLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dropDownLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.miscButtonLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.debugcheck = new System.Windows.Forms.CheckBox();
            this.myFirstProgram = new System.Windows.Forms.Button();
            this.tabControlGeneralModules = new System.Windows.Forms.TabControl();
            this.tabMathFunctions = new System.Windows.Forms.TabPage();
            this.basicMathLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tabAdvancedMath = new System.Windows.Forms.TabPage();
            this.advancedMathTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tabProgramFlow = new System.Windows.Forms.TabPage();
            this.generalProgrammingMenuLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tabCollections = new System.Windows.Forms.TabPage();
            this.tabInteroperability = new System.Windows.Forms.TabPage();
            this.tabStringMenu = new System.Windows.Forms.TabPage();
            this.tabFileMenu = new System.Windows.Forms.TabPage();
            this.viewswitcher.SuspendLayout();
            this.tabPUPPICADBasic.SuspendLayout();
            this.menuTabControl.SuspendLayout();
            this.tabPUPPICADPrimitives.SuspendLayout();
            this.tabPUPPICADCurves.SuspendLayout();
            this.advancedWPF.SuspendLayout();
            this.aWPFtabControl.SuspendLayout();
            this.advancedHelix3D.SuspendLayout();
            this.aHelix3DtabControl.SuspendLayout();
            this.pluginsTabControl.SuspendLayout();
            this.dllModules.SuspendLayout();
            this.mainLayoutPanel.SuspendLayout();
            this.menuLayoutPanel.SuspendLayout();
            this.miscButtonLayoutPanel.SuspendLayout();
            this.tabControlGeneralModules.SuspendLayout();
            this.tabMathFunctions.SuspendLayout();
            this.tabAdvancedMath.SuspendLayout();
            this.tabProgramFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(588, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "Show Debug and Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // viewswitcher
            // 
            this.viewswitcher.Controls.Add(this.PUPPIcanvaspage);
            this.viewswitcher.Controls.Add(this.PUPPIcad3dviewpage);
            this.viewswitcher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewswitcher.Location = new System.Drawing.Point(3, 204);
            this.viewswitcher.Name = "viewswitcher";
            this.viewswitcher.SelectedIndex = 0;
            this.viewswitcher.Size = new System.Drawing.Size(1258, 464);
            this.viewswitcher.TabIndex = 3;
            // 
            // PUPPIcanvaspage
            // 
            this.PUPPIcanvaspage.Location = new System.Drawing.Point(4, 22);
            this.PUPPIcanvaspage.Name = "PUPPIcanvaspage";
            this.PUPPIcanvaspage.Padding = new System.Windows.Forms.Padding(3);
            this.PUPPIcanvaspage.Size = new System.Drawing.Size(1250, 438);
            this.PUPPIcanvaspage.TabIndex = 0;
            this.PUPPIcanvaspage.Text = "PUPPI Canvas View";
            this.PUPPIcanvaspage.UseVisualStyleBackColor = true;
            // 
            // PUPPIcad3dviewpage
            // 
            this.PUPPIcad3dviewpage.Location = new System.Drawing.Point(4, 22);
            this.PUPPIcad3dviewpage.Name = "PUPPIcad3dviewpage";
            this.PUPPIcad3dviewpage.Padding = new System.Windows.Forms.Padding(3);
            this.PUPPIcad3dviewpage.Size = new System.Drawing.Size(1250, 438);
            this.PUPPIcad3dviewpage.TabIndex = 1;
            this.PUPPIcad3dviewpage.Text = "CAD Model View";
            this.PUPPIcad3dviewpage.UseVisualStyleBackColor = true;
            // 
            // tabPUPPICADBasic
            // 
            this.tabPUPPICADBasic.Controls.Add(this.basicCADLayoutPanel);
            this.tabPUPPICADBasic.Location = new System.Drawing.Point(4, 22);
            this.tabPUPPICADBasic.Name = "tabPUPPICADBasic";
            this.tabPUPPICADBasic.Padding = new System.Windows.Forms.Padding(3);
            this.tabPUPPICADBasic.Size = new System.Drawing.Size(729, 133);
            this.tabPUPPICADBasic.TabIndex = 0;
            this.tabPUPPICADBasic.Text = "Basic Modeling";
            this.tabPUPPICADBasic.UseVisualStyleBackColor = true;
            // 
            // basicCADLayoutPanel
            // 
            this.basicCADLayoutPanel.ColumnCount = 3;
            this.basicCADLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.basicCADLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.basicCADLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.basicCADLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicCADLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.basicCADLayoutPanel.Name = "basicCADLayoutPanel";
            this.basicCADLayoutPanel.RowCount = 1;
            this.basicCADLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.basicCADLayoutPanel.Size = new System.Drawing.Size(723, 127);
            this.basicCADLayoutPanel.TabIndex = 0;
            this.basicCADLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.basicCADLayoutPanel_Paint);
            // 
            // menuTabControl
            // 
            this.menuTabControl.Controls.Add(this.tabPUPPICADPrimitives);
            this.menuTabControl.Controls.Add(this.tabPUPPICADCurves);
            this.menuTabControl.Controls.Add(this.tabPUPPICADBasic);
            this.menuTabControl.Controls.Add(this.advancedWPF);
            this.menuTabControl.Controls.Add(this.advancedHelix3D);
            this.menuTabControl.Controls.Add(this.pluginsTabControl);
            this.menuTabControl.Controls.Add(this.dllModules);
            this.menuTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuTabControl.Location = new System.Drawing.Point(518, 33);
            this.menuTabControl.Name = "menuTabControl";
            this.menuTabControl.SelectedIndex = 0;
            this.menuTabControl.Size = new System.Drawing.Size(737, 159);
            this.menuTabControl.TabIndex = 4;
            // 
            // tabPUPPICADPrimitives
            // 
            this.tabPUPPICADPrimitives.Controls.Add(this.primitivesLayoutPanel);
            this.tabPUPPICADPrimitives.Location = new System.Drawing.Point(4, 22);
            this.tabPUPPICADPrimitives.Name = "tabPUPPICADPrimitives";
            this.tabPUPPICADPrimitives.Padding = new System.Windows.Forms.Padding(3);
            this.tabPUPPICADPrimitives.Size = new System.Drawing.Size(729, 133);
            this.tabPUPPICADPrimitives.TabIndex = 3;
            this.tabPUPPICADPrimitives.Text = "Primitives";
            this.tabPUPPICADPrimitives.UseVisualStyleBackColor = true;
            // 
            // primitivesLayoutPanel
            // 
            this.primitivesLayoutPanel.ColumnCount = 4;
            this.primitivesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.primitivesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.primitivesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19F));
            this.primitivesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.primitivesLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.primitivesLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.primitivesLayoutPanel.Name = "primitivesLayoutPanel";
            this.primitivesLayoutPanel.RowCount = 1;
            this.primitivesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.primitivesLayoutPanel.Size = new System.Drawing.Size(723, 127);
            this.primitivesLayoutPanel.TabIndex = 0;
            // 
            // tabPUPPICADCurves
            // 
            this.tabPUPPICADCurves.Controls.Add(this.curvesLayoutPanel);
            this.tabPUPPICADCurves.Location = new System.Drawing.Point(4, 22);
            this.tabPUPPICADCurves.Name = "tabPUPPICADCurves";
            this.tabPUPPICADCurves.Padding = new System.Windows.Forms.Padding(3);
            this.tabPUPPICADCurves.Size = new System.Drawing.Size(729, 133);
            this.tabPUPPICADCurves.TabIndex = 6;
            this.tabPUPPICADCurves.Text = "Curves";
            this.tabPUPPICADCurves.UseVisualStyleBackColor = true;
            // 
            // curvesLayoutPanel
            // 
            this.curvesLayoutPanel.ColumnCount = 3;
            this.curvesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.curvesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.curvesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.curvesLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curvesLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.curvesLayoutPanel.Name = "curvesLayoutPanel";
            this.curvesLayoutPanel.RowCount = 1;
            this.curvesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.curvesLayoutPanel.Size = new System.Drawing.Size(723, 127);
            this.curvesLayoutPanel.TabIndex = 0;
            // 
            // advancedWPF
            // 
            this.advancedWPF.Controls.Add(this.aWPFtabControl);
            this.advancedWPF.Location = new System.Drawing.Point(4, 22);
            this.advancedWPF.Name = "advancedWPF";
            this.advancedWPF.Padding = new System.Windows.Forms.Padding(3);
            this.advancedWPF.Size = new System.Drawing.Size(729, 133);
            this.advancedWPF.TabIndex = 1;
            this.advancedWPF.Text = "Advanced WPF";
            this.advancedWPF.UseVisualStyleBackColor = true;
            // 
            // aWPFtabControl
            // 
            this.aWPFtabControl.Controls.Add(this.aWPFtabPage1);
            this.aWPFtabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aWPFtabControl.Location = new System.Drawing.Point(3, 3);
            this.aWPFtabControl.Name = "aWPFtabControl";
            this.aWPFtabControl.SelectedIndex = 0;
            this.aWPFtabControl.Size = new System.Drawing.Size(723, 127);
            this.aWPFtabControl.TabIndex = 0;
            // 
            // aWPFtabPage1
            // 
            this.aWPFtabPage1.Location = new System.Drawing.Point(4, 22);
            this.aWPFtabPage1.Name = "aWPFtabPage1";
            this.aWPFtabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.aWPFtabPage1.Size = new System.Drawing.Size(715, 101);
            this.aWPFtabPage1.TabIndex = 0;
            this.aWPFtabPage1.Text = "3D CAD Access";
            this.aWPFtabPage1.UseVisualStyleBackColor = true;
            // 
            // advancedHelix3D
            // 
            this.advancedHelix3D.Controls.Add(this.aHelix3DtabControl);
            this.advancedHelix3D.Location = new System.Drawing.Point(4, 22);
            this.advancedHelix3D.Name = "advancedHelix3D";
            this.advancedHelix3D.Padding = new System.Windows.Forms.Padding(3);
            this.advancedHelix3D.Size = new System.Drawing.Size(729, 133);
            this.advancedHelix3D.TabIndex = 2;
            this.advancedHelix3D.Text = "Advanced Helix3D";
            this.advancedHelix3D.UseVisualStyleBackColor = true;
            // 
            // aHelix3DtabControl
            // 
            this.aHelix3DtabControl.Controls.Add(this.aHelix3Dtabpage1);
            this.aHelix3DtabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aHelix3DtabControl.Location = new System.Drawing.Point(3, 3);
            this.aHelix3DtabControl.Name = "aHelix3DtabControl";
            this.aHelix3DtabControl.SelectedIndex = 0;
            this.aHelix3DtabControl.Size = new System.Drawing.Size(723, 127);
            this.aHelix3DtabControl.TabIndex = 1;
            // 
            // aHelix3Dtabpage1
            // 
            this.aHelix3Dtabpage1.Location = new System.Drawing.Point(4, 22);
            this.aHelix3Dtabpage1.Name = "aHelix3Dtabpage1";
            this.aHelix3Dtabpage1.Padding = new System.Windows.Forms.Padding(3);
            this.aHelix3Dtabpage1.Size = new System.Drawing.Size(715, 101);
            this.aHelix3Dtabpage1.TabIndex = 0;
            this.aHelix3Dtabpage1.Text = "3D CAD Access";
            this.aHelix3Dtabpage1.UseVisualStyleBackColor = true;
            // 
            // pluginsTabControl
            // 
            this.pluginsTabControl.Controls.Add(this.plugTabControl);
            this.pluginsTabControl.Location = new System.Drawing.Point(4, 22);
            this.pluginsTabControl.Name = "pluginsTabControl";
            this.pluginsTabControl.Padding = new System.Windows.Forms.Padding(3);
            this.pluginsTabControl.Size = new System.Drawing.Size(729, 133);
            this.pluginsTabControl.TabIndex = 4;
            this.pluginsTabControl.Text = "Plugin PUPPIModules";
            this.pluginsTabControl.ToolTipText = "Loads DLL with PUPPIModules from folder \"PluginPUPPIModules\"";
            this.pluginsTabControl.UseVisualStyleBackColor = true;
            // 
            // plugTabControl
            // 
            this.plugTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plugTabControl.Location = new System.Drawing.Point(3, 3);
            this.plugTabControl.Name = "plugTabControl";
            this.plugTabControl.SelectedIndex = 0;
            this.plugTabControl.Size = new System.Drawing.Size(723, 127);
            this.plugTabControl.TabIndex = 0;
            // 
            // dllModules
            // 
            this.dllModules.Controls.Add(this.dllTabControl);
            this.dllModules.Location = new System.Drawing.Point(4, 22);
            this.dllModules.Name = "dllModules";
            this.dllModules.Padding = new System.Windows.Forms.Padding(3);
            this.dllModules.Size = new System.Drawing.Size(729, 133);
            this.dllModules.TabIndex = 5;
            this.dllModules.Text = "Modules from DLLs";
            this.dllModules.UseVisualStyleBackColor = true;
            // 
            // dllTabControl
            // 
            this.dllTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dllTabControl.Location = new System.Drawing.Point(3, 3);
            this.dllTabControl.Name = "dllTabControl";
            this.dllTabControl.SelectedIndex = 0;
            this.dllTabControl.Size = new System.Drawing.Size(723, 127);
            this.dllTabControl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "PUPPICAD Tutorials";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Controls.Add(this.menuLayoutPanel, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.viewswitcher, 0, 1);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(1264, 671);
            this.mainLayoutPanel.TabIndex = 6;
            // 
            // menuLayoutPanel
            // 
            this.menuLayoutPanel.ColumnCount = 2;
            this.menuLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.95238F));
            this.menuLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.04762F));
            this.menuLayoutPanel.Controls.Add(this.menuTabControl, 1, 1);
            this.menuLayoutPanel.Controls.Add(this.dropDownLayoutPanel, 0, 0);
            this.menuLayoutPanel.Controls.Add(this.miscButtonLayoutPanel, 1, 0);
            this.menuLayoutPanel.Controls.Add(this.tabControlGeneralModules, 0, 1);
            this.menuLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.menuLayoutPanel.Name = "menuLayoutPanel";
            this.menuLayoutPanel.RowCount = 2;
            this.menuLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.menuLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.menuLayoutPanel.Size = new System.Drawing.Size(1258, 195);
            this.menuLayoutPanel.TabIndex = 0;
            // 
            // dropDownLayoutPanel
            // 
            this.dropDownLayoutPanel.ColumnCount = 8;
            this.dropDownLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.dropDownLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.dropDownLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.dropDownLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.dropDownLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
            this.dropDownLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.dropDownLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.dropDownLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.dropDownLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.dropDownLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dropDownLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.dropDownLayoutPanel.Name = "dropDownLayoutPanel";
            this.dropDownLayoutPanel.RowCount = 1;
            this.dropDownLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.dropDownLayoutPanel.Size = new System.Drawing.Size(509, 24);
            this.dropDownLayoutPanel.TabIndex = 5;
            // 
            // miscButtonLayoutPanel
            // 
            this.miscButtonLayoutPanel.ColumnCount = 5;
            this.miscButtonLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.miscButtonLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.miscButtonLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.miscButtonLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.miscButtonLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.miscButtonLayoutPanel.Controls.Add(this.button1, 4, 0);
            this.miscButtonLayoutPanel.Controls.Add(this.debugcheck, 2, 0);
            this.miscButtonLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.miscButtonLayoutPanel.Controls.Add(this.myFirstProgram, 1, 0);
            this.miscButtonLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.miscButtonLayoutPanel.Location = new System.Drawing.Point(518, 3);
            this.miscButtonLayoutPanel.Name = "miscButtonLayoutPanel";
            this.miscButtonLayoutPanel.RowCount = 1;
            this.miscButtonLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.miscButtonLayoutPanel.Size = new System.Drawing.Size(737, 24);
            this.miscButtonLayoutPanel.TabIndex = 6;
            this.miscButtonLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // debugcheck
            // 
            this.debugcheck.AutoSize = true;
            this.debugcheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugcheck.Location = new System.Drawing.Point(297, 3);
            this.debugcheck.Name = "debugcheck";
            this.debugcheck.Size = new System.Drawing.Size(141, 18);
            this.debugcheck.TabIndex = 2;
            this.debugcheck.Text = "Debug Mode";
            this.debugcheck.UseVisualStyleBackColor = true;
            this.debugcheck.CheckedChanged += new System.EventHandler(this.debugcheck_CheckedChanged);
            // 
            // myFirstProgram
            // 
            this.myFirstProgram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myFirstProgram.Location = new System.Drawing.Point(147, 0);
            this.myFirstProgram.Margin = new System.Windows.Forms.Padding(0);
            this.myFirstProgram.Name = "myFirstProgram";
            this.myFirstProgram.Size = new System.Drawing.Size(147, 24);
            this.myFirstProgram.TabIndex = 6;
            this.myFirstProgram.Text = "My First Program";
            this.myFirstProgram.UseVisualStyleBackColor = true;
            this.myFirstProgram.Click += new System.EventHandler(this.myFirstProgram_Click);
            // 
            // tabControlGeneralModules
            // 
            this.tabControlGeneralModules.Controls.Add(this.tabMathFunctions);
            this.tabControlGeneralModules.Controls.Add(this.tabAdvancedMath);
            this.tabControlGeneralModules.Controls.Add(this.tabProgramFlow);
            this.tabControlGeneralModules.Controls.Add(this.tabCollections);
            this.tabControlGeneralModules.Controls.Add(this.tabInteroperability);
            this.tabControlGeneralModules.Controls.Add(this.tabStringMenu);
            this.tabControlGeneralModules.Controls.Add(this.tabFileMenu);
            this.tabControlGeneralModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlGeneralModules.Location = new System.Drawing.Point(3, 33);
            this.tabControlGeneralModules.Name = "tabControlGeneralModules";
            this.tabControlGeneralModules.SelectedIndex = 0;
            this.tabControlGeneralModules.Size = new System.Drawing.Size(509, 159);
            this.tabControlGeneralModules.TabIndex = 7;
            // 
            // tabMathFunctions
            // 
            this.tabMathFunctions.Controls.Add(this.basicMathLayoutPanel);
            this.tabMathFunctions.Location = new System.Drawing.Point(4, 22);
            this.tabMathFunctions.Name = "tabMathFunctions";
            this.tabMathFunctions.Padding = new System.Windows.Forms.Padding(3);
            this.tabMathFunctions.Size = new System.Drawing.Size(501, 133);
            this.tabMathFunctions.TabIndex = 0;
            this.tabMathFunctions.Text = "Basic Math";
            this.tabMathFunctions.UseVisualStyleBackColor = true;
            // 
            // basicMathLayoutPanel
            // 
            this.basicMathLayoutPanel.ColumnCount = 1;
            this.basicMathLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.basicMathLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.basicMathLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicMathLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.basicMathLayoutPanel.Name = "basicMathLayoutPanel";
            this.basicMathLayoutPanel.RowCount = 1;
            this.basicMathLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.basicMathLayoutPanel.Size = new System.Drawing.Size(495, 127);
            this.basicMathLayoutPanel.TabIndex = 0;
            // 
            // tabAdvancedMath
            // 
            this.tabAdvancedMath.Controls.Add(this.advancedMathTableLayoutPanel);
            this.tabAdvancedMath.Location = new System.Drawing.Point(4, 22);
            this.tabAdvancedMath.Name = "tabAdvancedMath";
            this.tabAdvancedMath.Size = new System.Drawing.Size(501, 133);
            this.tabAdvancedMath.TabIndex = 7;
            this.tabAdvancedMath.Text = "Advanced Math";
            this.tabAdvancedMath.UseVisualStyleBackColor = true;
            // 
            // advancedMathTableLayoutPanel
            // 
            this.advancedMathTableLayoutPanel.ColumnCount = 2;
            this.advancedMathTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.advancedMathTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.advancedMathTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advancedMathTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.advancedMathTableLayoutPanel.Name = "advancedMathTableLayoutPanel";
            this.advancedMathTableLayoutPanel.RowCount = 1;
            this.advancedMathTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.advancedMathTableLayoutPanel.Size = new System.Drawing.Size(501, 133);
            this.advancedMathTableLayoutPanel.TabIndex = 0;
            // 
            // tabProgramFlow
            // 
            this.tabProgramFlow.Controls.Add(this.generalProgrammingMenuLayout);
            this.tabProgramFlow.Location = new System.Drawing.Point(4, 22);
            this.tabProgramFlow.Name = "tabProgramFlow";
            this.tabProgramFlow.Padding = new System.Windows.Forms.Padding(3);
            this.tabProgramFlow.Size = new System.Drawing.Size(501, 133);
            this.tabProgramFlow.TabIndex = 2;
            this.tabProgramFlow.Text = "General Programming";
            this.tabProgramFlow.UseVisualStyleBackColor = true;
            // 
            // generalProgrammingMenuLayout
            // 
            this.generalProgrammingMenuLayout.ColumnCount = 3;
            this.generalProgrammingMenuLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.generalProgrammingMenuLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.generalProgrammingMenuLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.generalProgrammingMenuLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generalProgrammingMenuLayout.Location = new System.Drawing.Point(3, 3);
            this.generalProgrammingMenuLayout.Name = "generalProgrammingMenuLayout";
            this.generalProgrammingMenuLayout.RowCount = 1;
            this.generalProgrammingMenuLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.generalProgrammingMenuLayout.Size = new System.Drawing.Size(495, 127);
            this.generalProgrammingMenuLayout.TabIndex = 0;
            // 
            // tabCollections
            // 
            this.tabCollections.Location = new System.Drawing.Point(4, 22);
            this.tabCollections.Name = "tabCollections";
            this.tabCollections.Size = new System.Drawing.Size(501, 133);
            this.tabCollections.TabIndex = 6;
            this.tabCollections.Text = "Collections";
            this.tabCollections.UseVisualStyleBackColor = true;
            // 
            // tabInteroperability
            // 
            this.tabInteroperability.Location = new System.Drawing.Point(4, 22);
            this.tabInteroperability.Name = "tabInteroperability";
            this.tabInteroperability.Padding = new System.Windows.Forms.Padding(3);
            this.tabInteroperability.Size = new System.Drawing.Size(501, 133);
            this.tabInteroperability.TabIndex = 3;
            this.tabInteroperability.Text = "Interoperability";
            this.tabInteroperability.UseVisualStyleBackColor = true;
            // 
            // tabStringMenu
            // 
            this.tabStringMenu.Location = new System.Drawing.Point(4, 22);
            this.tabStringMenu.Name = "tabStringMenu";
            this.tabStringMenu.Padding = new System.Windows.Forms.Padding(3);
            this.tabStringMenu.Size = new System.Drawing.Size(501, 133);
            this.tabStringMenu.TabIndex = 4;
            this.tabStringMenu.Text = "Strings";
            this.tabStringMenu.UseVisualStyleBackColor = true;
            // 
            // tabFileMenu
            // 
            this.tabFileMenu.Location = new System.Drawing.Point(4, 22);
            this.tabFileMenu.Name = "tabFileMenu";
            this.tabFileMenu.Size = new System.Drawing.Size(501, 133);
            this.tabFileMenu.TabIndex = 5;
            this.tabFileMenu.Text = " Files";
            this.tabFileMenu.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.AutoScrollMinSize = new System.Drawing.Size(800, 600);
            this.ClientSize = new System.Drawing.Size(1264, 671);
            this.Controls.Add(this.mainLayoutPanel);
            this.Name = "Form1";
            this.Text = "PUPPICAD 073 - No warranty provided or implied.";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.viewswitcher.ResumeLayout(false);
            this.tabPUPPICADBasic.ResumeLayout(false);
            this.menuTabControl.ResumeLayout(false);
            this.tabPUPPICADPrimitives.ResumeLayout(false);
            this.tabPUPPICADCurves.ResumeLayout(false);
            this.advancedWPF.ResumeLayout(false);
            this.aWPFtabControl.ResumeLayout(false);
            this.advancedHelix3D.ResumeLayout(false);
            this.aHelix3DtabControl.ResumeLayout(false);
            this.pluginsTabControl.ResumeLayout(false);
            this.dllModules.ResumeLayout(false);
            this.mainLayoutPanel.ResumeLayout(false);
            this.menuLayoutPanel.ResumeLayout(false);
            this.miscButtonLayoutPanel.ResumeLayout(false);
            this.miscButtonLayoutPanel.PerformLayout();
            this.tabControlGeneralModules.ResumeLayout(false);
            this.tabMathFunctions.ResumeLayout(false);
            this.tabAdvancedMath.ResumeLayout(false);
            this.tabProgramFlow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl viewswitcher;
        private System.Windows.Forms.TabPage PUPPIcanvaspage;
        private System.Windows.Forms.TabPage PUPPIcad3dviewpage;
        private System.Windows.Forms.TabPage tabPUPPICADBasic;
        private System.Windows.Forms.TabControl menuTabControl;
        private System.Windows.Forms.TabPage advancedWPF;
        private System.Windows.Forms.TabPage advancedHelix3D;
        private System.Windows.Forms.TabControl aWPFtabControl;
        private System.Windows.Forms.TabPage aWPFtabPage1;
        private System.Windows.Forms.TabControl aHelix3DtabControl;
        private System.Windows.Forms.TabPage aHelix3Dtabpage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel menuLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel dropDownLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel miscButtonLayoutPanel;
        private System.Windows.Forms.TabControl tabControlGeneralModules;
        private System.Windows.Forms.TabPage tabMathFunctions;
        private System.Windows.Forms.TabPage tabProgramFlow;
        private System.Windows.Forms.TabPage tabInteroperability;
        private System.Windows.Forms.TableLayoutPanel generalProgrammingMenuLayout;
        private System.Windows.Forms.TabPage tabPUPPICADPrimitives;
        private System.Windows.Forms.TableLayoutPanel primitivesLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel basicCADLayoutPanel;
        private System.Windows.Forms.TabPage tabStringMenu;
        private System.Windows.Forms.TabPage tabFileMenu;
        private System.Windows.Forms.CheckBox debugcheck;
        private System.Windows.Forms.Button myFirstProgram;
        private System.Windows.Forms.TabPage pluginsTabControl;
        private System.Windows.Forms.TabControl plugTabControl;
        private System.Windows.Forms.TabPage tabCollections;
        private System.Windows.Forms.TabPage dllModules;
        private System.Windows.Forms.TabControl dllTabControl;
        private System.Windows.Forms.TabPage tabPUPPICADCurves;
        private System.Windows.Forms.TableLayoutPanel curvesLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel basicMathLayoutPanel;
        private System.Windows.Forms.TabPage tabAdvancedMath;
        private System.Windows.Forms.TableLayoutPanel advancedMathTableLayoutPanel;
    }
}

