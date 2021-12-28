using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Arithmetic
{
    class SET : TestCommand
    {
        public SET()
        {
            Name = "SET";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Value", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Value to set."),
                new Parameter("Variable", DataTypes.Float, ParaTypes.Condition, ConstTypes.NotAllow, "Variable to be set.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');    
            
            Vars.SetValue(para[1], Vars.GetDoubleValue(para[0]));
            Vars.NextIdx++;
            return false;
        }
    }
}
