namespace Self_Inspection_III.Dialogs
{
    partial class DlgSystemSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgSystemSetting));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbbStation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbMES = new System.Windows.Forms.GroupBox();
            this.btnMESAll = new System.Windows.Forms.Button();
            this.cbHolidayLst = new System.Windows.Forms.CheckBox();
            this.cbEmployeeID = new System.Windows.Forms.CheckBox();
            this.gbEmail = new System.Windows.Forms.GroupBox();
            this.btnEmailAll = new System.Windows.Forms.Button();
            this.cbErrorEmail = new System.Windows.Forms.CheckBox();
            this.cbAlarmEmail = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.gbMES.SuspendLayout();
            this.gbEmail.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbbStation);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.gbMES);
            this.panel1.Controls.Add(this.gbEmail);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 220);
            this.panel1.TabIndex = 0;
            // 
            // cbbStation
            // 
            this.cbbStation.FormattingEnabled = true;
            this.cbbStation.Location = new System.Drawing.Point(75, 7);
            this.cbbStation.Name = "cbbStation";
            this.cbbStation.Size = new System.Drawing.Size(156, 21);
            this.cbbStation.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Station:";
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(156, 188);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(75, 188);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // gbMES
            // 
            this.gbMES.Controls.Add(this.btnMESAll);
            this.gbMES.Controls.Add(this.cbHolidayLst);
            this.gbMES.Controls.Add(this.cbEmployeeID);
            this.gbMES.Font = new System.Drawing.Font("Garamond", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbMES.Location = new System.Drawing.Point(12, 111);
            this.gbMES.Name = "gbMES";
            this.gbMES.Size = new System.Drawing.Size(282, 71);
            this.gbMES.TabIndex = 1;
            this.gbMES.TabStop = false;
            this.gbMES.Text = "MES";
            // 
            // btnMESAll
            // 
            this.btnMESAll.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMESAll.Location = new System.Drawing.Point(204, 12);
            this.btnMESAll.Name = "btnMESAll";
            this.btnMESAll.Size = new System.Drawing.Size(75, 23);
            this.btnMESAll.TabIndex = 3;
            this.btnMESAll.Text = "Select All";
            this.btnMESAll.UseVisualStyleBackColor = true;
            this.btnMESAll.Click += new System.EventHandler(this.BtnAll_Click);
            // 
            // cbHolidayList
            // 
            this.cbHolidayLst.AutoSize = true;
            this.cbHolidayLst.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbHolidayLst.Location = new System.Drawing.Point(18, 46);
            this.cbHolidayLst.Name = "cbHolidayList";
            this.cbHolidayLst.Size = new System.Drawing.Size(105, 17);
            this.cbHolidayLst.TabIndex = 1;
            this.cbHolidayLst.Text = "Get Holiday List";
            this.cbHolidayLst.UseVisualStyleBackColor = true;
            // 
            // cbEmployeeID
            // 
            this.cbEmployeeID.AutoSize = true;
            this.cbEmployeeID.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEmployeeID.Location = new System.Drawing.Point(18, 23);
            this.cbEmployeeID.Name = "cbEmployeeID";
            this.cbEmployeeID.Size = new System.Drawing.Size(121, 17);
            this.cbEmployeeID.TabIndex = 0;
            this.cbEmployeeID.Text = "Check Employee ID";
            this.cbEmployeeID.UseVisualStyleBackColor = true;
            // 
            // gbEmail
            // 
            this.gbEmail.Controls.Add(this.btnEmailAll);
            this.gbEmail.Controls.Add(this.cbErrorEmail);
            this.gbEmail.Controls.Add(this.cbAlarmEmail);
            this.gbEmail.Font = new System.Drawing.Font("Garamond", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbEmail.Location = new System.Drawing.Point(12, 34);
            this.gbEmail.Name = "gbEmail";
            this.gbEmail.Size = new System.Drawing.Size(282, 71);
            this.gbEmail.TabIndex = 0;
            this.gbEmail.TabStop = false;
            this.gbEmail.Text = "Email";
            // 
            // btnEmailAll
            // 
            this.btnEmailAll.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmailAll.Location = new System.Drawing.Point(204, 12);
            this.btnEmailAll.Name = "btnEmailAll";
            this.btnEmailAll.Size = new System.Drawing.Size(75, 23);
            this.btnEmailAll.TabIndex = 2;
            this.btnEmailAll.Text = "Select All";
            this.btnEmailAll.UseVisualStyleBackColor = true;
            this.btnEmailAll.Click += new System.EventHandler(this.BtnAll_Click);
            // 
            // cbErrorEmail
            // 
            this.cbErrorEmail.AutoSize = true;
            this.cbErrorEmail.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbErrorEmail.Location = new System.Drawing.Point(18, 46);
            this.cbErrorEmail.Name = "cbErrorEmail";
            this.cbErrorEmail.Size = new System.Drawing.Size(83, 17);
            this.cbErrorEmail.TabIndex = 1;
            this.cbErrorEmail.Text = "Error Email";
            this.cbErrorEmail.UseVisualStyleBackColor = true;
            // 
            // cbAlarmEmail
            // 
            this.cbAlarmEmail.AutoSize = true;
            this.cbAlarmEmail.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAlarmEmail.Location = new System.Drawing.Point(18, 23);
            this.cbAlarmEmail.Name = "cbAlarmEmail";
            this.cbAlarmEmail.Size = new System.Drawing.Size(86, 17);
            this.cbAlarmEmail.TabIndex = 0;
            this.cbAlarmEmail.Text = "Alarm Email";
            this.cbAlarmEmail.UseVisualStyleBackColor = true;
            // 
            // DlgSystemSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 220);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgSystemSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Setting";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbMES.ResumeLayout(false);
            this.gbMES.PerformLayout();
            this.gbEmail.ResumeLayout(false);
            this.gbEmail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbMES;
        private System.Windows.Forms.Button btnMESAll;
        private System.Windows.Forms.CheckBox cbHolidayLst;
        private System.Windows.Forms.CheckBox cbEmployeeID;
        private System.Windows.Forms.GroupBox gbEmail;
        private System.Windows.Forms.Button btnEmailAll;
        private System.Windows.Forms.CheckBox cbErrorEmail;
        private System.Windows.Forms.CheckBox cbAlarmEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbStation;
    }
}