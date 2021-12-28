using Self_Inspection_III.Class;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Self_Inspection_III.SI
{
    public partial class DlgDevcieList : Form
    {
        List<Device> devices;
        List<string> modelList;

        public DlgDevcieList(List<Device> devices, List<string> modelList)
        {
            isLoad = true;

            InitializeComponent();
            this.devices = devices;
            this.modelList = modelList;

            isLoad = false;
        }
      private  bool isLoad = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            isLoad = true;

            SetDeviceList();

            isLoad = false;
        }
        private void SetDeviceList()
        {
            for (int i = 0; i < modelList.Count; i++)
                dgvDeviceList.Rows.Add(new object[] { modelList[i] });
        }

        public object[] ObjectArray
        {
            get
            {
                List<object> comboList = new List<object>();
                for (int i = 0; i < dgvDeviceList.RowCount; i++)
                    comboList.Add(dgvDeviceList[0, i].Value.ToString());
                return comboList.ToArray();
            }
        }
        public List<string> ModelList
        {
            get
            {
                List<string> list = new List<string>();
                for (int i = 0; i < dgvDeviceList.RowCount; i++)
                    list.Add(dgvDeviceList[0, i].Value.ToString());
                return list;
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            dgvDeviceList.Rows.Add();
        }

        private bool isDelete = false;
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                isDelete = true;
                dgvDeviceList.BeginEdit(true);

                foreach (DataGridViewRow row in dgvDeviceList.SelectedRows)
                    dgvDeviceList.Rows.Remove(row);

                dgvDeviceList.EndEdit();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            finally { isDelete = false; }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvDeviceList.RowCount; i++)
                for (int j = 0; j < dgvDeviceList.ColumnCount; j++)
                    if (dgvDeviceList[j, i].Value == null || string.IsNullOrEmpty(dgvDeviceList[j, i].Value.ToString()))
                    {
                        dgvDeviceList.Rows.RemoveAt(i);
                        break;
                    }

            DialogResult = DialogResult.OK;
        }

        private bool isDoubleClick = false;
        private void DgvDeviceList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            try
            {
                isDoubleClick = true;

                dgvDeviceList.ReadOnly = false;
                //dgvDeviceList.BeginEdit(true);

                DataGridViewComboBoxCell dgvCbbCell = new DataGridViewComboBoxCell();
                foreach (Device device in devices)
                    dgvCbbCell.Items.Add($"{device.ModelName}-{device.No}");

                dgvDeviceList[e.ColumnIndex, e.RowIndex] = dgvCbbCell;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            finally { isDoubleClick = false; }
        }

        private void DgvDeviceList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isLoad || isDoubleClick || isDelete) return;
            object modelName = dgvDeviceList[e.ColumnIndex, e.RowIndex].Value;

            DataGridViewTextBoxCell dgvTbCell = new DataGridViewTextBoxCell { Value = modelName };
            dgvDeviceList[e.ColumnIndex, e.RowIndex] = dgvTbCell;

            dgvDeviceList.EndEdit();
            dgvDeviceList.ReadOnly = false;
        }
    }
}
