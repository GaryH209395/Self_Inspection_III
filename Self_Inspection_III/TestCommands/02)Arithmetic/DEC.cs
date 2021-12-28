using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Arithmetic
{
    class DEC : TestCommand
    {
        public DEC()
        {
            Name = "DEC";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Value", DataTypes.Float, ParaTypes.Condition, ConstTypes.NotAllow, "Variable to be decreased.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            double d = Vars.GetDoubleValue(ParaString) - 1;
            Vars.SetValue(ParaString, d);
            Vars.NextIdx++;
            return false;
        }
    }
}
