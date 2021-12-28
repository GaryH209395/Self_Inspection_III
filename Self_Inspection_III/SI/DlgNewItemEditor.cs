using Self_Inspection_III.Class;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Self_Inspection_III.SI
{
    public partial class DlgNewItemEditor : Form
    {
        ProgramDB ProgramDB = new ProgramDB();
        List<Device> DeviceList;

        bool isNew = false;

        public DlgNewItemEditor(bool isNew)
        {
            InitializeComponent();
            lblStation.Text = SISystem.Setting.ThisStation;
            this.isNew = isNew;
            
            DeviceList = ProgramDB.GetStation(SISystem.Setting.ThisStation).Devices;

            if (!isNew) Text = "Item Editor";

            foreach (string tp in Enum.GetNames(typeof(TestPeriod)))
                Console.WriteLine($"DlgNewItemEditor.cbbPeriod.Items.Add({tp})");
            cbbPeriod.Items.AddRange(Enum.GetNames(typeof(TestPeriod)));
            cbbPeriod.Text = TestPeriod.Week.ToString();

            cbbSourceList.Items.Add(Item.NONE);
            cbbMeterList.Items.Add(Item.NONE);
        }

        public List<string> SourceList = new List<string>();
        public List<string> MeterList = new List<string>();
        public string ItemName { get => txtItemName.Text; }
        public string Source { get => cbbSourceList.Text; }
        public string Meter { get => cbbMeterList.Text; }
        public TestPeriod Period { get => MyEnum.GetPeriod(cbbPeriod.Text); }

        public void Setting(Item item)
        {
            txtItemName.Text = item.Name;
            cbbPeriod.Text = item.Period.ToString();

            SourceList = item.Sources;
            foreach (string src in item.Sources)
                if (!cbbSourceList.Items.Contains(src))
                    cbbSourceList.Items.Add(src);
            cbbSourceList.Text = item.Source;

            MeterList = item.Meters;
            foreach (string mtr in item.Meters)
                if (!cbbMeterList.Items.Contains(mtr))
                    cbbMeterList.Items.Add(mtr);
            cbbMeterList.Text = item.Meter;
        }

        private void BtnSourceList_Click(object sender, EventArgs e)
        {
            DlgDevcieList dlgDevLst = new DlgDevcieList(DeviceList, SourceList);
            if (dlgDevLst.ShowDialog(Owner) == DialogResult.OK)
            {
                cbbSourceList.Items.Clear();
                cbbSourceList.Items.AddRange(dlgDevLst.ObjectArray);
                cbbSourceList.SelectedIndex = 0;
                SourceList = dlgDevLst.ModelList;
            }
        }

        private void BtnMeterList_Click(object sender, EventArgs e)
        {
            DlgDevcieList dlgDevLst = new DlgDevcieList(DeviceList, MeterList);
            if (dlgDevLst.ShowDialog(Owner) == DialogResult.OK)
            {
                cbbMeterList.Items.Clear();
                cbbMeterList.Items.AddRange(dlgDevLst.ObjectArray);
                cbbMeterList.SelectedIndex = 0;
                MeterList = dlgDevLst.ModelList;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemName.Text))
                MessageBox.Show("Item Name cannot be null!", "Error");
            else
            if (string.IsNullOrEmpty(cbbSourceList.Text))
                MessageBox.Show("Source cannot be null!", "Error");
            else
            if (string.IsNullOrEmpty(cbbMeterList.Text))
                MessageBox.Show("Meter cannot be null!", "Error");
            else
            {
                Console.WriteLine(
                    "Station: " + lblStation.Text +
                    "\nItem Name: " + txtItemName.Text +
                    "\nSource: " + cbbSourceList.Text +
                    "\nMeter: " + cbbMeterList.Text);

                DialogResult = DialogResult.OK;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
