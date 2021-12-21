using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Miscellaneous
{
    class SetArrayFloatValue : TestCommand
    {
        public SetArrayFloatValue()
        {
            Name = "SetArrayFloatValue";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Member Value", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, ""),
                new Parameter("Member Index in Float Array", DataTypes.Integer, ParaTypes.Constant, ConstTypes.EditBox, "Index range: 1 ~ Array.Length"),
                new Parameter("Float Array", DataTypes.Float_Array, ParaTypes.Result, ConstTypes.NotAllow, "")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            string[] FloatArray = Vars.GetStringValue(para[2]).Split(',');
            int idx = Vars.GetIntegerValue(para[1]) - 1;

            FloatArray[idx] = para[0];

            string str = FloatArray[0];
            for (int i = 1; i < FloatArray.Length; i++)
                str += $",{FloatArray[i]}";

            Vars.SetValue(para[2], str);

            Vars.NextIdx++;
            return false;
        }
    }
}
