using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class WritePMDev_Command : TestCommand
    {
        public WritePMDev_Command()
        {
            Name = "WritePMDev_Command";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>()
            {
                new Parameter("Command", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Command to be written into Powermeter.")
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
