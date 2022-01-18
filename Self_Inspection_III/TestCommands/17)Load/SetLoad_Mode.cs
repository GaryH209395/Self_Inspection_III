using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Threading;

namespace Self_Inspection_III.TestCommands.Load
{
    class SetLoad_Mode : TestCommand
    {
        public SetLoad_Mode()
        {
            Name = "SetLoad_Mode";
            DeviceType = DeviceTypes.Load;
            Parameters = new List<Parameter>()
            {
                new Parameter("Mode",  DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Set the operational mode of the electronic load.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("CCL","CURR"),
                        new EnumItem("CCH","CURR"),
                        new EnumItem("CPL","POW"),
                        new EnumItem("CPH","POW"),
                        new EnumItem("CRL","RES"),
                        new EnumItem("CRH","RES")
                    }
                },
                new Parameter("Level",  DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Set the static value level in the constant resistance mode.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("L1","L1"),
                        new EnumItem("L2","L2")
                    }
                },
                new Parameter("Value",  DataTypes.Float, ParaTypes.Condition, ConstTypes.EditBox, "Set value in the constant resistance mode.")                
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            WriteCommand(NIDriver, ModelName, $"MODE {para[0]}");
            Thread.Sleep(300);
            WriteCommand(NIDriver, ModelName, $"{FindEIValue(para[0], 0)}:STAT:{para[1]} {Vars.GetDoubleValue(para[2])}");
            Vars.NextIdx++;
            return false;
        }
    }
}
