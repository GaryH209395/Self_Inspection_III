namespace Self_Inspection_III
{
    partial class Form1
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

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSystemSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiModeSwtiching = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDescription = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpSIProgram = new System.Windows.Forms.TabPage();
            this.tpSIItem = new System.Windows.Forms.TabPage();
            this.tpDataSearch = new System.Windows.Forms.TabPage();
            this.tpManagement = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFunction,
            this.tsmiDescription});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1009, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFunction
            // 
            this.tsmiFunction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSystemSetting,
            this.toolStripSeparator1,
            this.tsmiModeSwtiching});
            this.tsmiFunction.Name = "tsmiFunction";
            this.tsmiFunction.Size = new System.Drawing.Size(43, 20);
            this.tsmiFunction.Text = "功能";
            // 
            // tsmiSystemSetting
            // 
            this.tsmiSystemSetting.Name = "tsmiSystemSetting";
            this.tsmiSystemSetting.Size = new System.Drawing.Size(122, 22);
            this.tsmiSystemSetting.Text = "系統設定";
            this.tsmiSystemSetting.Click += new System.EventHandler(this.TsmiSystemSetting_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
            // 
            // tsmiModeSwtiching
            // 
            this.tsmiModeSwtiching.Name = "tsmiModeSwtiching";
            this.tsmiModeSwtiching.Size = new System.Drawing.Size(122, 22);
            this.tsmiModeSwtiching.Text = "模式切換";
            this.tsmiModeSwtiching.Click += new System.EventHandler(this.TsmiModeSwtiching_Click);
            // 
            // tsmiDescription
            // 
            this.tsmiDescription.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAbout});
            this.tsmiDescription.Name = "tsmiDescription";
            this.tsmiDescription.Size = new System.Drawing.Size(43, 20);
            this.tsmiDescription.Text = "說明";
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(98, 22);
            this.tsmiAbout.Text = "關於";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpSIProgram);
            this.tabControl1.Controls.Add(this.tpSIItem);
            this.tabControl1.Controls.Add(this.tpDataSearch);
            this.tabControl1.Controls.Add(this.tpManagement);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Bold);
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1009, 687);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tpSIProgram
            // 
            this.tpSIProgram.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Bold);
            this.tpSIProgram.Location = new System.Drawing.Point(4, 23);
            this.tpSIProgram.Name = "tpSIProgram";
            this.tpSIProgram.Padding = new System.Windows.Forms.Padding(3);
            this.tpSIProgram.Size = new System.Drawing.Size(1001, 660);
            this.tpSIProgram.TabIndex = 0;
            this.tpSIProgram.Text = "SI-Program";
            this.tpSIProgram.UseVisualStyleBackColor = true;
            // 
            // tpSIItem
            // 
            this.tpSIItem.Location = new System.Drawing.Point(4, 23);
            this.tpSIItem.Name = "tpSIItem";
            this.tpSIItem.Padding = new System.Windows.Forms.Padding(3);
            this.tpSIItem.Size = new System.Drawing.Size(1001, 660);
            this.tpSIItem.TabIndex = 1;
            this.tpSIItem.Text = "SI-Item";
            this.tpSIItem.UseVisualStyleBackColor = true;
            // 
            // tpDataSearch
            // 
            this.tpDataSearch.Location = new System.Drawing.Point(4, 23);
            this.tpDataSearch.Name = "tpDataSearch";
            this.tpDataSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tpDataSearch.Size = new System.Drawing.Size(1001, 660);
            this.tpDataSearch.TabIndex = 2;
            this.tpDataSearch.Text = "Data Search";
            this.tpDataSearch.UseVisualStyleBackColor = true;
            // 
            // tpManagement
            // 
            this.tpManagement.Location = new System.Drawing.Point(4, 23);
            this.tpManagement.Name = "tpManagement";
            this.tpManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tpManagement.Size = new System.Drawing.Size(1001, 660);
            this.tpManagement.TabIndex = 3;
            this.tpManagement.Text = "Management";
            this.tpManagement.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 711);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Self Inspection System";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFunction;
        private System.Windows.Forms.ToolStripMenuItem tsmiSystemSetting;
        private System.Windows.Forms.ToolStripMenuItem tsmiModeSwtiching;
        private System.Windows.Forms.ToolStripMenuItem tsmiDescription;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpSIProgram;
        private System.Windows.Forms.TabPage tpSIItem;
        private System.Windows.Forms.TabPage tpDataSearch;
        private System.Windows.Forms.TabPage tpManagement;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

