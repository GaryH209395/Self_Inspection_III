using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using CviVisaDriver;
using Self_Inspection.Class;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;

namespace Self_Inspection_III.TestCommands.DC_Source
{
    class ReadDC_Vout : TestCommand
    {
        public ReadDC_Vout()
        {
            Name = "ReadDC_Iout";
            DeviceType = DeviceTypes.DC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Return value", DataTypes.Float, ParaTypes.Result, ConstTypes.NotAllow, "Return output voltage reading (V).")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            StringBuilder Command = new StringBuilder();
            if (Regex.IsMatch(ModelName, @"62[\d]{3}(P|H)")) // 62000P-Series, 62000H-Serires
                Command.Append("MEAS:VOLT?");
            else
            if (Regex.IsMatch(ModelName, @"6030A"))
                Command.Append("");
            else
            if (Regex.IsMatch(ModelName, @"6671A"))
                Command.Append("");

            WriteCommand(NIDriver, ModelName, Command);
            Vars.SetValue(ParaString, ReadResponse(NIDriver));

            Vars.NextIdx++;
            return false;
        }
    }
}
