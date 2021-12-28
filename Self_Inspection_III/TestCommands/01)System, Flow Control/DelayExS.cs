using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Self_Inspection_III.TestCommands.System_FlowControl
{
    class DelayExS : TestCommand
    {
        public DelayExS()
        {
            Name = "DelayExS";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Delay Time", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Time to delay (sec)."),
                new Parameter("Title", DataTypes.String, ParaTypes.Constant, ConstTypes.EditBox, "Title of dialog."),
                new Parameter("Termination", DataTypes.Integer, ParaTypes.Temporary, ConstTypes.NotAllow, 
                "Returns status of termination.\n" +
                "1. Finish\n" +
                "2. User Break")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            double delayTime = Vars.GetDoubleValue(para[0]);
            string Title = Vars.GetStringValue(para[1]);

            Vars.SetValue(para[2], new DlgDelayExS(Title, (int)delayTime).ShowDialog() == DialogResult.OK ? "1" : "2");
            Vars.NextIdx++;
            return false;
        }
    }
}
