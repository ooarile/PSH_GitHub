using Microsoft.Toolkit.Mvvm.Input;
using Serial_protocol.Data;
using Serial_protocol.Protocol;
using Serial_protocol.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;

namespace Serial_protocol.ViewModel
{
    internal class SocketViewModel : CloseCommand
    {
        SocketServerProtocol serverSocket;
        SocketClientProtocol clientSocket;
        Thread th_FasSocket;
        private DataStruct recContent; 
        public ICommand connectCommand { get; } = null;
        public ICommand SendCommand { get; } = null; 
        public ICommand DisconnectCommand { get; } = null;
        public ICommand ListClearCommand { get; } = null;

        protected Serilog.ILogger _logger = Serial_protocol.App.Logger;

        private string _ip;
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private string _port;
        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }
        private bool _CheckServer =true;
        public bool CheckServer
        {
            get { return _CheckServer; }
            set { _CheckServer = value; }
        }
        private Brush _connectColor;
        public Brush connectColor
        {
            get { return _connectColor; }
            set { _connectColor = value; OnPropertyChanged(); }
        }

        private bool _isAutoScroll0 = true;
        public bool IsAutoScroll
        {
            get { return _isAutoScroll0; }
            set { if (value != _isAutoScroll0) { _isAutoScroll0 = value; OnPropertyChanged(); } }
        }

        private string _contents;
        public string Contents
        {
            get { return _contents; }
            set
            {
                if(value != _contents) { _contents = value; OnPropertyChanged(); }
            }
        }

        public ObservableCollection<DataStruct> Data { get; private set; }


        public ObservableCollection<string> startCharList { get; private set; }
        private string _startCharacter;
        public string startCharater
        {
            get { return _startCharacter; }
            set { _startCharacter = value; }
        }

        private string _Command ;
        public string Command
        {
            get { return _Command; }
            set { _Command = value; }
        }

        public ObservableCollection<string> endCharList { get; private set; }
        private string _endCharacter;
        public string endCharater
        {
            get { return _endCharacter; }
            set { _endCharacter = value; }
        }

        public SocketViewModel()
        {
            IP = "192.168.1.222";
            Port = "23";

            connectCommand = new RelayCommand(() => this.SocketConnect());
            SendCommand = new RelayCommand(() => this.SendFunc());
            DisconnectCommand = new RelayCommand(() => this.SocketDisconnect()); 
            ListClearCommand = new RelayCommand(() => Data.Clear());

            Data = new ObservableCollection<DataStruct>();
            startCharList = new ObservableCollection<string>
            {
                 "STX"
            };
            startCharater = "STX";
            endCharList = new ObservableCollection<string>
            {
                 "ETX","CRLF"
            };
            endCharater = "ETX";

        }
        public void SendFunc()
        {
            try
            {
                string strEndChar = null; ;// = combobox_EndChar.Text;
                string strStartChar = null;

                if (startCharater == "STX")
                    strStartChar = "\x02";
                else
                    strStartChar = null;

                if (endCharater == "CRLF")
                    strEndChar = "\r\n";
                else if (endCharater == "ETX")
                    strEndChar = "\x03";
                else
                    strEndChar = null;
                if(serverSocket!=null)
                    serverSocket.SendMessage(strStartChar + Command + strEndChar);
                if(clientSocket!=null)
                    clientSocket.SendMessage(strStartChar + Command + strEndChar);

                //_logger.Debug("Socket");
            }
            catch (System.Exception /*ex*/)
            {

            }
        }
       
        public void SocketConnect()
        {
            try
            {
                if (serverSocket == null)
                {
                    if (CheckServer)
                    {
                        serverSocket = new SocketServerProtocol("", IP, Port);
                        serverSocket.DataSendEvent += new SocketServerProtocol.DataGetEventHandler(RecBarcode);
                        serverSocket.ConnectStatus += new SocketServerProtocol.ConnectStatusEventHandler(SocketStatus);
                        serverSocket.Status += new SocketServerProtocol.StatusEventHandler(SocketStatusString);
                        th_FasSocket = new Thread(new ThreadStart(serverSocket.SocketOpen));
                        th_FasSocket.IsBackground = true;
                        th_FasSocket.Start();
                    }
                    else
                    {
                        clientSocket = new SocketClientProtocol("", IP, Port);
                        clientSocket.DataSendEvent += new SocketClientProtocol.DataGetEventHandler(RecBarcode);
                        clientSocket.ConnectStatus += new SocketClientProtocol.ConnectStatusEventHandler(SocketStatus);
                        clientSocket.Status += new SocketClientProtocol.StatusEventHandler(SocketStatusString);
                        th_FasSocket = new Thread(new ThreadStart(clientSocket.SocketOpen));
                        th_FasSocket.IsBackground = true;
                        th_FasSocket.Start();
                    }
                }
                //else
                //{
                //    if (!th_FasSocket.IsAlive)
                //    {
                //        th_FasSocket = new Thread(new ThreadStart(serverSocket.SocketOpen));
                //        th_FasSocket.IsBackground = true;
                //        th_FasSocket.Start();
                //    }
                //}
            }
            catch (System.Exception /*ex*/)
            {

            }
        }
        private void SocketStatusString(object obj,object str)
        {
            try
            {
                Contents = (string)str;

            }
            catch (System.Exception /*ex*/)
            {

            }
        }
        public void SocketDisconnect()
        {
            try
            {

                if (serverSocket != null)
                {
                    serverSocket.Disconnect();
                    serverSocket = null;
                    //SocketStatus(null, false);
                }
                else if (clientSocket != null)
                {
                    clientSocket.Disconnect();
                    clientSocket = null;
                    //SocketStatus(null, false);
                }

            }
            catch (System.Exception /*ex*/)
            {

            }
        }
        private void RecBarcode(object ob, object str)
        {
            recContent = new DataStruct();
            recContent.Time = DateTime.Now;
            recContent.Content = (string)str;

            DispatcherService.Invoke((System.Action)(() =>
            {
                Data.Add(recContent);
                // https://afsdzvcx123.tistory.com/entry/WPF-WPF-DataGrid-%EC%BB%A8%ED%8A%B8%EB%A1%A4-MVVM-%ED%8C%A8%ED%84%B4-%EB%8D%B0%EC%9D%B4%ED%84%B0-%EB%B0%94%EC%9D%B8%EB%94%A9%ED%95%98%EA%B8%B0
            }));
        }
        private void SocketStatus(object ob, object status)
        {
            Thread.Sleep(1);
            try
            {
                if ((bool)status)
                {
                    connectColor = Brushes.Green;
                }
                else
                {
                    connectColor = Brushes.Red;
                }
            }
            catch (System.Exception ex)
            {

            }

        }
        private void SocketStatusString(object ob, object IP, object Port, object Status)
        {
            string str = "";// string.Format("Client {0}::{1} 접속", IP, Port);

            if ((bool)Status)
                str = string.Format("Client {0}::{1} 접속", IP, Port);
            else
                str = string.Format("Client {0}::{1} 접속 끊김", IP, Port);

        }
    }
}
