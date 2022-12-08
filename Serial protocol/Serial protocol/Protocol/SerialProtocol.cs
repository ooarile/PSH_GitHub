using Serial_protocol.SettingData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serial_protocol.Protocol
{
    public enum Expecting : byte
    {
        ANY = 1,
        ESCAPED_CHAR,
        HEX_1ST_DIGIT,
        HEX_2ND_DIGIT
    };

    internal class SerialProtocol
    {
        SerialPort _serialPort;
        Thread _readThread;
        volatile bool _keepReading;

        public SerialProtocol()
        {
            _serialPort = new SerialPort();
            _readThread = null;
            _keepReading = false;
        }
        public bool KeepReading
        {
            get
            {
                return _keepReading;
            }
        }

        //begin Observer pattern
        public delegate void EventHandler(string param);
        public EventHandler StatusChanged;
        public EventHandler DataReceived;
        //end Observer pattern
        public string ConvertEscapeSequences(string s)
        {
            Expecting expecting = Expecting.ANY;

            int hexNum = 0;
            string outs = "";
            foreach (char c in s)
            {
                switch (expecting)
                {
                    case Expecting.ANY:
                        if (c == '\\')
                            expecting = Expecting.ESCAPED_CHAR;
                        else
                            outs += c;
                        break;
                    case Expecting.ESCAPED_CHAR:
                        if (c == 'x')
                        {
                            expecting = Expecting.HEX_1ST_DIGIT;
                        }
                        else
                        {
                            char c2 = c;
                            switch (c)
                            {
                                case 'n': c2 = '\n'; break;
                                case 'r': c2 = '\r'; break;
                                case 't': c2 = '\t'; break;
                            }
                            outs += c2;
                            expecting = Expecting.ANY;
                        }
                        break;
                    case Expecting.HEX_1ST_DIGIT:
                        hexNum = GetHexDigit(c) * 16;
                        expecting = Expecting.HEX_2ND_DIGIT;
                        break;
                    case Expecting.HEX_2ND_DIGIT:
                        hexNum += GetHexDigit(c);
                        outs += (char)hexNum;
                        expecting = Expecting.ANY;
                        break;
                }
            }
            return outs;
        }
        private static int GetHexDigit(char c)
        {
            if ('0' <= c && c <= '9') return (c - '0');
            if ('a' <= c && c <= 'f') return (c - 'a') + 10;
            if ('A' <= c && c <= 'F') return (c - 'A') + 10;
            return 0;

        }
        private void StartReading()
        {
            if (!_keepReading)
            {
                _keepReading = true;
                _readThread = new Thread(ReadPort);
                _readThread.Start();
            }
        }

        private void StopReading()
        {
            if (_keepReading)
            {
                _keepReading = false;
                _readThread.Join(); //block until exits
                _readThread = null;
            }
        }

        /// <summary> Get the data and pass it on. </summary>
        private void ReadPort()
        {
            ArrayList SerialIn;
            string str = "";
            while (_keepReading)
            {
                if (_serialPort.IsOpen)
                {
                    Thread.Sleep(1);
                    byte[] readBuffer = new byte[_serialPort.ReadBufferSize + 1];
                    try
                    {
                        if (readBuffer != null && readBuffer.Length > 0)
                        {
                            int count = _serialPort.Read(readBuffer, 0, _serialPort.ReadBufferSize);

                            string lineEnding = "";
                            switch (SettingsComm.Option.AppendToSend)
                            {
                                case SettingsComm.Option.AppendType.AppendCR:
                                    lineEnding = "\r"; break;
                                case SettingsComm.Option.AppendType.AppendLF:
                                    lineEnding = "\n"; break;
                                case SettingsComm.Option.AppendType.AppendCRLF:
                                    lineEnding = "\r\n"; break;
                            }

                            string rec = System.Text.Encoding.ASCII.GetString(readBuffer, 0, count);
                         
                            if (rec.Contains(lineEnding))
                            {
                                SerialIn = new ArrayList(rec.Split(new string[] { lineEnding },
                                    StringSplitOptions.None));
                                splitDataSend(SerialIn, ref str);
                            }
                            else    // 라인피드 없을경우 계속 데이터 축적
                            {
                                str += rec;
                                if(str.Contains(lineEnding))
                                {
                                    SerialIn = new ArrayList(str.Split(new string[] { lineEnding },
                                        StringSplitOptions.None));
                                    str = "";
                                    splitDataSend(SerialIn,  ref str);
                                }
                            }                          
                        }

                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("ReadPort() timeout");
                    }



                }
                else
                {
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
                    Thread.Sleep(waitTime);
                }
            }
        }
        private void splitDataSend(ArrayList SerialIn, ref string str)
        {
            for (int i = 0; i < SerialIn.Count; i++)
            {
                if (SerialIn[i].ToString() == "" && str != "")
                {
                    DataReceived(str);
                    str = "";
                }
                if (SerialIn[i].ToString() == "")
                    continue;

                //마지막 데이터의 경우
                if (i + 1 == SerialIn.Count)
                {
                    //ex) a\r b\r cd 로 잘렸을 경우  a,b는 이미 보냈을 거고, cd는 데이터 축적
                    str = "";
                    str += SerialIn[i].ToString();
                }
                else if (str != "" && i == 0)//데이터가 잘려서 왔을 경우 ex) a      bcd\r
                {
                    str += SerialIn[i].ToString();
                    DataReceived(str);
                    str = "";
                }
                else
                {
                    // str += SerialIn[i].ToString();
                    DataReceived(SerialIn[i].ToString());
                    str = "";

                }
            }

        }

        /// <summary> Open the serial port with current settings. </summary>
        public void Open()
        {
            Close();

            try
            {
                _serialPort.PortName = SettingsComm.Port.PortName;
                _serialPort.BaudRate = SettingsComm.Port.BaudRate;
                _serialPort.Parity = SettingsComm.Port.Parity;
                _serialPort.DataBits = SettingsComm.Port.DataBits;
                _serialPort.StopBits = SettingsComm.Port.StopBits;
                _serialPort.Handshake = SettingsComm.Port.Handshake;

                // Set the read/write timeouts
                _serialPort.ReadTimeout = 50;
                _serialPort.WriteTimeout = 50;

                _serialPort.Open();
                StartReading();
            }
            catch (IOException)
            {
                StatusChanged(String.Format("{0} does not exist", SettingsComm.Port.PortName));
            }
            catch (UnauthorizedAccessException)
            {
                StatusChanged(String.Format("{0} already in use", SettingsComm.Port.PortName));
            }
            catch (Exception ex)
            {
                StatusChanged(String.Format("{0}", ex.ToString()));
            }

            // Update the status
            if (_serialPort.IsOpen)
            {
                string p = _serialPort.Parity.ToString().Substring(0, 1);   //First char
                string h = _serialPort.Handshake.ToString();
                if (_serialPort.Handshake == Handshake.None)
                    h = "no handshake"; // more descriptive than "None"

                StatusChanged(String.Format("{0}: {1} bps, {2}{3}{4}, {5}",
                    _serialPort.PortName, _serialPort.BaudRate,
                    _serialPort.DataBits, p, (int)_serialPort.StopBits, h));
            }
            else
            {
                StatusChanged(String.Format("{0} already in use", SettingsComm.Port.PortName));
            }
        }

        /// <summary> Close the serial port. </summary>
        public void Close()
        {
            if (_serialPort != null)
            {
                StopReading();

                _serialPort.Close();
                StatusChanged("connection closed");
            }
        }

        /// <summary> Get the status of the serial port. </summary>
        public bool IsOpen
        {
            get
            {
                return _serialPort.IsOpen;
            }
        }

        /// <summary> Get a list of the available ports. Already opened ports
        /// are not returend. </summary>
        public string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>Send data to the serial port after appending line ending. </summary>
        /// <param name="data">An string containing the data to send. </param>
        public void Send(string data)
        {
            if (IsOpen)
            {
                string lineEnding = "";
                switch (SettingsComm.Option.AppendToSend)
                {
                    case SettingsComm.Option.AppendType.AppendCR:
                        lineEnding = "\r"; break;
                    case SettingsComm.Option.AppendType.AppendLF:
                        lineEnding = "\n"; break;
                    case SettingsComm.Option.AppendType.AppendCRLF:
                        lineEnding = "\r\n"; break;
                }

                _serialPort.Write(data + lineEnding);
            }
        }
    }
}
