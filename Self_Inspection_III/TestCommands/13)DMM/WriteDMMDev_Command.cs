using System;
using System.Collections.Generic;
using System.Threading;
using CviVisaDriver;
using Self_Inspection.Class;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;

namespace Self_Inspection_III.TestCommands.DMM
{
    class WriteDMMDev_Command : TestCommand
    {
        public WriteDMMDev_Command()
        {
            Name = "WriteDMMDev_Command";
            DeviceType = DeviceTypes.DMM;
            Parameters = new List<Parameter>()
            {
                new Parameter("Command", DataTypes.String, ParaTypes.Constant, ConstTypes.EditBox, "Command to be written into DMM device.")               
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
