using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Arithmetic
{
    class ADD : TestCommand
    {
        public ADD()
        {
            Name = "ADD";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Summand", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Variable or constant to be added."),
                new Parameter("Addend", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Variable or constant to add summand."),
                new Parameter("Sum", DataTypes.Float, ParaTypes.Condition, ConstTypes.NotAllow, "Return the sum to parameter.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            double op1 = Vars.GetDoubleValue(para[0]);
            double op2 = Vars.GetDoubleValue(para[1]);

            Vars.SetValue(para[2], op1 + op2);
            Vars.NextIdx++;

            return false;
        }
    }
}
