using CviVisaDriver;
using Self_Inspection_III.Class;
using Self_Inspection_III.SI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Ports;

namespace Self_Inspection_III.TestCommands.IO_Card
{
    class SetIO_OutputState : TestCommand
    {
        public short m_dev;
        public SetIO_OutputState()
        {
            Name = "SetIO_OutputState";
            DeviceType = DeviceTypes.IO_Card;
            Parameters = new List<Parameter>()
            {
                new Parameter("Specific index of IO Card", DataTypes.Integer, ParaTypes.Constant, ConstTypes.EditBox,""),
                new Parameter("Specific bit(4~n)", DataTypes.Integer, ParaTypes.Constant, ConstTypes.EditBox, "Note that the first 3 bits are reserved for system use."),
                new Parameter("Output state", DataTypes.Integer, ParaTypes.Constant, ConstTypes.EditBox, "0:low, 1:high")               
            };
        }
        
        protected override bool Function(string ParaString, string ModelName, ushort Card_Type,out short IO_Dev, ref ItemVars Vars)
        {
            string[] para = ParaString.Split(',');
            short IO_Status;            

            //***  Setting CardNumber/Port/Switch  ***//
            ushort CardNumber = Convert.ToUInt16(Vars.GetValue(para[0]));
            ushort RelayChannel = Convert.ToUInt16(Vars.GetValue(para[1]));
            //byte[] Port = Encoding.UTF8.GetBytes(Vars.GetValue(para[1]));
            ushort RelaySwitch = Convert.ToUInt16(Vars.GetValue(para[2]));

            //***  Register the IO Card  ***//                      
            m_dev = DASK.Register_Card(Card_Type, CardNumber);
            IO_Dev = m_dev;
            if (m_dev < 0){
                Console.WriteLine("Register_Card error!");
            }

            //*****************************// 
            //*        DO_WritePort       *//
            //* Control the IO Card in PC *//
            //*****************************// 
            //IO_Status = DASK.DO_WritePort(CardNumber, Port[0], Switch);
            IO_Status = DASK.DO_WriteLine(CardNumber, 0, RelayChannel, RelaySwitch);        //PCI-7230/7234 ->  The Port need to set Zero.
            if (IO_Status < 0){
                Console.WriteLine("DO_WriteLine error!");
            }

            //*******************************//
            //*      Func: DI_ReadPort()    *//
            //* Read the Status from Device *//
            //*******************************//
            //*      Func: DO_ReadPort()    *//
            //* Read the Status from PC     *//
            //*******************************//

            Vars.NextIdx++;
            return false;
        }

        protected override bool Function(string ParaString, string ModelName, CviVisaCtrl NIDriver, ref ItemVars Vars)
        {
            throw new NotImplementedException();
        }
    }
}
