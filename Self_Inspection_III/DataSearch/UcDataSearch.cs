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
using System.Text.RegularExpressions;

namespace Self_Inspection_III.DataSearch
{
    public partial class UcDataSearch : UserControl
    {
        ResultDB ResultDB = new ResultDB();

        private SISystem SISystem;
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        private DataTable RightTable { get; set; }

        public UcDataSearch(SISystem SISystem)
        {
            InitializeComponent();
            Dock = DockStyle.Fill;

            StartDate = DateTime.Today.AddDays(-1);
            EndDate = DateTime.Today;
            tsbtnDatetimes.Text = $"{StartDate.ToString("yyyy/MM/dd")} ~ {EndDate.ToString("yyyy/MM/dd")}";

            tscbbStation.Items.Add("");
            foreach (DataRow row in new ProgramDB().GetStationList().Rows)
                tscbbStation.Items.Add(row[0]);

            tscmbStatus.Items.AddRange(new object[] { "", "Pass", "Fail" });
            this.SISystem = SISystem;
        }
        private void UcDataSearch_Load(object sender, EventArgs e)
        {
            tsbtnSearch.PerformClick();
        }

        private void DgvLeft_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            tscbbStation.Text = ResultDB.GetStationByItem(
                dgvLeft[(int)ColLeft.UUT, e.RowIndex].Value,
                dgvLeft[(int)ColLeft.Employee_ID, e.RowIndex].Value,
                dgvLeft[(int)ColLeft.Test_Time, e.RowIndex].Value);

            RightTable = ResultDB.ItemSearch(tscbbStation.Text,
               dgvLeft[(int)ColLeft.UUT, e.RowIndex].Value,
               dgvLeft[(int)ColLeft.Employee_ID, e.RowIndex].Value,
               dgvLeft[(int)ColLeft.Test_Time, e.RowIndex].Value);

            dgvRight.Rows.Clear();
            for (int i = 0; i < RightTable.Rows.Count; i++)
            {
                dgvRight.Rows.Add(new object[] { RightTable.Rows[i][(int)ColRight.Test_Item], RightTable.Rows[i][(int)ColRight.Test_Value] });
                dgvRight[(int)ColRight.Test_Item, i].Style.BackColor = Color.FromArgb(255, 255, 192);

                try
                {
                    decimal v = Convert.ToDecimal(Regex.Replace(RightTable.Rows[i][(int)ColRight.Test_Value].ToString(), @"V|A|Hz", ""));
                    decimal min = Convert.ToDecimal(Regex.Replace(RightTable.Rows[i][(int)ColRight.SpecMin].ToString(), @"V|A|Hz", ""));
                    decimal max = Convert.ToDecimal(Regex.Replace(RightTable.Rows[i][(int)ColRight.SpecMax].ToString(), @"V|A|Hz", ""));

                    if (v < min || v > max)
                        dgvRight[(int)ColRight.Test_Value, i].Style.ForeColor = Color.Red;
                    else
                        dgvRight[(int)ColRight.Test_Value, i].Style.ForeColor = Color.Green;
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
        }


        private void DgvRight_MouseHover(object sender, EventArgs e)
        {
            try
            {
                var pos = dgvRight.PointToClient(Cursor.Position);
                var info = dgvRight.HitTest(pos.X, pos.Y);
                //WriteLine($"info: ({info.ColumnIndex}, {info.RowIndex})");

                //tip.RemoveAll();
                if (info.RowIndex < 0 || info.ColumnIndex < 0) return;

                string showName = RightTable.Select($"{ResultDB.Col.Test_Item}='{dgvRight[(int)ColRight.Test_Item,info.RowIndex].Value}'")[0].ToString();
                string value = RightTable.Select($"{ResultDB.Col.Test_Item}='{dgvRight[(int)ColRight.Test_Item, info.RowIndex].Value}'")[1].ToString();
                string specMin = RightTable.Select($"{ResultDB.Col.Test_Item}='{dgvRight[(int)ColRight.Test_Item, info.RowIndex].Value}'")[2].ToString();
                string specMax = RightTable.Select($"{ResultDB.Col.Test_Item}='{dgvRight[(int)ColRight.Test_Item, info.RowIndex].Value}'")[3].ToString();

                TestItem ti = new TestItem();
                string specStr = $"Min: {ti.Min}{ti.Unit}\nMax: {ti.Max}{ti.Unit}";

                dgvRight[(int)ColRight.Test_Item, info.RowIndex].ToolTipText = specStr;
            }
            catch { }
        }

        private void TsbtnSearch_Click(object sender, EventArgs e)
        {
            DataTable dtLeft = ResultDB.DataSearch(tscbbStation.Text, tsbtnDatetimes.Text, tscmbStatus.Text);
            dgvLeft.Rows.Clear();
            for (int i = 0; i < dtLeft.Rows.Count; i++)
            {
                dgvLeft.Rows.Add(new object[] {
                    dtLeft.Rows[i][$"{ResultDB.Col.Test_Program}"],
                    $"{dtLeft.Rows[i][$"{ResultDB.Col.Device_Name}"]}-{string.Format("{0:00}",dtLeft.Rows[i][$"{ResultDB.Col.No}."])}",
                    dtLeft.Rows[i][$"{ResultDB.Col.Employee}"],
                    Convert.ToDateTime(dtLeft.Rows[i][$"{ResultDB.Col.Test_Time}"]).ToString("yyyy-MM-dd HH:mm:ss"),
                    dtLeft.Rows[i][$"{ResultDB.Col.Status}"] });
                try
                {
                    if (dgvLeft[(int)ColLeft.PF, i].Value.ToString().Contains("P"))
                    {
                        dgvLeft[(int)ColLeft.PF, i].Style.ForeColor = Color.Green;
                    }
                    else
                    if (dgvLeft[(int)ColLeft.PF, i].Value.ToString().Contains("F"))
                    {
                        dgvLeft[(int)ColLeft.PF, i].Style.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
        }

        private void TsbtnDatetimes_Click(object sender, EventArgs e)
        {
            DlgDatetimePickers dlgDatetimePickers = new DlgDatetimePickers(tsbtnDatetimes.Text);
            if (dlgDatetimePickers.ShowDialog() == DialogResult.OK)
            {
                tsbtnDatetimes.Text = $"{dlgDatetimePickers.StartDate} ~ {dlgDatetimePickers.EndDate}";
            }
        }
    }

    public enum ColLeft { Item_Name = 0, UUT, Employee_ID, Test_Time, PF }
    public enum ColRight { Test_Item = 0, Test_Value, SpecMin, SpecMax }
}
