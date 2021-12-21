using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Self_Inspection_III.TestCommands.AC_Source
{
    class SetAC_OutputMode : TestCommand
    {
        public SetAC_OutputMode()
        {
            Name = "SetAC_OutputMode";
            DeviceType = DeviceTypes.AC_Source;
            Parameters = new List<Parameter>();
            Parameters.AddRange(new SetAC_Instrument().Parameters);
            Parameters.AddRange(new SetAC_Vout().Parameters);
            Parameters.AddRange(new SetAC_Frequency().Parameters);
            Parameters.AddRange(new SetAC_VoltRange().Parameters);
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            int FunIdx = Vars.NextIdx, idx = 0;
            StringBuilder paraStr = new StringBuilder();

            //Instrument
            SetAC_Instrument setInstrument = new SetAC_Instrument();
            setInstrument.DoFunction(TCDoFunc(setInstrument, para, ref idx), ModelName, NIDriver, ref Vars);

            //Volt 
            SetAC_Vout setVolt = new SetAC_Vout();
            setVolt.DoFunction(TCDoFunc(setVolt, para, ref idx), ModelName, NIDriver, ref Vars);

            //Freq 
            SetAC_Frequency setFreq = new SetAC_Frequency();
            setFreq.DoFunction(TCDoFunc(setFreq, para, ref idx), ModelName, NIDriver, ref Vars);

            //Volt Range
            SetAC_VoltRange setVoltRange = new SetAC_VoltRange();
            setVoltRange.DoFunction(TCDoFunc(setVoltRange, para, ref idx), ModelName, NIDriver, ref Vars);

            Vars.NextIdx = FunIdx + 1;
            return false;
        }
    }
}
