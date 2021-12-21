using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Arithmetic
{
    class MUL : TestCommand
    {
        public MUL()
        {
            Name = "MUL";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Multiplicand", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Variable or constant to be multiplied."),
                new Parameter("Multiplier", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Variable or constant to multiply multiplicand."),
                new Parameter("Product", DataTypes.Float, ParaTypes.Condition, ConstTypes.NotAllow, "Return the product to parameter.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            double op1 = Vars.GetDoubleValue(para[0]);
            double op2 = Vars.GetDoubleValue(para[1]);

            Vars.SetValue(para[2], op1 * op2);
            Vars.NextIdx++;

            return false;
        }
    }
}
