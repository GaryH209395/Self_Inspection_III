using System;
using System.Windows.Forms;

namespace Self_Inspection_III.DataSearch
{
    public partial class DlgDatetimePickers : Form
    {
        private string Datetimes { get; set; }

        public DlgDatetimePickers(string Datetimes)
        {
            InitializeComponent();
            label3.Visible = false;

            this.Datetimes = Datetimes;
        }
        private void DlgDatetimePickers_Load(object sender, EventArgs e)
        {
            string start = Datetimes.Split('~')[0].Trim();
            string end = Datetimes.Split('~')[1].Trim();

            dateTimePicker1.Value = new DateTime(Convert.ToInt32(start.Split('/')[0]), Convert.ToInt32(start.Split('/')[1]), Convert.ToInt32(start.Split('/')[2]));
            dateTimePicker2.Value = new DateTime(Convert.ToInt32(end.Split('/')[0]), Convert.ToInt32(end.Split('/')[1]), Convert.ToInt32(end.Split('/')[2]));
        }

        public string StartDate { get => dateTimePicker1.Value.ToString("yyyy/MM/dd"); }
        public string EndDate { get => dateTimePicker2.Value.ToString("yyyy/MM/dd"); }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
                label3.Visible = true;
            else
                DialogResult = DialogResult.OK;
        }
    }
}
