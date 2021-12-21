using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;

namespace Self_Inspection_III.TestCommands.DMM
{
    class SetDMM_FuncRange : TestCommand
    {
        public SetDMM_FuncRange()
        {
            Name = "SetDMM_FuncRange";
            DeviceType = DeviceTypes.DMM;
            Parameters = new List<Parameter>()
            {
                new Parameter("Function", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "1.AC Voltage; 2.DC Voltage; 3.DC Ratio; 4.AC Current; 5.DC Current; 6.2-Wire Resistence; 7.4-Wire Resistence; 8.Frequency; 9.Period; 10.Continuity Test; 11.Diode Test")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("AC Voltage","MEAS:VOLT:AC?"),
                        new EnumItem("DC Voltage","MEAS:VOLT:DC?"),
                        new EnumItem("DC Ratio","MEAS:VOLT:DC:RAT?"),
                        new EnumItem("AC Current","MEAS:CURR:AC?"),
                        new EnumItem("DC Current","MEAS:CURR:DC?"),
                        new EnumItem("2-Wire Resistence","MEAS:RES?"),
                        new EnumItem("4-Wire Resistence","MEAS:FRES?"),
                        new EnumItem("Frequency","MEAS:FREQ?"),
                        new EnumItem("Period","MEAS:PER?"),
                        new EnumItem("Continuity Test","MEAS:CONT?"),
                        new EnumItem("Diode Test","MEAS:DIOD?")
                    }
                },
                new Parameter("Auto Zero", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "The OFF and ONCE parameters have a similar effect.\nAutozero OFF does not issue a new zero measurement.\nAutozero ONCE issues an immediate zero measurement.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("OFF","OFF"),
                        new EnumItem("ONCE","ONCE"),
                        new EnumItem("ON","ON")
                    }
                },
                new Parameter("NPLC", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Select the integration time in number of power line cycles for the present function (the default is 10 PLC).\n" +
                "This command is valid only for dc volts, ratio, dc current, 2 - wire ohms, and 4 - wire ohms.\n" +
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
                },
                new Parameter("APER", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList,
                "Select the aperture time (or gate time) for frequency measurements (the default is 0.1 seconds).\n" +
                "Specify 10 ms (4 1⁄2 digits), 100 ms (default; 5 1⁄2 digits), or 1 second(6 1⁄2 digits).\n" +
                "MIN = 0.01 seconds.MAX = 1 second.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("MAX","MAX"),
                        new EnumItem("MIN","MIN"),
                        new EnumItem("1","100"),
                        new EnumItem("0.1","1"),
                        new EnumItem("0.01","0.1")
                    }
                }
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            string[] paraValue = new string[Parameters.Count];

            for (int i = 0; i < paraValue.Length; i++)
            {
                paraValue[i] = FindEIValue(para[i], i);
            }

            //DMM Set
            string cmdFunc = paraValue[0];
            string cmdZero = $"ZERO:AUTO {paraValue[1]}";
            string cmdNPLC = $":NPLC {paraValue[2]}";
            string cmdAPER = $":APER {paraValue[3]}";

            string Func = cmdFunc.ToString().Split(':')[1].Replace("?","");
            switch (Func)
            {
                case "VOLT":
                case "CURR":
                case "RES":
                case "FRES":
                    if (cmdFunc.Contains("AC")) break;
                    WriteCommand(NIDriver, ModelName, Func + cmdNPLC);
                    break;
                case "FREQ":
                case "PER":
                    WriteCommand(NIDriver, ModelName, Func + cmdAPER);
                    break;
                default:
                    break;
            }

            //WriteCommand(NIDriver, ModelName, cmdZero);
            WriteCommand(NIDriver, ModelName, cmdFunc);

            Vars.NextIdx++;
            return false;
        }
    }
}
