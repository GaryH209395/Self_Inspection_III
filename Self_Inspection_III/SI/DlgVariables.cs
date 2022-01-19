using Self_Inspection_III.Class;
using Self_Inspection_III.Style;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Self_Inspection_III.SI
{
    public partial class DlgVariables : Form
    {
        bool isConstructor = false;
        bool isLoad = false;

        private ItemVars _itemVars;
        private DataGridView DgvFocused;
        private List<DataGridViewRow> dgvRows = new List<DataGridViewRow>();

        #region Constructor
        public DlgVariables(ItemVars vars)
        {
            isConstructor = true;
            InitializeComponent();
            ItemVars = vars;
            isConstructor = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            isLoad = true;

            DgvFocused = dgvCondition;
            ButtonStyleSetting();
            VarsInitialize();

            isLoad = false;
        }
        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                e.Cancel = true;
            }
        }
        private void ButtonStyleSetting()
        {
            foreach (ToolStripItem item in toolStrip3.Items)
            {
                if (item.GetType() == typeof(ToolStripLabel) && item.Enabled)
                {
                    item.MouseHover += new EventHandler(TsbtnStyle.MouseHover);
                    item.MouseLeave += new EventHandler(TsbtnStyle.MouseLeave);
                    item.MouseEnter += new EventHandler(TsbtnStyle.MouseEnter);
                    item.MouseMove += new MouseEventHandler(TsbtnStyle.MouseMove);
                    item.MouseDown += new MouseEventHandler(TsbtnStyle.MouseDown);
                    item.MouseUp += new MouseEventHandler(TsbtnStyle.MouseUp);
                }
            }
        }
        private void ComboBoxSetting()
        {
            //dgvColConDataType.Items.AddRange(Variables.DataTypesList);
            //dgvColTempDataType.Items.AddRange(Variables.DataTypesList);
            //dgvColResDataType.Items.AddRange(Variables.DataTypesList);

            //dgvColConEditType.Items.AddRange(Variables.EditTypesList);
        }
        private void VarsInitialize()
        {
            // Condition
            foreach (ConditionVar cv in ItemVars.Conditions)
                dgvCondition.Rows.Add(new object[] { cv.ShowName, cv.CallName, cv.DataType, cv.EditType, cv.Value, cv.Enum });

            //Temporary
            foreach (TemporaryVar tv in ItemVars.Temporaries)
                dgvTemporary.Rows.Add(new object[] { tv.ShowName, tv.CallName, tv.DataType });

            //Result
            foreach (ResultVar rv in ItemVars.Results)
                dgvResult.Rows.Add(new object[] { rv.ShowName, rv.CallName, rv.DataType, rv.SpecMin.CallName, rv.SpecMax.CallName });
        }
        private void Save()
        {
            bool invalid = false;

            CheckDuplicatedCallName();
            CheckValueType();

            //Condition
            dgvCondition.EndEdit();
            if (ItemVars.Conditions.Count > 0) ItemVars.Conditions.Clear();
            for (int i = 0; i < dgvCondition.RowCount; i++)
            {
                invalid = false;
                for (int col = 0; col < dgvCondition.ColumnCount; col++)
                {
                    try
                    {
                        if (string.IsNullOrEmpty((dgvCondition[col, i].Value ?? string.Empty).ToString()))
                            if (col == (int)ColCondiVar.Value)
                                dgvCondition[col, i].Value = string.Empty;
                            else
                                invalid = true;
                        if (col == (int)ColCondiVar.Enum && dgvCondition[(int)ColCondiVar.EditType, i].Value.ToString() == EditTypes.EditBox.ToString())
                            invalid = false;
                    }
                    catch (Exception ex) { Console.WriteLine($"{dgvCondition[col, i].Value}\n{ex.ToString()}"); }
                }
                if (invalid) continue;

                ItemVars.Conditions.Add(new ConditionVar(
                    dgvCondition[(int)ColCondiVar.ShowName, i].Value.ToString(),
                    dgvCondition[(int)ColCondiVar.CallName, i].Value.ToString(),
                    dgvCondition[(int)ColCondiVar.DataType, i].Value.ToString(),
                    dgvCondition[(int)ColCondiVar.EditType, i].Value.ToString(),
                    dgvCondition[(int)ColCondiVar.Value, i].Value.ToString(),
                    dgvCondition[(int)ColCondiVar.Enum, i].Value.ToString()));

                ConditionVar cv = ItemVars.Conditions[i];
                Console.WriteLine($"Save: {cv.ShowName}, {cv.CallName}, {cv.DataType}, {cv.EditType}, {cv.Value}, {cv.Enum ?? "n/a"}");
            }
            Console.WriteLine("Condition Vars Saved!");

            //Temporary
            dgvTemporary.EndEdit();
            if (ItemVars.Temporaries.Count > 0) ItemVars.Temporaries.Clear();
            for (int i = 0; i < dgvTemporary.RowCount; i++)
            {
                invalid = false;
                for (int col = 0; col < dgvTemporary.ColumnCount; col++)
                    if (string.IsNullOrEmpty(dgvTemporary[col, i].Value.ToString())) invalid = true;
                if (invalid) continue;

                ItemVars.Temporaries.Add(new TemporaryVar(
                    dgvTemporary[(int)ColTempVar.ShowName, i].Value.ToString(),
                    dgvTemporary[(int)ColTempVar.CallName, i].Value.ToString(),
                    dgvTemporary[(int)ColTempVar.DataType, i].Value.ToString()));

                TemporaryVar tv = ItemVars.Temporaries[i];
                Console.WriteLine($"{tv.ShowName}, {tv.CallName}, {tv.DataType}");
            }
            Console.WriteLine("Temporary Vars Saved!");

            //Result
            dgvResult.EndEdit();
            if (ItemVars.Results.Count > 0) ItemVars.Results.Clear();
            for (int i = 0; i < dgvResult.RowCount; i++)
            {
                invalid = false;
                for (int col = 0; col < dgvResult.ColumnCount; col++)
                    try { if (string.IsNullOrEmpty(dgvResult[col, i].Value.ToString())) invalid = true; } catch { invalid = true; }
                if (invalid) continue;

                Variables SpecMin, SpecMax;
                if (dgvResult[(int)ColResultVar.SpecMin, i].Value == null)
                    SpecMin = null;
                else
                    SpecMin = ItemVars.Conditions[ItemVars.Conditions.FindIndex(x => x.CallName == dgvResult[(int)ColResultVar.SpecMin, i].Value.ToString())];

                if (dgvResult[(int)ColResultVar.SpecMax, i].Value == null)
                    SpecMax = null;
                else
                    SpecMax = ItemVars.Conditions[ItemVars.Conditions.FindIndex(x => x.CallName == dgvResult[(int)ColResultVar.SpecMax, i].Value.ToString())];

                ItemVars.Results.Add(new ResultVar(
                    dgvResult[(int)ColResultVar.ShowName, i].Value.ToString(),
                    dgvResult[(int)ColResultVar.CallName, i].Value.ToString(),
                    dgvResult[(int)ColResultVar.DataType, i].Value.ToString(),
                    SpecMin,
                    SpecMax));

                ResultVar rv = ItemVars.Results[i];
                Console.WriteLine($"{rv.ShowName}, {rv.CallName}, {rv.DataType}, {rv.SpecMin.CallName}, {rv.SpecMax.CallName}");
            }
            Console.WriteLine("Result Vars Saved!");
        }
        #endregion

        #region Attributes
        public ItemVars ItemVars
        {
            get
            {
                if (_itemVars == null)
                    _itemVars = new ItemVars();
                return _itemVars;
            }
            set => _itemVars = value;
        }
        #endregion

        #region TsBtn Functions
        private void TsbtnNew_Click(object sender, EventArgs e)
        {
            if (DgvFocused == null) { MessageBox.Show("No Selected Sheet", "Error"); return; }
            try
            {
                DgvFocused.Rows.Add();
                Console.WriteLine($"{DgvFocused.Name}.Rows Added");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void TsbtnSave_Click(object sender, EventArgs e)
        {
            if (DgvFocused == null) { MessageBox.Show("No Selected Sheet", "Error"); return; }
            try
            {
                Save();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void TsbtnCut_Click(object sender, EventArgs e)
        {
            if (DgvFocused == null) { MessageBox.Show("No Selected Sheet", "Error"); return; }
            foreach (DataGridViewRow row in DgvFocused.SelectedRows)
            {
                DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();

                for (int i = 0; i < row.Cells.Count; i++)
                    clonedRow.Cells[i].Value = row.Cells[i].Value;

                dgvRows.Add(clonedRow);
                DgvFocused.Rows.Remove(row);
                Console.WriteLine($"{DgvFocused.Name}.Rows Copied");
            }
        }
        private void TsbtnCopy_Click(object sender, EventArgs e)
        {
            if (dgvRows != null && dgvRows.Count > 0) dgvRows.Clear();

            foreach (DataGridViewRow row in DgvFocused.SelectedRows)
            {
                DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();

                for (int i = 0; i < row.Cells.Count; i++)
                    clonedRow.Cells[i].Value = row.Cells[i].Value;

                dgvRows.Add(clonedRow);
                Console.WriteLine($"{DgvFocused.Name}.Rows Copied");
            }
        }
        private void TsbtnPaste_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRows.Count > 0)
                {
                    for (int i = dgvRows.Count, j = 1; i > 0; i--, j++)
                    {
                        int rIdx = DgvFocused.SelectedRows[0].Index + j;
                        DgvFocused.Rows.Insert(rIdx, dgvRows[i - 1]);
                    }
                }
                else
                {
                    MessageBox.Show("No Selected Sheet");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void TsbtnDelete_Click(object sender, EventArgs e)
        {
            if (DgvFocused == null) { MessageBox.Show("No Selected Sheet", "Error"); return; }
            foreach (DataGridViewRow row in DgvFocused.SelectedRows)
            {
                DgvFocused.Rows.Remove(row);
            }
        }
        private void TsbtnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (DgvFocused.RowCount == 0) return;
                if (DgvFocused.SelectedRows.Count == 0) return;

                DataGridViewRow dr = DgvFocused.SelectedRows[0];
                int index = dr.Index;

                if (index > 0)
                {
                    DgvFocused.Rows.RemoveAt(index);
                    DgvFocused.Rows.Insert(index - 1, dr);

                    DgvFocused.Rows[index].Selected = false;
                    DgvFocused.Rows[index - 1].Selected = true;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void TsbtnDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (DgvFocused.RowCount == 0) return;
                if (DgvFocused.SelectedRows.Count == 0) return;

                DataGridViewRow dr = DgvFocused.SelectedRows[0];
                int index = dr.Index;

                if (index < DgvFocused.RowCount - 1)
                {
                    DgvFocused.Rows.RemoveAt(index);
                    DgvFocused.Rows[index].Selected = false;

                    DgvFocused.Rows.Insert(index + 1, dr);
                    DgvFocused.Rows[index + 1].Selected = true;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region Save Check
        private void CheckDuplicatedCallName()
        {
            void ShowMsg(string duplicatedName) { MessageBox.Show($"CallName: {duplicatedName} Duplicated!", "Warning"); }
            List<string> checkList = new List<string>();

            //Condition
            foreach (ConditionVar cv in ItemVars.Conditions)
            {
                if (checkList.Exists(x => x == cv.CallName))
                {
                    for (int i = 0; i < dgvCondition.RowCount; i++)
                        if (dgvCondition[(int)ColCondiVar.CallName, i].Value.ToString() == cv.CallName)
                            dgvCondition[(int)ColCondiVar.CallName, i].Selected = true;

                    tbCtrlVariables.SelectedIndex = 0;
                    ShowMsg(cv.CallName);
                    return;
                }
                else { checkList.Add(cv.CallName); }
            }

            //Temporary
            foreach (TemporaryVar tv in ItemVars.Temporaries)
            {
                if (checkList.Exists(x => x == tv.CallName))
                {
                    for (int i = 0; i < dgvTemporary.RowCount; i++)
                        if (dgvTemporary[(int)ColTempVar.CallName, i].Value.ToString() == tv.CallName)
                            dgvTemporary[(int)ColTempVar.CallName, i].Selected = true;

                    tbCtrlVariables.SelectedIndex = 1;
                    ShowMsg(tv.CallName);
                    return;
                }
                else { checkList.Add(tv.CallName); }
            }

            //Result
            foreach (ResultVar rv in ItemVars.Results)
            {
                if (checkList.Exists(x => x == rv.CallName))
                {
                    for (int i = 0; i < dgvResult.RowCount; i++)
                        if (dgvResult[(int)ColResultVar.CallName, i].Value.ToString() == rv.CallName)
                            dgvResult[(int)ColResultVar.CallName, i].Selected = true;

                    tbCtrlVariables.SelectedIndex = 2;
                    ShowMsg(rv.CallName);
                    return;
                }
                else { checkList.Add(rv.CallName); }
            }
        }
        private void CheckValueType()
        {
            for (int i = 0; i < dgvCondition.RowCount; i++)
            {
                if (dgvCondition[(int)ColCondiVar.DataType, i].Value != null)
                {
                    string default_value = (dgvCondition[(int)ColCondiVar.DataType, i].Value ?? string.Empty).ToString();
                    if (!string.IsNullOrEmpty(default_value))
                    {
                        switch (dgvCondition[(int)ColCondiVar.DataType, i].Value.ToString())
                        {
                            case "Float":
                                try { double d = Convert.ToDouble(dgvCondition[(int)ColCondiVar.Value, i].Value); }
                                catch { InvalidMsg(i); }
                                break;
                            case "Integer":
                                try { int j = Convert.ToInt32(dgvCondition[(int)ColCondiVar.Value, i].Value); }
                                catch { InvalidMsg(i); }
                                break;
                        }
                    }
                }
            }

            void InvalidMsg(int i)
            {
                MessageBox.Show($"Invalid Value\n" +
                    $"Call Name: {dgvCondition[(int)ColCondiVar.CallName, i].Value.ToString()}\n" +
                    $"Data Type: {dgvCondition[(int)ColCondiVar.DataType, i].Value.ToString()}\n" +
                    $"Value:     {dgvCondition[(int)ColCondiVar.Value, i].Value.ToString()}",
                    "Error");
            }
        }
        #endregion

        #region DgvCondition Functions
        private void DgvCondition_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isConstructor || isLoad) return;

            switch (e.ColumnIndex)
            {
                case (int)ColCondiVar.DataType:
                    if (dgvCondition[(int)ColCondiVar.DataType, e.RowIndex].Value.ToString().Contains("[ ]"))
                    {
                        string v = dgvCondition[(int)ColCondiVar.DataType, e.RowIndex].Value.ToString();
                        DlgSetArrayLength dlgSetArrayLength = new DlgSetArrayLength();
                        if (dlgSetArrayLength.ShowDialog() == DialogResult.OK)
                        {
                            DataGridViewTextBoxCell newTxtCell = new DataGridViewTextBoxCell { Value = v.Replace("[ ]", $"[{dlgSetArrayLength.ArraySize}]") };
                            dgvCondition[(int)ColCondiVar.DataType, e.RowIndex] = newTxtCell;
                            dgvCondition[(int)ColCondiVar.Value, e.RowIndex].Value = string.Empty;
                        }
                        dgvCondition[(int)ColCondiVar.EditType, e.RowIndex].Value = EditTypes.EditBox.ToString();
                    }
                    break;
                case (int)ColCondiVar.EditType:
                    if (dgvCondition[(int)ColCondiVar.EditType, e.RowIndex].Value.ToString() == EditTypes.EditBox.ToString())
                    {
                        dgvCondition[(int)ColCondiVar.Value, e.RowIndex] = new DataGridViewTextBoxCell();
                        dgvCondition[(int)ColCondiVar.Enum, e.RowIndex].Value = string.Empty;
                    }
                    else
                    if (dgvCondition[(int)ColCondiVar.EditType, e.RowIndex].Value.ToString() == EditTypes.ComboList.ToString())
                    {
                        dgvCondition[(int)ColCondiVar.Value, e.RowIndex] = new DataGridViewComboBoxCell();
                        ValueComboListAddItems();
                    }
                    break;
                case (int)ColCondiVar.Enum:
                    if (dgvCondition[(int)ColCondiVar.EditType, e.RowIndex].Value.ToString() == EditTypes.ComboList.ToString())
                        ValueComboListAddItems();
                    break;
                default:
                    break;
            }

            void ValueComboListAddItems()
            {
                if (dgvCondition[(int)ColCondiVar.Enum, e.RowIndex].Value != null)
                {
                    ((DataGridViewComboBoxCell)dgvCondition[(int)ColCondiVar.Value, e.RowIndex]).Items.Clear();
                    try
                    {
                        foreach (string str in dgvCondition[(int)ColCondiVar.Enum, e.RowIndex].Value.ToString().Split(','))
                            ((DataGridViewComboBoxCell)dgvCondition[(int)ColCondiVar.Value, e.RowIndex]).Items.Add(str.Split('=')[1]);
                    }
                    catch { }
                }
            }
        }
        private void DgvCondition_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.ColumnIndex < 0)) return;
            try
            {
                switch (e.ColumnIndex)
                {
                    case (int)ColCondiVar.DataType:
                        dgvCondition.EndEdit();
                        dgvCondition.ReadOnly = true;
                        object dv = dgvCondition[e.ColumnIndex, e.RowIndex].Value;
                        DataGridViewComboBoxCell dtypeCmbCell = new DataGridViewComboBoxCell();
                        dtypeCmbCell.Items.AddRange(Variables.DataTypesList);
                        dgvCondition[e.ColumnIndex, e.RowIndex] = dtypeCmbCell;
                        dgvCondition.ReadOnly = false;
                        break;
                    case (int)ColCondiVar.EditType:
                        dgvCondition.EndEdit();
                        object ev = dgvCondition[e.ColumnIndex, e.RowIndex].Value;
                        DataGridViewComboBoxCell editCmbCell = new DataGridViewComboBoxCell { Value = ev ?? string.Empty };
                        editCmbCell.Items.AddRange(Variables.EditTypesList);
                        dgvCondition[e.ColumnIndex, e.RowIndex] = editCmbCell;
                        dgvCondition.ReadOnly = false;
                        break;
                    case (int)ColCondiVar.Value:
                        dgvCondition.EndEdit();
                        dgvCondition.ReadOnly = true;
                        Console.WriteLine($"Regex.IsMatch({dgvCondition[(int)ColCondiVar.DataType, e.RowIndex].Value}, {@"\[\d{1,}\]"}): " + Regex.IsMatch(dgvCondition[(int)ColCondiVar.DataType, e.RowIndex].Value.ToString(), @"\[\d{1,}\]"));
                        if (Regex.IsMatch(dgvCondition[(int)ColCondiVar.DataType, e.RowIndex].Value.ToString(), @"\[\d{1,}\]"))
                        {
                            object vo = dgvCondition[(int)ColCondiVar.Value, e.RowIndex].Value;
                            Console.WriteLine($"dgvCondition[{(int)ColCondiVar.Value}, {e.RowIndex}].Value = " + vo ?? "string.Empty");
                            DlgArrayEditor dlgArrayEditor = new DlgArrayEditor(vo ?? string.Empty, dgvCondition[(int)ColCondiVar.DataType, e.RowIndex].Value.ToString());
                            if (dlgArrayEditor.ShowDialog() == DialogResult.OK)
                            {
                                dgvCondition[(int)ColCondiVar.Value, e.RowIndex].Value = dlgArrayEditor.ArrayString;
                            }
                        }
                        else
                        {
                            dgvCondition.ReadOnly = false;
                            dgvCondition.BeginEdit(false);
                        }
                        break;
                    case (int)ColCondiVar.Enum:
                        dgvCondition.EndEdit();
                        if (dgvCondition[(int)ColCondiVar.EditType, e.RowIndex].Value.ToString() == EditTypes.ComboList.ToString())
                        {
                            DlgCondiEnumEditor cee = new DlgCondiEnumEditor(dgvCondition[(int)ColCondiVar.Enum, e.RowIndex].Value);
                            if (cee.ShowDialog() == DialogResult.OK)
                            {
                                dgvCondition[(int)ColCondiVar.Enum, e.RowIndex].Value = cee.EnumString;
                                dgvCondition.EndEdit();
                            }
                        }
                        break;
                    default:
                        dgvCondition.ReadOnly = false;
                        dgvCondition.BeginEdit(false);
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region DgvTemporary Functions
        private void DgvTemp_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.ColumnIndex < 0)) return;
            try
            {
                switch (e.ColumnIndex)
                {
                    case (int)ColCondiVar.DataType:
                        dgvTemporary.EndEdit();
                        dgvTemporary.ReadOnly = true;
                        object dv = dgvTemporary[e.ColumnIndex, e.RowIndex].Value;
                        DataGridViewComboBoxCell dtypeCmbCell = new DataGridViewComboBoxCell();
                        dtypeCmbCell.Items.AddRange(Variables.DataTypesList);
                        dgvTemporary[e.ColumnIndex, e.RowIndex] = dtypeCmbCell;
                        dgvTemporary.ReadOnly = false;
                        break;
                    default:
                        dgvTemporary.ReadOnly = false;
                        dgvTemporary.BeginEdit(false);
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region DgvResult Functions
        private void DgvResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.ColumnIndex < 0)) return;
            dgvResult.ReadOnly = true;
            switch (e.ColumnIndex)
            {
                case (int)ColResultVar.ShowName:
                case (int)ColResultVar.CallName:
                    dgvResult.ReadOnly = false;
                    dgvResult.BeginEdit(true);
                    break;
                case (int)ColResultVar.DataType:
                    dgvResult.EndEdit();
                    dgvResult.ReadOnly = false;
                    object dv = dgvResult[e.ColumnIndex, e.RowIndex].Value;
                    DataGridViewComboBoxCell dtypeCmbCell = new DataGridViewComboBoxCell();
                    dtypeCmbCell.Items.AddRange(Variables.DataTypesList);
                    dgvResult[e.ColumnIndex, e.RowIndex] = dtypeCmbCell;
                    break;
                case (int)ColResultVar.SpecMax:
                case (int)ColResultVar.SpecMin:
                    //dgvResult.EndEdit();
                    //dgvResult.ReadOnly = false;
                    //ResultSpecComboBoxSetting(e.ColumnIndex, e.RowIndex);
                    break;
                default: break;
            }
        }
        private void DgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvResult.ReadOnly = true;

            #region DGV EndEdit
            if (e.ColumnIndex < (int)ColResultVar.DataType || e.ColumnIndex > (int)ColResultVar.SpecMax)
            {
                Dgv_EndEdit();
                for (int rowIdx = 0; rowIdx < dgvResult.RowCount; rowIdx++)
                {
                    for (int colIdx = (int)ColResultVar.SpecMin; colIdx <= (int)ColResultVar.SpecMax; colIdx++)
                    {
                        string temp = dgvResult[colIdx, rowIdx].Value == null ? string.Empty : dgvResult[colIdx, rowIdx].Value.ToString();
                        dgvResult[colIdx, rowIdx] = new DataGridViewTextBoxCell { Value = temp };
                        Console.WriteLine($"{colIdx}: {temp}");
                    }
                }
            }
            #endregion

            if ((e.RowIndex < 0) || (e.ColumnIndex < 0)) return;

            switch (e.ColumnIndex)
            {
                case (int)ColResultVar.ShowName:
                case (int)ColResultVar.CallName:
                    dgvResult.ReadOnly = false;
                    dgvResult.BeginEdit(true);
                    break;
                case (int)ColResultVar.DataType:
                    dgvResult.ReadOnly = false;
                    dgvResult.BeginEdit(true);
                    break;
                case (int)ColResultVar.SpecMax:
                case (int)ColResultVar.SpecMin:
                    dgvResult.ReadOnly = false;
                    //dgvResult.BeginEdit(true);
                    ResultSpecComboBoxSetting(e.ColumnIndex, e.RowIndex);
                    break;
                default: break;
            }
        }
        private void ResultSpecComboBoxSetting(int colIdx, int rowIdx)
        {
            try
            {
                string spec = dgvResult[colIdx, rowIdx].Value == null ? string.Empty : dgvResult[colIdx, rowIdx].Value.ToString();
                dgvResult[colIdx, rowIdx].Value = null;

                DataGridViewComboBoxCell dgvCbbCell = new DataGridViewComboBoxCell();
                for (int conIdx = 0; conIdx < dgvCondition.RowCount; conIdx++)
                {
                    Console.WriteLine($"dgvResult[{(int)ColResultVar.DataType}, {rowIdx}]: {dgvResult[(int)ColResultVar.DataType, rowIdx].Value}");
                    Console.WriteLine($"dgvCondition[{(int)ColCondiVar.DataType}, {conIdx}]: {dgvCondition[(int)ColCondiVar.DataType, conIdx].Value}");
                    if (dgvResult[(int)ColResultVar.DataType, rowIdx].Value.ToString() == dgvCondition[(int)ColCondiVar.DataType, conIdx].Value.ToString())
                    {
                        dgvCbbCell.Items.Add(dgvCondition[(int)ColCondiVar.CallName, conIdx].Value.ToString());
                    }
                }
                dgvResult[colIdx, rowIdx] = dgvCbbCell;
                if (dgvCbbCell.Items.Contains(spec))
                    dgvResult[colIdx, rowIdx].Value = spec;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void DgvResult_MouseClick(object sender, MouseEventArgs e)
        {
            //bool isEditBox = false;
            //foreach (DataGridViewCell cell in dgvResult.SelectedCells)
            //    if (cell.ColumnIndex <= (int)ColResultVar.CallName)
            //        isEditBox = true;

            //if (isEditBox) Dgv_EndEdit();
        }
        private void Dgv_EndEdit()
        {
            dgvResult.EndEdit();
            dgvResult.ReadOnly = true;
            Console.WriteLine("DgvResult: End Edit");
        }
        #endregion

        private void TbCtrl_SelectIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (tbCtrlVariables.SelectedIndex)
                {
                    case 0:
                        DgvFocused = dgvCondition;
                        break;
                    case 1:
                        DgvFocused = dgvTemporary;
                        break;
                    case 2:
                        DgvFocused = dgvResult;
                        break;
                }
                Console.WriteLine("DgvFocused: " + DgvFocused.Name);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
    }
    public enum ColCondiVar
    {
        ShowName = 0,
        CallName,
        DataType,
        EditType,
        Value,
        Enum
    }
    public enum ColTempVar
    {
        ShowName = 0,
        CallName,
        DataType
    }
    public enum ColResultVar
    {
        ShowName = 0,
        CallName,
        DataType,
        SpecMin,
        SpecMax
    }
}
