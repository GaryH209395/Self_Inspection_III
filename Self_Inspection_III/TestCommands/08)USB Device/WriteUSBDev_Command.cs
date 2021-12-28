using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.USB_Device
{
    class WriteUSBDev_Command : TestCommand
    {
        public WriteUSBDev_Command()
        {
            Name = "WriteUSBDev_Command";
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
