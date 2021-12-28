using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateApp_SI_III
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            DirectoryInfo ServerUpdateDir = new DirectoryInfo(@"\\silver\聯合製造中心\產工\ATS 資料備份\ATS User Mars\Self_Inspection(E_Request)\Self_Inspection_III\bin\Debug");
            DirectoryInfo LocalUpdateDir = new DirectoryInfo(@"..\Debug");
            string SI_3 = "Self_Inspection_III";

            try
            {
                Process[] ps = Process.GetProcesses();
                foreach (Process p in ps)
                {
                    if (p.ProcessName == SI_3)
                    {
                        Console.WriteLine("Kill: " + p.ProcessName);
                        p.Kill();
                    }
                }

                foreach (FileInfo fi in ServerUpdateDir.GetFiles())
                {
                    try
                    {
                        FileInfo fiLocal = new FileInfo(Path.Combine(LocalUpdateDir.FullName, fi.Name));
                        Console.WriteLine($"Copy: {fi.FullName}\nTo: {fiLocal.FullName}");
                        fi.CopyTo(fiLocal.FullName, true);
                    }
                    catch(Exception ex) { Console.WriteLine(ex); }
                }

                Process P_Start = new Process();
                P_Start.StartInfo.FileName = Path.Combine(LocalUpdateDir.FullName, $"{SI_3}.exe");
                P_Start.Start();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
            Close();
        }

        private void Form1_Closed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
