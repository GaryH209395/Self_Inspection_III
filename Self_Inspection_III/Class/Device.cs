using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Self_Inspection_III.Class
{
    [Serializable]
    public class Device
    {
        private string m_Active;
        private string m_Station;
        private string m_ShowName;
        private string m_ModelName;
        private int m_No;
        private string m_Type;
        private string m_Interface;
        private string m_Address;

        #region Constructor
        public Device() { No = "0"; }
        public Device(string ModelName, string Station, object Number)
        {
            Init(ModelName, Station, Number, string.Empty, string.Empty, string.Empty);
        }
        public Device(string ModelName, string Station, object Number, string Type)
        {
            Init(ModelName, Station, Number, Type, string.Empty, string.Empty);
        }
        public Device(string ModelName, string Station, object Number, string Type, string Interface, string Address)
        {
            Init(ModelName, Station, Number, Type, Interface, Address);
        }
        public Device(string ShowName,string ModelName, string Station, object Number, string Type, string Interface, string Address)
        {
            Init(ModelName, Station, Number, Type, Interface, Address);
            this.ShowName = ShowName;
        }
        private void Init(string Name, string Station, object Number, string Type, string Interface, string Address)
        {
            this.ModelName = Name;
            this.Station = Station;
            this.No = Number.ToString();
            this.Type = Type;
            this.Interface = Interface;
            this.Address = Address;
        }
        #endregion

        #region Attributes
        public string ID
        {
            get
            {
                if (Type == "Model") return Type;
                string ptrn = @"[^0-9A-Z]|SERIES";
                try
                {
                    string station = Regex.Replace(Station.ToUpper(), ptrn, "");
                    string model = Regex.Replace(ModelName.ToUpper(), ptrn, "");
                    string no = No;
                    return station + model + no;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Station: {Station}\nModel: {ModelName}-{No}\n" + ex.ToString(), "ID Error");
                    return "-1";
                }
            }
        }
        public string ShowName { get => m_ShowName??string.Empty; set => m_ShowName = value; }
        public string ModelName { get => m_ModelName ?? string.Empty; set => m_ModelName = value; }
        public string Station { get => m_Station ?? string.Empty; set => m_Station = value; }
        public string No { get => m_No.ToString("d2"); set { try { m_No = Convert.ToInt32(value); } catch { m_No = 0; } } }
        public string Interface { get => m_Interface ?? string.Empty; set => m_Interface = value; }
        public string Address { get => m_Address ?? string.Empty; set => m_Address = value; }
        public string Type
        {
            get
            {
                if (string.IsNullOrEmpty(m_Type))
                {
                    if (Regex.IsMatch(ModelName, @"(62\d{3}(P|H))|(6\d{3}A)"))
                        m_Type = DeviceTypes.DC_Source.ToString();

                    else if (Regex.IsMatch(ModelName, @"(61\d{3})|(65\d{2})"))
                        m_Type = DeviceTypes.AC_Source.ToString();

                    else if (Regex.IsMatch(ModelName, @"(344\d{1}1A)|(9102)"))
                        m_Type = DeviceTypes.DMM.ToString();

                    else if (Regex.IsMatch(ModelName, @"(66\d{2,3})|(VP-7723A)|(3\d{3}A)"))
                        m_Type = DeviceTypes.PowerMeter.ToString();

                    else if (Regex.IsMatch(ModelName, @"(TDS)|(3\d{2}2[A-Z])"))
                        m_Type = DeviceTypes.Oscilloscope.ToString();

                    else if (Regex.IsMatch(ModelName, @"7550A"))
                        m_Type = DeviceTypes.Current_Shunt.ToString();

                    else if (Regex.IsMatch(ModelName, @"Fluke"))
                        m_Type = DeviceTypes.Fluke.ToString();

                    else if (Regex.IsMatch(ModelName, @"63\d{3}"))
                        m_Type = DeviceTypes.Load.ToString();

                    else if (Regex.IsMatch(ModelName, @"Fixture"))
                        m_Type = DeviceTypes.Fixture.ToString();

                    else
                        m_Type = string.Empty;
                }
                return m_Type;
            }
            set => m_Type = value;
        }
        public bool Active
        {
            get { try { return Convert.ToBoolean(m_Active); } catch { return false; } }
            set => m_Active = value.ToString();
        }
        #endregion

        #region Methods

        #endregion
    }

    [Serializable]
    public class DeviceType
    {
        private string m_Name;
        private List<Device> m_Devices;

        public DeviceType() { }
        public DeviceType(string Name) { this.Name = Name; }
        public string Name { get => m_Name; set => m_Name = value; }
        public List<Device> Devices
        {
            get
            {
                if (m_Devices == null)
                    m_Devices = new List<Device>();
                return m_Devices;
            }
            set => m_Devices = value;
        }
        public static object[] Types
        {
            get
            {
                List<object> tempObj = new List<object>();

                foreach (DeviceTypes dt in Enum.GetValues(typeof(DeviceTypes)))
                    tempObj.Add(dt.ToString());

                tempObj.Remove(DeviceTypes.Null.ToString());
                return tempObj.ToArray();
            }
        }
    }

    public enum DeviceTypes
    {
        DC_Source = 0,
        AC_Source,
        DMM,
        PowerMeter,
        Oscilloscope,
        Current_Shunt,
        Fluke,
        Load,
        Fixture,
        Null,
        All
    }
}
