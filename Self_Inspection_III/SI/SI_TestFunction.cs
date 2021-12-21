using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Self_Inspection_III.SI
{
    [Serializable]
    public class SI_TestFunction
    {
        private string m_Active;
        private string m_Label;
        private string m_TestCommand;
        private string m_Device;
        private string m_Parameter;
        private string m_Decription;

        public SI_TestFunction() { }

        public SI_TestFunction(string Active, string Label, string TestCommand, string Device, string Parameter, string Description)
        {
            try { this.Active = Convert.ToBoolean(Active); } catch { this.Active = false; }
            this.Label = Label;
            this.TestCommand = TestCommand;
            this.Device = Device;
            this.Parameter = Parameter;
            this.Description = Description;
        }

        public bool Active
        {
            get { try { return Convert.ToBoolean(m_Active); } catch { return false; } }
            set => m_Active = value.ToString();
        }
        public string Label { get => m_Label; set => m_Label = value; }
        public string TestCommand { get => m_TestCommand; set => m_TestCommand = value; }
        public string Parameter { get => m_Parameter; set => m_Parameter = value; }
        public string Description { get => m_Decription; set => m_Decription = value; }
        public string Device { get => m_Device; set => m_Device = value; }
    }
}
