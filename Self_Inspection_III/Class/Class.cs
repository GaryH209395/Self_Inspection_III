using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Self_Inspection_III.Class
{
    public class MsgBox
    {
        /// <summary>
        /// 查找視窗
        /// </summary>
        /// <param name="hwnd">視窗句柄</param>
        /// <param name="title">視窗標題</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(IntPtr hwnd, string title);

        /// <summary>
        /// 移動視窗
        /// </summary>
        /// <param name="hwnd">視窗句柄</param>
        /// <param name="x">起始位置X</param>
        /// <param name="y">起始位置Y</param>
        /// <param name="nWidth">視窗寬度</param>
        /// <param name="nHeight">視窗高度</param>
        /// <param name="rePaint">是否重繪</param>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern void MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool rePaint);

        /// <summary>
        /// 獲取視窗矩形
        /// </summary>
        /// <param name="hwnd">視窗句柄</param>
        /// <param name="rect"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool GetWindowRect(IntPtr hwnd, out Rectangle rect);

        /// <summary>
        /// 向視窗發送信息
        /// </summary>
        /// <param name="hwnd">視窗句柄</param>
        /// <param name="msg">信息</param>
        /// <param name="wParam">高位元組</param>
        /// <param name="lParam">低位元組</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int PostMessage(IntPtr hwnd, int msg, uint wParam, uint lParam);

        public const int WM_CLOSE = 0x10; //關閉命令

        public const int WM_KEYDOWN = 0x0100;//按下鍵

        public const int WM_KEYUP = 0x0101;//按鍵起來

        public const int VK_RETURN = 0x0D;//回車鍵

        public static bool IsWorking = false;

        /// <summary>
        /// 對話框標題
        /// </summary>
        public static List<string> titles = new List<string>();

        /// <summary>
        /// 查找和移動視窗
        /// </summary>
        /// <param name="title">視窗標題</param>
        /// <param name="x">起始位置X</param>
        /// <param name="y">起始位置Y</param>
        public static void FindAndMoveWindow(string title, int x, int y)
        {
            Thread t = new Thread(() =>
            {
                IntPtr msgBox = IntPtr.Zero;
                while ((msgBox = FindWindow(IntPtr.Zero, title)) == IntPtr.Zero) ;
                Rectangle r = new Rectangle();
                GetWindowRect(msgBox, out r);
                MoveWindow(msgBox, x, y, r.Width - r.X, r.Height - r.Y, true);
            });
            t.Start();
        }

        /// <summary>
        /// 查找和關閉視窗
        /// </summary>
        /// <param name="title">標題</param>
        private static void FindAndKillWindow(string title)
        {
            IntPtr ptr = FindWindow(IntPtr.Zero, title);
            if (ptr != IntPtr.Zero)
            {
                int ret = PostMessage(ptr, WM_CLOSE, 0, 0);
                Thread.Sleep(1000);
                ptr = FindWindow(IntPtr.Zero, title);
                if (ptr != IntPtr.Zero)
                {
                    PostMessage(ptr, WM_KEYDOWN, VK_RETURN, 0);
                    PostMessage(ptr, WM_KEYUP, VK_RETURN, 0);
                }
            }
        }

        /// <summary>
        /// 查找和關閉視窗
        /// </summary>
        public static void FindAndKillWindow()
        {
            Thread t = new Thread(() =>
            {
                while (IsWorking)
                {
                     //按標題查找
                     foreach (string title in titles)
                    {
                        FindAndKillWindow(title);
                    }
                    Thread.Sleep(3000);
                }
            });
            t.Start();
        }
    }

    public class Paths
    {
        //Constant
        public const string Server_GaryPath = @"";
        public const string Server_ERequestPath = @"\\silver\聯合製造中心\產工\ATS 資料備份\ATS User Mars\Self_Inspection(E_Request)";
        public const string Local_bin = @"..\bin";
        public const string Local_debug = @"..\bin\Debug";
        public const string Self_Inspection = "Self_Inspection";

        //Local
        public static string Local_VersionTxt = Path.Combine(Local_bin, "Version.txt");
        public static string Local_SystemXml = Path.Combine(Local_bin, "System.xml");
        public static string Local_UpdateApp = @".\UpdateApp_SI_III.exe";

        //Server
        public static string Server_SlnBackUp = Path.Combine(Server_GaryPath, @"Backup");
        public static string Server_ExeBackUp = Path.Combine(Server_ERequestPath, @"Backup");
        public static string Server_UpdateFile = Path.Combine(Server_ERequestPath, @"更新檔案");
        public static string Server_SIExe = Path.Combine(Server_ERequestPath, $"{Self_Inspection}.exe");
        public static string Server_VersionTxt = Path.Combine(Server_ERequestPath, "Version.txt");
        public static string Server_UpdateApp = Path.Combine(Server_ERequestPath, $"{Self_Inspection}_UpdateApp");
    }

    [Serializable]
    public class SISystem
    {
        public string ThisStation;
        public bool AlarmEmail;
        public bool ErrorEmail;
        public bool CheckEmployeeID;
        public bool GetHolidayList;

        public SISystem()
        {
            AlarmEmail = true;
            ErrorEmail = true;
            CheckEmployeeID = true;
            GetHolidayList = true;
        }

        public static SISystem Setting
        {
            get
            {
                if (File.Exists(Paths.Local_SystemXml))
                {
                    StreamReader reader = new StreamReader(Paths.Local_SystemXml);
                    try
                    {
                        string setting = reader.ReadToEnd();
                        Console.WriteLine(setting);
                        return XmlUtil.Deserialize(typeof(SISystem), setting) as SISystem;
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                    finally { reader.Close(); }
                }
                return new SISystem();
            }
            set
            {
                string xml = XmlUtil.Serializer(typeof(SISystem), value);
                Console.WriteLine(xml);
                StreamWriter str = new FileInfo(Paths.Local_SystemXml).CreateText();
                str.WriteLine(xml);
                str.Close();
            }
        }
    }

    [Serializable]
    public class Interface
    {
        public string Name;
        public string AddressFormat;

        public Interface() { }
        public Interface(string name)
        {
            Name = name;
            AddressFormat = string.Empty;
        }
    }

    public class MyEnum
    {
        public static TestPeriod GetPeriod(string str)
        {
            foreach (TestPeriod tp in Enum.GetValues(typeof(TestPeriod)))
                if (str == tp.ToString())
                    return tp;

            throw new Exception($"{str} is not in TestPeriod");
        }
    }
}

