using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.AC_Source
{
    class SetAC_Frequency : TestCommand
    {
        public SetAC_Frequency()
        {
            Name = "SetAC_Frequency";
            DeviceType = DeviceTypes.AC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Frequency", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "AC Frequency to be set. (Hz)")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {            
            WriteCommand(NIDriver, ModelName, "FREQ " + Vars.GetDoubleValue(ParaString));
            Vars.NextIdx++;
            return false;
        }
    }
}
