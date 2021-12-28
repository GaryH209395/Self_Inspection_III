namespace Self_Inspection_III.SI
{
    partial class DlgVariables
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgVariables));
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbCtrlVariables = new System.Windows.Forms.TabControl();
            this.tpCondition = new System.Windows.Forms.TabPage();
            this.dgvCondition = new System.Windows.Forms.DataGridView();
            this.dgvColConShowName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColConCallName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColConDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColConEditType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColConDefault = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColConEnum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpTemporary = new System.Windows.Forms.TabPage();
            this.dgvTemporary = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColTempDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpResult = new System.Windows.Forms.TabPage();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColResDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColResSpecMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColResSpecMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.tsbtnNew = new System.Windows.Forms.ToolStripLabel();
            this.tsbtnSave = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnCut = new System.Windows.Forms.ToolStripLabel();
            this.tsbtnCopy = new System.Windows.Forms.ToolStripLabel();
            this.tsbtnPaste = new System.Windows.Forms.ToolStripLabel();
            this.tsbtnDelete = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnUp = new System.Windows.Forms.ToolStripLabel();
            this.tsbtnDown = new System.Windows.Forms.ToolStripLabel();
            this.panel2.SuspendLayout();
            this.tbCtrlVariables.SuspendLayout();
            this.tpCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCondition)).BeginInit();
            this.tpTemporary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTemporary)).BeginInit();
            this.tpResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel2.Controls.Add(this.tbCtrlVariables);
            this.panel2.Controls.Add(this.toolStrip3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(853, 468);
            this.panel2.TabIndex = 1;
            // 
            // tbCtrlVariables
            // 
            this.tbCtrlVariables.Controls.Add(this.tpCondition);
            this.tbCtrlVariables.Controls.Add(this.tpTemporary);
            this.tbCtrlVariables.Controls.Add(this.tpResult);
            this.tbCtrlVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCtrlVariables.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbCtrlVariables.Location = new System.Drawing.Point(0, 33);
            this.tbCtrlVariables.Name = "tbCtrlVariables";
            this.tbCtrlVariables.SelectedIndex = 0;
            this.tbCtrlVariables.Size = new System.Drawing.Size(853, 435);
            this.tbCtrlVariables.TabIndex = 5;
            this.tbCtrlVariables.SelectedIndexChanged += new System.EventHandler(this.TbCtrl_SelectIndexChanged);
            // 
            // tpCondition
            // 
            this.tpCondition.Controls.Add(this.dgvCondition);
            this.tpCondition.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tpCondition.Location = new System.Drawing.Point(4, 22);
            this.tpCondition.Name = "tpCondition";
            this.tpCondition.Padding = new System.Windows.Forms.Padding(3);
            this.tpCondition.Size = new System.Drawing.Size(845, 409);
            this.tpCondition.TabIndex = 0;
            this.tpCondition.Text = "Condition";
            this.tpCondition.UseVisualStyleBackColor = true;
            // 
            // dgvCondition
            // 
            this.dgvCondition.AllowUserToAddRows = false;
            this.dgvCondition.AllowUserToDeleteRows = false;
            this.dgvCondition.AllowUserToOrderColumns = true;
            this.dgvCondition.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCondition.BackgroundColor = System.Drawing.Color.White;
            this.dgvCondition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCondition.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColConShowName,
            this.dgvColConCallName,
            this.dgvColConDataType,
            this.dgvColConEditType,
            this.dgvColConDefault,
            this.dgvColConEnum});
            this.dgvCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCondition.Location = new System.Drawing.Point(3, 3);
            this.dgvCondition.Name = "dgvCondition";
            this.dgvCondition.ReadOnly = true;
            this.dgvCondition.RowHeadersWidth = 30;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCondition.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCondition.RowTemplate.Height = 24;
            this.dgvCondition.ShowCellErrors = false;
            this.dgvCondition.ShowRowErrors = false;
            this.dgvCondition.Size = new System.Drawing.Size(839, 403);
            this.dgvCondition.TabIndex = 2;
            this.dgvCondition.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvCondition_CellDoubleClick);
            this.dgvCondition.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvCondition_CellValueChanged);
            // 
            // dgvColConShowName
            // 
            this.dgvColConShowName.HeaderText = "Show Name";
            this.dgvColConShowName.Name = "dgvColConShowName";
            this.dgvColConShowName.ReadOnly = true;
            // 
            // dgvColConCallName
            // 
            this.dgvColConCallName.HeaderText = "Call Name";
            this.dgvColConCallName.Name = "dgvColConCallName";
            this.dgvColConCallName.ReadOnly = true;
            // 
            // dgvColConDataType
            // 
            this.dgvColConDataType.FillWeight = 75F;
            this.dgvColConDataType.HeaderText = "Data Type";
            this.dgvColConDataType.Name = "dgvColConDataType";
            this.dgvColConDataType.ReadOnly = true;
            this.dgvColConDataType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvColConDataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvColConEditType
            // 
            this.dgvColConEditType.FillWeight = 75F;
            this.dgvColConEditType.HeaderText = "Edit Type";
            this.dgvColConEditType.Name = "dgvColConEditType";
            this.dgvColConEditType.ReadOnly = true;
            this.dgvColConEditType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvColConEditType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvColConDefault
            // 
            this.dgvColConDefault.HeaderText = "Default";
            this.dgvColConDefault.Name = "dgvColConDefault";
            this.dgvColConDefault.ReadOnly = true;
            // 
            // dgvColConEnum
            // 
            this.dgvColConEnum.HeaderText = "Enumerative Items";
            this.dgvColConEnum.Name = "dgvColConEnum";
            this.dgvColConEnum.ReadOnly = true;
            // 
            // tpTemporary
            // 
            this.tpTemporary.Controls.Add(this.dgvTemporary);
            this.tpTemporary.Location = new System.Drawing.Point(4, 22);
            this.tpTemporary.Name = "tpTemporary";
            this.tpTemporary.Padding = new System.Windows.Forms.Padding(3);
            this.tpTemporary.Size = new System.Drawing.Size(845, 409);
            this.tpTemporary.TabIndex = 1;
            this.tpTemporary.Text = "Temporary";
            this.tpTemporary.UseVisualStyleBackColor = true;
            // 
            // dgvTemporary
            // 
            this.dgvTemporary.AllowUserToAddRows = false;
            this.dgvTemporary.AllowUserToDeleteRows = false;
            this.dgvTemporary.AllowUserToOrderColumns = true;
            this.dgvTemporary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTemporary.BackgroundColor = System.Drawing.Color.White;
            this.dgvTemporary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTemporary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dgvColTempDataType});
            this.dgvTemporary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTemporary.Location = new System.Drawing.Point(3, 3);
            this.dgvTemporary.Name = "dgvTemporary";
            this.dgvTemporary.ReadOnly = true;
            this.dgvTemporary.RowHeadersWidth = 30;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTemporary.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTemporary.RowTemplate.Height = 24;
            this.dgvTemporary.ShowCellErrors = false;
            this.dgvTemporary.ShowRowErrors = false;
            this.dgvTemporary.Size = new System.Drawing.Size(839, 403);
            this.dgvTemporary.TabIndex = 2;
            this.dgvTemporary.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvTemp_CellDoubleClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Show Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Call Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dgvColTempDataType
            // 
            this.dgvColTempDataType.FillWeight = 75F;
            this.dgvColTempDataType.HeaderText = "Data Type";
            this.dgvColTempDataType.Name = "dgvColTempDataType";
            this.dgvColTempDataType.ReadOnly = true;
            this.dgvColTempDataType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvColTempDataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tpResult
            // 
            this.tpResult.Controls.Add(this.dgvResult);
            this.tpResult.Location = new System.Drawing.Point(4, 22);
            this.tpResult.Name = "tpResult";
            this.tpResult.Padding = new System.Windows.Forms.Padding(3);
            this.tpResult.Size = new System.Drawing.Size(845, 409);
            this.tpResult.TabIndex = 2;
            this.tpResult.Text = "Result";
            this.tpResult.UseVisualStyleBackColor = true;
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToOrderColumns = true;
            this.dgvResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResult.BackgroundColor = System.Drawing.Color.White;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dgvColResDataType,
            this.dgvColResSpecMin,
            this.dgvColResSpecMax});
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.Location = new System.Drawing.Point(3, 3);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersWidth = 30;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvResult.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvResult.RowTemplate.Height = 24;
            this.dgvResult.ShowCellErrors = false;
            this.dgvResult.ShowRowErrors = false;
            this.dgvResult.Size = new System.Drawing.Size(839, 403);
            this.dgvResult.TabIndex = 2;
            this.dgvResult.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvResult_CellClick);
            this.dgvResult.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvResult_CellDoubleClick);
            this.dgvResult.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DgvResult_MouseClick);
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Show Name";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Call Name";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dgvColResDataType
            // 
            this.dgvColResDataType.FillWeight = 75F;
            this.dgvColResDataType.HeaderText = "Data Type";
            this.dgvColResDataType.Name = "dgvColResDataType";
            this.dgvColResDataType.ReadOnly = true;
            this.dgvColResDataType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvColResDataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvColResSpecMin
            // 
            this.dgvColResSpecMin.HeaderText = "Spec. Min";
            this.dgvColResSpecMin.Name = "dgvColResSpecMin";
            this.dgvColResSpecMin.ReadOnly = true;
            this.dgvColResSpecMin.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dgvColResSpecMax
            // 
            this.dgvColResSpecMax.HeaderText = "Spec. Max";
            this.dgvColResSpecMax.Name = "dgvColResSpecMax";
            this.dgvColResSpecMax.ReadOnly = true;
            this.dgvColResSpecMax.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // toolStrip3
            // 
            this.toolStrip3.BackColor = System.Drawing.Color.White;
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnNew,
            this.tsbtnSave,
            this.toolStripSeparator2,
            this.tsbtnCut,
            this.tsbtnCopy,
            this.tsbtnPaste,
            this.tsbtnDelete,
            this.toolStripSeparator1,
            this.tsbtnUp,
            this.tsbtnDown});
            this.toolStrip3.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip3.Size = new System.Drawing.Size(853, 33);
            this.toolStrip3.TabIndex = 4;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // tsbtnNew
            // 
            this.tsbtnNew.AutoSize = false;
            this.tsbtnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnNew.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnNew.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnNew.Image")));
            this.tsbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnNew.Name = "tsbtnNew";
            this.tsbtnNew.Size = new System.Drawing.Size(30, 30);
            this.tsbtnNew.Text = "Cut";
            this.tsbtnNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbtnNew.Click += new System.EventHandler(this.TsbtnNew_Click);
            // 
            // tsbtnSave
            // 
            this.tsbtnSave.AutoSize = false;
            this.tsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSave.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSave.Image")));
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(30, 30);
            this.tsbtnSave.Text = "Cut";
            this.tsbtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbtnSave.Click += new System.EventHandler(this.TsbtnSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // tsbtnCut
            // 
            this.tsbtnCut.AutoSize = false;
            this.tsbtnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnCut.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnCut.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCut.Image")));
            this.tsbtnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCut.Name = "tsbtnCut";
            this.tsbtnCut.Size = new System.Drawing.Size(30, 30);
            this.tsbtnCut.Text = "Cut";
            this.tsbtnCut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbtnCut.Click += new System.EventHandler(this.TsbtnCut_Click);
            // 
            // tsbtnCopy
            // 
            this.tsbtnCopy.AutoSize = false;
            this.tsbtnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnCopy.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCopy.Image")));
            this.tsbtnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCopy.Name = "tsbtnCopy";
            this.tsbtnCopy.Size = new System.Drawing.Size(30, 30);
            this.tsbtnCopy.Text = "Copy";
            this.tsbtnCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbtnCopy.Click += new System.EventHandler(this.TsbtnCopy_Click);
            // 
            // tsbtnPaste
            // 
            this.tsbtnPaste.AutoSize = false;
            this.tsbtnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnPaste.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnPaste.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnPaste.Image")));
            this.tsbtnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnPaste.Name = "tsbtnPaste";
            this.tsbtnPaste.Size = new System.Drawing.Size(30, 30);
            this.tsbtnPaste.Text = "Paste";
            this.tsbtnPaste.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbtnPaste.Click += new System.EventHandler(this.TsbtnPaste_Click);
            // 
            // tsbtnDelete
            // 
            this.tsbtnDelete.AutoSize = false;
            this.tsbtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnDelete.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDelete.Image")));
            this.tsbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDelete.Name = "tsbtnDelete";
            this.tsbtnDelete.Size = new System.Drawing.Size(30, 30);
            this.tsbtnDelete.Text = "Delete";
            this.tsbtnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbtnDelete.Click += new System.EventHandler(this.TsbtnDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // tsbtnUp
            // 
            this.tsbtnUp.AutoSize = false;
            this.tsbtnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnUp.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnUp.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnUp.Image")));
            this.tsbtnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnUp.Name = "tsbtnUp";
            this.tsbtnUp.Size = new System.Drawing.Size(30, 30);
            this.tsbtnUp.Text = "Move up";
            this.tsbtnUp.Click += new System.EventHandler(this.TsbtnUp_Click);
            // 
            // tsbtnDown
            // 
            this.tsbtnDown.AutoSize = false;
            this.tsbtnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnDown.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtnDown.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDown.Image")));
            this.tsbtnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDown.Name = "tsbtnDown";
            this.tsbtnDown.Size = new System.Drawing.Size(30, 30);
            this.tsbtnDown.Text = "Move down";
            this.tsbtnDown.Click += new System.EventHandler(this.TsbtnDown_Click);
            // 
            // DlgVariables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 468);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgVariables";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Variables";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tbCtrlVariables.ResumeLayout(false);
            this.tpCondition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCondition)).EndInit();
            this.tpTemporary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTemporary)).EndInit();
            this.tpResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripLabel tsbtnCut;
        private System.Windows.Forms.ToolStripLabel tsbtnCopy;
        private System.Windows.Forms.ToolStripLabel tsbtnPaste;
        private System.Windows.Forms.ToolStripLabel tsbtnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel tsbtnUp;
        private System.Windows.Forms.ToolStripLabel tsbtnDown;
        private System.Windows.Forms.ToolStripLabel tsbtnNew;
        private System.Windows.Forms.ToolStripLabel tsbtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TabControl tbCtrlVariables;
        private System.Windows.Forms.TabPage tpCondition;
        private System.Windows.Forms.DataGridView dgvCondition;
        private System.Windows.Forms.TabPage tpTemporary;
        private System.Windows.Forms.DataGridView dgvTemporary;
        private System.Windows.Forms.TabPage tpResult;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColConShowName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColConCallName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColConDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColConEditType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColConDefault;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColConEnum;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColTempDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColResDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColResSpecMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColResSpecMax;
    }
}