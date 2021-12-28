using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.System_FlowControl
{
    class InputTextbox : TestCommand
    {
        public InputTextbox()
        {
            Name = "InputTextbox";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Message", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Message to be displayed."),
                new Parameter("Input Var", DataTypes.String, ParaTypes.Result, ConstTypes.NotAllow, "Input value to variable.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            DlgInputTextbox dlgInputTextbox = new DlgInputTextbox(Vars.GetStringValue(para[0]));
            if (dlgInputTextbox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Vars.SetValue(para[1], dlgInputTextbox.Value);
            }

            Vars.NextIdx++;
            return false;
        }
    }
}
