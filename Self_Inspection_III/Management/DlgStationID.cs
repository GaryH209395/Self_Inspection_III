using System.Windows.Forms;

namespace Self_Inspection_III.Management
{
    public partial class DlgStationID : Form
    {
        public DlgStationID()
        {
            InitializeComponent();
        }

        public DataGridView DgvIdList { get => dgvIDList; set => dgvIDList = value; }
    }
}
