﻿namespace PUPPI
{
    internal partial class aboutlicense
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(aboutlicense));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.abutt = new System.Windows.Forms.Button();
            this.dabutt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(29, 191);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(532, 113);
            this.textBox1.TabIndex = 0;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(29, 69);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(532, 103);
            this.textBox2.TabIndex = 1;
            this.textBox2.TabStop = false;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "PUPPI License:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Other Licenses:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(496, 36);
            this.label3.TabIndex = 4;
            this.label3.Text = "Parametric Universal Programming & Process Interface (PUPPI). Copyright 2013-2017" +
    " Mihai Pruna. This form must be displayed by all PUPPI-powered applications.";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // abutt
            // 
            this.abutt.Location = new System.Drawing.Point(115, 320);
            this.abutt.Name = "abutt";
            this.abutt.Size = new System.Drawing.Size(75, 23);
            this.abutt.TabIndex = 5;
            this.abutt.Text = "Agree";
            this.abutt.UseVisualStyleBackColor = true;
            this.abutt.Click += new System.EventHandler(this.button1_Click);
            // 
            // dabutt
            // 
            this.dabutt.Location = new System.Drawing.Point(370, 320);
            this.dabutt.Name = "dabutt";
            this.dabutt.Size = new System.Drawing.Size(75, 23);
            this.dabutt.TabIndex = 6;
            this.dabutt.Text = "Disagree";
            this.dabutt.UseVisualStyleBackColor = true;
            this.dabutt.Click += new System.EventHandler(this.dabutt_Click);
            // 
            // aboutlicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 366);
            this.Controls.Add(this.dabutt);
            this.Controls.Add(this.abutt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "aboutlicense";
            this.Text = "PUPPI Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button abutt;
        private System.Windows.Forms.Button dabutt;
    }
}