using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class SetPM_InputWiring : TestCommand
    {
        public SetPM_InputWiring()
        {
            Name = "SetPM_InputWiring";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>()
            {
                new Parameter("Wiring", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Input wiring mode.\n" +
                "1. 1P2W\n2. 1P3W\n3. 3P3W\n4. 3P4W\n5. 3V3A")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("1P2W", "0"),
                        new EnumItem("1P3W", "1"),
                        new EnumItem("3P3W", "2"),
                        new EnumItem("3P4W", "3"),
                        new EnumItem("3V3A", "4")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            WriteCommand(NIDriver, ModelName, "INP:WIR " + FindEIValue(ParaString, 0));
            Vars.NextIdx++;
            return false;
        }
    }
}
