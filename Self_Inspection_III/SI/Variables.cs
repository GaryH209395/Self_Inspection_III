using Self_Inspection_III.SP;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.SI
{
    [Serializable]
    public class Variables
    {
        private string m_ShowName;
        private string m_CallName;
        private string m_DataType;
        private string m_Value;

        public string ShowName { get => m_ShowName ?? string.Empty; set => m_ShowName = value; }
        public string CallName { get => m_CallName ?? string.Empty; set => m_CallName = value; }
        public string DataType { get => m_DataType ?? string.Empty; set => m_DataType = value.Replace("[ ]", "_Array"); }
        public string Value { get => m_Value ?? string.Empty; set => m_Value = value; }

        public static string[] DataTypesList
        {
            get
            {
                List<string> dtLst = new List<string>();
                foreach (DataTypes dt in Enum.GetValues(typeof(DataTypes)))
                    dtLst.Add(Regex.Replace(dt.ToString(), "_Array", "[ ]"));
                return dtLst.ToArray();
            }
        }
        public static string[] EditTypesList
        {
            get
            {
                List<string> etLst = new List<string>();
                foreach (EditTypes et in Enum.GetValues(typeof(EditTypes)))
                    etLst.Add(et.ToString());
                return etLst.ToArray();
            }
        }
    }

    public class ConditionVar : Variables
    {
        private string m_EditType;
        private string m_Enum;
        public ConditionVar() { }
        public ConditionVar(string showName, string callName, string dataType, string editType, string defaultValue, string enumerative)
        {
            ShowName = showName;
            CallName = callName;
            DataType = dataType;
            EditType = editType;
            Value = defaultValue;
            Enum = enumerative;
        }

        public string EditType { get => m_EditType ?? string.Empty; set => m_EditType = value; }
        public string Enum { get => m_Enum ?? string.Empty; set => m_Enum = value; }
    }
    public class TemporaryVar : Variables
    {
        public TemporaryVar() { }
        public TemporaryVar(string showName, string callName, string dataType)
        {
            ShowName = showName;
            CallName = callName;
            DataType = dataType;
        }
    }
    public class ResultVar : Variables
    {
        private Variables m_SpecMin;
        private Variables m_SpecMax;
        public ResultVar() { }
        public ResultVar(string showName, string callName, string dataType, Variables specMin, Variables specMax)
        {
            ShowName = showName;
            CallName = callName;
            DataType = dataType;
            SpecMin = specMin;
            SpecMax = specMax;
        }

        public Variables SpecMin { get => m_SpecMin; set => m_SpecMin = value; }
        public Variables SpecMax { get => m_SpecMax; set => m_SpecMax = value; }
        public ColorText PF { get => ColorText.GetPF(Value, SpecMin.Value, SpecMax.Value); }
    }
    public enum DataTypes
    {
        Float,
        Integer,
        String,
        Float_Array,
        Integer_Array
    }
    public enum EditTypes
    {
        EditBox,
        ComboList
    }
}
