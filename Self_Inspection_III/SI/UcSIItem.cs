using Self_Inspection_III.Class;
using Self_Inspection_III.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Self_Inspection_III.SI
{
    public partial class UcSIItem : UserControl
    {
        #region Variables Declaration
        bool isLoading = false;
        bool isNewBtn = false;

        private string SelectedTreeNode = string.Empty;
        private List<CommandGroup> TestCmds = new List<CommandGroup>();

        private SISystem SISystem;
        
        private List<DataGridViewRow> dgvRows = new List<DataGridViewRow>();

        private ProgramDB ProgramDB = new ProgramDB();
        private ItemDB ItemDB = new ItemDB();
        private SettingDB SettingDB = new SettingDB();
        #endregion

        #region Constructor
        public UcSIItem(SISystem SISystem)
        {
            isLoading = true;

            InitializeComponent();
            Dock = DockStyle.Fill;
            this.SISystem = SISystem;

            isLoading = false;
        }
        private void UcSIItem_Load(object sender, EventArgs e)
        {
            isLoading = true;

            ButtonStyleSetting();
            TreeViewPopulate();

            isLoading = false;
        }
        private void ButtonStyleSetting()
        {
            Console.WriteLine("==ButtonStyleSetting==");

            foreach (ToolStripItem item in toolStrip1.Items) BtnStyle(item);
            foreach (ToolStripItem item in toolStrip2.Items) BtnStyle(item);
            foreach (ToolStripItem item in toolStrip3.Items) BtnStyle(item);

            void BtnStyle(ToolStripItem item)
            {
                if (item.GetType() == typeof(ToolStripLabel) && item.Tag != null)
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
        public void ChangeItem()
        {
            if (isLoading) return;
            Console.WriteLine("==ChangeItem==");

            if (tscbbItem.Items.Count > 0)
            {
                tscbbItem.SelectedIndex = 0;
                ThisItem = ItemDB.GetItem(ThisStation.Name, tscbbItem.Text);
                SetItemDetail(ThisItem);
            }
            else
            {
                tsbtnNew.PerformClick();
            }
        }
        private void SetItemDetail(Item item)
        {
            if (item == null)
            {
                tscbbItem.Text = string.Empty;
                tscbbSource.Text = string.Empty;
                tscbbMeter.Text = string.Empty;
                tscbbPeriod.Text = string.Empty;
                dgvTestFunction.Rows.Clear();
            }
            else
            {
                Console.WriteLine($"== SetItemDetail({item.Name})==");

                tscbbSource.Items.Clear();
                foreach (string src in item.Sources)
                    tscbbSource.Items.Add(src);

                tscbbMeter.Items.Clear();
                foreach (string mtr in item.Meters)
                    tscbbMeter.Items.Add(mtr);

                tscbbPeriod.Items.Clear();
                foreach (string tp in Enum.GetNames(typeof(TestPeriod)))
                    tscbbPeriod.Items.Add(tp);

                tscbbItem.Text = item.Name;
                tscbbSource.Text = item.Source;
                tscbbMeter.Text = item.Meter;
                tscbbPeriod.Text = item.Period.ToString();

                DgvTestFunction_Reset(item);
            }
        }
       #endregion

        #region Attributes
        private Station m_ThisStation;
        public Station ThisStation
        {
            get => m_ThisStation;
            set
            {
                m_ThisStation = value;
                tscbbItem.Items.Clear();
                foreach (Item item in ItemDB.GetItems(m_ThisStation.Name))
                    if (item == null) continue;
                    else
                        tscbbItem.Items.Add(item.Name);

                //if (tscbbItem.Items.Count > 0) tscbbItem.SelectedIndex = 0;
            }
        }

        private Item m_ThisItem;
        public Item ThisItem
        {
            get
            {
                if (m_ThisItem == null)
                    m_ThisItem = ItemDB.GetItem(ThisStation.Name, tscbbItem.Text);
                return m_ThisItem;
            }
            set => m_ThisItem = value;
        }

        public List<string> LabelList
        {
            get
            {
                List<string> lbllst = new List<string>();
                for (int i = 0; i < dgvTestFunction.RowCount; i++)
                    if (dgvTestFunction[(int)ColSITeFunc.Label, i].Value != null)
                        if (!lbllst.Exists(x => x == dgvTestFunction[(int)ColSITeFunc.Label, i].Value.ToString()))
                            lbllst.Add(dgvTestFunction[(int)ColSITeFunc.Label, i].Value.ToString());
                return lbllst;
            }
        }

        public ToolStripComboBox TscbbItem { get => tscbbItem; set => tscbbItem = value; }
        #endregion

        #region ToolStrip1 Items
        private void TsbtnNew_Click(object sender, EventArgs e)
        {
            Console.WriteLine("==TsbtnNew_Click==");

            isNewBtn = true;
            bool isNew = !(((ToolStripLabel)sender).Name == tsbtnEdit.Name);
            try
            {
                DlgNewItemEditor newItemEditor = new DlgNewItemEditor(isNew);

                if (!isNew)
                    newItemEditor.Setting(ThisItem);

                if (newItemEditor.ShowDialog() == DialogResult.OK)
                {
                    if (isNew)
                        ThisItem = new Item() { ID = ItemDB.NewID(ThisStation.ID) };
                    Console.WriteLine($"{ThisItem.Name}: {ThisItem.ID}");

                    ThisItem.Active = true;
                    ThisItem.Name = newItemEditor.ItemName;
                    ThisItem.Source = newItemEditor.Source;
                    ThisItem.Meter = newItemEditor.Meter;
                    ThisItem.Sources = newItemEditor.SourceList;
                    ThisItem.Meters = newItemEditor.MeterList;
                    ThisItem.Period = newItemEditor.Period;

                    tscbbItem.Text = ThisItem.Name;
                }
                else ThisItem = null;

                SetItemDetail(ThisItem);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            finally { isNewBtn = false; }
        }
        private void TsbtnSave_Click(object sender, EventArgs e)
        {
            Console.WriteLine("==TsbtnSave_Click==");

            dgvTestFunction.EndEdit();
            
            ThisItem.Name = tscbbItem.Text;
            ThisItem.Source = tscbbSource.Text;
            ThisItem.Meter = tscbbMeter.Text;
            ThisItem.Period = MyEnum.GetPeriod(tscbbPeriod.Text);
            ThisItem.Vars.Labels = new string[dgvTestFunction.RowCount];

            ThisItem.ClearNone();
            ThisItem.TestFunctions.Clear();
            for (int i = 0; i < dgvTestFunction.RowCount; i++)
            {
                for (int j = 0; j < dgvTestFunction.ColumnCount; j++)
                    if (dgvTestFunction[j, i].Value == null)
                        dgvTestFunction[j, i].Value = string.Empty;

                ThisItem.TestFunctions.Add(new SI_TestFunction(
                    dgvTestFunction[(int)ColSITeFunc.Active, i].Value.ToString(),
                    dgvTestFunction[(int)ColSITeFunc.Label, i].Value.ToString(),
                    dgvTestFunction[(int)ColSITeFunc.TestCommand, i].Value.ToString(),
                    dgvTestFunction[(int)ColSITeFunc.Device, i].Value.ToString(),
                    dgvTestFunction[(int)ColSITeFunc.Parameter, i].Value.ToString(),
                    dgvTestFunction[(int)ColSITeFunc.Desciption, i].Value.ToString()));

                ThisItem.Vars.Labels[i] = dgvTestFunction[(int)ColSITeFunc.Label, i].Value.ToString();
            }

            if (ItemDB.Write(ThisItem.ID, ThisStation.Name, ThisItem)) Console.WriteLine(ThisItem.Name + " updated to MySQL!");
        }
        private void TsbtnEdit_Click(object sender, EventArgs e)
        {
            Item oldItem = ThisItem;
            TsbtnNew_Click(sender, e);
        }
        private void TsbtnImportFunction_Click(object sender, EventArgs e)
        {
            DialogResult msgRes = MessageBox.Show("Yes: Function\nNo: Item", "Import", MessageBoxButtons.YesNo);
            DlgImportFunction dif = new DlgImportFunction();
            if (dif.ShowDialog() == DialogResult.OK)
            {
                if (msgRes == DialogResult.Yes)
                {
                    Item temp = ItemDB.GetItem(dif.StationSelected, dif.FunctionSelected);
                    ThisItem.Vars = temp.Vars;
                    DgvTestFunction_Reset(temp);
                }
                else if (msgRes == DialogResult.No)
                {
                    if (ItemDB.GetItem(ThisStation.Name, dif.FunctionSelected) == null)
                    {
                        ThisItem = ItemDB.GetItem(dif.StationSelected, dif.FunctionSelected);
                        ThisItem.ID = ItemDB.NewID(ThisStation.ID);
                        SetItemDetail(ThisItem);
                    }
                    else MessageBox.Show($"Item {dif.FunctionSelected} is already exists in {ThisStation.Name}", "System");
                }
            }
        }
        #endregion

        #region ToolStrip2 Items
        private void TscbbItem_SelectesIndexChanged(object sender, EventArgs e)
        {
            if (isLoading || isNewBtn) return;

            ThisItem = ItemDB.GetItem(ThisStation.Name, tscbbItem.Text);
            SetItemDetail(ThisItem);
        }
        private void TscbbSource__SelectesIndexChanged(object sender, EventArgs e)
        {
            if (isLoading || isNewBtn) return;
        }
        private void TscbbMeter__SelectesIndexChanged(object sender, EventArgs e)
        {
            if (isLoading || isNewBtn) return;
        }
        private void TscbbPeriod__SelectesIndexChanged(object sender, EventArgs e)
        {
            if (isLoading || isNewBtn) return;
        }
        private void TsbtnVariables_Click(object sender, EventArgs e)
        {
            DlgVariables dv = new DlgVariables(ThisItem.Vars);
            dv.ShowDialog(Parent);
            ThisItem.Vars = dv.ItemVars;
        }
        #endregion

        #region ToolStrip3 Items
        private void RowsCopy(bool isCut)
        {
            if (dgvRows != null && dgvRows.Count > 0) dgvRows.Clear();

            for (int i = 0; i < dgvTestFunction.RowCount; i++)
            {
                if (dgvTestFunction.Rows[i].Selected)
                {
                    DataGridViewRow clonedRow = (DataGridViewRow)dgvTestFunction.Rows[i].Clone();

                    for (int j = 0; j < dgvTestFunction.Rows[i].Cells.Count; j++)
                        clonedRow.Cells[j].Value = dgvTestFunction.Rows[i].Cells[j].Value;

                    dgvRows.Add(clonedRow);
                }
            }

            if (isCut)
                TsbtnDelete_Click(null, null);
        }
        private void TsbtnCut_Click(object sender, EventArgs e)
        {
            RowsCopy(true);
        }
        private void TsbtnCopy_Click(object sender, EventArgs e)
        {
            RowsCopy(false);
        }
        private void TsbtnPaste_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRows.Count > 0)
                {
                    int rIdx = dgvTestFunction.SelectedRows[0].Index;
                    foreach (DataGridViewRow row in dgvRows)
                        dgvTestFunction.Rows.Insert(rIdx++, row);
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
            try
            {
                foreach (DataGridViewRow row in dgvTestFunction.SelectedRows)
                    dgvTestFunction.Rows.Remove(row);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void TsbtnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTestFunction.RowCount == 0) return;
                if (dgvTestFunction.SelectedRows.Count == 0) return;

                DataGridViewRow row = dgvTestFunction.SelectedRows[0];
                int index = row.Index;
                if (row.Index > 0)
                {
                    DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
                    for (int i = 0; i < row.Cells.Count; i++)
                        clonedRow.Cells[i].Value = row.Cells[i].Value;
                    dgvTestFunction.Rows.Remove(row);
                    dgvTestFunction.Rows.Insert(index - 1, clonedRow);

                    for (int i = 0; i < dgvTestFunction.RowCount; i++)
                        dgvTestFunction.Rows[i].Selected = false;

                    dgvTestFunction.Rows[index - 1].Selected = true;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void TsbtnDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTestFunction.RowCount == 0) return;
                if (dgvTestFunction.SelectedRows.Count == 0) return;

                DataGridViewRow row = dgvTestFunction.SelectedRows[0];
                int index = row.Index;
                if (row.Index < dgvTestFunction.RowCount - 1)
                {
                    DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
                    for (int i = 0; i < row.Cells.Count; i++)
                        clonedRow.Cells[i].Value = row.Cells[i].Value;
                    dgvTestFunction.Rows.Remove(row);
                    dgvTestFunction.Rows.Insert(index + 1, clonedRow);

                    for (int i = 0; i < dgvTestFunction.RowCount; i++)
                        dgvTestFunction.Rows[i].Selected = false;

                    dgvTestFunction.Rows[index + 1].Selected = true;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region TreeView Functions
        private void TreeViewPopulate()
        {
            Console.WriteLine("==TreeViewPopulate==");

            tvTestCommand.Nodes.Clear();
            DirectoryInfo diTestCommands = new DirectoryInfo(@"..\..\TestCommands");
            Console.WriteLine(diTestCommands.FullName);

            //更新TestCommands TreeView
            if (diTestCommands.Exists && diTestCommands.FullName.Contains(@"Gary\Project"))
            {
                foreach (DirectoryInfo di in diTestCommands.GetDirectories())
                {
                    string key = Regex.Replace(di.Name, @"\d{2}\)", "");
                    if (!TestCmds.Exists(x=>x.Name==key)) TestCmds.Add(new CommandGroup(key));
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        int tcIdx = TestCmds.FindIndex(x => x.Name == key);
                        string tc = Regex.Replace(fi.Name, @".cs", "");
                        if (!TestCmds[tcIdx].Commands.Exists(x => x == tc))
                            TestCmds[tcIdx].Commands.Add(tc);
                    }
                }
                SettingDB.WriteSettingList(SettingDB.List.Command, XmlUtil.Serializer(typeof(List<CommandGroup>), TestCmds));
            }

            TestCmds = XmlUtil.Deserialize(typeof(List<CommandGroup>), SettingDB.GetSettingList(SettingDB.List.Command)) as List<CommandGroup>;
            foreach (CommandGroup cg in TestCmds)
            {
                TreeNode groupNode = new TreeNode(cg.Name);
                foreach (string cmd in cg.Commands)
                    groupNode.Nodes.Add(new TreeNode(cmd));

                tvTestCommand.Nodes.Add(groupNode);
            }
        }
        private void TvTestCommand_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level > 0)
                SelectedTreeNode = e.Node.Text;
        }
        private void TvTestCommand_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string ParaString = string.Empty, Device = string.Empty;
                if (TestCommands.TestCommand.ShowEditor(SelectedTreeNode, ThisItem, LabelList, ref ParaString, ThisStation.Devices, ref Device))
                {
                    SI_TestFunction sitf = new SI_TestFunction()
                    {
                        Active = true,
                        Label = string.Empty,
                        Device = Device,
                        TestCommand = SelectedTreeNode,
                        Parameter = ParaString,
                        Description = string.Empty
                    };
                    if (dgvTestFunction.SelectedRows.Count > 0)
                        DgvTestFunction_Insert(dgvTestFunction.SelectedRows[dgvTestFunction.SelectedRows.Count - 1].Index, sitf);
                    else
                        DgvTestFunction_Add(sitf);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region DataGridView Function
        private void DgvTestFunction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            #region DGV EndEdit
            dgvTestFunction.ReadOnly = true;
            dgvTestFunction.EndEdit();
            Console.WriteLine("DgvTestFunction: End Edit");
            #endregion

            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (e.ColumnIndex == (int)ColSITeFunc.Active)
            {
                dgvTestFunction.ReadOnly = false;
                dgvTestFunction.BeginEdit(false);
            }
        }
        private void DgvTestFunction_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            foreach (DataGridViewCell cell in dgvTestFunction.SelectedCells)
                if (cell.ColumnIndex == (int)ColSITeFunc.Active)
                    dgvTestFunction.Rows[cell.RowIndex].Selected = true;
        }
        private void DgvTestFunction_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == (int)ColSITeFunc.Active)
            {
                for (int i = 0; i < dgvTestFunction.RowCount; i++)
                {
                    if (dgvTestFunction[(int)ColSITeFunc.Active, i].Selected)
                        try { dgvTestFunction[(int)ColSITeFunc.Active, i].Value = !(bool)dgvTestFunction[(int)ColSITeFunc.Active, i].Value; } catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
            }
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            switch (e.ColumnIndex)
            {
                case (int)ColSITeFunc.Label:
                case (int)ColSITeFunc.Desciption:
                    dgvTestFunction.ReadOnly = false;
                    dgvTestFunction.BeginEdit(true);
                    Console.WriteLine("DgvTestFunction: Begin Edit");
                    break;
                case (int)ColSITeFunc.TestCommand:
                case (int)ColSITeFunc.Device:
                case (int)ColSITeFunc.Parameter:
                    try
                    {
                        string tCmd = dgvTestFunction[(int)ColSITeFunc.TestCommand, e.RowIndex].Value.ToString();
                        string ParaString = dgvTestFunction[(int)ColSITeFunc.Parameter, e.RowIndex].Value.ToString();
                        string Device = dgvTestFunction[(int)ColSITeFunc.Device, e.RowIndex].Value.ToString();

                        if (TestCommands.TestCommand.ShowEditor(tCmd, ThisItem, LabelList, ref ParaString, ThisStation.Devices, ref Device))
                        {
                            dgvTestFunction[(int)ColSITeFunc.Parameter, e.RowIndex].Value = ParaString;
                            dgvTestFunction[(int)ColSITeFunc.Device, e.RowIndex].Value = Device;
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                    break;
                default:
                    break;
            }
        }
        private void DgvTestFunction_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ThisItem.TestFunctions.Clear();
            for (int i = 0; i < dgvTestFunction.RowCount; i++)
                ThisItem.TestFunctions.Add(new SI_TestFunction(
                    dgvTestFunction[(int)ColSITeFunc.Active, i].Value == null ? string.Empty : dgvTestFunction[(int)ColSITeFunc.Active, i].Value.ToString(),
                    dgvTestFunction[(int)ColSITeFunc.Label, i].Value == null ? string.Empty : dgvTestFunction[(int)ColSITeFunc.Label, i].Value.ToString(),
                    dgvTestFunction[(int)ColSITeFunc.TestCommand, i].Value == null ? string.Empty : dgvTestFunction[(int)ColSITeFunc.TestCommand, i].Value.ToString(),
                    dgvTestFunction[(int)ColSITeFunc.Device, i].Value == null ? string.Empty : dgvTestFunction[(int)ColSITeFunc.Device, i].Value.ToString(),
                    dgvTestFunction[(int)ColSITeFunc.Parameter, i].Value == null ? string.Empty : dgvTestFunction[(int)ColSITeFunc.Parameter, i].Value.ToString(),
                    dgvTestFunction[(int)ColSITeFunc.Desciption, i].Value == null ? string.Empty : dgvTestFunction[(int)ColSITeFunc.Desciption, i].Value.ToString()));
        }
        private void DgvTestFunction_Reset(Item item)
        {
            dgvTestFunction.Rows.Clear();
            foreach (SI_TestFunction fn in item.TestFunctions)
                DgvTestFunction_Add(fn);
        }
        private void DgvTestFunction_Add(SI_TestFunction fn)
        {
            dgvTestFunction.Rows.Add(new object[] { fn.Active, fn.Label, fn.Device, fn.TestCommand, fn.Parameter, fn.Description });
        }
        private void DgvTestFunction_Insert(int rowIdx, SI_TestFunction fn)
        {
            dgvTestFunction.Rows.Insert(rowIdx, new object[] { fn.Active, fn.Label, fn.Device, fn.TestCommand, fn.Parameter, fn.Description });
        }
        #endregion
    }

    [Serializable]
    public class CommandGroup
    {
       public string Name { get; set; }
        public List<string> Commands;

        public CommandGroup()
        {
            Name = string.Empty;
            Commands = new List<string>();
        }

        public CommandGroup(string name)
        {
            Name = name;
            Commands = new List<string>();
        }
    }

    public enum ColSITeFunc
    {
        Active = 0,
        Label,
        Device,
        TestCommand,
        Parameter,
        Desciption
    }
}
