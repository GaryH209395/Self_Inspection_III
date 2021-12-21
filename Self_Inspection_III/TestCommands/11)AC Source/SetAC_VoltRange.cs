using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.AC_Source
{
    class SetAC_VoltRange : TestCommand
    {
        public SetAC_VoltRange()
        {
            Name = "SetAC_VoltRange";
            DeviceType = DeviceTypes.AC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Range", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Output voltage range to be set.\n" +
                "1. LOW (150V)\n" +
                "2. HIGH (300V)\n" +
                "3. AUTO")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("LOW", "LOW"),
                        new EnumItem("HIGH", "HIGH"),
                        new EnumItem("AUTO", "AUTO")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            StringBuilder Command = new StringBuilder();

            if (Regex.IsMatch(ModelName, @"6530")) return false;
            else
            if (Regex.IsMatch(ModelName, @"615..|618..|65.0"))
                Command.Append("VOLT:RANG");
            else
                throw new Exception($"{ModelName} invalid in SetAC_VoltRange.Function()");

            Command.Append(" " + FindEIValue(ParaString, 0));
            WriteCommand(NIDriver, ModelName, Command);

            Vars.NextIdx++;
            return false;
        }
    }
}
