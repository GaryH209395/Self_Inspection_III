using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.StringFunc
{
    class StrCat : TestCommand
    {
        public StrCat()
        {
            Name = "StrCat";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Previos", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Previos string"),
                new Parameter("Latter", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Latter string"),
                new Parameter("Variable", DataTypes.String, ParaTypes.Condition, ConstTypes.NotAllow, "Return the concatenated sring")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            string str1 = Vars.GetStringValue(para[0]);
            string str2 = Vars.GetStringValue(para[1]);

            Vars.SetValue(para[2], str1 + str2);
            Vars.NextIdx++;
            return false;
        }
    }
}
