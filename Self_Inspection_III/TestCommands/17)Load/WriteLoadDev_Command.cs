using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Load
{
    class WriteLoadDev_Command : TestCommand
    {
        public WriteLoadDev_Command()
        {
            Name = "WriteLoadDev_Command";
            DeviceType = DeviceTypes.Load;
            Parameters = new List<Parameter>()
            {
                new Parameter("Command", DataTypes.String, ParaTypes.Constant, ConstTypes.EditBox, "Command to be written into Load device.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            WriteCommand(NIDriver, ModelName, ParaString.Replace("\"", ""));
            Vars.NextIdx++;
            return false;
        }
    }
}
