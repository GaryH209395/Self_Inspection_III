using CviVisaDriver;
using Self_Inspection.Class;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace Self_Inspection_III.TestCommands.DC_Source
{
    class WriteDCDev_Command : TestCommand
    {
        public WriteDCDev_Command()
        {
            Name = "WriteDCDev_Command";
            DeviceType = DeviceTypes.DC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Command", DataTypes.String, ParaTypes.Condition, ConstTypes.EditBox, "Command to be written into DC Source.")
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
