using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Self_Inspection_III.TestCommands.DMM
{
    class ReadDMMDev_Response : TestCommand
    {
        public ReadDMMDev_Response()
        {
            Name = "ReadDMMDev_Response";
            DeviceType = DeviceTypes.DMM;
            Parameters = new List<Parameter>()
            {
                new Parameter("Return value", DataTypes.String, ParaTypes.Result, ConstTypes.NotAllow, "Measurement of DMM.")
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
