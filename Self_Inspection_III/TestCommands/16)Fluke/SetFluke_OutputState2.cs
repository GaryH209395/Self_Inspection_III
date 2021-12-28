using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Threading;

namespace Self_Inspection_III.TestCommands.Fluke
{
    class SetFluke_OutputState2 : TestCommand
    {
        public SetFluke_OutputState2()
        {
            Name = "SetFluke_OutputState2";
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
                new Parameter("Value", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Output Value."),
                new Parameter("Freq", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Output Frequency (Hz).\n Empty Allowed."),
                new Parameter("DPF", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Power Factor (PF).\n Default: 1.0"),
                new Parameter("Output", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Output State.\n" +
                " ON\n" +
                " 1. *CLS\n" +
                " 2. RANGELCK OFF\n" +
                " 3. OUT ?V/A;DPF 1.0,LEAD\n"+
                " 4. OPER\n" +
                " OFF\n" +
                " 1. STBY")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("ON", "ON"),
                        new EnumItem("OFF", "OFF")
                    }
                }
            };
        }

        enum Para { Mode = 0, Value, Freq, DPF, Output }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            if (FindEIValue(para[(int)Para.Output], (int)Para.Output) == "ON")
            {
                WriteCommand(NIDriver, ModelName, "*CLS");

                Thread.Sleep(500);
                WriteCommand(NIDriver, ModelName, "RANGELCK OFF");
                WriteCommand(NIDriver, ModelName, $"OUT {Vars.GetDoubleValue(para[(int)Para.Value])}{FindEIValue(para[(int)Para.Mode], (int)Para.Mode)}");
                WriteCommand(NIDriver, ModelName, $"{(string.IsNullOrEmpty(para[(int)Para.DPF]) ? string.Empty : $",{Vars.GetDoubleValue(para[(int)Para.Freq])}")}HZ");
                WriteCommand(NIDriver, ModelName, $";DPF {(string.IsNullOrEmpty(para[(int)Para.DPF]) ? "1.0" : $"{Vars.GetDoubleValue(para[(int)Para.DPF])}")}");
                WriteCommand(NIDriver, ModelName, ",LEAD");

                Thread.Sleep(500);
                WriteCommand(NIDriver, ModelName, "OPER");
            }
            else
                WriteCommand(NIDriver, ModelName, "STBY");

            Vars.NextIdx++;
            return false;
        }
    }
}
