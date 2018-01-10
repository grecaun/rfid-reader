using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RFIDReader
{
    class Reader
    {
        byte deviceNo = 0;
        int Delay = 500;
        bool KeepAlive = false;
        int counter = 1;

        MainWindow mWindow;

        public Reader(int delay, MainWindow mWindow)
        {
            this.Delay = delay;
            this.mWindow = mWindow;
        }

        public void Run()
        {
            KeepAlive = true;
            do
            {
                System.Console.WriteLine("Active - Loop Number " + counter++);
                string text;
                byte[] outData = new byte[64];
                byte[] antNo = new byte[4];
                byte[] hexEPC = new byte[12];
                string hexStrEPC = "";
                long decEPC;
                int dv;
                Dis.GetSingleParameter(deviceNo, 0x64, out dv);
                int nub = 5;
                while (true)
                {
                    if (0 != Dis.ReadSingleTag(deviceNo, outData, antNo))
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            hexEPC[i] = outData[i];
                            hexStrEPC += string.Format("{0:X2} ", outData[i]);
                        }
                        byte[] epc = new byte[8];
                        int ix = 0;
                        for (int i=11; i>3; i--)
                        {
                            epc[ix++] = hexEPC[i];
                        }
                        decEPC = BitConverter.ToInt64(epc, 0);
                        text = "Read - " + decEPC + " - " + hexStrEPC + " - " + BitConverter.ToInt32(antNo, 0) + " - " + dv;
                        mWindow.AddDataItem(decEPC, hexStrEPC, BitConverter.ToInt32(antNo, 0), dv);
                        break;
                    }
                    else
                    {
                        if (--nub == 0)
                        {
                            text = "Unable to read this go around.";
                            break;
                        }
                    }
                }
                System.Console.WriteLine(text);
                Thread.Sleep(Delay);
            } while (KeepAlive);
            System.Console.WriteLine("InActive - Finished after " + counter + " loops.");
        }

        public void Kill()
        {
            System.Console.WriteLine("Kill command received.");
            KeepAlive = false;
        }

        public void Stop()
        {
            Kill();
        }
    }
}
