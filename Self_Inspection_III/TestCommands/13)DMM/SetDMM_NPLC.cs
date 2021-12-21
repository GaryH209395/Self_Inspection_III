using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.DMM
{
    class SetDMM_NPLC : TestCommand
    {
        public SetDMM_NPLC()
        {
            Name = "SetDMM_NPLC";
            DeviceType = DeviceTypes.DMM;
            Parameters = new List<Parameter>()
            {
                new Parameter("Test Item", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "1.Voltage\n2.Current\n3.2-Wire Resistence\n4.4-Wire Resistence\n5.Frequency\n6.Period\n7.Continuity Test\n8.Diode Test")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("Voltage","VOLT"),
                        new EnumItem("Current","CURR"),
                        new EnumItem("2-Wire Resistence","RES"),
                        new EnumItem("4-Wire Resistence","FRES"),
                        new EnumItem("Frequency","FREQ"),
                        new EnumItem("Period","PER"),
                        new EnumItem("Continuity Test","CONT"),
                        new EnumItem("Diode Test","DIOD")
                    }
                },
                new Parameter("NPLC", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Select the integration time in number of power line cycles for the present function (the default is 10 PLC).\n" +
                "This command is valid only for dc volts, ratio, dc current, 2-wire ohms, and 4-wire ohms.\n" +
                "MIN = 0.02. MAX = 100.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("MAX","MAX"),
                        new EnumItem("MIN","MIN"),
                        new EnumItem("100","100"),
                        new EnumItem("10","10"),
                        new EnumItem("1","1"),
                        new EnumItem("0.2","0.2"),
                        new EnumItem("0.02","0.02")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            string[] paraValue = new string[Parameters.Count];

            for (int i = 0; i < paraValue.Length; i++)
                paraValue[i] = FindEIValue(para[i], i);

            if (Regex.IsMatch(ModelName, @"344.1A"))
            {
                WriteCommand(NIDriver, ModelName, $"{paraValue[0]}:NPLC {paraValue[1]}");
            }
            else
            if (Regex.IsMatch(ModelName, @"3458A"))
            {
               
            }

            Vars.NextIdx++;
            return false;
        }
    }
}
