using System;
using System.Collections.Generic;
using System.Threading;
using CviVisaDriver;
using Self_Inspection.Class;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;

namespace Self_Inspection_III.TestCommands.DC_Source
{
    class ReadDCDev_Response : TestCommand
    {
        public ReadDCDev_Response()
        {
            Name = "ReadDCDev_Response";
            DeviceType = DeviceTypes.DC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Return value", DataTypes.String, ParaTypes.Result, ConstTypes.NotAllow, "Measurement of DC Source.")
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
