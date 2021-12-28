using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using Self_Inspection_III.TestCommands.DC_Source;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.DC_Source
{
    class SetDC_OutputMode : TestCommand
    {
        public SetDC_OutputMode()
        {
            Name = "SetDC_OutputMode";
            DeviceType = DeviceTypes.DC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Voltage", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Output voltage to be set. (V)"),
                new Parameter("Current", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Output current to be set. (A)")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            int FunIdx = Vars.NextIdx;
            string[] para = ParaString.Split(',');

            new SetDC_Vout().DoFunction(para[0], ModelName, NIDriver, ref Vars);
            new SetDC_Ilimit().DoFunction(para[1], ModelName, NIDriver, ref Vars);

            Vars.NextIdx = FunIdx + 1;
            return false;
        }
    }
}
