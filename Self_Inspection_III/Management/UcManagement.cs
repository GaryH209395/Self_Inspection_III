using Self_Inspection.Class;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using Self_Inspection_III.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Self_Inspection_III.Setting
{
    public partial class UcManagement : UserControl
    {
        #region Variables Declaration
        private bool isLoading = false;
        private bool isDgvStationListSetting = false;
        private bool isDgvDeviceList_CellDoubleClick = false;

        private const string InvalidAcquire = "Invalid Acquirement!";
        
        private List<Station> m_StationList;
        private List<DeviceType> m_DeviceList;
        private List<Interface> m_InterfaceList;
        private List<Email> m_EmailList;

        private DataGridView DgvFocused;
        private int StatSlctIdx = 9;
        private int ItemSlctIdx = 9;

        private List<DataGridViewRow> dgvRows = new List<DataGridViewRow>();

        private ProgramDB ProgramDB = new ProgramDB();
        private ItemDB ItemDB = new ItemDB();
        private DeviceDB DeviceDB = new DeviceDB();
        private SettingDB SettingDB = new SettingDB();
        #endregion

        #region Constructor
        public UcManagement(SISystem SISystem)
        {
            isLoading = true;
            InitializeComponent();
            Dock = DockStyle.Fill;
            dgvItemColPeriod.Items.AddRange(Enum.GetNames(typeof(TestPeriod)));
            isLoading = false;
        }
        private void UcManagement_Load(object sender, EventArgs e)
        {
            #region Loading start
            LoadingGIF loading = null;
            new Thread((ThreadStart)delegate
            {
                loading = new LoadingGIF { Text = "System loading..." };
                Application.Run(loading);
            }).Start();
            #endregion

            isLoading = true;
            ButtonStyleSetting();
            TpSetting_DgvSetting();

            DgvFocused = dgv_StationList;
            TpSetting_DgvClick(DgvFocused, null);

            for (int i = 0; i < StationList.Count; i++) StationList[i].Edited = false;

            isLoading = false;

            #region Loading end
            loading.Invoke((EventHandler)delegate { loading.Close(); });
            #endregion
        }

        private void ButtonStyleSetting()
        {
            foreach (ToolStripItem item in toolStrip1.Items)
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
        private void TpSetting_DgvSetting()
        {
            SettingDgv_StationList();
            SettingTscbbDeviceType();
            SettingDgv_IntfaceLst();
            SettingDgv_EmailList();
        }
        private void SettingDgv_StationList()
        {
            Console.WriteLine("==Setting DGV Station List==");
            isDgvStationListSetting = true;

            if (dgv_StationList.RowCount > 0) dgv_StationList.Rows.Clear();
            foreach (Station station in StationList)
                dgv_StationList.Rows.Add(new object[] { station.Active, station.Name.Replace('+', '&') });

            isDgvStationListSetting = false;
        }
        private void SettingTscbbDeviceType()
        {
            Console.WriteLine("==Setting Tscbb Device List==");

            if (tscbbDeviceType.Items.Count > 0) tscbbDeviceType.Items.Clear();
            foreach (DeviceType dt in DeviceList)
            {
                tscbbDeviceType.Items.Add(dt.Name);
                foreach (Device device in dt.Devices)
                    dgv_DeviceTypeList.Rows.Add(new object[] { device.ModelName });
            }
        }
        private void SettingDgv_IntfaceLst()
        {
            Console.WriteLine("==Setting Interface List==");

            if (dgv_IntfaceLst.RowCount > 0) dgv_IntfaceLst.Rows.Clear();
            foreach (Interface intf in InterfaceList)
                dgv_IntfaceLst.Rows.Add(new object[] { intf.Name, intf.AddressFormat });
        }
        private void SettingDgv_EmailList()
        {
            Console.WriteLine("==Setting Email List==");

            if (dgv_EmailList.RowCount > 0) dgv_EmailList.Rows.Clear();
            foreach (Email email in EmailList)
                dgv_EmailList.Rows.Add(new object[] { email.Active, email.Name, email.Title, email.Address });
        }
        #endregion

        #region Attributes
        public List<Station> StationList
        {
            get
            {
                if (m_StationList == null)  m_StationList = new List<Station>();
                if (m_StationList.Count == 0)
                {
                    foreach (DataRow row in ProgramDB.GetStationList().Rows)
                    {
                        Console.WriteLine($"Getting StationList: {row[0]}\n{row[1]}");
                        try { m_StationList.Add(XmlUtil.Deserialize(typeof(Station), row[1].ToString()) as Station); }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                }
                return m_StationList;
            }
            set => m_StationList = value;
        }
        public List<DeviceType> DeviceList
        {
            get
            {
                if (m_DeviceList == null)
                {
                    m_DeviceList = new List<DeviceType>();
                    foreach (string type in DeviceType.Types)
                        m_DeviceList.Add(new DeviceType(type) { Devices = DeviceDB.GetDevices(type) });
                }
                return m_DeviceList;
            }
            set => m_DeviceList = value;
        }    
        public List<Interface> InterfaceList
        {
            get
            {
                if (m_InterfaceList == null)
                    m_InterfaceList = XmlUtil.Deserialize(typeof(List<Interface>), SettingDB.GetSettingList(SettingDB.List.Interface)) as List<Interface>;
                return m_InterfaceList;
            }
            set => m_InterfaceList = value;
        }
        public List<Email> EmailList
        {
            get
            {
                if (m_EmailList == null)
                    m_EmailList = XmlUtil.Deserialize(typeof(List<Email>), SettingDB.GetSettingList(SettingDB.List.Email)) as List<Email>;
                return m_EmailList;
            }
            set => m_EmailList = value;
        }
        #endregion

        #region ToolStrip Buttons
        private void TsbtnSave_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tpStationList)
            {
                // Station List
                foreach (DataRow row in ProgramDB.GetStationList().Rows)
                    if (!StationList.Exists(x => x.Name == row[0].ToString()))
                        ProgramDB.Delete(row[0].ToString());

                for (int i = 0; i < StationList.Count; i++)
                {
                    if (StationList[i].Edited)
                    {
                        Console.WriteLine($"{StationList[i].Name}.Edited: " + StationList[i].Edited);
                        if (ProgramDB.Write(StationList[i]))
                        {
                            StationList[i].Edited = false;
                            Console.WriteLine($"{StationList[i].Name} Saved.");
                            TpSetting_DgvSetting();
                        }
                        else
                        {
                            Console.WriteLine($"{StationList[i].Name} fail to update to MySQL.");
                        }
                    }
                }
            }
            else
            if (tabControl1.SelectedTab == tpData)
            {
                // Data Setting   
                foreach (DeviceType deviceType in DeviceList)
                {
                    foreach (Device mysqlDev in DeviceDB.GetDevices(deviceType.Name))
                        if (!deviceType.Devices.Exists(x => x.ModelName == mysqlDev.ModelName))
                            DeviceDB.Delete(mysqlDev.ModelName);

                    foreach (Device device in deviceType.Devices)
                        DeviceDB.Write(device.ModelName, device.Type);
                }
                SettingDB.WriteSettingList(SettingDB.List.Interface, XmlUtil.Serializer(typeof(List<Interface>), InterfaceList));
                SettingDB.WriteSettingList(SettingDB.List.Email, XmlUtil.Serializer(typeof(List<Email>), EmailList));
            }
            else { MessageBox.Show("No tabpage selected!"); return; }
            MessageBox.Show("Saved!");
            for (int i = 0; i < StationList.Count; i++) StationList[i].Edited = false;
            ButtonStyleSetting();
        }
        private void TsbtnAdd_Click(object sender, EventArgs e)
        {
            if (DgvFocused == null) { MessageBox.Show("No Selected Sheet", "Error"); return; }
            try
            {
                switch (DgvFocused.Name)
                {
                    case "dgv_StationList":
                        DlgAddStation dlgAddSt = new DlgAddStation();
                        if (dlgAddSt.ShowDialog(Parent) == DialogResult.OK)
                        {
                            StationList.Add(dlgAddSt.NewStation);
                            dgv_StationList.Rows.Add(new object[] { dlgAddSt.NewStation.Active, dlgAddSt.NewStation.Name + "*" });
                        }
                        break;
                    case "dgv_DeviceList":
                        DlgAddStationDevice dlgasd = new DlgAddStationDevice(StationList[StatSlctIdx].Name);
                        if (dlgasd.ShowDialog(this) == DialogResult.OK)
                        {
                            Device newDevice = dlgasd.NewDevice;
                            StationList[StatSlctIdx].Devices.Add(newDevice);
                            dgv_DeviceList.Rows.Add(new object[] { true, newDevice.ShowName, newDevice.ModelName, newDevice.No, newDevice.Interface, newDevice.Address });
                            StationList[StatSlctIdx].Edited = true;
                        }
                        break;
                    default:
                        DgvFocused.Rows.Add();
                        break;
                }
                Console.WriteLine($"{DgvFocused.Name}.Rows Added");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void TsbtnCut_Click(object sender, EventArgs e)
        {
            if (DgvFocused == null) { MessageBox.Show("No Selected Sheet", "Error"); return; }
            List<int> selected = new List<int>();
            foreach (DataGridViewRow row in DgvFocused.SelectedRows)
            {
                DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();

                for (int i = 0; i < row.Cells.Count; i++)
                    clonedRow.Cells[i].Value = row.Cells[i].Value;

                dgvRows.Add(clonedRow);
                selected.Add(row.Index);
                Console.WriteLine($"{DgvFocused.Name}.Rows Copied");
            }
            foreach (int i in selected)
                DgvFocused.Rows.RemoveAt(i);
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
                //Console.WriteLine($"{clonedRow.Cells[1].Value}.ID: {StationList[StationList.FindIndex(x => x.Name == clonedRow.Cells[1].Value.ToString())].ID}");
            }
            Console.WriteLine($"{DgvFocused.Name}.Rows Copied");
        }
        private void TsbtnPaste_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRows.Count > 0)
                {
                    if (DgvFocused.Name == "dgv_StationList")
                    {
                        for (int i = 0; i < dgvRows.Count; i++)
                        {
                            int rIdx = StationList.FindIndex(x => x.Name == dgvRows[i].Cells[1].Value.ToString());
                            StationList.Add(StationList[rIdx].Clone());
                        }
                        StationList.Sort((x, y) => x.ID.CompareTo(y.ID));                        
                        dgv_StationList.Rows.Clear();
                        SettingDgv_StationList();
                    }
                    else
                    {
                        for (int i = dgvRows.Count, j = 1; i > 0; i--, j++)
                        {
                            int rIdx = DgvFocused.SelectedRows[0].Index + j;
                            DgvFocused.Rows.Insert(rIdx, dgvRows[i - 1]);
                        }
                        switch (DgvFocused.Name)
                        {
                            case "dgv_StationInfo":
                            case "dgv_ItemList":
                            case "dgv_DeviceList":
                            case "dgv_TestItems":
                                StationList[StatSlctIdx].Edited = true;
                                break;
                        }
                    }
                }
                else { MessageBox.Show("No Selected Sheet"); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void TsbtnDelete_Click(object sender, EventArgs e)
        {
            if (DgvFocused == null) { MessageBox.Show("No Selected Sheet", "Error"); return; }

            #region Inner Method
            bool ThinkTwice(object str)
            {
                DialogResult res = MessageBox.Show($"Are you sure to delete {str}?", "Warning", MessageBoxButtons.YesNo);
                Console.WriteLine($"ThinkTwice({str}): {res}");
                if (res == DialogResult.Yes) return false; else return true;
            }
            void Delete(int rIdx)
            {
                try
                {
                    int index = 0, di = 0;
                    switch (DgvFocused.Name)
                    {
                        case "dgv_StationList":
                            if (ThinkTwice("Station " + StationList[StatSlctIdx].Name)) return;
                            ProgramDB.Delete(StationList[StatSlctIdx].ID);
                            Console.WriteLine($"StationList.Remove({StationList[StatSlctIdx]}, {StationList[StatSlctIdx].ID});");
                            StationList.RemoveAt(StatSlctIdx);
                            break;
                        case "dgv_ItemList":
                            if (ThinkTwice(StationList[StatSlctIdx].Items[ItemSlctIdx] + " in " + StationList[StatSlctIdx].Name)) return;
                            StationList[StatSlctIdx].Items.RemoveAt(ItemSlctIdx);
                            StationList[StatSlctIdx].Edited = true;
                            break;
                        case "dgv_TestItems":
                            MessageBox.Show(InvalidAcquire);
                            break;
                        case "dgv_DeviceList":
                            if (ThinkTwice($"{dgv_DeviceList[(int)ColSetDeviceLst.ModelName, rIdx].Value} in {StationList[StatSlctIdx].Name}")) return;
                            di = StationList[StatSlctIdx].Devices.FindIndex(x => x.ModelName == dgv_DeviceList[(int)ColSetDeviceLst.ModelName, rIdx].Value.ToString());
                            Console.WriteLine($"{StationList[StatSlctIdx].Name}.Devices.Remove({StationList[StatSlctIdx].Devices[di].ModelName})");
                            StationList[StatSlctIdx].Devices.RemoveAt(di);
                            StationList[StatSlctIdx].Edited = true;
                            break;
                        case "dgv_DeviceTypeList":
                            if (!string.IsNullOrEmpty(tscbbDeviceType.Text) && ThinkTwice(dgv_DeviceTypeList[0, rIdx].Value)) return;
                            index = DeviceList.FindIndex(x => x.Name == tscbbDeviceType.Text);
                            di = DeviceList[index].Devices.FindIndex(x => x.ModelName == dgv_DeviceTypeList[0, rIdx].Value.ToString());
                            Console.WriteLine($"{DeviceList[index].Name}.Devices.Remove({DeviceList[index].Devices[di].ModelName})");
                            DeviceList[index].Devices.RemoveAt(di);
                            break;
                        case "dgv_IntfaceLst":
                            if (ThinkTwice(dgv_IntfaceLst[(int)ColIntfaceLst.Interface, rIdx].Value)) return;
                            index = InterfaceList.FindIndex(x => x.Name == dgv_IntfaceLst[(int)ColIntfaceLst.Interface, rIdx].Value.ToString());
                            Console.WriteLine($"InterfaceList.Remove({InterfaceList[index].Name})");
                            InterfaceList.RemoveAt(index);
                            break;
                        case "dgv_EmailList":
                            if (ThinkTwice(dgv_EmailList[(int)ColEmailLst.Name, rIdx].Value)) return;
                            index = EmailList.FindIndex(x => x.Name == dgv_EmailList[(int)ColEmailLst.Name, rIdx].Value.ToString());
                            Console.WriteLine($"EmailList.Remove({EmailList[index].Name})");
                            EmailList.RemoveAt(index);
                            break;
                        default:
                            break;
                    }
                    DgvFocused.Rows.RemoveAt(rIdx);
                }
                catch (Exception exp) { Console.WriteLine(exp.ToString()); }
            }
            #endregion

            try
            {
                if (DgvFocused.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in DgvFocused.SelectedRows) { Delete(row.Index); }
                }
                else
                {
                    foreach (DataGridViewCell cell in DgvFocused.SelectedCells) { Delete(cell.RowIndex); }
                }
                if (DgvFocused.Name == "dgv_StationList")
                {
                    dgv_StationInfo.Rows.Clear();
                    dgv_ItemList.Rows.Clear();
                    dgv_TestItems.Rows.Clear();
                    dgv_DeviceList.Rows.Clear();
                }
                else if (DgvFocused.Name == "dgv_ItemList")
                {
                    dgv_TestItems.Rows.Clear();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void TsbtnMoveUp_Click(object sender, EventArgs e)
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
                    DgvFocused.Rows[index].Selected = false;

                    DgvFocused.Rows.Insert(index - 1, dr);
                    DgvFocused.Rows[index - 1].Selected = true;
                }
            }
            catch { }
        }
        private void TsbtnMoveDown_Click(object sender, EventArgs e)
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
            catch { }
        }
        #endregion

        #region TpSetting Dgvs Common Function
        private void TpSetting_DgvClick(object sender, EventArgs e)
        {
            try
            {
                ((GroupBox)DgvFocused.Parent).BackColor = Color.White;

                DgvFocused = (DataGridView)sender;
                ((GroupBox)DgvFocused.Parent).BackColor = Color.Yellow;
                Console.WriteLine("DgvFocused: " + DgvFocused.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                //Email.SendErrorEmail("TpSetting_DgvClick", ex.Message, ex.ToString());
            }
        }
        private void TpSetting_DgvCellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        #endregion

        #region DgvStationList Function
        private void DgvStationList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 1 || e.RowIndex > StationList.Count) return;
            try
            {
                string StationName = dgv_StationList[(int)ColSetStationLst.Station, e.RowIndex].Value.ToString();
                StatSlctIdx = StationList.FindIndex(x => x.Name == StationName);
                Console.WriteLine($"StationList[{StatSlctIdx}]: " + StationList[StatSlctIdx].Name);

                #region dgvSettingInfo setting
                try
                {
                    if (dgv_StationInfo.RowCount > 0) dgv_StationInfo.Rows.Clear();
                    dgv_StationInfo.Rows.Add(new object[] { StationName, StationList[StatSlctIdx].CL, StationList[StatSlctIdx].TL, StationList[StatSlctIdx].TE, StationList[StatSlctIdx].PE });
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString() + $"\n{StationList[StatSlctIdx].Name} Exception"); }
                #endregion

                #region dgv_ItemList setting
                try
                {
                    dgv_TestItems.Rows.Clear();
                    dgv_ItemList.Rows.Clear();
                    foreach (Item item in ItemDB.GetItems(StationList[StatSlctIdx].Name))
                    {
                        try { dgv_ItemList.Rows.Add(new object[] { item.Active, item.Name, item.Period.ToString() }); } catch { }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                #endregion

                #region dgv_DeviceList setting
                try
                {
                    dgv_DeviceList.Rows.Clear();
                    foreach (Device device in StationList[StatSlctIdx].Devices)
                        dgv_DeviceList.Rows.Add(new object[]
                        { device.Active, device.ShowName, device.ModelName, device.No, device.Interface, device.Address });
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                #endregion
            }
            catch (Exception exp) { Console.WriteLine(exp.ToString()); MessageBox.Show(exp.Message); }
        }
        private void DgvStationList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"DgStationList_CellValueChanged: {isLoading}");
            if (isLoading || isDgvStationListSetting) return;
            try
            {
                StationList[StatSlctIdx].Active = (bool)dgv_StationList[(int)ColSetStationLst.Active, e.RowIndex].Value;
                StationList[StatSlctIdx].Name = (string)dgv_StationList[(int)ColSetStationLst.Station, e.RowIndex].Value;
                StationList[StatSlctIdx].Edited = true;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void DgStationList_EditedUnSaved(int index)
        {
            //dgv_StationList[(int)ColSetStationLst.Station, index].Value += "*";
        }
        #endregion

        #region DgvInformation Function
        private void DgvStationInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"DgvStationInfo_CellValueChanged: {isLoading}");
            if (isLoading) return;
            try
            {
                StationList[StatSlctIdx].Name = dgv_StationInfo[(int)ColSetInfo.Station, 0].Value.ToString();
                StationList[StatSlctIdx].CL = dgv_StationInfo[(int)ColSetInfo.CL, 0].Value.ToString();
                StationList[StatSlctIdx].TL = dgv_StationInfo[(int)ColSetInfo.TL, 0].Value.ToString();
                StationList[StatSlctIdx].TE = dgv_StationInfo[(int)ColSetInfo.TE, 0].Value.ToString();
                StationList[StatSlctIdx].PE = dgv_StationInfo[(int)ColSetInfo.PE, 0].Value.ToString();
                
                StationList[StatSlctIdx].Edited = true;

                SettingDgv_StationList();

                Console.WriteLine($"DgvStationInfo_CellValueChanged: StationList[StaSlctIdx] = {StationList[StatSlctIdx].Name};");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region DgvItemList Function
        private void DgvItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 1) return;
            if (e.ColumnIndex == (int)ColSetItemLst.ItemName) { dgv_ItemList.EndEdit(); }
            try
            {
                Item thisItem = ItemDB.GetItem(StationList[StatSlctIdx].Name, dgv_ItemList[(int)ColSetItemLst.ItemName, e.RowIndex].Value.ToString());

                #region dgv_TestItems setting
                try
                {
                    dgv_TestItems.Rows.Clear();
                    foreach (ResultVar rv in thisItem.Vars.Results)
                        dgv_TestItems.Rows.Add(new object[] { rv.ShowName.Split('(')[0], rv.SpecMin, rv.SpecMax, rv.ShowName.Split('(')[1].Split(')')[0] });
                    //foreach (TestItem ti in StationList[StatSlctIdx].Items[ItemSlctIdx].TestItems)
                    //    dgv_TestItems.Rows.Add(new object[] { ti.Name.Split('(')[0], ti.Min, ti.Max, ti.Unit });
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                #endregion

                Console.WriteLine("ItemSelected: " + StationList[StatSlctIdx].Items[ItemSlctIdx].Name);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void DgvItemList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"DgvItemList_CellValueChanged: {isLoading}");
            if (isLoading) return;
            try
            {
                StatSlctIdx = StationList.FindIndex(x => x.Name == StationList[StatSlctIdx].Name);
                StationList[StatSlctIdx].Devices.Clear();
                for (int i = 0; i < dgv_ItemList.RowCount; i++)
                {
                    StationList[StatSlctIdx].Items.Add(new SPItem()
                    {
                        Active = (bool)dgv_ItemList[(int)ColSetItemLst.Active, i].Value,
                        Name = (string)dgv_ItemList[(int)ColSetItemLst.ItemName, i].Value,
                        Period = MyEnum.GetPeriod(dgv_ItemList[(int)ColSetItemLst.Period, i].Value.ToString())
                    });
                }
                StationList[StatSlctIdx].Edited = true;
                DgStationList_EditedUnSaved(StatSlctIdx);
                Console.WriteLine($"DgvDeviceList_CellValueChanged: StationList[StaSlctIdx] = StationList[StatSlctIdx];");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region DgvTestItemList Function
        private void DgvTestItem_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Console.WriteLine($"DgvTestItem_CellValueChanged: {isLoading}");
            //if (isLoading) return;
            //try
            //{
            //    int index = StationList[StaSlctIdx].Items.FindIndex(x => x == ItemSelected.Name);
            //    ItemSelected.TestItems.Clear();
            //    for (int i = 0; i < dgv_TestItems.RowCount; i++)
            //    {
            //        ItemSelected.TestItems.Add(new TestItem());
            //        ItemSelected.TestItems[i].Name = (string)(dgv_TestItems[(int)ColSetTestItem.TestItem, i].Value ?? string.Empty);
            //        ItemSelected.TestItems[i].Min = Convert.ToDouble(dgv_TestItems[(int)ColSetTestItem.SpecMin, i].Value ?? 0);
            //        ItemSelected.TestItems[i].Max = Convert.ToDouble(dgv_TestItems[(int)ColSetTestItem.SpecMax, i].Value ?? 0);
            //        ItemSelected.TestItems[i].Unit = (string)(dgv_TestItems[(int)ColSetTestItem.Unit, i].Value ?? string.Empty);
            //    }
            //    StationList[StaSlctIdx].Items[index] = ItemSelected.Name;
            //    StationList[StaSlctIdx].Edited = true;
            //    Console.WriteLine($"DgvTestItem_CellValueChanged: StationList[StaSlctIdx].Items[index] = ItemSelected");
            //}
            //catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region DgvDeviceList Function
        private void DgvDeviceList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"DgvDeviceList_CellValueChanged: {isLoading}");
            if (isLoading || isDgvDeviceList_CellDoubleClick) return;
            try
            {
                StationList[StatSlctIdx].Devices.Clear();
                for (int i = 0; i < dgv_DeviceList.RowCount; i++)
                {
                    StationList[StatSlctIdx].Devices.Add(new Device()
                    {
                        Active = (bool)(dgv_DeviceList[(int)ColSetDeviceLst.Active, i].Value ?? false),
                        ShowName = (string)(dgv_DeviceList[(int)ColSetDeviceLst.ShowName, i].Value ?? string.Empty),
                        ModelName = (string)(dgv_DeviceList[(int)ColSetDeviceLst.ModelName, i].Value ?? string.Empty),
                        No = (string)dgv_DeviceList[(int)ColSetDeviceLst.No, i].Value ?? "01",
                        Station = StationList[StatSlctIdx].Name,
                        Type = DeviceDB.GetType(dgv_DeviceList[(int)ColSetDeviceLst.ModelName, i].Value.ToString()).ToString(),
                        Interface = (string)(dgv_DeviceList[(int)ColSetDeviceLst.Interface, i].Value ?? string.Empty),
                        Address = (string)(dgv_DeviceList[(int)ColSetDeviceLst.Address, i].Value ?? string.Empty)
                    });
                }                
                StationList[StatSlctIdx].Edited = true;
                DgStationList_EditedUnSaved(StatSlctIdx);
                Console.WriteLine($"DgvDeviceList_CellValueChanged: StationList[StaSlctIdx] = StationList[StatSlctIdx];");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void DgvDeviceList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            isDgvDeviceList_CellDoubleClick = true;

            try
            {
                dgv_DeviceList.EndEdit();
                DlgAddStationDevice dlgasd = new DlgAddStationDevice(StationList[StatSlctIdx].Name);
                try
                {
                    string modelSelected = $"{dgv_DeviceList[(int)ColSetDeviceLst.ModelName, e.RowIndex].Value.ToString()}-{dgv_DeviceList[(int)ColSetDeviceLst.No, e.RowIndex].Value.ToString()}";
                    int deviceIdx = StationList[StatSlctIdx].Devices.FindIndex(x => $"{x.ModelName}-{x.No}" == modelSelected);
                    dlgasd.SetDeviceDetail(StationList[StatSlctIdx].Devices[deviceIdx]);
                    if (dlgasd.ShowDialog(this) == DialogResult.OK)
                    {
                        StationList[StatSlctIdx].Edited = true;
                        StationList[StatSlctIdx].Devices[deviceIdx] = dlgasd.NewDevice;
                        dgv_DeviceList[(int)ColSetDeviceLst.Active, e.RowIndex].Value = true;
                        dgv_DeviceList[(int)ColSetDeviceLst.ShowName, e.RowIndex].Value = StationList[StatSlctIdx].Devices[deviceIdx].ShowName;
                        dgv_DeviceList[(int)ColSetDeviceLst.ModelName, e.RowIndex].Value = StationList[StatSlctIdx].Devices[deviceIdx].ModelName;
                        dgv_DeviceList[(int)ColSetDeviceLst.No, e.RowIndex].Value = StationList[StatSlctIdx].Devices[deviceIdx].No;
                        dgv_DeviceList[(int)ColSetDeviceLst.Interface, e.RowIndex].Value = StationList[StatSlctIdx].Devices[deviceIdx].Interface;
                        dgv_DeviceList[(int)ColSetDeviceLst.Address, e.RowIndex].Value = StationList[StatSlctIdx].Devices[deviceIdx].Address;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (NullReferenceException nrex) { Console.WriteLine(nrex.ToString()); MessageBox.Show("Please Select Station!"); }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            isDgvDeviceList_CellDoubleClick = false;
        }
        #endregion

        #region DgvDeviceTypeList Function
        private bool isTypeChanging = false;
        private void TscbbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            try
            {
                dgv_DeviceTypeList.Rows.Clear();

                if (tscbbDeviceType.Text == DeviceTypes.All.ToString())
                {
                    foreach (DeviceType deviceType in DeviceList)
                        foreach (Device device in deviceType.Devices)
                            dgv_DeviceTypeList.Rows.Add(new object[] { device.ModelName });
                }
                else
                {
                    int index = DeviceList.FindIndex(x => x.Name == tscbbDeviceType.Text);
                    foreach (Device device in DeviceList[index].Devices)
                        dgv_DeviceTypeList.Rows.Add(new object[] { device.ModelName });
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void TsbtnNewDevice_Click(object sender, EventArgs e)
        {
            DlgAddDevice dlgAddDev = new DlgAddDevice(tscbbDeviceType.Text);
            if (dlgAddDev.ShowDialog(this) == DialogResult.OK)
            {
                Model newModel = dlgAddDev.NewModel;
                int index = DeviceList.FindIndex(x => x.Name == newModel.Type);
                if (DeviceList[index].Devices.Exists(x => x.ModelName == newModel.Name))
                {
                    MessageBox.Show(newModel.Name + " is already exists!");
                }
                else
                {
                    DeviceList[index].Devices.Add(new Device() { ModelName = newModel.Name, Type = newModel.Type });
                    tscbbDeviceType.SelectedIndex = tscbbDeviceType.SelectedIndex == 0 ? 1 : 0;
                    tscbbDeviceType.Text = newModel.Type;
                }
            }
        }
        private void DgvDeviceTypeList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isLoading || isTypeChanging || string.IsNullOrEmpty(tscbbDeviceType.Text)) return;

            int dtIdx = DeviceList.FindIndex(x => x.Name == tscbbDeviceType.Text);
            DeviceList[dtIdx].Devices.Clear();
            for (int i = 0; i < dgv_DeviceTypeList.RowCount; i++)
            {
                DeviceList[dtIdx].Devices.Add(new Device()
                {
                    ModelName = dgv_DeviceTypeList[0, i].Value.ToString(),
                    Type = tscbbDeviceType.Text
                });
            }
        }
        #endregion

        #region Dg_InterfaceList Function
        private void Dgv_InterfaceList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isLoading) return;
            try
            {
                InterfaceList.Clear();
                for (int i = 0; i < dgv_IntfaceLst.RowCount; i++)
                {
                    InterfaceList.Add(new Interface()
                    {
                        Name = dgv_IntfaceLst[(int)ColIntfaceLst.Interface, i].Value.ToString(),
                        AddressFormat = dgv_IntfaceLst[(int)ColIntfaceLst.AddrFormat, i].Value.ToString()
                    });
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region dgv_EmailList Function
        private void Dgv_EmailList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isLoading) return;
            try
            {
                EmailList.Clear();
                for (int i = 0; i < dgv_EmailList.RowCount; i++)
                {
                    EmailList.Add(new Email(
                         dgv_IntfaceLst[(int)ColEmailLst.Name, i].Value.ToString(),
                         dgv_IntfaceLst[(int)ColEmailLst.Title, i].Value.ToString(),
                         dgv_IntfaceLst[(int)ColEmailLst.Address, i].Value.ToString(),
                         dgv_IntfaceLst[(int)ColEmailLst.Active, i].Value.ToString()));
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion
    }

    #region Enums
    public enum ColSetStationLst
    {
        Active = 0,
        Station
    }
    public enum ColSetInfo
    {
        Station = 0,
        CL, TL, TE, PE
    }
    public enum ColSetItemLst
    {
        Active = 0,
        ItemName,
        Period
    }
    public enum ColSetTestItem
    {
        TestItem = 0,
        SpecMin,
        SpecMax,
        Unit
    }
    public enum ColSetDeviceLst
    {
        Active = 0,
        ShowName,
        ModelName,
        No,
        Interface,
        Address
    }
    public enum ColIntfaceLst
    {
        Interface = 0,
        AddrFormat
    }
    public enum ColEmailLst
    {
        Active = 0,
        Name,
        Title,
        Address
    }
    public enum ColCmdGroup
    {
        Active = 0,
        Group
    }
    public enum ColTestCmds
    {
        Active = 0,
        Command,
        Group,
        Description
    }
    #endregion
}