using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class SetPM_ShowChannel : TestCommand
    {
        public SetPM_ShowChannel()
        {
            Name = "SetPM_ShowChannel";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>();
            for (int i = 1; i <= 4; i++)
            {
                Parameter p = new Parameter("Para " + i, DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Channel will be displayed.") { EnumItems = new EnumItem[4] };
                for (int j = 1; j < p.EnumItems.Length; j++)
                    p.EnumItems[j] = new EnumItem($"{j}", $"{j}");
                Parameters.Add(p);
            }
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            WriteCommand(NIDriver, ModelName, "SHOW:CHAN " + ParaString);
            Vars.NextIdx++;
            return false;
        }
    }
}
