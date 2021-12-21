using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Threading;

namespace Self_Inspection_III.TestCommands.System_FlowControl
{
    class DelayS : TestCommand
    {
        public DelayS()
        {
            Name = "DelayS";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Delay Time", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Time to delay (sec).")               
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            Thread.Sleep((int)(Vars.GetDoubleValue(ParaString) * 1000));

            Vars.NextIdx++;
            return false;
        }
    }
}
