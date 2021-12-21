using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.Dialogs;
using Self_Inspection_III.SI;
using Self_Inspection_III.Style;
using Self_Inspection_III.TestCommands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Self_Inspection_III.SP
{
    public partial class UcSIProgram : UserControl
    {
        #region Variables Declaration
        private bool isLoading = false;
        private bool isRun = false;
        private bool isStop = true;
        private bool RootMode { get => ((Form1)Parent.Parent.Parent).isRootMode; }

        private SISystem SISystem;
        private List<DataGridViewRow> CopiedDgvRows = new List<DataGridViewRow>();
        private List<SPItem> copiedItems = new List<SPItem>();

        //測試使用參數
        private Dictionary<string, CviVisaCtrl> _NIDriver = new Dictionary<string, CviVisaCtrl>() { { "*", null } };

        //Database
        private ProgramDB ProgramDB = new ProgramDB();
        private ItemDB ItemDB = new ItemDB();
        private ResultDB ResultDB = new ResultDB();

        #endregion

        #region Constructor
        public UcSIProgram(SISystem SISystem)
        {
            isLoading = true;

            InitializeComponent();
            Dock = DockStyle.Fill;
            this.SISystem = SISystem;

            isLoading = false;
        }
        public void UcSIProgram_Load(object sender, EventArgs e)
        {
            isLoading = true;

            SetButtonStyle();
            CreateDirectories();

            SetTscbbStationList();

            //ItemDB.ModifyID();

            isLoading = false;

            try
            {
                tscbbStation.Text = SISystem.Setting.ThisStation;
                ThisStation = ProgramDB.GetStation(tscbbStation.Text);
                if (dgv_Program.RowCount > 0)
                {
                    for (int i = 0; i < dgv_Program.Rows.Count; i++) dgv_Program.Rows[i].Selected = i == 0;
                    DgvProgram_CellClick(dgv_Program[1, 0], new DataGridViewCellEventArgs(1, 0));
                }
            }
            catch (Exception ex) { WriteLine(ex.ToString()); }
        }
        private void SetButtonStyle()
        {
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item.GetType() == typeof(ToolStripLabel) && item.Tag != null)
                {
                    StyleSetting(item);
                }
            }
            foreach (ToolStripItem item in toolStrip2.Items)
            {
                if (item.GetType() == typeof(ToolStripLabel) && item.Tag != null)
                {
                    StyleSetting(item);
                }
            }

            void StyleSetting(ToolStripItem item)
            {
                item.MouseHover += new EventHandler(TsbtnStyle.MouseHover);
                item.MouseLeave += new EventHandler(TsbtnStyle.MouseLeave);
                item.MouseEnter += new EventHandler(TsbtnStyle.MouseEnter);
                item.MouseMove += new MouseEventHandler(TsbtnStyle.MouseMove);
                item.MouseDown += new MouseEventHandler(TsbtnStyle.MouseDown);
                item.MouseUp += new MouseEventHandler(TsbtnStyle.MouseUp);
            }
        }
        private void SetTscbbStationList()
        {
            tscbbStation.Items.Clear();
            foreach (DataRow row in ProgramDB.GetStationList().Rows)
            {
                WriteLine(row[0].ToString());
                tscbbStation.Items.Add(row[0].ToString());
            }

            WriteLine("==SetTscbbStationList Done==");
        }
        private void SetDgvSIProgram()
        {
            dgv_Program.Rows.Clear();
            foreach (SPItem spi in ThisStation.Items)
            {
                try
                {
                    foreach (TestItem ti in spi.TestItems)
                        ti.Value = string.Empty;
                    dgv_Program.Rows.Add(new object[] { spi.Active, spi.Name, string.Empty, string.Empty, string.Empty });
                }
                catch { }
            }
            if (dgv_Program.RowCount > 0)
                dgv_Program.Rows[0].Selected = true;

            dgvSPDeviceList.Rows.Clear();
            foreach (Device device in ThisStation.Devices)
                dgvSPDeviceList.Rows.Add(new object[] { device.Active, device.ShowName, device.ModelName, device.No, device.Interface, device.Address });

            dgv_Parameters.Rows.Clear();
            dgv_Results.Rows.Clear();

            if (dgv_Program.RowCount > 0)
                DgvProgram_Click(dgv_Program[0, 0], new EventArgs());
        }
        private void CreateDirectories()
        {
            if (Directory.Exists(Paths.Local_bin) == false)
                Directory.CreateDirectory(Paths.Local_bin);
        }
        #endregion

        #region Attributes
        private Station m_ThisStation;
        private int ItemSlctIdx = 0;
        public Station ThisStation { get => m_ThisStation; set => m_ThisStation = value; }
        public SPItem ThisSPItem
        {
            get
            {
                for (int i = ItemSlctIdx; i >= 0; i--)
                    try { return ThisStation.Items[i]; } catch { }
                throw new Exception("There's no SPItem");
            }
            set => ThisStation.Items[ItemSlctIdx] = value;
        }
        public TabControl TcRight { get => tabControl1; set => tabControl1 = value; }
        public TabPage TpLog { get => tpLog; set => tpLog = value; }
        #endregion

        #region Go
        private bool Go(out string StopMsg)
        {
            #region 檢查待測物數量，數量為0時 不執行
            //待測清單數量為0
            if (dgv_Program.RowCount == 0)
            {
                StopMsg = "There is no test item!";
                return true;
            }
            #endregion

            DeviceInfo driverInfo = new DeviceInfo();
            DlgEmployeeID dlgEmp_ID = new DlgEmployeeID(SISystem.Setting.CheckEmployeeID);

            StopMsg = string.Empty;
            if (RootMode || dlgEmp_ID.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Invoke(new Action(() =>
                    {
                        tabControl1.SelectedIndex = (int)SPTabPage.Results;
                        for (int i = 0; i < dgv_Program.RowCount; i++) dgv_Program.Rows[i].Selected = false;
                        ClearStatusAndValue(true);
                    }));

                    #region Run Items
                    WriteLine("::Run Items::");
                    if (_NIDriver.Count > 0) _NIDriver.Clear();
                    if (_NIDriver.Count == 0) _NIDriver.Add("*", null);

                    ColorText PF = ColorText.Pass; bool uploadSucceed = true;
                    for (int itemIdx = 0; itemIdx < dgv_Program.RowCount; itemIdx++)
                    {
                        PF = ColorText.Pass;
                        if (isStop) { StopMsg = "Program Stop!"; return isStop; }
                        if (!(bool)dgv_Program[(int)ColSPProgram.Active, itemIdx].Value) continue;
                        Invoke(new Action(() =>
                        {
                            if (itemIdx > 0)
                                dgv_Program.Rows[itemIdx - 1].Selected = false;
                            dgv_Program.Rows[itemIdx].Selected = true;
                            DgvProgram_CellClick(null, new DataGridViewCellEventArgs(1, itemIdx));
                            ClearStatusAndValue(false);
                        }));

                        Item thisItem = ItemDB.GetItem(ThisStation.Name, dgv_Program[(int)ColSPProgram.ItemName, itemIdx].Value.ToString());
                        Invoke(new Action(() =>
                        {
                            dgv_Program[(int)ColSPProgram.TestTime, itemIdx].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            dgv_Program[(int)ColSPProgram.EmployeeID, itemIdx].Value = RootMode ? "TestID" : dlgEmp_ID.EmpID;
                            InitProgressbar(ItemDB.GetFuncCount(thisItem.ID));
                        }));
                        void SetResultValue(ResultVar rv)
                        {
                            int rIdx = thisItem.Vars.Results.FindIndex(x => x.ShowName == rv.ShowName);
                            Invoke(new Action(() =>
                            {
                                WriteLine($"Set Result: {thisItem.Vars.Results[rIdx].ShowName} ({rv.Value})");
                                dgv_Results[(int)ColResults.Value, rIdx].Value = rv.Value;
                                dgv_Results[(int)ColResults.Value, rIdx].Style = new DataGridViewCellStyle() { ForeColor = rv.PF.Color };
                            }));
                        }

                        #region Set Condition vars
                        foreach (ConditionVar cv in ThisStation.Items[itemIdx].Conditions)
                        {
                            if (isStop) { StopMsg = "Program Stop!"; return isStop; }

                            int ConIdx = thisItem.Vars.Conditions.FindIndex(x => x.ShowName == cv.ShowName);
                            WriteLine($"{thisItem.Vars.Conditions[ConIdx].ShowName}: {thisItem.Vars.Conditions[ConIdx].Value} --> {cv.Value}");
                            thisItem.Vars.Conditions[ConIdx].Value = cv.Value;
                            Invoke(new Action(() =>
                            {
                                thisItem.Source = cbbParaSource.Text;
                                thisItem.Meter = cbbParaMeter.Text;
                            }));

                            if (thisItem.Vars.Results.Exists(x => x.SpecMin.CallName == thisItem.Vars.Conditions[ConIdx].CallName))
                            {
                                int ResIdx = thisItem.Vars.Results.FindIndex(x => x.SpecMin.CallName == thisItem.Vars.Conditions[ConIdx].CallName);
                                thisItem.Vars.Results[ResIdx].SpecMin.Value = thisItem.Vars.Conditions[ConIdx].Value;
                                WriteLine($"Result: {thisItem.Vars.Results[ResIdx].SpecMin.CallName} --> {thisItem.Vars.Results[ResIdx].SpecMin.Value}");

                                try
                                {
                                    ThisSPItem.TestItems[ThisSPItem.TestItems.FindIndex(x => x.Name == thisItem.Vars.Results[ResIdx].ShowName)].Min = Convert.ToDouble(thisItem.Vars.Results[ResIdx].SpecMin.Value);
                                }
                                catch { ThisSPItem.TestItems[ThisSPItem.TestItems.FindIndex(x => x.Name == thisItem.Vars.Results[ResIdx].ShowName)].Min = double.NegativeInfinity; }
                            }
                            if (thisItem.Vars.Results.Exists(x => x.SpecMax.CallName == thisItem.Vars.Conditions[ConIdx].CallName))
                            {
                                int ResIdx = thisItem.Vars.Results.FindIndex(x => x.SpecMax.CallName == thisItem.Vars.Conditions[ConIdx].CallName);
                                thisItem.Vars.Results[ResIdx].SpecMax.Value = thisItem.Vars.Conditions[ConIdx].Value;
                                WriteLine($"Result: {thisItem.Vars.Results[ResIdx].SpecMax.CallName} --> {thisItem.Vars.Results[ResIdx].SpecMax.Value}");

                                try
                                {
                                    ThisSPItem.TestItems[ThisSPItem.TestItems.FindIndex(x => x.Name == thisItem.Vars.Results[ResIdx].ShowName)].Max = Convert.ToDouble(thisItem.Vars.Results[ResIdx].SpecMax.Value);
                                }
                                catch { ThisSPItem.TestItems[ThisSPItem.TestItems.FindIndex(x => x.Name == thisItem.Vars.Results[ResIdx].ShowName)].Max = double.PositiveInfinity; }
                            }
                        }
                        ItemVars tempVars = thisItem.Vars;
                        #endregion

                        #region Show Devices Used Message
                        List<string> deviceUsed = new List<string>();
                        StringBuilder sbMsg = new StringBuilder($"Testing {ThisSPItem.UUT}\n\nMake sure below models are linked:");
                        foreach (SI_TestFunction sitf in thisItem.TestFunctions)
                        {
                            if (!sitf.Active) continue;
                            string tempModel = sitf.Device;
                            if (tempModel == "Source") tempModel = ThisSPItem.Source;
                            else
                            if (tempModel == "Meter") tempModel = ThisSPItem.Meter;

                            if (!deviceUsed.Exists(x => x == tempModel))
                            {
                                foreach (Device device in ThisStation.Devices)
                                {
                                    if (tempModel == $"{device.ModelName}-{device.No}")
                                    {
                                        sbMsg.Append($"\n{device.ShowName} ({device.Address})");
                                        deviceUsed.Add(tempModel);
                                        break;
                                    }
                                }
                            }
                        }
                        sbMsg.Append($"\n\nY: Continue / N: Stop testing");
                        DialogResult result = MessageBox.Show(sbMsg.ToString(), "System", MessageBoxButtons.YesNo);
                        if (result != DialogResult.Yes) { StopMsg = $"Program Stop!"; return true; }
                        Console.WriteLine("DialogResult: " + result.ToString());
                        #endregion

                        #region 建立Driver List

                        #region Loading start
                        LoadingGIF loading = null;
                        new Thread((ThreadStart)delegate
                        {
                            loading = new LoadingGIF { Text = "Check connecting..." };
                            Application.Run(loading);
                        }).Start();
                        #endregion

                        WriteLine("::建立Driver List::");

                        bool DeviceConnectFail = false;
                        StringBuilder ConnectFailMsg = new StringBuilder("Device not found: ");
                        try
                        {
                            for (int i = 0; i < dgvSPDeviceList.RowCount; i++)
                            {
                                if (isStop) { StopMsg = "Program Stop!"; return isStop; }
                                string key = $"{dgvSPDeviceList[(int)ColSPDevice.ModelName, i].Value}-{dgvSPDeviceList[(int)ColSPDevice.No, i].Value}";
                                ColorText connectResult;
                                if (deviceUsed.Exists(x => x == key))
                                {
                                    connectResult = CheckConnect(i, ref driverInfo, ref ConnectFailMsg);
                                    WriteLine($"{key}: {connectResult.Text}");
                                    Thread.Sleep(500);
                                }
                                else if (_NIDriver.ContainsKey(key)) { connectResult = ColorText.Pass; }
                                else { connectResult = ColorText.None; }

                                dgvSPDeviceList[(int)ColSPDevice.Connect, i].Value = connectResult.Text;
                                dgvSPDeviceList[(int)ColSPDevice.Connect, i].Style = new DataGridViewCellStyle() { ForeColor = connectResult.Color };

                                if (connectResult.Text == ColorText.Fail.Text) DeviceConnectFail = true;
                            }
                            if (DeviceConnectFail) throw new Exception("Device Connect Fail!");
                        }
                        catch (Exception ex)
                        {
                            if (DeviceConnectFail) MessageBox.Show(ConnectFailMsg.ToString(), ex.Message);
                            WriteLine(ex.ToString());
                            //Email.SendErrorEmail($"=Run()@{tscbbStation.Text}=", ex.Message, ex.ToString());
                            StopMsg = ex.Message;
                            return false;
                        }
                        finally { try { loading.Invoke((EventHandler)delegate { loading.Close(); }); } catch { } }
                        #endregion

                        #region Do Functions
                        int FunctionCount = 0;
                        for (int funcIdx = 0; funcIdx < thisItem.TestFunctions.Count; funcIdx = tempVars.NextIdx)
                        {
                            SI_TestFunction func = thisItem.TestFunctions[funcIdx];

                            if (isStop) { StopMsg = $"Program Stop! (Item Func. {funcIdx})"; return isStop; }
                            if (!func.Active) { tempVars.NextIdx++; continue; }

                            WriteLine($"Do: {func.TestCommand} ({func.Parameter})");

                            #region Find Device
                            CviVisaCtrl niDriver = _NIDriver["*"];
                            try
                            {
                                switch (func.Device)
                                {
                                    case "Source":
                                        niDriver = _NIDriver[thisItem.Source];
                                        func.Device = thisItem.Source;
                                        break;
                                    case "Meter":
                                        niDriver = _NIDriver[thisItem.Meter];
                                        func.Device = thisItem.Meter;
                                        break;
                                    default:
                                        if (!string.IsNullOrEmpty(func.Device))
                                            niDriver = _NIDriver[func.Device];
                                        break;
                                }
                            }
                            catch (KeyNotFoundException knfEx)
                            {
                                Console.WriteLine(knfEx.ToString());
                                MessageBox.Show($"{func.Device} not connected.\nPlease check.");
                            }
                            #endregion

                            if (DeviceDB.GetType(func.Device) == DeviceTypes.IO_Card)
                            {
                                if (TestCommand.DoIOFunction(func, IOCardDB.Get_IOCard(func.Device), ref tempVars))
                                {
                                    PF = ColorText.Error;
                                    StopMsg = $"Function Error:\n {func.TestCommand}({func.Parameter}) at line {funcIdx}";
                                    return false;
                                }
                            }
                            else if (TestCommand.DoFunction(func, niDriver, ref tempVars))
                            {
                                PF = ColorText.Error;
                                StopMsg = $"Function Error:\n {func.TestCommand}({func.Parameter}) at line {funcIdx}";
                                return false;
                            }

                            thisItem.Vars = tempVars;

                            foreach (ResultVar rv in thisItem.Vars.Results)
                            {
                                if (string.IsNullOrEmpty(rv.Value)) continue;
                                if (isStop) { StopMsg = "Program Stop!"; return isStop; }
                                SetResultValue(rv);
                                if (rv.PF == ColorText.Fail)
                                {
                                    ThisStation.Items[itemIdx] = thisItem.SPItem;
                                    ThisStation.Items[itemIdx].TestTime = dgv_Program[(int)ColSPProgram.TestTime, itemIdx].Value.ToString();
                                    ThisStation.Items[itemIdx].Employee = dgv_Program[(int)ColSPProgram.EmployeeID, itemIdx].Value.ToString();
                                    ThisStation.Items[itemIdx].PF = PF;
                                    Invoke(new Action(() => { ThisStation.Items[itemIdx].UUT = cbbParaTestModel.Text; }));
                                    ResultDB.Write(ThisStation.Name, ThisStation.Items[itemIdx], RootMode);
                                    StopMsg = $"Test Item: {rv.ShowName} Fail!"; return true;
                                }
                            }

                            if (tempVars.NextIdx == thisItem.TestFunctions.Count + 1) //TestPass
                            { break; }
                            else
                            if (tempVars.NextIdx == thisItem.TestFunctions.Count + 2) //TestFail
                            { isStop = true; break; }

                            FunctionCount++;
                            Invoke(new Action(() => { if (progressBar1.Value < progressBar1.Maximum) progressBar1.PerformStep(); }));
                            Thread.Sleep(200);
                        }

                        #region Write Function Counts
                        if (ItemDB.WriteFuncCount(thisItem.ID, FunctionCount))
                        {
                            if (RootMode)
                                MessageBox.Show($"ItemDB.WriteFuncCount({ThisStation.Name}, {thisItem.Name}, {FunctionCount}) Failed", "Error");
                        }
                        else if (RootMode)
                            MessageBox.Show($"ItemDB.WriteFuncCount({ThisStation.Name}, {thisItem.Name}, {FunctionCount}) Success", "System");
                        #endregion

                        #endregion

                        #region Check Spec
                        if (isStop == false)
                        {
                            for (int i = 0; i < thisItem.Vars.Results.Count; i++)
                            {
                                if (isStop) { StopMsg = "Program Stop!"; return isStop; }

                                thisItem.Vars.Results[i].SpecMin.Value = tempVars.Conditions[tempVars.Conditions.FindIndex(x => x.CallName == thisItem.Vars.Results[i].SpecMin.CallName)].Value;
                                thisItem.Vars.Results[i].SpecMax.Value = tempVars.Conditions[tempVars.Conditions.FindIndex(x => x.CallName == thisItem.Vars.Results[i].SpecMax.CallName)].Value;

                                ResultVar rv = thisItem.Vars.Results[i];

                                WriteLine($"Check: {rv.ShowName} ({rv.Value}) SpecMin: {rv.SpecMin.Value} SpecMax: {rv.SpecMax.Value}");
                                if (rv.PF == ColorText.Fail) PF = rv.PF;
                                SetResultValue(rv);
                            }
                        }
                        #endregion

                        Invoke(new Action(() =>
                        {
                            dgv_Program[(int)ColSPProgram.PF, itemIdx].Value = PF.Text;
                            dgv_Program[(int)ColSPProgram.PF, itemIdx].Style = new DataGridViewCellStyle() { ForeColor = PF.Color };
                        }));

                        ThisStation.Items[itemIdx] = thisItem.SPItem;
                        ThisStation.Items[itemIdx].TestTime = dgv_Program[(int)ColSPProgram.TestTime, itemIdx].Value.ToString();
                        ThisStation.Items[itemIdx].Employee = dgv_Program[(int)ColSPProgram.EmployeeID, itemIdx].Value.ToString();
                        ThisStation.Items[itemIdx].PF = PF;
                        Invoke(new Action(() => { ThisStation.Items[itemIdx].UUT = cbbParaTestModel.Text; }));

                        if (PF != ColorText.Error)
                            if (ResultDB.Write(ThisStation.Name, ThisStation.Items[itemIdx], RootMode))
                                uploadSucceed = false;
                    }
                    #endregion

                    if (uploadSucceed)
                    {
                        StopMsg = "Inspection Done!\nThank you :)";
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Test Result upload failed!", "Error");
                        if (!MsgBox.titles.Exists(x => x == "Error")) MsgBox.titles.Add("Error");
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (RootMode)
                        msg += "\n" + ex.ToString();
                    MessageBox.Show(msg, "Error");
                    if (!MsgBox.titles.Exists(x => x == "Error")) MsgBox.titles.Add("Error");
                }
                finally
                {
                    for (int i = 0; i < dgvSPDeviceList.RowCount; i++)
                    {
                        try { if (dgvSPDeviceList[(int)ColSPDevice.Connect, i].Value.ToString() != ColorText.Pass.Text) continue; } catch { continue; }
                        if ((bool)dgvSPDeviceList[(int)ColSPDevice.Active, i].Value)
                        {
                            string modelName = dgvSPDeviceList[(int)ColSPDevice.ModelName, i].Value.ToString();
                            string key = $"{dgvSPDeviceList[(int)ColSPDevice.ModelName, i].Value}-{dgvSPDeviceList[(int)ColSPDevice.No, i].Value}";
                            try
                            {
                                if (TestCommand.SourceOff(modelName, _NIDriver[key])) throw new Exception();
                                else
                                {
                                    dgvSPDeviceList[(int)ColSPDevice.Connect, i].Value = ColorText.Off.Text;
                                    dgvSPDeviceList[(int)ColSPDevice.Connect, i].Style.ForeColor = ColorText.Off.Color;
                                }
                            }
                            catch
                            {
                                dgvSPDeviceList[(int)ColSPDevice.Connect, i].Value = ColorText.Fail.Text;
                                dgvSPDeviceList[(int)ColSPDevice.Connect, i].Style.ForeColor = ColorText.Fail.Color;
                                MessageBox.Show($"{key} not found, unable to turn off.");
                            }
                        }
                    }
                    CloseNIDrivers();
                }
                StopMsg = "Program Stop!";
            }
            return false;
        }
        private void ClearStatusAndValue(bool isRun)
        {
            if (isRun)
            {
                //清空 DgvSPProgram
                for (int rowIdx = 0; rowIdx < dgv_Program.RowCount; rowIdx++)
                    for (int colIdx = (int)ColSPProgram.EmployeeID; colIdx <= (int)ColSPProgram.PF; colIdx++)
                        dgv_Program[colIdx, rowIdx].Value = null;

                //清空 DgvSPDevice
                for (int rowIdx = 0; rowIdx < dgvSPDeviceList.RowCount; rowIdx++)
                    dgvSPDeviceList[(int)ColSPDevice.Connect, rowIdx].Value = null;
            }
            else CloseNIDrivers();

            //清空 DgvResult
            for (int rowIdx = 0; rowIdx < dgv_Results.RowCount; rowIdx++)
                dgv_Results[(int)ColResults.Value, rowIdx].Value = null;
        }
        private void CloseNIDrivers()
        {
            try
            {
                foreach (KeyValuePair<string, CviVisaCtrl> kvp in _NIDriver)
                    try { if (kvp.Value != null) kvp.Value.Close(); } catch { }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private ColorText CheckConnect(int Index, ref DeviceInfo DriverInfo, ref StringBuilder ConnectFailMsg)
        {
            //if ((bool)dgvSPDeviceList[(int)ColSPDevice.Active, Index].Value == false) { return ColorText.None; }
            //return ColorText.Pass;
            dgvSPDeviceList[(int)ColSPDevice.Active, Index].Value = true;

            try
            {
                //DriverKey: Model-No
                string key = $"{dgvSPDeviceList[(int)ColSPDevice.ModelName, Index].Value.ToString()}-{dgvSPDeviceList[(int)ColSPDevice.No, Index].Value.ToString()}";
                if (!_NIDriver.ContainsKey(key)) _NIDriver.Add(key, new CviVisaCtrl());
                //return ColorText.Pass;

                string Interface = dgvSPDeviceList[(int)ColSPDevice.Interface, Index].Value.ToString();
                string Address = dgvSPDeviceList[(int)ColSPDevice.Address, Index].Value.ToString();

                #region Device 連線
                byte byteTerminal = 0x0A;
                DriverInfo.nBaudrate = key.Contains("7550A") ? 9600 : 38400;
                DriverInfo.Parity = SerialParity.None;
                DriverInfo.shortDataBits = 8;
                DriverInfo.StopBits = SerialStopBitsMode.One;
                DriverInfo.nInterface = NInterface(ref Interface);
                if (Interface == "Ethernet")
                    DriverInfo.strConnectInfo = Address;
                else
                    DriverInfo.strConnectInfo = Interface + Address;

                if (_NIDriver[key].Open(DriverInfo))
                    _NIDriver[key].SetEos(TerminationType.TERMCHAR_LF, byteTerminal);
                else
                {
                    WriteLine($"_NIDriver[{key}].Open() Fail");
                    MessageBox.Show(_NIDriver[key].ErrorMessage);
                    return ColorText.Fail;
                }

                switch (DeviceDB.GetType(key.Split('-')[0]))
                {
                    case DeviceTypes.Fixture:
                    case DeviceTypes.Current_Shunt:
                        WriteLine($"Fixture or Current_Shunt Pass");
                        return ColorText.Pass;
                    default:
                        if (Regex.IsMatch(key, "3458A|6030")) return ColorText.Pass;
                        break;
                }

                string modelName = dgvSPDeviceList[(int)ColSPDevice.ModelName, Index].Value.ToString();
                string ReadCom = string.Empty;
                if (!_NIDriver[key].Write("*IDN?") || !_NIDriver[key].Read(ref ReadCom))
                {
                    WriteLine($"ModelName: {modelName}\nReadCom: {ReadCom}");
                    MessageBox.Show(_NIDriver[key].ErrorMessage);
                    return ColorText.Fail;
                }
                WriteLine(key + " " + ReadCom);

                if (string.IsNullOrEmpty(ReadCom) || !ReadCom.Contains(modelName))
                {
                    WriteLine($"ModelName: {modelName}\nReadCom: {ReadCom}");
                    if (Regex.IsMatch(modelName, @"62[\d]{3}P") && Regex.IsMatch(ReadCom, @"62[\d]{3}P")) return ColorText.Pass;
                    else
                    if (Regex.IsMatch(modelName, @"62[\d]{3}H") && Regex.IsMatch(ReadCom, @"62[\d]{3}H")) return ColorText.Pass;
                    else
                    if (Regex.IsMatch(modelName, @"344.1A") && Regex.IsMatch(ReadCom, @"344.1A")) return ColorText.Pass;
                    else
                    {
                        ConnectFailMsg.Append($"\n  {key} ({Address})");
                        return ColorText.Fail;
                    }
                }
                return ColorText.Pass;
                #endregion
            }
            catch (Exception ex) { WriteLine(ex.ToString()); }
            return ColorText.Fail;

            int NInterface(ref string Intf)
            {
                switch (Intf)
                {
                    case "Ethernet":
                        return (int)InterfaceType.INTERFACE_ENET;
                    case "GPIB":
                        return (int)InterfaceType.INTERFACE_GPIB;
                    case "USB":
                        return (int)InterfaceType.INTERFACE_USB;
                    case "Comport":
                        Intf = "ASRL";
                        return (int)InterfaceType.INTERFACE_ASRL;
                    default:
                        return 0;
                }
            }
        }
        #endregion

        #region ToolStrip1 Functions
        private void TsbtnNew_Click(object sender, EventArgs e)
        {

        }
        private void TsbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dgv_Program.EndEdit();
                dgvSPDeviceList.EndEdit();
                dgv_Parameters.EndEdit();

                for (int i = 0; i < dgvSPDeviceList.RowCount; i++)
                {
                    string mn = $"{dgvSPDeviceList[(int)ColSPDevice.ModelName, i].Value}-{dgvSPDeviceList[(int)ColSPDevice.No, i].Value}";
                    int dvcIdx = ThisStation.Devices.FindIndex(x => $"{x.ModelName}-{x.No}" == mn);
                    ThisStation.Devices[dvcIdx].Active = Convert.ToBoolean(dgvSPDeviceList[(int)ColSPDevice.Active, i].Value);
                    ThisStation.Devices[dvcIdx].ShowName = dgvSPDeviceList[(int)ColSPDevice.ShowName, i].Value.ToString();
                    ThisStation.Devices[dvcIdx].Interface = dgvSPDeviceList[(int)ColSPDevice.Interface, i].Value.ToString();
                    ThisStation.Devices[dvcIdx].Address = dgvSPDeviceList[(int)ColSPDevice.Address, i].Value.ToString();
                }

                for (int i = 0; i < ThisStation.Items.Count; i++)
                    if (ThisStation.Items[i].ID == null)
                        ThisStation.Items[i].ID = ItemDB.GetID(ThisStation.Name, ThisStation.Items[i].Name);

                if (ProgramDB.Write(ThisStation))
                {
                    if (isRun)
                        WriteLine("SProgram Saved");
                    else
                        MessageBox.Show("Saved");
                }
                else MessageBox.Show("Saving failed");
            }
            catch { }
        }
        private void TsbtnItems_Click(object sender, EventArgs e)
        {
            try
            {
                DlgItemList dlgItemList = new DlgItemList(ThisStation.Name);
                if (dlgItemList.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (object si in dlgItemList.Items)
                    {
                        Item item = ItemDB.GetItem(ThisStation.Name, si.ToString());
                        dgv_Program.Rows.Add(new object[] { item.Active, item.Name, string.Empty, string.Empty, string.Empty });
                        Console.WriteLine($"ThisStation({ThisStation.Name}).Items.Add(item.SPItem({item.Name}));");
                        ThisStation.Items.Add(item.SPItem);
                    }
                    //SetDgvSIProgram();
                }
            }
            catch { }
        }
        private void TsbtnGo_Click(object sender, EventArgs e)
        {
            isRun = true;
            isStop = false;

            tsbtnSave.PerformClick();
            try
            {
                new Thread((ThreadStart)delegate
                {
                    Invoke(new Action(() => { tsbtnGo.Enabled = false; }));
                    if (Go(out string StopMsg))
                    {
                        MessageBox.Show(StopMsg, "System");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(StopMsg)) MessageBox.Show(StopMsg, "Error");
                    }
                    isStop = true;
                    isRun = false;
                    Invoke(new Action(() => { tsbtnGo.Enabled = true; }));
                }).Start();
            }
            catch { }
        }
        private void TsbtnStop_Click(object sender, EventArgs e)
        {
            isStop = true;

            //按照MessageBox的標題，找到MessageBox的視窗 
            //MsgBox.FindAndKillWindow();
        }
        #endregion

        #region ToolStrip2 Functions
        private void TscbbStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            WriteLine("==TscbbStation_SelectedIndexChanged==");
            try
            {
                ThisStation = ProgramDB.GetStation(tscbbStation.Text);
                SISystem ss = SISystem.Setting;
                ss.ThisStation = ThisStation.Name;
                SISystem.Setting = ss;
                WriteLine($"Select Station: {ThisStation.Name}");

                SetDgvSIProgram();

                tabControl1.SelectedIndex = (int)SPTabPage.Setting;
            }
            catch { }
        }
        private void TsbtnCut_Click(object sender, EventArgs e)
        {
            RowsCopy(true);
        }
        private void TsbtnCopy_Click(object sender, EventArgs e)
        {
            RowsCopy(false);
        }
        private void RowsCopy(bool isCut)
        {
            if ( CopiedDgvRows.Count > 0) CopiedDgvRows.Clear();
            if ( copiedItems.Count > 0) copiedItems.Clear();

            foreach (DataGridViewRow row in dgv_Program.SelectedRows)
            {
                try
                {
                    DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();

                    for (int i = 0; i < row.Cells.Count; i++)
                        clonedRow.Cells[i].Value = row.Cells[i].Value;

                    CopiedDgvRows.Add(clonedRow);
                    copiedItems.Add(ThisStation.Items[row.Index].Clone());
                }
                catch { }
            }
            if (isCut)
            {
                foreach (DataGridViewRow row in dgv_Program.SelectedRows)
                {
                    ThisStation.Items.RemoveAt(row.Index);
                    dgv_Program.Rows.RemoveAt(row.Index);
                }
            }
        }
        private void TsbtnPaste_Click(object sender, EventArgs e)
        {
            try
            {
                if (CopiedDgvRows.Count > 0)
                {
                    if (dgv_Program.SelectedRows.Count > 0)
                    {
                        for (int i = CopiedDgvRows.Count - 1, rIdx = dgv_Program.SelectedRows[0].Index + 1; i >= 0; i--, rIdx++)
                        {
                            dgv_Program.Rows.Insert(rIdx, CopiedDgvRows[i]);
                            ThisStation.Items.Insert(rIdx, copiedItems[i]);
                        }
                    }
                    else
                    {
                        for (int i = CopiedDgvRows.Count - 1; i >= 0; i--)
                        {
                            dgv_Program.Rows.Add(CopiedDgvRows[i]);
                            ThisStation.Items.Add(copiedItems[i]);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Selected Row");
                }
            }
            catch (Exception ex) { WriteLine(ex.ToString()); }
        }
        private void TsbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgv_Program.SelectedRows)
                {
                    ThisStation.Items.RemoveAt(row.Index);
                    dgv_Program.Rows.Remove(row);
                }
            }
            catch (Exception ex) { WriteLine(ex.ToString()); }
        }
        private void TsbtnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_Program.RowCount == 0) return;
                if (dgv_Program.SelectedRows.Count == 0) return;

                DataGridViewRow row = dgv_Program.SelectedRows[0];
                int index = row.Index;
                if (row.Index > 0)
                {
                    SPItem tempItem = ThisStation.Items[row.Index];
                    DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
                    for (int i = 0; i < row.Cells.Count; i++)
                        clonedRow.Cells[i].Value = row.Cells[i].Value;

                    ThisStation.Items.RemoveAt(index);
                    dgv_Program.Rows.Remove(row);

                    ThisStation.Items.Insert(index - 1, tempItem);
                    dgv_Program.Rows.Insert(index - 1, clonedRow);

                    for (int i = 0; i < dgv_Program.RowCount; i++)
                        dgv_Program.Rows[i].Selected = false;

                    dgv_Program.Rows[index - 1].Selected = true;
                }
            }
            catch (Exception ex) { WriteLine(ex.ToString()); }
        }
        private void TsbtnDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_Program.RowCount == 0) return;
                if (dgv_Program.SelectedRows.Count == 0) return;

                DataGridViewRow row = dgv_Program.SelectedRows[0];
                int index = row.Index;
                if (row.Index < dgv_Program.RowCount - 1)
                {
                    SPItem tempItem = ThisStation.Items[row.Index];
                    DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
                    for (int i = 0; i < row.Cells.Count; i++)
                        clonedRow.Cells[i].Value = row.Cells[i].Value;

                    ThisStation.Items.RemoveAt(index);
                    dgv_Program.Rows.Remove(row);

                    ThisStation.Items.Insert(index + 1, tempItem);
                    dgv_Program.Rows.Insert(index + 1, clonedRow);

                    for (int i = 0; i < dgv_Program.RowCount; i++)
                        dgv_Program.Rows[i].Selected = false;

                    dgv_Program.Rows[index + 1].Selected = true;
                }
            }
            catch (Exception ex) { WriteLine(ex.ToString()); }
        }
        #endregion

        #region Dgv Functions
        private void Dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                for (int i = 0; i < dgv.RowCount; i++)
                {
                    if (dgv[0, i].Selected)
                        try { dgv[0, i].Value = !(bool)dgv[0, i].Value; } catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
            }
        }
        #endregion

        #region DgvProgram Functions
        private void DgvProgram_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_Program.Rows)
            {
                try
                {
                    if (row.Selected)
                    {
                        DgvProgram_CellClick(dgv_Program[(int)ColSPProgram.ItemName, row.Index], new DataGridViewCellEventArgs((int)ColSPProgram.ItemName, row.Index));
                        return;
                    }
                }
                catch { }
            }
        }
        private void DgvProgram_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            WriteLine("==DgvProgram_CellClick==");

            ItemSlctIdx = e.RowIndex;

            // Set dgv_Parameters
            try
            {
                cbbParaTestModel.Items.Clear();
                foreach (Device device in ThisStation.Devices)
                    if (!cbbParaTestModel.Items.Contains($"{device.ModelName}-{device.No}"))
                        cbbParaTestModel.Items.Add($"{device.ModelName}-{device.No}");
                cbbParaTestModel.Text = ThisSPItem.UUT;
            }
            catch { }
            try
            {
                cbbParaSource.Items.Clear();
                foreach (string source in ThisSPItem.Sources)
                    if (!cbbParaSource.Items.Contains(source))
                        cbbParaSource.Items.Add(source);
                cbbParaSource.Text = ThisSPItem.Source;
            }
            catch { }
            try
            {
                cbbParaMeter.Items.Clear();
                foreach (string meter in ThisSPItem.Meters)
                    if (!cbbParaMeter.Items.Contains(meter))
                        cbbParaMeter.Items.Add(meter);
                cbbParaMeter.Text = ThisSPItem.Meter;
            }
            catch { }

            dgv_Parameters.Rows.Clear(); int cvIdx = 0;
            foreach (ConditionVar cv in ThisSPItem.Conditions)
            {
                try
                {
                    WriteLine($"Condition Set: {cv.ShowName}");
                    dgv_Parameters.Rows.Add(new object[] { cv.ShowName, cv.Value });
                    if (cv.EditType == EditTypes.ComboList.ToString())
                    {
                        string enumName = string.Empty;
                        DataGridViewComboBoxCell dgvCbbCell = new DataGridViewComboBoxCell();
                        foreach (string ei in cv.Enum.Split(','))
                        {
                            string[] eia = ei.Split('=');
                            dgvCbbCell.Items.Add(eia[0]);
                            if (cv.Value == eia[1]) enumName = eia[0];
                        }
                        dgv_Parameters[(int)ColParameters.Value, cvIdx] = dgvCbbCell;
                        dgv_Parameters[(int)ColParameters.Value, cvIdx].Value = enumName;
                    }
                }
                catch { }
                cvIdx++;
            }

            // Set dgv_Results
            dgv_Results.Rows.Clear(); int tiIdx = 0;
            foreach (TestItem ti in ThisSPItem.TestItems)
            {
                try
                {
                    WriteLine(XmlUtil.Serializer(typeof(TestItem), ti));
                    dgv_Results.Rows.Add(new object[] { ti.Name, ti.Value });
                    if (ti.Value != "...")
                        dgv_Results[(int)ColResults.Value, tiIdx++].Style.ForeColor = ColorText.GetPF(ti.Value, ti.Min.ToString(), ti.Max.ToString()).Color;
                }
                catch { }
            }
        }
        private void DgvParameters_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgv_Parameters[(int)ColParameters.Value, e.RowIndex].Value.ToString().Contains(",") && !dgv_Parameters[(int)ColParameters.Value, e.RowIndex].Value.ToString().Contains("\""))
            {
                DlgArrayEditor dlgArrayEditor = new DlgArrayEditor(dgv_Parameters[(int)ColParameters.Value, e.RowIndex].Value.ToString());
                if (dlgArrayEditor.ShowDialog() == DialogResult.OK)
                    dgv_Parameters[(int)ColParameters.Value, e.RowIndex].Value = dlgArrayEditor.ArrayString;
                dgv_Parameters.EndEdit();
            }
        }
        private void DgvProgram_CallValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isLoading) return;
            if (e.ColumnIndex == (int)ColSPProgram.Active)
            {
                ThisStation.Items[e.RowIndex].Active = (bool)dgv_Program[e.ColumnIndex, e.RowIndex].Value;
                Console.WriteLine($"{ThisStation.Items[e.RowIndex].Name}.Active: " + ThisStation.Items[e.RowIndex].Active);
            }
        }
        private void DgvProgram_MouseLeave(object sender, EventArgs e)
        {
            dgv_Program.EndEdit();
        }
        #endregion

        #region Dgv_Results Functions
        private void DgvResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgv_Results[(int)ColResults.Value, e.RowIndex].Value.ToString() == "...")
            {
                //展開
            }
        }
        #endregion

        #region DgvSPDeviceList Functions
        private void DgvSPDeviceList_CellMouseLeaved(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)ColSPDevice.Active)
                dgvSPDeviceList.EndEdit();
        }
        #endregion

        #region Rightside Panel Functions
        private void CbbParaTestModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteLine("==CbbParaTestModel_SelectedIndexChanged==");
            try
            {
                ThisSPItem.UUT = cbbParaTestModel.Text;
                ThisStation.Items[dgv_Program.SelectedRows[0].Index] = ThisSPItem;
            }
            catch { }

        }
        private void CbbParaSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteLine("==CbbParaSource_SelectedIndexChanged==");
            try
            {
                ThisSPItem.Source = cbbParaSource.Text;
                ThisStation.Items[dgv_Program.SelectedRows[0].Index] = ThisSPItem;
            }
            catch { }
        }
        private void CbbParaMeter_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteLine("==CbbParaMeter_SelectedIndexChanged==");
            try
            {
                ThisSPItem.Meter = cbbParaMeter.Text;
                ThisStation.Items[dgv_Program.SelectedRows[0].Index] = ThisSPItem;
            }
            catch { }
        }
        private void DgvParameters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            WriteLine("==DgvParameters_CellEndEdit==");
            try
            {
                for (int i = 0; i < dgv_Parameters.RowCount; i++)
                {
                    int ConIdx = ThisSPItem.Conditions.FindIndex(x => x.ShowName == dgv_Parameters[(int)ColParameters.Name, i].Value.ToString());
                    if (dgv_Parameters[(int)ColParameters.Value, i].GetType() == typeof(DataGridViewTextBoxCell))
                    {
                        WriteLine($"{ThisSPItem.Conditions[ConIdx].ShowName}: {ThisSPItem.Conditions[ConIdx].Value} --> {dgv_Parameters[(int)ColParameters.Value, i].Value}");
                        ThisSPItem.Conditions[ConIdx].Value = dgv_Parameters[(int)ColParameters.Value, i].Value.ToString();
                    }
                    else
                    if (dgv_Parameters[(int)ColParameters.Value, i].GetType() == typeof(DataGridViewComboBoxCell))
                    {
                        List<EnumItem> EIs = new List<EnumItem>();
                        foreach (string ei in ThisSPItem.Conditions[ConIdx].Enum.Split(','))
                            EIs.Add(new EnumItem(ei.Split('=')[0], ei.Split('=')[1]));

                        int EnumIdx = EIs.FindIndex(x => x.Name == dgv_Parameters[(int)ColParameters.Value, i].Value.ToString());
                        WriteLine($"{ThisSPItem.Conditions[ConIdx].ShowName}: {ThisSPItem.Conditions[ConIdx].Value} --> {dgv_Parameters[(int)ColParameters.Value, i].Value}");
                        ThisSPItem.Conditions[ConIdx].Value = EIs[EnumIdx].Value;
                    }
                    WriteLine($"{ThisSPItem.Conditions[ConIdx].ShowName}: {ThisSPItem.Conditions[ConIdx].Value}");
                }

                for (int i = 0; i < ThisSPItem.TestItems.Count; i++)
                {
                    Item tempItem = ItemDB.GetItem(ThisStation.Name, ThisSPItem.Name);
                    int resIdx = tempItem.Vars.Results.FindIndex(x => x.ShowName == ThisSPItem.TestItems[i].Name);
                    if (ThisSPItem.Conditions.Exists(x => x.CallName == tempItem.Vars.Results[resIdx].SpecMin.CallName))
                        tempItem.Vars.Results[resIdx].SpecMin = ThisSPItem.Conditions[ThisSPItem.Conditions.FindIndex(x => x.CallName == tempItem.Vars.Results[resIdx].SpecMin.CallName)];
                    if (ThisSPItem.Conditions.Exists(x => x.CallName == tempItem.Vars.Results[resIdx].SpecMax.CallName))
                        tempItem.Vars.Results[resIdx].SpecMax = ThisSPItem.Conditions[ThisSPItem.Conditions.FindIndex(x => x.CallName == tempItem.Vars.Results[resIdx].SpecMax.CallName)];

                    ThisSPItem.TestItems[i].Min = Convert.ToDouble(tempItem.Vars.Results[resIdx].SpecMin.Value);
                    ThisSPItem.TestItems[i].Max = Convert.ToDouble(tempItem.Vars.Results[resIdx].SpecMax.Value);
                }
            }
            catch { }
        }
        private void DgvResults_MouseHover(object sender, EventArgs e)
        {
            try
            {
                var pos = dgv_Results.PointToClient(Cursor.Position);
                var info = dgv_Results.HitTest(pos.X, pos.Y);
                //WriteLine($"info: ({info.ColumnIndex}, {info.RowIndex})");

                //tip.RemoveAll();
                if (info.RowIndex < 0 || info.ColumnIndex < 0) return;

                TestItem ti = ThisStation.Items[dgv_Program.SelectedRows[0].Index].TestItems[info.RowIndex];
                string specStr = $"Min: {ti.Min}{ti.Unit}\nMax: {ti.Max}{ti.Unit}";

                dgv_Results[(int)ColResults.Name, info.RowIndex].ToolTipText = specStr;
            }
            catch { }
        }
        private void DgvResults_MouseMove(object sender, MouseEventArgs e)
        {
            DgvResults_MouseHover(sender, null);
        }
        #endregion

        #region Progressbar
        private void InitProgressbar(int max)
        {
            WriteLine("==InitProgressbar==");
            progressBar1.Minimum = 0;
            progressBar1.Maximum = max;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
        }
        #endregion

        private void WriteLine(object line)
        {
            Console.WriteLine(line);

            if (!RootMode) return;
            if (line.ToString().Contains("xml")) return;
            try { txtLog.AppendText($"\n{line}\n"); }
            catch { Invoke(new Action(() => { txtLog.AppendText($"\n{line}\n"); })); }
        }
        public void ButtonsEnable(bool rootMode)
        {
            foreach (ToolStripLabel tsl in new ToolStripLabel[] { tsbtnNew, tsbtnItems, tsbtnCut, tsbtnCopy, tsbtnPaste, tsbtnDelete, tsbtnUp, tsbtnDown })
                tsl.Enabled = rootMode;
        }
    }

    [Serializable]
    public class ColorText
    {
        private string m_Text;
        private Color m_Color;

        public ColorText() { }
        public ColorText(string Text, Color Color)
        {
            this.Text = Text;
            this.Color = Color;
        }

        public string Text { get => m_Text; set => m_Text = value; }
        public Color Color { get => m_Color; set => m_Color = value; }

        public static ColorText GetPF(string Value, string SpecMin, string SpecMax)
        {
            if (string.IsNullOrEmpty(Value)) return None;
            try
            {
                double value = Convert.ToDouble(Value);

                if (!string.IsNullOrEmpty(SpecMin) && SpecMin != "*")
                    if (value < Convert.ToDouble(SpecMin))
                        return Fail;

                if (!string.IsNullOrEmpty(SpecMax) && SpecMax != "*")
                    if (value > Convert.ToDouble(SpecMax))
                        return Fail;

                return Pass;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Error;
            }
        }

        public static ColorText Pass = new ColorText("Pass", Color.Green);
        public static ColorText Fail = new ColorText("Fail", Color.Red);
        public static ColorText Testing = new ColorText("Testing", Color.Yellow);
        public static ColorText None = new ColorText("None", Color.Gray);
        public static ColorText Error = new ColorText("Error", Color.Red);
        public static ColorText Off = new ColorText("OFF", Color.Blue);
    }

    #region Enums
    enum SPTabPage
    {
        Setting = 0,
        Results
    }
    enum ColSPProgram
    {
        Active = 0,
        ItemName,
        EmployeeID,
        TestTime,
        PF
    }
    enum ColSPDevice
    {
        Active = 0,
        ShowName,
        ModelName,
        No,
        Interface,
        Address,
        Connect
    }
    enum ColParameters
    {
        Name = 0,
        Value
    }
    enum ColResults
    {
        Name = 0,
        Value
    }
    #endregion
}
