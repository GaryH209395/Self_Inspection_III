using System;
using System.Threading;
using System.Windows.Forms;

namespace Self_Inspection_III.TestCommands
{
    public partial class DlgDelayExS : Form
    {
        private int m_Sec = 0;

        public DlgDelayExS(string Msg, double Sec)
        {
            InitializeComponent();
            m_Sec = (int)(Sec * 1000);
            lblMsg.Text = Msg;
            lblSec.Text = $"{Sec} sec";
            InitProgressbar();

            Console.WriteLine($"Sec: " + m_Sec);
        }


        private void BtnBreak_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void InitProgressbar()
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = m_Sec;
            progressBar1.Value = progressBar1.Minimum;
            progressBar1.Step = 100;
        }
        private void DlgDelayExS_Shown(object sender, EventArgs e)
        {
            timer1.Interval = progressBar1.Step; 
            timer1.Enabled = true; // 啟動 Timer

            //Thread t = new Thread((ThreadStart)delegate { Start(); });
            //t.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                progressBar1.PerformStep();
                lblSec.Text = $"{((m_Sec - progressBar1.Value) * 0.001).ToString("#0.0")} sec"; Application.DoEvents();
                if (progressBar1.Value >= progressBar1.Maximum)
                {
                    timer1.Enabled = false;
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
    }
}
