using Self_Inspection_III.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Self_Inspection_III.Setting
{
    public partial class DlgAddDevice : Form
    {
        public DlgAddDevice(string deviceType)
        {
            InitializeComponent();

            foreach (object type in DeviceType.Types)
                cbbDeviceType.Items.Add(type);

            cbbDeviceType.Text = deviceType;
        }

        public Model NewModel { get => new Model(txtModelName.Text, cbbDeviceType.Text); }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbbDeviceType.Text))
            {
                MessageBox.Show("Device Type cannot be Empty!", "Error");
            }
            else
            if (string.IsNullOrEmpty(txtModelName.Text))
            {
                MessageBox.Show("Model Name cannot be Empty!", "Error");
            }
            else DialogResult = DialogResult.OK;
        }

        private void TxtModelName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                BtnOK_Click(btnOK, null);
        }
    }
}
