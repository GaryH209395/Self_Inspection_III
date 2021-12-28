namespace Self_Inspection_III.DataSearch
{
    partial class UcDataSearch
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcDataSearch));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpResults = new System.Windows.Forms.TabPage();
            this.dgvRight = new System.Windows.Forms.DataGridView();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvLeft = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tscbbStation = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.tsbtnDatetimes = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.tscmbStatus = new System.Windows.Forms.ToolStripComboBox();
            this.tsbtnSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnImportFunction = new System.Windows.Forms.ToolStripButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dgvColItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColEmployeeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColTestTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColPF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRight)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLeft)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(990, 650);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.63008F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.36992F));
            this.tableLayoutPanel3.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 53);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 594F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(984, 594);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpResults);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Bold);
            this.tabControl1.Location = new System.Drawing.Point(697, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(284, 588);
            this.tabControl1.TabIndex = 5;
            // 
            // tpResults
            // 
            this.tpResults.Controls.Add(this.dgvRight);
            this.tpResults.Location = new System.Drawing.Point(4, 23);
            this.tpResults.Name = "tpResults";
            this.tpResults.Padding = new System.Windows.Forms.Padding(3);
            this.tpResults.Size = new System.Drawing.Size(276, 561);
            this.tpResults.TabIndex = 1;
            this.tpResults.Text = "Results";
            this.tpResults.UseVisualStyleBackColor = true;
            // 
            // dgvRight
            // 
            this.dgvRight.AllowUserToAddRows = false;
            this.dgvRight.AllowUserToDeleteRows = false;
            this.dgvRight.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRight.BackgroundColor = System.Drawing.Color.White;
            this.dgvRight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRight.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column3});
            this.dgvRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRight.Location = new System.Drawing.Point(3, 3);
            this.dgvRight.Name = "dgvRight";
            this.dgvRight.ReadOnly = true;
            this.dgvRight.RowHeadersVisible = false;
            this.dgvRight.RowTemplate.Height = 24;
            this.dgvRight.Size = new System.Drawing.Size(270, 555);
            this.dgvRight.TabIndex = 2;
            this.dgvRight.MouseHover += new System.EventHandler(this.DgvRight_MouseHover);
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Test Item";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Test Value";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.dgvLeft, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.67742F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(688, 588);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // dgvLeft
            // 
            this.dgvLeft.AllowUserToAddRows = false;
            this.dgvLeft.AllowUserToDeleteRows = false;
            this.dgvLeft.AllowUserToResizeRows = false;
            this.dgvLeft.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLeft.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLeft.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLeft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLeft.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColItemName,
            this.Column1,
            this.dgvColEmployeeID,
            this.dgvColTestTime,
            this.dgvColPF});
            this.dgvLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLeft.Location = new System.Drawing.Point(3, 3);
            this.dgvLeft.Name = "dgvLeft";
            this.dgvLeft.ReadOnly = true;
            this.dgvLeft.RowHeadersVisible = false;
            this.dgvLeft.RowTemplate.Height = 24;
            this.dgvLeft.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLeft.Size = new System.Drawing.Size(682, 582);
            this.dgvLeft.TabIndex = 1;
            this.dgvLeft.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvLeft_CellClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 44);
            this.panel1.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel4,
            this.toolStripLabel2,
            this.tscbbStation,
            this.toolStripLabel5,
            this.tsbtnDatetimes,
            this.toolStripLabel6,
            this.toolStripLabel3,
            this.tscmbStatus,
            this.tsbtnSearch,
            this.toolStripSeparator1,
            this.tsbtnImportFunction});
            this.toolStrip1.Location = new System.Drawing.Point(172, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(812, 44);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(10, 41);
            this.toolStripLabel4.Text = " ";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(68, 41);
            this.toolStripLabel2.Text = "Station : ";
            // 
            // tscbbStation
            // 
            this.tscbbStation.AutoSize = false;
            this.tscbbStation.BackColor = System.Drawing.SystemColors.Info;
            this.tscbbStation.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.tscbbStation.Name = "tscbbStation";
            this.tscbbStation.Size = new System.Drawing.Size(100, 23);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(10, 41);
            this.toolStripLabel5.Text = " ";
            // 
            // tsbtnDatetimes
            // 
            this.tsbtnDatetimes.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnDatetimes.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDatetimes.Image")));
            this.tsbtnDatetimes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDatetimes.Name = "tsbtnDatetimes";
            this.tsbtnDatetimes.Size = new System.Drawing.Size(235, 41);
            this.tsbtnDatetimes.Text = "0001/01/01 ~ 0001/01/01";
            this.tsbtnDatetimes.ToolTipText = "Select Time Zone";
            this.tsbtnDatetimes.Click += new System.EventHandler(this.TsbtnDatetimes_Click);
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(10, 41);
            this.toolStripLabel6.Text = " ";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(55, 41);
            this.toolStripLabel3.Text = "Filter : ";
            // 
            // tscmbStatus
            // 
            this.tscmbStatus.AutoSize = false;
            this.tscmbStatus.BackColor = System.Drawing.SystemColors.Info;
            this.tscmbStatus.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.tscmbStatus.Items.AddRange(new object[] {
            "All",
            "Pass",
            "Fail"});
            this.tscmbStatus.Name = "tscmbStatus";
            this.tscmbStatus.Size = new System.Drawing.Size(50, 23);
            // 
            // tsbtnSearch
            // 
            this.tsbtnSearch.BackColor = System.Drawing.Color.White;
            this.tsbtnSearch.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnSearch.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSearch.Image")));
            this.tsbtnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tsbtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSearch.MergeIndex = 1;
            this.tsbtnSearch.Name = "tsbtnSearch";
            this.tsbtnSearch.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tsbtnSearch.Size = new System.Drawing.Size(98, 41);
            this.tsbtnSearch.Text = "Query";
            this.tsbtnSearch.Click += new System.EventHandler(this.TsbtnSearch_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 44);
            // 
            // tsbtnImportFunction
            // 
            this.tsbtnImportFunction.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbtnImportFunction.BackColor = System.Drawing.Color.White;
            this.tsbtnImportFunction.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnImportFunction.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnImportFunction.Image")));
            this.tsbtnImportFunction.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tsbtnImportFunction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnImportFunction.MergeIndex = 1;
            this.tsbtnImportFunction.Name = "tsbtnImportFunction";
            this.tsbtnImportFunction.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tsbtnImportFunction.Size = new System.Drawing.Size(96, 41);
            this.tsbtnImportFunction.Text = "Export";
            this.tsbtnImportFunction.ToolTipText = "Export Excel";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(172, 44);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // dgvColItemName
            // 
            this.dgvColItemName.FillWeight = 125F;
            this.dgvColItemName.HeaderText = "Item Name";
            this.dgvColItemName.Name = "dgvColItemName";
            this.dgvColItemName.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 75F;
            this.Column1.HeaderText = "UUT";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // dgvColEmployeeID
            // 
            this.dgvColEmployeeID.FillWeight = 75F;
            this.dgvColEmployeeID.HeaderText = "Employee ID";
            this.dgvColEmployeeID.Name = "dgvColEmployeeID";
            this.dgvColEmployeeID.ReadOnly = true;
            // 
            // dgvColTestTime
            // 
            this.dgvColTestTime.HeaderText = "Test Time";
            this.dgvColTestTime.Name = "dgvColTestTime";
            this.dgvColTestTime.ReadOnly = true;
            // 
            // dgvColPF
            // 
            this.dgvColPF.FillWeight = 50F;
            this.dgvColPF.HeaderText = "P/F";
            this.dgvColPF.Name = "dgvColPF";
            this.dgvColPF.ReadOnly = true;
            // 
            // UcDataSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UcDataSearch";
            this.Size = new System.Drawing.Size(990, 650);
            this.Load += new System.EventHandler(this.UcDataSearch_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRight)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLeft)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox tscbbStation;
        private System.Windows.Forms.ToolStripButton tsbtnDatetimes;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox tscmbStatus;
        private System.Windows.Forms.ToolStripButton tsbtnSearch;
        private System.Windows.Forms.ToolStripButton tsbtnImportFunction;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpResults;
        private System.Windows.Forms.DataGridView dgvRight;
        private System.Windows.Forms.DataGridView dgvLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColEmployeeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColTestTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColPF;
    }
}
