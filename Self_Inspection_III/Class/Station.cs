using Self_Inspection_III.SI;
using Self_Inspection_III.SP;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.Class
{
    [Serializable]
    public class Station
    {
        #region Attributes
        private string m_Active;
        private string m_ID;
        private string m_Name;
        private string m_CL; //課長
        private string m_TL; //組長
        private string m_PE; //產工
        private string m_TE; //測試人員
        private List<SPItem> m_Items;
        private List<Device> m_Devices;
        private bool m_Edited = false;

        public bool Active
        {
            get { try { return Convert.ToBoolean(m_Active); } catch { return false; } }
            set => m_Active = value.ToString();
        }
        public string ID
        {
            get
            {
                if (!Regex.IsMatch(m_ID ?? "0", @"[A-Z]\d{3}"))
                {
                    Edited = true;
                    m_ID = new ProgramDB().GetID(Name);
                }
                Console.WriteLine($"{Name}: {m_ID}");
                return m_ID;
            }
            set => m_ID = value;
        }
        public string Name { get => m_Name ?? string.Empty; set => m_Name = value; }
        public string CL { get => m_CL ?? "-"; set => m_CL = value; }
        public string TL { get => m_TL ?? "-"; set => m_TL = value; }
        public string PE { get => m_PE ?? "-"; set => m_PE = value; }
        public string TE { get => m_TE ?? "-"; set => m_TE = value; }
        public List<SPItem> Items
        {
            get
            {
                if (m_Items == null)
                    m_Items = new List<SPItem>();
                return m_Items;
            }
            set => m_Items = value;
        }
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

        public bool Edited { get => m_Edited; set => m_Edited = value; }
        #endregion

        #region Constructor
        public Station() { }
        public Station(string Name) { this.Name = Name; }
        #endregion

        #region Methods       
        public Station Clone()
        {
            Console.WriteLine($"This Station: ({ID}) {Name} is being cloned.");

            string newID = ID[0] + (Convert.ToInt32(ID.Substring(1, 3)) + 1).ToString("d3");
            string newName = Name + "(2)";

            Console.WriteLine($"New Station: ({newID}) {newName} is being cloned.");
            return new Station
            {
                Active = true,
                ID = newID,
                Name = newName,
                CL = CL,
                TL = TL,
                PE = PE,
                TE = TE,
                Devices = Devices,
                Edited = true
            };
        }
        public void ClearSpItem_None()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Sources.RemoveAll(x => x == "(None)");
                Items[i].Sources.Add("(None)");
                Items[i].Meters.RemoveAll(x => x == "(None)");
                Items[i].Meters.Add("(None)");
            }
        }
    #endregion
}

    [Serializable]
    public class SPItem
    {
        #region Attributes
        private bool m_Active;
        private string m_Id;
        private string m_Name;
        private string m_Employee;
        private string m_TestTime;
        private ColorText m_PF;
        private TestPeriod m_Period;
        private string m_UUT;
        private string m_Source;
        private string m_Meter;
        private List<string> m_Sources;
        private List<string> m_Meters;
        private List<ConditionVar> m_Conditions;
        private List<TestItem> m_TestItems;

        public bool Active { get => m_Active; set => m_Active = value; }
        public string ID { get => m_Id; set => m_Id = value; }
        public string Name { get => m_Name; set => m_Name = value; }
        public string Employee { get => m_Employee; set => m_Employee = value; }
        public string TestTime { get => m_TestTime; set => m_TestTime = value; }
        public ColorText PF { get => m_PF; set => m_PF = value; }
        public string UUT { get => m_UUT; set => m_UUT = value; }
        public string Source { get => m_Source; set => m_Source = value; }
        public string Meter { get => m_Meter; set => m_Meter = value; }
        public List<string> Sources { get => m_Sources; set => m_Sources = value; }
        public List<string> Meters { get => m_Meters; set => m_Meters = value; }
        public List<ConditionVar> Conditions { get => m_Conditions; set => m_Conditions = value; }
        public List<TestItem> TestItems { get => m_TestItems; set => m_TestItems = value; }
        public TestPeriod Period { get => m_Period; set => m_Period = value; }
        #endregion

        #region Constructor
        public SPItem() { SettingLists(); }
        public SPItem(string Name)
        {
            this.Name = Name;
            SettingLists();
        }
        #endregion

        #region Methods
        private void SettingLists()
        {
            Sources = new List<string>();
            Meters = new List<string>();
            Conditions = new List<ConditionVar>();
            TestItems = new List<TestItem>();
        }
        public SPItem Clone() => new SPItem()
        {
            Active = Active,
            ID = ID,
            Name = Name,
            UUT = UUT,
            Source = Source,
            Meter = Meter,
            Sources = Sources,
            Meters = Meters,
            Conditions = Conditions,
            TestItems = TestItems,
            Period = Period
        };
        #endregion
    }

    [Serializable]
    public class Item
    {
        #region Attributes
        private string m_ID;
        private string m_Active;
        private string m_Name;
        private ColorText m_PF;
        private TestPeriod m_Period;
        private ItemVars m_Vars;
        private string m_UUT;
        private string m_Source;
        private string m_Meter;
        private List<string> m_Sources;
        private List<string> m_Meters;
        private List<SI_TestFunction> m_SITestFuncs;

        public bool Active
        {
            get { try { return Convert.ToBoolean(m_Active); } catch { return false; } }
            set => m_Active = value.ToString();
        }
        public string Name { get => m_Name ?? string.Empty; set => m_Name = value; }
        public string UUT { get => m_UUT ?? string.Empty; set => m_UUT = value; }
        public string Source { get => m_Source ?? string.Empty; set => m_Source = value; }
        public string Meter { get => m_Meter ?? string.Empty; set => m_Meter = value; }
        public TestPeriod Period { get => m_Period; set => m_Period = value; }
        public object[] PeriodList
        {
            get => new object[] { TestPeriod.Day, TestPeriod.Week, TestPeriod.Season };
        }
        public List<SI_TestFunction> TestFunctions
        {
            get
            {
                if (m_SITestFuncs == null)
                {
                    m_SITestFuncs = new List<SI_TestFunction>();
                }
                return m_SITestFuncs;
            }
            set => m_SITestFuncs = value;
        }
        public List<string> Sources
        {
            get
            {
                if (!m_Sources.Exists(x => x == NONE))
                    m_Sources.Insert(0, NONE);
                return m_Sources;
            }
            set => m_Sources = value;
        }
        public List<string> Meters
        {
            get
            {
                if (!m_Meters.Exists(x => x == NONE))
                    m_Meters.Insert(0, NONE);
                return m_Meters;
            }
            set => m_Meters = value;
        }
        public ColorText PF { get => m_PF; set => m_PF = value; }
        public ItemVars Vars { get => m_Vars; set => m_Vars = value; }
        public string ID { get => m_ID; set => m_ID = value; }
        public SPItem SPItem
        {
            get
            {
                SPItem spi = new SPItem
                {
                    Active = Active,
                    ID = ID,
                    Name = Name,
                    PF = PF,
                    Period = Period,
                    UUT = UUT,
                    Source = Source,
                    Meter = Meter,
                    Sources = Sources,
                    Meters = Meters,
                    Conditions = Vars.Conditions
                };
                Console.WriteLine($"return new SPItem({spi.Name})");
                foreach (ResultVar rv in Vars.Results)
                {
                    Console.WriteLine($"new TestItem({rv.ShowName}, {rv.Value}, {rv.SpecMin.Value}, {rv.SpecMax.Value})");
                    spi.TestItems.Add(new TestItem(rv.ShowName, /*rv.DataType.Contains("Array") ? "..." :*/ rv.Value ?? string.Empty, rv.SpecMin.Value ?? string.Empty, rv.SpecMax.Value ?? string.Empty));
                }
                return spi;
            }
        }
        #endregion

        #region Constructor
        public Item() { SettingLists(); }
        public Item(string Name)
        {
            this.Name = Name;
            SettingLists();
        }
        #endregion

        #region Methods
        public const string NONE = "(None)";
        private void SettingLists()
        {
            Sources = new List<string>();
            Meters = new List<string>();
            Vars = new ItemVars();
        }
        public void ClearNone()
        {
            Sources.RemoveAll(x => x == "(None)");
            Sources.Add("(None)");
            Meters.RemoveAll(x => x == "(None)");
            Meters.Add("(None)");
        }
        #endregion
    }

    [Serializable]
    public class ItemVars
    {
        private List<ConditionVar> m_Conditions;
        private List<TemporaryVar> m_Temporaries;
        private List<ResultVar> m_Results;

        private string[] m_Labels;
        private int m_NextIdx = 0;

        public ItemVars()
        {
            this.Conditions = new List<ConditionVar>();
            this.Temporaries = new List<TemporaryVar>();
            this.Results = new List<ResultVar>();
        }
        public ItemVars(List<ConditionVar> Conditions, List<TemporaryVar> Temporaries, List<ResultVar> Results)
        {
            this.Conditions = Conditions;
            this.Temporaries = Temporaries;
            this.Results = Results;
        }

        public List<ConditionVar> Conditions { get => m_Conditions; set => m_Conditions = value; }
        public List<TemporaryVar> Temporaries { get => m_Temporaries; set => m_Temporaries = value; }
        public List<ResultVar> Results { get => m_Results; set => m_Results = value; }
        public string[] Labels { get => m_Labels ?? new string[] { }; set => m_Labels = value; }
        public int NextIdx { get => m_NextIdx; set => m_NextIdx = value; }

        public void Clear()
        {
            Conditions.Clear();
            Temporaries.Clear();
            Results.Clear();
        }
        public double GetDoubleValue(string para)
        {
            try { return Convert.ToDouble(para); }
            catch
            {
                string pValue = GetValue(para);
                try { return Convert.ToDouble(pValue); }
                catch
                {
                    double.TryParse(pValue, out double result);
                    return result;
                }
            }
        }
        public int GetIntegerValue(string para)
        {
            try { return Convert.ToInt32(para); }
            catch { return Convert.ToInt32(GetValue(para)); }
        }
        public string GetStringValue(string para)
        {
            if (para.Contains("\""))
                return Regex.Replace(para, "\"", "");
            else
                return GetValue(para);
        }
        public string GetValue(string callName)
        {
            if (Conditions.Exists(x => x.CallName == callName))
                return Conditions[Conditions.FindIndex(x => x.CallName == callName)].Value;
            else
            if (Temporaries.Exists(x => x.CallName == callName))
                return Temporaries[Temporaries.FindIndex(x => x.CallName == callName)].Value;
            else
            if (Results.Exists(x => x.CallName == callName))
                return Results[Results.FindIndex(x => x.CallName == callName)].Value;
            else
                throw new NullReferenceException($"CallName: {callName} not found.");
        }
        public void SetValue(string callName, object v)
        {
            string va;
            try { va = Convert.ToDouble(v).ToString("F12"); } catch { va = v.ToString(); }

            if (Temporaries.Exists(x => x.CallName == callName))
                Temporaries[Temporaries.FindIndex(x => x.CallName == callName)].Value = va;
            else
            if (Results.Exists(x => x.CallName == callName))
                Results[Results.FindIndex(x => x.CallName == callName)].Value = va;
            else
            if (Conditions.Exists(x => x.CallName == callName))
                Conditions[Conditions.FindIndex(x => x.CallName == callName)].Value = va;
            else
                throw new NullReferenceException($"CallName: {callName} not found.");
        }
    }
    public enum TestPeriod
    {
        Day = 5,
        Week,
        Season
    }
    public enum PF
    {
        Pass = 0,
        Fail
    }

    [Serializable]
    public class TestItem //測項
    {
        #region Attributes
        private string m_ShowName;
        private string m_SpecMin;
        private string m_SpecMax;
        private string m_Value;
        private string m_Unit;
        private ColorText m_PF;

        public string Name { get => m_ShowName ?? string.Empty; set => m_ShowName = value; }
        public ColorText PF { get => ColorText.GetPF(Value, Min.ToString(), Max.ToString()); }
        public string Value { get => m_Value; set => m_Value = value; }
        public double Min
        {
            get { try { return Convert.ToDouble(m_SpecMin); } catch { return 0; } }
            set { m_SpecMin = value.ToString("0.000000"); }
        }
        public double Max
        {
            get { try { return Convert.ToDouble(m_SpecMax); } catch { return 0; } }
            set { m_SpecMax = value.ToString("0.000000"); }
        }
        public string Unit { get => m_Unit ?? GetUnit(); }
        #endregion

        #region Constructor
        public TestItem() { }
        public TestItem(string ShowName, string Value, string SpecMin, string SpecMax)
        {
            Name = ShowName;
            this.Value = string.IsNullOrEmpty(Value) ? string.Empty : Value;
            try
            {
                Min = Convert.ToDouble(SpecMin);
                Max = Convert.ToDouble(SpecMax);
            }
            catch { Min = double.NegativeInfinity; Max = double.PositiveInfinity; }
            Console.WriteLine($"new TestItem({Name}, {this.Value}, {Min}, {Max})");
        }
        private string GetUnit()
        {
            try
            {
                return Name.Substring(Name.IndexOf('(') + 1, Name.LastIndexOf(')') - (Name.IndexOf('(') + 1));
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion
    }
    public class Model
    {
        #region Attributes
        private string m_Name;
        private string m_Type;

        public string Type { get => m_Type; set => m_Type = value; }
        public string Name { get => m_Name; set => m_Name = value; }
        #endregion

        #region Constructor
        public Model() { }
        public Model(string name, string type)
        {
            Name = name;
            Type = type;
        }
        #endregion
    }
}
