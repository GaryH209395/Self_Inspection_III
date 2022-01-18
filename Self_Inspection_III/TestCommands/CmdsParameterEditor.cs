using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Self_Inspection_III.Class;
using System.Text;
using System.Text.RegularExpressions;
using Self_Inspection_III.TestCommands.DMM;

namespace Self_Inspection_III.TestCommands
{
    public partial class CmdsParameterEditor : Form
    {
        private TestCommand m_TestCommand;
        private Item m_Item;
        private List<string> Labels;
        private List<Device> Devices;
        private string ParaString;

        private Parameter SelectedPara;

        public CmdsParameterEditor(TestCommand TestCommand, Item Item, string ParaString, List<Device> Devices)
        {
            InitializeComponent();
            this.TestCommand = TestCommand;
            this.Item = Item;
            this.ParaString = ParaString;
            this.Devices = Devices;

            Labels = new List<string>();
            foreach (string label in Item.Vars.Labels)
                Labels.Add(label);
            Labels.AddRange(new string[] { "TestFail", "TestPass" });

            foreach (RadioButton rbtn in gbParaType.Controls)
                rbtn.CheckedChanged += new EventHandler(ParaType_CheckedChanged);

            foreach (RadioButton rbtn in gbConstEditType.Controls)
                rbtn.CheckedChanged += new EventHandler(ConstType_CheckedChanged);

            dgvParameters.CellClick += new DataGridViewCellEventHandler(DgvParameters_CellClick);

            InitComboBox();
        }

