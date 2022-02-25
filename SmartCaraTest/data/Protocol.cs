using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCaraTest.data
{
    public class Protocol
    {
        public static byte STX = 0xCC;
        public static byte ETX = 0xEF;
        public static byte CONTROL = 0xAA;
        public static byte STATUS = 0xA0;
        public static byte START = 0x02;
        public static byte END = 0x04;
        public static byte STATE = 0x00;

        public static byte[] GetCommand(int kind)
        {
            byte[] command = new byte[7];
            command[0] = STX;
            command[1] = 0;
            command[3] = 0x07;
            switch (kind)
            {
                case 1: //state
                    command[2] = STATUS;
                    command[4] = 0x00;
                    break;
                case 2: //start
                    command[2] = CONTROL;
                    command[4] = START;
                    break;
                case 3: //end
                    command[2] = CONTROL;
                    command[4] = END;
                    break;
            }
            command[5] = (byte)(command[1] ^ command[2] ^ command[3] ^ command[4]);
            command[6] = ETX;
            return command;
        }
    }
}
