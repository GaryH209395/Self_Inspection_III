using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.DC_Source
{
    class SetDC_Ilimit : TestCommand
    {
        public SetDC_Ilimit()
        {
            Name = "SetDC_Ilimit";
            DeviceType = DeviceTypes.DC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Current", DataTypes.Float, ParaTypes.Result, ConstTypes.EditBox, "Output current to be set. (A)")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            StringBuilder Command = new StringBuilder();
            if (Regex.IsMatch(ModelName, @"62[\d]{3}(P|H)")) // 62000P-Series, 62000H-Serires
                Command.Append("SOUR:CURR");
            else
            if (Regex.IsMatch(ModelName, @"6030A"))
                Command.Append("ISET");
            else
            if (Regex.IsMatch(ModelName, @"6671A"))
                Command.Append("CURR");
            Command.Append(" " + Vars.GetDoubleValue(ParaString));

            WriteCommand(NIDriver, ModelName, Command);

            Vars.NextIdx++;
            return false;
        }
    }
}
