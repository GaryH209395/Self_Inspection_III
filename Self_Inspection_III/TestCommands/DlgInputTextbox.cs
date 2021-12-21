using System;
using System.Windows.Forms;

namespace Self_Inspection_III.TestCommands
{
    public partial class DlgInputTextbox : Form
    {
        public DlgInputTextbox(string message)
        {
            InitializeComponent();
            label2.Visible = false;
            label1.Text = message;
        }
        public decimal Value { get; set; }
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                label2.Text = "*** Please key in value! ***";
                label2.Visible = true;
            }
            else
            {
                try
                {
                    Value = Convert.ToDecimal(textBox1.Text);
                    DialogResult = DialogResult.OK;
                }
                catch
                {
                    label2.Text = "*** Value not number! ***";
                    label2.Visible = true;
                }
            }
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                btnOK.PerformClick();
        }
    }
}
