using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.DMM
{
    class Set3458A_FuncRange : TestCommand
    {
        public Set3458A_FuncRange()
        {
            Name = "Set3458A_FuncRange";
            DeviceType = DeviceTypes.DMM;
            Parameters = new List<Parameter>()
            {
                new Parameter("Mode", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, " 1.DC Voltage Mode\n 2.DC Current Mode\n 3.AC Voltage Mode\n 4.AC Current Mode\n")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("DC Voltage","DCV"),
                        new EnumItem("DC Current","DCI"),
                        new EnumItem("AC Voltage","ACV"),
                        new EnumItem("AC Current","ACI")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            WriteCommand(NIDriver, ModelName, "END ALWAYS");
            WriteCommand(NIDriver, ModelName, "PRESET NORM");
            WriteCommand(NIDriver, ModelName, FindEIValue(ParaString, 0));

            Vars.NextIdx++;
            return false;
        }
    }
}
