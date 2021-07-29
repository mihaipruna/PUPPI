namespace PUPPIGUI
{
    internal partial class PUPPInodeinfo
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
            this.ninfo = new System.Windows.Forms.Label();
            this.closeb = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.outputvalues = new System.Windows.Forms.DataGridView();
            this.inputvalues = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.showContainer = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputvalues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputvalues)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ninfo
            // 
            this.ninfo.AutoSize = true;
            this.ninfo.Location = new System.Drawing.Point(3, 0);
            this.ninfo.Name = "ninfo";
            this.ninfo.Size = new System.Drawing.Size(35, 13);
            this.ninfo.TabIndex = 2;
            this.ninfo.Text = "label1";
            // 
            // closeb
            // 
            this.closeb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.closeb.Location = new System.Drawing.Point(654, 3);
            this.closeb.Name = "closeb";
            this.closeb.Size = new System.Drawing.Size(214, 53);
            this.closeb.TabIndex = 3;
            this.closeb.Text = "Close";
            this.closeb.UseVisualStyleBackColor = true;
            this.closeb.Click += new System.EventHandler(this.closeb_Click);
            this.closeb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.closeb_MouseDown);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.outputvalues, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.inputvalues, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 287);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(871, 278);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // outputvalues
            // 
            this.outputvalues.AllowUserToAddRows = false;
            this.outputvalues.AllowUserToDeleteRows = false;
            this.outputvalues.AllowUserToOrderColumns = true;
            this.outputvalues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputvalues.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.outputvalues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.outputvalues.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.outputvalues.Location = new System.Drawing.Point(438, 3);
            this.outputvalues.Name = "outputvalues";
            this.outputvalues.ReadOnly = true;
            this.outputvalues.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.outputvalues.Size = new System.Drawing.Size(430, 272);
            this.outputvalues.TabIndex = 8;
            // 
            // inputvalues
            // 
            this.inputvalues.AllowUserToAddRows = false;
            this.inputvalues.AllowUserToDeleteRows = false;
            this.inputvalues.AllowUserToOrderColumns = true;
            this.inputvalues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputvalues.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.inputvalues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inputvalues.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.inputvalues.Location = new System.Drawing.Point(3, 3);
            this.inputvalues.Name = "inputvalues";
            this.inputvalues.ReadOnly = true;
            this.inputvalues.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.inputvalues.Size = new System.Drawing.Size(429, 272);
            this.inputvalues.TabIndex = 7;
            this.inputvalues.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.inputvalues_CellContentClick);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.ninfo, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(877, 633);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.closeb, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.showContainer, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 571);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(871, 59);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // showContainer
            // 
            this.showContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showContainer.Location = new System.Drawing.Point(3, 3);
            this.showContainer.Name = "showContainer";
            this.showContainer.Size = new System.Drawing.Size(211, 53);
            this.showContainer.TabIndex = 4;
            this.showContainer.Text = "Show Container Contents";
            this.showContainer.UseVisualStyleBackColor = true;
            this.showContainer.Click += new System.EventHandler(this.showContainer_Click);
            // 
            // PUPPInodeinfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 633);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "PUPPInodeinfo";
            this.Text = "Node Information";
            this.Load += new System.EventHandler(this.PUPPInodeinfo_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.outputvalues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputvalues)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label ninfo;
       private System.Windows.Forms.Button closeb;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal System.Windows.Forms.DataGridView outputvalues;
        internal System.Windows.Forms.DataGridView inputvalues;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        internal System.Windows.Forms.Button showContainer;
    }
}