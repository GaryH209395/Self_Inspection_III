using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Arithmetic
{
    class SUB : TestCommand
    {
        public SUB()
        {
            Name = "SUB";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Minute", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Variable or constant to be substracted."),
                new Parameter("Subtract", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Variable or constant to substract minute."),
                new Parameter("Difference", DataTypes.Float, ParaTypes.Condition, ConstTypes.NotAllow, "Return the difference to parameter.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            double op1 = Vars.GetDoubleValue(para[0]);
            double op2 = Vars.GetDoubleValue(para[1]);

            Vars.SetValue(para[2], op1 - op2);
            Vars.NextIdx++;

            return false;
        }
    }
}
