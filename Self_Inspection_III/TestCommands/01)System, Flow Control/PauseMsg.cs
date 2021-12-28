using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Self_Inspection_III.TestCommands.System_FlowControl
{
    class PauseMsg : TestCommand
    {
        public PauseMsg()
        {
            Name = "PauseMsg";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Title", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Title of the dialog."),
                new Parameter("Message", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Message to be displayed.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(','); //para[ParaIdx]
            string Title = Vars.GetStringValue(para[0]);
            string Msg = Vars.GetStringValue(para[1]);
            
            MessageBox.Show(Msg, Title);

            Vars.NextIdx++;
            return false;
        }
    }
}
