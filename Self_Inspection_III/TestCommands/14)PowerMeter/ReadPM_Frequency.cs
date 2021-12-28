using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class ReadPM_Frequency : TestCommand
    {
        public ReadPM_Frequency()
        {
            Name = "ReadPM_Frequency";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>()
            {
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
            WriteCommand(NIDriver, ModelName, "FETC:FREQ? " + FindEIValue(para[0], 0));
            Vars.SetValue(para[1], ReadResponse(NIDriver));

            Vars.NextIdx++;
            return false;
        }
    }
}
