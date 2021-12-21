using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.PowerMeter
{
    class SetPM_ShowItem : TestCommand
    {
        public SetPM_ShowItem()
        {
            Name = "SetPM_ShowItem";
            DeviceType = DeviceTypes.PowerMeter;
            Parameters = new List<Parameter>()
            {
                new Parameter("Item 1", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Which item will be displayed.\n" +
                "1. V (Voltage)\n2. I (Current)\n3. W (Power)\n4. IS (?)\n5. VPK+ (?)\n6. PF (?)\n7. F (?)")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("V", "V"),
                        new EnumItem("I", "I"),
                        new EnumItem("W", "W"),
                        new EnumItem("IS", "IS"),
                        new EnumItem("VPK+", "VPK+"),
                        new EnumItem("PF", "PF"),
                        new EnumItem("F", "F")
                    }
                },
                new Parameter("Item 2", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Which item will be displayed.\n" +
                "1. V (Voltage)\n2. I (Current)\n3. W (Power)\n4. IS (?)\n5. VPK- (?)\n6. EFF (?)\n7. CFI (?)")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("V", "V"),
                        new EnumItem("I", "I"),
                        new EnumItem("W", "W"),
                        new EnumItem("IS", "IS"),
                        new EnumItem("VPK-", "VPK-"),
                        new EnumItem("EFF", "EFF"),
                        new EnumItem("CFI", "CFI")
                    }
                },
                new Parameter("Item 3", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Which item will be displayed.\n" +
                "1. V (Voltage)\n2. I (Current)\n3. W (Power)\n4. IS (?)\n5. E (?)\n6. THDV (?)\n7. THDI (?)")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("V", "V"),
                        new EnumItem("I", "I"),
                        new EnumItem("W", "W"),
                        new EnumItem("IS", "IS"),
                        new EnumItem("E", "E"),
                        new EnumItem("THDV", "THDV"),
                        new EnumItem("THDI", "THDI")
                    }
                },
                new Parameter("Item 4", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Which item will be displayed.\n" +
                "1. V (Voltage)\n2. I (Current)\n3. W (Power)\n4. PF (?)\n5. VA (?)\n6. VAR (?)")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("V", "V"),
                        new EnumItem("I", "I"),
                        new EnumItem("W", "W"),
                        new EnumItem("PF", "PF"),
                        new EnumItem("VA", "VA"),
                        new EnumItem("VAR", "VAR")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            WriteCommand(NIDriver, ModelName, "SHOW:ITEM " + ParaString);
            Vars.NextIdx++;
            return false;
        }
    }
}

