namespace PUPPInamespaceexplorer
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.savefile = new System.Windows.Forms.Button();
            this.quitbutton = new System.Windows.Forms.Button();
            this.browsefile = new System.Windows.Forms.Button();
            this.assemblyL = new System.Windows.Forms.Label();
            this.assemblyPath = new System.Windows.Forms.Label();
            this.loadmtps = new System.Windows.Forms.Button();
            this.findMethod = new System.Windows.Forms.Button();
            this.findTextbox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 60);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(930, 450);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // savefile
            // 
            this.savefile.Location = new System.Drawing.Point(142, 3);
            this.savefile.Name = "savefile";
            this.savefile.Size = new System.Drawing.Size(101, 23);
            this.savefile.TabIndex = 1;
            this.savefile.Text = "Save Selected";
            this.savefile.UseVisualStyleBackColor = true;
            this.savefile.Click += new System.EventHandler(this.button1_Click);
            // 
            // quitbutton
            // 
            this.quitbutton.Location = new System.Drawing.Point(792, 3);
            this.quitbutton.Name = "quitbutton";
            this.quitbutton.Size = new System.Drawing.Size(75, 23);
            this.quitbutton.TabIndex = 2;
            this.quitbutton.Text = "Quit";
            this.quitbutton.UseVisualStyleBackColor = true;
            this.quitbutton.Click += new System.EventHandler(this.quitbutton_Click);
            // 
            // browsefile
            // 
            this.browsefile.Location = new System.Drawing.Point(3, 3);
            this.browsefile.Name = "browsefile";
            this.browsefile.Size = new System.Drawing.Size(87, 24);
            this.browsefile.TabIndex = 3;
            this.browsefile.Text = "Open Assembly";
            this.browsefile.UseVisualStyleBackColor = true;
            this.browsefile.Click += new System.EventHandler(this.browsefile_Click);
            // 
            // assemblyL
            // 
            this.assemblyL.AutoSize = true;
            this.assemblyL.Location = new System.Drawing.Point(96, 0);
            this.assemblyL.Name = "assemblyL";
            this.assemblyL.Size = new System.Drawing.Size(85, 39);
            this.assemblyL.TabIndex = 4;
            this.assemblyL.Text = "Assembly (Unsafe Loading Used):";
            this.assemblyL.Click += new System.EventHandler(this.assemblyL_Click);
            // 
            // assemblyPath
            // 
            this.assemblyPath.AutoSize = true;
            this.assemblyPath.Location = new System.Drawing.Point(189, 0);
            this.assemblyPath.Name = "assemblyPath";
            this.assemblyPath.Size = new System.Drawing.Size(0, 13);
            this.assemblyPath.TabIndex = 5;
            // 
            // loadmtps
            // 
            this.loadmtps.Location = new System.Drawing.Point(3, 3);
            this.loadmtps.Name = "loadmtps";
            this.loadmtps.Size = new System.Drawing.Size(101, 23);
            this.loadmtps.TabIndex = 6;
            this.loadmtps.Text = "Open MTPS File";
            this.loadmtps.UseVisualStyleBackColor = true;
            this.loadmtps.Click += new System.EventHandler(this.loadmtps_Click);
            // 
            // findMethod
            // 
            this.findMethod.Location = new System.Drawing.Point(281, 3);
            this.findMethod.Name = "findMethod";
            this.findMethod.Size = new System.Drawing.Size(46, 23);
            this.findMethod.TabIndex = 7;
            this.findMethod.Text = "Find";
            this.findMethod.UseVisualStyleBackColor = true;
            this.findMethod.Click += new System.EventHandler(this.findMethod_Click);
            // 
            // findTextbox
            // 
            this.findTextbox.Location = new System.Drawing.Point(420, 3);
            this.findTextbox.Name = "findTextbox";
            this.findTextbox.Size = new System.Drawing.Size(202, 20);
            this.findTextbox.TabIndex = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.treeView1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(936, 570);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel3.Controls.Add(this.browsefile, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.assemblyPath, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.assemblyL, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(930, 51);
            this.tableLayoutPanel3.TabIndex = 10;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.Controls.Add(this.loadmtps, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.savefile, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.findMethod, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.quitbutton, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.findTextbox, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 516);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(930, 51);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(936, 570);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(1440, 1024);
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PUPPI NameSpace Explorer  ©2015 Mihai Pruna";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button savefile;
        private System.Windows.Forms.Button quitbutton;
        private System.Windows.Forms.Button browsefile;
        private System.Windows.Forms.Label assemblyL;
        private System.Windows.Forms.Label assemblyPath;
        private System.Windows.Forms.Button loadmtps;
        private System.Windows.Forms.Button findMethod;
        private System.Windows.Forms.TextBox findTextbox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}

