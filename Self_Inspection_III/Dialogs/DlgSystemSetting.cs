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

namespace Self_Inspection_III.Dialogs
{
    public partial class DlgSystemSetting : Form
    {
        bool allState = true;
        SISystem systemSetting;

        ProgramDB ProgramDB = new ProgramDB();

        public DlgSystemSetting()
        {
            InitializeComponent();
            CbbStation_ItemsAddRange();
            systemSetting = SISystem.Setting;
            SettingInitialize();
        }
        private void CbbStation_ItemsAddRange()
        {
            foreach (DataRow row in ProgramDB.GetStationList().Rows)
            {
                try { cbbStation.Items.Add(row[0].ToString()); }
                catch (Exception ex) { Console.WriteLine(ex.ToString() + $"\nStation: {row[0]}"); }
            }
        }
        private void SettingInitialize()
        {
            cbbStation.Text = SystemSetting.ThisStation;
            cbAlarmEmail.Checked = SystemSetting.AlarmEmail;
            cbErrorEmail.Checked = SystemSetting.ErrorEmail;
            cbEmployeeID.Checked = SystemSetting.CheckEmployeeID;
            cbHolidayLst.Checked = SystemSetting.GetHolidayList;
        }

        #region Attributes
        public SISystem SystemSetting { get => systemSetting; }
        #endregion

        private void BtnAll_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnEmailAll":
                    SetAllCheckBox(gbEmail);
                    break;
                case "btnMESAll":
                    SetAllCheckBox(gbMES);
                    break;
            }
            allState = !allState;

            #region InnerMethod
            void SetAllCheckBox(GroupBox groupBox)
            {
                foreach (Control ctrl in groupBox.Controls)
                    if (ctrl.GetType() == typeof(CheckBox))
                        ((CheckBox)ctrl).Checked = allState;
            }
            #endregion
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            systemSetting = new SISystem
            {
                ThisStation = cbbStation.Text,
                AlarmEmail = cbAlarmEmail.Checked,
                ErrorEmail = cbErrorEmail.Checked,
                CheckEmployeeID = cbEmployeeID.Checked,
                GetHolidayList = cbHolidayLst.Checked
            };

            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
