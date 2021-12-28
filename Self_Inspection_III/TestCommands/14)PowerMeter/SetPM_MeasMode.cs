using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class SetPM_MeasMode : TestCommand
    {
        public SetPM_MeasMode()
        {
            Name = "SetPM_MeasMode";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>()
            {
                new Parameter("Mode", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Set the mode of measure.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("Window", "WINDOW"),
                        new EnumItem("Average", "AVERAGE")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            WriteCommand(NIDriver, ModelName, "MEAS:MODE " + FindEIValue(ParaString, 0));
            Vars.NextIdx++;
            return false;
        }
    }
}
