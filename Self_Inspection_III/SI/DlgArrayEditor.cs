using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Self_Inspection_III.SI
{
    public partial class DlgArrayEditor : Form
    {
        private string[] ThisArray { get; set; }
        private DataTypes DataTypes { get; set; }
        public string ArrayString
        {
            get
            {
                string str = ThisArray[0];
                for (int i = 1; i < ThisArray.Length; i++)
                    str += "," + ThisArray[i];
                return str;
            }
        }
        public DlgArrayEditor(object arrayValue)
        {
            Console.WriteLine($"DlgArrayEditor(object \"{arrayValue}\")");

            InitializeComponent();

            ThisArray = arrayValue.ToString().Split(',');
        }
        public DlgArrayEditor(object arrayValue, string dataType)
        {
            Console.WriteLine($"DlgArrayEditor(object \"{arrayValue}\", string \"{dataType}\")");

            InitializeComponent();

            if (string.IsNullOrEmpty(arrayValue.ToString()))
            {
                Console.WriteLine($"ThisArray = new string[{Regex.Match(dataType, @"\d{1,}")}];");
                ThisArray = new string[Convert.ToInt32(Regex.Match(dataType, @"\d{1,}").ToString())];
            }
            else ThisArray = arrayValue.ToString().Split(',');

            switch (Regex.Replace(dataType, @"\[\d{1,}\]", "_Array"))
            {
                case "Float":
                    DataTypes = DataTypes.Float_Array;
                    break;
                case "Integer":
                    DataTypes = DataTypes.Integer_Array;
                    break;
            }
        }
        private void DlgArrayEditor_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= ThisArray.Length; i++)
            {
                dgvArraySet.Columns.Add($"dgvArraySetCol{i}", $"{i}");

                cmbFrom.Items.Add(i.ToString());
                cmbTo.Items.Add(i.ToString());
            }
            dgvArraySet.Rows.Add();

            for (int i = 0; i < ThisArray.Length; i++)
                dgvArraySet[i, 0].Value = ThisArray[i];
        }

        private void DgvArraySet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void DgvArraySet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvArraySet[e.ColumnIndex, e.RowIndex].Value == null)
            {
                ThisArray[e.ColumnIndex] = string.Empty;
            }
            else
            {
                ThisArray[e.ColumnIndex] = dgvArraySet[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            int start = Convert.ToInt32(cmbFrom.Text);
            int stop = Convert.ToInt32(cmbTo.Text);
            for (int i = start - 1; i < stop; i++)
            {
                dgvArraySet[i, 0].Value = txtValue.Text;
            }
        }
        private void BtnOK_Click(object sender, EventArgs e)
        {
            dgvArraySet.EndEdit();
            DialogResult = DialogResult.OK;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
