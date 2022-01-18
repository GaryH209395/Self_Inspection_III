using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Load
{
    class ReadLoadDev_Response : TestCommand
    {
        public ReadLoadDev_Response()
        {
            Name = "ReadLoadDev_Response";
            DeviceType = DeviceTypes.Load;
            Parameters = new List<Parameter>()
            {
                new Parameter("Return value", DataTypes.String, ParaTypes.Result, ConstTypes.NotAllow, "Measurement of Load.")
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