        private void InitComboBox()
        {
            if (TestCommand.DeviceType == DeviceTypes.Null)
                cbbDevice.Enabled = false;
            else
                cbbDevice.Items.AddRange(new object[] { "Source", "Meter", "UUT" });

            foreach (Device device in Devices)
                if (TestCommand.DeviceType == DeviceTypes.All || device.Type == TestCommand.DeviceType.ToString())
                    cbbDevice.Items.Add($"{device.ModelName}-{device.No}");
        }
        private void Editor_Load(object sender, EventArgs e)
        {
            Console.WriteLine("=Editor_Load= Start");

            try
            {
                Title = TestCommand.Name;
                Console.WriteLine($"Title: {Title}");
                if (TestCommand.Parameters.Count > 0)
                {
                    for (int i = 0; i < TestCommand.Parameters.Count; i++)
                    {
                        Parameter tcp = TestCommand.Parameters[i];
                        Console.WriteLine($"Set Parameters[{i}]: {tcp.Name}");
                        dgvParameters.Rows.Add(new object[] { tcp.Name, tcp.DefaultValue });
                        dgvParameters.Rows[i].Selected = false;
                        if (i == 0) dgvParameters[(int)ColPara.Value, i].Selected = true;

                        try { dgvParameters[(int)ColPara.Value, i].Value = ParaString.Split(',')[i]; } catch { }
                    }
                    DgvParameters_CellClick(null, new DataGridViewCellEventArgs((int)ColPara.Value, 0));
                    SelectedPara = TestCommand.Parameters[0];
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            Console.WriteLine("=Editor_Load= END");
        }
        public string Title { get => Text; set => Text = value; }
        public string Description { get => lblParaDescription.Text; }
        public DataGridView DgvParaSetting { get => dgvParameters; set => dgvParameters = value; }
        public TestCommand TestCommand { get => m_TestCommand; set => m_TestCommand = value; }
        public Item Item { get => m_Item; set => m_Item = value; }
        public string Paras
        {
            get
            {
                if (dgvParameters.RowCount > 0)
                {
                    StringBuilder sbPara = new StringBuilder(dgvParameters[(int)ColPara.Value, 0].Value.ToString());
                    for (int i = 1; i < dgvParameters.RowCount; i++)
                        sbPara.Append("," + dgvParameters[(int)ColPara.Value, i].Value.ToString());
                    return sbPara.ToString();
                }
                else
                    return string.Empty;
            }
        }
        public string Device { get => cbbDevice.Text; set => cbbDevice.Text = value; }

        private void DgvParameters_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            Console.WriteLine("=DgvParameters_CellClick= Start");

            int pIdx = TestCommand.Parameters.FindIndex(x => x.Name == dgvParameters[(int)ColPara.Name, e.RowIndex].Value.ToString());
            SelectedPara = TestCommand.Parameters[pIdx];

            string paraValue = SelectedPara.DefaultValue ?? string.Empty;
            try { paraValue = dgvParameters[(int)ColPara.Value, e.RowIndex].Value.ToString(); } catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            Console.WriteLine($"{SelectedPara.Name}'s Value: {paraValue}");
            if (!string.IsNullOrEmpty(paraValue))
            {
                if (SelectedPara.ParaType == ParaTypes.Label.ToString())
                {
                    if (!Labels.Exists(x => x == paraValue)) 
                        dgvParameters[(int)ColPara.Value, e.RowIndex].Value = null;
                }
                else
                if (Parameter.OperatorList.Exists(x => x == paraValue)) SelectedPara.ParaType = ParaTypes.Operator.ToString();
                else
                if (Item.Vars.Conditions.Exists(x => x.CallName == paraValue)) SelectedPara.ParaType = ParaTypes.Condition.ToString();
                else
                if (Item.Vars.Temporaries.Exists(x => x.CallName == paraValue)) SelectedPara.ParaType = ParaTypes.Temporary.ToString();
                else
                if (Item.Vars.Results.Exists(x => x.CallName == paraValue)) SelectedPara.ParaType = ParaTypes.Result.ToString();
                else
                {
                    SelectedPara.ParaType = ParaTypes.Constant.ToString();
                    if (Array.Exists(SelectedPara.EnumItems, x => x.Name == paraValue))
                        SelectedPara.ConstType = ConstTypes.ComboList.ToString();
                    else
                        SelectedPara.ConstType = ConstTypes.EditBox.ToString();
                }
            }
            Console.WriteLine($"{SelectedPara.Name}'s Type: {SelectedPara.ParaType}");

            SetParaType(SelectedPara.ParaType, SelectedPara.ConstType);
            dgvParameters[(int)ColPara.Value, e.RowIndex].Value = paraValue;

            //Show Description
            lblParaName.Text = SelectedPara.Name;
            lblParaType.Location = new System.Drawing.Point(lblParaName.Location.X + lblParaName.Size.Width, lblParaType.Location.Y);
            lblParaType.Text = $"({SelectedPara.DataType})";
            lblParaDescription.Text = SelectedPara.Description;

            Console.WriteLine("=DgvParameters_CellClick= END");
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (TestCommand.Parameters.Count > 0)
            {
                for (int i = 0; i < dgvParameters.RowCount; i++)
                {
                    if (dgvParameters[(int)ColPara.Value, i].Value == null || string.IsNullOrEmpty(dgvParameters[(int)ColPara.Value, i].Value.ToString()))
                    {
                        MessageBox.Show(dgvParameters[(int)ColPara.Name, i].Value + " cannot be empty!");
                        return;
                    }
                }
            }

            if (TestCommand.DeviceType != DeviceTypes.Null)
            {
                if (string.IsNullOrEmpty(cbbDevice.Text))
                {
                    MessageBox.Show("Device cannot be empty!");
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void SetParaType(string ParaType, string ConstType)
        {
            Console.WriteLine("=SetParaType= Start");

            foreach (RadioButton rbtn in gbParaType.Controls) rbtn.Enabled = false;
            rbtnConstant.Enabled = ConstType != ConstTypes.NotAllow.ToString();
            switch (ParaType)
            {
                case "Condition":
                    rbtnCondition.Enabled = true;
                    rbtnResult.Enabled = true;
                    rbtnTemporary.Enabled = true;

                    rbtnCondition.Checked = true;
                    ParaType_CheckedChanged(rbtnCondition, null);
                    break;
                case "Temporary":
                    rbtnCondition.Enabled = true;
                    rbtnResult.Enabled = true;
                    rbtnTemporary.Enabled = true;

                    rbtnTemporary.Checked = true;
                    ParaType_CheckedChanged(rbtnTemporary, null);
                    break;
                case "Result":
                    rbtnCondition.Enabled = true;
                    rbtnResult.Enabled = true;
                    rbtnTemporary.Enabled = true;

                    rbtnResult.Checked = true;
                    ParaType_CheckedChanged(rbtnResult, null);
                    break;
                case "Constant":
                    rbtnCondition.Enabled = true;
                    rbtnResult.Enabled = true;
                    rbtnTemporary.Enabled = true;

                    rbtnConstant.Checked = true;
                    ParaType_CheckedChanged(rbtnConstant, null);
                    break;
                case "Operator":
                    rbtnOperator.Enabled = true;

                    rbtnOperator.Checked = true;
                    ParaType_CheckedChanged(rbtnOperator, null);
                    break;
                case "Label":
                    rbtnLabel.Enabled = true;

                    rbtnLabel.Checked = true;
                    ParaType_CheckedChanged(rbtnLabel, null);
                    break;
                case "Signal":
                    rbtnSignal.Enabled = true;

                    rbtnSignal.Checked = true;
                    ParaType_CheckedChanged(rbtnSignal, null);
                    break;
                default: MessageBox.Show("Invalid Parameter Type: " + ParaType, "Error"); break;
            }
            Console.WriteLine("=SetParaType= END");
        }

        private void ParaType_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false) return;

            Console.WriteLine("=ParaType_CheckedChanged= Start");
            int SelectedPIdx = TestCommand.Parameters.FindIndex(x => x.Name == SelectedPara.Name);

            if (rbtnCondition.Checked)
            {
                Console.WriteLine("rbtnCondition.Checked: " + rbtnCondition.Checked);
                DataGridViewComboBoxCell tempCbbCell = new DataGridViewComboBoxCell();
                foreach (ConditionVar cv in Item.Vars.Conditions)
                    tempCbbCell.Items.Add(cv.CallName);
                dgvParameters[(int)ColPara.Value, SelectedPIdx] = tempCbbCell;
                SelectedPara.ParaType = ParaTypes.Condition.ToString();
            }
            else
            if (rbtnResult.Checked)
            {
                Console.WriteLine("rbtnResult.Checked: " + rbtnResult.Checked);
                DataGridViewComboBoxCell tempCbbCell = new DataGridViewComboBoxCell();
                foreach (ResultVar rv in Item.Vars.Results)
                    tempCbbCell.Items.Add(rv.CallName);
                dgvParameters[(int)ColPara.Value, SelectedPIdx] = tempCbbCell;
                SelectedPara.ParaType = ParaTypes.Result.ToString();
            }
            else
            if (rbtnTemporary.Checked)
            {
                Console.WriteLine("rbtnTemporary.Checked: " + rbtnTemporary.Checked);
                DataGridViewComboBoxCell tempCbbCell = new DataGridViewComboBoxCell();
                foreach (TemporaryVar tv in Item.Vars.Temporaries)
                    tempCbbCell.Items.Add(tv.CallName);
                dgvParameters[(int)ColPara.Value, SelectedPIdx] = tempCbbCell;
                SelectedPara.ParaType = ParaTypes.Temporary.ToString();
            }
            else
            if (rbtnGlobal.Checked)
            {
                Console.WriteLine("rbtnGlobal.Checked: " + rbtnGlobal.Checked);
                SelectedPara.ParaType = ParaTypes.Global.ToString();
            }
            else
            if (rbtnConstant.Checked)
            {
                Console.WriteLine("rbtnConstant.Checked: " + rbtnConstant.Checked);
                if (SelectedPara.EnumItems.Length == 0)
                {
                    rbtnComboList.Enabled = false;
                    SelectedPara.ConstType = EditTypes.EditBox.ToString();
                }
                if (SelectedPara.ConstType == EditTypes.ComboList.ToString())
                {
                    rbtnComboList.Checked = true;
                    ConstType_CheckedChanged(rbtnComboList, null);
                }
                else
                {
                    rbtnEditBox.Checked = true;
                    ConstType_CheckedChanged(rbtnEditBox, null);
                }
                SelectedPara.ParaType = ParaTypes.Constant.ToString();
            }
            else
            if (rbtnOperator.Checked)
            {
                Console.WriteLine("rbtnOperator.Checked: " + rbtnOperator.Checked);
                DataGridViewComboBoxCell tempCbbCell = new DataGridViewComboBoxCell();
                tempCbbCell.Items.AddRange(Parameter.OperatorList.ToArray());
                dgvParameters[(int)ColPara.Value, SelectedPIdx] = tempCbbCell;
                SelectedPara.ParaType = ParaTypes.Operator.ToString();
            }
            else
            if (rbtnLabel.Checked)
            {
                Console.WriteLine("rbtnLabel.Checked: " + rbtnLabel.Checked);
                DataGridViewComboBoxCell tempCbbCell = new DataGridViewComboBoxCell();
                foreach (SI_TestFunction tf in Item.TestFunctions)
                    if (!string.IsNullOrEmpty(tf.Label))
                        tempCbbCell.Items.Add(tf.Label);
                dgvParameters[(int)ColPara.Value, SelectedPIdx] = tempCbbCell;
                SelectedPara.ParaType = ParaTypes.Label.ToString();
            }
            else
            if (rbtnSignal.Checked)
            {
                Console.WriteLine("rbtnSignal.Checked: " + rbtnSignal.Checked);
                SelectedPara.ParaType = ParaTypes.Signal.ToString();
            }

            //Constant Edit Type
            foreach (RadioButton rbtn in gbConstEditType.Controls)
                rbtn.Enabled = rbtnConstant.Checked;

            Console.WriteLine("=ParaType_CheckedChanged= END");
        }

        private void ConstType_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("=ConstType_CheckedChanged= Start");
            int SelectedPIdx = TestCommand.Parameters.FindIndex(x => x.Name == SelectedPara.Name);

            if (rbtnEditBox.Checked)
            {
                if (dgvParameters[(int)ColPara.Value, SelectedPIdx].GetType() == typeof(DataGridViewComboBoxCell))
                {
                    DataGridViewTextBoxCell tempTxtCell = new DataGridViewTextBoxCell();
                    dgvParameters[(int)ColPara.Value, SelectedPIdx] = tempTxtCell;
                    SelectedPara.ConstType = EditTypes.EditBox.ToString();
                }
            }
            else
            if (rbtnComboList.Checked)
            {
                if (dgvParameters[(int)ColPara.Value, SelectedPIdx].GetType() == typeof(DataGridViewTextBoxCell))
                {
                    DataGridViewComboBoxCell tempCbbCell = new DataGridViewComboBoxCell();
                    foreach (EnumItem ei in SelectedPara.EnumItems)
                        tempCbbCell.Items.Add(ei.Name);
                    dgvParameters[(int)ColPara.Value, SelectedPIdx] = tempCbbCell;
                    SelectedPara.ConstType = EditTypes.ComboList.ToString();
                }
            }
            SelectedPara.DefaultValue = string.Empty;
            Console.WriteLine("=ConstType_CheckedChanged= END");
        }

        private void DgvParameters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("==DgvParameters_CellEndEdit==");
            Console.WriteLine($"SelectedPara.ParaType: {SelectedPara.ParaType}\nSelectedPara.DataType: {SelectedPara.DataType}");

            if (SelectedPara.ParaType == ParaTypes.Constant.ToString() &&
                SelectedPara.DataType == DataTypes.String.ToString() &&
                SelectedPara.ConstType == ConstTypes.EditBox.ToString())
            {
                string tempStr = ((DataGridView)sender)[(int)ColPara.Value, e.RowIndex].Value == null ? string.Empty : ((DataGridView)sender)[(int)ColPara.Value, e.RowIndex].Value.ToString();
                Console.WriteLine( $"{tempStr} --> \"{Regex.Replace(tempStr, "\"", "")}\"");
                ((DataGridView)sender)[(int)ColPara.Value, e.RowIndex].Value = $"\"{Regex.Replace(tempStr, "\"", "")}\"";
            }
        }
    }
    enum ColPara
    {
        Name = 0,
        Value
    }
}
