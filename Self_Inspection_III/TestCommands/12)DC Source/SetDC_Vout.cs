using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.DC_Source
{
    class SetDC_Vout : TestCommand
    {
        public SetDC_Vout()
        {
            Name = "SetDC_Vout";
            DeviceType = DeviceTypes.DC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Voltage", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Output voltage to be set. (V)")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            StringBuilder Command = new StringBuilder();
            if (Regex.IsMatch(ModelName, @"62[\d]{3}(P|H)")) // 62000P-Series, 62000H-Serires
                Command.Append("SOUR:VOLT");
            else
            if (Regex.IsMatch(ModelName, @"6030A"))
                Command.Append("VSET");
            else
            if (Regex.IsMatch(ModelName, @"6671A"))
                Command.Append("VOLT");
            Command.Append(" " + Vars.GetDoubleValue(ParaString));

            WriteCommand(NIDriver, ModelName, Command);

            Vars.NextIdx++;
            return false;
        }
    }
}
