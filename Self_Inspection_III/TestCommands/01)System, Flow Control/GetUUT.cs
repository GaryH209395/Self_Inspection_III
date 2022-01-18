using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System.Collections.Generic;
using System.Threading;

namespace Self_Inspection_III.TestCommands.System_FlowControl
{
    class GetUUT : TestCommand
    {
        public GetUUT()
        {
            Name = "GetUUT";
            DeviceType = DeviceTypes.Null;
            Parameters = new List<Parameter>()
            {
                new Parameter("Get Item", DataTypes.String, ParaTypes.Constant, ConstTypes.ComboList, "Select the info of UUT.")
                {
                    EnumItems = new EnumItem[]
                    {
                        new EnumItem("Model", "0"),
                        new EnumItem("S/N", "1")
                    }
                },
                new Parameter("Input Var", DataTypes.String, ParaTypes.Result, ConstTypes.NotAllow, "Input value to variable.")
            };
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');

            switch (FindEIValue(para[0], 0)) {
                case "0":
                    Vars.SetValue(para[1], ModelName);
                    break;
                case "1":
                    WriteCommand(NIDriver, ModelName, "*IDN?");
                    Thread.Sleep(500);
                    Vars.SetValue(para[1], ReadResponse(NIDriver));
                    break;
                default:
                    break;
            }

            Vars.NextIdx++;
            return false;
        }
    }
}
