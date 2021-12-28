using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Self_Inspection_III.SI
{
    public partial class DlgCondiEnumEditor : Form
    {
        private string _EnumString;
        public DlgCondiEnumEditor(object enumString)
        {
            _EnumString = enumString == null ? string.Empty : enumString.ToString();
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            EnumItemSetting();
        }

        public string EnumString
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append($"{dgvEnumItem[0, 0].Value}={dgvEnumItem[1, 0].Value}");
                for (int i = 1; i < dgvEnumItem.RowCount; i++)
                    sb.Append($",{dgvEnumItem[0, i].Value}={dgvEnumItem[1, i].Value}");

                return sb.ToString();
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            dgvEnumItem.Rows.Add();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvEnumItem.SelectedRows)
            {
                dgvEnumItem.Rows.Remove(row);
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEnumItem.RowCount == 0) return;
                if (dgvEnumItem.SelectedRows.Count == 0) return;

                DataGridViewRow dr = dgvEnumItem.SelectedRows[0];
                int index = dr.Index;

                if (index > 0)
                {
                    dgvEnumItem.Rows.RemoveAt(index);
                    dgvEnumItem.Rows.Insert(index - 1, dr);

                    dgvEnumItem.Rows[index].Selected = false;
                    dgvEnumItem.Rows[index - 1].Selected = true;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEnumItem.RowCount == 0) return;
                if (dgvEnumItem.SelectedRows.Count == 0) return;

                DataGridViewRow dr = dgvEnumItem.SelectedRows[0];
                int index = dr.Index;

                if (index < dgvEnumItem.RowCount - 1)
                {
                    dgvEnumItem.Rows.RemoveAt(index);
                    dgvEnumItem.Rows[index].Selected = false;

                    dgvEnumItem.Rows.Insert(index + 1, dr);
                    dgvEnumItem.Rows[index + 1].Selected = true;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvEnumItem.RowCount; i++)
            {
                if (string.IsNullOrEmpty(dgvEnumItem[0, i].Value.ToString()))
                {
                    MessageBox.Show("Item Name Empty!", "Error");
                    return;
                }
                if (string.IsNullOrEmpty(dgvEnumItem[1, i].Value.ToString()))
                {
                    MessageBox.Show("Item Value Empty!", "Error");
                    return;
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void EnumItemSetting()
        {
            if (string.IsNullOrEmpty(_EnumString)) return;
            try
            {
                foreach (string str in _EnumString.Split(','))
                {
                    dgvEnumItem.Rows.Add(new object[] { str.Split('=')[0], str.Split('=')[1] });
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
    }
}
