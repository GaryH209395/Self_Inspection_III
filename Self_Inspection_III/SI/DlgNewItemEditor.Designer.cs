namespace Self_Inspection_III.SI
{
    partial class DlgNewItemEditor
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblStation = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbbPeriod = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnMeterList = new System.Windows.Forms.Button();
            this.cbbMeterList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSourceList = new System.Windows.Forms.Button();
            this.cbbSourceList = new System.Windows.Forms.ComboBox();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.lblStation);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cbbPeriod);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnMeterList);
            this.panel1.Controls.Add(this.cbbMeterList);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnSourceList);
            this.panel1.Controls.Add(this.cbbSourceList);
            this.panel1.Controls.Add(this.txtItemName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(355, 205);
            this.panel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(190, 165);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(81, 165);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // lblStation
            // 
            this.lblStation.AutoSize = true;
            this.lblStation.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblStation.Location = new System.Drawing.Point(88, 20);
            this.lblStation.Name = "lblStation";
            this.lblStation.Size = new System.Drawing.Size(79, 12);
            this.lblStation.TabIndex = 11;
            this.lblStation.Text = "Station Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(34, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "Station:";
            // 
            // cbbPeriod
            // 
            this.cbbPeriod.FormattingEnabled = true;
            this.cbbPeriod.Location = new System.Drawing.Point(81, 127);
            this.cbbPeriod.Name = "cbbPeriod";
            this.cbbPeriod.Size = new System.Drawing.Size(184, 20);
            this.cbbPeriod.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(33, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "Period:";
            // 
            // btnMeterList
            // 
            this.btnMeterList.Location = new System.Drawing.Point(270, 99);
            this.btnMeterList.Name = "btnMeterList";
            this.btnMeterList.Size = new System.Drawing.Size(24, 23);
            this.btnMeterList.TabIndex = 7;
            this.btnMeterList.Text = "...";
            this.btnMeterList.UseVisualStyleBackColor = true;
            this.btnMeterList.Click += new System.EventHandler(this.BtnMeterList_Click);
            // 
            // cbbMeterList
            // 
            this.cbbMeterList.FormattingEnabled = true;
            this.cbbMeterList.Location = new System.Drawing.Point(81, 101);
            this.cbbMeterList.Name = "cbbMeterList";
            this.cbbMeterList.Size = new System.Drawing.Size(184, 20);
            this.cbbMeterList.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(38, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Meter:";
            // 
            // btnSourceList
            // 
            this.btnSourceList.Location = new System.Drawing.Point(270, 72);
            this.btnSourceList.Name = "btnSourceList";
            this.btnSourceList.Size = new System.Drawing.Size(24, 23);
            this.btnSourceList.TabIndex = 4;
            this.btnSourceList.Text = "...";
            this.btnSourceList.UseVisualStyleBackColor = true;
            this.btnSourceList.Click += new System.EventHandler(this.BtnSourceList_Click);
            // 
            // cbbSourceList
            // 
            this.cbbSourceList.FormattingEnabled = true;
            this.cbbSourceList.Location = new System.Drawing.Point(81, 74);
            this.cbbSourceList.Name = "cbbSourceList";
            this.cbbSourceList.Size = new System.Drawing.Size(184, 20);
            this.cbbSourceList.TabIndex = 3;
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(81, 44);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(264, 21);
            this.txtItemName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(33, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Source: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(11, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item Name:";
            // 
            // DlgNewItemEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 205);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DlgNewItemEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Item";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblStation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbbPeriod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnMeterList;
        private System.Windows.Forms.ComboBox cbbMeterList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSourceList;
        private System.Windows.Forms.ComboBox cbbSourceList;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}