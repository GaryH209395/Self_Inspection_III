using System;
using System.Windows.Forms;

namespace Self_Inspection_III.SI
{
    public partial class DlgSetArrayLength : Form
    {
        public DlgSetArrayLength()
        {
            InitializeComponent();
        }

        public int ArraySize { get { try {return Convert.ToInt32(textBox1.Text); } catch (Exception ex) { MessageBox.Show(ex.Message); return 0; } } }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (ArraySize > 0)
                DialogResult = DialogResult.OK;
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                button1.PerformClick();
        }
    }
}
