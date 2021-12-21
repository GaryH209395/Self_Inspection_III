using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.AC_Source
{
    class ReadAC_Current : TestCommand
    {
        public ReadAC_Current()
        {
            Name = "ReadAC_Current";
            DeviceType = DeviceTypes.AC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Measurement Function", DataTypes.Integer, ParaTypes.Constant, ConstTypes.ComboList,
                "1. RMS\n2. Peak+\n3. Peak-\n4. Inrush")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("RMS","0"),
                        new EnumItem("Peak+","1"),
                        new EnumItem("Peak-","2"),
                        new EnumItem("Inrush","3")
                    }
                },
                new Parameter("Return value", DataTypes.Float, ParaTypes.Result, ConstTypes.NotAllow, "Current (A).")
            };
        }
        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
         
            WriteCommand(NIDriver, ModelName, "MEAS:CURR:AC?");
            Vars.SetValue(para[1], ReadResponse(NIDriver));
            Vars.NextIdx++;
            return false;
        }
    }
}
