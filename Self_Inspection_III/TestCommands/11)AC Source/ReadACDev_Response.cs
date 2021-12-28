using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.AC_Source
{
    class ReadACDev_Response : TestCommand
    {
        public ReadACDev_Response()
        {
            Name = "ReadACDev_Response";
            DeviceType = DeviceTypes.AC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Return value", DataTypes.String, ParaTypes.Result, ConstTypes.NotAllow, "Measurement of AC Source.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            Vars.SetValue(ParaString, ReadResponse(NIDriver));
            Vars.NextIdx++;
            return false;
        }
    }
}
