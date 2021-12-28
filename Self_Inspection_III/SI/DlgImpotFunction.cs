using Self_Inspection_III.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Self_Inspection_III.SI
{
    public partial class DlgImportFunction : Form
    {
        ItemDB ItemDB = new ItemDB();

        public DlgImportFunction()
        {
            InitializeComponent();
        }
        private void DlgImportFunc_Load(object sender, EventArgs e)
        {
            foreach (string str in ItemDB.GetItems())
                listBox1.Items.Add(str);
        }
        private string ItemSelected { get => listBox1.SelectedItem.ToString(); }
        private int Idx { get => ItemSelected.LastIndexOf(']'); }
        public string StationSelected { get => ItemSelected.Substring(1, Idx - 1); }
        public string FunctionSelected { get => ItemSelected.Substring(Idx + 2, ItemSelected.Length - (Idx + 2)); }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
