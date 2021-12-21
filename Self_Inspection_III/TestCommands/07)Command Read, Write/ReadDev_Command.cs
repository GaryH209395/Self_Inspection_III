using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Command_ReadWrite
{
    class ReadDev_Command : TestCommand
    {
        public ReadDev_Command()
        {
            Name = "ReadDev_Command";
            DeviceType = DeviceTypes.All;
            Parameters = new List<Parameter>()
            {
                new Parameter("Command", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Command to read GPIB device."),
                new Parameter("Variable", DataTypes.String, ParaTypes.Condition, ConstTypes.NotAllow, "Return sring from GPIB device.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            WriteCommand(NIDriver, ModelName, Vars.GetStringValue(para[0]));
            Vars.SetValue(para[1], ReadResponse(NIDriver));
            Vars.NextIdx++;
            return false;
        }
    }
}
