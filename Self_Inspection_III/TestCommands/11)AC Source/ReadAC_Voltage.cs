using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.AC_Source
{
    class ReadAC_Voltage : TestCommand
    {
        public ReadAC_Voltage()
        {
            Name = "ReadAC_Voltage";
            DeviceType = DeviceTypes.AC_Source;
            Parameters = new List<Parameter>()
            {              
                new Parameter("Return value", DataTypes.Float, ParaTypes.Result, ConstTypes.NotAllow, "Current (A).")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            WriteCommand(NIDriver, ModelName, "MEAS:VOLT:AC?");
            Vars.SetValue(ParaString, ReadResponse(NIDriver));
            Vars.NextIdx++;
            return false;
        }
    }
}
