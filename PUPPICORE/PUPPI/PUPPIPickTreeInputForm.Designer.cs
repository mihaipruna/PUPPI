namespace PUPPIGUI
{
    internal partial class PUPPIPickTreeInputForm
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
            this.sinput = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sinput
            // 
            this.sinput.Location = new System.Drawing.Point(72, 390);
            this.sinput.Name = "sinput";
            this.sinput.Size = new System.Drawing.Size(75, 23);
            this.sinput.TabIndex = 0;
            this.sinput.Text = "Set Input";
            this.sinput.UseVisualStyleBackColor = true;
            this.sinput.Click += new System.EventHandler(this.sinput_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(211, 390);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // PUPPIPickTreeInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 433);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.sinput);
            this.Name = "PUPPIPickTreeInputForm";
            this.Text = "Pick a Node Output";
            this.Load += new System.EventHandler(this.PUPPIPickTreeInputForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button sinput;
        private System.Windows.Forms.Button cancel;
    }
}