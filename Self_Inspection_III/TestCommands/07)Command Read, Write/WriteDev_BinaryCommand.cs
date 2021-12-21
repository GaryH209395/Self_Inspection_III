using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;

namespace Self_Inspection_III.TestCommands._07_Command_Read__Write
{
    class WriteDev_BinaryCommand : TestCommand
    {
        public WriteDev_BinaryCommand()
        {
            Name = "WriteDev_BinaryCommand";
            DeviceType = DeviceTypes.All;
            Parameters = new List<Parameter>()
            {
                new Parameter("Command", DataTypes.String, ParaTypes.Constant, ConstTypes.EditBox, "Command to be written into GPIB device.")
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
