using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Miscellaneous
{
    class GetArrayFloatElement : TestCommand
    {
        public GetArrayFloatElement()
        {
            Name = "GetArrayFloatElement";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Float Array", DataTypes.Float_Array, ParaTypes.Condition, ConstTypes.NotAllow, ""),
                new Parameter("Element Index", DataTypes.Integer, ParaTypes.Constant, ConstTypes.EditBox, "Index range: 1 ~ Array.Length"),
                new Parameter("Return the value of the element", DataTypes.Float, ParaTypes.Temporary, ConstTypes.NotAllow, "")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            string[] FloatArray = Vars.GetStringValue(para[0]).Split(',');
            int idx = Vars.GetIntegerValue(para[1]) - 1;

            Vars.SetValue(para[2], FloatArray[idx]);

            Vars.NextIdx++;
            return false;
        }
    }
}
