namespace PUPPIGUI
{
    partial class PUPPITreeView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PUPPINodeTree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // PUPPINodeTree
            // 
            this.PUPPINodeTree.Location = new System.Drawing.Point(22, 17);
            this.PUPPINodeTree.Name = "PUPPINodeTree";
            this.PUPPINodeTree.Size = new System.Drawing.Size(387, 306);
            this.PUPPINodeTree.TabIndex = 0;
            // 
            // PUPPITreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PUPPINodeTree);
            this.Name = "PUPPITreeView";
            this.Size = new System.Drawing.Size(429, 339);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TreeView PUPPINodeTree;
    }
}
