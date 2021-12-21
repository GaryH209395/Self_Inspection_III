using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.AC_Source
{
    class SetAC_Instrument : TestCommand
    {
        public SetAC_Instrument()
        {
            Name = "SetAC_Instrument";
            DeviceType = DeviceTypes.AC_Source;
            Parameters = new List<Parameter>()
            {
                new Parameter("Couple", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Set coupled phases for programming.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("ALL", "ALL"),
                        new EnumItem("NONE", "NONE")
                    }
                },
                new Parameter("Phase", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Select output phase to program.\nFor model 6590 only.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("1", "1"),
                        new EnumItem("2", "2"),
                        new EnumItem("3", "3")
                    }
                },
                new Parameter("Edit", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "If INST:EDIT ALL has been programmed, it will be set to all phases.\nINST:EDIT EACH command disables EDIT ALL command.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("ALL", "ALL"),
                        new EnumItem("EACH", "EACH")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            if (Regex.IsMatch(ModelName, @"6530")) return false;
            else
            if (Regex.IsMatch(ModelName, @"6560")) SetCouple();
            else
            if (Regex.IsMatch(ModelName, @"6590")) { SetCouple(); SetPhase(); }
            else
            if (Regex.IsMatch(ModelName, @"615..|618..")) SetEdit();
            else
                throw new Exception($"{ModelName} invalid in SetAC_OutputState.Function()");

            Vars.NextIdx++;
            return false;

            void SetCouple() { WriteCommand(NIDriver, ModelName, $"INST:COUP {FindEIValue(para[0], 0)}"); }
            void SetPhase() { WriteCommand(NIDriver, ModelName, $"INST:NSEL {FindEIValue(para[1], 1)}"); }
            void SetEdit() { WriteCommand(NIDriver, ModelName, $"INST:EDIT {FindEIValue(para[2], 2)}"); }
        }
    }
}
