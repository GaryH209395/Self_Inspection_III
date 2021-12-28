using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.AC_Source
{
    class SetAC_Vout : TestCommand
    {
        public SetAC_Vout()
        {
            Name = "SetAC_Vout";
            DeviceType = DeviceTypes.AC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("AC/DC", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Select Output AC or DC Voltage.")
                {
                     EnumItems = new EnumItem[]
                    {
                        new EnumItem("AC","AC"),
                        new EnumItem("DC","DC")
                    }
                },
                new Parameter("Voltage", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Output voltage to be set. (V)")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            StringBuilder Command = new StringBuilder();

            if(Regex.IsMatch(ModelName,@"615.."))
                Command.Append($"VOLT:{FindEIValue(para[0], 0)}");
            else
            if (Regex.IsMatch(ModelName, @"618..|65.0"))
                Command.Append("VOLT:LEV");
            else
                throw new Exception($"{ModelName} invalid in SetAC_Vout.Function()");
          
            Command.Append(" " + Vars.GetDoubleValue(para[1]));
            WriteCommand(NIDriver, ModelName, Command);

            Vars.NextIdx++;
            return false;
        }
    }
}
