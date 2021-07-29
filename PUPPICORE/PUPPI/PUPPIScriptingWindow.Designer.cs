namespace PUPPI
{
    partial class PUPPIScriptingWindow
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.loadedDLLList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.loadDLLScript = new System.Windows.Forms.Button();
            this.removeScriptDLL = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.addInput = new System.Windows.Forms.Button();
            this.removeInput = new System.Windows.Forms.Button();
            this.renSelInp = new System.Windows.Forms.Button();
            this.inputsArrayList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.addoutput1 = new System.Windows.Forms.Button();
            this.removeOutput = new System.Windows.Forms.Button();
            this.renLastOut = new System.Windows.Forms.Button();
            this.outputsArrayList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.doneButton = new System.Windows.Forms.Button();
            this.tesRunButton = new System.Windows.Forms.Button();
            this.codeInConsole = new System.Windows.Forms.Button();
            this.addOutput = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.vbRadioButton = new System.Windows.Forms.RadioButton();
            this.csRadioButton = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.autoLoadBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.consoleTextBox = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.addOutput, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel7, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.4902F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.38503F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(634, 627);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel8, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel10, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.33334F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(628, 154);
            this.tableLayoutPanel2.TabIndex = 0;
            this.tableLayoutPanel2.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel2_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Loaded DLLs";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(212, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Input ArrayList: nodeInputs[]";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(421, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(204, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Outputs ArrayList: scriptOutputs[]";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.loadedDLLList, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(203, 123);
            this.tableLayoutPanel3.TabIndex = 6;
            this.tableLayoutPanel3.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel3_Paint);
            // 
            // loadedDLLList
            // 
            this.loadedDLLList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadedDLLList.FormattingEnabled = true;
            this.loadedDLLList.HorizontalScrollbar = true;
            this.loadedDLLList.Location = new System.Drawing.Point(3, 3);
            this.loadedDLLList.Name = "loadedDLLList";
            this.loadedDLLList.Size = new System.Drawing.Size(197, 80);
            this.loadedDLLList.TabIndex = 1;
            this.loadedDLLList.SelectedIndexChanged += new System.EventHandler(this.loadedDLLList_SelectedIndexChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.loadDLLScript, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.removeScriptDLL, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.helpButton, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 89);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(197, 31);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // loadDLLScript
            // 
            this.loadDLLScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadDLLScript.Location = new System.Drawing.Point(3, 3);
            this.loadDLLScript.Name = "loadDLLScript";
            this.loadDLLScript.Size = new System.Drawing.Size(59, 25);
            this.loadDLLScript.TabIndex = 0;
            this.loadDLLScript.Text = "Add";
            this.loadDLLScript.UseVisualStyleBackColor = true;
            this.loadDLLScript.Click += new System.EventHandler(this.loadDLLScript_Click);
            // 
            // removeScriptDLL
            // 
            this.removeScriptDLL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.removeScriptDLL.Location = new System.Drawing.Point(68, 3);
            this.removeScriptDLL.Name = "removeScriptDLL";
            this.removeScriptDLL.Size = new System.Drawing.Size(59, 25);
            this.removeScriptDLL.TabIndex = 1;
            this.removeScriptDLL.Text = "Remove";
            this.removeScriptDLL.UseVisualStyleBackColor = true;
            this.removeScriptDLL.Click += new System.EventHandler(this.removeScriptDLL_Click);
            // 
            // helpButton
            // 
            this.helpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpButton.Location = new System.Drawing.Point(133, 3);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(61, 25);
            this.helpButton.TabIndex = 2;
            this.helpButton.Text = "Help";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel14, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.inputsArrayList, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(212, 28);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(203, 123);
            this.tableLayoutPanel8.TabIndex = 7;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 3;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel14.Controls.Add(this.addInput, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.removeInput, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.renSelInp, 2, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(3, 89);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(197, 31);
            this.tableLayoutPanel14.TabIndex = 5;
            this.tableLayoutPanel14.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel14_Paint);
            // 
            // addInput
            // 
            this.addInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addInput.Location = new System.Drawing.Point(3, 3);
            this.addInput.Name = "addInput";
            this.addInput.Size = new System.Drawing.Size(33, 25);
            this.addInput.TabIndex = 0;
            this.addInput.Text = "Add";
            this.addInput.UseVisualStyleBackColor = true;
            this.addInput.Click += new System.EventHandler(this.addInput_Click_1);
            // 
            // removeInput
            // 
            this.removeInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.removeInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeInput.Location = new System.Drawing.Point(42, 3);
            this.removeInput.Name = "removeInput";
            this.removeInput.Size = new System.Drawing.Size(62, 25);
            this.removeInput.TabIndex = 1;
            this.removeInput.Text = "Erase Last";
            this.removeInput.UseVisualStyleBackColor = true;
            this.removeInput.Click += new System.EventHandler(this.removeInput_Click_1);
            // 
            // renSelInp
            // 
            this.renSelInp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renSelInp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.renSelInp.Location = new System.Drawing.Point(110, 3);
            this.renSelInp.Name = "renSelInp";
            this.renSelInp.Size = new System.Drawing.Size(84, 25);
            this.renSelInp.TabIndex = 2;
            this.renSelInp.Text = "Rename Sel.";
            this.renSelInp.UseVisualStyleBackColor = true;
            this.renSelInp.Click += new System.EventHandler(this.renSelInp_Click);
            // 
            // inputsArrayList
            // 
            this.inputsArrayList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputsArrayList.FormattingEnabled = true;
            this.inputsArrayList.Location = new System.Drawing.Point(3, 3);
            this.inputsArrayList.Name = "inputsArrayList";
            this.inputsArrayList.Size = new System.Drawing.Size(197, 80);
            this.inputsArrayList.TabIndex = 4;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel15, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.outputsArrayList, 0, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(421, 28);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 2;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(204, 123);
            this.tableLayoutPanel10.TabIndex = 8;
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 3;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel15.Controls.Add(this.addoutput1, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.removeOutput, 1, 0);
            this.tableLayoutPanel15.Controls.Add(this.renLastOut, 2, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(3, 89);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(198, 31);
            this.tableLayoutPanel15.TabIndex = 7;
            // 
            // addoutput1
            // 
            this.addoutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addoutput1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addoutput1.Location = new System.Drawing.Point(3, 3);
            this.addoutput1.Name = "addoutput1";
            this.addoutput1.Size = new System.Drawing.Size(33, 25);
            this.addoutput1.TabIndex = 0;
            this.addoutput1.Text = "Add";
            this.addoutput1.UseVisualStyleBackColor = true;
            this.addoutput1.Click += new System.EventHandler(this.addoutput1_Click_1);
            // 
            // removeOutput
            // 
            this.removeOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.removeOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeOutput.Location = new System.Drawing.Point(42, 3);
            this.removeOutput.Name = "removeOutput";
            this.removeOutput.Size = new System.Drawing.Size(63, 25);
            this.removeOutput.TabIndex = 1;
            this.removeOutput.Text = "Erase Last";
            this.removeOutput.UseVisualStyleBackColor = true;
            this.removeOutput.Click += new System.EventHandler(this.removeOutput_Click_1);
            // 
            // renLastOut
            // 
            this.renLastOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renLastOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.renLastOut.Location = new System.Drawing.Point(111, 3);
            this.renLastOut.Name = "renLastOut";
            this.renLastOut.Size = new System.Drawing.Size(84, 25);
            this.renLastOut.TabIndex = 2;
            this.renLastOut.Text = "Rename Sel.";
            this.renLastOut.UseVisualStyleBackColor = true;
            this.renLastOut.Click += new System.EventHandler(this.renLastOut_Click);
            // 
            // outputsArrayList
            // 
            this.outputsArrayList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputsArrayList.FormattingEnabled = true;
            this.outputsArrayList.Location = new System.Drawing.Point(3, 3);
            this.outputsArrayList.Name = "outputsArrayList";
            this.outputsArrayList.Size = new System.Drawing.Size(198, 80);
            this.outputsArrayList.TabIndex = 6;
            this.toolTip1.SetToolTip(this.outputsArrayList, "In your code, add outputs with scriptOutputs.Add()");
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Controls.Add(this.cancelButton, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.doneButton, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.tesRunButton, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.codeInConsole, 3, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 441);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(628, 41);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(317, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(151, 35);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // doneButton
            // 
            this.doneButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doneButton.Location = new System.Drawing.Point(160, 3);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(151, 35);
            this.doneButton.TabIndex = 1;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // tesRunButton
            // 
            this.tesRunButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tesRunButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tesRunButton.Location = new System.Drawing.Point(3, 3);
            this.tesRunButton.Name = "tesRunButton";
            this.tesRunButton.Size = new System.Drawing.Size(151, 35);
            this.tesRunButton.TabIndex = 0;
            this.tesRunButton.Text = "Test Run / Refresh";
            this.tesRunButton.UseVisualStyleBackColor = true;
            this.tesRunButton.Click += new System.EventHandler(this.tesRunButton_Click);
            // 
            // codeInConsole
            // 
            this.codeInConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeInConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.codeInConsole.Location = new System.Drawing.Point(474, 3);
            this.codeInConsole.Name = "codeInConsole";
            this.codeInConsole.Size = new System.Drawing.Size(151, 35);
            this.codeInConsole.TabIndex = 3;
            this.codeInConsole.Text = "Show Full Code";
            this.codeInConsole.UseVisualStyleBackColor = true;
            this.codeInConsole.Click += new System.EventHandler(this.codeInConsole_Click);
            // 
            // addOutput
            // 
            this.addOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addOutput.Location = new System.Drawing.Point(3, 163);
            this.addOutput.Multiline = true;
            this.addOutput.Name = "addOutput";
            this.addOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.addOutput.Size = new System.Drawing.Size(628, 272);
            this.addOutput.TabIndex = 2;
            this.addOutput.TextChanged += new System.EventHandler(this.scriptTextBox_TextChanged);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel12, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel13, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 488);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(628, 41);
            this.tableLayoutPanel6.TabIndex = 3;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 3;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.11037F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.44481F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.44482F));
            this.tableLayoutPanel12.Controls.Add(this.vbRadioButton, 1, 0);
            this.tableLayoutPanel12.Controls.Add(this.csRadioButton, 1, 0);
            this.tableLayoutPanel12.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(306, 33);
            this.tableLayoutPanel12.TabIndex = 2;
            this.tableLayoutPanel12.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel12_Paint);
            // 
            // vbRadioButton
            // 
            this.vbRadioButton.AutoSize = true;
            this.vbRadioButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vbRadioButton.Location = new System.Drawing.Point(104, 3);
            this.vbRadioButton.Name = "vbRadioButton";
            this.vbRadioButton.Size = new System.Drawing.Size(96, 27);
            this.vbRadioButton.TabIndex = 2;
            this.vbRadioButton.TabStop = true;
            this.vbRadioButton.Text = "VB";
            this.vbRadioButton.UseVisualStyleBackColor = true;
            // 
            // csRadioButton
            // 
            this.csRadioButton.AutoSize = true;
            this.csRadioButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.csRadioButton.Location = new System.Drawing.Point(206, 3);
            this.csRadioButton.Name = "csRadioButton";
            this.csRadioButton.Size = new System.Drawing.Size(97, 27);
            this.csRadioButton.TabIndex = 1;
            this.csRadioButton.TabStop = true;
            this.csRadioButton.Text = "C#";
            this.csRadioButton.UseVisualStyleBackColor = true;
            this.csRadioButton.CheckedChanged += new System.EventHandler(this.csRadioButton_CheckedChanged_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 33);
            this.label5.TabIndex = 3;
            this.label5.Text = "Language";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Controls.Add(this.autoLoadBox, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(317, 4);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(307, 33);
            this.tableLayoutPanel13.TabIndex = 3;
            // 
            // autoLoadBox
            // 
            this.autoLoadBox.AutoSize = true;
            this.autoLoadBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoLoadBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoLoadBox.Location = new System.Drawing.Point(3, 3);
            this.autoLoadBox.Name = "autoLoadBox";
            this.autoLoadBox.Size = new System.Drawing.Size(147, 27);
            this.autoLoadBox.TabIndex = 0;
            this.autoLoadBox.Text = "Auto-load DLL Namespaces";
            this.autoLoadBox.UseVisualStyleBackColor = true;
            this.autoLoadBox.CheckedChanged += new System.EventHandler(this.autoLoadBox_CheckedChanged);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.consoleTextBox, 0, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 535);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(628, 89);
            this.tableLayoutPanel7.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(622, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Console";
            // 
            // consoleTextBox
            // 
            this.consoleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleTextBox.Location = new System.Drawing.Point(3, 20);
            this.consoleTextBox.Multiline = true;
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.consoleTextBox.Size = new System.Drawing.Size(622, 66);
            this.consoleTextBox.TabIndex = 1;
            // 
            // toolTip1
            // 
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // PUPPIScriptingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 627);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(350, 300);
            this.Name = "PUPPIScriptingWindow";
            this.Text = "PUPPI Script";
            this.Load += new System.EventHandler(this.PUPPIScriptingWindow_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel13.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListBox loadedDLLList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button loadDLLScript;
        private System.Windows.Forms.Button removeScriptDLL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button doneButton;
        private System.Windows.Forms.Button tesRunButton;
        private System.Windows.Forms.TextBox addOutput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox consoleTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Button codeInConsole;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.ListBox inputsArrayList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.ListBox outputsArrayList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.RadioButton vbRadioButton;
        private System.Windows.Forms.RadioButton csRadioButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.CheckBox autoLoadBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.Button addInput;
        private System.Windows.Forms.Button removeInput;
        private System.Windows.Forms.Button renSelInp;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.Button addoutput1;
        private System.Windows.Forms.Button removeOutput;
        private System.Windows.Forms.Button renLastOut;
    }
}