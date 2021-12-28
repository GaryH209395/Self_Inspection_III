using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class ReadPM_Current : TestCommand
    {
        public ReadPM_Current()
        {
            Name = "ReadPM_Current";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>()
            {
                new Parameter("Function", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "1.R.M.S\n2.Peak+\n3.Peak-\n4.DC\n5.Inrush\n6.Crest factor\n7.Total Harmonic Distortion (THD)")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("R.M.S","RMS"),
                        new EnumItem("Peak+","PEAk+"),
                        new EnumItem("Peak-","PEAk-"),
                        new EnumItem("DC","DC"),
                        new EnumItem("Inrush","INR"),
                        new EnumItem("Crest factor","CRES"),
                        new EnumItem("THD","THD")
                    }
                },
                new Parameter("Channel", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "The query channel")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("All","0"),
                        new EnumItem("Chan 1","1"),
                        new EnumItem("Chan 2","2"),
                        new EnumItem("Chan 3","3"),
                        new EnumItem("Chan 4","4")
                    }
                },
                new Parameter("Return value", DataTypes.Float, ParaTypes.Result, ConstTypes.NotAllow, "Variable which the return value will be assigned to.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            WriteCommand(NIDriver, ModelName, $"FETC:CURR:{FindEIValue(para[0], 0)}? " + FindEIValue(para[1], 1));
            Vars.SetValue(para[2], ReadResponse(NIDriver));

            Vars.NextIdx++;
            return false;
        }
    }
}
