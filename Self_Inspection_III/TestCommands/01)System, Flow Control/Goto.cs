using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;


namespace Self_Inspection_III.TestCommands.System_FlowControl
{
    class Goto : TestCommand
    {
        public Goto()
        {
            Name = "Goto";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Label", DataTypes.String, ParaTypes.Label, ConstTypes.NotAllow, "Label.\n" +
                "If the label is 'TestFail', the current test will terminate with Fail.\n" +
                "If the label is 'TestPass', the current test will terminate with test result. ")
            };            
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            if (ParaString == "TestPass")
            {
                Vars.NextIdx = Vars.Labels.Length + 1;
            }
            else
            if (ParaString == "TestFail")
            {
                Vars.NextIdx = Vars.Labels.Length + 2;
            }
            else
            {
                for (int i = 0; i < Vars.Labels.Length; i++)
                    if (Vars.Labels[i] == ParaString)
                        Vars.NextIdx = i;
            }
            return false;
        }
    }
}
