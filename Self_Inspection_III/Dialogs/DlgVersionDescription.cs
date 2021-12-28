using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Self_Inspection_III.Dialogs
{
    public partial class DlgVersionDescription : Form
    {
        public DlgVersionDescription()
        {
            InitializeComponent();
            label1.Text = DateTime.Now.Date.ToString("yyyy / MM / dd");
        }

        public string VersionInfo { get => textBox1.Text; }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                MessageBox.Show("資訊欄為空!");
            else
                DialogResult = DialogResult.OK;
        }
    }
}
