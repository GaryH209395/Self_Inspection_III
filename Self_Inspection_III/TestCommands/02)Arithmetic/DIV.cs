using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Arithmetic
{
    class DIV : TestCommand
    {
        public DIV()
        {
            Name = "DIV";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Dividend", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Variable or constant to be divided."),
                new Parameter("Divisor", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Variable or constant to devide devidend."),
                new Parameter("Quotient", DataTypes.Float, ParaTypes.Condition, ConstTypes.NotAllow, "Return the quotient to parameter.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            double op1 = Vars.GetDoubleValue(para[0]);
            double op2 = Vars.GetDoubleValue(para[1]);

            Vars.SetValue(para[2], op1 / op2);
            Vars.NextIdx++;

            return false;
        }
    }
}
