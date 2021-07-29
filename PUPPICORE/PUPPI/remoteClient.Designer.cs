namespace PUPPI
{
    partial class remoteClient
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.passtxt = new System.Windows.Forms.TextBox();
            this.portxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lip = new System.Windows.Forms.Label();
            this.iptxt = new System.Windows.Forms.TextBox();
            this.okbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timeoutText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.cancelButton, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.passtxt, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.portxt, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.iptxt, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.okbutton, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.timeoutText, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 261);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(145, 211);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(136, 47);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // passtxt
            // 
            this.passtxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passtxt.Location = new System.Drawing.Point(145, 159);
            this.passtxt.Name = "passtxt";
            this.passtxt.PasswordChar = '*';
            this.passtxt.Size = new System.Drawing.Size(136, 20);
            this.passtxt.TabIndex = 7;
            // 
            // portxt
            // 
            this.portxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.portxt.Location = new System.Drawing.Point(145, 55);
            this.portxt.Name = "portxt";
            this.portxt.Size = new System.Drawing.Size(136, 20);
            this.portxt.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 52);
            this.label4.TabIndex = 4;
            this.label4.Text = "Server Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 52);
            this.label2.TabIndex = 2;
            this.label2.Text = "Server Port";
            // 
            // lip
            // 
            this.lip.AutoSize = true;
            this.lip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lip.Location = new System.Drawing.Point(3, 0);
            this.lip.Name = "lip";
            this.lip.Size = new System.Drawing.Size(136, 52);
            this.lip.TabIndex = 0;
            this.lip.Text = "Server IP Address";
            this.lip.Click += new System.EventHandler(this.lip_Click);
            // 
            // iptxt
            // 
            this.iptxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iptxt.Location = new System.Drawing.Point(145, 3);
            this.iptxt.Name = "iptxt";
            this.iptxt.Size = new System.Drawing.Size(136, 20);
            this.iptxt.TabIndex = 5;
            // 
            // okbutton
            // 
            this.okbutton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okbutton.Location = new System.Drawing.Point(3, 211);
            this.okbutton.Name = "okbutton";
            this.okbutton.Size = new System.Drawing.Size(136, 47);
            this.okbutton.TabIndex = 8;
            this.okbutton.Text = "OK";
            this.okbutton.UseVisualStyleBackColor = true;
            this.okbutton.Click += new System.EventHandler(this.okbutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 52);
            this.label1.TabIndex = 10;
            this.label1.Text = "Timeout (ms)";
            // 
            // timeoutText
            // 
            this.timeoutText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeoutText.Location = new System.Drawing.Point(145, 107);
            this.timeoutText.Name = "timeoutText";
            this.timeoutText.Size = new System.Drawing.Size(136, 20);
            this.timeoutText.TabIndex = 11;
            // 
            // remoteClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "remoteClient";
            this.Text = "remoteClient";
            this.Load += new System.EventHandler(this.remoteClient_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox passtxt;
        private System.Windows.Forms.TextBox portxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lip;
        private System.Windows.Forms.TextBox iptxt;
        private System.Windows.Forms.Button okbutton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox timeoutText;
    }
}