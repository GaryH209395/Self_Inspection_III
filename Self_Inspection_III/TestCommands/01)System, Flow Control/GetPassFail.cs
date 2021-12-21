using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Self_Inspection_III.TestCommands.System_FlowControl
{
    class GetPassFail : TestCommand
    {
        public GetPassFail()
        {
            Name = "GetPassFail";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Title", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Pop-up dialog title."),
                new Parameter("Hint", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Pop-up dialog hint text."),
                new Parameter("Pass/Fail", DataTypes.Integer, ParaTypes.Temporary, ConstTypes.NotAllow, "Return Pass or Fail.\n" +
                "1. Pass\n" +
                "2. Fail")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            string Title = Vars.GetStringValue(para[0]);
            string Hint = Vars.GetStringValue(para[1]);

            Vars.SetValue(para[2], MessageBox.Show(Title, Hint, MessageBoxButtons.OKCancel) == DialogResult.OK ? "1" : "2");
            Vars.NextIdx++;
            return false;
        }
    }
}
