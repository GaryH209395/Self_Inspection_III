using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.Fluke
{
    class WriteFluke_Command : TestCommand
    {
        public WriteFluke_Command()
        {
            Name = "WriteFluke_Command";
            DeviceType = DeviceTypes.Fluke;
            Parameters = new List<Parameter>()
            {
                new Parameter("Command", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Command to be written into Fluke.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("OPER","OPER"),
                        new EnumItem("STBY","STBY"),
                        new EnumItem("*CLS","*CLS"),
                        new EnumItem("RANGELCK OFF","RANGELCK OFF")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            WriteCommand(NIDriver, ModelName, ParaString.Replace("\"", ""));
            Vars.NextIdx++;
            return false;
        }
    }
}
