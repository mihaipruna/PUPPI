namespace PUPPIGUI
{
    partial class setlistlevelform
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
            this.singleitem = new System.Windows.Forms.RadioButton();
            this.listproc = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // singleitem
            // 
            this.singleitem.AutoSize = true;
            this.singleitem.Location = new System.Drawing.Point(30, 30);
            this.singleitem.Name = "singleitem";
            this.singleitem.Size = new System.Drawing.Size(77, 17);
            this.singleitem.TabIndex = 0;
            this.singleitem.TabStop = true;
            this.singleitem.Text = "Single Item";
            this.singleitem.UseVisualStyleBackColor = true;
            this.singleitem.CheckedChanged += new System.EventHandler(this.singleitem_CheckedChanged);
            // 
            // listproc
            // 
            this.listproc.AutoSize = true;
            this.listproc.Location = new System.Drawing.Point(152, 30);
            this.listproc.Name = "listproc";
            this.listproc.Size = new System.Drawing.Size(96, 17);
            this.listproc.TabIndex = 1;
            this.listproc.TabStop = true;
            this.listproc.Text = "List Processing";
            this.listproc.UseVisualStyleBackColor = true;
            this.listproc.CheckedChanged += new System.EventHandler(this.listproc_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter Number of Elements:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(36, 91);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(98, 20);
            this.textBox1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 85);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Done";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button1_MouseDown);
            // 
            // setlistlevelform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 120);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listproc);
            this.Controls.Add(this.singleitem);
            this.Name = "setlistlevelform";
            this.Text = "Set Node Processing Type";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton singleitem;
        private System.Windows.Forms.RadioButton listproc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
    }
}