using CviVisaDriver;
using Self_Inspection.Class;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using Self_Inspection_III.TestCommands.AC_Source;
using Self_Inspection_III.TestCommands.Arithmetic;
using Self_Inspection_III.TestCommands.Command_ReadWrite;
using Self_Inspection_III.TestCommands.DC_Source;
using Self_Inspection_III.TestCommands.DMM;
using Self_Inspection_III.TestCommands.Fluke;
using Self_Inspection_III.TestCommands.IO_Card;
using Self_Inspection_III.TestCommands.Miscellaneous;
using Self_Inspection_III.TestCommands.PowerMeter;
using Self_Inspection_III.TestCommands.StringFunc;
using Self_Inspection_III.TestCommands.System_FlowControl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Self_Inspection_III.TestCommands
{
    public abstract class TestCommand
    {
        private string m_Name;
        private DeviceTypes m_Device;
        private List<Parameter> m_Parameters;

        public string Name { get => m_Name; set => m_Name = value; }
        public DeviceTypes DeviceType { get => m_Device; set => m_Device = value; }
        public List<Parameter> Parameters
        {
            get
            {
                if (m_Parameters == null)
                    m_Parameters = new List<Parameter>();
                return m_Parameters;
            }
            set => m_Parameters = value;
        }

        protected void WriteCommand(CviVisaCtrl NIDriver, string ModelName, object Command)
        {
            Console.WriteLine($"{ModelName}'s command: {Command}");
            if (NIDriver.Write(Command.ToString())) Thread.Sleep(200);
            else throw new Exception(NIDriver.ErrorMessage);
        }
        protected string ReadResponse(CviVisaCtrl NIDriver)
        {
            Console.WriteLine($"== ReadResponse() ==");
            string ReturnString = string.Empty;
            if (NIDriver.Read(ref ReturnString)) Thread.Sleep(200);
            else throw new Exception(NIDriver.ErrorMessage);

            if (string.IsNullOrEmpty(ReturnString)) { throw new Exception("No read value from "); }
            Console.WriteLine("ReturnString: " + ReturnString);

            return ReturnString;
        }
        protected string[] ParaValue(string ParaString)
        {
            string[] para = ParaString.Split(',');
            string[] paraValue = new string[Parameters.Count];

            for (int i = 0; i < paraValue.Length; i++)
                if (Parameters[i].ConstType == ConstTypes.ComboList.ToString())
                    paraValue[i] = FindEIValue(para[i], i);
                else
                    paraValue[i] = para[i];

            return paraValue;
        }
        protected string FindEIValue(string para, int pIdx)
        {
            foreach (EnumItem ei in Parameters[pIdx].EnumItems)
                if (ei.Name == para)
                    return ei.Value;

            throw new NullReferenceException($"EnumItem: para[{pIdx}]: {para} not found!");
        }
        protected void ShowException(Exception ex)
        {
            Console.WriteLine($"::{Name}.Function::\n" + ex.ToString());
            MessageBox.Show($"::{Name}.Function::\n" + ex.Message, "Error");
            Email.SendErrorEmail($"{Name}.Function", ex.Message, ex.ToString());
        }
        public static bool ShowEditor(string command, Item item, List<string> labels, ref string paraString, List<Device> devices, ref string device)
        {
            CmdsParameterEditor editor = new CmdsParameterEditor(TC(command), item, paraString, devices) { Device = device };
            if (editor.ShowDialog() == DialogResult.OK)
            {
                paraString = editor.Paras;
                device = editor.Device;
                return true;
            }
            else
            {
                device = editor.Device;
                return false;
            }
        }
        protected static string TCDoFunc(TestCommand testCommand, string[] para, ref int idx)
        {
            StringBuilder paraStr = new StringBuilder(para[idx++]);
            for (int i = 1; i < testCommand.Parameters.Count; i++)
                paraStr.Append($",{para[idx++]}");
            Console.WriteLine($"::{testCommand.Name} --> {paraStr}");
            return paraStr.ToString();
        }
        protected abstract bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars);
        public bool DoFunction(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            return Function(ParaString, ModelName, NIDriver, ref Vars);
        }
        protected virtual bool Function(string ParaString, ushort CardNumber, ushort CardType, out short IO_Dev, ref ItemVars Vars) { IO_Dev = 0; return false; }
        public static bool DoIOFunction(SI_TestFunction Func, ushort CardNumber, ushort CardType, out short IO_Dev, ref ItemVars Vars)
        {
            short io_dev = 0;
            try
            {
                TestCommand testCommand = TC(Func.TestCommand);

                Console.WriteLine($"=={testCommand.Name}.Function({Func.Parameter}, {Func.Device}, NIDriver, Vars)==");
                try
                {
                    return testCommand.Function(Func.Parameter, CardNumber, CardType, out io_dev, ref Vars);
                }
                catch (NullReferenceException nrEx)
                {
                    testCommand.ShowException(nrEx);
                }
                catch (Exception ex)
                {
                    if (Regex.IsMatch(ex.Message, "格式|No read value"))
                        throw new Exception("No read value from " + Func.Device);
                    else
                        testCommand.ShowException(ex);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (Regex.IsMatch(ex.Message, "格式|No read value"))
                    throw new Exception("No read value from " + Func.Device);
                else
                    MessageBox.Show(ex.ToString());
            }
            finally { IO_Dev = io_dev; }
            return true;
        }
        public static bool DoFunction(SI_TestFunction Func, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            // Return: true: Fail/Error; false: Pass
            try
            {                
                TestCommand testCommand = TC(Func.TestCommand);

                Console.WriteLine($"=={testCommand.Name}.Function({Func.Parameter}, {Func.Device}, NIDriver, Vars)==");
                try
                {
                    return testCommand.Function(Func.Parameter, Func.Device, NIDriver, ref Vars);
                }
                catch (NullReferenceException nrEx)
                {
                    testCommand.ShowException(nrEx);
                }
                catch (Exception ex)
                {
                    if (Regex.IsMatch(ex.Message, "格式|No read value"))
                        throw new Exception("No read value from " + Func.Device);
                    else
                        testCommand.ShowException(ex);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (Regex.IsMatch(ex.Message, "格式|No read value"))
                    throw new Exception("No read value from " + Func.Device);
                else
                    MessageBox.Show(ex.ToString());
            }
            return true;
        }
        private static TestCommand TC(string tc)
        {
            switch (tc)
            {
                #region System, Flow Control
                case "DelayS": return new DelayS();
                case "DelayExS": return new DelayExS();
                case "GetPassFail": return new GetPassFail();
                case "Goto": return new Goto();
                case "If_Then": return new If_Then();
                case "PauseMsg": return new PauseMsg();
                case "InputTextbox": return new InputTextbox();
                #endregion

                #region Arithmetic
                case "ABS": return new ABS();
                case "ADD": return new ADD();
                case "DEC": return new DEC();
                case "DIV": return new DIV();
                case "INC": return new INC();
                case "MUL": return new MUL();
                case "SET": return new SET();
                case "SUB": return new SUB();
                #endregion

                #region String
                case "SETS": return new SETS();
                case "StrCat": return new StrCat();
                #endregion

                #region Miscellaneous
                case "GetArrayFloatElement": return new GetArrayFloatElement();
                case "GetArrayIntegerElement": return new GetArrayIntegerElement();
                case "SetArrayFloatValue": return new SetArrayFloatValue();
                case "SetArrayIntegerValue": return new SetArrayIntegerValue();
                #endregion

                #region Command Read, Write
                case "ReadDev_Command": return new ReadDev_Command();
                case "WriteDev_Command": return new WriteDev_Command();
                #endregion

                #region AC Source
                case "ReadAC_Current": return new ReadAC_Current();
                case "ReadAC_Voltage": return new ReadAC_Voltage();
                case "ReadACDev_Response": return new ReadACDev_Response();
                case "SetAC_Frequency": return new SetAC_Frequency();
                case "SetAC_Instrument": return new SetAC_Instrument(); 
                case "SetAC_OutputMode": return new SetAC_OutputMode();
                case "SetAC_OutputState": return new SetAC_OutputState();
                case "SetAC_VoltRange": return new SetAC_VoltRange();
                case "SetAC_Vout": return new SetAC_Vout();
                case "WriteACDev_Command": return new WriteACDev_Command();
                #endregion

                #region DC Source
                case "ReadDC_Iout": return new ReadDC_Iout();
                case "ReadDC_Vout": return new ReadDC_Vout();
                case "ReadDCDev_Response": return new ReadDCDev_Response();
                case "SetDC_Ilimit": return new SetDC_Ilimit();
                case "SetDC_OutputMode": return new SetDC_OutputMode();
                case "SetDC_OutputState": return new SetDC_OutputState();
                case "SetDC_Vout": return new SetDC_Vout();
                case "WriteDCDev_Command": return new WriteDCDev_Command();
                #endregion

                #region DMM
                case "ReadDMM_Measure": return new ReadDMM_Measure();
                case "ReadDMMDev_Response": return new ReadDMMDev_Response();
                case "SetDMM_FuncRange": return new SetDMM_FuncRange();
                case "WriteDMMDev_Command": return new WriteDMMDev_Command();
                case "Set3458A_FuncRange": return new Set3458A_FuncRange();
                case "SetDMM_NPLC": return new SetDMM_NPLC();
                #endregion

                #region Powermeter
                case "ReadPM_Current": return new ReadPM_Current();
                case "ReadPM_Frequency": return new ReadPM_Frequency();
                case "ReadPM_Power": return new ReadPM_Power();
                case "ReadPM_Voltage": return new ReadPM_Voltage();
                case "SetPM_Default": return new SetPM_Default();
                case "SetPM_InputCT": return new SetPM_InputCT();
                case "SetPM_InputWiring": return new SetPM_InputWiring();
                case "SetPM_MeasMode": return new SetPM_MeasMode();
                case "SetPM_ShowChannel": return new SetPM_ShowChannel();
                case "SetPM_ShowItem": return new SetPM_ShowItem();
                case "WritePMDev_Command": return new WritePMDev_Command();
                #endregion

                #region Fluke
                case "SetFluke_OutputState": return new SetFluke_OutputState();
                case "SetFluke_OutputState2": return new SetFluke_OutputState2();
                case "SetFluke_OutputValue": return new SetFluke_OutputValue();
                case "WriteFluke_Command": return new WriteFluke_Command();
                #endregion

                #region IO Card
                case "SetIO_OutputState": return new SetIO_OutputState();
                #endregion

                default: throw new Exception("Error Test Command");
            }
        }
        public static bool SourceOff(string ModelName, CviVisaCtrl NIDriver)
        {
            try
            {
                ItemVars vars = new ItemVars();
                switch (DeviceDB.GetType(ModelName))
                {
                    case DeviceTypes.DC_Source:
                        return new SetDC_OutputState().DoFunction("OFF", ModelName, NIDriver, ref vars);
                    case DeviceTypes.AC_Source:
                        return new SetAC_OutputState().DoFunction("OFF", ModelName, NIDriver, ref vars);
                    case DeviceTypes.Fluke:
                        return new WriteFluke_Command().DoFunction("STBY", ModelName, NIDriver, ref vars);
                    default:
                        return false;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); return true; }
        }
    }

    public class Parameter
    {
        private string m_Name;
        private DataTypes m_DataType;
        private ParaTypes m_ParaType;
        private ConstTypes m_ConstType;
        private string m_Description;
        private string m_DefaultValue;
        private string m_ReturnValue;
        private EnumItem[] m_EnumItems;

        public Parameter(string name, DataTypes dataType, ParaTypes paraType, ConstTypes constType, string description)
        {
            Name = name;
            DataType = dataType.ToString();
            ParaType = paraType.ToString();
            ConstType = constType.ToString();
            Description = description;

            //Console.WriteLine($"new Parameter({Name}, {DataType}, {ParaType}, {ConstType})");
        }

        public string Name { get => m_Name ?? string.Empty; set => m_Name = value; }
        public string Description { get => m_Description ?? string.Empty; set => m_Description = value; }
        public string ReturnValue { get => m_ReturnValue ?? string.Empty; set => m_ReturnValue = value; }
        public string DefaultValue { get => m_DefaultValue ?? string.Empty; set => m_DefaultValue = value; }
        public string DataType
        {
            get => Regex.Replace(m_DataType.ToString(), "Array", "[]");
            set
            {
                try
                {
                    List<DataTypes> DTs = new List<DataTypes>();
                    foreach (DataTypes dt in Enum.GetValues(typeof(DataTypes)))
                        DTs.Add(dt);

                    int i = DTs.FindIndex(x => x.ToString() == Regex.Replace(value, @"\[]", "Array"));
                    m_DataType = DTs[i];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Invalid Data Type: " + value, "Error");
                }
            }
        }
        public string ParaType
        {
            get => m_ParaType.ToString();
            set
            {
                try
                {
                    List<ParaTypes> PTs = new List<ParaTypes>();
                    foreach (ParaTypes pt in Enum.GetValues(typeof(ParaTypes)))
                        PTs.Add(pt);

                    int i = PTs.FindIndex(x => x.ToString() == value);
                    m_ParaType = PTs[i];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Invalid Parameter Type: " + value, "Error");
                }
            }
        }
        public string ConstType
        {
            get => m_ConstType.ToString();
            set
            {
                try
                {
                    List<ConstTypes> CTs = new List<ConstTypes>();
                    foreach (ConstTypes ct in Enum.GetValues(typeof(ConstTypes)))
                        CTs.Add(ct);

                    int i = CTs.FindIndex(x => x.ToString() == value);
                    m_ConstType = CTs[i];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Invalid Constant Type: " + value, "Error");
                }
            }
        }
        public EnumItem[] EnumItems
        {
            get
            {
                if (m_EnumItems == null)
                    m_EnumItems = new EnumItem[] { };
                return m_EnumItems;
            }
            set => m_EnumItems = value;
        }
        public static List<string> OperatorList { get => new List<string>() { Eql, Neq, Grt, Lst, Gne, Lne }; }
        public const string Eql = "=";
        public const string Neq = "!=";
        public const string Grt = ">";
        public const string Lst = "<";
        public const string Gne = ">=";
        public const string Lne = "<=";

    }
    public class EnumItem
    {
        private string m_Name;
        private string m_Value;
        public EnumItem(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public string Name { get => m_Name; set => m_Name = value; }
        public string Value { get => m_Value; set => m_Value = value; }
    }
    public enum ParaTypes
    {
        Condition = 0,
        Temporary,
        Result,
        Global,
        Constant,
        Operator,
        Label,
        Signal
    }
    public enum ConstTypes
    {
        NotAllow = 0,
        EditBox,
        ComboList
    }
}
