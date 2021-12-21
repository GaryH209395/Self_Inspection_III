using Self_Inspection_III.Class;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Self_Inspection_III.Setting
{
    public partial class DlgAddStationDevice : Form
    {
        bool isLoading = false;

        DeviceDB DeviceDB = new DeviceDB();
        SettingDB SettingDB = new SettingDB();

        private List<Interface> m_InterfaceList;
        public List<Interface> InterfaceList {
            get
            {
                if (m_InterfaceList == null)
                    m_InterfaceList = XmlUtil.Deserialize(typeof(List<Interface>), SettingDB.GetSettingList(SettingDB.List.Interface)) as List<Interface>;
                return m_InterfaceList;
            }
            set => m_InterfaceList = value;
        }

        public DlgAddStationDevice(string StationName)
        {
            isLoading = true;

            InitializeComponent();
            SetComboList();

            lblStation.Text = StationName;

            isLoading = false;
        }

        private void SetComboList()
        {
            cbbDeviceType.Items.AddRange(DeviceType.Types);
            cbbInterface.Items.Clear();
            foreach (Interface intf in InterfaceList)
                cbbInterface.Items.Add(intf.Name);
        }

        public Device NewDevice
        {
            get => new Device(txtShowName.Text, cbbModelName.Text, lblStation.Text, nudNumber.Value, cbbDeviceType.Text, cbbInterface.Text, txtAddress.Text);
        }

        public void SetDeviceDetail(Device device)
        {
            txtShowName.Text = device.ShowName;
            cbbDeviceType.Text = device.Type;
            cbbModelName.Text = device.ModelName;
            nudNumber.Value = Convert.ToInt32(device.No);
            cbbInterface.Text = device.Interface;
            txtAddress.Text = device.Address;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtShowName.Text))
            {
                MsgShow("Show Name");
            }
            else
            if (string.IsNullOrEmpty(cbbDeviceType.Text))
            {
                MsgShow("Device Type");
            }
            else
            if (string.IsNullOrEmpty(cbbModelName.Text))
            {
                MsgShow("Model Name");
            }
            else
            if (string.IsNullOrEmpty(cbbInterface.Text))
            {
                MsgShow("Interface Type");
            }
            else
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MsgShow("Interface Address");
            }
            else DialogResult = DialogResult.OK;

            void MsgShow(string str) { MessageBox.Show(str + " cannot be empty!", "Error"); }
        }

        private void TxtAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                BtnOK_Click(btnOK, null);
        }

        private void CbbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            cbbModelName.Items.Clear();
            foreach (Device device in DeviceDB.GetDevices(cbbDeviceType.Text))
                cbbModelName.Items.Add(device.ModelName);
        }

        private void CbbInterface_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            int intfIdx = InterfaceList.FindIndex(x => x.Name == cbbInterface.Text);
            txtAddress.Text = InterfaceList[intfIdx].AddressFormat;
        }
    }
}
