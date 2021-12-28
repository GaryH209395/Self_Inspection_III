using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.DC_Source
{
    class SetDC_OutputState : TestCommand
    {
        public SetDC_OutputState()
        {
            Name = "SetDC_OutputState";
            DeviceType = DeviceTypes.DC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Output", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Set output state.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("ON", "True"),
                        new EnumItem("OFF", "False")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            bool OutpState = Convert.ToBoolean(FindEIValue(ParaString, 0));

            StringBuilder Command = new StringBuilder();
            if (Regex.IsMatch(ModelName, @"62[\d]{3}(P|H)")) // 62000P-Series, 62000H-Serires
                Command.Append($"CONF:OUTP {(OutpState ? "ON" : "OFF")}");
            else
            if (Regex.IsMatch(ModelName, @"6030A"))
                Command.Append(OutpState ? "OUT ON" : "CLR");
            else
            if (Regex.IsMatch(ModelName, @"6671A"))
                Command.Append($"OUTP {(OutpState ? "ON" : "OFF")}");

            WriteCommand(NIDriver, ModelName, Command);

            Vars.NextIdx++;
            return false;
        }
    }
}
