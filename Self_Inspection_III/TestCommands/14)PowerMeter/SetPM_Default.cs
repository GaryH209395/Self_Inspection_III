using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class SetPM_Default : TestCommand
    {
        public SetPM_Default()
        {
            Name = "SetPM_Default";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>()
            {
                new Parameter("Channel", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Show channel select.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("default","default")
                    }
                },
                new Parameter("Item", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Show item in each channel.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("Voltage","V"),
                        new EnumItem("Current","I"),
                        new EnumItem("Power","W")
                    }
                },
                new Parameter("CT Ratio", DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Set the CT ratio.")
            };
            Parameters.AddRange(new SetPM_InputWiring().Parameters);
            Parameters.AddRange(new SetPM_MeasMode().Parameters);
            Parameters.AddRange(new SetPM_MeasAverage().Parameters);
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            int FunIdx = Vars.NextIdx, idx = 1;

            new SetPM_ShowChannel().DoFunction("1,2,3,4", ModelName, NIDriver, ref Vars);

            string s = FindEIValue(para[idx++], 1);
            new SetPM_ShowItem().DoFunction($"{s},{s},{s},{s}", ModelName, NIDriver, ref Vars);

            new SetPM_InputCT().DoFunction($"ON,{para[idx++]}", ModelName, NIDriver, ref Vars);

            SetPM_InputWiring setInputWiring = new SetPM_InputWiring();
            setInputWiring.DoFunction(TCDoFunc(setInputWiring, para, ref idx), ModelName, NIDriver, ref Vars);

            SetPM_MeasMode setMeasMode = new SetPM_MeasMode();
            setMeasMode.DoFunction(TCDoFunc(setMeasMode, para, ref idx), ModelName, NIDriver, ref Vars);

            SetPM_MeasAverage setMeasAvg = new SetPM_MeasAverage();
            setMeasAvg.DoFunction(TCDoFunc(setMeasAvg, para, ref idx), ModelName, NIDriver, ref Vars);

            Vars.NextIdx = FunIdx + 1;
            return false;
        }
    }
}
