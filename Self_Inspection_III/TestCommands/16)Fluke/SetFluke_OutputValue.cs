using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;

namespace Self_Inspection_III.TestCommands.Fluke
{
    class SetFluke_OutputValue : TestCommand
    {
        public SetFluke_OutputValue()
        {
            Name = "SetFluke_OutputValue";
            DeviceType = DeviceTypes.Fluke;
            Parameters = new List<Parameter>()
            {
                new Parameter("Mode", DataTypes.Float, ParaTypes.Constant, ConstTypes.ComboList, "Output Mode.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("Voltage", "V"),
                        new EnumItem("Current", "A")
                    }
                },
                new Parameter("Value", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Output Value.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            WriteCommand(NIDriver, ModelName, $"OUT {Vars.GetDoubleValue(para[1])}{FindEIValue(para[0], 0)}");

            Vars.NextIdx++;
            return false;
        }
    }
}
