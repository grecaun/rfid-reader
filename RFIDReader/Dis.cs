﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RFIDReader
{
    /**
     *
     * This is code from the disk provided with the reader I am using.
     * 
     */
    class Dis
    {
        // Parameter Addresses
        public const byte ADD_USERCODE = 0x64;
        public const byte ADD_POWER = 0x65;
        public const byte ADD_WORKMODE = 0x70;
        public const byte ADD_TIME_INTERVAL = 0x71;
        public const byte ADD_COMM_MODE = 0x72;
        public const byte ADD_WIEGAND_PROTO = 0x73;
        public const byte ADD_WIEGAND_PULSEWIDTH = 0x74;
        public const byte ADD_WIEGAND_PULSECYCLE = 0x75;
        public const byte ADD_NEIGHJUDGE_TIME = 0x7A;
        public const byte ADD_NEIGHJUDGE_SET = 0x7B;
        public const byte ADD_TRIG_SWITCH = 0x80;
        public const byte ADD_TRIG_MODE = 0x81;
        public const byte ADD_TRIG_DELAYTIME = 0x84;
        public const byte ADD_BAUD_RATE = 0x85;
        public const byte ADD_SINGLE_OR_MULTI_TAG = 0x87;
        public const byte ADD_ANT_MODE = 0x89;
        public const byte ADD_ANT_SET = 0x8A;
        public const byte ADD_FREQUENCY_SET = 0x90;
        public const byte ADD_FREQUENCY_PARA_92 = 0x92;
        public const byte ADD_FREQUENCY_PARA_93 = 0x93;
        public const byte ADD_FREQUENCY_PARA_94 = 0x94;
        public const byte ADD_FREQUENCY_PARA_95 = 0x95;
        public const byte ADD_FREQUENCY_PARA_96 = 0x96;
        public const byte ADD_FREQUENCY_PARA_97 = 0x97;
        public const byte ADD_FREQUENCY_PARA_98 = 0x98;
        public const byte ADD_SERIAL = 0x34;
        public const byte ADD_DELAYER_TIME = 0xC6;
        public const byte ADD_WIEGAND_VALUE = 0xB4;//2016-01-08 hz 韦根取位设置位
        public const byte ADD_READSPEED = 0xC8;//2016-01-08 hz 读卡速度设置
        public const byte ADD_RELAY_AUTOMATIC_CLOSE = 0xC7;//2016-01-08 hz 继电器自动闭合使能设置
        public const byte ADD_RELAY_TIME_DELAY = 0xC6;//2016-01-08 hz 继电器延时设置
        public const byte ADD_BAND_SET = 0X8F;//2016-05-13 hz 美标和欧标切换

        // Mask bits
        public const byte MASK_BIT_0 = 0x1;
        public const byte MASK_BIT_1 = 0x2;
        public const byte MASK_BIT_2 = 0x4;
        public const byte MASK_BIT_3 = 0x8;
        public const byte MASK_BIT_4 = 0x10;
        public const byte MASK_BIT_5 = 0x20;
        public const byte MASK_BIT_6 = 0x40;
        public const byte MASK_BIT_7 = 0x80;

        // Antenna
        public const byte ANT_BIT_0 = 0x1;
        public const byte ANT_BIT_1 = 0x2;
        public const byte ANT_BIT_2 = 0x4;
        public const byte ANT_BIT_3 = 0x8;
        public const byte ANT_BIT_4 = 0x10;
        public const byte ANT_BIT_5 = 0x20;
        public const byte ANT_BIT_6 = 0x40;
        public const byte ANT_BIT_7 = 0x80;


        public delegate void HANDLE_FUN(IntPtr pData, int length);

        [DllImport("dll\\disdll.dll")]
        public static extern int DeviceInit(byte[] Host, int CommMode, int PortOrBandRate);

        [DllImport("dll\\disdll.dll")]
        public static extern int DeviceConnect();

        [DllImport("dll\\disdll.dll")]
        public static extern int DeviceDisconnect();

        [DllImport("dll\\disdll.dll")]
        public static extern int DeviceUninit();

        [DllImport("dll\\disdll.dll")]
        public static extern int ResetReader(byte usercode);

        [DllImport("dll\\disdll.dll")]
        public static extern int BeginMultiInv(byte usercode, HANDLE_FUN fun_name);

        [DllImport("dll\\disdll.dll")]
        public static extern int StopInv(byte usercode);

        [DllImport("dll\\disdll.dll")]
        public static extern int GetDeviceVersion(byte usercode, out int MainVerion, out int SubVersion);

        [DllImport("dll\\disdll.dll")]
        public static extern int GetSingleParameter(byte usercode, byte ParaAddr, out int Value);

        [DllImport("dll\\disdll.dll")]
        public static extern int SetSingleParameter(byte usercode, byte ParaAddr, byte Value);

        [DllImport("dll\\disdll.dll")]
        public static extern int GetMultiParameters(byte usercode, int addr, int NumOfPara, byte[] Paras);

        [DllImport("dll\\disdll.dll")]
        public static extern int SetMultiParameters(byte usercode, int addr, int NumOfPara, byte[] Paras);

        [DllImport("dll\\disdll.dll")]
        public static extern int ReadSingleTag(byte usercode, byte[] TagID, byte[] AntNo);

        [DllImport("dll\\disdll.dll")]
        public static extern int ReadTagData(byte usercode, byte bank, byte begin, byte length, byte[] OutData);

        [DllImport("dll\\disdll.dll")]
        public static extern int WriteTagData(byte usercode, byte bank, byte begin, byte length, byte[] Data);// 0x81

        [DllImport("dll\\disdll.dll")]
        public static extern int WriteTagMultiWord(byte usercode, byte bank, byte begin, byte length, byte[] Data);// 0xAB

        [DllImport("dll\\disdll.dll")]
        public static extern int WriteTagSingleWord(byte usercode, byte bank, byte begin, byte data1, byte data2);

        [DllImport("dll\\disdll.dll")]
        public static extern int FastWriteTag(byte usercode, byte[] Data, byte length);

        [DllImport("dll\\disdll.dll")]
        public static extern int ReadTIDByEpc(byte usercode, byte[] pEpc, byte[] TID);

        [DllImport("dll\\disdll.dll")]
        public static extern int InitializeTag(byte usercode);

        [DllImport("dll\\disdll.dll")]
        public static extern int BeepCtrl(byte usercode, byte status);

        [DllImport("dll\\disdll.dll")]
        public static extern int LockTag(byte usercode, byte lockBank, byte[] Password);

        [DllImport("dll\\disdll.dll")]
        public static extern int UnlockTag(byte usercode, byte unlockBank, byte[] Password);

        [DllImport("dll\\disdll.dll")]
        public static extern int KillTag(byte usercode, byte[] Password);

        [DllImport("dll\\disdll.dll")]
        public static extern int SetBaudRate(byte usercode, byte BaudRate);

        [DllImport("dll\\disdll.dll")]
        public static extern int StopWork(byte usercode);

        [DllImport("dll\\disdll.dll")]
        public static extern int SetRelay(byte usercode, byte relayOnOff);

        [DllImport("dll\\disdll.dll")]
        public static extern int SetRelayTime(byte usercode, byte time);

        [DllImport("dll\\disdll.dll")]
        public static extern int SetAutherPwd(byte usercode, byte[] Pwd);

        [DllImport("dll\\disdll.dll")]
        public static extern int TagAuther(byte usercode);

        [DllImport("dll\\disdll.dll")]
        public static extern int GetAutherPwd(byte usercode, byte[] Pwd);
    }
}
