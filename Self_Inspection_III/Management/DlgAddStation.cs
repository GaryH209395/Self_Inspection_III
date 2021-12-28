using Self_Inspection_III.Class;
using Self_Inspection_III.Management;
using System;
using System.Data;
using System.Windows.Forms;

namespace Self_Inspection_III.Setting
{
    public partial class DlgAddStation : Form
    {
        public DlgAddStation()
        {
            InitializeComponent();
        }

        public Station NewStation
        {
            get
            {
                return new Station(txtStationName.Text)
                {
                    Active = true,
                    Edited = true,
                    CL = txtCL.Text,
                    TL = txtTL.Text,
                    TE = txtTE.Text,
                    PE = txtPE.Text,
                    ID = txtStationID.Text
                };
            }
        }
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStationName.Text))
            {
                MessageBox.Show("Station Name is null!");
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnStationID_Click(object sender, EventArgs e)
        {
            DlgStationID dlgStationID = new DlgStationID();
            foreach (DataRow row in new ProgramDB().GetStationList().Rows)
            {
                Station station = XmlUtil.Deserialize(typeof(Station), row[1].ToString()) as Station;
                dlgStationID.DgvIdList.Rows.Add(new object[] { station.ID, station.Name });
            }
            dlgStationID.ShowDialog();
        }
    }
}
