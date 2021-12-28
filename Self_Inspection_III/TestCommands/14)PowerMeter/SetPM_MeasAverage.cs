using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class SetPM_MeasAverage : TestCommand
    {
        public SetPM_MeasAverage()
        {
            Name = "SetPM_MeasAverage";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>()
            {
                new Parameter("Average", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Set the number of measurements over which the average calculation is to be performed.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("1", "1"),
                        new EnumItem("2", "2"),
                        new EnumItem("8", "8"),
                        new EnumItem("16", "16"),
                        new EnumItem("32", "32"),
                        new EnumItem("64", "64")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            WriteCommand(NIDriver, ModelName, "MEAS:AVER " + FindEIValue(ParaString, 0));
            Vars.NextIdx++;
            return false;
        }
    }
}
