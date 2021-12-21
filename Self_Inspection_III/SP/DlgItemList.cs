using Self_Inspection_III.Class;
using System;
using System.Windows.Forms;

namespace Self_Inspection_III.SP
{
    public partial class DlgItemList : Form
    {
        ItemDB ItemDB = new ItemDB();

        public string Station { get; set; }
        public DlgItemList(string station)
        {
            InitializeComponent();
            Station = station;
        }
        private void DlgItemList_Load(object sender, EventArgs e)
        {
            foreach (Item item in ItemDB.GetItems(Station))
                if (item == null) continue;
                else
                    listBox1.Items.Add(item.Name);
        }

        public ListBox.SelectedObjectCollection Items { get => listBox1.SelectedItems; }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            btnOK.PerformClick();
        }
    }
}
