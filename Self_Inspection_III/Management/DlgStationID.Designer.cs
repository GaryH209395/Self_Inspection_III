namespace Self_Inspection_III.Management
{
    partial class DlgStationID
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
            this.dgvIDList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIDList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvIDList
            // 
            this.dgvIDList.AllowUserToAddRows = false;
            this.dgvIDList.AllowUserToDeleteRows = false;
            this.dgvIDList.AllowUserToResizeColumns = false;
            this.dgvIDList.AllowUserToResizeRows = false;
            this.dgvIDList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIDList.BackgroundColor = System.Drawing.Color.White;
            this.dgvIDList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIDList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgvIDList.Location = new System.Drawing.Point(12, 12);
            this.dgvIDList.Name = "dgvIDList";
            this.dgvIDList.RowHeadersVisible = false;
            this.dgvIDList.RowTemplate.Height = 24;
            this.dgvIDList.Size = new System.Drawing.Size(233, 392);
            this.dgvIDList.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 40F;
            this.Column1.HeaderText = "ID";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Station Name";
            this.Column2.Name = "Column2";
            // 
            // DlgStationID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 419);
            this.Controls.Add(this.dgvIDList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgStationID";
            this.ShowIcon = false;
            this.Text = "Station ID";
            ((System.ComponentModel.ISupportInitialize)(this.dgvIDList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvIDList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}