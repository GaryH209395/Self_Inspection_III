using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.AC_Source
{
    class SetAC_OutputState : TestCommand
    {
        public SetAC_OutputState()
        {
            Name = "SetAC_OutputState";
            DeviceType = DeviceTypes.AC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Output", DataTypes.Integer, ParaTypes.Constant, ConstTypes.ComboList, "Set output state.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("ON","ON"),
                        new EnumItem("OFF","OFF")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            StringBuilder Command = new StringBuilder();

            if (Regex.IsMatch(ModelName, @"61504"))
                Command.Append($"OUTP:STAT {FindEIValue(ParaString, 0)}");
            else
            if (Regex.IsMatch(ModelName, @"615..|618..|65.0"))
                Command.Append($"OUTP {FindEIValue(ParaString, 0)}");
            else
                throw new Exception($"{ModelName} invalid in SetAC_OutputState.Function()");

            WriteCommand(NIDriver, ModelName, Command);

            Vars.NextIdx++;
            return false;
        }
    }
}