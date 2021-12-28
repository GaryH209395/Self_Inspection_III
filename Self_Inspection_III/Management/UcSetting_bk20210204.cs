using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Self_Inspection_III.Class;
using System.Xml.Serialization;
using System.IO;
using Self_Inspection.Class;
using System.Xml;
using System.Text.RegularExpressions;
using Self_Inspection_III.Style;
using System.Threading;

namespace Self_Inspection_III.Setting
{
    public partial class UcSetting : UserControl
    {
        #region Variables Declaration
        private bool isLoading = false;

        private const string InvalidAcquire = "Invalid Acquirement!";

        private List<Station> m_StationList;
        private List<DeviceType> m_DeviceList;
        private List<Interface> m_InterfaceList;
        private List<Email> m_EmailList;

        private DataGridView DgvFocused;
        private Station StationSelected;
        private Item ItemSelected;

        private List<DataGridViewRow> dgvRows = new List<DataGridViewRow>();

        private ProgramDB ProgramDB = new ProgramDB();
        private ItemDB ItemDB = new ItemDB();
        private SettingDB SettingDB = new SettingDB();
        #endregion

        #region Constructor
        public UcSetting()
        {
            isLoading = true;
            InitializeComponent();
            isLoading = false;
        }
        private void UcSetting_Load(object sender, EventArgs e)
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
            Console.WriteLine("==Setting DGV Station List==");
            foreach (Station station in StationList)
                dgv_StationList.Rows.Add(new object[] { station.Active, station.Name.Replace('+', '&') });

            Console.WriteLine("==Setting Cbb Device List==");
            foreach (DeviceType dt in DeviceList)
                tscbbDeviceType.Items.Add(dt.Name);

            Console.WriteLine("==Setting Interface List==");
            foreach (Interface intf in InterfaceList)
                dgv_IntfaceLst.Rows.Add(new object[] { intf.Name, intf.AddressFormat });

            Console.WriteLine("==Setting Email List==");
            foreach (Email email in EmailList)
                dgv_EmailList.Rows.Add(new object[] { email.Active, email.Name, email.Title, email.Address });
        }
        #endregion

