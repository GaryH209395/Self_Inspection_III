using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;

namespace Self_Inspection_III.TestCommands.Arithmetic
{
    class ABS : TestCommand
    {
        public ABS()
        {
            Name = "ABS";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Numeric", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Variable or constant to do 'Absolute'."),
                new Parameter("Return Value", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Return the absolute value of parameter.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            double op1 = Vars.GetDoubleValue(para[0]);

            if (op1 < 0) op1 *= -1;

            Vars.SetValue(para[1], op1);
            Vars.NextIdx++;

            return false;
        }
    }
}
