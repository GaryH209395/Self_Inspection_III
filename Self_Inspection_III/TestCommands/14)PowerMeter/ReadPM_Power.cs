using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class ReadPM_Power : TestCommand
    {
        public ReadPM_Power()
        {
            Name = "ReadPM_Power";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>()
            {
                new Parameter("Function", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "1.True Power (Real)\n2.Power Factor (PFactor)\n3.Apparent\n4.Reactive\n5.Average\n6.Energy in Joule (Need ENER:MODE)")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("Real","REAL"),
                        new EnumItem("PFactor","PFAC"),
                        new EnumItem("Apparent","APP"),
                        new EnumItem("Reactive","REAC"),
                        new EnumItem("Average","DC"),
                        new EnumItem("Energy","ENER")
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
            WriteCommand(NIDriver, ModelName, $"FETC:POW:{FindEIValue(para[0], 0)}? " + FindEIValue(para[1], 1));
            Vars.SetValue(para[2], ReadResponse(NIDriver));

            Vars.NextIdx++;
            return false;
        }
    }
}