        #region Attributes
        public List<Station> StationList
        {
            get
            {
                if (m_StationList == null)
                {
                    Console.WriteLine("==Getting Station List from MySQL==");
                    m_StationList = new List<Station>();

                    #region XmlDocument Method
                    //XmlDocument doc = new XmlDocument();
                    //doc.Load(@"\\silver\聯合製造中心\產工\ATS 資料備份\ATS User Mars\Self_Inspection(E_Request)\bin\DataList.xml");
                    //foreach (XmlNode node in doc.SelectNodes("Data_List/Station_List/Station"))
                    //{
                    //    string stName = node.Attributes["Name"].Value.ToString().Replace('+', '&');
                    //    m_StationList.Add(new Station(stName)
                    //    {
                    //        PE = node.Attributes["PE"].Value.ToString(),
                    //        CL = node.Attributes["CL"].Value.ToString(),
                    //        TL = node.Attributes["TL"].Value.ToString(),
                    //        TE = node.Attributes["TE"].Value.ToString()
                    //    });
                    //    int index = m_StationList.FindIndex(x => x.Name == stName);
                    //    XmlDocument spDoc = new XmlDocument();
                    //    spDoc.Load(@"\\silver\聯合製造中心\產工\ATS 資料備份\ATS User Mars\Self_Inspection(E_Request)\SP\" + stName.Replace('&', '+') + ".xml");
                    //    foreach (XmlNode sp in spDoc.SelectNodes("SPPara/TestProgram")) {
                    //        m_StationList[index].Items.Add(new Item()
                    //        {
                    //            Active = Convert.ToBoolean(sp.Attributes["Active"].Value),
                    //            Station = stName,
                    //            Name = $"{sp.Attributes["TestDeviceName"].Value}-{sp.Attributes["TestDeviceID"].Value}"
                    //        });
                    //    }
                    //    foreach (XmlNode intf in spDoc.SelectNodes("SPPara/lstTestInterface/TestInterface")) {
                    //        m_StationList[index].Devices.Add(new Device()
                    //        {
                    //            Active = Convert.ToBoolean(intf.Attributes["Active"].Value),
                    //            Station = stName,
                    //            ShowName = intf.Attributes["TestItem"].Value,
                    //            ModelName = intf.Attributes["TestModel"].Value,
                    //            No = intf.Attributes["TestID"].Value,
                    //            Interface = intf.Attributes["TestInterface"].Value,
                    //            Address = intf.Attributes["TestAddress"].Value
                    //        });
                    //    }
                    //}
                    #endregion

                    foreach (DataRow row in ProgramDB.GetStationList().Rows)
                    {
                        try
                        {
                            Station tempSt = XmlUtil.Deserialize(typeof(Station), row[1].ToString()) as Station;
                            try { Console.WriteLine($"Get {tempSt.Name}"); }
                            catch { tempSt = new Station(row[0].ToString()); }
                            m_StationList.Add(tempSt);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.ToString() + $"\nStation: {row[0]}"); }
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
                        m_DeviceList.Add(new DeviceType(type));

                    foreach (Station station in StationList)
                        foreach (Device device in station.Devices)
                        {
                            if (m_DeviceList.Exists(x => x.Name == device.Type) == false)
                                m_DeviceList.Add(new DeviceType(device.Type));
                            int index = m_DeviceList.FindIndex(x => x.Name == device.Type);
                            if (m_DeviceList[index].Devices.Exists(x => x.ModelName == device.ModelName) == false)
                                m_DeviceList[index].Devices.Add(device);
                        }
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
                {
                    Console.WriteLine("==Getting Interface List from MySQL==");
                    m_InterfaceList = XmlUtil.Deserialize(typeof(List<Interface>), SettingDB.GetSettingList(SettingDB.List.Interface)) as List<Interface>;

                    //m_InterfaceList = new List<Interface>() { new Interface("GPIB"), new Interface("Ethernet"), new Interface("USB"), new Interface("RS232") };
                }
                return m_InterfaceList;
            }
            set => m_InterfaceList = value;
        }
        public List<Email> EmailList
        {
            get
            {
                if (m_EmailList == null)
                {
                    Console.WriteLine("==Getting Email List from MySQL==");
                    m_EmailList = XmlUtil.Deserialize(typeof(List<Email>), SettingDB.GetSettingList(SettingDB.List.Email)) as List<Email>;

                    #region XmlDocument Method
                    //XmlDocument doc = new XmlDocument();
                    //doc.Load(@"\\silver\聯合製造中心\產工\ATS 資料備份\ATS User Mars\Self_Inspection(E_Request)\bin\DataList.xml");
                    //foreach (string title in new string[] { "MR", "CL", "TL", "SE", "PE", "TE" })
                    //{
                    //    foreach (XmlNode node in doc.SelectNodes("Data_List/Email/" + title))
                    //    {
                    //        Email tempEm = new Email(node.Attributes["Name"].Value, title, node.Attributes["Email"].Value, node.Attributes["Active"].Value);
                    //        Console.WriteLine($"Email.Add({tempEm.Name}, {tempEm.Title}, {tempEm.Address})");
                    //        if (!m_EmailList.Exists(x => x.Name == tempEm.Name))
                    //        {
                    //            m_EmailList.Add(tempEm);
                    //        }
                    //    }
                    //}
                    #endregion
                }
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
                foreach (Station st in StationList) Console.WriteLine(st.Name);
                for (int i = 0; i < dgv_StationList.RowCount; i++)
                {
                    string stName = dgv_StationList[(int)ColSetStationLst.Station, i].Value.ToString();
                    Station station = StationList[StationList.FindIndex(x => x.Name == stName)];
                    if (ProgramDB.Write(station.Name, XmlUtil.Serializer(typeof(Station), station)))
                    {
                        Console.WriteLine(station.Name + " updated from Local.");
                    }
                    else
                        Console.WriteLine($"SPTimeList ContainsKey({station.Name})\n_ProgramDB.Write() Fail...");
                }
            }
            else if (tabControl1.SelectedTab == tpData)
            {
                // Data Setting           
                SettingDB.WriteSettingList(SettingDB.List.Interface, XmlUtil.Serializer(typeof(List<Interface>), InterfaceList));
                SettingDB.WriteSettingList(SettingDB.List.Email, XmlUtil.Serializer(typeof(List<Email>), EmailList));
            }
            else { MessageBox.Show("No tabpage selected!"); return; }
            MessageBox.Show("Saved!");
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
                            m_StationList.Add(dlgAddSt.NewStation);
                            dgv_StationList.Rows.Add(new object[] { dlgAddSt.NewStation.Active, dlgAddSt.NewStation.Name + "*" });
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

                        if (tpStationList.Focused)
                        {
                            string stName = StationSelected.Name;
                            if (DgvFocused.Name == "dgv_StationList")
                                DgvStationList_EditedUnSaved(rIdx);
                        }
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

            #region Inner Method
            bool ThinkTwice(string str)
            {
                if (MessageBox.Show($"Are you sure to delete {str}?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    return true;
                else return false;
            }
            void Delete(int rIdx)
            {
                try
                {
                    int index = 0, stIdx = 0, itIdx = 0, tiIdx = 0, di = 0;
                    switch (DgvFocused.Name)
                    {
                        case "dgv_StationList":
                            stIdx = StationList.FindIndex(x => x.Name == StationSelected.Name);
                            if (ThinkTwice("Station " + StationList[stIdx].Name))
                            {
                                StationList.RemoveAt(stIdx);
                                ProgramDB.Delete(StationList[stIdx].Name);
                            }
                            break;
                        case "dgv_ItemList":
                            stIdx = StationList.FindIndex(x => x.Name == StationSelected.Name);
                            itIdx = StationList[stIdx].Items.FindIndex(x => x == dgv_ItemList[(int)ColSetItemLst.ItemName, rIdx].Value.ToString());
                            if (ThinkTwice(StationList[stIdx].Items[itIdx] + " in Station " + StationList[stIdx].Name))
                            {
                                StationList[stIdx].Items.RemoveAt(itIdx);
                                ItemDB.Delete(StationList[stIdx].Items[itIdx]);
                            }
                            break;
                        case "dgv_TestItems":
                            MessageBox.Show(InvalidAcquire);
                            break;
                        case "dgv_DeviceList":
                            stIdx = StationList.FindIndex(x => x.Name == StationSelected.Name);
                            di = StationList[stIdx].Devices.FindIndex(x => x.ModelName == dgv_DeviceList[(int)ColSetDeviceLst.ModelName, rIdx].Value.ToString());
                            Console.WriteLine($"{StationList[index].Name}.Devices.Remove({StationList[index].Devices[di].ModelName})");
                            StationList[index].Devices.RemoveAt(di);
                            break;
                        case "dgv_DeviceTypeList":
                            if (!string.IsNullOrEmpty(tscbbDeviceType.Text))
                            {
                                index = DeviceList.FindIndex(x => x.Name == tscbbDeviceType.Text);
                                di = DeviceList[index].Devices.FindIndex(x => x.ModelName == dgv_DeviceTypeList[0, rIdx].Value.ToString());
                                Console.WriteLine($"{DeviceList[index].Name}.Devices.Remove({DeviceList[index].Devices[di].ModelName})");
                                DeviceList[index].Devices.RemoveAt(di);
                            }
                            break;
                        case "dgv_IntfaceLst":
                            index = InterfaceList.FindIndex(x => x.Name == dgv_IntfaceLst[(int)ColIntfaceLst.Interface, rIdx].Value.ToString());
                            Console.WriteLine($"InterfaceList.Remove({InterfaceList[index].Name})");
                            InterfaceList.RemoveAt(index);
                            break;
                        case "dgv_EmailList":
                            index = EmailList.FindIndex(x => x.Name == dgv_EmailList[(int)ColEmailLst.Name, rIdx].Value.ToString());
                            Console.WriteLine($"EmailList.Remove({EmailList[index].Name})");
                            EmailList.RemoveAt(index);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception exp) { Console.WriteLine(exp.ToString()); }
                DgvFocused.Rows.RemoveAt(rIdx);
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
                DgvFocused = (DataGridView)sender;
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
            if (e.RowIndex < 0 || e.ColumnIndex < 1) return;
            try
            {
                string StationName = dgv_StationList[(int)ColSetStationLst.Station, e.RowIndex].Value.ToString();
                StationSelected = StationList[StationList.FindIndex(x => x.Name == StationName)];
                Console.WriteLine("StationSelected: " + StationSelected.Name);

                #region dgvSettingInfo setting
                try
                {
                    if (dgv_StationInfo.RowCount > 0) dgv_StationInfo.Rows.Clear();
                    dgv_StationInfo.Rows.Add(new object[] { StationName, StationSelected.CL, StationSelected.TL, StationSelected.TE, StationSelected.PE });
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString() + $"\n{StationSelected.Name} Exception"); }
                #endregion

                #region dgv_ItemList setting
                try
                {
                    dgv_TestItems.Rows.Clear();
                    dgv_ItemList.Rows.Clear();
                    foreach (string itName in StationSelected.Items)
                    {
                        Item item = XmlUtil.Deserialize(typeof(Item), ItemDB.Read(itName)) as Item;
                        dgv_ItemList.Rows.Add(new object[] { item.Active, item.Name, item.Period });
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                #endregion

                #region dgv_DeviceList setting
                try
                {
                    dgv_DeviceList.Rows.Clear();
                    foreach (Device device in StationSelected.Devices)
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
            Console.WriteLine($"DgvStationList_CellValueChanged: {isLoading}");
            if (isLoading) return;
            try
            {
                Station[] tempArr = new Station[dgv_StationList.RowCount];
                for (int i = 0; i < dgv_StationList.RowCount; i++)
                {
                    string stName = dgv_StationList[(int)ColSetStationLst.Station, i].Value.ToString();
                    if (StationList.Exists(x => x.Name == stName))
                    {
                        tempArr[i] = StationList[StationList.FindIndex(x => x.Name == stName)];
                        tempArr[i].Active = (bool)dgv_StationList[(int)ColSetStationLst.Active, i].Value;
                    }
                    else
                    {
                        tempArr[i] = new Station(stName) { Active = (bool)dgv_StationList[(int)ColSetStationLst.Active, i].Value };
                    }
                    Console.WriteLine($"tempArr[{i}] = {stName}");
                }
                StationList.Clear();
                StationList = tempArr.ToList();
                Console.WriteLine($"StationList = tempArr.ToList();");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void DgvStationList_EditedUnSaved(int index)
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
                int index = StationList.FindIndex(x => x.Name == StationSelected.Name);
                StationList[index].Name = dgv_StationInfo[(int)ColSetInfo.Station, 0].Value.ToString();
                StationList[index].CL = dgv_StationInfo[(int)ColSetInfo.CL, 0].Value.ToString();
                StationList[index].TL = dgv_StationInfo[(int)ColSetInfo.TL, 0].Value.ToString();
                StationList[index].TE = dgv_StationInfo[(int)ColSetInfo.TE, 0].Value.ToString();
                StationList[index].PE = dgv_StationInfo[(int)ColSetInfo.PE, 0].Value.ToString();
                StationSelected = StationList[index];
                Console.WriteLine($"DgvStationInfo_CellValueChanged: StationSelected = {StationSelected.Name};");
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
                int index = StationSelected.Items.FindIndex(x => x == dgv_ItemList[(int)ColSetItemLst.ItemName, e.RowIndex].Value.ToString());
                ItemSelected = XmlUtil.Deserialize(typeof(Item), ItemDB.Read(StationSelected.Items[index])) as Item;

                #region dgv_TestItems setting
                try
                {
                    dgv_TestItems.Rows.Clear();
                    foreach (TestItem ti in ItemSelected.TestItems)
                        dgv_TestItems.Rows.Add(new object[] { ti.Name, ti.Min, ti.Max, ti.Unit });
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                #endregion

                Console.WriteLine("ItemSelected: " + ItemSelected.Name);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void DgvItemList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
        #endregion

        #region DgvTestItemList Function
        private void DgvTestItem_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"DgvTestItem_CellValueChanged: {isLoading}");
            if (isLoading) return;
            try
            {
                int index = StationSelected.Items.FindIndex(x => x == ItemSelected.Name);
                ItemSelected.TestItems.Clear();
                for (int i = 0; i < dgv_TestItems.RowCount; i++)
                {
                    ItemSelected.TestItems.Add(new TestItem());
                    ItemSelected.TestItems[i].Name = (string)(dgv_TestItems[(int)ColSetTestItem.TestItem, i].Value ?? string.Empty);
                    ItemSelected.TestItems[i].Min = Convert.ToDouble(dgv_TestItems[(int)ColSetTestItem.SpecMin, i].Value ?? 0);
                    ItemSelected.TestItems[i].Max = Convert.ToDouble(dgv_TestItems[(int)ColSetTestItem.SpecMax, i].Value ?? 0);
                    ItemSelected.TestItems[i].Unit = (string)(dgv_TestItems[(int)ColSetTestItem.Unit, i].Value ?? string.Empty);
                }
                StationSelected.Items[index] = ItemSelected.Name;
                Console.WriteLine($"DgvTestItem_CellValueChanged: StationSelected.Items[index] = ItemSelected");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region DgvDeviceList Function
        private void DgvDeviceList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"DgvDeviceList_CellValueChanged: {isLoading}");
            if (isLoading) return;
            try
            {
                int index = StationList.FindIndex(x => x.Name == StationSelected.Name);
                StationList[index].Devices.Clear();
                for (int i = 0; i < dgv_DeviceList.RowCount; i++)
                {
                    StationList[index].Devices.Add(new Device()
                    {
                        Active = (bool)(dgv_DeviceList[(int)ColSetDeviceLst.Active, i].Value ?? false),
                        ShowName = (string)(dgv_DeviceList[(int)ColSetDeviceLst.ShowName, i].Value ?? string.Empty),
                        ModelName = (string)(dgv_DeviceList[(int)ColSetDeviceLst.ModelName, i].Value ?? string.Empty),
                        No = (string)dgv_DeviceList[(int)ColSetDeviceLst.No, i].Value ?? "01",
                        Station = StationSelected.Name,
                        Interface = (string)(dgv_DeviceList[(int)ColSetDeviceLst.Interface, i].Value ?? string.Empty),
                        Address = (string)(dgv_DeviceList[(int)ColSetDeviceLst.Address, i].Value ?? string.Empty)
                    });
                }
                StationSelected = StationList[index];
                DgvStationList_EditedUnSaved(index);
                Console.WriteLine($"DgvDeviceList_CellValueChanged: StationSelected = StationList[index];");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void DgvDeviceList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            try
            {
                dgv_DeviceList.EndEdit();
                DlgAddStationDevice dlgasd = new DlgAddStationDevice(StationSelected.Name, this);
                try
                {
                    int stationIdx = StationList.FindIndex(x => x.Name == StationSelected.Name);
                    string modelSelected = $"{dgv_DeviceList[(int)ColSetDeviceLst.ModelName, e.RowIndex].Value.ToString()}-{dgv_DeviceList[(int)ColSetDeviceLst.No, e.RowIndex].Value.ToString()}";
                    int deviceIdx = StationList[stationIdx].Devices.FindIndex(x => $"{x.ModelName}-{x.No}" == modelSelected);
                    dlgasd.SetDeviceDetail(StationList[stationIdx].Devices[deviceIdx]);
                }
                catch { }
                if (dlgasd.ShowDialog(this) == DialogResult.OK)
                {
                    dgv_DeviceList[(int)ColSetDeviceLst.Active, e.RowIndex].Value = true;
                    dgv_DeviceList[(int)ColSetDeviceLst.ShowName, e.RowIndex].Value = dlgasd.NewDevice.ShowName;
                    dgv_DeviceList[(int)ColSetDeviceLst.ModelName, e.RowIndex].Value = dlgasd.NewDevice.ModelName;
                    dgv_DeviceList[(int)ColSetDeviceLst.No, e.RowIndex].Value = dlgasd.NewDevice.No;
                    dgv_DeviceList[(int)ColSetDeviceLst.Interface, e.RowIndex].Value = dlgasd.NewDevice.Interface;
                    dgv_DeviceList[(int)ColSetDeviceLst.Address, e.RowIndex].Value = dlgasd.NewDevice.Address;
                }
            }
            catch (NullReferenceException nrex) { Console.WriteLine(nrex.ToString()); MessageBox.Show("Please Select Station!"); }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        #region DgvDeviceTypeList Function
        private void TscbbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            try
            {
                dgv_DeviceTypeList.Rows.Clear();
                int index = DeviceList.FindIndex(x => x.Name == tscbbDeviceType.Text);
                foreach (Device device in DeviceList[index].Devices)
                    dgv_DeviceTypeList.Rows.Add(new object[] { device.ModelName });
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
        #endregion

        #region DgvInterfaceList Function
        private void DgvInterfaceList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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

        #region DgvEmailList Function
        private void DgvEmailList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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
}