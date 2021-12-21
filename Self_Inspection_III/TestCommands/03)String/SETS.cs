using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.StringFunc
{
    class SETS : TestCommand
    {
        public SETS()
        {
            Name = "SETS";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("String", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "String to SETS."),
                new Parameter("Variable", DataTypes.String, ParaTypes.Condition, ConstTypes.NotAllow, "Variable to be SETS.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(','); //para[ParaIdx]               

            string str = Vars.GetStringValue(para[0]);

            Vars.SetValue(para[1], str);
            Vars.NextIdx++;
            return false;
        }
    }
}
