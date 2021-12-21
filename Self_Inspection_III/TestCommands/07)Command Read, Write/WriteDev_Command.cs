using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Command_ReadWrite
{
    class WriteDev_Command : TestCommand
    {
        public WriteDev_Command()
        {
            Name = "WriteDev_Command";
            DeviceType = DeviceTypes.All;
            Parameters = new List<Parameter>()
            {
                new Parameter("Command", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Command to be written into GPIB device.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            WriteCommand(NIDriver, ModelName, Vars.GetStringValue(ParaString));
            Vars.NextIdx++;
            return false;
        }
    }
}
