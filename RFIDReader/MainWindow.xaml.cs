using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RFIDReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static byte deviceNo = 0;
        private static Thread readingThread;
        private static NewReader reader;
        private static int ReadNo = 1;
        RFIDSerial serial;

        public MainWindow()
        {
            InitializeComponent();
            InstantiateSerialPortList();
            reader = new NewReader(600, this);
        }

        public void InstantiateSerialPortList()
        {
            serialPortCB.Items.Clear();
            var Ports = SerialPort.GetPortNames();
            foreach (string port in Ports)
            {
                serialPortCB.Items.Add(port);
            }
            if (serialPortCB.Items.Count > 0)
            {
                serialPortCB.SelectedIndex = 0;
            }
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            InstantiateSerialPortList();
        }

        private void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (connectBtn.Content.Equals("Connect"))
            {   //*
                if (serialPortCB.SelectedIndex >= 0)
                {
                    serial = new RFIDSerial(serialPortCB.Text, 9600);
                    reader.SetSerial(serial);
                }
                else
                {
                    MessageBox.Show("No serial port selected.");
                    return;
                }
                if (serial.Connect() != RFIDSerial.Error.NOERR)
                {
                    MessageBox.Show("Unable to connect to device.");
                    return;
                }
                connectBtn.Content = "Disconnect";
                chipNumbers.Items.Add(new RFIDSerial.Info { DecNumber = 0 });
                readingThread = new Thread(new ThreadStart(reader.Run));
                readingThread.Start(); //*/
                /*
                byte[] ip = new byte[32];
                int CommPort = 0;
                int PortOrBaudRate = 0;
                if (serialPortCB.SelectedIndex >= 0)
                {
                    CommPort = int.Parse(serialPortCB.Text.Trim("COM".ToCharArray()));
                    PortOrBaudRate = 9600;
                }
                else
                {
                    // error no port selected
                    MessageBox.Show("No Serial Port Selected.");
                    return;
                }
                if (Dis.DeviceInit(ip, CommPort, PortOrBaudRate) == 0 || Dis.DeviceConnect() == 0)
                {
                    MessageBox.Show("Unable to initialize device.");
                    return;
                }
                for (int i = 0; i < 3; ++i)
                {
                    Dis.StopWork(deviceNo);
                }
                int mainVer = 0, minSer = 0;
                Dis.GetDeviceVersion(deviceNo, out mainVer, out minSer);
                string version = string.Format("{0}:{1}.{2}", "Version", mainVer, minSer);
                if (version != "Version:0.0")
                {
                    connectBtn.Content = "Disconnect";
                    chipNumbers.Items.Add(new RFIDSerial.RFIDInfo { DecNumber = 0 });
                    readingThread = new Thread(new ThreadStart(reader.Run));
                    readingThread.Start();
                }
                else
                {
                    MessageBox.Show("Unable to connect to device.");
                } //*/
            }
            else
            {   //*
                serial.Disconnect(); //*/
                /*
                Dis.ResetReader(deviceNo);
                Dis.DeviceDisconnect();
                Dis.DeviceUninit(); //*/
                connectBtn.Content = "Connect";
                chipNumbers.Items.Add(new RFIDSerial.Info { DecNumber = -1 });
                reader.Kill();
            }
        }

        internal void AddDataItem(long decNumber, string hexNumber, int antNo, int devNo)
        {
            DataItem data = new DataItem
            {
                DecNumber = decNumber,
                HexNumber = hexNumber,
                AntennaNumber = antNo,
                DeviceNumber = devNo,
                ReadNumber = ReadNo++
            };
            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
            {
                chipNumbers.Items.Add(data);
            }));
        }

        internal void AddRFIDItems(List<RFIDSerial.Info> reads)
        {
            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
            {
                foreach (RFIDSerial.Info info in reads)
                {
                    info.ReadNumber = ReadNo++;
                    chipNumbers.Items.Add(info);
                }
            }));
        }

        internal void AddRFIDItem(RFIDSerial.Info read)
        {
            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
            {
                read.ReadNumber = ReadNo++;
                chipNumbers.Items.Add(read);
            }));
        }

        internal class DataItem
        {
            public long DecNumber { get; set; }
            public string HexNumber { get; set; }
            public int AntennaNumber { get; set; }
            public int DeviceNumber { get; set; }
            public int ReadNumber { get; set; }
            public string DataRep { get; set; }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            reader.Kill();
            if (readingThread != null)
            {
                readingThread.Join();
            }
        }
    }
}
