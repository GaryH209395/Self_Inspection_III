using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class SetPM_InputCT : TestCommand
    {
        public SetPM_InputCT()
        {
            Name = "SetPM_InputCT";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>()
            {
                new Parameter("CT", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "CT Function")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("ON", "ON"),
                        new EnumItem("OFF", "OFF")
                    }
                },
                new Parameter("Ratio", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Set the CT ratio.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            WriteCommand(NIDriver, ModelName, "INP:CT " + FindEIValue(para[0], 0));

            if (para[0] == "ON")
                WriteCommand(NIDriver, ModelName, "INP:CT:RAT " + Vars.GetDoubleValue(para[1]));

            Vars.NextIdx++;
            return false;
        }
    }
}