using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Self_Inspection_III.TestCommands.DMM
{
    class ReadDMM_Measure : TestCommand
    {
        public ReadDMM_Measure()
        {
            Name = "ReadDMM_Measure";
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
                new Parameter("Return value", DataTypes.Float, ParaTypes.Result, ConstTypes.NotAllow, "Measurement of DMM.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaValue(ParaString);

            Console.WriteLine("ModelName: " + ModelName);
            if (Regex.IsMatch(ModelName, @"344.1A"))
            {
                WriteCommand(NIDriver, ModelName, para[0]);
                Vars.SetValue(para[1], Convert.ToDouble(ReadResponse(NIDriver)));
            }
            else
            if (Regex.IsMatch(ModelName, @"3458A"))
            {
                para[0] = "TARM SGL";
                // 第一次試讀
                WriteCommand(NIDriver, ModelName, para[0]);
                ReadResponse(NIDriver);

                // 第二次正式讀取
                WriteCommand(NIDriver, ModelName, para[0]);
                Vars.SetValue(para[1], Convert.ToDouble(ReadResponse(NIDriver)));
            }
            Vars.NextIdx++;
            return false;
        }
    }
}
