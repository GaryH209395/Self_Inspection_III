using Self_Inspection_III.Class;
using Self_Inspection_III.DataSearch;
using Self_Inspection_III.Dialogs;
using Self_Inspection_III.Setting;
using Self_Inspection_III.SI;
using Self_Inspection_III.SP;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Self_Inspection_III
{
    public partial class Form1 : Form
    {
        #region 變數宣告
        private bool isTest = true;
        public bool isRootMode = true;
        bool isLoading = false;
        bool isCheckVersion = false;

        string originalTitle;

        UcSIProgram ucSIProgram;
        UcSIItem ucSIItem;
        UcManagement ucManagement;
        UcDataSearch ucDataSearch;

        SISystem SISystem;

        Database Database = new Database();
        ProgramDB ProgramDB = new ProgramDB();
        VersionDB VersionDB = new VersionDB();
        SettingDB SettingDB = new SettingDB();
        #endregion

        #region 建構子
        public Form1()
        {
            isLoading = true;

            InitializeComponent();
            TopMost = true;
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            Text += $" ver.{version}";
            originalTitle = Text;

            SISystem = SISystem.Setting;

            ucManagement = new UcManagement(SISystem);
            ucSIProgram = new UcSIProgram(SISystem);
            ucSIItem = new UcSIItem(SISystem);
            ucDataSearch = new UcDataSearch(SISystem);

            isLoading = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            isLoading = true;

            #region Loading start
            LoadingGIF loading = null;
            new Thread((ThreadStart)delegate
            {
                loading = new LoadingGIF { Text = "System loading..." };
                Application.Run(loading);
            }).Start();
            #endregion

            DirectoryInfo ServerUpdateAppDir = new DirectoryInfo(@"\\silver\聯合製造中心\產工\ATS 資料備份\ATS User Mars\Self_Inspection(E_Request)\更新軟體");
            DirectoryInfo ServerUpdateFileDir = new DirectoryInfo(@"\\silver\聯合製造中心\產工\ATS 資料備份\ATS User Mars\Self_Inspection(E_Request)\Self_Inspection_III\bin\Debug");

            if (ServerUpdateAppDir.Exists)
            {
                CheckVersion_Temp(ServerUpdateAppDir, ServerUpdateFileDir);

                AddUserControls();
                tsmiModeSwtiching.PerformClick();
            }

            #region Loading end
            try { loading.Invoke((EventHandler)delegate { loading.Close(); }); } catch { }
            #endregion

            isLoading = false;

            if (!ServerUpdateAppDir.Exists)
            {
                WindowState = FormWindowState.Minimized;
                MessageBox.Show(@"\\Silver not Connected!");
                this.Close();
            }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            TopMost = false;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Database.ClearCache();
            Environment.Exit(0);
        }
        private void AddUserControls()
        {
            // SI-Progam
            tpSIProgram.Controls.Add(ucSIProgram);

            // SI-Item
            tpSIItem.Controls.Add(ucSIItem);

            // Management
            tpManagement.Controls.Add(ucManagement);

            // Data Search
            tpDataSearch.Controls.Add(ucDataSearch);
        }
        #endregion

        #region MenuStrip1 Function
        private void TsmiSystemSetting_Click(object sender, EventArgs e)
        {
            if (isRootMode == false) return;

            DlgSystemSetting dss = new DlgSystemSetting();
            if (dss.ShowDialog() == DialogResult.OK)
                SISystem.Setting = dss.SystemSetting;
        }
        private void TsmiModeSwtiching_Click(object sender, EventArgs e)
        {
            if (isRootMode)
            {
                Text = originalTitle;
                tsmiSystemSetting.Enabled = false;
                tpSIItem.Parent = null;
                tpManagement.Parent = null;
                ucSIProgram.TpLog.Parent = null;
                ucSIProgram.ButtonsEnable(false);
            }
            else
            {
                DlgRootMode dlgRootMode = new DlgRootMode();
                if (isTest || dlgRootMode.ShowDialog(this) == DialogResult.OK) 
                {
                    Text = originalTitle + " (RootMode)";
                    tsmiSystemSetting.Enabled = true;
                    tabControl1.TabPages.Insert(1, tpSIItem);
                    tabControl1.TabPages.Add(tpManagement);
                    ucSIProgram.TcRight.TabPages.Add(ucSIProgram.TpLog);
                    ucSIProgram.ButtonsEnable(true);
                }
                else return;
            }
            isRootMode = !isRootMode;
        }
        #endregion

        #region 版本檢查 & 更新 
        /**********************************************
         * Author: Gary                               *
         * Date: 2021/01                              *
         * Purpose: 版本檢查&版本更新                 *
         * Memo:                                      *
         *   版本資訊由下列四個值所組成:              *
         *   1. 主板號: 大規模架構變革                *
         *   2. 次版號: 小規模架構、方法參數型別變動  *
         *   3. 組建號: 確認為新版發行，上傳MySQL     *
         *   4. 修訂號: 修正bug、加強內部演算法       *
         **********************************************/
        private void CheckVersion_Temp(DirectoryInfo ServerUpdateAppDir, DirectoryInfo ServerUpdateFileDir)
        {
            Console.WriteLine("== CheckVersion_Temp ==");
            isCheckVersion = true;
            string Update_SI3 = "UpdateApp_SI_III";

            string version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            string[] local_ver = version.Split('.');
            string mver = SettingDB.GetSettingList(SettingDB.List.Version_Temp);
            string[] mysql_ver = (string.IsNullOrEmpty(mver) ? "0.0.0.0" : mver).Split('.');
            int lv = 0, mv = 0;

            for (int i = 0; i < 3; i++)
            {
                lv = Convert.ToInt32(local_ver[i]);
                mv = Convert.ToInt32(mysql_ver[i]);
                if (lv == mv) continue;
                else
                {
                    if (lv < mv)
                    {
                        if (ServerUpdateAppDir.Exists)
                        {
                            try { foreach (Process p in Process.GetProcesses()) if (p.ProcessName.Contains(Update_SI3)) p.Kill(); }
                            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                            foreach (FileInfo fi in ServerUpdateAppDir.GetFiles())
                                if (fi.Name.Contains(Update_SI3))
                                    fi.CopyTo(Path.Combine(@".\", fi.Name), true);

                            Process P_Update = new Process();
                            P_Update.StartInfo.FileName = Path.Combine(@".\", $"{Update_SI3}.exe");
                            P_Update.Start();
                            Close();
                        }
                    }
                    else
                    {
                        if (SettingDB.WriteSettingList(SettingDB.List.Version_Temp, version))
                            Console.WriteLine("Update version fail!");
                        else
                            Console.WriteLine("Update version success!");

                        foreach (FileInfo fi in new DirectoryInfo(Environment.CurrentDirectory).GetFiles())
                        {
                            string serverFile = Path.Combine(ServerUpdateFileDir.FullName, fi.Name);
                            Console.WriteLine($"Copy: {fi.FullName}\nTo: {serverFile}");
                            fi.CopyTo(serverFile, true);
                        }
                    }
                    break;
                }
            }
        }

        private void CheckVersion()
        {
            Console.WriteLine("--CheckVersion--");
            isCheckVersion = true;
            string sources_file = Application.ExecutablePath;
            string tmp_file = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            int ver_no = 0;
            string VersionTxt = Paths.Local_VersionTxt;

            switch (VersionDB.Read(version, ref ver_no))
            {
                //版本不變
                case 0: break;

                //MySQL版本須更新
                case 1:
                    if (!Environment.CurrentDirectory.Contains("Project")) break;
                    string backup_file = Path.Combine(Paths.Server_ExeBackUp, $"{Paths.Self_Inspection}_{ver_no}.exe");
                    Console.WriteLine("Backup File: " + backup_file);
                    DlgVersionDescription dvd = new DlgVersionDescription();
                    if (dvd.ShowDialog() == DialogResult.OK)
                    {
                        //變更舊檔名稱
                        try { File.Move(Paths.Server_SIExe, backup_file); } catch { }
                        // 將新檔複製至更新檔案夾
                        foreach (FileInfo fi in new DirectoryInfo(Environment.CurrentDirectory).GetFiles())
                        {
                            if (!fi.Name.Contains(".lnk"))
                            {
                                string serverPath = Path.Combine(Paths.Server_UpdateFile, fi.Name);
                                Console.WriteLine($"Copy {fi.FullName}\nTo {serverPath}");
                                fi.CopyTo(serverPath, true);
                            }
                        }
                        //MySQL更新
                        ver_no++;
                        VersionDB.Write(ver_no, version, dvd.VersionInfo);
                        VersionUpdate(version, ver_no);
                    }
                    break;

                //Local版本須更新
                case 2:
                    try
                    {
                        //確認可連接上伺服器
                        if (File.Exists(Paths.Server_UpdateApp))
                        {
                            //更新Local_UpdateApp
                            foreach (FileInfo file in new DirectoryInfo(Path.Combine(Paths.Server_ERequestPath, "更新軟體")).GetFiles())
                                file.CopyTo(Path.Combine(Paths.Local_debug, file.Name), true);
                            try
                            {
                                Process[] ps = Process.GetProcesses();
                                foreach (Process p in ps)
                                {
                                    if (p.ProcessName == Paths.Local_UpdateApp)
                                    {
                                        p.Kill();
                                    }
                                }
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }

                            Process P_Update = new Process();

                            // FileName 是要執行的檔案
                            P_Update.StartInfo.FileName = Paths.Local_UpdateApp;
                            P_Update.Start();

                            //關閉程式
                           Close();
                        }
                    }
                    catch
                    {
                        MessageBox.Show(
                            @"Server Unavailable!" + "\n" + "Please Check Server Connection.", @"\\silver\聯合製造中心\產工\");
                        Close();//關閉程式
                    }
                    break;
                default: break;
            }
            try
            {
                AutoBackUp(version);
                VersionUpdate(version, ver_no);
                isCheckVersion = false;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void VersionUpdate(string version, int ver_no)
        {
            try
            {
                StreamWriter str = new FileInfo(Paths.Local_VersionTxt).CreateText();
                str.WriteLine($"{version};{ver_no}");
                str.Close();
            }
            catch { }
        }
        private void AutoBackUp(string version)
        {
            //限制僅有開發者專案才需要此備份功能
            if (!Directory.Exists(@"C:\Users\gary.huang\Desktop\Gary")) return;
            //限制位於Progrma Files的測試執行檔
            if (Environment.CurrentDirectory != @"C:\Program Files (x86)\Chroma\Self_Inspection_III\Self_Inspection\bin\Debug") return;
            //檢查Version.txt是否存在
            if (File.Exists(Paths.Local_VersionTxt) == false) return;

            string BackUpPath = Paths.Server_SlnBackUp;
            string txtVersion = string.Empty;
            using (StreamReader sr = new StreamReader(Paths.Local_VersionTxt))
            {
                while (sr.Peek() >= 0)
                {
                    txtVersion += sr.ReadLine();
                }
                sr.Close();
            }
            Console.WriteLine($"現行版本: {version}\n紀錄版本: {txtVersion.Split(';')[0]}");
            int txt_ver = Convert.ToInt32(txtVersion.Split(';')[0].Split('.')[3]);
            int prog_ver = Convert.ToInt32(version.Split('.')[3]);

            // 版本更新，需備份
            if (txt_ver < prog_ver)
            {
                Console.WriteLine("版本更新，需備份");
                string[] SISubDirs = { "bin", "Class", "Dialog", "UserControls" };
                DirectoryInfo SIFolder = new DirectoryInfo(@"..\..\..\Self_Inspection");
                DirectoryInfo newFolder = new DirectoryInfo(Path.Combine(BackUpPath, $"txtVersion.Split(';')[0]"));
                if (newFolder.Exists == false) newFolder.Create();
                Console.WriteLine(newFolder);

                #region Backup Form1.cs, Form1.Designer.cs, Form1.resx
                foreach (FileInfo fi in SIFolder.GetFiles())
                {
                    Console.WriteLine(fi.FullName);
                    if (fi.Name.Contains("Form1"))
                    {
                        File.Copy(fi.FullName, newFolder + fi.Name);
                        Console.WriteLine($"Copy {fi.FullName}\nTo {newFolder + fi.Name}");
                    }
                }
                #endregion

                #region Backup directories
                foreach (string dir in SISubDirs)
                {
                    string folderNeedBup = SIFolder + dir;
                    Console.WriteLine(folderNeedBup);
                    DirectoryCopy(folderNeedBup, Path.Combine(newFolder.FullName, dir), true);
                }
                #endregion
            }
            else Console.WriteLine("版本無更新，不需備份");
            Console.WriteLine("Auto Backup Done." + DateTime.Now);
        }
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            if (Regex.IsMatch(sourceDirName, @"Debug|Re|Back|Tree")) return;
            Console.WriteLine("--DirectoryCopy--");
            void ShowAmount(string DirOrFile, int amount, string dirName)
            {
                string howManyOfWhat = $"no {DirOrFile}";
                if (amount > 0)
                    if (amount > 1) howManyOfWhat = $"{amount} {DirOrFile.Replace("y", "ie")}s";
                    else howManyOfWhat = $"{amount} {DirOrFile}";
                Console.WriteLine($"Move {howManyOfWhat} into backup {dirName}.");
            }
            try
            {
                // Get the sub-directories for the specified directory.
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);

                if (dir.Exists) Console.WriteLine($"{dir.FullName} Exists");
                else throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);

                DirectoryInfo[] subDirs = dir.GetDirectories();
                ShowAmount("sub-directory", subDirs.Length, destDirName);

                // If the destination directory doesn't exist, create it.       
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                    Console.WriteLine("Create " + destDirName);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                ShowAmount("file", files.Length, destDirName);
                foreach (FileInfo file in files)
                {
                    string tempPath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(tempPath, false);
                }

                // If copying subdirectories, copy them and their contents to new location.
                if (copySubDirs && subDirs.Length > 0)
                {
                    foreach (DirectoryInfo subDir in subDirs)
                    {
                        string tempPath = Path.Combine(destDirName, subDir.Name);
                        DirectoryCopy(subDir.FullName, tempPath, copySubDirs);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        #endregion

        #region Modify Changing
        #endregion

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            try
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        //ucSIProgram.ThisStation = ProgramDB.GetStation(ucSIProgram.ThisStation.Name);
                        ucSIProgram.UcSIProgram_Load(null, null);
                        break;
                    case 1:
                        ucSIItem.ThisStation = ucSIProgram.ThisStation;
                        ucSIItem.ChangeItem();
                        if (ucSIProgram.ThisSPItem != null)
                            ucSIItem.TscbbItem.Text = ucSIProgram.ThisSPItem.Name;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
