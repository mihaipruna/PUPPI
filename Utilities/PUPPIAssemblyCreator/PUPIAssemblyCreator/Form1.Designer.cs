namespace PUPPIAssemblyCreator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.generate_assembly = new System.Windows.Forms.Button();
            this.exit_program = new System.Windows.Forms.Button();
            this.assembly_name_label = new System.Windows.Forms.Label();
            this.mtps_file_name = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonNamespace = new System.Windows.Forms.RadioButton();
            this.radioButtonMTPS = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.load_mtps_file = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.load_dll_file = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.namespaceTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.assembly_dll_file = new System.Windows.Forms.Button();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.ass_name_textbox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.generate_assembly, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.exit_program, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.assembly_name_label, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.mtps_file_name, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(779, 331);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // generate_assembly
            // 
            this.generate_assembly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generate_assembly.Location = new System.Drawing.Point(3, 282);
            this.generate_assembly.Name = "generate_assembly";
            this.generate_assembly.Size = new System.Drawing.Size(383, 46);
            this.generate_assembly.TabIndex = 0;
            this.generate_assembly.Text = "Generate Assemblies";
            this.generate_assembly.UseVisualStyleBackColor = true;
            this.generate_assembly.Click += new System.EventHandler(this.generate_assembly_Click);
            // 
            // exit_program
            // 
            this.exit_program.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exit_program.Location = new System.Drawing.Point(392, 282);
            this.exit_program.Name = "exit_program";
            this.exit_program.Size = new System.Drawing.Size(384, 46);
            this.exit_program.TabIndex = 1;
            this.exit_program.Text = "Exit";
            this.exit_program.UseVisualStyleBackColor = true;
            this.exit_program.Click += new System.EventHandler(this.exit_program_Click);
            // 
            // assembly_name_label
            // 
            this.assembly_name_label.AutoSize = true;
            this.assembly_name_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assembly_name_label.Location = new System.Drawing.Point(392, 230);
            this.assembly_name_label.Name = "assembly_name_label";
            this.assembly_name_label.Size = new System.Drawing.Size(384, 49);
            this.assembly_name_label.TabIndex = 3;
            this.assembly_name_label.Text = "No assembly defined";
            this.assembly_name_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mtps_file_name
            // 
            this.mtps_file_name.AutoSize = true;
            this.mtps_file_name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtps_file_name.Location = new System.Drawing.Point(392, 164);
            this.mtps_file_name.Name = "mtps_file_name";
            this.mtps_file_name.Size = new System.Drawing.Size(384, 66);
            this.mtps_file_name.TabIndex = 5;
            this.mtps_file_name.Text = "no MTPS file loaded";
            this.mtps_file_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mtps_file_name.Click += new System.EventHandler(this.mtps_file_name_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(383, 99);
            this.label1.TabIndex = 6;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.radioButtonNamespace, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.radioButtonMTPS, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 102);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(383, 43);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // radioButtonNamespace
            // 
            this.radioButtonNamespace.AutoSize = true;
            this.radioButtonNamespace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonNamespace.Location = new System.Drawing.Point(194, 3);
            this.radioButtonNamespace.Name = "radioButtonNamespace";
            this.radioButtonNamespace.Size = new System.Drawing.Size(186, 37);
            this.radioButtonNamespace.TabIndex = 1;
            this.radioButtonNamespace.TabStop = true;
            this.radioButtonNamespace.Text = "DLL+Namespace";
            this.radioButtonNamespace.UseVisualStyleBackColor = true;
            this.radioButtonNamespace.CheckedChanged += new System.EventHandler(this.radioButtonNamespace_CheckedChanged);
            // 
            // radioButtonMTPS
            // 
            this.radioButtonMTPS.AutoSize = true;
            this.radioButtonMTPS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonMTPS.Location = new System.Drawing.Point(3, 3);
            this.radioButtonMTPS.Name = "radioButtonMTPS";
            this.radioButtonMTPS.Size = new System.Drawing.Size(185, 37);
            this.radioButtonMTPS.TabIndex = 0;
            this.radioButtonMTPS.TabStop = true;
            this.radioButtonMTPS.Text = "MTPS File";
            this.radioButtonMTPS.UseVisualStyleBackColor = true;
            this.radioButtonMTPS.CheckedChanged += new System.EventHandler(this.radioButtonMTPS_CheckedChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel3.Controls.Add(this.load_mtps_file, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 167);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(383, 60);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // load_mtps_file
            // 
            this.load_mtps_file.Dock = System.Windows.Forms.DockStyle.Fill;
            this.load_mtps_file.Location = new System.Drawing.Point(3, 3);
            this.load_mtps_file.Name = "load_mtps_file";
            this.load_mtps_file.Size = new System.Drawing.Size(121, 54);
            this.load_mtps_file.TabIndex = 6;
            this.load_mtps_file.Text = "Load MTPS File...";
            this.load_mtps_file.UseVisualStyleBackColor = true;
            this.load_mtps_file.Click += new System.EventHandler(this.load_mtps_file_Click_1);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.load_dll_file, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(130, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(250, 54);
            this.tableLayoutPanel4.TabIndex = 7;
            this.tableLayoutPanel4.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel4_Paint);
            // 
            // load_dll_file
            // 
            this.load_dll_file.Dock = System.Windows.Forms.DockStyle.Fill;
            this.load_dll_file.Location = new System.Drawing.Point(3, 3);
            this.load_dll_file.Name = "load_dll_file";
            this.load_dll_file.Size = new System.Drawing.Size(119, 48);
            this.load_dll_file.TabIndex = 7;
            this.load_dll_file.Text = "Load DLL File...";
            this.load_dll_file.UseVisualStyleBackColor = true;
            this.load_dll_file.Click += new System.EventHandler(this.load_dll_file_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.namespaceTextBox, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(128, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(119, 48);
            this.tableLayoutPanel5.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "Namespace in DLL:";
            // 
            // namespaceTextBox
            // 
            this.namespaceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.namespaceTextBox.Location = new System.Drawing.Point(3, 24);
            this.namespaceTextBox.Name = "namespaceTextBox";
            this.namespaceTextBox.Size = new System.Drawing.Size(113, 20);
            this.namespaceTextBox.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.assembly_dll_file, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 233);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(383, 43);
            this.tableLayoutPanel6.TabIndex = 9;
            // 
            // assembly_dll_file
            // 
            this.assembly_dll_file.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assembly_dll_file.Location = new System.Drawing.Point(3, 3);
            this.assembly_dll_file.Name = "assembly_dll_file";
            this.assembly_dll_file.Size = new System.Drawing.Size(185, 37);
            this.assembly_dll_file.TabIndex = 2;
            this.assembly_dll_file.Text = "Set Assembly Export DLL Folder...";
            this.assembly_dll_file.UseVisualStyleBackColor = true;
            this.assembly_dll_file.Click += new System.EventHandler(this.assembly_dll_file_Click);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.ass_name_textbox, 0, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(194, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(186, 37);
            this.tableLayoutPanel7.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Assembly File Suffix (not extension) ";
            // 
            // ass_name_textbox
            // 
            this.ass_name_textbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ass_name_textbox.Location = new System.Drawing.Point(3, 21);
            this.ass_name_textbox.Name = "ass_name_textbox";
            this.ass_name_textbox.Size = new System.Drawing.Size(180, 20);
            this.ass_name_textbox.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 331);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "PUPPI Assembly Creator ©2015 Mihai Pruna";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button generate_assembly;
        private System.Windows.Forms.Button exit_program;
        private System.Windows.Forms.Button assembly_dll_file;
        private System.Windows.Forms.Label assembly_name_label;
        private System.Windows.Forms.Label mtps_file_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton radioButtonNamespace;
        private System.Windows.Forms.RadioButton radioButtonMTPS;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button load_mtps_file;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button load_dll_file;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox namespaceTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ass_name_textbox;
    }
}

