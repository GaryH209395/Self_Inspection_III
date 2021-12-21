using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.System_FlowControl
{
    class If_Then : TestCommand
    {
        public If_Then()
        {
            Name = "If_Then";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("1st Operand", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "1st Operand to be compared."),
                new Parameter("Operator", DataTypes.String, ParaTypes.Operator, ConstTypes.ComboList, "Used to compare between 1st and 2nd operands."),
                new Parameter("2st Operand", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "2st Operand to be compared."),
                new Parameter("Label", DataTypes.String, ParaTypes.Label, ConstTypes.ComboList, "If the comparison result is true, then jump to the defined label.\n" +
                "If label is 'TestPass' or 'TestFail', the current test item will terminate with pass or fail.'")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            bool compareResult = false;
            string[] para = ParaString.Split(','); //para[ParaIdx]

            double op1 = Vars.GetDoubleValue(para[0]);
            double op2 = Vars.GetDoubleValue(para[2]);

            switch (para[1])
            {
                case Parameter.Eql: compareResult = op1 == op2; break;
                case Parameter.Neq: compareResult = op1 != op2; break;
                case Parameter.Grt: compareResult = op1 > op2; break;
                case Parameter.Lst: compareResult = op1 < op2; break;
                case Parameter.Gne: compareResult = op1 >= op2; break;
                case Parameter.Lne: compareResult = op1 <= op2; break;
            }
            if (compareResult)
            {
                if (para[3] == "TestPass")
                {
                    Vars.NextIdx = Vars.Labels.Length + 1;
                }
                else
                if (para[3] == "TestFail")
                {
                    Vars.NextIdx = Vars.Labels.Length + 2;
                }
                else
                {
                    for (int i = 0; i < Vars.Labels.Length; i++)
                        if (Vars.Labels[i] == para[3])
                        { Vars.NextIdx = i; break; }
                }
            }
            else Vars.NextIdx++;

            return false;
        }
    }
}
