namespace PUPPI
{
    partial class inputModulePicker
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mNumber = new System.Windows.Forms.Label();
            this.modList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.fBox = new System.Windows.Forms.TextBox();
            this.aBut = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.useSel = new System.Windows.Forms.Button();
            this.cBut = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.mNumber, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.modList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(387, 416);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mNumber
            // 
            this.mNumber.AutoSize = true;
            this.mNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mNumber.Location = new System.Drawing.Point(3, 0);
            this.mNumber.Name = "mNumber";
            this.mNumber.Size = new System.Drawing.Size(381, 41);
            this.mNumber.TabIndex = 0;
            this.mNumber.Text = "label1";
            this.mNumber.Click += new System.EventHandler(this.mNumber_Click);
            // 
            // modList
            // 
            this.modList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modList.FormattingEnabled = true;
            this.modList.Location = new System.Drawing.Point(3, 44);
            this.modList.Name = "modList";
            this.modList.Size = new System.Drawing.Size(381, 285);
            this.modList.TabIndex = 1;
            this.modList.SelectedIndexChanged += new System.EventHandler(this.modList_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.fBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.aBut, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 335);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(381, 35);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Partial or full module name filter. Empty for all.";
            // 
            // fBox
            // 
            this.fBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fBox.Location = new System.Drawing.Point(130, 3);
            this.fBox.Name = "fBox";
            this.fBox.Size = new System.Drawing.Size(121, 20);
            this.fBox.TabIndex = 1;
            // 
            // aBut
            // 
            this.aBut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aBut.Location = new System.Drawing.Point(257, 3);
            this.aBut.Name = "aBut";
            this.aBut.Size = new System.Drawing.Size(121, 29);
            this.aBut.TabIndex = 2;
            this.aBut.Text = "Apply";
            this.aBut.UseVisualStyleBackColor = true;
            this.aBut.Click += new System.EventHandler(this.aBut_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.useSel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cBut, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 376);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(381, 37);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // useSel
            // 
            this.useSel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.useSel.Location = new System.Drawing.Point(3, 3);
            this.useSel.Name = "useSel";
            this.useSel.Size = new System.Drawing.Size(184, 31);
            this.useSel.TabIndex = 0;
            this.useSel.Text = "Use Selected";
            this.useSel.UseVisualStyleBackColor = true;
            this.useSel.Click += new System.EventHandler(this.useSel_Click);
            // 
            // cBut
            // 
            this.cBut.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.cBut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cBut.Location = new System.Drawing.Point(193, 3);
            this.cBut.Name = "cBut";
            this.cBut.Size = new System.Drawing.Size(185, 31);
            this.cBut.TabIndex = 1;
            this.cBut.Text = "Cancel";
            this.cBut.UseVisualStyleBackColor = true;
            this.cBut.Click += new System.EventHandler(this.cBut_Click);
            // 
            // inputModulePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 416);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "inputModulePicker";
            this.Text = "Module Picker";
            this.Load += new System.EventHandler(this.inputModulePicker_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label mNumber;
        private System.Windows.Forms.ListBox modList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fBox;
        private System.Windows.Forms.Button aBut;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button useSel;
        private System.Windows.Forms.Button cBut;
    }
}