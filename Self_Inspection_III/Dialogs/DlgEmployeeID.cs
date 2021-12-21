using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Self_Inspection_III.Dialogs
{
    public partial class DlgEmployeeID : Form
    {
        private bool MESIDConfirm;
        public DlgEmployeeID(bool mesIDConfirm)
        {
            InitializeComponent();
            MESIDConfirm = mesIDConfirm;
        }
        public string EmpID { get => textBox1.Text == "209395" ? "TestID" : textBox1.Text; }     
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                lblIDInvalid.Text = "Please key in employee ID";
                lblIDInvalid.Visible = true;
            }
            else if (textBox1.Text.Length != 6)
            {
                lblIDInvalid.Text = "Wrong ID length.";
                lblIDInvalid.Visible = true;
                textBox1.Text = string.Empty;
            }
            else
            {
                switch (LoginEmpInfo(textBox1.Text))
                {
                    case (int)Status.ID_Pass:
                        DialogResult = DialogResult.OK;
                        return;
                    case (int)Status.ID_NotFound:
                        lblIDInvalid.Text = "ID not found.";
                        break;
                    case (int)Status.wsv_Error:
                        lblIDInvalid.Text = "Web Service Error.";
                        break;
                    default:
                        lblIDInvalid.Text = "ID Error.";
                        break;
                }
                lblIDInvalid.Visible = true;
                textBox1.Text = string.Empty;
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private int LoginEmpInfo(string id)
        {
            if (MESIDConfirm)
            {
                try
                {
                    Console.WriteLine($"Web Service Get Data");
                    DataTable dt = new MesSvc.Service1SoapClient().Get_Data("EMP_NAME", id).Tables[0];
                    if (string.IsNullOrEmpty(dt.Rows[0].ToString())) { return (int)Status.ID_NotFound; }
                    else { return (int)Status.ID_Pass; }
                }
                catch (IndexOutOfRangeException iorEx) { Console.WriteLine(iorEx.ToString()); return (int)Status.ID_NotFound; }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message); return (int)Status.wsv_Error; }
            }
            else return (int)Status.ID_Pass;
        }
        enum Status
        {
            ID_Pass = 9,
            ID_NotFound = 8,
            wsv_Error = 7
        }
    }
}
