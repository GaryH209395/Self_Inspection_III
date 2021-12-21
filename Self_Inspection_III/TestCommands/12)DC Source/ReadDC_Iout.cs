using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.DC_Source
{
    class ReadDC_Iout : TestCommand
    {
        public ReadDC_Iout()
        {
            Name = "ReadDC_Iout";
            DeviceType = DeviceTypes.DC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Return value", DataTypes.Float, ParaTypes.Result, ConstTypes.NotAllow, "Return output current reading (A).")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            StringBuilder Command = new StringBuilder();
            if (Regex.IsMatch(ModelName, @"62[\d]{3}(P|H)")) // 62000P-Series, 62000H-Serires
                Command.Append("MEAS:CURR?");
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
