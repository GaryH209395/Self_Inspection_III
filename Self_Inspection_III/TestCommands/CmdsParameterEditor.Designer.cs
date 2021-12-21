namespace Self_Inspection_III.TestCommands
{
    partial class CmdsParameterEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CmdsParameterEditor));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbbDevice = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gbParaType = new System.Windows.Forms.GroupBox();
            this.rbtnSignal = new System.Windows.Forms.RadioButton();
            this.rbtnLabel = new System.Windows.Forms.RadioButton();
            this.rbtnOperator = new System.Windows.Forms.RadioButton();
            this.rbtnConstant = new System.Windows.Forms.RadioButton();
            this.rbtnGlobal = new System.Windows.Forms.RadioButton();
            this.rbtnTemporary = new System.Windows.Forms.RadioButton();
            this.rbtnResult = new System.Windows.Forms.RadioButton();
            this.rbtnCondition = new System.Windows.Forms.RadioButton();
            this.gbConstEditType = new System.Windows.Forms.GroupBox();
            this.rbtnComboList = new System.Windows.Forms.RadioButton();
            this.rbtnEditBox = new System.Windows.Forms.RadioButton();
            this.dgvParameters = new System.Windows.Forms.DataGridView();
            this.ColParaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColParaValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblParaDescription = new System.Windows.Forms.Label();
            this.lblParaType = new System.Windows.Forms.Label();
            this.lblParaName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.gbParaType.SuspendLayout();
            this.gbConstEditType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameters)).BeginInit();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(448, 398);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.67857F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.32143F));
            this.tableLayoutPanel2.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.dgvParameters, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 117);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.40329F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.59671F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(448, 243);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cbbDevice);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(229, 28);
            this.panel5.TabIndex = 15;
            // 
            // cbbDevice
            // 
            this.cbbDevice.FormattingEnabled = true;
            this.cbbDevice.Location = new System.Drawing.Point(51, 3);
            this.cbbDevice.Name = "cbbDevice";
            this.cbbDevice.Size = new System.Drawing.Size(173, 21);
            this.cbbDevice.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gbParaType);
            this.panel3.Controls.Add(this.gbConstEditType);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(238, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(207, 203);
            this.panel3.TabIndex = 14;
            // 
            // gbParaType
            // 
            this.gbParaType.Controls.Add(this.rbtnSignal);
            this.gbParaType.Controls.Add(this.rbtnLabel);
            this.gbParaType.Controls.Add(this.rbtnOperator);
            this.gbParaType.Controls.Add(this.rbtnConstant);
            this.gbParaType.Controls.Add(this.rbtnGlobal);
            this.gbParaType.Controls.Add(this.rbtnTemporary);
            this.gbParaType.Controls.Add(this.rbtnResult);
            this.gbParaType.Controls.Add(this.rbtnCondition);
            this.gbParaType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbParaType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gbParaType.Location = new System.Drawing.Point(0, 0);
            this.gbParaType.Name = "gbParaType";
            this.gbParaType.Size = new System.Drawing.Size(207, 133);
            this.gbParaType.TabIndex = 11;
            this.gbParaType.TabStop = false;
            this.gbParaType.Text = "Parameter Type";
            // 
            // rbtnSignal
            // 
            this.rbtnSignal.AutoSize = true;
            this.rbtnSignal.Enabled = false;
            this.rbtnSignal.Location = new System.Drawing.Point(111, 89);
            this.rbtnSignal.Name = "rbtnSignal";
            this.rbtnSignal.Size = new System.Drawing.Size(53, 17);
            this.rbtnSignal.TabIndex = 7;
            this.rbtnSignal.Text = "Signal";
            this.rbtnSignal.UseVisualStyleBackColor = true;
            // 
            // rbtnLabel
            // 
            this.rbtnLabel.AutoSize = true;
            this.rbtnLabel.Location = new System.Drawing.Point(111, 66);
            this.rbtnLabel.Name = "rbtnLabel";
            this.rbtnLabel.Size = new System.Drawing.Size(51, 17);
            this.rbtnLabel.TabIndex = 6;
            this.rbtnLabel.Text = "Label";
            this.rbtnLabel.UseVisualStyleBackColor = true;
            // 
            // rbtnOperator
            // 
            this.rbtnOperator.AutoSize = true;
            this.rbtnOperator.Location = new System.Drawing.Point(111, 43);
            this.rbtnOperator.Name = "rbtnOperator";
            this.rbtnOperator.Size = new System.Drawing.Size(68, 17);
            this.rbtnOperator.TabIndex = 5;
            this.rbtnOperator.Text = "Operator";
            this.rbtnOperator.UseVisualStyleBackColor = true;
            // 
            // rbtnConstant
            // 
            this.rbtnConstant.AutoSize = true;
            this.rbtnConstant.Location = new System.Drawing.Point(112, 20);
            this.rbtnConstant.Name = "rbtnConstant";
            this.rbtnConstant.Size = new System.Drawing.Size(67, 17);
            this.rbtnConstant.TabIndex = 4;
            this.rbtnConstant.Text = "Constant";
            this.rbtnConstant.UseVisualStyleBackColor = true;
            // 
            // rbtnGlobal
            // 
            this.rbtnGlobal.AutoSize = true;
            this.rbtnGlobal.Enabled = false;
            this.rbtnGlobal.Location = new System.Drawing.Point(6, 89);
            this.rbtnGlobal.Name = "rbtnGlobal";
            this.rbtnGlobal.Size = new System.Drawing.Size(80, 17);
            this.rbtnGlobal.TabIndex = 3;
            this.rbtnGlobal.Text = "Global Var.";
            this.rbtnGlobal.UseVisualStyleBackColor = true;
            // 
            // rbtnTemporary
            // 
            this.rbtnTemporary.AutoSize = true;
            this.rbtnTemporary.Location = new System.Drawing.Point(6, 66);
            this.rbtnTemporary.Name = "rbtnTemporary";
            this.rbtnTemporary.Size = new System.Drawing.Size(99, 17);
            this.rbtnTemporary.TabIndex = 2;
            this.rbtnTemporary.Text = "Temporary Var.";
            this.rbtnTemporary.UseVisualStyleBackColor = true;
            // 
            // rbtnResult
            // 
            this.rbtnResult.AutoSize = true;
            this.rbtnResult.Location = new System.Drawing.Point(6, 43);
            this.rbtnResult.Name = "rbtnResult";
            this.rbtnResult.Size = new System.Drawing.Size(78, 17);
            this.rbtnResult.TabIndex = 1;
            this.rbtnResult.Text = "Test Result";
            this.rbtnResult.UseVisualStyleBackColor = true;
            // 
            // rbtnCondition
            // 
            this.rbtnCondition.AutoSize = true;
            this.rbtnCondition.Checked = true;
            this.rbtnCondition.Location = new System.Drawing.Point(6, 20);
            this.rbtnCondition.Name = "rbtnCondition";
            this.rbtnCondition.Size = new System.Drawing.Size(95, 17);
            this.rbtnCondition.TabIndex = 0;
            this.rbtnCondition.TabStop = true;
            this.rbtnCondition.Text = "Test Condition";
            this.rbtnCondition.UseVisualStyleBackColor = true;
            // 
            // gbConstEditType
            // 
            this.gbConstEditType.Controls.Add(this.rbtnComboList);
            this.gbConstEditType.Controls.Add(this.rbtnEditBox);
            this.gbConstEditType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbConstEditType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gbConstEditType.Location = new System.Drawing.Point(0, 133);
            this.gbConstEditType.Name = "gbConstEditType";
            this.gbConstEditType.Size = new System.Drawing.Size(207, 70);
            this.gbConstEditType.TabIndex = 10;
            this.gbConstEditType.TabStop = false;
            this.gbConstEditType.Text = "Constant Edit Type";
            // 
            // rbtnComboList
            // 
            this.rbtnComboList.AutoSize = true;
            this.rbtnComboList.Location = new System.Drawing.Point(6, 43);
            this.rbtnComboList.Name = "rbtnComboList";
            this.rbtnComboList.Size = new System.Drawing.Size(77, 17);
            this.rbtnComboList.TabIndex = 4;
            this.rbtnComboList.Text = "ComboList";
            this.rbtnComboList.UseVisualStyleBackColor = true;
            // 
            // rbtnEditBox
            // 
            this.rbtnEditBox.AutoSize = true;
            this.rbtnEditBox.Checked = true;
            this.rbtnEditBox.Location = new System.Drawing.Point(6, 20);
            this.rbtnEditBox.Name = "rbtnEditBox";
            this.rbtnEditBox.Size = new System.Drawing.Size(65, 17);
            this.rbtnEditBox.TabIndex = 0;
            this.rbtnEditBox.TabStop = true;
            this.rbtnEditBox.Text = "EditBox";
            this.rbtnEditBox.UseVisualStyleBackColor = true;
            // 
            // dgvParameters
            // 
            this.dgvParameters.AllowUserToAddRows = false;
            this.dgvParameters.AllowUserToDeleteRows = false;
            this.dgvParameters.AllowUserToResizeColumns = false;
            this.dgvParameters.AllowUserToResizeRows = false;
            this.dgvParameters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColParaName,
            this.ColParaValue});
            this.dgvParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvParameters.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvParameters.Location = new System.Drawing.Point(3, 37);
            this.dgvParameters.MultiSelect = false;
            this.dgvParameters.Name = "dgvParameters";
            this.dgvParameters.RowHeadersVisible = false;
            this.dgvParameters.RowTemplate.Height = 24;
            this.dgvParameters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvParameters.ShowCellErrors = false;
            this.dgvParameters.ShowCellToolTips = false;
            this.dgvParameters.ShowEditingIcon = false;
            this.dgvParameters.ShowRowErrors = false;
            this.dgvParameters.Size = new System.Drawing.Size(229, 203);
            this.dgvParameters.TabIndex = 13;
            this.dgvParameters.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvParameters_CellEndEdit);
            // 
            // ColParaName
            // 
            this.ColParaName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ColParaName.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColParaName.HeaderText = "Parameter Name";
            this.ColParaName.Name = "ColParaName";
            this.ColParaName.ReadOnly = true;
            this.ColParaName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColParaName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColParaName.Width = 124;
            // 
            // ColParaValue
            // 
            this.ColParaValue.FillWeight = 75F;
            this.ColParaValue.HeaderText = "Value";
            this.ColParaValue.Name = "ColParaValue";
            this.ColParaValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColParaValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblParaDescription);
            this.panel2.Controls.Add(this.lblParaType);
            this.panel2.Controls.Add(this.lblParaName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(448, 117);
            this.panel2.TabIndex = 3;
            // 
            // lblParaDescription
            // 
            this.lblParaDescription.Location = new System.Drawing.Point(12, 29);
            this.lblParaDescription.Name = "lblParaDescription";
            this.lblParaDescription.Size = new System.Drawing.Size(395, 75);
            this.lblParaDescription.TabIndex = 2;
            this.lblParaDescription.Text = "Description";
            // 
            // lblParaType
            // 
            this.lblParaType.AutoSize = true;
            this.lblParaType.Location = new System.Drawing.Point(107, 9);
            this.lblParaType.Name = "lblParaType";
            this.lblParaType.Size = new System.Drawing.Size(30, 13);
            this.lblParaType.TabIndex = 1;
            this.lblParaType.Text = "Type";
            // 
            // lblParaName
            // 
            this.lblParaName.AutoSize = true;
            this.lblParaName.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParaName.Location = new System.Drawing.Point(3, 9);
            this.lblParaName.Name = "lblParaName";
            this.lblParaName.Size = new System.Drawing.Size(98, 13);
            this.lblParaName.TabIndex = 0;
            this.lblParaName.Text = "Parameter Name";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.38095F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.61905F));
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Garamond", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 360);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(448, 38);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(237, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(208, 32);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(228, 32);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // CmdsParameterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 398);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CmdsParameterEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parameter Editor";
            this.Load += new System.EventHandler(this.Editor_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.gbParaType.ResumeLayout(false);
            this.gbParaType.PerformLayout();
            this.gbConstEditType.ResumeLayout(false);
            this.gbConstEditType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameters)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblParaDescription;
        private System.Windows.Forms.Label lblParaType;
        private System.Windows.Forms.Label lblParaName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox cbbDevice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox gbParaType;
        private System.Windows.Forms.RadioButton rbtnSignal;
        private System.Windows.Forms.RadioButton rbtnLabel;
        private System.Windows.Forms.RadioButton rbtnOperator;
        private System.Windows.Forms.RadioButton rbtnConstant;
        private System.Windows.Forms.RadioButton rbtnGlobal;
        private System.Windows.Forms.RadioButton rbtnTemporary;
        private System.Windows.Forms.RadioButton rbtnResult;
        private System.Windows.Forms.RadioButton rbtnCondition;
        private System.Windows.Forms.GroupBox gbConstEditType;
        private System.Windows.Forms.RadioButton rbtnComboList;
        private System.Windows.Forms.RadioButton rbtnEditBox;
        private System.Windows.Forms.DataGridView dgvParameters;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColParaName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColParaValue;
    }
}