using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Miscellaneous
{
    class SetArrayIntegerValue : TestCommand
    {
        public SetArrayIntegerValue()
        {
            Name = "SetArrayIntegerValue";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Member Value", DataTypes.Integer, ParaTypes.Condition, ConstTypes.EditBox, ""),
                new Parameter("Member Index in Float Array", DataTypes.Integer, ParaTypes.Constant, ConstTypes.EditBox, "Index range: 1 ~ Array.Length"),
                new Parameter("Integer Array", DataTypes.Integer_Array, ParaTypes.Result, ConstTypes.NotAllow, "")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            string[] IntegerArray = Vars.GetStringValue(para[2]).Split(',');
            int idx = Vars.GetIntegerValue(para[1]) - 1;

            IntegerArray[idx] = para[0];

            string str = IntegerArray[0];
            for (int i = 1; i < IntegerArray.Length; i++)
                str += $",{IntegerArray[i]}";

            Vars.SetValue(para[2], str);

            Vars.NextIdx++;
            return false;
        }
    }
}
